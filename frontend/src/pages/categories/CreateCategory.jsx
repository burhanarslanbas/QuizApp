import React, { useState, useEffect } from 'react';
import { Container, Typography, TextField, Button, Paper, Stack, Alert, CircularProgress } from '@mui/material';
import { categoryService } from '../../services/categoryService';
import { useNavigate, useParams } from 'react-router-dom';
import { useNotification } from '../../context/NotificationContext';
import Layout from '../../components/layout/Layout';

const CreateCategory = () => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const { id } = useParams();
  const { showNotification } = useNotification();
  const isEdit = Boolean(id);

  useEffect(() => {
    if (isEdit) {
      const fetchCategory = async () => {
        try {
          setLoading(true);
          const response = await categoryService.getCategory(id);
          setName(response.name);
          setDescription(response.description || '');
        } catch (err) {
          console.error('Category fetch error:', err);
          setError(err.message || 'Kategori bilgisi alınamadı.');
          showNotification(err.message || 'Kategori bilgisi alınırken bir hata oluştu', 'error');
        } finally {
          setLoading(false);
        }
      };
      fetchCategory();
    }
  }, [id, isEdit, showNotification]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!name.trim()) {
      setError('Kategori adı zorunludur.');
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const categoryData = {
        name: name.trim(),
        description: description.trim()
      };

      if (isEdit) {
        await categoryService.updateCategory({
          id: id,
          name: categoryData.name,
          description: categoryData.description
        });
        showNotification('Kategori başarıyla güncellendi', 'success');
      } else {
        await categoryService.createCategory(categoryData);
        showNotification('Kategori başarıyla oluşturuldu', 'success');
      }
      
      setTimeout(() => navigate('/categories'), 1000);
    } catch (err) {
      console.error('Category save error:', err);
      setError(err.message || (isEdit ? 'Kategori güncellenemedi.' : 'Kategori oluşturulamadı.'));
      showNotification(
        err.message || (isEdit ? 'Kategori güncellenirken bir hata oluştu' : 'Kategori oluşturulurken bir hata oluştu'),
        'error'
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <Layout>
      <Container maxWidth="sm" sx={{ py: 6 }}>
        <Paper sx={{ p: 4, borderRadius: 3, boxShadow: 3 }}>
          <Typography variant="h5" fontWeight={700} mb={3}>
            {isEdit ? 'Kategoriyi Düzenle' : 'Yeni Kategori Oluştur'}
          </Typography>
          <form onSubmit={handleSubmit}>
            <Stack spacing={3}>
              <TextField
                label="Kategori Adı"
                value={name}
                onChange={e => setName(e.target.value)}
                fullWidth
                required
                helperText="Kategori adı zorunludur."
                error={Boolean(error) && !name.trim()}
                disabled={loading}
                inputProps={{
                  maxLength: 100
                }}
              />
              <TextField
                label="Açıklama"
                value={description}
                onChange={e => setDescription(e.target.value)}
                fullWidth
                multiline
                rows={3}
                disabled={loading}
                inputProps={{
                  maxLength: 500
                }}
                helperText={`${description.length}/500 karakter`}
              />
              {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
              <Stack direction="row" spacing={2}>
                <Button
                  variant="outlined"
                  size="large"
                  fullWidth
                  disabled={loading}
                  onClick={() => navigate('/categories')}
                >
                  İptal
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  size="large"
                  fullWidth
                  disabled={loading}
                  startIcon={loading && <CircularProgress size={20} />}
                >
                  {isEdit ? 'Güncelle' : 'Kaydet'}
                </Button>
              </Stack>
            </Stack>
          </form>
        </Paper>
      </Container>
    </Layout>
  );
};

export default CreateCategory; 