import React from 'react';
import { Box, Container, Typography, Button, Grid, Card, CardContent } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const features = [
  {
    title: 'Çeşitli Kategoriler',
    description: 'Farklı konularda hazırlanmış quizler ile bilginizi test edin.',
    icon: '📚'
  },
  {
    title: 'Anlık Geri Bildirim',
    description: 'Cevaplarınızı anında öğrenin ve doğru cevapları görün.',
    icon: '⚡'
  },
  {
    title: 'İlerleme Takibi',
    description: 'Quiz sonuçlarınızı takip edin ve gelişiminizi görün.',
    icon: '📊'
  },
  {
    title: 'Rekabet',
    description: 'Diğer kullanıcılarla yarışın ve sıralamada yerinizi alın.',
    icon: '🏆'
  }
];

const Home = () => {
  const navigate = useNavigate();

  return (
    <Box sx={{ 
      minHeight: '100vh',
      width: '100vw',
      background: 'linear-gradient(135deg, #64b5f6 0%, #81c784 100%)',
      py: { xs: 4, md: 8 },
      px: { xs: 2, md: 4 },
      overflow: 'hidden'
    }}>
      <Container 
        maxWidth="xl" 
        sx={{ 
          height: '100%',
          display: 'flex',
          flexDirection: 'column'
        }}
      >
        {/* Header */}
        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          mb: { xs: 4, md: 8 },
          flexWrap: 'wrap',
          gap: 2
        }}>
          <Typography 
            variant="h4" 
            sx={{ 
              color: 'white', 
              fontWeight: 'bold',
              fontSize: { xs: '1.5rem', md: '2rem' }
            }}
          >
            QuizApp
          </Typography>
          <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
            <Button 
              variant="outlined" 
              sx={{ 
                color: 'white', 
                borderColor: 'white',
                '&:hover': {
                  borderColor: 'white',
                  backgroundColor: 'rgba(255, 255, 255, 0.1)'
                }
              }}
              onClick={() => navigate('/login')}
            >
              Giriş Yap
            </Button>
            <Button 
              variant="contained" 
              sx={{ 
                backgroundColor: 'white',
                color: '#64b5f6',
                '&:hover': {
                  backgroundColor: 'rgba(255, 255, 255, 0.9)'
                }
              }}
              onClick={() => navigate('/register')}
            >
              Kayıt Ol
            </Button>
          </Box>
        </Box>

        {/* Welcome Section */}
        <Box sx={{ 
          textAlign: 'center', 
          mb: { xs: 6, md: 8 },
          flex: 1,
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center'
        }}>
          <Typography 
            variant="h1" 
            sx={{ 
              color: 'white',
              mb: 2,
              fontSize: { xs: '2rem', sm: '2.5rem', md: '3.5rem' },
              lineHeight: 1.2
            }}
          >
            QuizApp'e Hoş Geldiniz!
          </Typography>
          <Typography 
            variant="h5" 
            sx={{ 
              color: 'rgba(255, 255, 255, 0.9)',
              maxWidth: '800px',
              mx: 'auto',
              fontSize: { xs: '1.1rem', md: '1.5rem' }
            }}
          >
            Bilginizi test edin, yeni şeyler öğrenin ve diğer kullanıcılarla yarışın.
          </Typography>
        </Box>

        {/* Features Grid */}
        <Grid 
          container 
          spacing={{ xs: 2, md: 4 }}
          sx={{ 
            flex: 1,
            alignItems: 'stretch'
          }}
        >
          {features.map((feature, index) => (
            <Grid item xs={12} sm={6} md={3} key={index}>
              <Card sx={{ 
                height: '100%',
                backgroundColor: 'rgba(255, 255, 255, 0.95)',
                transition: 'all 0.3s ease',
                '&:hover': {
                  transform: 'translateY(-5px)',
                  boxShadow: '0 8px 16px rgba(0, 0, 0, 0.1)'
                }
              }}>
                <CardContent sx={{ 
                  textAlign: 'center',
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'center',
                  p: { xs: 2, md: 3 }
                }}>
                  <Typography variant="h1" sx={{ fontSize: { xs: '2.5rem', md: '3rem' }, mb: 2 }}>
                    {feature.icon}
                  </Typography>
                  <Typography 
                    variant="h6" 
                    sx={{ 
                      mb: 1, 
                      color: '#64b5f6',
                      fontSize: { xs: '1.1rem', md: '1.25rem' }
                    }}
                  >
                    {feature.title}
                  </Typography>
                  <Typography 
                    variant="body2" 
                    color="text.secondary"
                    sx={{ fontSize: { xs: '0.875rem', md: '1rem' } }}
                  >
                    {feature.description}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    </Box>
  );
};

export default Home; 