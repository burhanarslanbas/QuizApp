import React, { useState, useEffect } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Link,
  Alert,
  Checkbox,
  FormControlLabel,
  InputAdornment,
  IconButton,
  Avatar,
  Fade
} from '@mui/material';
import { useToken } from '../../context/TokenContext';
import { useNotification } from '../../context/NotificationContext';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import { REMEMBER_CONFIG } from '../../config';

const Login = () => {
  const navigate = useNavigate();
  const { login, isAuthenticated, isLoading } = useToken();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [formData, setFormData] = useState({
    email: localStorage.getItem(REMEMBER_CONFIG.EMAIL_KEY) || '',
    password: '',
  });
  const [rememberMe, setRememberMe] = useState(localStorage.getItem(REMEMBER_CONFIG.REMEMBER_KEY) === 'true');
  const [emailError, setEmailError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  useEffect(() => {
    if (isAuthenticated && !isLoading) {
      navigate('/dashboard', { replace: true });
    }
  }, [isAuthenticated, isLoading, navigate]);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
    if (e.target.name === 'email') setEmailError('');
    if (e.target.name === 'password') setPasswordError('');
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setEmailError('');
    setPasswordError('');
    setLoading(true);

    try {
      if (!formData.email) {
        setEmailError('E-posta adresi gereklidir');
        return;
      }
      if (!formData.password) {
        setPasswordError('Şifre gereklidir');
        return;
      }

      if (rememberMe) {
        localStorage.setItem(REMEMBER_CONFIG.EMAIL_KEY, formData.email);
        localStorage.setItem(REMEMBER_CONFIG.REMEMBER_KEY, 'true');
      } else {
        localStorage.removeItem(REMEMBER_CONFIG.EMAIL_KEY);
        localStorage.removeItem(REMEMBER_CONFIG.REMEMBER_KEY);
      }

      console.log('Login attempt with:', { email: formData.email });
      
      const response = await login({ 
        email: formData.email, 
        password: formData.password
      });
      
      console.log('Login response:', response);
      
    } catch (err) {
      console.error('Login error:', err);
      let errorMsg = 'Giriş yapılırken bir hata oluştu';
      
      if (err.message) {
        errorMsg = err.message;
      }
      
      if (err.errors) {
        if (err.errors.email) {
          setEmailError(err.errors.email[0]);
        }
        if (err.errors.password) {
          setPasswordError(err.errors.password[0]);
        }
        if (err.errors.general) {
          errorMsg = err.errors.general[0];
        }
      }
      
      setError(errorMsg);
      showNotification(errorMsg, 'error');
    } finally {
      setLoading(false);
    }
  };

  const handleClickShowPassword = () => setShowPassword((show) => !show);
  const handleMouseDownPassword = (event) => event.preventDefault();

  if (isLoading) {
    return null;
  }

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
              <LockOutlinedIcon fontSize="large" />
            </Avatar>
            <Typography variant="h4" component="h1" align="center" gutterBottom fontWeight={700}>
              Giriş Yap
            </Typography>
            <Typography variant="body2" color="text.secondary" align="center" sx={{ mb: 3 }}>
              Quiz uygulamasına hoş geldiniz
            </Typography>

            {error && (
              <Alert severity="error" sx={{ mb: 2, width: '100%' }}>
                {error}
              </Alert>
            )}

            <form onSubmit={handleSubmit} style={{ width: '100%' }} autoComplete="on">
              <TextField
                fullWidth
                label="E-posta"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                margin="normal"
                required
                autoFocus
                error={!!emailError}
                helperText={emailError}
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
                error={!!passwordError}
                helperText={passwordError}
                size="medium"
                variant="outlined"
                autoComplete="current-password"
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="Şifreyi göster/gizle"
                        onClick={handleClickShowPassword}
                        onMouseDown={handleMouseDownPassword}
                        edge="end"
                        size="large"
                      >
                        {showPassword ? <VisibilityOff /> : <Visibility />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
              <FormControlLabel
                control={<Checkbox checked={rememberMe} onChange={e => setRememberMe(e.target.checked)} color="primary" />}
                label="Beni Hatırla"
                sx={{ mt: 1, mb: 1 }}
              />
              <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                disabled={loading}
                sx={{ mt: 2, mb: 2, fontWeight: 600, borderRadius: 2, boxShadow: 2 }}
              >
                {loading ? 'Giriş yapılıyor...' : 'Giriş Yap'}
              </Button>
            </form>

            <Box sx={{ textAlign: 'center', mt: 2 }}>
              <Typography variant="body2">
                Hesabınız yok mu?{' '}
                <Link component={RouterLink} to="/register">
                  Kayıt Ol
                </Link>
              </Typography>
            </Box>
          </CardContent>
        </Card>
      </Fade>
    </Box>
  );
};

export default Login; 