import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const quizService = {
  // Get quiz by ID
  getQuiz: async (quizId) => {
    return get(`${API_ENDPOINTS.QUIZ.DETAIL}/${quizId}`);
  },

  // Get all quizzes with pagination
  getQuizzes: async (params = {}) => {
    try {
      console.log('Fetching quizzes with params:', params);
      const response = await get(API_ENDPOINTS.QUIZ.LIST, params);
      console.log('Quizzes response:', response);
      return response;
    } catch (error) {
      console.error('Error fetching quizzes:', error);
      throw error;
    }
  },

  // Get quizzes by category
  getQuizzesByCategory: async (categoryId) => {
    return get(`${API_ENDPOINTS.QUIZ.BY_CATEGORY}/${categoryId}`);
  },

  // Get quizzes by user
  getQuizzesByUser: async (userId) => {
    return get(`${API_ENDPOINTS.QUIZ.BY_USER}/${userId}`);
  },

  // Get quizzes created by current user
  getMyQuizzes: async () => {
    return get(API_ENDPOINTS.QUIZ.MY_QUIZZES);
  },

  // Create new quiz
  createQuiz: async (data) => {
    const request = {
      Title: data.title,
      Description: data.description,
      CategoryId: data.categoryId,
      IsActive: data.isActive ?? true,
      TimeLimit: data.timeLimit,
      PassingScore: data.passingScore,
      MaxAttempts: data.maxAttempts
    };
    return post(API_ENDPOINTS.QUIZ.CREATE, request);
  },

  // Update existing quiz
  updateQuiz: async (data) => {
    const request = {
      Id: data.id,
      Title: data.title,
      Description: data.description,
      CategoryId: data.categoryId,
      IsActive: data.isActive,
      TimeLimit: data.timeLimit,
      PassingScore: data.passingScore,
      MaxAttempts: data.maxAttempts
    };
    return put(API_ENDPOINTS.QUIZ.UPDATE, request);
  },

  // Delete quiz
  deleteQuiz: async (quizId) => {
    return del(`${API_ENDPOINTS.QUIZ.DETAIL}/${quizId}`);
  },

  deleteRange: async (data) => {
    // data: { QuizIds: ["id1", ...] }
    return del(`${API_ENDPOINTS.QUIZ.DELETE}/range`, { data });
  },

  // Quiz results operations
  getQuizResult: async (quizResultId) => {
    return get(`${API_ENDPOINTS.QUIZ.RESULTS}/${quizResultId}`);
  },

  getQuizResults: async () => {
    return get(API_ENDPOINTS.QUIZ.RESULTS);
  },

  getResultsByQuiz: async (quizId) => {
    return get(`${API_ENDPOINTS.QUIZ.RESULTS}/by-quiz/${quizId}`);
  },

  getResultsByUser: async (userId) => {
    return get(`${API_ENDPOINTS.QUIZ.RESULTS}/by-user/${userId}`);
  },

  getMyResults: async () => {
    try {
      const results = await get(API_ENDPOINTS.QUIZ_RESULT.MY_RESULTS);
      const totalQuizzes = results.length;
      const completedQuizzes = results.filter(r => r.isCompleted).length;
      const averageScore = results.length > 0 
        ? results.reduce((acc, curr) => acc + curr.score, 0) / results.length 
        : 0;
      const recentQuizzes = results
        .sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate))
        .slice(0, 5);

      return {
        totalQuizzes,
        completedQuizzes,
        averageScore,
        recentQuizzes
      };
    } catch (error) {
      console.error('My results fetch error:', error);
      throw error;
    }
  },

  createQuizResult: async (data) => {
    // data: { QuizId, Score, ... }
    return post(API_ENDPOINTS.QUIZ.RESULTS, data);
  },

  updateQuizResult: async (data) => {
    // data: { QuizResultId, ... }
    return put(API_ENDPOINTS.QUIZ.RESULTS, data);
  },

  deleteQuizResult: async (quizResultId) => {
    return del(`${API_ENDPOINTS.QUIZ.RESULTS}/${quizResultId}`);
  },

  deleteQuizResultsRange: async (ids) => {
    // ids: ["id1", ...]
    return del(`${API_ENDPOINTS.QUIZ.RESULTS}/range`, { data: ids });
  },

  // Quiz statistics
  getQuizStats: async (id, timeRange = 'all') => {
    return get(`${API_ENDPOINTS.QUIZ.DETAIL}/${id}/stats`, { timeRange });
  },

  getQuizLeaderboard: async (id) => {
    return get(`${API_ENDPOINTS.QUIZ.DETAIL}/${id}/leaderboard`);
  },

  // Quiz questions
  getQuizQuestions: async (id) => {
    return get(`${API_ENDPOINTS.QUIZ.DETAIL}/${id}/questions`);
  },

  addQuestionToQuiz: async (quizId, questionId) => {
    return post(`${API_ENDPOINTS.QUIZ.DETAIL}/${quizId}/questions`, { questionId });
  },

  removeQuestionFromQuiz: async (quizId, questionId) => {
    return del(`${API_ENDPOINTS.QUIZ.DETAIL}/${quizId}/questions/${questionId}`);
  },

  // Quiz participants
  getQuizParticipants: async (id) => {
    return get(`${API_ENDPOINTS.QUIZ.DETAIL}/${id}/participants`);
  },

  // Quiz categories
  getQuizCategories: async () => {
    return get(API_ENDPOINTS.CATEGORY.LIST);
  },

  // Quiz export/import
  exportQuiz: async (id) => {
    return get(`${API_ENDPOINTS.QUIZ.DETAIL}/${id}/export`, {
      responseType: 'blob'
    });
  },

  importQuiz: async (file) => {
    const formData = new FormData();
    formData.append('file', file);
    return post(`${API_ENDPOINTS.QUIZ.CREATE}/import`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
  },

  getStats: async () => {
    try {
      const results = await get(API_ENDPOINTS.QUIZ.RESULTS);
      const totalQuizzes = results.length;
      const completedQuizzes = results.filter(r => r.isCompleted).length;
      const averageScore = results.length > 0 
        ? results.reduce((acc, curr) => acc + curr.score, 0) / results.length 
        : 0;
      const recentQuizzes = results
        .sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate))
        .slice(0, 5);

      return {
        totalQuizzes,
        completedQuizzes,
        averageScore,
        recentQuizzes
      };
    } catch (error) {
      console.error('Stats fetch error:', error);
      throw error;
    }
  }
}; 