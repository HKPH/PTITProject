import React, { useState, useEffect } from "react";
import { Layout, Input, AutoComplete, Badge, Dropdown, Menu, Tooltip, Breadcrumb, theme, Row, Col } from "antd";
import type { MenuProps } from 'antd';
import AdminBookList from '../components/AdminBook';
import AccountList from '../components/AdminAccount';
import OrderList from '../components/AdminOrderManagement';
import AdminStatisticChart from '../components/AdminStatisticChart';
import {
  LaptopOutlined,
  BarChartOutlined,
  ShoppingCartOutlined,
  BellOutlined,
  UserOutlined,
  SearchOutlined,
  FacebookOutlined,
  InstagramOutlined,
  YoutubeOutlined,
  TwitterOutlined,
  PinterestOutlined,
} from "@ant-design/icons";
import { getBooks } from "../api/bookApi";
import { Link, useNavigate } from 'react-router-dom';
import { getOrdersByUserId } from "../api/orderApi";
import { useMediaQuery } from 'react-responsive';

const { Header, Content, Footer, Sider } = Layout;
const { Search } = Input;

const primaryColor = "rgb(255, 87, 51)";
const items: MenuProps['items'] = [
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
      { key: '4', label: 'Thống kê doanh số' },
      { key: '5', label: 'Thống kê doanh thu' },
      { key: '6', label: 'Thống kê đánh giá' },
    ],
  },
];

