import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, InputNumber, message, Pagination, AutoComplete, Select } from 'antd';
import { getBooks, getBookById, updateBook, deleteBook, createBook, getCategoriesByBookId, joinCategoryToBook } from '../api/bookApi';
import { getPublisherById, getAllPublishers, createPublisher } from '../api/publisherApi';
import { getAllCategories, getCategoryById, createCategory } from '../api/categoryApi';
import dayjs from 'dayjs';
import { useCallback } from 'react';
import { UpOutlined, DownOutlined } from '@ant-design/icons';

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
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [page, setPage] = useState(1);
  const [isCreating, setIsCreating] = useState(false);

  const [totalBooks, setTotalBooks] = useState(0);
  const [sortBy, setSortBy] = useState<string>('newest'); // Default sort option
  const [searchTerm, setSearchTerm] = useState<string>(''); // Search term input  
  const [isActive, setIsActive] = useState(false);
  // Publisher states
  const [publishers, setPublishers] = useState<{ id: number; name: string }[]>([]);
  const [publisherInput, setPublisherInput] = useState<string>('');
  const [selectedPublisherId, setSelectedPublisherId] = useState<number | null>(null);
  const [isCreatingPublisher, setIsCreatingPublisher] = useState(false);

  // Category states
  const [categories, setCategories] = useState<{ id: number; name: string }[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<number | null>(null);
  const [categoryInput, setCategoryInput] = useState<string>('');
  const [isCreatingCategory, setIsCreatingCategory] = useState(false);
  const [selectedCategoriesList, setSelectedCategoriesList] = useState<{ id: number; name: string }[]>([]);


  useEffect(() => {
    fetchBooks();
  }, [page, sortBy, isActive, searchTerm]);

  useEffect(() => {
    fetchPublishers(publisherInput);
  }, [publisherInput]);

  useEffect(() => {
    fetchCategories(categoryInput);
  }, [categoryInput]);

  const fetchBooks = useCallback(async () => {
    setLoading(true);
    try {
      const data = await getBooks(page, 10, undefined, sortBy, isActive, searchTerm);
      console.log(data);
      if (data && Array.isArray(data.items)) {
        const formattedBooks = await Promise.all(data.items.map(async (book: Book) => {
          try {
            const publisher = await getPublisherById(book.publisherId);
            const categories = await getCategoriesByBookId(book.id);
            return {
              ...book,
              author: book.author,
              publisherName: publisher.name,
              categories: categories || [],
            };
          } catch (error: any) {
            console.error("Error fetching publisher:", error);
            return null;
          }
        }));

        setBooks(formattedBooks.filter(book => book !== null))
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
  }, [page, sortBy, isActive, searchTerm]);

  const fetchPublishers = async (searchTerm: string = '') => {
    try {
      const data: { id: number; name: string }[] = await getAllPublishers();
      const filteredPublishers = data.filter((publisher) =>
        publisher.name.toLowerCase().includes(searchTerm.toLowerCase())
      );
      setPublishers(filteredPublishers);
    } catch (error) {
      message.error("Failed to load publishers");
    }
  };

  const fetchCategories = async (searchTerm: string = '') => {
    try {
      const data: { id: number; name: string }[] = await getAllCategories();
      const filteredCategories = data.filter((category) =>
        category.name.toLowerCase().includes(searchTerm.toLowerCase())
      );
      setCategories(filteredCategories);
    } catch (error) {
      message.error("Failed to load categories");
    }
  };

  const handlePublisherSelect = (value: string) => {
    const publisher = publishers.find(p => p.name === value);
    if (publisher) {
      setPublisherInput(value);
      setSelectedPublisherId(publisher.id);

      form.setFieldsValue({ publisherName: publisher.name });
    }
  };

  const handleCategorySelect = (value: string) => {
    const category = categories.find(p => p.name === value);
    if (category) {
      setSelectedCategoriesList(prevList => [...prevList, category]);
      setCategoryInput(value);
      setSelectedCategories(category.id);
      form.setFieldsValue({ categoryName: category.name });
    }
  };
  const handleSelectAllCategories = async (id: number) => {
    const categoriesBook = await getCategoriesByBookId(id);

    for (const categoryBook of categoriesBook) {
      const category = categories.find(p => p.name === categoryBook.name);

      if (category) {
        setSelectedCategoriesList(prevList => [...prevList, category]);
        setCategoryInput(categoryBook.name);
        setSelectedCategories(category.id);
        form.setFieldsValue({ categoryName: category.name });
      }
    }
  };



  const handlePublisherSearch = (value: string) => {
    setPublisherInput(value);
    fetchPublishers(value);
  };

  const handleCategorySearch = (value: string) => {
    setCategoryInput(value);
    fetchCategories(value);
  };

  const handleViewDetails = async (id: number) => {
    try {
      const book = await getBookById(id);
      setSelectedBook(book);
      const publisher = await getPublisherById(book.publisherId);
      form.setFieldsValue({
        ...book,
        author: book.author,
        publicationDate: dayjs(book.publicationDate).format('YYYY-MM-DD'),
        publisherId: book.publisherId,
        publisherName: publisher?.name || '',
      });
      setPublisherInput(publisher?.name || '');
      setIsModalVisible(true);
      setSelectedCategoriesList([]);
      handleSelectAllCategories(id)
    } catch {
      message.error("Failed to load book details");
    }
  };

  const handleUpdate = async () => {
    try {
      const updatedBook = await form.validateFields();
      if (selectedBook) {
        const bookToUpdate = {
          id: selectedBook.id,
          title: updatedBook.title,
          author: updatedBook.author,
          publisherId: publishers.find(p => p.name === publisherInput)?.id || selectedBook.publisherId,
          price: updatedBook.price,
          publicationDate: dayjs(updatedBook.publicationDate).toISOString(),
          active: updatedBook.active,
          stockQuantity: updatedBook.stockQuantity,
          description: updatedBook.description || '',
          image: updatedBook.image || ''
        };

        await updateBook(selectedBook.id, bookToUpdate);

        if (selectedCategoriesList.length > 0) {
          console.log(selectedCategoriesList)
          for (const category of selectedCategoriesList) {
            await joinCategoryToBook(selectedBook.id, category.id); // Gọi API để liên kết danh mục
          }
        }
        message.success("Book updated successfully");
        fetchBooks();
        handleCloseModal();
      }
    } catch (error) {
      message.error("Failed to update book");
    }
  };


  const handleDelete = async () => {
    if (selectedBook) {
      try {
        await deleteBook(selectedBook.id);
        message.success("Book deleted successfully");
        fetchBooks();
        handleCloseModal();
      } catch {
        message.error("Failed to delete book");
      }
    }
  };

  const handleCreateBook = async () => {
    try {
      // Validate the form and get the new book data
      const newBook = await form.validateFields();

      // Parse author field to ensure it's in the right format
      newBook.author = newBook.author;

      // Set publisherId and other necessary fields
      newBook.publisherId = selectedPublisherId;
      newBook.publisherName = publisherInput;

      newBook.price = Number(newBook.price);
      newBook.publicationDate = new Date().toISOString();
      newBook.active = true;
      newBook.stockQuantity = Number(newBook.stockQuantity);
      newBook.description = newBook.description || "string";
      newBook.image = newBook.image || "string";

      const createdBook = await createBook(newBook);

      if (createdBook && selectedCategoriesList.length > 0) {
        console.log(selectedCategoriesList)
        for (const category of selectedCategoriesList) {
          await joinCategoryToBook(createdBook.id, category.id);
        }
      }
      // Show success message
      message.success("Book created successfully");

      // Fetch updated list of books
      fetchBooks();

      // Close the modal
      handleCloseModal();
    } catch (error) {
      // Show error message if something goes wrong
      message.error("Failed to create book");
      console.error(error); // Log the error for debugging
    }
  };

  const handleCreatePublisher = async () => {
    try {
      await createPublisher({ name: publisherInput });
      message.success("Publisher created successfully");
      fetchPublishers(); // Refresh the list of publishers
      setIsCreatingPublisher(false);
      setPublisherInput(''); // Clear the publisher input
    } catch (error) {
      message.error("Failed to create publisher");
    }
  };

  const handleCreateCategory = async () => {
    if (categoryInput) {
      try {
        await createCategory({ name: categoryInput });
        message.success("Category created successfully");
        fetchCategories(); // Refresh category list
        setCategoryInput('');
      } catch (error) {
        message.error("Failed to create category");
      }
    }
  };

  const handleCloseModal = () => {
    setIsModalVisible(false);
    form.resetFields();
    setIsCreating(false);
    setPublisherInput('');
    setCategoryInput('');
    setSelectedCategoriesList([])
  };

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
  };
  const handleRemoveCategory = (id: number) => {
    setSelectedCategoriesList(prevList => prevList.filter(category => category.id !== id));
  };

  const columns = [
    { title: 'Title', dataIndex: 'title', key: 'title' },
    {
      title: 'Author',
      dataIndex: 'author',
      key: 'author',
      render: (authors: string) => authors,
    },
    {
      title: 'Publisher',
      dataIndex: 'publisherName',
      key: 'publisherName',
    },
    {
      title: 'Categories',
      dataIndex: 'categories', // Cột mới cho danh mục
      key: 'categories',
      render: (categories: { id: number; name: string }[]) => (
        categories.map(category => category.name).join(', ')
      ),
    },
    {
      title: 'Price',
      dataIndex: 'price',
      key: 'price',
      render: (price: number) => `${price.toLocaleString()} VND`,
    },
    {
      title: 'Publication Date',
      dataIndex: 'publicationDate',
      key: 'publicationDate',
      render: (date: string) => dayjs(date).format('DD/MM/YYYY'),
    },
    { title: 'Stock Quantity', dataIndex: 'stockQuantity', key: 'stockQuantity' },
    {
      title: 'Action',
      key: 'action',
      render: (_: any, record: Book) => (
        <Button type="link" onClick={() => handleViewDetails(record.id)}>View</Button>
      ),
    },
  ];

  return (
    <>
      <div style={{ marginBottom: 16 }}>
        <Input
          placeholder="Search by title"
          value={searchTerm}
          onChange={e => {
            setSearchTerm(e.target.value);
            setPage(1);
          }}
          style={{ width: 300 }}
        />
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

        <Button type="primary" onClick={() => { setIsCreating(true); setIsModalVisible(true); }}>
          Create New Book
        </Button>
      </div>

      <Table
        dataSource={books}
        columns={columns}
        loading={loading}
        pagination={false}
        rowKey="id"
      />

      <Pagination
        current={page}
        onChange={handlePageChange}
        pageSize={10}
        total={totalBooks}
      />

      <Modal
        title={isCreating ? 'Create Book' : 'Update Book'}
        open={isModalVisible}
        onOk={isCreating ? handleCreateBook : handleUpdate}
        onCancel={handleCloseModal}
        footer={[
          <Button key="back" onClick={handleCloseModal}>Cancel</Button>,
          !isCreating && <Button key="delete" type="primary" danger onClick={handleDelete}>Delete</Button>,
          <Button key="submit" type="primary" onClick={isCreating ? handleCreateBook : handleUpdate}>
            {isCreating ? 'Create' : 'Update'}
          </Button>,
        ]}
      >
        <Form form={form} layout="vertical">
          <Form.Item label="Title" name="title" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Author" name="author" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Publisher" name="publisherName" rules={[{ required: true }]}>
            <div style={{ display: 'flex', alignItems: 'center' }}>
              <AutoComplete
                options={
                  publishers.length > 0
                    ? publishers.map(publisher => ({ value: publisher.name }))
                    : [{ value: 'Không có nhà phát hành phù hợp' }]
                }
                onSelect={handlePublisherSelect}
                onSearch={handlePublisherSearch}
                value={publisherInput}
                placeholder="Enter publisher name"
                allowClear
                style={{ flex: 1, marginRight: '8px' }}
              />
              <Button onClick={() => { handleCreatePublisher(); }}>+</Button>
            </div>
          </Form.Item>
          <Form.Item label="Categories" name="categories">
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px', marginBottom: '8px' }}>
              {selectedCategoriesList.map(category => (
                <div
                  key={category.id}
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    padding: '4px 8px',
                    backgroundColor: '#f0f0f0',
                    borderRadius: '16px',
                    marginBottom: '8px'
                  }}
                >
                  {category.name}
                  <Button
                    type="text"
                    onClick={() => handleRemoveCategory(category.id)}
                    style={{ marginLeft: '8px', color: 'red', fontSize: '12px' }}
                  >
                    ✕
                  </Button>
                </div>
              ))}
            </div>

            <div style={{ display: 'flex', alignItems: 'center' }}>
              <AutoComplete
                options={
                  categories.length > 0
                    ? categories.map(category => ({ value: category.name }))
                    : [{ value: 'Không có thể loại phù hợp' }]
                }
                onSelect={handleCategorySelect}
                onSearch={handleCategorySearch}
                value={categoryInput}
                placeholder="Enter category name"
                allowClear
                style={{ flex: 1, marginRight: '8px' }}
              />
              <Button onClick={() => handleCreateCategory()}>+</Button>
            </div>
          </Form.Item>

          <Form.Item label="Price" name="price" rules={[{ required: true }]}>
            <InputNumber min={0} style={{ width: '100%' }} />
          </Form.Item>
          {/* Add more fields as necessary */}
        </Form>
      </Modal>
    </>
  );
};

export default BookList;
