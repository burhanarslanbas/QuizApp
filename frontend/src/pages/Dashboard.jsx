import React from 'react';
import { 
  Box, 
  Container, 
  Typography, 
  Button, 
  Paper,
  Grid
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/api/auth';

const Dashboard = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };

  return (
    <Box
      sx={{
        minHeight: '100vh',
        width: '100vw',
        background: 'linear-gradient(135deg, #64b5f6 0%, #81c784 100%)',
        py: 4,
        px: 2
      }}
    >
      <Container maxWidth="lg">
        {/* Header */}
        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          mb: 4
        }}>
          <Typography variant="h4" sx={{ color: 'white', fontWeight: 'bold' }}>
            QuizApp Dashboard
          </Typography>
          <Button 
            variant="contained" 
            onClick={handleLogout}
            sx={{ 
              backgroundColor: 'white',
              color: '#64b5f6',
              '&:hover': {
                backgroundColor: 'rgba(255, 255, 255, 0.9)'
              }
            }}
          >
            Çıkış Yap
          </Button>
        </Box>

        {/* Content */}
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Paper
              sx={{
                p: 3,
                height: '100%',
                backgroundColor: 'rgba(255, 255, 255, 0.95)',
                borderRadius: 2
              }}
            >
              <Typography variant="h5" sx={{ mb: 2, color: '#64b5f6' }}>
                Hoş Geldiniz!
              </Typography>
              <Typography variant="body1">
                QuizApp'e başarıyla giriş yaptınız. Buradan quizleri görüntüleyebilir ve çözebilirsiniz.
              </Typography>
            </Paper>
          </Grid>
          <Grid item xs={12} md={6}>
            <Paper
              sx={{
                p: 3,
                height: '100%',
                backgroundColor: 'rgba(255, 255, 255, 0.95)',
                borderRadius: 2
              }}
            >
              <Typography variant="h5" sx={{ mb: 2, color: '#64b5f6' }}>
                Hızlı Başlangıç
              </Typography>
              <Typography variant="body1">
                Henüz bir quiz çözmediniz. Hemen başlamak için kategorilerden birini seçin.
              </Typography>
            </Paper>
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
};

export default Dashboard; 