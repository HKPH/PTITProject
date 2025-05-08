import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Shipment';

export const getAllShipments = async (): Promise<any[]> => {
  try {
    const response = await axiosInstance.get(API_URL);
    return response.data;
  } catch (error) {
    console.error('Error fetching shipments:', error);
    throw error;
  }
};

export const getShipmentById = async (id: number): Promise<any | null> => {
  try {
    const response = await axiosInstance.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching shipment with ID ${id}:`, error);
    throw error;
  }
};

export const createShipment = async (shipmentDto: any): Promise<any> => {
  try {
    const response = await axiosInstance.post(API_URL, shipmentDto);
    return response.data;
  } catch (error) {
    console.error('Error creating shipment:', error);
    throw error;
  }
};

export const updateShipment = async (id: number, shipmentDto: any): Promise<any> => {
  try {
    const response = await axiosInstance.put(`${API_URL}/${id}`, shipmentDto);
    return response.data;
  } catch (error) {
    console.error(`Error updating shipment with ID ${id}:`, error);
    throw error;
  }
};

export const deleteShipment = async (id: number): Promise<any> => {
  try {
    const response = await axiosInstance.delete(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error deleting shipment with ID ${id}:`, error);
    throw error;
  }
};
