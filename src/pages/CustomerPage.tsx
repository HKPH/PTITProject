import React from 'react';
import { Layout, Input } from 'antd';
import { ShoppingCartOutlined, UserOutlined, ProfileOutlined, SearchOutlined } from '@ant-design/icons';
import { FacebookOutlined, InstagramOutlined, YoutubeOutlined, TwitterOutlined, PinterestOutlined } from '@ant-design/icons';
import CustomerBookList from '../components/CustomerBookList';
const { Header, Content, Footer } = Layout;
const { Search } = Input;

const CustomerPage: React.FC = () => {
  return (
    <div style={{ width: '80%', margin: '0 auto', backgroundColor: 'white', color: '#FF5733' }}>
      <Layout>
        {/* Header */}
        <Header style={{ backgroundColor: 'white', padding: '0 24px', display: 'flex', alignItems: 'center' }}>
          <div style={{ marginRight: '16px', fontSize: '24px', fontWeight: 'bold', color: '#FF5733' }}>
            Fahasa.com
          </div>

          {/* Search bar */}
          <Search
            placeholder="Tìm kiếm sản phẩm"
            enterButton={<SearchOutlined />}
            style={{ width: '50%', marginRight: 'auto' }}
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

        {/* Content */}
        <Content style={{ padding: '24px' }}>
          <CustomerBookList />
        </Content>

        {/* Footer */}
        <Footer style={{ backgroundColor: '#f1f1f1', padding: '20px', color: '#333' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '20px' }}>
            {/* Contact Info */}
            <div>
              <h3 style={{ color: '#FF5733' }}>Fahasa.com</h3>
              <p>60-62 Lê Lợi, Q.1, TP. HCM</p>
              <p>Email: cskh@fahasa.com.vn</p>
              <p>Phone: 1900636467</p>
            </div>

            {/* Services */}
            <div>
              <h4>Dịch Vụ</h4>
              <p>Điều khoản sử dụng</p>
              <p>Chính sách bảo mật</p>
              <p>Giới thiệu Fahasa</p>
            </div>

            {/* Support */}
            <div>
              <h4>Hỗ Trợ</h4>
              <p>Chính sách đổi - trả - hoàn tiền</p>
              <p>Chính sách bảo hành</p>
              <p>Chính sách vận chuyển</p>
            </div>

            {/* Account */}
            <div>
              <h4>Tài Khoản Của Tôi</h4>
              <p>Đăng nhập/Tạo mới tài khoản</p>
              <p>Thay đổi địa chỉ khách hàng</p>
              <p>Lịch sử mua hàng</p>
            </div>
          </div>

          {/* Social Media and Payment Icons */}
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            {/* Social Media */}
            <div>
              <FacebookOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#4267B2' }} />
              <InstagramOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#E4405F' }} />
              <YoutubeOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#FF0000' }} />
              <TwitterOutlined style={{ fontSize: '24px', marginRight: '10px', color: '#1DA1F2' }} />
              <PinterestOutlined style={{ fontSize: '24px', color: '#E60023' }} />
            </div>

            {/* Payment Options */}
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

export default CustomerPage;
