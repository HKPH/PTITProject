import React, { useState, useEffect } from "react";
import { Layout, Input, AutoComplete, Badge, Dropdown, Menu, Tooltip, Row, Col } from "antd";
import { ShoppingCartOutlined, BellOutlined, UserOutlined, SearchOutlined, FacebookOutlined, InstagramOutlined, YoutubeOutlined, TwitterOutlined, PinterestOutlined } from "@ant-design/icons";
import Component from "../components/CustomerBook";
import { getBooks } from "../api/bookApi";
import { Link, useNavigate } from 'react-router-dom';
import { getOrdersByUserId } from "../api/orderApi";
import { useMediaQuery } from 'react-responsive';

const { Header, Content, Footer } = Layout;
const { Search } = Input;

const primaryColor = "rgb(255, 87, 51)";

const CustomerHomePage: React.FC = () => {
  const navigate = useNavigate();
  const userId = Number(localStorage.getItem("userId"));
  const [searchSuggestions, setSearchSuggestions] = useState<{
    title: string;
    image: string;
    price: number;
    id: number;
  }[]>([]);
  const [searchInput, setSearchInput] = useState("");
  const [hasPendingReviews, setHasPendingReviews] = useState(false);

  const isSmallScreen = useMediaQuery({ query: "(max-width: 1200px)" });

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
        <img src={book.image} alt={book.title} style={{ width: "40px", height: "60px", objectFit: "cover" }} />
        <div>
          <div>{book.title}</div>
          <div style={{ color: primaryColor }}>{book.price.toLocaleString()} VND</div>
        </div>
      </div>
    ),
  });

  const accountMenu = {
    items: [

      {
        key: "1",
        label: <Link to="/order">Đơn hàng của tôi</Link>,
      },
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
      <Layout>
        <Header
          style={{
            backgroundColor: "white",
            padding: "0 24px",
            display: "flex",
            alignItems: "center",
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
            {isSmallScreen ? (
              <>
                <Tooltip
                  title={hasPendingReviews ? "Bạn có sản phẩm chưa đánh giá" : "Không có thông báo mới"}
                  overlayInnerStyle={{
                    backgroundColor: "#fff",
                    color: "#333",
                    border: "1px solid #ddd",
                  }}
                >
                  <div
                    style={{ marginRight: "16px", display: "flex", alignItems: "center", cursor: "pointer" }}
                    onClick={() => navigate("/order")}
                  >
                    <Badge
                      dot={hasPendingReviews}
                      style={{ backgroundColor: primaryColor }}
                    >
                      <BellOutlined style={{ fontSize: "20px", color: primaryColor }} />
                    </Badge>
                  </div>
                </Tooltip>

                <Link to="/cart" style={{ textDecoration: "none", color: primaryColor }}>
                  <div style={{ marginRight: "16px", display: "flex", alignItems: "center" }}>
                    <ShoppingCartOutlined style={{ fontSize: "20px", color: primaryColor, marginRight: "8px" }} />
                  </div>
                </Link>

                <Dropdown menu={accountMenu} trigger={["hover"]}>
                  <div style={{ display: "flex", alignItems: "center", cursor: "pointer" }}>
                    <UserOutlined style={{ fontSize: "20px", marginRight: "8px", color: primaryColor }} />
                  </div>
                </Dropdown>
              </>
            ) : (
              <>
                <Tooltip
                  title={hasPendingReviews ? "Bạn có sản phẩm chưa đánh giá" : "Không có thông báo mới"}
                  overlayInnerStyle={{
                    backgroundColor: "#fff",
                    color: "#333",
                    border: "1px solid #ddd",
                  }}
                >
                  <div
                    style={{ marginRight: "16px", display: "flex", alignItems: "center", cursor: "pointer" }}
                    onClick={() => navigate("/order")}
                  >
                    <Badge
                      dot={hasPendingReviews}
                      style={{ backgroundColor: primaryColor }}
                    >
                      <BellOutlined style={{ fontSize: "20px", color: primaryColor }} />
                    </Badge>
                    <span style={{ fontWeight: "500", marginLeft: "8px" }}>Thông Báo</span>
                  </div>
                </Tooltip>

                <Link to="/cart" style={{ textDecoration: "none", color: primaryColor }}>
                  <div style={{ marginRight: "16px", display: "flex", alignItems: "center" }}>
                    <ShoppingCartOutlined style={{ fontSize: "20px", color: primaryColor, marginRight: "8px" }} />
                    <span style={{ fontWeight: "500" }}>Giỏ Hàng</span>
                  </div>
                </Link>

                <Dropdown menu={accountMenu} trigger={["hover"]}>
                  <div style={{ display: "flex", alignItems: "center", cursor: "pointer" }}>
                    <UserOutlined style={{ fontSize: "20px", marginRight: "8px", color: primaryColor }} />
                    <span style={{ fontWeight: "500" }}>Tài Khoản</span>
                  </div>
                </Dropdown>
              </>
            )}
          </div>
        </Header>

        <Content style={{ padding: "24px 16px" }}>
          <Component />
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

          <Row justify="space-between" align="middle" style={{ marginTop: "20px" }}>
            <div>
              <FacebookOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#4267B2" }} />
              <InstagramOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#E4405F" }} />
              <YoutubeOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#FF0000" }} />
              <TwitterOutlined style={{ fontSize: "24px", marginRight: "10px", color: "#1DA1F2" }} />
              <PinterestOutlined style={{ fontSize: "24px", color: "#E60023" }} />
            </div>
          </Row>

          <p style={{ textAlign: "center", marginTop: "20px", fontSize: "14px", color: "#999" }}>
            PTITBook ©{new Date().getFullYear()} Created by Your Company
          </p>
        </Footer>
      </Layout>
    </div>
  );
};

export default CustomerHomePage;
