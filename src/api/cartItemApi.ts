import axiosInstance from './axiosInstance';

// Địa chỉ API của bạn
const API_URL = 'https://localhost:7262/api/CartItem';

export const getAllCartItems = async (): Promise<any[]> => {
  try {
    const response = await axiosInstance.get(API_URL);
    return response.data;
  } catch (error) {
    console.error("Error fetching cart items", error);
    throw error;
  }
};

export const getCartItemById = async (id: number): Promise<any> => {
  try {
    const response = await axiosInstance.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching cart item with id: ${id}`, error);
    throw error;
  }
};

export const createCartItem = async (cartItemData: any): Promise<any> => {
  try {
    const response = await axiosInstance.post(API_URL, cartItemData);
    return response.data;
  } catch (error) {
    console.error("Error creating cart item", error);
    throw error;
  }
};

export const updateCartItem = async (id: number, cartItemData: any): Promise<string> => {
  try {
    const response = await axiosInstance.put(`${API_URL}/${id}`, cartItemData);
    return response.data.message;
  } catch (error) {
    console.error(`Error updating cart item with id: ${id}`, error);
    throw error;
  }
};

export const deleteCartItem = async (id: number): Promise<string> => {
  try {
    const response = await axiosInstance.delete(`${API_URL}/${id}`);
    return response.data.message;
  } catch (error) {
    console.error(`Error deleting cart item with id: ${id}`, error);
    throw error;
  }
};

export const getCartItemByCartId = async (cartId: number): Promise<any> => {
  try {
    const response = await axiosInstance.get(`${API_URL}/cart/${cartId}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching cart items`, error);
    throw error;
  }
}