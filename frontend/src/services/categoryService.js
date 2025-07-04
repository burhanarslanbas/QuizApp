import api from './api/axiosConfig';
import { API_ENDPOINTS } from '../config';

export const categoryService = {
  getAllCategories: async () => {
    try {
      const response = await api.get(API_ENDPOINTS.CATEGORY.LIST);
      // API yanıtı doğrudan kategori listesi dönüyor olabilir
      return {
        data: Array.isArray(response.data) ? response.data : response.data.data,
        totalCount: Array.isArray(response.data) ? response.data.length : response.data.totalCount
      };
    } catch (error) {
      throw error;
    }
  },

  getCategory: async (id) => {
    try {
      const response = await api.get(`${API_ENDPOINTS.CATEGORY.DETAIL}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  createCategory: async (data) => {
    try {
      const response = await api.post(API_ENDPOINTS.CATEGORY.CREATE, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  updateCategory: async (data) => {
    try {
      const response = await api.put(API_ENDPOINTS.CATEGORY.UPDATE, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  deleteCategory: async (id) => {
    try {
      const response = await api.delete(`${API_ENDPOINTS.CATEGORY.DELETE}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Get category quizzes
  getCategoryQuizzes: async (categoryId) => {
    return get(`${API_ENDPOINTS.CATEGORY.QUIZZES}/${categoryId}`);
  }
}; 