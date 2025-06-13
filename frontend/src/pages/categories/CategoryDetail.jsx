import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Stack,
  Alert,
  CircularProgress,
} from '@mui/material';
import {
  Edit as EditIcon,
  ArrowBack as ArrowBackIcon,
} from '@mui/icons-material';
import { categoryService } from '../../services/categoryService';
import { useNotification } from '../../context/NotificationContext';
import { useToken } from '../../context/TokenContext';
import Layout from '../../components/layout/Layout';

const CategoryDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const { user } = useToken();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [category, setCategory] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const categoryResponse = await categoryService.getCategory(id);
        setCategory(categoryResponse);
      } catch (err) {
        console.error('Error fetching category details:', err);
        setError(err.message || 'Kategori detayları yüklenirken bir hata oluştu');
        showNotification(err.message || 'Kategori detayları yüklenirken bir hata oluştu', 'error');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, showNotification]);

  if (loading) {
    return (
      <Layout>
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
          <CircularProgress />
        </Box>
      </Layout>
    );
  }

  if (error) {
    return (
      <Layout>
        <Box p={3}>
          <Alert severity="error">{error}</Alert>
        </Box>
      </Layout>
    );
  }

  if (!category) {
    return (
      <Layout>
        <Box p={3}>
          <Alert severity="warning">Kategori bulunamadı</Alert>
        </Box>
      </Layout>
    );
  }

  return (
    <Layout>
      <Box p={3}>
        <Stack direction="row" spacing={2} alignItems="center" mb={3}>
          <Button
            startIcon={<ArrowBackIcon />}
            onClick={() => navigate('/categories')}
          >
            Geri
          </Button>
          {(user?.role === 'admin' || user?.role === 'teacher') && (
            <Button
              variant="contained"
              startIcon={<EditIcon />}
              onClick={() => navigate(`/categories/${id}/edit`)}
            >
              Düzenle
            </Button>
          )}
        </Stack>

        <Card>
          <CardContent>
            <Stack spacing={3}>
              <Typography variant="h4" component="h1">
                {category.name}
              </Typography>
              <Typography variant="body1" color="text.secondary">
                {category.description}
              </Typography>
            </Stack>
          </CardContent>
        </Card>
      </Box>
    </Layout>
  );
};

export default CategoryDetail; 