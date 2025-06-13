import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const userService = {
  // Get current user information
  getCurrentUser: async () => {
    try {
      return await get(API_ENDPOINTS.USER.ME);
    } catch (error) {
      if (error.response?.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
      }
      throw error;
    }
  },

  // Get user by ID
  getUser: async (id) => {
    return get(`${API_ENDPOINTS.USER.DETAIL}/${id}`);
  },

  // Get all users
  getUsers: async () => {
    return get(API_ENDPOINTS.USER.LIST);
  },

  // Update user information
  updateUser: async (userData) => {
    const request = {
      Id: userData.id,
      FirstName: userData.firstName,
      LastName: userData.lastName,
      Email: userData.email,
      PhoneNumber: userData.phoneNumber,
      IsActive: userData.isActive
    };
    return put(API_ENDPOINTS.USER.UPDATE, request);
  },

  // Delete user
  deleteUser: async (id) => {
    return del(`${API_ENDPOINTS.USER.DETAIL}/${id}`);
  },

  // Create user
  createUser: async (userData) => {
    return post(API_ENDPOINTS.USER.CREATE, userData);
  },

  getUsersByRole: async (role) => {
    return get(`${API_ENDPOINTS.USER.LIST}/role/${role}`);
  },

  getUsersByStatus: async (status) => {
    return get(`${API_ENDPOINTS.USER.LIST}/status/${status}`);
  },

  // User profile operations
  getProfile: async () => {
    return get(API_ENDPOINTS.USER.ME);
  },

  updateProfile: async (userData) => {
    return put(API_ENDPOINTS.USER.PROFILE, userData);
  },

  changePassword: async (currentPassword, newPassword) => {
    return put(API_ENDPOINTS.USER.CHANGE_PASSWORD, { currentPassword, newPassword });
  },

  // User statistics
  getUserStats: async (id) => {
    return get(`${API_ENDPOINTS.USER.DETAIL}/${id}/stats`);
  }
}; 