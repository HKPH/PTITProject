import axios from 'axios';
const API_URL = 'https://localhost:7262/api/User';


export const getUserInfo = async (id: number) => {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  };