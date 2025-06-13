import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../services/authService';
import { TOKEN_CONFIG } from '../config';

// Standard keys
const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const EXPIRES_AT_KEY = 'expires_at';
const REFRESH_TOKEN_EXPIRES_AT_KEY = 'refresh_token_expires_at';
const USER_DATA_KEY = 'user_data';

const TokenContext = createContext();

export const TokenProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [checking, setChecking] = useState(true);
  const [tokenExpiresAt, setTokenExpiresAt] = useState(null);
  const [refreshTokenExpiresAt, setRefreshTokenExpiresAt] = useState(null);
  const [user, setUser] = useState(null);

  // Kullanıcının rollerini işle
  const processUserRoles = (userData) => {
    if (!userData) return null;

    // Eğer roles bir string ise, virgülle ayrılmış rolleri diziye çevir
    if (typeof userData.roles === 'string') {
      userData.roles = userData.roles.split(',').map(role => role.trim());
    }
    // Eğer roles bir dizi değilse, boş dizi olarak ayarla
    else if (!Array.isArray(userData.roles)) {
      userData.roles = [];
    }

    // En yüksek öncelikli rolü belirle (admin > teacher > student)
    const rolePriority = {
      'admin': 3,
      'teacher': 2,
      'student': 1
    };

    userData.primaryRole = userData.roles.reduce((highest, current) => {
      const currentPriority = rolePriority[current.toLowerCase()] || 0;
      const highestPriority = rolePriority[highest.toLowerCase()] || 0;
      return currentPriority > highestPriority ? current : highest;
    }, 'student');

    return userData;
  };

  const register = async (userData) => {
    try {
      const response = await authService.register(userData);
      return response;
    } catch (error) {
      throw error;
    }
  };

  const checkAuth = async () => {
    setChecking(true);
    try {
      const token = localStorage.getItem(ACCESS_TOKEN_KEY);
      const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY);
      const userData = localStorage.getItem(USER_DATA_KEY);
      const expiresAt = localStorage.getItem(EXPIRES_AT_KEY);
      const refreshExpiresAt = localStorage.getItem(REFRESH_TOKEN_EXPIRES_AT_KEY);

      setTokenExpiresAt(expiresAt);
      setRefreshTokenExpiresAt(refreshExpiresAt);
      
      if (userData) {
        const parsedUserData = JSON.parse(userData);
        setUser(processUserRoles(parsedUserData));
      } else {
        setUser(null);
      }

      if (!token || !refreshToken || !userData || !expiresAt || !refreshExpiresAt) {
        setIsAuthenticated(false);
        setIsLoading(false);
        setChecking(false);
        return;
      }

      const now = new Date().getTime();
      const tokenExp = new Date(expiresAt).getTime();
      const refreshTokenExp = new Date(refreshExpiresAt).getTime();

      if (now >= refreshTokenExp) {
        setIsAuthenticated(false);
        setIsLoading(false);
        setChecking(false);
        return;
      }

      if (now >= tokenExp) {
        try {
          setIsLoading(true);
          const response = await authService.refreshToken();
          
          if (response && response.token) {
            const newToken = response.token.accessToken;
            const newRefreshToken = response.token.refreshToken;
            const newExpiresAt = response.token.expiration;
            const newRefreshExpiresAt = response.token.refreshTokenExpiration;
            
            localStorage.setItem(ACCESS_TOKEN_KEY, newToken);
            localStorage.setItem(REFRESH_TOKEN_KEY, newRefreshToken);
            localStorage.setItem(EXPIRES_AT_KEY, newExpiresAt);
            localStorage.setItem(REFRESH_TOKEN_EXPIRES_AT_KEY, newRefreshExpiresAt);
            
            setTokenExpiresAt(newExpiresAt);
            setRefreshTokenExpiresAt(newRefreshExpiresAt);
            
            if (response.user) {
              const processedUser = processUserRoles(response.user);
              localStorage.setItem(USER_DATA_KEY, JSON.stringify(processedUser));
              setUser(processedUser);
            }
            
            setIsAuthenticated(true);
          } else {
            throw new Error('Invalid token response');
          }
        } catch (error) {
          console.error('Token refresh error:', error);
          setIsAuthenticated(false);
          setUser(null);
          authService.logout();
        } finally {
          setIsLoading(false);
        }
      } else {
        setIsAuthenticated(true);
      }
    } catch (error) {
      console.error('Auth check error:', error);
      setIsAuthenticated(false);
      setUser(null);
    } finally {
      setChecking(false);
      setIsLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  useEffect(() => {
    let interval;
    if (isAuthenticated) {
      interval = setInterval(checkAuth, TOKEN_CONFIG.REFRESH_INTERVAL);
    }
    return () => {
      if (interval) clearInterval(interval);
    };
  }, [isAuthenticated]);

  const login = async (data) => {
    try {
      setIsLoading(true);
      console.log('TokenContext login attempt:', data);
      const response = await authService.login(data);
      console.log('TokenContext login response:', response);
      
      if (response?.success && response?.token) {
        const token = response.token;
        
        localStorage.setItem(ACCESS_TOKEN_KEY, token.accessToken);
        localStorage.setItem(REFRESH_TOKEN_KEY, token.refreshToken);
        localStorage.setItem(EXPIRES_AT_KEY, token.expiration);
        localStorage.setItem(REFRESH_TOKEN_EXPIRES_AT_KEY, token.refreshTokenExpiration);
        
        if (response.user) {
          const processedUser = processUserRoles(response.user);
          localStorage.setItem(USER_DATA_KEY, JSON.stringify(processedUser));
          setUser(processedUser);
        }
        
        setTokenExpiresAt(token.expiration);
        setRefreshTokenExpiresAt(token.refreshTokenExpiration);
        setIsAuthenticated(true);
        
        return response;
      }
      throw new Error(response?.message || 'Giriş başarısız');
    } catch (error) {
      console.error('TokenContext login error:', error);
      setIsAuthenticated(false);
      setUser(null);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(EXPIRES_AT_KEY);
    localStorage.removeItem(REFRESH_TOKEN_EXPIRES_AT_KEY);
    localStorage.removeItem(USER_DATA_KEY);
    
    setIsAuthenticated(false);
    setUser(null);
    setTokenExpiresAt(null);
    setRefreshTokenExpiresAt(null);
  };

  // Access token'ın süresi bitmeye ne kadar kaldığını döner (saniye cinsinden)
  const getTokenRemainingTime = () => {
    if (!tokenExpiresAt) return 0;
    const expTime = new Date(tokenExpiresAt).getTime();
    const now = Date.now();
    return Math.floor((expTime - now) / 1000); // saniye
  };

  useEffect(() => {
    let interval;
    if (isAuthenticated) {
      // Her 30 saniyede bir access token süresini kontrol et
      interval = setInterval(async () => {
        const remaining = getTokenRemainingTime();
        if (remaining > 0 && remaining < 60) {
          try {
            const response = await authService.refreshToken();
            if (response && response.token) {
              setTokenExpiresAt(response.token.expiration);
              setRefreshTokenExpiresAt(response.token.refreshTokenExpiration);
              if (response.user) {
                const processedUser = processUserRoles(response.user);
                setUser(processedUser);
              }
            }
          } catch (err) {
            console.error('Access token otomatik yenileme hatası:', err);
            logout();
          }
        }
      }, 30000);
    }
    return () => {
      if (interval) clearInterval(interval);
    };
  }, [isAuthenticated]);

  const value = {
    isAuthenticated,
    isLoading,
    checking,
    user,
    login,
    logout,
    register,
    checkAuth
  };

  return (
    <TokenContext.Provider value={value}>
      {children}
    </TokenContext.Provider>
  );
};

export const useToken = () => {
  const context = useContext(TokenContext);
  if (!context) {
    throw new Error('useToken must be used within a TokenProvider');
  }
  return context;
}; 