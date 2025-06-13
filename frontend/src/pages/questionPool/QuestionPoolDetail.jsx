import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Stack,
  Chip,
  Tooltip,
  Divider,
  Avatar,
  Grid,
  CircularProgress,
  Alert
} from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';
import LockIcon from '@mui/icons-material/Lock';
import PublicIcon from '@mui/icons-material/Public';
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth';
import QuestionAnswerIcon from '@mui/icons-material/QuestionAnswer';
import NumbersIcon from '@mui/icons-material/Numbers';
import DescriptionIcon from '@mui/icons-material/Description';
import { questionPoolService } from '../../services/questionPoolService';
import { useNotification } from '../../context/NotificationContext';
import { userService } from '../../services/userService';
import { useToken } from '../../context/TokenContext';

// Tarih format fonksiyonu
const formatDateTR = (dateString) => {
  const date = new Date(dateString);
  const localDate = new Date(date.getTime() + (3 * 60 * 60 * 1000));
  const pad = (n) => n.toString().padStart(2, '0');
  return `${pad(localDate.getDate())}.${pad(localDate.getMonth() + 1)}.${localDate.getFullYear()}, ${pad(localDate.getHours())}:${pad(localDate.getMinutes())}:${pad(localDate.getSeconds())}`;
};

const QuestionPoolDetail = (props) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pool, setPool] = useState(null);
  const [creatorName, setCreatorName] = useState('');
  const { user } = useToken();

  useEffect(() => {
    fetchPoolDetails();
    // eslint-disable-next-line
  }, [id]);

  const fetchPoolDetails = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await questionPoolService.getById(id);
      setPool({
        name: data.name,
        description: data.description,
        maxQuestions: data.maxQuestions,
        isPublic: data.isPublic,
        isActive: data.isActive,
        questionCount: data.questionCount,
        createdAt: data.createdAt,
        updatedAt: data.updatedAt,
        creatorId: data.creatorId
      });
      // Kullanıcıyı getir
      if (data.creatorId) {
        try {
          const user = await userService.getUser(data.creatorId);
          setCreatorName(user.fullName || user.name || user.username || 'Bilinmiyor');
        } catch (userErr) {
          setCreatorName('Bilinmiyor');
        }
      } else {
        setCreatorName('Bilinmiyor');
      }
    } catch (err) {
      setError(err.message);
      showNotification('Soru havuzu detayları yüklenirken bir hata oluştu', 'error');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box p={3}>
        <Alert severity="error">{error}</Alert>
      </Box>
    );
  }

  if (!pool) {
    return (
      <Box p={3}>
        <Alert severity="info">Soru havuzu bulunamadı.</Alert>
      </Box>
    );
  }

  return (
    <Box p={{ xs: 1, md: 4 }} display="flex" justifyContent="center" alignItems="center" minHeight="80vh" bgcolor="#f5f7fa">
      <Card sx={{ maxWidth: 600, width: '100%', borderRadius: 4, boxShadow: 6 }}>
        <CardContent>
          <Stack direction="row" alignItems="center" spacing={2} mb={2}>
            <Avatar sx={{ bgcolor: '#1976d2', width: 56, height: 56 }}>
              <QuestionAnswerIcon fontSize="large" />
            </Avatar>
            <Box>
              <Typography variant="h4" fontWeight={700} gutterBottom>
                {pool.name}
              </Typography>
              <Stack direction="row" spacing={1}>
                <Chip
                  icon={pool.isActive ? <CheckCircleIcon /> : <CancelIcon />}
                  label={pool.isActive ? 'Aktif' : 'Pasif'}
                  color={pool.isActive ? 'success' : 'default'}
                  size="small"
                  sx={{ fontWeight: 600 }}
                />
                <Chip
                  icon={pool.isPublic ? <PublicIcon /> : <LockIcon />}
                  label={pool.isPublic ? 'Herkese Açık' : 'Özel'}
                  color={pool.isPublic ? 'primary' : 'warning'}
                  size="small"
                  sx={{ fontWeight: 600 }}
                />
              </Stack>
            </Box>
          </Stack>

          <Divider sx={{ my: 2 }} />

          <Grid container spacing={2}>
            <Grid item xs={12}>
              <Stack direction="row" alignItems="center" spacing={1}>
                <DescriptionIcon color="action" />
                <Typography variant="body1" color="text.secondary">
                  {pool.description}
                </Typography>
              </Stack>
            </Grid>
            <Grid item xs={12}>
              <Stack direction="row" alignItems="center" spacing={1}>
                <Avatar sx={{ width: 24, height: 24, bgcolor: '#90caf9', fontSize: 14 }}>
                  {creatorName ? creatorName[0] : '?'}
                </Avatar>
                <Typography variant="subtitle2" color="text.secondary">
                  Oluşturan: {creatorName || 'Bilinmiyor'}
                </Typography>
              </Stack>
            </Grid>
            <Grid item xs={6}>
              <Stack direction="row" alignItems="center" spacing={1}>
                <NumbersIcon color="primary" />
                <Typography variant="subtitle2">Maksimum Soru:</Typography>
                <Typography variant="subtitle1" fontWeight={600}>{pool.maxQuestions}</Typography>
              </Stack>
            </Grid>
            <Grid item xs={6}>
              <Stack direction="row" alignItems="center" spacing={1}>
                <QuestionAnswerIcon color="secondary" />
                <Typography variant="subtitle2">Mevcut Soru:</Typography>
                <Typography variant="subtitle1" fontWeight={600}>{pool.questionCount}</Typography>
              </Stack>
            </Grid>
            <Grid item xs={6}>
              <Stack direction="row" alignItems="center" spacing={1}>
                <CalendarMonthIcon color="info" />
                <Tooltip title="Oluşturulma Tarihi">
                  <Typography variant="subtitle2">{formatDateTR(pool.createdAt)}</Typography>
                </Tooltip>
              </Stack>
            </Grid>
            <Grid item xs={6}>
              <Stack direction="row" alignItems="center" spacing={1}>
                <CalendarMonthIcon color="success" />
                <Tooltip title="Son Güncelleme">
                  <Typography variant="subtitle2">{formatDateTR(pool.updatedAt)}</Typography>
                </Tooltip>
              </Stack>
            </Grid>
          </Grid>

          <Stack direction="row" spacing={2} mt={4} justifyContent="flex-end">
            <Button
              variant="outlined"
              onClick={() => navigate('/question-pool')}
              sx={{ borderRadius: 2 }}
            >
              Geri Dön
            </Button>
            <Button
              variant="contained"
              onClick={() => navigate(`/question-pool/${id}/edit`)}
              sx={{ borderRadius: 2 }}
            >
              Düzenle
            </Button>
          </Stack>
        </CardContent>
      </Card>
    </Box>
  );
};

export default QuestionPoolDetail; 