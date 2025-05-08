import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCartItemByCartId, updateCartItem, deleteCartItem } from '../api/cartItemApi';
import { getBookById } from '../api/bookApi';

interface CartItem {
    id: number;
    cartId: number;
    bookId: number;
    quantity: number;
    book: {
        id: number;
        title: string;
        price: number;
        image: string;
    };
}

const CustomerOrderList: React.FC = () => {
    const [cartItems, setCartItems] = useState<CartItem[]>([]);
    const [cartId, setCartId] = useState<number | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const storedCartId = localStorage.getItem('cartId');
        if (storedCartId) {
            setCartId(Number(storedCartId));
        }
    }, []);

    useEffect(() => {
        if (cartId !== null) {
            const fetchCartItems = async () => {
                try {
                    const items = await getCartItemByCartId(cartId);

                    const itemsWithBooks = await Promise.all(
                        items.map(async (item: CartItem) => {
                            const book = await getBookById(item.bookId);
                            return { ...item, book };
                        })
                    );

                    setCartItems(itemsWithBooks);
                } catch (error) {
                    console.error('Error fetching cart items:', error);
                }
            };

            fetchCartItems();
        }
    }, [cartId]);

    const handleQuantityChange = async (itemId: number, quantity: number) => {
        try {
            const updatedItem = cartItems.find((item) => item.id === itemId);
            const newQuantity = quantity;
            if (updatedItem) {
                if (newQuantity <= 0) {
                    await deleteCartItem(itemId);
                    setCartItems((prevItems) => prevItems.filter((item) => item.id !== itemId));
                } else {
                    await updateCartItem(itemId, { ...updatedItem, quantity: newQuantity });
                    setCartItems((prevItems) =>
                        prevItems.map((item) =>
                            item.id === itemId ? { ...item, quantity: newQuantity } : item
                        )
                    );
                }
            }
        } catch (error) {
            console.error('Error updating cart item:', error);
        }
    };

    const handleDeleteItem = async (itemId: number) => {
        try {
            await deleteCartItem(itemId);
            setCartItems((prevItems) => prevItems.filter((item) => item.id !== itemId));
        } catch (error) {
            console.error('Error deleting cart item:', error);
        }
    };

    const totalPrice = cartItems.reduce((sum, item) => sum + item.quantity * item.book.price, 0);

    const handleOrder = () => {
        navigate('/order-confirm');
    };

    const handleCardClick = (slug: string, id: number) => {
        navigate(`/${slug}/${id}`);
    };

    return (
        <div style={{ maxWidth: '1000px', margin: '20px auto', padding: '20px', backgroundColor: 'white' }}>
            <h2 style={{ marginBottom: '20px' }}>Giỏ hàng</h2>
            {
                cartItems.length === 0 ? (
                    <p>Giỏ hàng của bạn đang trống.</p>
                ) : (
                    <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                        <div style={{ flex: 3 }}>
                            {cartItems.map((item) => (
                                <div
                                    key={item.id}
                                    style={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '20px',
                                        padding: '10px',
                                        borderBottom: '1px solid #ddd',
                                    }}
                                >
                                    <img
                                        src={item.book.image}
                                        alt={item.book.title}
                                        style={{ width: '80px', height: '100px', objectFit: 'cover' }}
                                    />

                                    <div style={{ flex: 2 }}>
                                        <h4
                                            style={{ cursor: 'pointer' }}
                                            onClick={() => handleCardClick(item.book.title, item.book.id)}
                                        >
                                            {item.book.title}
                                        </h4>
                                        <p>{item.book.price.toLocaleString()}đ</p>
                                        <button
                                            onClick={() => handleDeleteItem(item.id)}
                                            style={{
                                                marginTop: '10px',
                                                background: '#f44336',
                                                color: '#fff',
                                                border: 'none',
                                                padding: '5px 10px',
                                                cursor: 'pointer',
                                            }}
                                        >
                                            Xóa
                                        </button>
                                    </div>

                                    <p style={{ flex: 1, fontWeight: 'bold' }}>
                                        {(item.book.price).toLocaleString()}đ
                                    </p>

                                    <div
                                        style={{
                                            display: 'flex',
                                            alignItems: 'center',
                                            gap: '10px',
                                            flex: 1,
                                        }}
                                    >
                                        <button
                                            onClick={() =>
                                                handleQuantityChange(item.id, item.quantity - 1)
                                            }
                                            style={{
                                                padding: '5px',
                                                cursor: 'pointer',
                                                background: '#ddd',
                                                border: 'none',
                                                width: '30px',
                                                height: '30px',
                                            }}
                                        >
                                            -
                                        </button>
                                        <span>{item.quantity}</span>
                                        <button
                                            onClick={() =>
                                                handleQuantityChange(item.id, item.quantity + 1)
                                            }
                                            style={{
                                                padding: '5px',
                                                cursor: 'pointer',
                                                background: '#ddd',
                                                border: 'none',
                                                width: '30px',
                                                height: '30px',
                                            }}
                                        >
                                            +
                                        </button>
                                    </div>
                                </div>
                            ))}
                        </div>

                        <div
                            style={{
                                flex: 1,
                            }}
                        >
                            <div
                                style={{
                                    flex: 1,
                                    marginLeft: '20px',
                                    border: '1px solid #ddd',
                                    borderRadius: '5px',
                                    textAlign: 'center',
                                }} >
                                <h4 style={{ marginBottom: '10px', color: '#333' }}>
                                    {cartItems.reduce((sum, item) => sum + item.quantity, 0)} sản phẩm
                                </h4>
                                <h3 style={{ color: '#ff0000' }}>{totalPrice.toLocaleString()}đ</h3>

                                <button
                                    onClick={handleOrder} // Thêm hàm điều hướng khi nhấn "ĐẶT HÀNG"
                                    style={{
                                        background: '#ff6600',
                                        color: '#fff',
                                        border: 'none',
                                        padding: '10px 20px',
                                        cursor: 'pointer',
                                        marginTop: '20px',
                                        width: '100%',
                                    }}
                                >
                                    ĐẶT HÀNG
                                </button>
                            </div>
                        </div>
                    </div>
                )
            }
        </div >
    );
};

export default CustomerOrderList;
