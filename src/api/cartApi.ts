import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Cart';

export const getCartByUserId = async (userId: number): Promise<any> => {
    try {
      const response = await axiosInstance.get(`${API_URL}/user/${userId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching cart for userId: ${userId}`, error);
      throw error;
    }
  };