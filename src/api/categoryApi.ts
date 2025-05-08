import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Category';

export const getCategoryById = async (id: number) => {
  const response = await axiosInstance.get(`${API_URL}/${id}`);
  return response.data;
};
export const getAllCategories = async () => {
    const response = await axiosInstance.get(API_URL);
    return response.data;
  };
  
  
export const createCategory= async (categoryData: any) => {
    console.log('Creating new  with data:', JSON.stringify(categoryData, null, 2));
    const response = await axiosInstance.post(API_URL, categoryData);
    return response.data;
};