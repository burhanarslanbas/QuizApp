import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const categoryService = {
  // Get all categories with pagination
  getCategories: async (params = {}) => {
    return get(API_ENDPOINTS.CATEGORY.LIST, params);
  },

  // Get category by ID
  getCategory: async (categoryId) => {
    return get(`${API_ENDPOINTS.CATEGORY.DETAIL}/${categoryId}`);
  },

  // Create new category
  createCategory: async (data) => {
    const request = {
      Name: data.name,
      Description: data.description
    };
    return post(API_ENDPOINTS.CATEGORY.CREATE, request);
  },

  // Update category
  updateCategory: async (data) => {
    const request = {
      Id: data.id,
      Name: data.name,
      Description: data.description
    };
    return put(API_ENDPOINTS.CATEGORY.UPDATE, request);
  },

  // Delete category
  deleteCategory: async (categoryId) => {
    return del(`${API_ENDPOINTS.CATEGORY.DETAIL}/${categoryId}`);
  },

  // Get category quizzes
  getCategoryQuizzes: async (categoryId) => {
    return get(`${API_ENDPOINTS.CATEGORY.QUIZZES}/${categoryId}`);
  }
}; 