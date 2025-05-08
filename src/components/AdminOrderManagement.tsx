import React, { useState, useEffect } from "react";
import { Table, Button, Input, Select, Modal, Spin, Tabs } from "antd";
import { getAllOrders, updateOrderStatus } from "../api/orderApi";
import { getShipmentById } from "../api/shipmentApi";
import { getPaymentById } from "../api/paymentApi";
import { getUserInfo } from "../api/userApi";
import { getBookById } from "../api/bookApi";
import { getShippingAddressById } from "../api/shippingAddressApi"

const { TabPane } = Tabs;

const AdminOrderManagement: React.FC = () => {
    const [orders, setOrders] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [selectedOrder, setSelectedOrder] = useState<any | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [sortBy, setSortBy] = useState<string>("OrderDate");
    const [sortDirection, setSortDirection] = useState<boolean>(false);
    const [modalVisible, setModalVisible] = useState<boolean>(false);

    const [currentPage, setCurrentPage] = useState<number>(1);
    const [pageSize] = useState<number>(10);
    const [totalOrders, setTotalOrders] = useState<number>(0);

    const fetchOrders = async (pageIndex = 1) => {
        setLoading(true);
        try {
            const response = await getAllOrders(pageIndex, pageSize, searchTerm, sortBy, sortDirection);
            const ordersWithDetails = await Promise.all(
                response.items.map(async (order: any) => {
                    const user = await getUserInfo(order.userId);
                    const shipment = await getShipmentById(order.shipmentId);
                    return {
                        ...order,
                        user,
                        shipment,
                    };
                })
            );

            setOrders(ordersWithDetails);
            setTotalOrders(response.totalCount);
            setCurrentPage(pageIndex);
        } catch (error) {
            console.error("Error fetching orders:", error);
        } finally {
            setLoading(false);
        }
    };

    const handleOrderClick = async (order: any) => {
        try {
            const user = await getUserInfo(order.userId);
            const shipment = await getShipmentById(order.shipmentId);
            const payment = await getPaymentById(order.paymentId);
            const books = await Promise.all(
                order.orderItems.map((item: any) => getBookById(item.bookId))
            );
            const shippingAddress = await getShippingAddressById(shipment.shippingAddressId);
            console.log(shipment)
            console.log(shippingAddress)

            setSelectedOrder({
                ...order,
                user,
                shipment,
                payment,
                books,
                shippingAddress,
            });
            setModalVisible(true);
        } catch (error) {
            console.error("Error fetching order details:", error);
        }
    };

    const handleStatusChange = async (orderId: number, newStatus: number) => {
        try {
            await updateOrderStatus(orderId, newStatus);
            fetchOrders(currentPage);
        } catch (error) {
            console.error("Error updating order status:", error);
        }
    };

    const columns = [
        {
            title: "ID",
            dataIndex: "id",
            render: (text: any, record: any) => (
                <Button type="link" onClick={() => handleOrderClick(record)}>
                    {text}
                </Button>
            ),
        },
        {
            title: "Khách hàng",
            render: (text: any, record: any) => record.user?.name || "Không rõ",
        },
        {
            title: "SĐT",
            render: (text: any, record: any) => record.user?.phone || "Không rõ",
        },
        {
            title: "Ngày đặt",
            dataIndex: "orderDate",
            render: (text: any) => new Date(text).toLocaleString().split(" ").pop(),
        },
        {
            title: "Ngày giao",
            render: (text: any, record: any) =>
                record.shipment?.dateReceived
                    ? new Date(record.shipment.dateReceived).toLocaleString().split(" ").pop()
                    : "Chưa xác định",
        },
        {
            title: "Giá trị",
            dataIndex: "totalPrice",
            render: (text: any) => `${text.toLocaleString()} VND`,
        },
        {
            title: "Trạng thái",
            render: (text: any, record: any) => {
                const statusMap = {
                    "-1": "Hủy",
                    "1": "Đang chuẩn bị",
                    "2": "Đang giao hàng",
                    "3": "Đã giao thành công",
                    "4": "Đã giao thành công",
                };
                return statusMap[record.status as "-1" | "1" | "2" | "3" | "4"];
            },
        },
        {
            title: "Thao tác",
            render: (text: any, record: any) => {
                const buttonText = record.status === 1 ? "Đã chuẩn bị" :
                    record.status === 2 ? "Đã giao hàng" :
                        "Thay đổi trạng thái";
                return (
                    (record.status === 1 || record.status === 2) && (
                        <Button onClick={() => handleStatusChange(record.id, record.status + 1)}>
                            {buttonText}
                        </Button>
                    )
                );
            },
        },

    ];

    const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(e.target.value);
    };

    const handleSortChange = (value: string) => {
        setSortBy(value);
        fetchOrders();
    };

    const handleSortDirectionChange = () => {
        setSortDirection(!sortDirection);
        fetchOrders();
    };

    useEffect(() => {
        fetchOrders();
    }, [searchTerm, sortBy, sortDirection]);

    const handlePageChange = (page: number) => {
        fetchOrders(page);
    };

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                <Input.Search
                    placeholder="Tìm kiếm đơn hàng"
                    value={searchTerm}
                    onChange={handleSearchChange}
                    onSearch={() => fetchOrders(1)}
                    style={{ width: 400 }}
                />

                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <Select
                        defaultValue="OrderDate-desc"
                        style={{ width: 200, marginRight: 10 }}
                        onChange={value => {
                            const [field, direct] = value.split('-');
                            setSortBy(field);

                            if (direct === 'asc') {
                                setSortDirection(true);
                            } else if (direct === 'desc') {
                                setSortDirection(false);
                            }
                        }}
                    >
                        <Select.Option value="OrderDate-asc">Thời gian tăng dần</Select.Option>
                        <Select.Option value="OrderDate-desc">Thời gian giảm dần</Select.Option>
                        <Select.Option value="status-asc">Trạng thái tăng dần</Select.Option>
                        <Select.Option value="status-desc">Trạng thái giảm dần</Select.Option>
                    </Select>
                </div>
            </div>


            <Table
                columns={columns}
                dataSource={orders}
                rowKey="id"
                loading={loading}
                pagination={{
                    current: currentPage,
                    pageSize,
                    total: totalOrders,
                    onChange: handlePageChange,
                }}
            />

            <Modal
                title="Chi tiết đơn hàng"
                open={modalVisible}
                onCancel={() => setModalVisible(false)}
                footer={null}
            >
                {selectedOrder ? (
                    <Tabs defaultActiveKey="1" items={[
                        {
                            label: "Thông tin",
                            key: "1",
                            children: (
                                <>
                                    <p><strong>Tên:</strong> {selectedOrder.user.name}</p>
                                    <p><strong>Số điện thoại:</strong> {selectedOrder.user.phone}</p>
                                </>
                            )
                        },
                        {
                            label: "Thanh toán",
                            key: "2",
                            children: (
                                <>
                                    <p><strong>Phương thức:</strong> {selectedOrder.payment?.paymentMethod}</p>
                                    <p><strong>Số tiền:</strong> {selectedOrder.payment?.amount}</p>
                                </>
                            )
                        },
                        {
                            label: "Địa chỉ",
                            key: "3",
                            children: (
                                <>
                                    <p><strong>Địa chỉ:</strong> {selectedOrder.shippingAddress?.address}</p>
                                    <p><strong>Ghi chú:</strong> {selectedOrder.shippingAddress?.note}</p>
                                    <p><strong>Phí giao hàng:</strong> {selectedOrder.shipment?.fee === 0 ? "Miễn phí" : selectedOrder.shipment?.fee}</p>
                                    <p><strong>Số điện thoại:</strong> {selectedOrder.shippingAddress?.customerNumber}</p>
                                    <p><strong>Ngày nhận hàng:</strong> {new Date(selectedOrder.shipment?.dateReceived).toLocaleString().split(" ").pop()}</p>

                                </>
                            )
                        },
                        {
                            label: "Sách",
                            key: "4",
                            children: (
                                <>
                                    {selectedOrder.books.map((book: any) => (
                                        <div key={book.id}>
                                            <p>{book.title} - {book.author}</p>
                                        </div>
                                    ))}
                                </>
                            )
                        }
                    ]} />

                ) : (
                    <Spin />
                )}
            </Modal>
        </div>
    );
};

export default AdminOrderManagement;
