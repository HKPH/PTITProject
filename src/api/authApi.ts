import axios from 'axios';

const API_URL = 'https://localhost:7262/api/Account';

export const login = async (username: string, password: string) => {
  const response = await axios.post(`${API_URL}/login`, { username, password });
  return response.data;
};


export const register = async (accountData: {
  account: {
    id: number;
    username: string;
    password: string;
    role: number;
    email: string;
    createDate: string;
    active: boolean;
  };
  user: {
    id: number;
    name: string;
    phone: string;
    email: string;
    address: string;
    dob: string;
    gender: number;
    accountId: number;
    active: boolean;
  };
}) => {
  return axios.post(`${API_URL}/create`, accountData);
};
