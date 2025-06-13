import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

const USER_DATA_KEY = 'user_data';

const getCurrentUserId = () => {
  const userData = localStorage.getItem(USER_DATA_KEY);
  if (!userData) return null;
  try {
    const user = JSON.parse(userData);
    return user.id;
  } catch {
    return null;
  }
};

const questionPoolService = {
  getQuestionRepo: async (questionRepoId) => {
    return get(`${API_ENDPOINTS.QUESTION_REPO.DETAIL}/${questionRepoId}`);
  },
  
  getQuestionRepos: async (params = {}) => {
    return get(API_ENDPOINTS.QUESTION_REPO.LIST, params);
  },
  
  createQuestionRepo: async (data) => {
    const request = {
      Name: data.name,
      Description: data.description,
      MaxQuestions: data.maxQuestions ?? 10,
      IsPublic: data.isPublic ?? false
    };
    return post(API_ENDPOINTS.QUESTION_REPO.CREATE, request);
  },
  
  updateQuestionRepo: async (data) => {
    const request = {
      Id: data.id,
      Name: data.name,
      Description: data.description,
      MaxQuestions: data.maxQuestions ?? 10,
      IsPublic: data.isPublic ?? false,
      IsActive: data.isActive ?? true
    };
    return put(API_ENDPOINTS.QUESTION_REPO.UPDATE, request);
  },
  
  deleteQuestionRepo: async (questionRepoId) => {
    return del(`${API_ENDPOINTS.QUESTION_REPO.DETAIL}/${questionRepoId}`);
  },
  
  deleteQuestionRepos: async (ids) => {
    return del(`${API_ENDPOINTS.QUESTION_REPO.DELETE}/range`, { data: { ids } });
  }
};

export { questionPoolService }; 