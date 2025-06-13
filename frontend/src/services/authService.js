import { get, post, put, del } from './baseService';
import { API_ENDPOINTS } from '../config';

// Standard keys
const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const EXPIRES_AT_KEY = 'expires_at';
const REFRESH_TOKEN_EXPIRES_AT_KEY = 'refresh_token_expires_at';
const USER_DATA_KEY = 'user_data';

const clearLegacyKeys = () => {
  // Remove all legacy/old keys
  localStorage.removeItem('token');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('tokenExpiration');
  localStorage.removeItem('refreshTokenExpiration');
  localStorage.removeItem('user');
  // Remove any previous new keys to avoid duplicates
  localStorage.removeItem(ACCESS_TOKEN_KEY);
  localStorage.removeItem(REFRESH_TOKEN_KEY);
  localStorage.removeItem(EXPIRES_AT_KEY);
  localStorage.removeItem(REFRESH_TOKEN_EXPIRES_AT_KEY);
  localStorage.removeItem(USER_DATA_KEY);
};

const authService = {
  login: async (data) => {
    try {
      console.log('Login request data:', data);
      const response = await post(API_ENDPOINTS.AUTH.LOGIN, {
        email: data.email,
        password: data.password
      });

      console.log('Login response:', response);

      if (!response) {
        throw new Error('Sunucudan yanıt alınamadı');
      }

      const responseData = response.data || response;

      if (!responseData.success) {
        throw {
          message: responseData.message || 'Giriş başarısız',
          errors: responseData.errors || []
        };
      }

      if (!responseData.token) {
        throw new Error('Token bilgisi alınamadı');
      }

      // Temizlik: Eski anahtarları sil
      clearLegacyKeys();

      // Store tokens and expiration times
      authService.saveTokens(responseData.token);
      
      if (responseData.user) {
        localStorage.setItem(USER_DATA_KEY, JSON.stringify(responseData.user));
      }

      return responseData;
    } catch (error) {
      console.error('Login error details:', error);
      
      if (error.isNetworkError) {
        throw new Error('Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edin.');
      }
      
      if (error.response?.data) {
        throw error.response.data;
      }
      
      throw error;
    }
  },

  register: async (data) => {
    try {
      console.log('Register request data:', data);
      const response = await post(API_ENDPOINTS.AUTH.REGISTER, data);
      console.log('Register response:', response);

      if (!response) {
        throw new Error('Sunucudan yanıt alınamadı');
      }

      const responseData = response.data || response;

      if (!responseData.success) {
        throw {
          message: responseData.message || 'Kayıt başarısız',
          errors: responseData.errors || []
        };
      }

      return {
        success: true,
        message: responseData.message || 'Kayıt başarılı'
      };
    } catch (error) {
      console.error('Register error details:', error);
      
      if (error.isNetworkError) {
        throw new Error('Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edin.');
      }
      
      if (error.response?.data) {
        throw error.response.data;
      }
      
      throw error;
    }
  },

  refreshToken: async () => {
    try {
      const refreshToken = authService.getRefreshToken();
      const accessToken = authService.getToken();

      console.log('Refresh token isteği için tokenlar:', {
        accessToken: accessToken ? 'Var' : 'Yok',
        refreshToken: refreshToken ? 'Var' : 'Yok'
      });

      if (!refreshToken || !accessToken) {
        throw new Error('Refresh token veya access token bulunamadı');
      }

      const response = await post(API_ENDPOINTS.AUTH.REFRESH_TOKEN, {
        AccessToken: accessToken,
        RefreshToken: refreshToken
      });

      console.log('Refresh token yanıtı:', response);

      const responseData = response && response.data ? response.data : response;

      if (!responseData.success) {
        console.error('Refresh token başarısız:', responseData);
        throw {
          message: responseData.message || 'Token yenileme başarısız',
          errors: responseData.errors || []
        };
      }

      if (!responseData.token) {
        console.error('Refresh token yanıtında token bilgisi yok:', responseData);
        throw new Error('Token bilgisi alınamadı');
      }

      console.log('Yeni tokenlar kaydediliyor:', responseData.token);
      authService.saveTokens(responseData.token);
      
      if (responseData.user) {
        localStorage.setItem(USER_DATA_KEY, JSON.stringify(responseData.user));
      }

      return responseData;
    } catch (error) {
      console.error('Refresh token error:', error);
      if (error.response?.data) {
        console.error('Refresh token error response:', error.response.data);
        throw error.response.data;
      }
      throw error;
    }
  },

  revokeToken: async () => {
    try {
      const accessToken = authService.getToken();
      const refreshToken = authService.getRefreshToken();

      if (!accessToken || !refreshToken) {
        throw new Error('Token bilgileri bulunamadı');
      }

      const response = await post(API_ENDPOINTS.AUTH.REVOKE_TOKEN, {
        AccessToken: accessToken,
        RefreshToken: refreshToken
      });

      const responseData = response.data || response;

      if (!responseData.success) {
        throw {
          message: responseData.message || 'Token iptali başarısız',
          errors: responseData.errors || []
        };
      }

      // Clear all tokens after successful revocation
      clearLegacyKeys();
      return responseData;
    } catch (error) {
      console.error('Revoke token error:', error);
      if (error.response?.data) {
        throw error.response.data;
      }
      throw error;
    }
  },

  logout: () => {
    clearLegacyKeys();
  },

  getCurrentUser: async () => {
    try {
      const response = await get(API_ENDPOINTS.USER.ME);
      return response.data;
    } catch (error) {
      if (error.response?.status === 401) {
        authService.logout();
      }
      throw error;
    }
  },

  validateToken: async (token) => {
    try {
      await get(API_ENDPOINTS.USER.ME);
      return true;
    } catch (error) {
      return false;
    }
  },

  getToken: () => {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  },

  getRefreshToken: () => {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  },

  saveTokens: (tokenData) => {
    if (tokenData.accessToken) {
      localStorage.setItem(ACCESS_TOKEN_KEY, tokenData.accessToken);
    }
    if (tokenData.refreshToken) {
      localStorage.setItem(REFRESH_TOKEN_KEY, tokenData.refreshToken);
    }
    if (tokenData.expiresAt) {
      localStorage.setItem(EXPIRES_AT_KEY, tokenData.expiresAt);
    }
    if (tokenData.refreshTokenExpiresAt) {
      localStorage.setItem(REFRESH_TOKEN_EXPIRES_AT_KEY, tokenData.refreshTokenExpiresAt);
    }
  },

  getTokenExpiration: () => {
    return localStorage.getItem(EXPIRES_AT_KEY);
  },

  getRefreshTokenExpiration: () => {
    return localStorage.getItem(REFRESH_TOKEN_EXPIRES_AT_KEY);
  },

  isTokenExpired: () => {
    const expiration = localStorage.getItem(EXPIRES_AT_KEY);
    if (!expiration) return true;
    return new Date(expiration) <= new Date();
  },

  isRefreshTokenExpired: () => {
    const expiration = localStorage.getItem(REFRESH_TOKEN_EXPIRES_AT_KEY);
    if (!expiration) return true;
    return new Date(expiration) <= new Date();
  },

  forgotPassword: async (email) => {
    try {
      const response = await post(API_ENDPOINTS.AUTH.FORGOT_PASSWORD, { email });
      return response.data;
    } catch (error) {
      if (error.response?.data) {
        throw error.response.data;
      }
      throw error;
    }
  },

  resetPassword: async (data) => {
    try {
      const response = await post(API_ENDPOINTS.AUTH.RESET_PASSWORD, data);
      return response.data;
    } catch (error) {
      if (error.response?.data) {
        throw error.response.data;
      }
      throw error;
    }
  },

  changePassword: async (data) => {
    try {
      const response = await post(API_ENDPOINTS.AUTH.CHANGE_PASSWORD, data);
      return response.data;
    } catch (error) {
      if (error.response?.data) {
        throw error.response.data;
      }
      throw error;
    }
  },

  updateProfile: async (data) => {
    try {
      const response = await put(API_ENDPOINTS.AUTH.UPDATE_PROFILE, data);
      return response.data;
    } catch (error) {
      if (error.response?.data) {
        throw error.response.data;
      }
      throw error;
    }
  }
};

export { authService }; 