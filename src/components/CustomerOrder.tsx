import React, { useEffect, useState } from 'react';
import { getOrdersByUserId, updateOrderStatus } from '../api/orderApi';
import { getBookById, uploadImage } from '../api/bookApi';
import { createCartItem } from '../api/cartItemApi';
import { createRating } from '../api/ratingApi'
import { addRating } from '../api/recommendApi';
import { UploadOutlined } from '@ant-design/icons';
import { Button, Card, Row, Col, Typography, Menu, Modal, Form, Input, Rate, Upload, message } from 'antd';
import { useNavigate } from 'react-router-dom';
const { Title, Text } = Typography;

interface OrderItem {
  bookId: number;
  quantity: number;
  price: number;
}

interface Order {
  id: number;
  status: number;
  totalPrice: number;
  orderItems: OrderItem[];
}

const CustomerOrder: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [bookDetails, setBookDetails] = useState<Record<number, any>>({});
  const [status, setStatus] = useState<number>(0);
  const userId = Number(localStorage.getItem('userId'));
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentOrderItems, setCurrentOrderItems] = useState<OrderItem[]>([]);
  const [form] = Form.useForm();
  const navigate = useNavigate();

  const openRatingPopup = (orderItems: OrderItem[]) => {
    setCurrentOrderItems(orderItems);
    setIsModalVisible(true);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
  };

  const handleSubmit = async (values: any) => {
    try {
      for (const item of currentOrderItems) {
        const photoUrl = values.photo ? await uploadImage(values.photo.file) : null;

        const ratingData = {
          bookId: item.bookId,
          userId,
          value: values.rating,
          createDate: new Date().toISOString(),
          comment: values.comment,
          photo: photoUrl,
          active: true,
        };

        await createRating(ratingData);
        await addRating(userId, item.bookId, values.rating);

      }
      const orderId = orders.find(order =>
        order.orderItems.every(item => currentOrderItems.some(ci => ci.bookId === item.bookId))
      )?.id;

      if (orderId) {
        await updateOrderStatus(orderId, 4);
      }

      const updatedOrders = await getOrdersByUserId(userId, status);
      setOrders(updatedOrders);


      message.success('Đánh giá thành công!');
      handleCancel();
    } catch (error) {
      console.error('Lỗi khi gửi đánh giá:', error);
      message.error('Lỗi khi gửi đánh giá.');
    }
  };

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const ordersData = await getOrdersByUserId(userId, status);
        setOrders(ordersData);
      } catch (error) {
        console.error('Lỗi khi lấy danh sách đơn hàng:', error);
        message.error('Lỗi khi tải danh sách đơn hàng.');
      }
    };

    fetchOrders();
  }, [userId, status]);

  const fetchBookDetails = async (bookId: number) => {
    if (!bookDetails[bookId]) {
      try {
        const book = await getBookById(bookId);
        setBookDetails((prev) => ({ ...prev, [bookId]: book }));
      } catch (error) {
        console.error(`Lỗi khi lấy thông tin sách với ID ${bookId}:`, error);
      }
    }
  };

  const handleCardClick = (slug: string, id: number) => {
    navigate(`/${slug}/${id}`);
  };

  const handleAddToCart = async (orderItems: OrderItem[]) => {
    const cartId = localStorage.getItem('cartId');
    if (!cartId) {
      message.error('Không tìm thấy giỏ hàng.');
      return;
    }
    try {
      for (let item of orderItems) {
        if (!bookDetails[item.bookId]) {
          await fetchBookDetails(item.bookId);
        }

        if (!bookDetails[item.bookId]) {
          console.error('Không tìm thấy thông tin sách cho ID', item.bookId);
          continue;
        }

        const cartItemData = {
          cartId: parseInt(cartId, 10),
          bookId: item.bookId,
          quantity: 1,
        };

        const response = await createCartItem(cartItemData);
        console.log('Thêm vào giỏ hàng:', response);
      }
      navigate('/cart');
    } catch (error) {
      console.error('Lỗi khi thêm sản phẩm vào giỏ hàng:', error);
    }
  };

  const handleCancelOrder = async (orderId: number) => {
    try {
      const result = await updateOrderStatus(orderId, -1);
      if (result) {
        const updatedOrders = await getOrdersByUserId(userId, status);
        setOrders(updatedOrders);
      }
    } catch (error) {
      console.error('Lỗi khi hủy đơn hàng:', error);
    }
  };

  const handleStatusChange = (key: string) => {
    setStatus(Number(key));
  };

  const menuItems = [
    { key: '0', label: 'Tất cả' },
    { key: '1', label: 'Đang chuẩn bị' },
    { key: '2', label: 'Đang giao hàng' },
    { key: '3', label: 'Đã giao thành công' },
    { key: '-1', label: 'Đã hủy' },
    { key: '4', label: 'Đã đánh giá' },
  ];

  return (
    <div>
      <Menu
        mode="horizontal"
        selectedKeys={[status.toString()]}
        onClick={(e) => handleStatusChange(e.key)}
        items={menuItems}
        style={{
          position: 'sticky',
          top: 0,
          zIndex: 10,
          backgroundColor: 'white',
          marginBottom: '20px',
          display: 'flex',
          justifyContent: 'center',
        }}
      />
      <div style={{ padding: '0 20px 20px 20px' }}>
        <Title level={3}>Danh sách đơn hàng</Title>
        <Row gutter={[16, 16]}>
          {orders.map((order) => (
            <Col span={24} key={order.id}>
              <Card>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Text strong>
                    Tình trạng:{' '}
                    {order.status === 1
                      ? 'Đang chuẩn bị'
                      : order.status === 2
                        ? 'Đang giao hàng'
                        : order.status === 3
                          ? 'Đã giao thành công'
                          : order.status === -1
                            ? 'Đã hủy'
                            : order.status === 4
                              ? 'Đã đánh giá'
                              : 'Không xác định'}
                  </Text>
                  <Text strong style={{ marginLeft: 'auto' }}>
                    Tổng giá: {order.totalPrice} đ
                  </Text>
                </div>
                <div style={{ marginTop: '10px' }}>
                  {order.orderItems.map((item) => {
                    fetchBookDetails(item.bookId);
                    const book = bookDetails[item.bookId];
                    return book ? (
                      <div
                        key={item.bookId}
                        style={{
                          display: 'flex',
                          alignItems: 'center',
                          marginBottom: '10px',
                        }}
                      >
                        <img
                          src={book.image}
                          alt={book.title}
                          style={{ width: '80px', height: '100px', marginRight: '15px' }}
                        />
                        <div>
                          <Text strong
                            style={{ cursor: 'pointer' }}
                            onClick={() => handleCardClick(book.title, book.id)}
                          >
                            {book.title}
                          </Text>
                          <br />
                          <Text>
                            Số lượng: {item.quantity} - Giá: {item.price} đ
                          </Text>
                        </div>
                      </div>
                    ) : (
                      <Text key={item.bookId}>Đang tải thông tin sách...</Text>
                    );
                  })}
                </div>
                <div
                  style={{
                    display: 'flex',
                    justifyContent: 'flex-end',
                    alignItems: 'center',
                    marginTop: '20px',
                  }}
                >
                  {order.status === 3 ? (
                    <Button
                      type="primary"
                      onClick={() => openRatingPopup(order.orderItems)}
                      style={{ marginRight: '10px' }}
                    >
                      Đánh giá
                    </Button>
                  ) : null}

                  {order.status === -1 || order.status === 3 || order.status === 4 ? (
                    <Button
                      type="primary"
                      onClick={() => handleAddToCart(order.orderItems)}
                    >
                      Mua lại
                    </Button>
                  ) : null}
                  {order.status === 1 ? (
                    <Button danger onClick={() => handleCancelOrder(order.id)} style={{ marginLeft: '10px' }}>
                      Hủy
                    </Button>
                  ) : null}
                </div>
              </Card>
            </Col>
          ))}
        </Row>
      </div>

      {/* Popup đánh giá */}
      <Modal
        visible={isModalVisible}
        title="Đánh giá sản phẩm"
        onCancel={handleCancel}
        onOk={() => form.submit()}
      >
        <Form form={form} onFinish={handleSubmit}>
          <Form.Item
            name="rating"
            label="Đánh giá"
            rules={[{ required: true, message: 'Vui lòng chọn mức đánh giá!' }]}
          >
            <Rate />
          </Form.Item>
          <Form.Item
            name="comment"
            label="Bình luận"
            rules={[{ required: true, message: 'Vui lòng nhập bình luận!' }]}
          >
            <Input.TextArea rows={4} />
          </Form.Item>
          <Form.Item name="photo" label="Ảnh">
            <Upload beforeUpload={() => false}>
              <Button icon={<UploadOutlined />}>Tải lên ảnh</Button>
            </Upload>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default CustomerOrder;
