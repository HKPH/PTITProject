import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Order';

export const getAllOrders = async (page = 1, pageSize = 10, searchTerm = '', sortBy = 'OrderDate', sortDirection = false) => {
  const response = await axiosInstance.get(API_URL, {
    params: { page, pageSize, searchTerm, sortBy, sortDirection },
  });
  console.log(response.data);
  return response.data;
};

export const getOrdersByUserId = async (userId: number, status: number = -1) => {
  const response = await axiosInstance.get(`${API_URL}/user/${userId}`, {
    params: { status },
  });
  return response.data;
};


export const getOrderById = async (id: number) => {
  const response = await axiosInstance.get(`${API_URL}/${id}`);
  return response.data;
};

export const createOrder = async (createOrderDto: any) => {
  const response = await axiosInstance.post(API_URL, createOrderDto);
  return response.data;
};

export const updateOrderStatus = async (id: number, status: number) => {
  try {
    const response = await axiosInstance.put(`${API_URL}/${id}`, null, {
      params: { status },
    });
    return response.data;
  } catch (error) {
    console.error('Error updating order status:', error);
  }
};

export const deleteOrder = async (id: number) => {
  const response = await axiosInstance.delete(`${API_URL}/${id}`);
  return response.data;
};
