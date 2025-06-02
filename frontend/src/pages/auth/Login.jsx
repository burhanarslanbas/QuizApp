import React, { useState } from 'react';
import { 
  Box, 
  Container, 
  Typography, 
  TextField, 
  Button, 
  Paper,
  Link,
  Alert
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../services/api/auth';

const Login = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    userNameOrEmail: '',
    password: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
    // Her değişiklikte hata mesajını temizle
    setError('');
  };

  const validateForm = () => {
    if (!formData.userNameOrEmail.trim()) {
      setError('Kullanıcı adı veya e-posta adresi gereklidir');
      return false;
    }
    if (!formData.password.trim()) {
      setError('Şifre gereklidir');
      return false;
    }
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    
    if (!validateForm()) {
      return;
    }

    setLoading(true);
    try {
      const response = await authService.login(formData.userNameOrEmail, formData.password);
      if (response && response.success && response.token) {
        navigate('/dashboard');
      } else {
        setError('Giriş başarısız. Lütfen bilgilerinizi kontrol edin.');
      }
    } catch (err) {
      if (err.message.includes('401')) {
        setError('Kullanıcı adı veya şifre hatalı');
      } else if (err.message.includes('404')) {
        setError('Kullanıcı bulunamadı');
      } else {
        setError(err.message || 'Giriş yapılırken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box
      sx={{
        minHeight: '100vh',
        width: '100vw',
        background: 'linear-gradient(135deg, #64b5f6 0%, #81c784 100%)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        py: 4,
        px: 2
      }}
    >
      <Container maxWidth="sm">
        <Paper
          elevation={3}
          sx={{
            p: 4,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            backgroundColor: 'rgba(255, 255, 255, 0.95)',
            borderRadius: 2
          }}
        >
          <Typography
            component="h1"
            variant="h4"
            sx={{
              mb: 3,
              color: '#64b5f6',
              fontWeight: 'bold'
            }}
          >
            Giriş Yap
          </Typography>

          {error && (
            <Alert severity="error" sx={{ width: '100%', mb: 2 }}>
              {error}
            </Alert>
          )}

          <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="userNameOrEmail"
              label="Kullanıcı Adı veya E-posta"
              name="userNameOrEmail"
              autoComplete="username"
              autoFocus
              value={formData.userNameOrEmail}
              onChange={handleChange}
              disabled={loading}
              error={!!error && error.includes('Kullanıcı adı')}
              sx={{ mb: 2 }}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Şifre"
              type="password"
              id="password"
              autoComplete="current-password"
              value={formData.password}
              onChange={handleChange}
              disabled={loading}
              error={!!error && error.includes('Şifre')}
              sx={{ mb: 3 }}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              disabled={loading}
              sx={{
                mt: 1,
                mb: 2,
                py: 1.5,
                backgroundColor: '#64b5f6',
                '&:hover': {
                  backgroundColor: '#42a5f5'
                }
              }}
            >
              {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
            </Button>
            <Box sx={{ textAlign: 'center' }}>
              <Link
                component="button"
                variant="body2"
                onClick={() => navigate('/register')}
                sx={{ color: '#64b5f6' }}
                disabled={loading}
              >
                Hesabınız yok mu? Kayıt olun
              </Link>
            </Box>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default Login; 