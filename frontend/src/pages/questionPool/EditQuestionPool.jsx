import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Stack,
  Alert,
  CircularProgress,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Avatar
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import { questionPoolService } from '../../services/questionPoolService';
import { categoryService } from '../../services/categoryService';
import { useNotification } from '../../context/NotificationContext';

const EditQuestionPool = (props) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [categories, setCategories] = useState([]);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    maxQuestions: 10,
    isPublic: false,
    isActive: true
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [poolResponse, categoriesResponse] = await Promise.all([
          questionPoolService.getQuestionPoolById(id),
          categoryService.getAllCategories()
        ]);

        setFormData(poolResponse);
        setCategories(categoriesResponse);
      } catch (err) {
        console.error('Error fetching data:', err);
        setError(err.message || 'Veriler yüklenirken bir hata oluştu');
        showNotification(err.message || 'Veriler yüklenirken bir hata oluştu', 'error');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, showNotification]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      setError(null);
      const request = {
        id: id,
        name: formData.name,
        description: formData.description,
        maxQuestions: formData.maxQuestions,
        isPublic: formData.isPublic,
        isActive: formData.isActive
      };
      await questionPoolService.update(request);
      showNotification('Soru havuzu başarıyla güncellendi', 'success');
      navigate('/question-pool');
    } catch (err) {
      setError(err.message);
      showNotification('Soru havuzu güncellenirken bir hata oluştu', 'error');
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

  return (
    <Box p={{ xs: 1, md: 4 }} display="flex" justifyContent="center" alignItems="center" minHeight="80vh" bgcolor="#f5f7fa">
      <Card sx={{ maxWidth: 600, width: '100%', borderRadius: 4, boxShadow: 6 }}>
        <CardContent>
          <Stack direction="row" alignItems="center" spacing={2} mb={3}>
            <Avatar sx={{ bgcolor: '#1976d2', width: 56, height: 56 }}>
              <EditIcon fontSize="large" />
            </Avatar>
            <Typography variant="h4" fontWeight={700}>
              Soru Havuzunu Düzenle
            </Typography>
          </Stack>

          {error && (
            <Alert severity="error" sx={{ mb: 3 }}>
              {error}
            </Alert>
          )}

          <form onSubmit={handleSubmit}>
            <Stack spacing={3}>
              <TextField
                fullWidth
                label="Başlık"
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
                sx={{ '& .MuiOutlinedInput-root': { borderRadius: 2 } }}
              />

              <TextField
                fullWidth
                label="Açıklama"
                name="description"
                value={formData.description}
                onChange={handleChange}
                multiline
                rows={4}
                required
                sx={{ '& .MuiOutlinedInput-root': { borderRadius: 2 } }}
              />

              <TextField
                fullWidth
                label="Maksimum Soru Sayısı"
                name="maxQuestions"
                type="number"
                value={formData.maxQuestions}
                onChange={handleChange}
                required
                inputProps={{ min: 1 }}
                sx={{ '& .MuiOutlinedInput-root': { borderRadius: 2 } }}
              />

              <FormControl fullWidth>
                <InputLabel>Herkese Açık</InputLabel>
                <Select
                  name="isPublic"
                  value={formData.isPublic}
                  onChange={handleChange}
                  label="Herkese Açık"
                  sx={{ borderRadius: 2 }}
                >
                  <MenuItem value={true}>Evet</MenuItem>
                  <MenuItem value={false}>Hayır</MenuItem>
                </Select>
              </FormControl>

              <FormControl fullWidth>
                <InputLabel>Aktif</InputLabel>
                <Select
                  name="isActive"
                  value={formData.isActive}
                  onChange={handleChange}
                  label="Aktif"
                  sx={{ borderRadius: 2 }}
                >
                  <MenuItem value={true}>Evet</MenuItem>
                  <MenuItem value={false}>Hayır</MenuItem>
                </Select>
              </FormControl>

              <Stack direction="row" spacing={2} justifyContent="flex-end" mt={2}>
                <Button
                  variant="outlined"
                  onClick={() => navigate('/question-pool')}
                  disabled={loading}
                  sx={{ borderRadius: 2, px: 3 }}
                >
                  İptal
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  disabled={loading}
                  startIcon={loading ? <CircularProgress size={20} /> : null}
                  sx={{ borderRadius: 2, px: 3 }}
                >
                  {loading ? 'Güncelleniyor...' : 'Güncelle'}
                </Button>
              </Stack>
            </Stack>
          </form>
        </CardContent>
      </Card>
    </Box>
  );
};

export default EditQuestionPool; 