const AdminPage: React.FC = () => {
  const navigate = useNavigate();
  const userId = Number(localStorage.getItem("userId"));
  const isSmallScreen = useMediaQuery({ query: "(max-width: 1200px)" });
  const [searchSuggestions, setSearchSuggestions] = useState<{
    title: string;
    image: string;
    price: number;
    id: number;
  }[]>([]);
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();
  const [searchInput, setSearchInput] = useState("");
  const [hasPendingReviews, setHasPendingReviews] = useState(false);
  const [selectedKey, setSelectedKey] = useState('1');
  const [collapsed, setCollapsed] = useState(false);

  const handleMenuClick = (e: { key: string }) => {
    setSelectedKey(e.key);
  };

  const renderContent = () => {
    switch (selectedKey) {
      case '1':
        return <AdminBookList />;
      case '2':
        return <OrderList />;
      case '3':
        return <AccountList />;
      case '4':
        return <AdminStatisticChart selectedStat="books" />;
      case '5':
        return <AdminStatisticChart selectedStat="revenue" />;
      case '6':
        return <AdminStatisticChart selectedStat="ratings" />;
      default:
        return null;
    }
  };

  useEffect(() => {
    const checkPendingReviews = async () => {
      try {
        const orders = await getOrdersByUserId(userId, 3);
        setHasPendingReviews(orders && orders.length > 0);
      } catch (error) {
        console.error("Error checking pending reviews:", error);
      }
    };
    checkPendingReviews();
  }, [userId]);

  const handleSearch = async (value: string) => {
    setSearchInput(value);
    if (value.trim()) {
      try {
        const { items } = await getBooks(1, 10, undefined, undefined, true, value);
        const suggestions = items.map((book: any) => ({
          title: book.title,
          image: book.image,
          price: book.price,
          id: book.id,
        }));
        setSearchSuggestions(suggestions);
      } catch (error) {
        console.error("Error fetching search suggestions:", error);
      }
    } else {
      setSearchSuggestions([]);
    }
  };

  const handleCardClick = (slug: string, id: number) => {
    navigate(`/${slug}/${id}`);
  };

  const renderSearchOption = (book: { title: string; image: string; price: number; id: number }) => ({
    value: JSON.stringify({ title: book.title, id: book.id }),
    label: (
      <div style={{ display: "flex", alignItems: "center", gap: "10px" }}>
        <img
          src={book.image}
          alt={book.title}
          style={{ width: "40px", height: "60px", objectFit: "cover" }}
        />
        <div>
          <div>{book.title}</div>
          <div style={{ color: primaryColor }}>{book.price.toLocaleString()} VND</div>
        </div>
      </div>
    ),
  });

  const accountMenu = {
    items: [
      { key: "1", label: <Link to="/order">Đơn hàng của tôi</Link> },
      ...(localStorage.getItem('userRole') === '0'
        ? [
          {
            key: "2",
            label: "Quản lý",
            onClick: () => navigate("/admin"),
          },
        ]
        : []),
      {
        key: "3",
        label: "Đăng xuất",
        onClick: () => {
          localStorage.clear();
          navigate('/login');
        },
      },
    ],
  };

  return (
    <div style={{ width: "80%", margin: "0 auto", backgroundColor: "white", color: primaryColor }}>
      <Layout style={{ minHeight: "100vh" }}>
        <Header
          style={{
            backgroundColor: "white",
            padding: "0 24px",
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
          }}
        >
          <div style={{ marginRight: "16px", fontSize: "24px", fontWeight: "bold", color: primaryColor }}>
            <Link to="/" style={{ textDecoration: "none", color: primaryColor }}>
              PTITBook.com
            </Link>
          </div>

          <AutoComplete
            popupMatchSelectWidth={true}
            style={{ maxWidth: "400px", marginRight: "auto", width: "100%" }}
            options={searchSuggestions.map(renderSearchOption)}
            onSelect={(value) => {
              const { title, id } = JSON.parse(value);
              handleCardClick(title, id);
            }}
            onSearch={handleSearch}
          >
            <Search placeholder="Tìm kiếm sản phẩm" enterButton={<SearchOutlined />} />
          </AutoComplete>

          <div style={{ display: "flex", alignItems: "center", color: primaryColor }}>
            <Tooltip title={hasPendingReviews ? "Bạn có sản phẩm chưa đánh giá" : "Không có thông báo mới"}>
              <div
                style={{ marginRight: "16px", display: "flex", alignItems: "center", cursor: "pointer" }}
                onClick={() => navigate("/order")}
              >
                <Badge dot={hasPendingReviews} style={{ backgroundColor: primaryColor }}>
                  <BellOutlined style={{ fontSize: "20px", color: primaryColor }} />
                </Badge>
                {!isSmallScreen && <span style={{ fontWeight: "500", marginLeft: "8px" }}>Thông Báo</span>}
              </div>
            </Tooltip>


            <Link to="/cart" style={{ textDecoration: "none", color: primaryColor }}>
              <div style={{ marginRight: "16px", display: "flex", alignItems: "center" }}>
                <ShoppingCartOutlined style={{ fontSize: "20px", color: primaryColor, marginRight: "8px" }} />
                {!isSmallScreen && <span style={{ fontWeight: "500" }}>Giỏ Hàng</span>}
              </div>
            </Link>


            <Dropdown menu={accountMenu} trigger={["hover"]}>
              <div style={{ display: "flex", alignItems: "center", cursor: "pointer" }}>
                <UserOutlined style={{ fontSize: "20px", marginRight: "8px", color: primaryColor }} />
                {!isSmallScreen && <span style={{ fontWeight: "500" }}>Tài Khoản</span>}
              </div>
            </Dropdown>

          </div>
        </Header>

        <Content style={{ padding: '0 24px' }}>
          <Breadcrumb style={{ margin: '16px 0' }}>
            <Breadcrumb.Item>Home</Breadcrumb.Item>
            <Breadcrumb.Item>Admin</Breadcrumb.Item>
          </Breadcrumb>

          <Layout style={{ padding: '24px 0', background: colorBgContainer, borderRadius: borderRadiusLG }}>
            <Sider
              style={{ background: colorBgContainer }}
              width={200}
              collapsible={false}
              collapsed={isSmallScreen || collapsed}
              onCollapse={(value) => setCollapsed(value)}
            >
              <Menu
                mode="inline"
                defaultSelectedKeys={['1']}
                defaultOpenKeys={['sub1']}
                style={{ height: '100%' }}
                items={items}
                onClick={handleMenuClick}
              />
            </Sider>


            <Content style={{ padding: '0 24px', minHeight: 280 }}>
              <Row gutter={[16, 16]}>
                <Col xs={24} sm={16} md={18} lg={24}>
                  {renderContent()}
                </Col>

                <Col xs={24} sm={8} md={6} lg={4}>

                </Col>
              </Row>
            </Content>
          </Layout>
        </Content>

        <Footer style={{ backgroundColor: "#f1f1f1", padding: "20px", color: "#333" }}>
          <Row gutter={[16, 16]}>
            <Col xs={24} sm={12} md={6}>
              <h3 style={{ color: primaryColor }}>PTITBook.com</h3>
              <p>60-62 Lê Lợi, Q.1, TP. HCM</p>
              <p>Email: cskh@PTITBook.com.vn</p>
              <p>Phone: 1900636467</p>
            </Col>
            <Col xs={24} sm={12} md={6}>
              <h4>Dịch Vụ</h4>
              <p>Điều khoản sử dụng</p>
              <p>Chính sách bảo mật</p>
              <p>Giới thiệu PTITBook</p>
            </Col>
            <Col xs={24} sm={12} md={6}>
              <h4>Hỗ Trợ</h4>
              <p>Chính sách đổi - trả - hoàn tiền</p>
              <p>Chính sách bảo hành</p>
              <p>Chính sách vận chuyển</p>
            </Col>
            <Col xs={24} sm={12} md={6}>
              <h4>Tài Khoản Của Tôi</h4>
              <p>Đăng nhập/Tạo mới tài khoản</p>
              <p>Thay đổi địa chỉ khách hàng</p>
              <p>Lịch sử mua hàng</p>
            </Col>
          </Row>

          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
            <div>
              <FacebookOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#4267B2" }} />
              <InstagramOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#E4405F" }} />
              <YoutubeOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#FF0000" }} />
              <TwitterOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#1DA1F2" }} />
              <PinterestOutlined style={{ fontSize: "24px", color: "#E60023" }} />
            </div>
          </div>

          <p style={{ textAlign: "center", marginTop: "20px", fontSize: "14px", color: "#999" }}>
            PTITBook ©{new Date().getFullYear()} Created by Your Company
          </p>
        </Footer>
      </Layout>
    </div>
  );
};

export default AdminPage;
