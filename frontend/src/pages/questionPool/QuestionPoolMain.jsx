import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Stack,
  Alert,
  CircularProgress,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  IconButton,
  Tooltip,
  Avatar,
  Chip
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Visibility as ViewIcon,
  Collections as CollectionsIcon
} from '@mui/icons-material';
import { questionPoolService } from '../../services/questionPoolService';
import { useNotification } from '../../context/NotificationContext';
import { userService } from '../../services/userService';

const QuestionPoolMain = (props) => {
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [questionPools, setQuestionPools] = useState([]);
  const [creatorNames, setCreatorNames] = useState({});

  useEffect(() => {
    fetchQuestionPools();
  }, []);

  useEffect(() => {
    // Havuzlar yüklendikten sonra oluşturucu adlarını çek
    const fetchCreators = async () => {
      const names = {};
      for (const pool of questionPools) {
        if (pool.creatorId && !names[pool.creatorId]) {
          try {
            const user = await userService.getUser(pool.creatorId);
            names[pool.creatorId] = user.fullName || user.name || user.username || '-';
          } catch {
            names[pool.creatorId] = '-';
          }
        }
      }
      setCreatorNames(names);
    };
    if (questionPools.length > 0) fetchCreators();
  }, [questionPools]);

  const formatDateTR = (dateString) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    // Türkiye saati için UTC+3 ekle
    const localDate = new Date(date.getTime() + (3 * 60 * 60 * 1000));
    const pad = (n) => n.toString().padStart(2, '0');
    return `${pad(localDate.getDate())}.${pad(localDate.getMonth() + 1)}.${localDate.getFullYear()} ${pad(localDate.getHours())}:${pad(localDate.getMinutes())}`;
  };

  const fetchQuestionPools = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await questionPoolService.getAll();
      setQuestionPools(Array.isArray(response) ? response : []);
    } catch (err) {
      setError(err.message);
      showNotification('Soru havuzları yüklenirken bir hata oluştu', 'error');
      setQuestionPools([]);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu soru havuzunu silmek istediğinizden emin misiniz?')) {
      try {
        await questionPoolService.delete(id);
        showNotification('Soru havuzu başarıyla silindi', 'success');
        fetchQuestionPools();
      } catch (err) {
        showNotification('Soru havuzu silinirken bir hata oluştu', 'error');
      }
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box p={{ xs: 1, md: 4 }} minHeight="80vh" bgcolor="#f5f7fa">
      <Card sx={{ borderRadius: 4, boxShadow: 6 }}>
        <CardContent>
          <Stack direction="row" alignItems="center" spacing={2} mb={3}>
            <Avatar sx={{ bgcolor: '#1976d2', width: 56, height: 56 }}>
              <CollectionsIcon fontSize="large" />
            </Avatar>
            <Box flex={1}>
              <Typography variant="h4" fontWeight={700}>
                Soru Havuzları
              </Typography>
              <Typography variant="subtitle1" color="text.secondary">
                Tüm soru havuzlarını görüntüleyin ve yönetin
              </Typography>
            </Box>
            <Button
              variant="contained"
              startIcon={<AddIcon />}
              onClick={() => navigate('/question-pool/create')}
              sx={{ borderRadius: 2, px: 3 }}
            >
              Yeni Havuz
            </Button>
          </Stack>

          {error && (
            <Alert severity="error" sx={{ mb: 3 }}>
              {error}
            </Alert>
          )}

          <TableContainer component={Paper} sx={{ borderRadius: 2, boxShadow: 2 }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell sx={{ fontWeight: 600 }}>Başlık</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Açıklama</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Oluşturan</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Oluşturulma Tarihi</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Soru Sayısı</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>Durum</TableCell>
                  <TableCell sx={{ fontWeight: 600 }}>İşlemler</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {questionPools.map((pool) => (
                  <TableRow key={pool.id} hover>
                    <TableCell>{pool.name || '-'}</TableCell>
                    <TableCell>{pool.description || '-'}</TableCell>
                    <TableCell>{creatorNames[pool.creatorId] || '-'}</TableCell>
                    <TableCell>{formatDateTR(pool.createdAt)}</TableCell>
                    <TableCell>{typeof pool.questionCount === 'number' ? pool.questionCount : 0}</TableCell>
                    <TableCell>
                      <Chip
                        label={pool.isActive ? 'Aktif' : 'Pasif'}
                        color={pool.isActive ? 'success' : 'default'}
                        size="small"
                        sx={{ fontWeight: 600 }}
                      />
                    </TableCell>
                    <TableCell>
                      <Stack direction="row" spacing={1}>
                        <Tooltip title="Görüntüle">
                          <IconButton
                            size="small"
                            onClick={() => navigate(`/question-pool/${pool.id}`)}
                            sx={{ color: 'primary.main' }}
                          >
                            <ViewIcon />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Düzenle">
                          <IconButton
                            size="small"
                            onClick={() => navigate(`/question-pool/${pool.id}/edit`)}
                            sx={{ color: 'info.main' }}
                          >
                            <EditIcon />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Sil">
                          <IconButton
                            size="small"
                            onClick={() => handleDelete(pool.id)}
                            sx={{ color: 'error.main' }}
                          >
                            <DeleteIcon />
                          </IconButton>
                        </Tooltip>
                      </Stack>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </CardContent>
      </Card>
    </Box>
  );
};

export default QuestionPoolMain; 