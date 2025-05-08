import React, { useEffect, useState } from "react";
import {
    getShippingAddressesByUserId,
    createShippingAddress,
    updateShippingAddress,
    deleteShippingAddress,
} from "../api/shippingAddressApi";
import { Modal, Form, Input, Button, List, message, Radio } from "antd";
import { createOrder } from "../api/orderApi";
import { useNavigate } from "react-router-dom";


const CustomerConfirmOrder: React.FC = () => {
    const [shippingAddresses, setShippingAddresses] = useState<any[]>([]);
    const [selectedAddressId, setSelectedAddressId] = useState<number | null>(null);
    const [isModalVisible, setIsModalVisible] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [form] = Form.useForm();
    const [listModalVisible, setListModalVisible] = useState(false);
    const [paymentMethod, setPaymentMethod] = useState<string>("cash");
    const navigate = useNavigate();
    const userId = parseInt(localStorage.getItem("userId") || "0", 10);

    useEffect(() => {
        if (userId) {
            fetchShippingAddresses();
        }
    }, [userId]);

    const fetchShippingAddresses = async () => {
        try {
            const data = await getShippingAddressesByUserId(userId);
            setShippingAddresses(data);
            if (data.length > 0) {
                setSelectedAddressId(data[0].id);
            }
        } catch (error) {
            message.error("Lỗi khi lấy danh sách địa chỉ giao hàng!");
        }
    };

    const handleCreate = async (values: any) => {
        try {
            const newAddress = {
                id: 0,
                address: values.address,
                note: values.note,
                customerNumber: values.phoneNumber,
                userId: userId,
            };

            await createShippingAddress(newAddress);
            message.success("Địa chỉ giao hàng đã được tạo!");
            setIsModalVisible(false);
            fetchShippingAddresses();
        } catch (error) {
            message.error("Lỗi khi tạo địa chỉ giao hàng!");
        }
    };

    const handleEdit = async (values: any) => {
        if (selectedAddressId) {
            try {
                const updatedAddress = {
                    id: selectedAddressId,
                    address: values.address,
                    note: values.note,
                    customerNumber: values.phoneNumber,
                    userId: userId,
                };

                await updateShippingAddress(selectedAddressId, updatedAddress);
                message.success("Địa chỉ giao hàng đã được cập nhật!");
                setIsModalVisible(false);
                fetchShippingAddresses();
            } catch (error) {
                message.error("Lỗi khi cập nhật địa chỉ giao hàng!");
            }
        }
    };

    const handleDelete = async (id: number) => {
        try {
            await deleteShippingAddress(id);
            fetchShippingAddresses();
        } catch (error) {
        }
    };

    const openModal = (address?: any) => {
        if (address) {
            setIsEditing(true);
            setSelectedAddressId(address.id);
            form.setFieldsValue({
                ...address,
                phoneNumber: address.customerNumber,
            });
        } else {
            setIsEditing(false);
            form.resetFields();
        }
        setIsModalVisible(true);
    };

    const closeModal = () => {
        setIsModalVisible(false);
        form.resetFields();
    };

    const renderDefaultAddress = () => {
        const defaultAddress = shippingAddresses.find(
            (address) => address.id === selectedAddressId
        );
        return defaultAddress ? (
            <div
                onClick={() => setListModalVisible(true)}
                style={{
                    marginBottom: "20px",
                    backgroundColor: "#fff",
                    padding: "10px",
                    cursor: "pointer",
                    border: "1px solid #d9d9d9",
                    borderRadius: "8px",
                }}
            >
                <div>Địa chỉ giao hàng: {defaultAddress.address}</div>
                <div>Số điện thoại: {defaultAddress.customerNumber}</div>
            </div>
        ) : (
            <div>Không có địa chỉ mặc định.</div>
        );
    };


    const handlePlaceOrder = async () => {
        const orderData = {
            order: {
                userId: userId,
            },
            shipment: {
                shippingAddressId: selectedAddressId,
            },
            payment: {
                paymentMethod: paymentMethod,
            },
        };
        console.log("Order Data being sent:", orderData);

        if (paymentMethod === "qr") {
            try {
                await createOrder(orderData);
            } catch (error) {
            }
        } else {
            try {
                const response = await createOrder(orderData);
                console.log("Order successfully created:", response);
                navigate('/order');
            } catch (error) {
            }
        }
    };

    return (
        <div>
            <div className="shipping-address" style={{ backgroundColor: "#ffffff", padding: "10px", paddingLeft: "10px", paddingRight: "10px", borderRadius: "10px", boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)" }}>
                <h2>Địa chỉ giao hàng</h2>
                <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "20px" }}>
                    <div style={{ flexGrow: 1 }}>
                        {renderDefaultAddress()}
                    </div>
                    <Button type="primary" onClick={() => openModal()} style={{ marginLeft: "20px" }}>
                        Thêm địa chỉ
                    </Button>
                </div>

                <Modal
                    title="Danh sách địa chỉ"
                    open={listModalVisible}
                    footer={null}
                    onCancel={() => {
                        setListModalVisible(false);
                        form.resetFields();
                    }}
                >
                    <List
                        dataSource={shippingAddresses}
                        renderItem={(item) => (
                            <List.Item
                                actions={[
                                    <Button onClick={() => openModal(item)}>Sửa</Button>,
                                    <Button danger onClick={() => handleDelete(item.id)}>
                                        Xóa
                                    </Button>,
                                ]}
                            >
                                <div
                                    onClick={() => {
                                        setSelectedAddressId(item.id);
                                        console.log("Selected Address ID:", item.id);
                                        setListModalVisible(false);

                                    }

                                    }
                                    style={{
                                        cursor: "pointer",
                                        backgroundColor:
                                            item.id === selectedAddressId ? "#white" : "transparent",
                                        padding: "5px",
                                        borderRadius: "4px",
                                    }}
                                >
                                    {item.address}
                                </div>
                            </List.Item>
                        )}
                    />
                </Modal>

                <Modal
                    title={isEditing ? "Chỉnh sửa địa chỉ" : "Tạo mới địa chỉ"}
                    open={isModalVisible}
                    onCancel={closeModal}
                    onOk={() => {
                        form.validateFields().then((values) => {
                            if (isEditing) {
                                handleEdit(values);
                            } else {
                                handleCreate(values);
                            }
                        });
                    }}
                >
                    <Form form={form} layout="vertical">
                        <Form.Item
                            name="address"
                            label="Địa chỉ"
                            rules={[{ required: true, message: "Vui lòng nhập địa chỉ!" }]}
                        >
                            <Input />
                        </Form.Item>
                        <Form.Item name="note" label="Ghi chú">
                            <Input />
                        </Form.Item>
                        <Form.Item
                            name="phoneNumber"
                            label="Số điện thoại"
                            rules={[{ required: true, message: "Vui lòng nhập số điện thoại!" }]}
                        >
                            <Input />
                        </Form.Item>
                    </Form>
                </Modal>
            </div>

            <div className="payment-method" style={{ margin: "10px 0", backgroundColor: "#ffffff", padding: "10px", paddingLeft: "10px", paddingRight: "10px", borderRadius: "8px", boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)" }}>
                <h2>Phương thức thanh toán</h2>
                <Radio.Group
                    value={paymentMethod}
                    onChange={(e) => setPaymentMethod(e.target.value)}
                    style={{ marginBottom: "20px" }}
                >
                    <Radio value="cash">Tiền mặt</Radio>
                    {/* <Radio value="qr">Quét mã</Radio> */}
                </Radio.Group>
            </div>

            <div style={{ textAlign: "center" }}>
                <Button type="primary" size="large" onClick={handlePlaceOrder}>
                    Đặt hàng
                </Button>
            </div>
        </div>
    );
};

export default CustomerConfirmOrder;
