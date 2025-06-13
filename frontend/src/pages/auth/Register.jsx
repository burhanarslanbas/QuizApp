import React, { useState } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import { useToken } from '../../context/TokenContext';
import { useNotification } from '../../context/NotificationContext';
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Alert,
  InputAdornment,
  IconButton,
  Avatar,
  Fade,
  Link
} from '@mui/material';
import PersonAddOutlinedIcon from '@mui/icons-material/PersonAddOutlined';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';

const Register = () => {
  const navigate = useNavigate();
  const { register } = useToken();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    userName: ''
  });
  const [errors, setErrors] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    userName: ''
  });
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    // Hata mesajını temizle
    setErrors(prev => ({
      ...prev,
      [name]: ''
    }));
    setError('');
  };

  const validateForm = () => {
    const newErrors = {
      fullName: '',
      email: '',
      password: '',
      confirmPassword: '',
      userName: ''
    };
    let isValid = true;

    if (!formData.fullName.trim()) {
      newErrors.fullName = 'Ad Soyad gereklidir';
      isValid = false;
    }

    if (!formData.email.trim()) {
      newErrors.email = 'E-posta adresi gereklidir';
      isValid = false;
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Geçerli bir e-posta adresi giriniz';
      isValid = false;
    }

    if (!formData.userName.trim()) {
      newErrors.userName = 'Kullanıcı adı gereklidir';
      isValid = false;
    }

    if (!formData.password) {
      newErrors.password = 'Şifre gereklidir';
      isValid = false;
    } else if (formData.password.length < 6) {
      newErrors.password = 'Şifre en az 6 karakter olmalıdır';
      isValid = false;
    }

    if (!formData.confirmPassword) {
      newErrors.confirmPassword = 'Şifre tekrarı gereklidir';
      isValid = false;
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Şifreler eşleşmiyor';
      isValid = false;
    }

    setErrors(newErrors);
    return isValid;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    
    if (!validateForm()) {
      return;
    }

    setLoading(true);
    try {
      const response = await register({
        fullName: formData.fullName,
        email: formData.email,
        password: formData.password,
        confirmPassword: formData.confirmPassword,
        userName: formData.userName
      });

      showNotification('Kayıt başarılı! Giriş yapabilirsiniz.', 'success');
      navigate('/login');
    } catch (error) {
      console.error('Register error:', error);
      if (error.errors && Array.isArray(error.errors)) {
        setError(error.errors.join('\n'));
      } else {
        setError(error.message || 'Kayıt sırasında bir hata oluştu');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleClickShowPassword = () => setShowPassword((show) => !show);
  const handleClickShowConfirmPassword = () => setShowConfirmPassword((show) => !show);
  const handleMouseDownPassword = (event) => event.preventDefault();

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        bgcolor: 'background.default',
        p: 2,
      }}
    >
      <Fade in timeout={600}>
        <Card sx={{ maxWidth: 400, width: '100%', borderRadius: 4, boxShadow: 6, p: 1 }}>
          <CardContent sx={{ p: 4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <Avatar sx={{ m: 1, bgcolor: 'primary.main', width: 56, height: 56 }}>
              <PersonAddOutlinedIcon fontSize="large" />
            </Avatar>
            <Typography variant="h4" component="h1" align="center" gutterBottom fontWeight={700}>
              Kayıt Ol
            </Typography>
            <Typography variant="body2" color="text.secondary" align="center" sx={{ mb: 3 }}>
              Quiz uygulamasına hoş geldiniz
            </Typography>

            {error && (
              <Alert severity="error" sx={{ mb: 2, width: '100%' }}>
                {error.split('\n').map((line, idx) => (
                  <div key={idx}>{line}</div>
                ))}
              </Alert>
            )}

            <form onSubmit={handleSubmit} style={{ width: '100%' }} autoComplete="on">
              <TextField
                fullWidth
                label="Ad Soyad"
                name="fullName"
                value={formData.fullName}
                onChange={handleChange}
                margin="normal"
                required
                error={!!errors.fullName}
                helperText={errors.fullName}
                size="medium"
                variant="outlined"
                autoComplete="name"
              />
              <TextField
                fullWidth
                label="Kullanıcı Adı"
                name="userName"
                value={formData.userName}
                onChange={handleChange}
                margin="normal"
                required
                error={!!errors.userName}
                helperText={errors.userName}
                size="medium"
                variant="outlined"
                autoComplete="username"
              />
              <TextField
                fullWidth
                label="E-posta"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                margin="normal"
                required
                error={!!errors.email}
                helperText={errors.email}
                size="medium"
                variant="outlined"
                autoComplete="email"
              />
              <TextField
                fullWidth
                label="Şifre"
                name="password"
                type={showPassword ? 'text' : 'password'}
                value={formData.password}
                onChange={handleChange}
                margin="normal"
                required
                error={!!errors.password}
                helperText={errors.password}
                size="medium"
                variant="outlined"
                autoComplete="new-password"
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle password visibility"
                        onClick={handleClickShowPassword}
                        onMouseDown={handleMouseDownPassword}
                        edge="end"
                      >
                        {showPassword ? <VisibilityOff /> : <Visibility />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
              <TextField
                fullWidth
                label="Şifre Tekrarı"
                name="confirmPassword"
                type={showConfirmPassword ? 'text' : 'password'}
                value={formData.confirmPassword}
                onChange={handleChange}
                margin="normal"
                required
                error={!!errors.confirmPassword}
                helperText={errors.confirmPassword}
                size="medium"
                variant="outlined"
                autoComplete="new-password"
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle confirm password visibility"
                        onClick={handleClickShowConfirmPassword}
                        onMouseDown={handleMouseDownPassword}
                        edge="end"
                      >
                        {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
              <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                disabled={loading}
                sx={{
                  mt: 3,
                  mb: 2,
                  py: 1.5,
                  borderRadius: 2,
                  textTransform: 'none',
                  fontSize: '1.1rem'
                }}
              >
                {loading ? 'Kayıt Yapılıyor...' : 'Kayıt Ol'}
              </Button>
              <Box sx={{ textAlign: 'center' }}>
                <Typography variant="body2" color="text.secondary">
                  Zaten hesabınız var mı?{' '}
                  <Link component={RouterLink} to="/login" color="primary" sx={{ textDecoration: 'none' }}>
                    Giriş Yap
                  </Link>
                </Typography>
              </Box>
            </form>
          </CardContent>
        </Card>
      </Fade>
    </Box>
  );
};

export default Register;