import React, { useEffect, useState } from 'react';
import { Layout, Input, Spin, Button, Divider } from 'antd';
import { useParams } from 'react-router-dom';
import { getBookById } from '../api/bookApi';
import { getPublisherById } from '../api/publisherApi';
import { getCategoriesByBookId } from '../api/bookApi';
import { ShoppingCartOutlined, UserOutlined, ProfileOutlined, SearchOutlined } from '@ant-design/icons';
import { FacebookOutlined, InstagramOutlined, YoutubeOutlined, TwitterOutlined, PinterestOutlined } from '@ant-design/icons';
import CustomerBookList from '../components/CustomerBookList';
const { Header, Content, Footer } = Layout;
const { Search } = Input;

interface BookDetail {
    id: number;
    title: string;
    author: string;
    publisherId: number;
    price: number;
    publicationDate: string;
    stockQuantity: number;
    description: string;
    image: string;
    categoryId: number;
}
interface Category {
    id: number;
    name: string;
    active: boolean;
}

const ProductDetail: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [book, setBook] = useState<BookDetail | null>(null);
    const [publisher, setPublisher] = useState<string>('');
    const [category, setCategory] = useState<string>('');
    const [quantity, setQuantity] = useState<number>(1);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        if (id) {
            const bookId = parseInt(id, 10);

            if (!isNaN(bookId)) {
                getBookById(bookId)
                    .then(async (data) => {
                        setBook(data);

                        // Lấy tên nhà xuất bản
                        const publisherName = await getPublisherById(data.publisherId);
                        setPublisher(publisherName.name);

                        // Lấy danh sách thể loại
                        const categories: Category[] = await getCategoriesByBookId(bookId);

                        if (categories.length > 0) {
                            const categoryNames = categories.map((category: Category) => category.name).join(', ');
                            setCategory(categoryNames);
                        } else {
                            setCategory('Không có thể loại');
                        }

                        setLoading(false);
                    })
                    .catch((err) => {
                        console.error('Failed to fetch book details:', err);
                        setLoading(false);
                    });
            } else {
                console.error('Invalid book ID');
                setLoading(false);
            }
        }
    }, [id]);

    interface Category {
        id: number;
        name: string;
        active: boolean;
    }

    const handleAddToCart = () => {
        console.log('Thêm vào giỏ hàng:', { book, quantity });
    };

    const handleBuyNow = () => {
        console.log('Mua ngay:', { book });
        // Implement redirect to checkout or purchase flow
    };
    
    const cleanAuthor = (author: string) => {
        return author.replace(/[^a-zA-Z, ]/g, "");
    };

    return (
        <div style={{ width: '80%', margin: '0 auto', backgroundColor: 'white' }}>
            <Layout>
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

                <Content style={{ padding: '24px' }}>
                    {loading ? (
                        <div style={{ textAlign: 'center' }}>
                            <Spin size="large" />
                        </div>
                    ) : book ? (
                        <div>
                            <div style={{ display: 'flex', gap: '24px', alignItems: 'flex-start', padding: '16px', backgroundColor: '#f9f9f9', borderRadius: '8px' }}>
                                <img
                                    src={book.image}
                                    alt={book.title}
                                    style={{ width: '300px', height: '400px', objectFit: 'cover', borderRadius: '8px', boxShadow: '0 4px 8px rgba(0,0,0,0.1)' }}
                                />
                                <div style={{ flex: 1, paddingLeft: '16px' }}>
                                    <h1 style={{ color: '#FF5733', fontSize: '28px', fontWeight: 'bold', marginBottom: '16px' }}>{book.title}</h1>
                                    <p style={{ fontSize: '16px', marginBottom: '8px' }}><strong style={{ color: '#333' }}>Tác giả:</strong> {cleanAuthor(book.author)}</p>
                                    <p style={{ fontSize: '16px', marginBottom: '8px' }}><strong style={{ color: '#333' }}>Nhà xuất bản:</strong> {publisher}</p>
                                    <p style={{ fontSize: '16px', marginBottom: '8px' }}><strong style={{ color: '#333' }}>Thể loại:</strong> {category || 'Không có thể loại'}</p>
                                    <p style={{ fontSize: '18px', fontWeight: 'bold', color: '#FF5733', marginBottom: '16px' }}>{book.price.toLocaleString()} đ</p>
                                    <p style={{ fontSize: '16px', marginBottom: '16px' }}><strong style={{ color: '#333' }}>Tình trạng:</strong> {book.stockQuantity > 0 ? 'Còn hàng' : 'Hết hàng'}</p>
                                    <Divider />
                                    <div style={{ display: 'flex', alignItems: 'center', gap: '16px', marginTop: '16px' }}>
                                        <Button
                                            type="primary"
                                            onClick={handleAddToCart}
                                            style={{ backgroundColor: '#FF5733', borderColor: '#FF5733', fontWeight: 'bold', height: '70px', }}
                                        >
                                            Thêm vào giỏ hàng
                                        </Button>
                                        <Button
                                            type="primary"
                                            onClick={handleBuyNow}
                                            style={{ backgroundColor: '#FF5733', borderColor: '#FF5733', fontWeight: 'bold', height: '70px'}}
                                        >
                                            Mua ngay
                                        </Button>

                                    </div>
                                    <div style={{ marginTop: '16px', color: '#000', fontSize: '14px', lineHeight: '1.8' }}>
                                        <p style={{ color: '#FFA500', fontWeight: 'bold' }}>Gọi đặt hàng: <span style={{ color: '#FFA500', fontWeight: 'bold', textDecoration: 'underline' }}>(028) 3820 7153 hoặc 0933 109 009</span></p>
                                        <p style={{ fontWeight: 'bold' }}>Thông tin & Khuyến mãi: </p>
                                        <p>Đổi trả hàng trong vòng 7 ngày.</p>
                                        <p>Freeship toàn quốc từ 250.000đ.</p>
                                    </div>



                                </div>
                            </div>

                            <Divider />
                            <h3>Giới thiệu sản phẩm</h3>
                            <p><strong>Ngày xuất bản:</strong> {new Date(book.publicationDate).toLocaleDateString()}</p>
                            <p>{book.description}</p>
                            <Divider />
                            <h3>Đánh giá sản phẩm</h3>
                            <p>Chưa có đánh giá nào cho sản phẩm này.</p>
                        </div>
                    ) : (
                        <h2 style={{ textAlign: 'center' }}>Không tìm thấy sản phẩm</h2>
                    )}
                </Content>

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

export default ProductDetail;
