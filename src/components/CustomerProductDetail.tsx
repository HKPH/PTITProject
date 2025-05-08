import React, { useEffect, useState, useRef } from 'react';
import { Layout, Spin, Button, Divider, Pagination, message, Row, Col, Avatar, Rate, Card } from 'antd';
import { useParams } from 'react-router-dom';
import { getBookById, getCategoriesByBookId } from '../api/bookApi';
import { getPublisherById } from '../api/publisherApi';
import { UserOutlined } from '@ant-design/icons';
import { createCartItem } from '../api/cartItemApi';
import { getRatingByBookId, getRatingCountByBookId } from '../api/ratingApi';
import { recommendBooks } from '../api/recommendApi';
import { LeftOutlined, RightOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useMediaQuery } from "react-responsive";
const { Content } = Layout;

interface BookDetail {
    id: number;
    title: string;
    author: string;
    publisherId: number;
    price: number;
    publicationDate: string;
    stockQuantity: number;
    description: string;
    image: string;
    categoryId: number;
}
interface Category {
    id: number;
    name: string;
    active: boolean;
}


interface Rating {
    id: number;
    bookId: number;
    userId: number;
    value: number;
    comment: string;
    createDate: string;
    active: boolean;
    image: string;
}

const CustomerProductDetail: React.FC = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const [book, setBook] = useState<BookDetail | null>(null);
    const [publisher, setPublisher] = useState<string>('');
    const [category, setCategory] = useState<string>('');
    const [quantity, setQuantity] = useState<number>(1);
    const [loading, setLoading] = useState<boolean>(true);
    const [ratings, setRatings] = useState<Rating[]>([]);
    const [totalRatings, setTotalRatings] = useState<number>(0);
    const [page, setPage] = useState<number>(1);

    const [averageRating, setAverageRating] = useState<number>(0);
    const [ratingCounts, setRatingCounts] = useState<{ [key: number]: number }>({});
    const [filter, setFilter] = useState<number | null>(null);
    const [recommendedBooks, setRecommendedBooks] = useState<BookDetail[]>([]);

    const scrollContainerRef = useRef<HTMLDivElement>(null);
    const scroll = (direction: 'left' | 'right') => {
        const container = scrollContainerRef.current;
        if (container) {
            const scrollAmount = direction === 'left' ? -300 : 300;
            container.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        }
    };
    const isSmallScreen = useMediaQuery({ query: "(max-width: 1200px)" });

    useEffect(() => {
        if (id) {
            const bookId = parseInt(id, 10);
            if (!isNaN(bookId)) {
                getBookById(bookId)
                    .then(async (data) => {
                        setBook(data);
                        const publisherName = await getPublisherById(data.publisherId);
                        setPublisher(publisherName.name);
                        const categories: Category[] = await getCategoriesByBookId(bookId);
                        if (categories.length > 0) {
                            const categoryNames = categories.map((category: Category) => category.name).join(', ');
                            setCategory(categoryNames);
                        } else {
                            setCategory('Không có thể loại');
                        }
                        const ratingData = await getRatingByBookId(bookId, page, 5);
                        console.log(ratingData)
                        setRatings(ratingData.items);
                        setTotalRatings(ratingData.totalCount);
                        const ratingCountsFromAPI = await getRatingCountByBookId(bookId);
                        const total = ratingData.items.reduce(
                            (acc: number, curr: { value: number }) => acc + curr.value,
                            0
                        );
                        setRatingCounts([
                            ratingCountsFromAPI[1],
                            ratingCountsFromAPI[2],
                            ratingCountsFromAPI[3],
                            ratingCountsFromAPI[4],
                            ratingCountsFromAPI[5],
                        ]);

                        setAverageRating(total / ratingData.items.length);
                        setLoading(false);
                    })
                    .catch((err) => {
                        console.error('Failed to fetch book details:', err);
                        setLoading(false);
                    });
            } else {
                console.error('Invalid book ID');
                setLoading(false);
            }
        }
    }, [id, page]);

    useEffect(() => {
        const userId = parseInt(localStorage.getItem('userId') || '0', 10);
        if (userId) {
            fetchRecommendedBooks(userId);
        } else {
            console.warn("User ID not found in localStorage.");
        }
    }, []);

    const handleFilterChange = (ratingValue: number | null) => {
        setFilter(ratingValue);
        getRatingByBookId(parseInt(id || '0', 10), page, 5, ratingValue ?? undefined).then((data) => {
            setRatings(data.items);
        });
    };

    const fetchRecommendedBooks = async (userId: number) => {
        try {
            const recommendedBookIds = await recommendBooks(userId);
            const recommended = await Promise.all(recommendedBookIds.map(async (id: number) => {
                try {
                    return await getBookById(id);
                } catch (error: any) {
                    console.error(`Error fetching book with ID ${id}:`, error);
                    return null;
                }
            }));
            setRecommendedBooks(recommended.filter(book => book !== null) as BookDetail[]);
        } catch (error) {
            console.error("Error fetching recommended books:", error);
            message.error("Failed to load recommended books");
        }
    };

    const cleanAuthor = (author: string) => {
        return author.replace(/[^a-zA-Z, ]/g, "");
    };

    const handleAddToCart = async () => {
        const cartId = localStorage.getItem('cartId');
        if (!cartId) {
            console.error('Cart ID not found in session storage.');
            return;
        }

        if (!book) {
            console.error('Book details are missing.');
            return;
        }

        const cartItemData = {
            cartId: parseInt(cartId, 10),
            bookId: book.id,
            quantity: 1,
        };

        try {
            const response = await createCartItem(cartItemData);
            console.log('Item added to cart:', response);
            message.success('Sản phẩm đã được thêm vào giỏ hàng!');
        } catch (error) {
            console.error('Failed to add item to cart:', error);
            message.error('Thêm sản phẩm vào giỏ hàng thất bại. Vui lòng thử lại.');
        }
    };

    const handleBuyNow = async () => {
        const cartId = localStorage.getItem('cartId');
        if (!cartId) {
            console.error('Cart ID not found in session storage.');
            return;
        }

        if (!book) {
            console.error('Book details are missing.');
            return;
        }

        const cartItemData = {
            cartId: parseInt(cartId, 10),
            bookId: book.id,
            quantity: quantity,
        };

        try {
            const response = await createCartItem(cartItemData);
            console.log('Item purchased:', response);
            navigate('/cart');
        } catch (error) {
            console.error('Failed to complete purchase:', error);
        }
    };
    const handleCardClick = (slug: string, id: number) => {
        navigate(`/${slug}/${id}`);
        window.scrollTo(0, 0);
    };

    return (
        <div style={{ width: '100%', margin: '0 auto', backgroundColor: 'white' }}>
            <Layout>
                <Content style={{ padding: '24px', backgroundColor: 'white' }}>
                    {loading ? (
                        <div style={{ textAlign: 'center' }}>
                            <Spin size="large" />
                        </div>
                    ) : book ? (
                        <div>
                            <div style={{ display: 'flex', gap: '24px', alignItems: 'flex-start', padding: '16px', borderRadius: '8px' }}>
                                <img
                                    src={book.image}
                                    alt={book.title}
                                    style={{ width: '300px', height: '400px', objectFit: 'cover', borderRadius: '8px', boxShadow: '0 4px 8px rgba(0,0,0,0.1)' }}
                                />
                                <div style={{ flex: 1, paddingLeft: '16px' }}>
                                    <h1 style={{ color: '#FF5733', fontSize: '28px', fontWeight: 'bold', marginBottom: '16px' }}>{book.title}</h1>
                                    <p style={{ fontSize: '16px', marginBottom: '8px' }}><strong style={{ color: '#333' }}>Tác giả:</strong> {cleanAuthor(book.author)}</p>
                                    <p style={{ fontSize: '16px', marginBottom: '8px' }}><strong style={{ color: '#333' }}>Nhà xuất bản:</strong> {publisher}</p>
                                    <p style={{ fontSize: '16px', marginBottom: '8px' }}><strong style={{ color: '#333' }}>Thể loại:</strong> {category || 'Không có thể loại'}</p>
                                    <p style={{ fontSize: '18px', fontWeight: 'bold', color: '#FF5733', marginBottom: '16px' }}>{book.price.toLocaleString()} đ</p>
                                    <p style={{ fontSize: '16px', marginBottom: '16px' }}><strong style={{ color: '#333' }}>Tình trạng:</strong> {book.stockQuantity > 0 ? 'Còn hàng' : 'Hết hàng'}</p>
                                    <Divider />
                                    <div style={{ display: 'flex', alignItems: 'center', gap: '16px', marginTop: '16px' }}>
                                        <Button
                                            type="primary"
                                            onClick={handleAddToCart}
                                            style={{
                                                backgroundColor: '#FF5733', borderColor: '#FF5733', fontWeight: 'bold', height: '80px', lineHeight: '70px', textAlign: 'center'
                                            }}
                                        >
                                            Thêm vào giỏ hàng
                                        </Button>
                                        <Button
                                            type="primary"
                                            onClick={handleBuyNow}
                                            style={{ backgroundColor: '#FF5733', borderColor: '#FF5733', fontWeight: 'bold', height: '80px', lineHeight: '70px', textAlign: 'center' }}
                                        >
                                            Mua ngay
                                        </Button>
                                    </div>
                                    <div style={{ marginTop: '16px', color: '#000', fontSize: '14px', lineHeight: '1.8' }}>
                                        <p style={{ color: '#FFA500', fontWeight: 'bold' }}>Gọi đặt hàng: <span style={{ color: '#FFA500', fontWeight: 'bold', textDecoration: 'underline' }}>(028) 3820 7153 hoặc 0933 109 009</span></p>
                                        <p style={{ fontWeight: 'bold' }}>Thông tin & Khuyến mãi: </p>
                                        <p>Đổi trả hàng trong vòng 7 ngày.</p>
                                        <p>Freeship toàn quốc từ 250.000đ.</p>
                                    </div>
                                </div>
                            </div>

                            <Divider />
                            <div style={{ marginTop: '24px' }}>
                                <h3 style={{ fontSize: '24px', fontWeight: 'bold', color: '#333' }}>Đánh giá sản phẩm</h3>
                                <div className="review-rating-filter" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
                                    <div className="rating" style={{ display: 'flex', alignItems: 'center' }}>
                                        <span className="rating-score" style={{ fontSize: '24px', fontWeight: 'bold', color: '#ff6600' }}>
                                            {averageRating.toFixed(1)}
                                        </span>
                                        <span style={{ marginLeft: '8px', fontSize: '16px', color: '#333' }}>trên 5</span>
                                        <div className="stars" style={{ color: '#ff6600', fontSize: '20px', marginLeft: '8px' }}>
                                            {'★'.repeat(Math.floor(averageRating))}{'☆'.repeat(5 - Math.floor(averageRating))}
                                        </div>
                                    </div>
                                    <div className="filter-tabs" style={{ display: 'flex' }}>
                                        {['Tất Cả', '5 Sao', '4 Sao', '3 Sao', '2 Sao', '1 Sao'].map((label, index) => {
                                            const value = index === 0 ? null : 6 - index;
                                            return (
                                                <button
                                                    key={label}
                                                    onClick={() => handleFilterChange(value)}
                                                    className={filter === value ? 'active' : ''}
                                                    style={{
                                                        backgroundColor: filter === value ? '#ff6600' : 'transparent',
                                                        color: filter === value ? '#fff' : '#333',
                                                        border: '1px solid #ddd',
                                                        borderRadius: '5px',
                                                        padding: '8px 15px',
                                                        marginLeft: '8px',
                                                        cursor: 'pointer',
                                                        transition: 'background-color 0.3s, color 0.3s',
                                                    }}
                                                >
                                                    {label} ({index === 0 ? totalRatings : ratingCounts[5 - index]})
                                                </button>
                                            );
                                        })}
                                    </div>
                                </div>

                                {ratings.length > 0 ? (
                                    <div>
                                        {ratings.map((rating) => (
                                            <Card key={rating.id} style={{ marginBottom: '16px', borderRadius: '8px', boxShadow: '0 2px 5px rgba(0, 0, 0, 0.1)' }}>
                                                <Row align="middle" gutter={16}>
                                                    <Col>
                                                        <Avatar icon={<UserOutlined />} />
                                                    </Col>
                                                    <Col>
                                                        <h4 style={{ fontSize: '18px', fontWeight: 'bold', color: '#333' }}>User {rating.userId}</h4>
                                                        <Rate disabled value={rating.value} />
                                                        <p style={{ fontSize: '16px', color: '#555' }}>{rating.comment}</p>
                                                        {rating.image && <img src={rating.image} alt="review image" style={{ maxWidth: '200px', borderRadius: '8px', marginTop: '10px' }} />}
                                                    </Col>
                                                </Row>
                                            </Card>
                                        ))}
                                        <Pagination
                                            current={page}
                                            total={totalRatings}
                                            pageSize={5}
                                            onChange={(page) => setPage(page)}
                                            style={{ textAlign: 'center', marginTop: '16px' }}
                                        />
                                    </div>
                                ) : (
                                    <p style={{ fontSize: '16px', color: '#888' }}>Chưa có đánh giá</p>
                                )}
                            </div>

                        </div>
                    ) : (
                        <h2 style={{ textAlign: 'center' }}>Không tìm thấy sản phẩm</h2>
                    )}
                </Content>
            </Layout>
            <div>
                <div style={{ backgroundColor: '#fff', padding: '5px 10px', marginBottom: '10px' }}>
                    <h3>Sách dành cho bạn</h3>
                    <div style={{ display: 'flex', alignItems: 'center' }}>
                        <Button
                            icon={<LeftOutlined />}
                            onClick={() => scroll('left')}
                            style={{
                                border: 'none',
                                backgroundColor: 'transparent',
                                padding: 0,
                                fontSize: '24px',
                                cursor: 'pointer',
                            }}
                        />

                        <div
                            ref={scrollContainerRef}
                            style={{
                                overflowX: 'auto',
                                display: 'flex',
                                flexWrap: 'nowrap',
                                gap: '18px',
                                padding: '8px 0',
                                width: '100%',
                                scrollbarWidth: 'none',
                                msOverflowStyle: 'none',
                            }}
                        >
                            {recommendedBooks.map((book: BookDetail) => (
                                <div key={book.id} style={{ width: 'calc(20% - 16px)', minWidth: '200px' }}>
                                    <Card
                                        hoverable
                                        cover={<img alt={book.title} src={book.image} style={{ width: '100%', height: 230, objectFit: 'cover' }} />}
                                        onClick={() => handleCardClick(book.title, book.id)}
                                    >
                                        <Card.Meta
                                            title={<span style={{ color: '#333' }}>{book.title}</span>}
                                            description={<span style={{ color: '#eb7c26' }}>{book.price.toLocaleString()} VND</span>}
                                        />
                                    </Card>
                                </div>
                            ))}
                        </div>

                        <Button
                            icon={<RightOutlined />}
                            onClick={() => scroll('right')}
                            style={{
                                border: 'none',
                                backgroundColor: 'transparent',
                                padding: 0,
                                fontSize: '24px',
                                cursor: 'pointer',
                            }}
                        />
                    </div>
                </div>
            </div>
        </div >

    );
};

export default CustomerProductDetail;
