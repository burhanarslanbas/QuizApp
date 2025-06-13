import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Card,
  CardContent,
  Grid,
  Typography,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  TextField,
  Stack,
  Menu,
  MenuItem,
  InputAdornment,
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  MoreVert as MoreVertIcon,
  Search as SearchIcon,
} from '@mui/icons-material';
import { categoryService } from '../../services/categoryService';
import Loading from '../../components/common/Loading';
import ErrorComponent from '../../components/common/Error';
import { useNotification } from '../../context/NotificationContext';
import Layout from '../../components/layout/Layout';
import { useToken } from '../../context/TokenContext';

const Categories = () => {
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [anchorEl, setAnchorEl] = useState(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredCategories, setFilteredCategories] = useState([]);
  const { user } = useToken();

  useEffect(() => {
    fetchCategories();
  }, []);

  useEffect(() => {
    if (categories) {
      const filtered = categories.filter(category =>
        category.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        category.description?.toLowerCase().includes(searchTerm.toLowerCase())
      );
      setFilteredCategories(filtered);
    }
  }, [searchTerm, categories]);

  const fetchCategories = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await categoryService.getCategories();
      setCategories(response);
      setFilteredCategories(response);
    } catch (err) {
      console.error('Error fetching categories:', err);
      setError(err.message || 'Failed to load categories');
      showNotification(err.message || 'Error loading categories', 'error');
    } finally {
      setLoading(false);
    }
  };

  const handleMenuClick = (event, category) => {
    setAnchorEl(event.currentTarget);
    setSelectedCategory(category);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedCategory(null);
  };

  const handleEditClick = () => {
    handleMenuClose();
    navigate(`/categories/${selectedCategory.id}/edit`);
  };

  const handleDeleteClick = () => {
    handleMenuClose();
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    try {
      await categoryService.deleteCategory(selectedCategory.id);
      showNotification('Kategori başarıyla silindi', 'success');
      fetchCategories();
    } catch (err) {
      console.error('Error deleting category:', err);
      showNotification(err.message || 'Kategori silinirken bir hata oluştu', 'error');
    } finally {
      setDeleteDialogOpen(false);
      setSelectedCategory(null);
    }
  };

  const handleDeleteCancel = () => {
    setDeleteDialogOpen(false);
    setSelectedCategory(null);
  };

  if (loading) return <Loading />;
  if (error) return <ErrorComponent message={error} />;

  return (
    <Layout>
      <Box sx={{ p: 3 }}>
        <Stack direction="row" justifyContent="space-between" alignItems="center" mb={3}>
          <Typography variant="h4" component="h1" gutterBottom>
            Kategoriler
          </Typography>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => navigate('/categories/create')}
          >
            Yeni Kategori
          </Button>
        </Stack>

        <Box mb={3}>
          <TextField
            fullWidth
            variant="outlined"
            placeholder="Kategori ara..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
            sx={{ maxWidth: 500 }}
          />
        </Box>

        <Grid container spacing={3}>
          {filteredCategories.map((category) => (
            <Grid item xs={12} sm={6} md={4} key={category.id}>
              <Card
                sx={{
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  position: 'relative',
                  transition: 'transform 0.2s, box-shadow 0.2s',
                  '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: '0 4px 20px rgba(0,0,0,0.1)',
                  },
                }}
                onClick={() => navigate(`/categories/${category.id}`)}
              >
                <CardContent sx={{ flexGrow: 1 }}>
                  <Box sx={{ position: 'absolute', top: 8, right: 8 }}>
                    <IconButton
                      size="small"
                      onClick={(e) => {
                        e.stopPropagation();
                        handleMenuClick(e, category);
                      }}
                      aria-label="category options"
                    >
                      <MoreVertIcon />
                    </IconButton>
                  </Box>
                  <Typography variant="h6" gutterBottom component="div">
                    {category.name}
                  </Typography>
                  <Typography variant="body2" color="text.secondary" paragraph>
                    {category.description}
                  </Typography>
                </CardContent>
                <Box sx={{ p: 2, pt: 0 }}>
                  <Button
                    variant="contained"
                    fullWidth
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate(`/categories/${category.id}`);
                    }}
                  >
                    Detayları Görüntüle
                  </Button>
                </Box>
              </Card>
            </Grid>
          ))}
        </Grid>

        <Menu
          anchorEl={anchorEl}
          open={Boolean(anchorEl)}
          onClose={handleMenuClose}
          onClick={(e) => e.stopPropagation()}
        >
          <MenuItem onClick={handleEditClick}>
            <EditIcon fontSize="small" sx={{ mr: 1 }} />
            Düzenle
          </MenuItem>
          <MenuItem onClick={handleDeleteClick} sx={{ color: 'error.main' }}>
            <DeleteIcon fontSize="small" sx={{ mr: 1 }} />
            Sil
          </MenuItem>
        </Menu>

        <Dialog
          open={deleteDialogOpen}
          onClose={handleDeleteCancel}
        >
          <DialogTitle>Kategoriyi Sil</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Bu kategoriyi silmek istediğinizden emin misiniz? Bu işlem geri alınamaz.
            </DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleDeleteCancel}>İptal</Button>
            <Button onClick={handleDeleteConfirm} color="error" variant="contained">
              Sil
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
    </Layout>
  );
};

export default Categories; 