import React, { useState } from 'react';
import { LaptopOutlined, BarChartOutlined, UserOutlined } from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Breadcrumb, Layout, Menu, theme } from 'antd';
import BookList from '../components/BookList';
import AccountList from '../components/AccountList';
import { ShoppingCartOutlined, ProfileOutlined } from '@ant-design/icons';
import { FacebookOutlined, InstagramOutlined, YoutubeOutlined, TwitterOutlined, PinterestOutlined } from '@ant-design/icons';

const { Header, Content, Footer, Sider } = Layout;

const items2: MenuProps['items'] = [
  {
    key: 'sub1',
    icon: <LaptopOutlined />,
    label: 'Quản lý',
    children: [
      { key: '1', label: 'Quản lý sách' },
      { key: '2', label: 'Quản lý đơn hàng' },
      { key: '3', label: 'Quản lý tài khoản' },
    ],
  },
  {
    key: 'sub2',
    icon: <BarChartOutlined />,
    label: 'Thống kê',
    children: [
      { key: '4', label: 'Thống kê doanh thu' },
      { key: '5', label: 'Thống kê khách hàng' },
      { key: '6', label: 'Thống kê sản phẩm' },
    ],
  },
];
const BookAdminPage: React.FC = () => {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const [selectedKey, setSelectedKey] = useState('1');

  const handleMenuClick = (e: { key: string }) => {
    setSelectedKey(e.key);
  };

  return (
    <div style={{ width: '80%', margin: '0 auto', backgroundColor: 'white', color: '#FF5733' }}>
      <Layout>
        <Header style={{ backgroundColor: 'white', padding: '0 24px', display: 'flex', alignItems: 'center' }}>
          <div style={{ marginRight: '16px', fontSize: '24px', fontWeight: 'bold', color: '#FF5733' }}>
            Fahasa.com
          </div>

          <input
            placeholder="Tìm kiếm sản phẩm"
            style={{ width: '50%', marginRight: 'auto', padding: '5px', borderRadius: '4px' }}
          />

          <div style={{ display: 'flex', alignItems: 'center', color: '#FF5733' }}>
            <div style={{ marginRight: '16px', display: 'flex', alignItems: 'center' }}>
              <ProfileOutlined style={{ marginRight: '8px' }} />
              Đơn Hàng
            </div>
            <div style={{ marginRight: '16px', display: 'flex', alignItems: 'center' }}>
              <ShoppingCartOutlined style={{ marginRight: '8px' }} />
              Giỏ Hàng
            </div>
            <div style={{ display: 'flex', alignItems: 'center' }}>
              <UserOutlined style={{ marginRight: '8px' }} />
              Tài Khoản
            </div>
          </div>
        </Header>

        <Content style={{ padding: '0 48px' }}>
          <Breadcrumb style={{ margin: '16px 0' }}>
            <Breadcrumb.Item>Home</Breadcrumb.Item>
            <Breadcrumb.Item>Admin</Breadcrumb.Item>
            <Breadcrumb.Item>Books</Breadcrumb.Item>
          </Breadcrumb>
          <Layout
            style={{ padding: '24px 0', background: colorBgContainer, borderRadius: borderRadiusLG }}
          >
            <Sider style={{ background: colorBgContainer }} width={200}>
              <Menu
                mode="inline"
                defaultSelectedKeys={['1']}
                defaultOpenKeys={['sub1']}
                style={{ height: '100%' }}
                items={items2}
                onClick={handleMenuClick}
              />
            </Sider>
            <Content style={{ padding: '0 24px', minHeight: 280 }}>
              {selectedKey === '1' && <BookList />}
              {selectedKey === '3' && <AccountList />}
            </Content>
          </Layout>
        </Content>

        <Footer style={{ backgroundColor: '#f1f1f1', padding: '20px', color: '#333' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '20px' }}>
            <div>
              <h3 style={{ color: '#FF5733' }}>Fahasa.com</h3>
              <p>60-62 Lê Lợi, Q.1, TP. HCM</p>
              <p>Email: cskh@fahasa.com.vn</p>
              <p>Phone: 1900636467</p>
            </div>

            <div>
              <h4>Dịch Vụ</h4>
              <p>Điều khoản sử dụng</p>
              <p>Chính sách bảo mật</p>
              <p>Giới thiệu Fahasa</p>
            </div>
            <div>
              <h4>Hỗ Trợ</h4>
              <p>Chính sách đổi - trả - hoàn tiền</p>
              <p>Chính sách bảo hành</p>
              <p>Chính sách vận chuyển</p>
            </div>
            <div>
              <h4>Tài Khoản Của Tôi</h4>
              <p>Đăng nhập/Tạo mới tài khoản</p>
              <p>Thay đổi địa chỉ khách hàng</p>
              <p>Lịch sử mua hàng</p>
            </div>
          </div>

          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <div>
              <FacebookOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#4267B2' }} />
              <InstagramOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#E4405F' }} />
              <YoutubeOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#FF0000' }} />
              <TwitterOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#1DA1F2' }} />
              <PinterestOutlined style={{ fontSize: '24px', color: '#E60023' }} />
            </div>

            <div>
              <img src="https://example.com/path/to/vnpay-logo.png" alt="VNPAY" style={{ height: '30px', marginRight: '10px' }} />
              <img src="https://example.com/path/to/momo-logo.png" alt="MOMO" style={{ height: '30px', marginRight: '10px' }} />
              <img src="https://example.com/path/to/shopee-pay-logo.png" alt="ShopeePay" style={{ height: '30px' }} />
            </div>
          </div>

          <p style={{ textAlign: 'center', marginTop: '20px', fontSize: '14px', color: '#999' }}>
            Fahasa ©{new Date().getFullYear()} Created by Your Company
          </p>
        </Footer>
      </Layout>
    </div>
  );
};

export default BookAdminPage;
