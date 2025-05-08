import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Account';

export const getAccounts = async (page: number, pageSize: number, searchUsername: string | null = null) => {
  try {
    const response = await axiosInstance.get(`${API_URL}`, {
      params: {
        page,
        pageSize,
        searchUsername
      }
    });
    return response.data;
  } catch (error) {
    console.error("Error fetching accounts:", error);
    throw error;
  }
};

export const toggleAccountActive = async (accountId: number) => {
    try {
      await axiosInstance.put(`${API_URL}/${accountId}/changeActive`);
    } catch (error) {
      console.error("Error toggling account active status:", error);
      throw error;
    }
};

export const resetPassword = async (accountId: number) => {
    try {
      await axiosInstance.put(`${API_URL}/${accountId}/resetPassword`);
    } catch (error) {
      console.error("Error resetting password:", error);
      throw error;
    }
};
