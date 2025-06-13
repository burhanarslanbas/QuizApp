import { ERROR_MESSAGES } from '../constants/errorMessages';

export const handleApiError = (error) => {
  // Axios hata nesnesini kontrol et
  if (error.response) {
    const status = error.response.status;
    const data = error.response.data;

    // API'den gelen hata detayları varsa onları döndür
    if (data && typeof data === 'object') {
      if (Array.isArray(data.errors) && data.errors.length > 0) {
        return data.errors.join(' ');
      }
      if (data.message) {
        return data.message;
      }
    }
    if (typeof data === 'string') {
      return data;
    }

    // HTTP durum kodlarına göre hata mesajları
    switch (status) {
      case 400:
        return ERROR_MESSAGES.VALIDATION.REQUIRED_FIELD;
      case 401:
        return ERROR_MESSAGES.AUTH.INVALID_CREDENTIALS;
      case 403:
        return ERROR_MESSAGES.AUTH.UNAUTHORIZED;
      case 404:
        return ERROR_MESSAGES.AUTH.USER_NOT_FOUND;
      case 409:
        return ERROR_MESSAGES.REGISTER.USERNAME_EXISTS;
      case 500:
        return ERROR_MESSAGES.SERVER_ERROR;
      default:
        return ERROR_MESSAGES.UNKNOWN_ERROR;
    }
  }

  // Ağ hatası
  if (error.message === 'Network Error') {
    return ERROR_MESSAGES.NETWORK_ERROR;
  }

  // İstek iptal edildi
  if (error.code === 'ECONNABORTED') {
    return ERROR_MESSAGES.NETWORK_ERROR;
  }

  // Diğer hatalar
  return ERROR_MESSAGES.UNKNOWN_ERROR;
}; 