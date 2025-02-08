from flask import Flask, request, jsonify
from flask_cors import CORS  # Import CORS
import pandas as pd
import numpy as np
import re
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.preprocessing import MultiLabelBinarizer
from sklearn.metrics.pairwise import cosine_similarity
from flask_caching import Cache
import pickle
import os
from apscheduler.schedulers.background import BackgroundScheduler
from datetime import datetime

# Initialize Flask app
app = Flask(__name__)

# Enable CORS for all routes
CORS(app)  # This allows CORS on all routes by default

cache = Cache(app, config={'CACHE_TYPE': 'SimpleCache'})

# Ensure cache directory exists
if not os.path.exists('cache'):
    os.makedirs('cache')

# Preprocess data
def clean_text(text):
    """Cleans the input text by removing URLs, HTML tags, non-alphabetic characters, and extra spaces."""
    text = re.sub(r'http\S+|www\S+', '', text)  # Remove URLs
    text = re.sub(r'<.*?>', '', text)  # Remove HTML tags
    text = re.sub(r'\s+', ' ', text).strip()  # Normalize whitespace
    text = re.sub(r'[^a-zA-Z\s]', '', text)  # Remove non-alphabetic characters
    return text.lower()

def clean_and_split(text):
    # và biến thành viết thường
    """Tách các chuỗi giá trị phân tách bằng dấu phẩy và loại bỏ khoảng trắng thừa."""
    if isinstance(text, str):
        return [item.strip() for item in text.split(',') if item.strip()]
    return []
    
BOOKS_FILE = 'data/books_rc.csv'
RATINGS_FILE = 'data/ratings_rc.csv'

def load_data():
    """Đọc lại dữ liệu từ các file CSV."""
    books = pd.read_csv(BOOKS_FILE)
    ratings = pd.read_csv(RATINGS_FILE)

    books['categories'] = books['categories'].apply(lambda x: clean_and_split(str(x)))
    books['authors'] = books['authors'].apply(lambda x: clean_and_split(str(x)))
    books['description'] = books['description'].apply(lambda x: clean_text(str(x)))
    
    ratings['review_score'] = pd.to_numeric(ratings['review_score'], errors='coerce')
    ratings = ratings.dropna(subset=['User_id', 'review_score'], how='all')
    ratings = ratings.groupby(['User_id', 'book_id'], as_index=False)['review_score'].mean()
    
    return books, ratings

def get_item_mean(book_id, item_mean_dict):
    return item_mean_dict.to_dict().get(book_id, None)  


# Define recommendation functions
@cache.memoize(timeout=600)
def recommend_books_hybrid(user_id, top_k=10):

    def books_for_new_user(file_path=RATINGS_FILE, top_n=10):
        try:
            ratings = pd.read_csv(file_path)
            positive_ratings = ratings[ratings['review_score'] >= 4]
            book_positive_counts = positive_ratings.groupby('book_id').size()
            book_positive_counts = book_positive_counts.sort_values(ascending=False)
            
            suggested_books = book_positive_counts.head(top_n).index.tolist()
            similarity_scores = book_positive_counts.head(top_n).values.tolist()
            print([[book_id, score] for book_id, score in zip(suggested_books, similarity_scores)])
            return [[book_id, score] for book_id, score in zip(suggested_books, similarity_scores)]
        except Exception as e:
            print(f"Đã xảy ra lỗi: {e}")
            return []
    
    def switching_hybrid_sum():
        # recommend_cb = recommend_books_cb()
        # recommend_cf = recommend_books_cf()
        recommend_cb = books_for_new_user()
        recommend_cf = books_for_new_user()
        if not recommend_cb and not recommend_cf:
            return books_for_new_user()
        
        if len(recommend_cf) < top_k and len(recommend_cb) >= top_k:
            return recommend_cb[:top_k]
        elif len(recommend_cb) < top_k and len(recommend_cf) >= top_k:
            return recommend_cf[:top_k]

        total_cf_score = sum([score for _, score in recommend_cf])
        total_cb_score = sum([score for _, score in recommend_cb])
        
        if total_cf_score >= total_cb_score:
            return recommend_cf[:top_k]
        else:
            return recommend_cb[:top_k]
        
    return switching_hybrid_sum()


