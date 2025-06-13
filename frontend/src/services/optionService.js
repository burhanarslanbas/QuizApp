import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const optionService = {
  // Get option by ID
  getOption: async (optionId) => {
    return get(`${API_ENDPOINTS.OPTION.DETAIL}/${optionId}`);
  },

  // Get all options with pagination
  getOptions: async (params = {}) => {
    return get(API_ENDPOINTS.OPTION.LIST, params);
  },

  // Get options by question ID
  getOptionsByQuestion: async (params = {}) => {
    return get(API_ENDPOINTS.OPTION.BY_QUESTION, params);
  },

  // Create new option
  createOption: async (data) => {
    const request = {
      QuestionId: data.questionId,
      Text: data.text,
      IsCorrect: data.isCorrect,
      Order: data.order,
      IsActive: data.isActive ?? true
    };
    return post(API_ENDPOINTS.OPTION.CREATE, request);
  },

  // Update existing option
  updateOption: async (data) => {
    const request = {
      Id: data.id,
      QuestionId: data.questionId,
      Text: data.text,
      IsCorrect: data.isCorrect,
      Order: data.order,
      IsActive: data.isActive
    };
    return put(API_ENDPOINTS.OPTION.UPDATE, request);
  },

  // Delete option
  deleteOption: async (optionId) => {
    return del(`${API_ENDPOINTS.OPTION.DETAIL}/${optionId}`);
  },

  createBulkOptions: async (questionId, optionsData) => {
    return post(`${API_ENDPOINTS.OPTION.LIST}/question/${questionId}/bulk`, optionsData);
  },

  updateBulkOptions: async (questionId, optionsData) => {
    return put(`${API_ENDPOINTS.OPTION.LIST}/question/${questionId}/bulk`, optionsData);
  }
}; 