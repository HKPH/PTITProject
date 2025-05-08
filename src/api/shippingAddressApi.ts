import axiosInstance from "./axiosInstance";

const API_URL = "https://localhost:7262/api/ShippingAddress";

export const getShippingAddressesByUserId = async (userId: number) => {
  const response = await axiosInstance.get(`${API_URL}/user/${userId}`);
  return response.data;
};

export const getShippingAddressById = async (id: number) => {
  const response = await axiosInstance.get(`${API_URL}/${id}`);
  return response.data;
};

export const createShippingAddress = async (shippingAddressData: any) => {
  const response = await axiosInstance.post(API_URL, shippingAddressData);
  return response.data;
};

export const updateShippingAddress = async (
  id: number,
  shippingAddressData: any
) => {
  const response = await axiosInstance.put(`${API_URL}/${id}`, shippingAddressData);
  return response.data;
};

export const deleteShippingAddress = async (id: number) => {
  console.log("Deleting shipping address with ID:", id);
  const response = await axiosInstance.delete(`${API_URL}/${id}`);
  return response.data;
};
