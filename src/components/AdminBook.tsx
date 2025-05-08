import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, InputNumber, message, Pagination, AutoComplete, Select } from 'antd';
import { getBooks, getBookById, updateBook, deleteBook, createBook, getCategoriesByBookId, joinCategoryToBook, uploadImage } from '../api/bookApi';
import { getPublisherById, getAllPublishers, createPublisher } from '../api/publisherApi';
import { getAllCategories, createCategory } from '../api/categoryApi';
import { addBookRC, deleteBookRC } from '../api/recommendApi';
import dayjs from 'dayjs';
import { useCallback } from 'react';
import { Upload } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
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

const AdminBook: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [page, setPage] = useState(1);
  const [isCreating, setIsCreating] = useState(false);

  const [totalBooks, setTotalBooks] = useState(0);
  const [searchTerm, setSearchTerm] = useState<string>('');


  const [sortBy, setSortBy] = useState<string>('Mới nhất');
  const [sortByTitle, setSortByTitle] = useState<string>('Mới nhất');
  const [activeSort, setActiveSort] = useState(false);

  const [publishers, setPublishers] = useState<{ id: number; name: string }[]>([]);
  const [publisherInput, setPublisherInput] = useState<string>('');
  const [selectedPublisherId, setSelectedPublisherId] = useState<number | null>(null);

  const [categories, setCategories] = useState<{ id: number; name: string }[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<number | null>(null);
  const [categoryInput, setCategoryInput] = useState<string>('');
  const [selectedCategoriesList, setSelectedCategoriesList] = useState<{ id: number; name: string }[]>([]);

  const [forceRender, setForceRender] = useState(false);

  useEffect(() => {
    fetchBooks();
  }, [page, sortBy, activeSort, searchTerm]);

  useEffect(() => {
    fetchPublishers(publisherInput);
  }, [publisherInput]);

  useEffect(() => {
    fetchCategories(categoryInput);
  }, [categoryInput]);

  const fetchBooks = useCallback(async () => {
    setLoading(true);
    try {
      const data = await getBooks(page, 10, undefined, sortBy, activeSort, searchTerm);
      console.log(data);
      if (data && Array.isArray(data.items)) {
        const formattedBooks = await Promise.all(data.items.map(async (book: Book) => {
          try {
            const publisher = await getPublisherById(book.publisherId);
            const categories = await getCategoriesByBookId(book.id);
            return {
              ...book,
              author: cleanAuthor(book.author),
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
  }, [page, sortBy, activeSort, searchTerm]);

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
      setPublisherInput('');
      setSelectedPublisherId(publisher.id);
      form.setFieldsValue({ publisherName: publisher.name });
    }
  };

  const handleCategorySelect = (value: string) => {
    const category = categories.find(p => p.name === value);
    if (category) {
      setSelectedCategoriesList(prevList => [...prevList, category]);
      setCategoryInput('');
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

  const cleanAuthor = (author: string) => {
    return author.replace(/[^a-zA-Z, ]/g, "");
  };

  const handleViewDetails = async (id: number) => {
    try {
      const book = await getBookById(id);
      setSelectedBook(book);
      const publisher = await getPublisherById(book.publisherId);
      form.setFieldsValue({
        ...book,
        author: cleanAuthor(book.author),
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
            await joinCategoryToBook(selectedBook.id, category.id);
          }
        }

        const categories = await getCategoriesByBookId(selectedBook.id);
        const categoriesString = categories.map((category: any) => category.name).join(',');
        const createdBookRC = {
          id: '',
          Title: updatedBook.title,
          description: updatedBook.description || '',
          authors: updatedBook.author,
          image: '',
          publisher: '',
          publishedDate: '',
          categories: categoriesString,
          book_id: selectedBook.id,
        };

        await deleteBookRC(selectedBook.id)
        // await addBookRC(createdBookRC);


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
        await deleteBookRC(selectedBook.id);
        fetchBooks();
        handleCloseModal();
      } catch {
      }
    }
  };

  const handleCreateBook = async () => {
    try {
      const newBook = await form.validateFields();

      newBook.author = newBook.author;

      newBook.publisherId = selectedPublisherId;
      newBook.publisherName = publisherInput;

      newBook.price = Number(newBook.price);
      newBook.publicationDate = new Date().toISOString();
      newBook.active = true;
      newBook.stockQuantity = Number(newBook.stockQuantity);
      newBook.description = newBook.description || '';
      newBook.image = newBook.image || '';

      const createdBook = await createBook(newBook);
      if (createdBook && selectedCategoriesList.length > 0) {
        console.log(selectedCategoriesList)
        for (const category of selectedCategoriesList) {
          await joinCategoryToBook(createdBook.id, category.id);
        }
      }

      const categories = await getCategoriesByBookId(createdBook.id);
      const categoriesString = categories.map((category: any) => category.name).join(',');
      const createdBookRC = {
        id: '',
        Title: createdBook.title,
        description: createdBook.description || '',
        authors: createdBook.author,
        image: '',
        publisher: '',
        publishedDate: '',
        categories: categoriesString,
        book_id: createdBook.id,
      };
      console.log(createdBookRC);
      // await addBookRC(createdBookRC);
      message.success("Book created successfully");
      fetchBooks();

      handleCloseModal();
    } catch (error) {
      message.error("Failed to create book");
      console.error(error);
    }
  };

  const handleCreatePublisher = async () => {
    if (publisherInput) {
      try {
        await createPublisher({ name: publisherInput });
        message.success("Publisher created successfully");
        fetchPublishers();
        setPublisherInput('');
      } catch (error) {
        message.error("Failed to create publisher");
      }
    }
  };

  const handleCreateCategory = async () => {
    if (categoryInput) {
      try {
        await createCategory({ name: categoryInput });
        message.success("Category created successfully");
        fetchCategories();
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

  const handleImageUpload = async (file: File) => {
    try {
      const imageUrl = await uploadImage(file);
      form.setFieldsValue({ image: imageUrl });
      setForceRender(!forceRender);
      console.log('Current form values after setting image:', form.getFieldsValue());
    } catch (error) {
      console.error('Error uploading image:', error);
      message.error('Failed to upload image');
    }
  };

  const columns = [
    { title: 'Tên sách', dataIndex: 'title', key: 'title' },
    {
      title: 'Tác giả',
      dataIndex: 'author',
      key: 'author',
      render: (authors: string) => cleanAuthor(authors),
    },
    {
      title: 'NSX',
      dataIndex: 'publisherName',
      key: 'publisherName',
    },
    {
      title: 'Thể loại',
      dataIndex: 'categories',
      key: 'categories',
      render: (categories: { id: number; name: string }[]) => (
        categories.map(category => category.name).join(', ')
      ),
    },
    {
      title: 'Giá',
      dataIndex: 'price',
      key: 'price',
      render: (price: number) => `${price.toLocaleString()} VND`,
    },
    {
      title: 'Ngày XB ',
      dataIndex: 'publicationDate',
      key: 'publicationDate',
      render: (date: string) => dayjs(date).format('DD/MM/YYYY'),
    },
    { title: 'Số lượng', dataIndex: 'stockQuantity', key: 'stockQuantity' },
    {
      title: 'Thao tác',
      key: 'action',
      render: (_: any, record: Book) => (
        <Button type="link" onClick={() => handleViewDetails(record.id)}>Xem</Button>
      ),
    },
  ];

  return (
    <>
      <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <div style={{ display: 'flex', gap: '16px' }}>
          <Input
            placeholder="Tìm kiếm theo tên"
            value={searchTerm}
            onChange={e => {
              setSearchTerm(e.target.value);
              setPage(1);
            }}
            style={{ width: 300 }}
          />
        </div>

        <div style={{ display: 'flex', gap: '16px', alignItems: 'center' }}>
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

          <Button type="primary" onClick={() => { setIsCreating(true); setIsModalVisible(true); }}>
            Tạo mới
          </Button>
        </div>
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
        title={isCreating ? 'Tạo mới' : 'Chi tiết'}
        open={isModalVisible}
        onOk={isCreating ? handleCreateBook : handleUpdate}
        onCancel={handleCloseModal}
        footer={[
          <Button key="back" onClick={handleCloseModal}>Hủy</Button>,
          !isCreating && <Button key="delete" type="primary" danger onClick={handleDelete}>Xóa</Button>,
          <Button key="submit" type="primary" onClick={isCreating ? handleCreateBook : handleUpdate}>
            {isCreating ? 'Tạo' : 'Cập nhật'}
          </Button>,
        ]}
      >
        <Form form={form} layout="vertical">
          <Form.Item label="Tên sách" name="title" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Tác giả" name="author" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Nhà xuất bản" name="publisherName" rules={[{ required: true }]}>
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
          <Form.Item label="Thể loại" name="categories" rules={[{ required: false }]}>
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
          <Form.Item label="Mô tả" name="description" rules={[{ required: true }]}>
            <Input.TextArea rows={4} />
          </Form.Item>
          <Form.Item label="Số lượng" name="stockQuantity" rules={[{ required: true }]}>
            <InputNumber min={0} style={{ width: '100%' }} />
          </Form.Item>

          <Form.Item label="Giá" name="price" rules={[{ required: true }]}>
            <InputNumber min={0} style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item label="Ảnh" name="image" rules={[{ required: true }]}>
            {form.getFieldValue('image') ? (
              <img
                src={form.getFieldValue('image')}
                alt="Book"
                style={{ width: '100px', height: '150px', marginBottom: '10px' }}
              />
            ) : (
              <span>No image available</span>
            )}
            <Upload
              accept="image/*"
              showUploadList={false}
              beforeUpload={(file) => {
                handleImageUpload(file);
                return false;
              }}
            >
              <Button icon={<UploadOutlined />}>Upload Image</Button>
            </Upload>
          </Form.Item>



        </Form>
      </Modal>
    </>
  );
};

export default AdminBook;
