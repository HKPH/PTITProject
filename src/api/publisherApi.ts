import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Publisher';

export const getPublisherById = async (id: number) => {
  const response = await axiosInstance.get(`${API_URL}/${id}`);
  return response.data;
};
export const getAllPublishers = async () => {
  const response = await axiosInstance.get(API_URL);
  return response.data;
};


export const createPublisher = async (publisherData: any) => {
  const response = await axiosInstance.post(API_URL, publisherData);
  return response.data;
};