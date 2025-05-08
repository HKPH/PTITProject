import axiosInstance from "./axiosInstance";

const API_URL = 'https://localhost:7262/api/Book';

export const getBooks = async (
    page: number = 1, 
    pageSize: number = 10, 
    category?: string, 
    sortBy?: string, 
    isDescending: boolean = false, 
    searchTerm?: string
  ) => {
    const response = await axiosInstance.get(`${API_URL}`, {
      params: { page, pageSize, category, sortBy, isDescending, searchTerm },
    });
    return response.data;
  };
  

export const joinCategoryToBook = async (bookId: number, categoryId: number) => {
    const response = await axiosInstance.post(`${API_URL}/${bookId}/categories/${categoryId}`);
    return response.data;
  };

export const getBookById = async (id: number) => {
  const response = await axiosInstance.get(`${API_URL}/${id}`);
  return response.data;
};

export const createBook = async (bookData: any) => {
  const response = await axiosInstance.post(API_URL, bookData);
  return response.data;
};

export const updateBook = async (id: number, bookData: any) => {
  const response = await axiosInstance.put(`${API_URL}/${id}`, bookData);
  return response.data;
};

export const deleteBook = async (id: number) => {
  const response = await axiosInstance.delete(`${API_URL}/${id}`);
  return response.data;
};
export const getCategoriesByBookId = async (bookId: number) => {
    const response = await axiosInstance.get(`${API_URL}/${bookId}/categories`);
    return response.data;
};
interface UploadResponse {
  url: string;
}

export const uploadImage = async (file: File): Promise<string> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await axiosInstance.post<UploadResponse>(
    'https://localhost:7262/api/ImageUpload/upload',
    formData,
    {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    }
  );

  return response.data.url;
};

