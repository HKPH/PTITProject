import React, { useState, useEffect, useCallback, useRef } from 'react';
import { Row, Col, Card, Button, message, Pagination, Select } from 'antd';
import { getBooks, getCategoriesByBookId, getBookById } from '../api/bookApi';
import { recommendBooks } from '../api/recommendApi';
import { LeftOutlined, RightOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

interface Book {
  id: number;
  title: string;
  author: string;
  publisherId: number;
  price: number;
  publicationDate: string;
  stockQuantity: number;
  description?: string;
  image?: string;
  publisherName?: string;
  active: boolean;
  categories?: { id: number; name: string }[];
}

const CustomerBook: React.FC = () => {
  const navigate = useNavigate();
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [totalBooks, setTotalBooks] = useState(0);
  const [sortBy, setSortBy] = useState<string>('Mới nhất');
  const [sortByTitle, setSortByTitle] = useState<string>('Mới nhất');
  const [activeSort, setActiveSort] = useState(false);
  const [recommendedBooks, setRecommendedBooks] = useState<Book[]>([]);
  const scrollContainerRef = useRef<HTMLDivElement>(null);

  const scroll = (direction: 'left' | 'right') => {
    const container = scrollContainerRef.current;
    if (container) {
      const scrollAmount = direction === 'left' ? -600 : 600;
      container.scrollBy({ left: scrollAmount, behavior: 'smooth' });
    }
  };

  useEffect(() => {
    fetchBooks();
  }, [page, sortBy, activeSort]);

  useEffect(() => {
    const userId = parseInt(localStorage.getItem('userId') || '0', 10);
    if (userId) {
      fetchRecommendedBooks(userId);
    } else {
      console.warn("User ID not found in localStorage.");
    }
  }, []);

  const fetchBooks = useCallback(async () => {
    setLoading(true);
    try {
      const data = await getBooks(page, 16, undefined, sortBy, activeSort, "");
      if (data && Array.isArray(data.items)) {
        const formattedBooks = await Promise.all(data.items.map(async (book: Book) => {
          try {
            const categories = await getCategoriesByBookId(book.id);
            return {
              ...book,
              author: book.author,
              categories: categories || [],
            };
          } catch (error: any) {
            console.error("Error fetching publisher:", error);
            return null;
          }
        }));

        setBooks(formattedBooks.filter(book => book !== null));
        setTotalBooks(data.totalCount);
      } else {
        throw new Error("Invalid data format");
      }
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error("Error fetching books:", error.message);
      } else {
        console.error("Unexpected error fetching books:", error);
      }
      message.error("Failed to load books");
    } finally {
      setLoading(false);
    }
  }, [page, sortBy, activeSort]);

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
      setRecommendedBooks(recommended.filter(book => book !== null) as Book[]);
    } catch (error) {
      console.error("Error fetching recommended books:", error);
      message.error("Failed to load recommended books");
    }
  };

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };

  const handleCardClick = (slug: string, id: number) => {
    navigate(`/${slug}/${id}`);
  };

  return (
    <>
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
            {recommendedBooks.map((book: Book) => (
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

      {/* Books Display Section */}
      <div style={{ backgroundColor: '#fff', padding: '16px' }}>
        <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'flex-end' }}>
          <Select
            value={sortByTitle}
            onChange={value => {
              if (value === 'id') {
                setSortByTitle('Mới nhất');
              } else if (value === 'title-asc') {
                setSortByTitle('Theo tên tăng dần');
              } else if (value === 'title-desc') {
                setSortByTitle('Theo tên giảm dần');
              } else if (value === 'price-asc') {
                setSortByTitle('Giá tiền tăng dần');
              } else if (value === 'price-desc') {
                setSortByTitle('Giá tiền giảm dần');
              }

              if (value === 'id') {
                setActiveSort(false);
                setSortBy(value);
              } else {
                const [field, order] = value.split('-');
                setSortBy(field);

                if (order === 'asc') {
                  setActiveSort(true);
                } else if (order === 'desc') {
                  setActiveSort(false);
                }
              }
              setPage(1);
            }}
            style={{ width: 200 }}
          >
            <Select.Option value="id">Mới nhất</Select.Option>
            <Select.Option value="title-asc">Theo tên tăng dần</Select.Option>
            <Select.Option value="title-desc">Theo tên giảm dần</Select.Option>
            <Select.Option value="price-asc">Giá tiền tăng dần</Select.Option>
            <Select.Option value="price-desc">Giá tiền giảm dần</Select.Option>
          </Select>
        </div>

        <Row gutter={[16, 16]} style={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'space-between' }}>
          {books.map((book: Book) => (
            <Col xs={24} sm={12} md={8} lg={6} key={book.id} style={{ marginBottom: 16 }}>
              <Card
                hoverable
                cover={<img alt={book.title} src={book.image} style={{ width: 'calc(100% - 4px)', height: 230, objectFit: 'cover', paddingLeft: 20, paddingRight: 20, backgroundColor: '#fff' }} />}
                style={{ height: '100%' }}
                onClick={() => handleCardClick(book.title, book.id)}
              >
                <Card.Meta
                  title={<span style={{ color: '#333' }}>{book.title}</span>}
                  description={<span style={{ color: '#eb7c26' }}>{book.price.toLocaleString()} VND</span>}
                />
              </Card>
            </Col>
          ))}
        </Row>

        <Pagination
          current={page}
          onChange={handlePageChange}
          pageSize={10}
          total={totalBooks}
          style={{ marginTop: 16 }}
        />
      </div>
    </>
  );
};

export default CustomerBook;
