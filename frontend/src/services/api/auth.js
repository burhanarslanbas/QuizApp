const API_URL = 'https://localhost:7001/api'; // Backend API URL'inizi buraya yazın

// Token yapısını saklamak için yardımcı fonksiyonlar
const TOKEN_KEY = 'auth_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const TOKEN_EXPIRATION_KEY = 'token_expiration';
const REFRESH_TOKEN_EXPIRATION_KEY = 'refresh_token_expiration';

const saveTokens = (tokenData) => {
  localStorage.setItem(TOKEN_KEY, tokenData.token.accessToken);
  localStorage.setItem(REFRESH_TOKEN_KEY, tokenData.token.refreshToken);
  localStorage.setItem(TOKEN_EXPIRATION_KEY, tokenData.token.expiration);
  localStorage.setItem(REFRESH_TOKEN_EXPIRATION_KEY, tokenData.token.refreshTokenExpiration);
};

const clearTokens = () => {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(REFRESH_TOKEN_KEY);
  localStorage.removeItem(TOKEN_EXPIRATION_KEY);
  localStorage.removeItem(REFRESH_TOKEN_EXPIRATION_KEY);
};

const isTokenExpired = (expirationDate) => {
  return new Date(expirationDate) <= new Date();
};

export const authService = {
  async login(userNameOrEmail, password) {
    try {
      const response = await fetch(`${API_URL}/auth/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userNameOrEmail, password }),
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(response.status.toString());
      }

      if (!data.success || !data.token) {
        throw new Error('Geçersiz yanıt: Token bulunamadı');
      }

      // Token bilgilerini kaydet
      saveTokens(data);
      return data;
    } catch (error) {
      if (error.message === '401') {
        throw new Error('401');
      } else if (error.message === '404') {
        throw new Error('404');
      }
      throw error;
    }
  },

  async register(userData) {
    try {
      const response = await fetch(`${API_URL}/auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || 'Kayıt olurken bir hata oluştu');
      }

      return data;
    } catch (error) {
      throw error;
    }
  },

  logout() {
    clearTokens();
  },

  getToken() {
    const token = localStorage.getItem(TOKEN_KEY);
    const expiration = localStorage.getItem(TOKEN_EXPIRATION_KEY);
    
    if (!token || !expiration) {
      return null;
    }

    // Token süresi dolmuşsa null döndür
    if (isTokenExpired(expiration)) {
      this.logout();
      return null;
    }

    return token;
  },

  getRefreshToken() {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
    const expiration = localStorage.getItem(REFRESH_TOKEN_EXPIRATION_KEY);
    
    if (!refreshToken || !expiration) {
      return null;
    }

    // Refresh token süresi dolmuşsa null döndür
    if (isTokenExpired(expiration)) {
      this.logout();
      return null;
    }

    return refreshToken;
  },

  isAuthenticated() {
    return !!this.getToken();
  },

  // Token'ın ne zaman dolacağını kontrol et
  getTokenExpirationTime() {
    const expiration = localStorage.getItem(TOKEN_EXPIRATION_KEY);
    if (!expiration) return null;
    return new Date(expiration);
  }
}; 