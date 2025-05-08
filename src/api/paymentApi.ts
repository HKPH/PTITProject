import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Payment';

export const getAllPayments = async (): Promise<any[]> => {
  try {
    const response = await axiosInstance.get(API_URL);
    return response.data;
  } catch (error) {
    console.error('Error fetching payments:', error);
    throw error;
  }
};

export const getPaymentById = async (id: number): Promise<any | null> => {
  try {
    const response = await axiosInstance.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching payment with ID ${id}:`, error);
    throw error;
  }
};

export const createPayment = async (paymentDto: any): Promise<any> => {
  try {
    const response = await axiosInstance.post(API_URL, paymentDto);
    return response.data;
  } catch (error) {
    console.error('Error creating payment:', error);
    throw error;
  }
};

export const updatePayment = async (id: number, paymentDto: any): Promise<any> => {
  try {
    const response = await axiosInstance.put(`${API_URL}/${id}`, paymentDto);
    return response.data;
  } catch (error) {
    console.error(`Error updating payment with ID ${id}:`, error);
    throw error;
  }
};

export const deletePayment = async (id: number): Promise<any> => {
  try {
    const response = await axiosInstance.delete(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error deleting payment with ID ${id}:`, error);
    throw error;
  }
};
