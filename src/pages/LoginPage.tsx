import React, { useState } from 'react';
import { Form, Input, Button, message } from 'antd';
import { login } from '../api/authApi';
import { getCartByUserId } from '../api/cartApi';
import { Link, useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import '../styles/AuthStyles.css';

const LoginPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const onFinish = async (values: { username: string; password: string }) => {
    setLoading(true);
    try {
      const { token, userId } = await login(values.username, values.password);

      localStorage.setItem('token', token);
      localStorage.setItem('userId', userId);

      const decodedToken: any = jwtDecode(token);
      const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

      if (userRole === '0') {
        navigate('/admin');
      } else {
        navigate('/');
      }

      const cartData = await getCartByUserId(userId);
      const cartId = cartData.id;
      localStorage.setItem('cartId', cartId);
    } catch (error) {
      message.error('Đăng nhập thất bại, vui lòng thử lại!');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <Form onFinish={onFinish} layout="vertical">
        <Form.Item
          label="Tên đăng nhập"
          name="username"
          rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}>
          <Input />
        </Form.Item>
        <Form.Item
          label="Mật khẩu"
          name="password"
          rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}>
          <Input.Password />
        </Form.Item>
        <Button type="primary" htmlType="submit" loading={loading} block>
          Đăng nhập
        </Button>
        <Link to="/register" className="link">
          Chưa có tài khoản? Đăng ký
        </Link>
      </Form>
    </div>
  );
};

export default LoginPage;
