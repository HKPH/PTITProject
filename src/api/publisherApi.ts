// src/api/publisherApi.ts
import axios from 'axios';

const API_URL = 'https://localhost:7262/api/Publisher';

export const getPublisherById = async (id: number) => {
  const response = await axios.get(`${API_URL}/${id}`);
  return response.data;
};
export const getAllPublishers = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};


export const createPublisher = async (publisherData: any) => {
  console.log('Creating new  with data:', JSON.stringify(publisherData, null, 2));
  const response = await axios.post(API_URL, publisherData);
  return response.data;
};