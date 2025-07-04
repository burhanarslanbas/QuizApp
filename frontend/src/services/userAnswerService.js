import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const userAnswerService = {
  // Get user answer by ID
  getUserAnswer: async (userAnswerId) => {
    return get(`${API_ENDPOINTS.USER_ANSWER.DETAIL}/${userAnswerId}`);
  },

  // Get all user answers with pagination
  getUserAnswers: async (params = {}) => {
    return get(API_ENDPOINTS.USER_ANSWER.LIST, params);
  },

  // Get user answers by quiz result
  getUserAnswersByQuizResult: async (params = {}) => {
    return get(API_ENDPOINTS.USER_ANSWER.BY_QUIZ_RESULT, params);
  },

  // Create new user answer
  createUserAnswer: async (data) => {
    const request = {
      QuizResultId: data.quizResultId,
      QuestionId: data.questionId,
      OptionId: data.optionId,
      TextAnswer: data.textAnswer
    };
    return post(API_ENDPOINTS.USER_ANSWER.CREATE, request);
  },

  // Update existing user answer
  updateUserAnswer: async (data) => {
    const request = {
      Id: data.id,
      QuizResultId: data.quizResultId,
      QuestionId: data.questionId,
      OptionId: data.optionId,
      TextAnswer: data.textAnswer
    };
    return put(API_ENDPOINTS.USER_ANSWER.UPDATE, request);
  },

  // Delete user answer
  deleteUserAnswer: async (userAnswerId) => {
    return del(`${API_ENDPOINTS.USER_ANSWER.DETAIL}/${userAnswerId}`);
  },

  // Delete multiple user answers
  deleteUserAnswers: async (ids) => {
    return del(`${API_ENDPOINTS.USER_ANSWER.DELETE}/range`, { data: { ids } });
  }
}; 