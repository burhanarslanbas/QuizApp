import React, { useState, useEffect } from 'react';
import { useNavigate, Navigate } from 'react-router-dom';
import {
  Box,
  Button,
  Container,
  Grid,
  Typography,
  Card,
  CardContent,
  useMediaQuery,
  Stack,
  Paper,
  IconButton,
  Avatar,
  Divider,
  Chip,
} from '@mui/material';
import {
  Quiz as QuizIcon,
  School as SchoolIcon,
  Assessment as AssessmentIcon,
  Group as GroupIcon,
  ArrowForward as ArrowForwardIcon,
  Timer as TimerIcon,
  EmojiEvents as TrophyIcon,
  TrendingUp as TrendingUpIcon,
  Security as SecurityIcon,
  Speed as SpeedIcon,
  Psychology as PsychologyIcon,
  AutoGraph as AutoGraphIcon,
} from '@mui/icons-material';
import { motion } from 'framer-motion';
import { useTheme } from '@mui/material/styles';
import { useToken } from '../../context/TokenContext';

const features = [
  {
    icon: <QuizIcon sx={{ fontSize: 40 }} />,
    title: 'Akıllı Sınav Sistemi',
    description: 'Yapay zeka destekli soru önerileri ve kişiselleştirilmiş sınav deneyimi.',
    color: '#2196f3',
  },
  {
    icon: <AssessmentIcon sx={{ fontSize: 40 }} />,
    title: 'Detaylı Analizler',
    description: 'Gerçek zamanlı performans takibi ve detaylı başarı analizleri.',
    color: '#4caf50',
  },
  {
    icon: <PsychologyIcon sx={{ fontSize: 40 }} />,
    title: 'Adaptif Öğrenme',
    description: 'Öğrenme hızınıza göre ayarlanan içerik ve zorluk seviyeleri.',
    color: '#ff9800',
  },
  {
    icon: <SecurityIcon sx={{ fontSize: 40 }} />,
    title: 'Güvenli Platform',
    description: 'SSL şifreleme ve güvenli veri depolama ile güvenli sınav deneyimi.',
    color: '#f44336',
  },
];

const stats = [
  { icon: <TimerIcon />, value: '10K+', label: 'Tamamlanan Sınav' },
  { icon: <TrophyIcon />, value: '95%', label: 'Başarı Oranı' },
  { icon: <GroupIcon />, value: '50K+', label: 'Aktif Kullanıcı' },
  { icon: <TrendingUpIcon />, value: '24/7', label: 'Kesintisiz Erişim' },
];

const testimonials = [
  {
    name: 'Ahmet Yılmaz',
    role: 'Matematik Öğretmeni',
    avatar: '/avatars/teacher1.jpg',
    content: 'Öğrencilerimin performansını takip etmek ve onlara özel sınavlar hazırlamak artık çok kolay.',
  },
  {
    name: 'Ayşe Demir',
    role: 'Öğrenci',
    avatar: '/avatars/student1.jpg',
    content: 'Adaptif öğrenme sistemi sayesinde kendi hızımda ilerleyebiliyorum. Harika bir deneyim!',
  },
];

