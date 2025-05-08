import axiosInstance from './axiosInstance';

const API_URL = 'https://localhost:7262/api/Rating';


export const getRatingById = async (id: number) => {
    const response = await axiosInstance.get(`${API_URL}/${id}`);
    return response.data;
};

export const getRatingByBookId = async (bookId: number, page: number = 1, pageSize: number = 10, ratingValue?: number, sortByDescending: boolean = true) => {
    try {
        const response = await axiosInstance.get(`${API_URL}/book/${bookId}`, {
            params: {
                page,
                pageSize,
                ratingValue,
                sortByDescending
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching book ratings:', error);
        throw error;
    }
};


export const createRating = async (createRatingDto: any) => {
    const response = await axiosInstance.post(API_URL, createRatingDto);
    return response.data;
};

export const getRatingCountByBookId = async (bookId: number) => {
    try {
        const response = await axiosInstance.get(`${API_URL}/ratings/${bookId}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching rating count by bookId:', error);
        throw error;
    }
};
