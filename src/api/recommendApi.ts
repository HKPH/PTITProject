import axios from "axios";

const BASE_API_URL = 'http://127.0.0.1:5000';
export const recommendBooks = async (userId: any, topK = 10) => {
    try {
        const response = await axios.get(`${BASE_API_URL}/recommend`, {
            params: { user_id: userId, top_k: topK },
        });
        return response.data;
    } catch (error) {
        console.error("Error fetching book recommendations:", error);
        throw error;
    }
};

export const addBookRC = async (bookData: any) => {
    try {
        const response = await axios.post(`${BASE_API_URL}/add_book`, bookData);
        return response.data;
    } catch (error) {
        console.error("Error adding book:", error);
        throw error;
    }
};

export const updateBookRC = async (bookId: any, fieldsToUpdate: any) => {
    try {
        const response = await axios.put(`${BASE_API_URL}/update_book/${bookId}`, {
            book_id: bookId,
            fields_to_update: fieldsToUpdate,
        });
        return response.data;
    } catch (error) {
        console.error("Error updating book:", error);
        throw error;
    }
};

export const deleteBookRC = async (bookId: any) => {
    try {
        const response = await axios.delete(`${BASE_API_URL}/delete_book/${bookId}`, {
            data: { book_id: bookId },
        });
        return response.data;
    } catch (error) {
        console.error("Error deleting book:", error);
        throw error;
    }
};

export const addRating = async (userId: any, bookId: any, reviewScore: any) => {
    try {
        const response = await axios.post(`${BASE_API_URL}/add_rating`, {
            User_id: userId,
            book_id: bookId,
            review_score: reviewScore,
        });
        return response.data;
    } catch (error) {
        console.error("Error adding/updating rating:", error);
        throw error;
    }
};
