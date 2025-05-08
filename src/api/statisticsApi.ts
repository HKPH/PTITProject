import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Statistics';

export const getBooksSoldByDay = async (date: string) => {
    const response = await axiosInstance.get(`${API_URL}/books-sold-by-day`, {
        params: { date }
    });
    return response.data;
};

export const getBooksSoldForLast7Days = async () => {
    const response = await axiosInstance.get(`${API_URL}/books-sold-for-last-7-days`);
    return response.data;
};

export const getBooksSoldByYear = async (year: number) => {
    const response = await axiosInstance.get(`${API_URL}/books-sold-by-year`, {
        params: { year }
    });
    return response.data;
};

export const getBooksSoldByQuarters = async (year: number) => {
    const response = await axiosInstance.get(`${API_URL}/books-sold-by-quarters`, {
        params: { year }
    });
    return response.data;
};

export const getRevenueByDay = async (date: string) => {
    const response = await axiosInstance.get(`${API_URL}/revenue-by-day`, {
        params: { date }
    });
    return response.data;
};

export const getRevenueForLast7Days = async () => {
    const response = await axiosInstance.get(`${API_URL}/revenue-for-last-7-days`);
    return response.data;
};

export const getRevenueByYear = async (year: number) => {
    const response = await axiosInstance.get(`${API_URL}/revenue-by-year`, {
        params: { year }
    });
    return response.data;
};

export const getRevenueByQuarters = async (year: number) => {
    const response = await axiosInstance.get(`${API_URL}/revenue-by-quarters`, {
        params: { year }
    });
    return response.data;
};

export const getRatingsCountByDay = async (date: string) => {
    const response = await axiosInstance.get(`${API_URL}/ratings-count-by-day`, {
        params: { date }
    });
    return response.data;
};

export const getRatingsForLast7Days = async () => {
    const response = await axiosInstance.get(`${API_URL}/ratings-for-last-7-days`);
    return response.data;
};

export const getRatingsByYear = async (year: number) => {
    const response = await axiosInstance.get(`${API_URL}/ratings-by-year`, {
        params: { year }
    });
    return response.data;
};

export const getRatingsByQuarters = async (year: number) => {
    const response = await axiosInstance.get(`${API_URL}/ratings-by-quarters`, {
        params: { year }
    });
    return response.data;
};
