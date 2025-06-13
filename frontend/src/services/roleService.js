import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

export const roleService = {
  // Get role by ID
  getRole: async (roleId) => {
    return get(`${API_ENDPOINTS.ROLE.DETAIL}/${roleId}`);
  },

  // Get role by name
  getRoleByName: async (roleName) => {
    return get(`${API_ENDPOINTS.ROLE.BY_NAME}/${roleName}`);
  },

  // Get all roles
  getRoles: async () => {
    return get(API_ENDPOINTS.ROLE.LIST);
  },

  // Get user roles
  getUserRoles: async (userId) => {
    return get(`${API_ENDPOINTS.ROLE.USER_ROLES}/${userId}`);
  },

  // Get role claims
  getRoleClaims: async (roleName) => {
    return get(`${API_ENDPOINTS.ROLE.CLAIMS}/${roleName}`);
  },

  // Create new role
  createRole: async (data) => {
    const request = {
      Name: data.name,
      Description: data.description
    };
    return post(API_ENDPOINTS.ROLE.CREATE, request);
  },

  // Update existing role
  updateRole: async (data) => {
    const request = {
      Id: data.id,
      Name: data.name,
      Description: data.description
    };
    return put(API_ENDPOINTS.ROLE.UPDATE, request);
  },

  // Delete role
  deleteRole: async (roleId) => {
    return del(`${API_ENDPOINTS.ROLE.DETAIL}/${roleId}`);
  },

  // Assign role to user
  assignRoleToUser: async (data) => {
    const request = {
      UserId: data.userId,
      RoleName: data.roleName
    };
    return post(API_ENDPOINTS.ROLE.ASSIGN, request);
  },

  // Remove role from user
  removeRoleFromUser: async (data) => {
    const request = {
      UserId: data.userId,
      RoleName: data.roleName
    };
    return post(API_ENDPOINTS.ROLE.REMOVE, request);
  },

  // Assign claims to role
  assignClaimsToRole: async (roleName, claims) => {
    return post(`${API_ENDPOINTS.ROLE.ASSIGN_CLAIMS}/${roleName}`, claims);
  }
}; 