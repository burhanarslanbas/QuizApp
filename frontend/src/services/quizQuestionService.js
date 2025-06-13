import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const quizQuestionService = {
  // Get quiz question by ID
  getQuizQuestion: async (quizQuestionId) => {
    return get(`${API_ENDPOINTS.QUIZ_QUESTION.DETAIL}/${quizQuestionId}`);
  },

  // Get all quiz questions with pagination
  getQuizQuestions: async (params = {}) => {
    return get(API_ENDPOINTS.QUIZ_QUESTION.LIST, params);
  },

  // Get quiz questions by quiz ID
  getQuestionsByQuiz: async (quizId) => {
    return get(`${API_ENDPOINTS.QUIZ_QUESTION.BY_QUIZ}/${quizId}`);
  },

  // Create new quiz question
  createQuizQuestion: async (data) => {
    const request = {
      QuizId: data.quizId,
      QuestionId: data.questionId,
      Order: data.order,
      Points: data.points,
      IsActive: data.isActive ?? true
    };
    return post(API_ENDPOINTS.QUIZ_QUESTION.CREATE, request);
  },

  // Update existing quiz question
  updateQuizQuestion: async (data) => {
    const request = {
      Id: data.id,
      QuizId: data.quizId,
      QuestionId: data.questionId,
      Order: data.order,
      Points: data.points,
      IsActive: data.isActive
    };
    return put(API_ENDPOINTS.QUIZ_QUESTION.UPDATE, request);
  },

  // Delete quiz question
  deleteQuizQuestion: async (quizQuestionId) => {
    return del(`${API_ENDPOINTS.QUIZ_QUESTION.DETAIL}/${quizQuestionId}`);
  },

  // Add question to quiz
  addQuestionToQuiz: async (quizId, questionId) => {
    return post(`${API_ENDPOINTS.QUIZ_QUESTION.LIST}/quiz/${quizId}/question/${questionId}`);
  },

  // Remove question from quiz
  removeQuestionFromQuiz: async (quizId, questionId) => {
    return del(`${API_ENDPOINTS.QUIZ_QUESTION.LIST}/quiz/${quizId}/question/${questionId}`);
  }
}; 