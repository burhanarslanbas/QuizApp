import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const quizResultService = {
  // Get quiz result by ID
  getQuizResult: async (quizResultId) => {
    return get(`${API_ENDPOINTS.QUIZ_RESULT.DETAIL}/${quizResultId}`);
  },

  // Get all quiz results with pagination
  getQuizResults: async (params = {}) => {
    return get(API_ENDPOINTS.QUIZ_RESULT.LIST, params);
  },

  // Get quiz results by quiz ID
  getResultsByQuiz: async (quizId) => {
    return get(`${API_ENDPOINTS.QUIZ_RESULT.BY_QUIZ}/${quizId}`);
  },

  // Get quiz results by user ID
  getResultsByUser: async (userId) => {
    return get(`${API_ENDPOINTS.QUIZ_RESULT.BY_USER}/${userId}`);
  },

  // Get quiz results for current user
  getMyResults: async () => {
    return get(API_ENDPOINTS.QUIZ_RESULT.MY_RESULTS);
  },

  // Create new quiz result
  createQuizResult: async (data) => {
    const request = {
      QuizId: data.quizId,
      Score: data.score,
      TimeSpent: data.timeSpent,
      IsPassed: data.isPassed,
      AttemptNumber: data.attemptNumber,
      Answers: data.answers?.map(answer => ({
        QuestionId: answer.questionId,
        SelectedOptionId: answer.selectedOptionId,
        IsCorrect: answer.isCorrect
      }))
    };
    return post(API_ENDPOINTS.QUIZ_RESULT.CREATE, request);
  },

  // Update existing quiz result
  updateQuizResult: async (data) => {
    const request = {
      Id: data.id,
      QuizId: data.quizId,
      Score: data.score,
      TimeSpent: data.timeSpent,
      IsPassed: data.isPassed,
      AttemptNumber: data.attemptNumber,
      Answers: data.answers?.map(answer => ({
        QuestionId: answer.questionId,
        SelectedOptionId: answer.selectedOptionId,
        IsCorrect: answer.isCorrect
      }))
    };
    return put(API_ENDPOINTS.QUIZ_RESULT.UPDATE, request);
  },

  // Delete quiz result
  deleteQuizResult: async (quizResultId) => {
    return del(`${API_ENDPOINTS.QUIZ_RESULT.DETAIL}/${quizResultId}`);
  },

  getQuizStatistics: async (quizId) => {
    return await get(`${API_ENDPOINTS.QUIZ_RESULT.LIST}/quiz/${quizId}/statistics`);
  },

  getUserQuizHistory: async (userId) => {
    return await get(`${API_ENDPOINTS.QUIZ_RESULT.LIST}/user/${userId}/history`);
  }
}; 