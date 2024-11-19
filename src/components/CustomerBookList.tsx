import React, { useState, useEffect, useCallback } from 'react';
import { Row, Col, Card, Button, message, Pagination, Select } from 'antd';
import { getBooks, getCategoriesByBookId } from '../api/bookApi';
import dayjs from 'dayjs';
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

const BookList: React.FC = () => {
  const navigate = useNavigate();
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [totalBooks, setTotalBooks] = useState(0);
  const [sortBy, setSortBy] = useState<string>('newest');
  const [isActive, setIsActive] = useState(false);

  useEffect(() => {
    fetchBooks();
  }, [page, sortBy, isActive]);

  const fetchBooks = useCallback(async () => {
    setLoading(true);
    try {
      const data = await getBooks(page, 16, undefined, sortBy, isActive, "");
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
  }, [page, sortBy, isActive]);

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };

  const handleCardClick = (slug: string, id: number) => {
    navigate(`/${slug}/${id}`);
  };

  return (
    <>
      <div style={{ marginBottom: 16 }}>
        <Select
          value={sortBy}
          onChange={value => {
            setSortBy(value);
            setPage(1);
          }}
          style={{ width: 200, marginRight: 16 }}
        >
          <Select.Option value="id">Newest</Select.Option>
          <Select.Option value="title">Name</Select.Option>
          <Select.Option value="price">Price</Select.Option>
        </Select>

        <Select
          value={isActive ? 1 : 0}
          onChange={value => {
            setIsActive(value === 1);
            setPage(1);
          }}
          style={{ width: 200, marginLeft: 16 }}
        >
          <Select.Option value={1}>Active</Select.Option>
          <Select.Option value={0}>Inactive</Select.Option>
        </Select>
      </div>
      <Row gutter={[16, 16]} style={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'space-between' }}>
        {books.map((book: Book) => (
          <Col span={5} key={book.id} style={{ marginBottom: 16 }}>
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
    </>
  );
};

export default BookList;
