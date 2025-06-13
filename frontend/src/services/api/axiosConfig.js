import axios from 'axios';
import { API_URL } from '../../config';
import { authService } from '../authService';

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json'
    },
    timeout: 10000, // 10 saniye timeout
    withCredentials: true // CORS için gerekli
});

// Request interceptor
api.interceptors.request.use(
    (config) => {
        console.log('Request config:', {
            url: config.url,
            method: config.method,
            headers: config.headers
        });

        const accessToken = localStorage.getItem('access_token');
        if (accessToken) {
            config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
    },
    (error) => {
        console.error('Request error:', error);
        return Promise.reject(error);
    }
);

// Response interceptor
api.interceptors.response.use(
    (response) => {
        console.log('Response:', {
            status: response.status,
            data: response.data
        });
        return response;
    },
    async (error) => {
        console.error('Response error:', {
            status: error.response?.status,
            data: error.response?.data,
            message: error.message
        });

        // Network hatası kontrolü
        if (!error.response) {
            return Promise.reject({
                message: 'Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edin.',
                isNetworkError: true
            });
        }

        // Token yenileme işlemi
        if (error.response?.status === 401 && !error.config._retry) {
            error.config._retry = true;

            try {
                const accessToken = localStorage.getItem('access_token');
                const refreshToken = localStorage.getItem('refresh_token');

                if (!accessToken || !refreshToken) {
                    throw new Error('No tokens available');
                }

                const response = await authService.refreshToken();
                
                if (response.success) {
                    const { accessToken: newAccessToken } = response.token;
                    localStorage.setItem('access_token', newAccessToken);
                    
                    // Yeni token ile orijinal isteği tekrarla
                    error.config.headers.Authorization = `Bearer ${newAccessToken}`;
                    return api(error.config);
                }
            } catch (refreshError) {
                console.error('Token refresh error:', refreshError);
                // Token yenileme başarısız olduğunda kullanıcıyı logout yap
                await authService.logout();
                window.location.href = '/login';
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

export default api; 