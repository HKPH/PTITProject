// src/pages/RegisterPage.tsx
import React, { useState } from 'react';
import { Form, Input, Button, DatePicker, Radio, message } from 'antd';
import { register } from '../api/authApi';
import { Link, useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import '../styles/AuthStyles.css'; // Nhập tệp CSS

const RegisterPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const onFinish = async (values: any) => {
    setLoading(true);
    const accountData = {
      account: {
        id: 0,
        username: values.username,
        password: values.password,
        role: 0,
        email: values.email,
        createDate: new Date().toISOString(),
        active: true,
      },
      user: {
        id: 0,
        name: values.name,
        phone: values.phone,
        email: values.email,
        address: values.address,
        dob: values.dob.format('YYYY-MM-DDTHH:mm:ss.SSSZ'),
        gender: values.gender,
        accountId: 0,
        active: true,
      },
    };

    try {
      await register(accountData);
      message.success('Đăng ký thành công! Hãy đăng nhập.');
      navigate('/login');
    } catch (error: any) {
      if (error.response?.data?.message === 'Username already exists.') {
        message.error('Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác!');
      } else {
        message.error('Đăng ký thất bại, vui lòng thử lại!');
      }
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
        <Form.Item label="Email" name="email" rules={[{ required: true, message: 'Vui lòng nhập email!' }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Tên đầy đủ" name="name" rules={[{ required: true, message: 'Vui lòng nhập tên!' }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Số điện thoại" name="phone" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại!' }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Địa chỉ" name="address" rules={[{ required: true, message: 'Vui lòng nhập địa chỉ!' }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Ngày sinh" name="dob" rules={[{ required: true, message: 'Vui lòng chọn ngày sinh!' }]}>
          <DatePicker />
        </Form.Item>
        <Form.Item label="Giới tính" name="gender" rules={[{ required: true, message: 'Vui lòng chọn giới tính!' }]}>
          <Radio.Group>
            <Radio value={0}>Nam</Radio>
            <Radio value={1}>Nữ</Radio>
          </Radio.Group>
        </Form.Item>
        <Button type="primary" htmlType="submit" loading={loading} block>
          Đăng ký
        </Button>
        <Link to="/login" className="link">Đã có tài khoản? Đăng nhập</Link>
      </Form>
    </div>
  );
};

export default RegisterPage;
