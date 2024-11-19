// src/api/publisherApi.ts
import axios from 'axios';

const API_URL = 'https://localhost:7262/api/Category';

export const getCategoryById = async (id: number) => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};
export const getAllCategories = async () => {
    const response = await axios.get(API_URL);
    return response.data;
  };
  
  
export const createCategory= async (categoryData: any) => {
    console.log('Creating new  with data:', JSON.stringify(categoryData, null, 2));
    const response = await axios.post(API_URL, categoryData);
    return response.data;
};