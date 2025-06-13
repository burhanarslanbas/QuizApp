import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
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
import AddCircleIcon from '@mui/icons-material/AddCircle';
import { questionPoolService } from '../../services/questionPoolService';
import { categoryService } from '../../services/categoryService';
import { useNotification } from '../../context/NotificationContext';
import { useToken } from '../../context/TokenContext';

const CreateQuestionPool = (props) => {
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const { user } = useToken();
  const userRole = Array.isArray(user?.roles) && user.roles.length > 0 ? user.roles[0].toLowerCase() : undefined;
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [categories, setCategories] = useState([]);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    maxQuestions: 10,
    isPublic: false
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const data = await categoryService.getAllCategories();
        setCategories(data);
      } catch (err) {
        console.error('Error fetching categories:', err);
        setError(err.message || 'Kategoriler yüklenirken bir hata oluştu');
        showNotification(err.message || 'Kategoriler yüklenirken bir hata oluştu', 'error');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [showNotification]);

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
        name: formData.name,
        description: formData.description,
        maxQuestions: formData.maxQuestions,
        isPublic: formData.isPublic
      };
      await questionPoolService.create(request);
      showNotification('Soru havuzu başarıyla oluşturuldu', 'success');
      navigate('/question-pool');
    } catch (err) {
      setError(err.message);
      showNotification('Soru havuzu oluşturulurken bir hata oluştu', 'error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box p={{ xs: 1, md: 4 }} display="flex" justifyContent="center" alignItems="center" minHeight="80vh" bgcolor="#f5f7fa">
      <Card sx={{ maxWidth: 600, width: '100%', borderRadius: 4, boxShadow: 6 }}>
        <CardContent>
          <Stack direction="row" alignItems="center" spacing={2} mb={3}>
            <Avatar sx={{ bgcolor: '#1976d2', width: 56, height: 56 }}>
              <AddCircleIcon fontSize="large" />
            </Avatar>
            <Typography variant="h4" fontWeight={700}>
              Yeni Soru Havuzu Oluştur
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
                  {loading ? 'Oluşturuluyor...' : 'Oluştur'}
                </Button>
              </Stack>
            </Stack>
          </form>
        </CardContent>
      </Card>
    </Box>
  );
};

export default CreateQuestionPool; 