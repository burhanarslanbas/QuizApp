// API Configuration
export const API_URL = 'https://localhost:7001';

// Token configuration
export const TOKEN_CONFIG = {
  ACCESS_TOKEN_KEY: 'access_token',
  REFRESH_TOKEN_KEY: 'refresh_token',
  TOKEN_EXPIRATION_KEY: 'expires_at',
  REFRESH_TOKEN_EXPIRATION_KEY: 'refresh_token_expires_at',
  USER_KEY: 'user_data',
  REFRESH_INTERVAL: 14 * 60 * 1000 // 14 minutes
};

// Auth Configuration
export const AUTH_CONFIG = {
  TOKEN_REFRESH_THRESHOLD: 60 * 1000, // 1 minute before token expiration
  MAX_REFRESH_ATTEMPTS: 3
};

// Remember Me Configuration
export const REMEMBER_CONFIG = {
  REMEMBER_KEY: 'remember_me',
  EMAIL_KEY: 'remembered_email'
};

// API Endpoints
export const API_ENDPOINTS = {
  AUTH: {
    LOGIN: '/api/auth/login',
    REGISTER: '/api/auth/register',
    REFRESH_TOKEN: '/api/auth/refresh-token',
    REVOKE_TOKEN: '/api/auth/revoke-token'
  },
  USER: {
    ME: '/api/users/me',
    LIST: '/api/users',
    DETAIL: '/api/users',
    UPDATE: '/api/users',
    DELETE: '/api/users',
    STATS: '/api/users/stats'
  },
  QUIZ: {
    LIST: '/api/quizzes',
    DETAIL: '/api/quizzes',
    CREATE: '/api/quizzes',
    UPDATE: '/api/quizzes',
    DELETE: '/api/quizzes',
    BY_CATEGORY: '/api/quizzes/by-category',
    BY_USER: '/api/quizzes/by-user',
    MY_QUIZZES: '/api/quizzes/my-quizzes'
  },
  QUESTION: {
    LIST: '/api/questions',
    DETAIL: '/api/questions',
    CREATE: '/api/questions',
    UPDATE: '/api/questions',
    DELETE: '/api/questions',
    BY_CATEGORY: '/api/questions/by-category'
  },
  CATEGORY: {
    LIST: '/api/categories',
    DETAIL: '/api/categories',
    CREATE: '/api/categories',
    UPDATE: '/api/categories',
    DELETE: '/api/categories'
  },
  OPTION: {
    LIST: '/api/options',
    DETAIL: '/api/options',
    CREATE: '/api/options',
    UPDATE: '/api/options',
    DELETE: '/api/options',
    BY_QUESTION: '/api/options/by-question'
  },
  QUIZ_RESULT: {
    LIST: '/api/quiz-results',
    DETAIL: '/api/quiz-results',
    CREATE: '/api/quiz-results',
    UPDATE: '/api/quiz-results',
    DELETE: '/api/quiz-results',
    BY_QUIZ: '/api/quiz-results/by-quiz',
    BY_USER: '/api/quiz-results/by-user',
    MY_RESULTS: '/api/quiz-results/my-results'
  },
  USER_ANSWER: {
    LIST: '/api/user-answers',
    DETAIL: '/api/user-answers',
    CREATE: '/api/user-answers',
    UPDATE: '/api/user-answers',
    DELETE: '/api/user-answers',
    BY_QUIZ_RESULT: '/api/user-answers/by-quiz-result'
  },
  ROLE: {
    LIST: '/api/roles',
    DETAIL: '/api/roles',
    CREATE: '/api/roles',
    UPDATE: '/api/roles',
    DELETE: '/api/roles',
    BY_NAME: '/api/roles/by-name',
    USER_ROLES: '/api/roles/user-roles',
    CLAIMS: '/api/roles/claims',
    ASSIGN: '/api/roles/assign',
    REMOVE: '/api/roles/remove',
    ASSIGN_CLAIMS: '/api/roles/assign-claims'
  },
  QUIZ_QUESTION: {
    LIST: '/api/quiz-questions',
    DETAIL: '/api/quiz-questions',
    CREATE: '/api/quiz-questions',
    UPDATE: '/api/quiz-questions',
    DELETE: '/api/quiz-questions',
    BY_QUIZ: '/api/quiz-questions/by-quiz'
  },
  QUESTION_REPO: {
    LIST: '/api/question-repos',
    DETAIL: '/api/question-repos',
    CREATE: '/api/question-repos',
    UPDATE: '/api/question-repos',
    DELETE: '/api/question-repos'
  }
}; 