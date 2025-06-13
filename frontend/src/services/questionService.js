import api from './api/axiosConfig';

export const questionService = {
  getAllQuestions: async (params = {}) => {
    try {
      const response = await api.get('/questions', { params });
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  getQuestion: async (id) => {
    try {
      const response = await api.get(`/questions/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  createQuestion: async (data) => {
    try {
      const response = await api.post('/questions', data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  updateQuestion: async (id, data) => {
    try {
      const response = await api.put(`/questions/${id}`, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  deleteQuestion: async (id) => {
    try {
      const response = await api.delete(`/questions/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  getQuestionsByCategory: async (categoryId) => {
    try {
      const response = await api.get(`/questions/by-category/${categoryId}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },
}; 