@app.route('/recommend', methods=['GET'])
def recommend():
    """API endpoint to get book recommendations for a user."""
    try:
        # Lấy tham số user_id từ query string (không cần chuyển thành số nguyên nữa)
        user_id = request.args.get('user_id')
        top_k = int(request.args.get('top_k', 10))

        # Gọi hàm recommend_books_hybrid với user_id là chuỗi
        recommendations = recommend_books_hybrid(user_id, top_k)

        # Trả về danh sách sách đề xuất dưới dạng JSON
        result = [book_id for book_id, _ in recommendations]
        return jsonify(result)
    
    except Exception as e:
        return jsonify({'error': str(e)}), 500
    

# Load books and ratings data
def load_books():
    return pd.read_csv(BOOKS_FILE)

def load_ratings():
    return pd.read_csv(RATINGS_FILE)

# Save books and ratings data back to CSV
def save_books(books_df):
    books_df.to_csv(BOOKS_FILE, index=False)

def save_ratings(ratings_df):
    ratings_df.to_csv(RATINGS_FILE, index=False)

# Add Book API
@app.route('/add_book', methods=['POST'])
def add_book():
    try:
        data = request.get_json()

        # Check if required fields are provided
        required_fields = ['id', 'Title', 'description', 'authors', 'image', 'publisher', 'publishedDate', 'categories','book_id']
        if not all(field in data for field in required_fields):
            return jsonify({'error': 'Missing required fields'}), 400

        # Load existing books and append new book
        books = load_books()
        new_book = pd.DataFrame([data])
        books = pd.concat([books, new_book], ignore_index=True)

        # Save updated books data
        save_books(books)

        return jsonify({'message': 'Book added successfully'}), 201
    except Exception as e:
        return jsonify({'error': str(e)}), 500

# Update Book API
@app.route('/update_book/<int:book_id>', methods=['PUT'])
def update_book(book_id):
    try:
        data = request.get_json()

        # Load books data
        books = load_books()

        # Find the book by ID
        book_index = books[books['book_id'] == book_id].index

        if book_index.empty:
            return jsonify({'error': 'Book not found'}), 404

        # Update book details
        for key, value in data.items():
            if key in books.columns:
                books.at[book_index[0], key] = value

        # Save updated books data
        save_books(books)

        return jsonify({'message': 'Book updated successfully'}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500

# Delete Book API
@app.route('/delete_book/<int:book_id>', methods=['DELETE'])
def delete_book(book_id):
    try:
        # Load books data
        books = load_books()

        # Find the book by ID and delete it
        books = books[books['book_id'] != book_id]

        # Save updated books data
        save_books(books)

        return jsonify({'message': 'Book deleted successfully'}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500

# Add Rating API
@app.route('/add_rating', methods=['POST'])
def add_rating():
    try:
        data = request.get_json()

        # Check if required fields are provided
        if 'User_id' not in data or 'book_id' not in data or 'review_score' not in data:
            return jsonify({'error': 'Missing required fields'}), 400

        # Load existing ratings
        ratings = load_ratings()

        # Create new rating entry
        new_rating = pd.DataFrame([data])

        # Append the new rating
        ratings = pd.concat([ratings, new_rating], ignore_index=True)

        # Save updated ratings data
        save_ratings(ratings)

        return jsonify({'message': 'Rating added successfully'}), 201
    except Exception as e:
        return jsonify({'error': str(e)}), 500

# Run the app
if __name__ == '__main__':
    # scheduler = BackgroundScheduler()
    # scheduler.add_job(func=update_similarity_matrices, trigger='interval', hours=1)
    # scheduler.start()
    # update_similarity_matrices()
    app.run(debug=False)
