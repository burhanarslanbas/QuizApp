import React, { useState } from 'react';
import { 
  Box, 
  Container, 
  Typography, 
  TextField, 
  Button, 
  Paper,
  Link,
  Alert,
  Grid
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../services/api/auth';

const Register = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: '',
    userName: '',
    fullName: '',
    password: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
    setError('');
  };

  const validateForm = () => {
    if (!formData.email.trim()) {
      setError('E-posta adresi gereklidir');
      return false;
    }
    if (!formData.userName.trim()) {
      setError('Kullanıcı adı gereklidir');
      return false;
    }
    if (!formData.fullName.trim()) {
      setError('Ad Soyad gereklidir');
      return false;
    }
    if (!formData.password.trim()) {
      setError('Şifre gereklidir');
      return false;
    }
    if (!formData.confirmPassword.trim()) {
      setError('Şifre tekrarı gereklidir');
      return false;
    }
    if (formData.password !== formData.confirmPassword) {
      setError('Şifreler eşleşmiyor');
      return false;
    }
    if (formData.password.length < 6) {
      setError('Şifre en az 6 karakter olmalıdır');
      return false;
    }
    if (formData.userName.length < 3) {
      setError('Kullanıcı adı en az 3 karakter olmalıdır');
      return false;
    }
    if (formData.fullName.length < 3) {
      setError('Ad Soyad en az 3 karakter olmalıdır');
      return false;
    }
    // E-posta formatı kontrolü
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      setError('Geçerli bir e-posta adresi giriniz');
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
      const response = await authService.register({
        email: formData.email,
        userName: formData.userName,
        fullName: formData.fullName,
        password: formData.password,
        confirmPassword: formData.confirmPassword
      });

      if (response && response.success) {
        // Başarılı kayıt sonrası otomatik giriş yap
        const loginResponse = await authService.login(formData.userName, formData.password);
        if (loginResponse && loginResponse.success && loginResponse.token) {
          navigate('/dashboard');
        } else {
          navigate('/login');
        }
      } else {
        setError('Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol edin.');
      }
    } catch (err) {
      if (err.message.includes('409')) {
        setError('Bu e-posta adresi veya kullanıcı adı zaten kullanılıyor');
      } else {
        setError(err.message || 'Kayıt olurken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
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
            Kayıt Ol
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
              id="email"
              label="E-posta Adresi"
              name="email"
              type="email"
              autoComplete="email"
              autoFocus
              value={formData.email}
              onChange={handleChange}
              disabled={loading}
              error={!!error && error.includes('e-posta')}
              sx={{ mb: 2 }}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              id="userName"
              label="Kullanıcı Adı"
              name="userName"
              autoComplete="username"
              value={formData.userName}
              onChange={handleChange}
              disabled={loading}
              error={!!error && error.includes('Kullanıcı adı')}
              sx={{ mb: 2 }}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              id="fullName"
              label="Ad Soyad"
              name="fullName"
              autoComplete="name"
              value={formData.fullName}
              onChange={handleChange}
              disabled={loading}
              error={!!error && error.includes('Ad Soyad')}
              sx={{ mb: 2 }}
            />
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  required
                  fullWidth
                  name="password"
                  label="Şifre"
                  type="password"
                  id="password"
                  autoComplete="new-password"
                  value={formData.password}
                  onChange={handleChange}
                  disabled={loading}
                  error={!!error && error.includes('Şifre')}
                  helperText="En az 6 karakter"
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  required
                  fullWidth
                  name="confirmPassword"
                  label="Şifre Tekrar"
                  type="password"
                  id="confirmPassword"
                  value={formData.confirmPassword}
                  onChange={handleChange}
                  disabled={loading}
                  error={!!error && error.includes('Şifre')}
                />
              </Grid>
            </Grid>
            <Button
              type="submit"
              fullWidth
              variant="contained"
              disabled={loading}
              sx={{
                mt: 3,
                mb: 2,
                py: 1.5,
                backgroundColor: '#64b5f6',
                '&:hover': {
                  backgroundColor: '#42a5f5'
                }
              }}
            >
              {loading ? 'Kayıt Yapılıyor...' : 'Kayıt Ol'}
            </Button>
            <Box sx={{ textAlign: 'center' }}>
              <Link
                component="button"
                variant="body2"
                onClick={() => navigate('/login')}
                sx={{ color: '#64b5f6' }}
                disabled={loading}
              >
                Zaten hesabınız var mı? Giriş yapın
              </Link>
            </Box>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default Register; 