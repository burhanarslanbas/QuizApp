import api from './api/axiosConfig';
import { API_ENDPOINTS } from '../config';

export const questionService = {
  getAllQuestions: async (params = {}) => {
    try {
      // Transform frontend params to match backend expectations
      const backendParams = {
        quizId: params.quizId,
        questionType: params.questionType === 0 ? undefined : params.questionType, // Only send if not 0
        isActive: true // Default to true as per backend
      };

      const response = await api.get(API_ENDPOINTS.QUESTION.LIST, { params: backendParams });
      return {
        data: {
          items: response.data,
          totalCount: response.headers['x-total-count'] || response.data.length,
        },
      };
    } catch (error) {
      throw error;
    }
  },

  getQuestion: async (id) => {
    try {
      const response = await api.get(`${API_ENDPOINTS.QUESTION.DETAIL}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  createQuestion: async (data) => {
    // data zaten backend formatında gelmeli
    try {
      const response = await api.post(API_ENDPOINTS.QUESTION.CREATE, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  updateQuestion: async (data) => {
    try {
      const response = await api.put(API_ENDPOINTS.QUESTION.UPDATE, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  deleteQuestion: async (id) => {
    try {
      const response = await api.delete(`${API_ENDPOINTS.QUESTION.DELETE}/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  getQuestionsByCategory: async (categoryId) => {
    try {
      const response = await api.get(`${API_ENDPOINTS.QUESTION.BY_CATEGORY}/${categoryId}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },
}; 