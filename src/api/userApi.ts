import axiosInstance from './axiosInstance';
const API_URL = 'https://localhost:7262/api/User';


export const getUserInfo = async (id: number) => {
    const response = await axiosInstance.get(`${API_URL}/${id}`);
    return response.data;
  };