const Home = (props) => {
  // const { user } = useToken();
  // const userRole = Array.isArray(user?.roles) && user.roles.length > 0 ? user.roles[0].toLowerCase() : undefined;
  // if (!userRole) {
  //   return <Navigate to="/login" replace />;
  // }
  const navigate = useNavigate();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const [animate, setAnimate] = useState(false);

  useEffect(() => {
    setAnimate(true);
  }, []);

  return (
    <Box>
      {/* Hero Section */}
      <Box
        sx={{
          background: 'linear-gradient(135deg, #1a237e 0%, #0d47a1 100%)',
          color: 'white',
          position: 'relative',
          overflow: 'hidden',
          py: { xs: 8, md: 12 },
        }}
      >
        <Container maxWidth="lg">
          <Grid container spacing={4} alignItems="center">
            <Grid item xs={12} md={6}>
              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.8 }}
              >
                <Typography
                  variant="h1"
                  component="h1"
                  gutterBottom
                  sx={{
                    fontWeight: 800,
                    fontSize: { xs: '2.5rem', md: '4rem' },
                    lineHeight: 1.2,
                  }}
                >
                  Geleceğin Öğrenme Platformu
                </Typography>
                <Typography
                  variant="h5"
                  sx={{ mb: 4, opacity: 0.9, fontWeight: 400 }}
                >
                  Yapay zeka destekli, kişiselleştirilmiş sınav deneyimi ile öğrenme sürecinizi dönüştürün.
                </Typography>
                <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
                  <Button
                    variant="contained"
                    color="secondary"
                    size="large"
                    endIcon={<ArrowForwardIcon />}
                    onClick={() => navigate('/register')}
                    sx={{
                      py: 1.5,
                      px: 4,
                      borderRadius: 2,
                      fontSize: '1.1rem',
                    }}
                  >
                    Hemen Başla
                  </Button>
                  <Button
                    variant="outlined"
                    color="inherit"
                    size="large"
                    onClick={() => navigate('/login')}
                    sx={{
                      py: 1.5,
                      px: 4,
                      borderRadius: 2,
                      fontSize: '1.1rem',
                    }}
                  >
                    Giriş Yap
                  </Button>
                </Stack>
              </motion.div>
            </Grid>
            <Grid item xs={12} md={6}>
              <motion.div
                initial={{ opacity: 0, scale: 0.8 }}
                animate={{ opacity: 1, scale: 1 }}
                transition={{ duration: 0.8, delay: 0.2 }}
              >
                <Box
                  component="img"
                  src="/hero-illustration.svg"
                  alt="Quiz App"
                  sx={{
                    width: '100%',
                    maxWidth: 600,
                    height: 'auto',
                    filter: 'drop-shadow(0 0 20px rgba(255,255,255,0.2))',
                  }}
                />
              </motion.div>
            </Grid>
          </Grid>
        </Container>
      </Box>

      {/* Stats Section */}
      <Container maxWidth="lg" sx={{ mt: -4, mb: 8 }}>
        <Grid container spacing={3}>
          {stats.map((stat, index) => (
            <Grid item xs={6} md={3} key={index}>
              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5, delay: index * 0.1 }}
              >
                <Paper
                  elevation={3}
                  sx={{
                    p: 3,
                    textAlign: 'center',
                    borderRadius: 2,
                    bgcolor: 'background.paper',
                  }}
                >
                  <Box sx={{ color: 'primary.main', mb: 1 }}>
                    {stat.icon}
                  </Box>
                  <Typography variant="h4" component="div" gutterBottom fontWeight="bold">
                    {stat.value}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    {stat.label}
                  </Typography>
                </Paper>
              </motion.div>
            </Grid>
          ))}
        </Grid>
      </Container>

      {/* Features Section */}
      <Container maxWidth="lg" sx={{ mb: 8 }}>
        <Typography
          variant="h2"
          component="h2"
          align="center"
          gutterBottom
          sx={{ mb: 6, fontWeight: 700 }}
        >
          Öne Çıkan Özellikler
        </Typography>
        <Grid container spacing={4}>
          {features.map((feature, index) => (
            <Grid item xs={12} sm={6} md={3} key={index}>
              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5, delay: index * 0.1 }}
              >
                <Card
                  sx={{
                    height: '100%',
                    display: 'flex',
                    flexDirection: 'column',
                    transition: 'all 0.3s ease',
                    '&:hover': {
                      transform: 'translateY(-8px)',
                      boxShadow: '0 12px 20px rgba(0,0,0,0.1)',
                    },
                  }}
                >
                  <CardContent sx={{ flexGrow: 1, textAlign: 'center', p: 4 }}>
                    <Box
                      sx={{
                        color: feature.color,
                        mb: 2,
                        p: 2,
                        borderRadius: '50%',
                        bgcolor: `${feature.color}15`,
                        display: 'inline-flex',
                      }}
                    >
                      {feature.icon}
                    </Box>
                    <Typography
                      variant="h5"
                      component="h3"
                      gutterBottom
                      fontWeight="bold"
                    >
                      {feature.title}
                    </Typography>
                    <Typography
                      variant="body1"
                      color="text.secondary"
                    >
                      {feature.description}
                    </Typography>
                  </CardContent>
                </Card>
              </motion.div>
            </Grid>
          ))}
        </Grid>
      </Container>

      {/* Testimonials Section */}
      <Box sx={{ bgcolor: 'background.paper', py: 8 }}>
        <Container maxWidth="lg">
          <Typography
            variant="h2"
            component="h2"
            align="center"
            gutterBottom
            sx={{ mb: 6, fontWeight: 700 }}
          >
            Kullanıcı Deneyimleri
          </Typography>
          <Grid container spacing={4}>
            {testimonials.map((testimonial, index) => (
              <Grid item xs={12} md={6} key={index}>
                <motion.div
                  initial={{ opacity: 0, x: index % 2 === 0 ? -20 : 20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ duration: 0.5, delay: index * 0.2 }}
                >
                  <Paper
                    elevation={2}
                    sx={{
                      p: 4,
                      borderRadius: 2,
                      height: '100%',
                    }}
                  >
                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
                      <Avatar
                        src={testimonial.avatar}
                        sx={{ width: 60, height: 60, mr: 2 }}
                      />
                      <Box>
                        <Typography variant="h6" component="div">
                          {testimonial.name}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          {testimonial.role}
                        </Typography>
                      </Box>
                    </Box>
                    <Typography variant="body1" color="text.secondary">
                      "{testimonial.content}"
                    </Typography>
                  </Paper>
                </motion.div>
              </Grid>
            ))}
          </Grid>
        </Container>
      </Box>

      {/* CTA Section */}
      <Box
        sx={{
          background: 'linear-gradient(135deg, #1a237e 0%, #0d47a1 100%)',
          color: 'white',
          py: 8,
        }}
      >
        <Container maxWidth="md">
          <Box sx={{ textAlign: 'center' }}>
            <Typography
              variant="h2"
              component="h2"
              gutterBottom
              sx={{ fontWeight: 700 }}
            >
              Hemen Başlayın
            </Typography>
            <Typography
              variant="h6"
              sx={{ mb: 4, opacity: 0.9 }}
            >
              Binlerce öğretmen ve öğrenci ile birlikte geleceğin öğrenme deneyimini keşfedin.
            </Typography>
            <Button
              variant="contained"
              color="secondary"
              size="large"
              endIcon={<ArrowForwardIcon />}
              onClick={() => navigate('/register')}
              sx={{
                py: 2,
                px: 6,
                borderRadius: 2,
                fontSize: '1.1rem',
              }}
            >
              Ücretsiz Hesap Oluştur
            </Button>
          </Box>
        </Container>
      </Box>
    </Box>
  );
};

export default Home; 