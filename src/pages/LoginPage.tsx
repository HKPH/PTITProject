// src/pages/LoginPage.tsx
import React, { useState } from 'react';
import { Form, Input, Button, message } from 'antd';
import { login } from '../api/authApi';
import { Link, useNavigate } from 'react-router-dom';
import '../styles/AuthStyles.css'; // Nhập tệp CSS

const LoginPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const onFinish = async (values: { username: string; password: string }) => {
    setLoading(true);
    try {
      const response = await login(values.username, values.password);
      message.success('Đăng nhập thành công!');
      navigate('/home'); // Điều hướng đến trang chính nếu đăng nhập thành công
    } catch (error) {
      message.error('Đăng nhập thất bại, vui lòng thử lại!');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container"> {/* Thêm lớp này */}
      <Form onFinish={onFinish} layout="vertical">
        <Form.Item label="Tên đăng nhập" name="username" rules={[{ required: true, message: 'Vui lòng nhập tên đăng nhập!' }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Mật khẩu" name="password" rules={[{ required: true, message: 'Vui lòng nhập mật khẩu!' }]}>
          <Input.Password />
        </Form.Item>
        <Button type="primary" htmlType="submit" loading={loading} block>
          Đăng nhập
        </Button>
        <Link to="/register" className="link">Chưa có tài khoản? Đăng ký</Link>
      </Form>
    </div>
  );
};

export default LoginPage;
