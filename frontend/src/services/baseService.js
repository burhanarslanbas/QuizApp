import axios from 'axios';
import { authService } from './authService';
import { API_URL } from '../config';

const createAxiosInstance = () => {
  const instance = axios.create({
    baseURL: API_URL,
    headers: {
      'Content-Type': 'application/json'
    },
    timeout: 10000 // 10 saniye timeout
  });

  instance.interceptors.request.use(
    (config) => {
      console.log('Request interceptor:', {
        url: config.url,
        method: config.method,
        headers: config.headers,
        data: config.data
      });

      // Add authorization token if not a refresh token request
      if (!config.url.includes('refresh-token')) {
        const token = authService.getToken();
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
      }

      return config;
    },
    (error) => {
      console.error('Request interceptor error:', error);
      return Promise.reject(error);
    }
  );

  instance.interceptors.response.use(
    (response) => {
      console.log('Response interceptor:', {
        url: response.config.url,
        status: response.status,
        data: response.data
      });
      return response;
    },
    async (error) => {
      console.error('Response interceptor error:', {
        url: error.config?.url,
        status: error.response?.status,
        data: error.response?.data,
        message: error.message
      });

      const originalRequest = error.config;

      // If error is 401 and not a refresh token request
      if (error.response?.status === 401 && !originalRequest._retry && !originalRequest.url.includes('refresh-token')) {
        originalRequest._retry = true;

        try {
          console.log('Token yenileme deneniyor...');
          const response = await authService.refreshToken();
          console.log('Token yenileme başarılı:', response);

          // Update authorization header with new token
          const newToken = authService.getToken();
          if (newToken) {
            originalRequest.headers.Authorization = `Bearer ${newToken}`;
          }

          // Retry the original request
          return axios(originalRequest);
        } catch (refreshError) {
          console.error('Token yenileme başarısız:', refreshError);
          // Redirect to login page
          window.location.href = '/login';
          return Promise.reject(refreshError);
        }
      }

      return Promise.reject(error);
    }
  );

  return instance;
};

const api = createAxiosInstance();

export const handleResponse = (response) => {
  if (response && response.data) {
    return response.data;
  }
  return response;
};

export const handleError = (error) => {
  if (!error) {
    return {
      message: 'An unknown error occurred',
      errors: ['An unknown error occurred']
    };
  }

  if (error.response) {
    const errorData = error.response.data;
    let errorMessage = 'An error occurred';
    
    if (errorData) {
      if (typeof errorData === 'string') {
        errorMessage = errorData;
      } else if (errorData.message) {
        errorMessage = errorData.message;
      } else if (errorData.error) {
        errorMessage = errorData.error;
      } else if (Array.isArray(errorData)) {
        errorMessage = errorData.join('\n');
      }
    }

    if (error.response.status === 400 && errorData.errors) {
      const validationErrors = [];
      for (const key in errorData.errors) {
        if (Array.isArray(errorData.errors[key])) {
          validationErrors.push(...errorData.errors[key]);
        }
      }
      if (validationErrors.length > 0) {
        return { message: errorMessage, errors: validationErrors };
      }
    }

    switch (error.response.status) {
      case 401:
        // 401 hatası durumunda kullanıcıyı logout yap
        authService.logout();
        window.location.href = '/login';
        return { message: 'Unauthorized access', errors: ['Please log in to continue'] };
      case 403:
        return { message: 'Access forbidden', errors: ['You do not have permission to perform this action'] };
      case 404:
        return { message: 'Resource not found', errors: ['The requested resource could not be found'] };
      case 500:
        return { message: 'Server error', errors: ['An internal server error occurred'] };
      default:
        return { message: errorMessage, errors: [errorMessage] };
    }
  }

  if (error.request) {
    return { message: 'Network error', errors: ['Unable to connect to the server'] };
  }

  return { message: error.message || 'An error occurred', errors: [error.message || 'An error occurred'] };
};

export const get = async (url, params = {}) => {
  try {
    const response = await api.get(url, { params });
    return handleResponse(response);
  } catch (error) {
    console.error('GET request error:', error);
    throw handleError(error);
  }
};

export const post = async (url, data = {}) => {
  try {
    console.log('POST request:', { url, data });
    const response = await api.post(url, data);
    console.log('POST response:', response);
    return handleResponse(response);
  } catch (error) {
    console.error('POST request error:', error);
    throw handleError(error);
  }
};

export const put = async (url, data = {}) => {
  try {
    const response = await api.put(url, data);
    return handleResponse(response);
  } catch (error) {
    console.error('PUT request error:', error);
    throw handleError(error);
  }
};

export const del = async (url) => {
  try {
    const response = await api.delete(url);
    return handleResponse(response);
  } catch (error) {
    console.error('DELETE request error:', error);
    throw handleError(error);
  }
};

export default api;