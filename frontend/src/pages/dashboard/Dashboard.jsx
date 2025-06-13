import React, { useState, useEffect } from 'react';
import { useNavigate, Navigate } from 'react-router-dom';
import {
  Box,
  Container,
  Typography,
  Button,
  Paper,
  Grid,
  Avatar,
  Stack,
  Card,
  CardContent,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
  Divider,
  LinearProgress,
  useMediaQuery,
  CircularProgress,
  Alert
} from '@mui/material';
import {
  Dashboard as DashboardIcon,
  Quiz as QuizIcon,
  Category as CategoryIcon,
  BarChart as BarChartIcon,
  Person as PersonIcon,
  Assignment as AssignmentIcon,
  EmojiEvents as TrophyIcon,
  Add as AddIcon,
  HelpOutline as HelpIcon,
  QuestionAnswer as QuestionIcon,
  ListAlt as ListIcon,
  Timeline as TimelineIcon,
  ArrowForward as ArrowForwardIcon,
} from '@mui/icons-material';
import { quizService } from '../../services/quizService';
// import { quizResultService } from '../../services/quizResultService';
import Loading from '../../components/common/Loading';
import Error from '../../components/common/Error';
import { useNotification } from '../../context/NotificationContext';
import { useToken } from '../../context/TokenContext';
import TokenTimer from '../../components/token/TokenTimer';
import '../../components/token/TokenTimer.css';
import { useTheme } from '@mui/material/styles';
import Layout from '../../components/layout/Layout';
import { authService } from '../../services/authService';

const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';
const EXPIRES_AT_KEY = 'expires_at';
const REFRESH_TOKEN_EXPIRES_AT_KEY = 'refresh_token_expires_at';

