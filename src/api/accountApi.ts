// accountApi.ts
import axios from 'axios';

const API_URL = 'https://localhost:7262/api/Account';

interface Account {
  id: number;
  username: string;
  password: string;
  role: number;
  email: string;
  createDate: string;
  active: boolean;
}

interface AccountResponse {
  items: Account[];
  totalCount: number;
  pageIndex: number;
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  totalPages: number;
}

// Hàm lấy danh sách Account
export const getAccounts = async (page: number, pageSize: number, searchUsername: string | null = null): Promise<AccountResponse> => {
  try {
    const response = await axios.get<AccountResponse>(`${API_URL}`, {
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

// Hàm bật/tắt tài khoản
export const toggleAccountActive = async (accountId: number): Promise<void> => {
    try {
      await axios.put(`${API_URL}/${accountId}/changeActive`);
    } catch (error) {
      console.error("Error toggling account active status:", error);
      throw error;
    }
  };
  

// Hàm reset mật khẩu
export const resetPassword = async (accountId: number): Promise<void> => {
    try {
      await axios.put(`${API_URL}/${accountId}/resetPassword`);
    } catch (error) {
      console.error("Error resetting password:", error);
      throw error;
    }
  };
  