const Dashboard = (props) => {
  const navigate = useNavigate();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const { showNotification } = useNotification();
  const { isAuthenticated, isLoading: tokenLoading, checking, tokenExpiresAt, refreshTokenExpiresAt, user } = useToken();
  const userRole = Array.isArray(user?.roles) && user.roles.length > 0 ? user.roles[0].toLowerCase() : undefined;
  console.log('Dashboard user:', user);
  console.log('Dashboard userRole:', userRole);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [stats, setStats] = useState({
    totalQuizzes: 0,
    completedQuizzes: 0,
    averageScore: 0,
    recentQuizzes: [],
  });

  // Fallback: get expiration from localStorage if not available in context
  const accessTokenExpiration = tokenExpiresAt || localStorage.getItem(EXPIRES_AT_KEY);
  const refreshTokenExpiration = refreshTokenExpiresAt || localStorage.getItem(REFRESH_TOKEN_EXPIRES_AT_KEY);

  // Access token'ın süresi bitmeye ne kadar kaldığını döner (saniye cinsinden)
  const getTokenRemainingTime = () => {
    if (!accessTokenExpiration) return 0;
    const expTime = new Date(accessTokenExpiration).getTime();
    const now = Date.now();
    return Math.floor((expTime - now) / 1000); // saniye
  };

  useEffect(() => {
    // Access token süresi bitmeye 1 dakika kala otomatik yenile
    const checkAndRefresh = async () => {
      const remaining = getTokenRemainingTime();
      if (remaining > 0 && remaining < 60) {
        try {
          const result = await authService.refreshToken();
          console.log('Access token otomatik yenilendi:', result);
        } catch (err) {
          console.error('Access token otomatik yenileme hatası:', err);
        }
      }
    };
    checkAndRefresh();
    const interval = setInterval(checkAndRefresh, 30000); // Her 30 sn'de bir kontrol
    return () => clearInterval(interval);
  }, [accessTokenExpiration]);

  useEffect(() => {
    if (!tokenLoading && !checking) {
      fetchStats();
    }
  }, [tokenLoading, checking]);

  const fetchStats = async () => {
    try {
      setLoading(true);
      setError(null);
      
      let data;
      if (userRole === 'admin' || userRole === 'teacher') {
        data = await quizService.getStats();
      } else {
        data = await quizService.getMyResults();
      }

      setStats({
        totalQuizzes: data.totalQuizzes || 0,
        completedQuizzes: data.completedQuizzes || 0,
        averageScore: data.averageScore || 0,
        recentQuizzes: data.recentQuizzes || []
      });
    } catch (err) {
      setError(err.message);
      showNotification('İstatistikler yüklenirken bir hata oluştu', 'error');
    } finally {
      setLoading(false);
    }
  };

  if (tokenLoading || checking || loading) {
    return <Loading />;
  }

  if (error) {
    return <Error message={error} />;
  }

  const quickAccessItems = [
    {
      title: 'Sınava Gir',
      description: 'Yeni bir sınava başla',
      icon: <QuizIcon />,
      action: () => navigate('/quizzes'),
      roles: ['admin', 'teacher', 'student']
    },
    {
      title: 'Sonuçlarım',
      description: 'Sınav sonuçlarını görüntüle',
      icon: <AssignmentIcon />,
      action: () => navigate('/my-results'),
      roles: ['admin', 'teacher', 'student']
    },
    {
      title: 'Sınav Oluştur',
      description: 'Yeni bir sınav oluştur',
      icon: <QuizIcon />,
      action: () => navigate('/quizzes/create'),
      roles: ['admin', 'teacher']
    },
    {
      title: 'Raporları Görüntüle',
      description: 'Sınav raporlarını incele',
      icon: <BarChartIcon />,
      action: () => navigate('/reports'),
      roles: ['admin', 'teacher']
    },
    {
      title: 'Kullanıcıları Yönet',
      description: 'Kullanıcı hesaplarını yönet',
      icon: <PersonIcon />,
      action: () => navigate('/users'),
      roles: ['admin']
    }
  ];

  // Kullanıcının rolüne göre hızlı erişim öğelerini filtrele
  const filteredQuickAccessItems = quickAccessItems.filter(item => 
    item.roles.includes(userRole)
  );

  return (
    <Layout>
      <Box sx={{ width: '100%', minHeight: '100vh', background: 'background.default', p: { xs: 1, md: 3 }, boxSizing: 'border-box' }}>
        {/* Token Info - Sadece admin ve teacher için göster */}
        {(userRole === 'admin' || userRole === 'teacher') && (
          <Card sx={{ mb: 2, bgcolor: 'warning.light', color: 'warning.contrastText', borderRadius: 2 }}>
            <CardContent>
              <Typography variant="subtitle2" fontWeight={700}>Token Bilgisi</Typography>
              <Typography variant="body2">Access Token: {tokenExpiresAt ? new Date(tokenExpiresAt).toLocaleString() : '-'}</Typography>
              <Typography variant="body2">Refresh Token: {refreshTokenExpiresAt ? new Date(refreshTokenExpiresAt).toLocaleString() : '-'}</Typography>
            </CardContent>
          </Card>
        )}

        {/* Action Buttons - Rol bazlı gösterim */}
        <Stack direction={{ xs: 'column', md: 'row' }} spacing={2} mb={4} alignItems={{ xs: 'stretch', md: 'center' }}>
          <Stack direction="row" spacing={2}>
            {(userRole === 'admin' || userRole === 'teacher') && (
              <Button variant="contained" startIcon={<AddIcon />} onClick={() => navigate('/quizzes/create')} sx={{ fontWeight: 600, borderRadius: 2 }}>
                Sınav Oluştur
              </Button>
            )}
            {(userRole === 'admin' || userRole === 'teacher') && (
              <Button variant="outlined" startIcon={<BarChartIcon />} onClick={() => navigate('/reports')} sx={{ fontWeight: 600, borderRadius: 2 }}>
                Raporları Görüntüle
              </Button>
            )}
            <Button variant="text" startIcon={<HelpIcon />} onClick={() => window.open('https://yardim.quizapp.com', '_blank')} sx={{ fontWeight: 600, borderRadius: 2 }}>
              Yardım & Destek
            </Button>
          </Stack>
          <Box flex={1} />
          <Stack direction="row" spacing={2} alignItems="center">
            <TokenTimer />
            <Card sx={{ minWidth: 220, bgcolor: 'primary.light', color: 'primary.contrastText', boxShadow: 0, borderRadius: 2 }}>
              <CardContent sx={{ py: 1.5, px: 2 }}>
                <Typography variant="subtitle2" fontWeight={700} color="primary.contrastText">Duyuru</Typography>
                <Typography variant="body2" color="primary.contrastText">Yarın 10:00'da sistem bakımı yapılacaktır.</Typography>
              </CardContent>
            </Card>
          </Stack>
        </Stack>

        {/* Quick Access Cards */}
        <Grid container spacing={3} sx={{ mb: 4 }}>
          {filteredQuickAccessItems.map((item, index) => (
            <Grid item xs={12} sm={6} md={4} key={index}>
              <Card 
                sx={{ 
                  height: '100%', 
                  cursor: 'pointer',
                  transition: 'transform 0.2s',
                  '&:hover': { 
                    transform: 'translateY(-4px)', 
                  },
                }}
                onClick={item.action}
              >
                <CardContent>
                  <Stack direction="row" spacing={2} alignItems="center">
                    <Avatar sx={{ bgcolor: 'primary.main' }}>
                      {item.icon}
                    </Avatar>
                    <Box>
                      <Typography variant="h6" gutterBottom>
                        {item.title}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        {item.description}
                      </Typography>
                    </Box>
                  </Stack>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>

        {/* Stats Cards */}
        <Grid container spacing={3} mb={4}>
          <Grid item xs={12} sm={6} md={3}>
            <Card sx={{ height: '100%', borderRadius: 2 }}>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Toplam Sınav
                </Typography>
                <Typography variant="h4" color="primary">
                  {stats.totalQuizzes}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={3}>
            <Card sx={{ height: '100%', borderRadius: 2 }}>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Tamamlanan Sınav
                </Typography>
                <Typography variant="h4" color="success.main">
                  {stats.completedQuizzes}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={3}>
            <Card sx={{ height: '100%', borderRadius: 2 }}>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Ortalama Puan
                </Typography>
                <Typography variant="h4" color="warning.main">
                  {stats.averageScore.toFixed(1)}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
          <Grid item xs={12} sm={6} md={3}>
            <Card sx={{ height: '100%', borderRadius: 2 }}>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Son Sınavlar
                </Typography>
                <Typography variant="h4" color="info.main">
                  {stats.recentQuizzes.length}
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        </Grid>

        {/* Recent Quizzes */}
        <Card sx={{ mb: 4, borderRadius: 2 }}>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              Son Tamamlanan Sınavlar
            </Typography>
            <List>
              {stats.recentQuizzes.map((quiz, index) => (
                <React.Fragment key={quiz.id}>
                  <ListItem>
                    <ListItemIcon>
                      <AssignmentIcon color="primary" />
                    </ListItemIcon>
                    <ListItemText
                      primary={quiz.title}
                      secondary={`Puan: ${quiz.score} - ${new Date(quiz.createdAt).toLocaleString()}`}
                    />
                    <Button
                      size="small"
                      endIcon={<ArrowForwardIcon />}
                      onClick={() => navigate(`/quizzes/${quiz.id}`)}
                    >
                      Detaylar
                    </Button>
                  </ListItem>
                  {index < stats.recentQuizzes.length - 1 && <Divider />}
                </React.Fragment>
              ))}
              {stats.recentQuizzes.length === 0 && (
                <ListItem>
                  <ListItemText
                    primary="Henüz tamamlanan sınav bulunmuyor"
                    sx={{ textAlign: 'center', color: 'text.secondary' }}
                  />
                </ListItem>
              )}
            </List>
          </CardContent>
        </Card>
      </Box>
    </Layout>
  );
};

export default Dashboard; 