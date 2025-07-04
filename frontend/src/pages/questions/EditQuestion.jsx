import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Container,
  Paper,
  Typography,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Box,
  Grid,
  IconButton,
  CircularProgress,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { questionService } from '@/services/questionService';
import { categoryService } from '@/services/categoryService';
import { useNotification } from '@/context/NotificationContext';

const EditQuestion = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [formData, setFormData] = useState({
    questionText: '',
    questionType: 1,
    points: 1,
    orderIndex: 0,
    explanation: '',
    imageUrl: '',
    isActive: true,
    questionRepoId: '',
    options: [{ optionText: '', isCorrect: false, orderIndex: 0 }, { optionText: '', isCorrect: false, orderIndex: 1 }],
    correctAnswer: '',
  });

  useEffect(() => {
    loadCategories();
    loadQuestion();
  }, [id]);

  const loadCategories = async () => {
    try {
      const response = await categoryService.getAllCategories();
      setCategories(response.data);
    } catch (error) {
      showNotification('Kategoriler yüklenirken bir hata oluştu', 'error');
    }
  };

  const loadQuestion = async () => {
    setLoading(true);
    try {
      const response = await questionService.getQuestion(id);
      const q = response.data;
      setFormData({
        questionText: q.questionText,
        questionType: q.questionType,
        points: q.points,
        orderIndex: q.orderIndex,
        explanation: q.explanation || '',
        imageUrl: q.imageUrl || '',
        isActive: q.isActive,
        questionRepoId: q.questionRepoId || '',
        options: (q.options || []).map((opt, idx) => ({
          optionText: opt.optionText,
          isCorrect: opt.isCorrect,
          orderIndex: opt.orderIndex ?? idx
        })),
        correctAnswer: q.questionType === 2
          ? (q.options?.find(opt => opt.isCorrect)?.optionText === 'Doğru')
          : (q.options?.find(opt => opt.isCorrect)?.optionText || ''),
      });
    } catch (error) {
      showNotification('Soru yüklenirken bir hata oluştu', 'error');
      navigate('/questions');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleOptionChange = (index, field, value) => {
    const newOptions = [...formData.options];
    newOptions[index] = {
      ...newOptions[index],
      [field]: value,
    };
    setFormData((prev) => ({
      ...prev,
      options: newOptions,
    }));
  };

  const addOption = () => {
    setFormData((prev) => ({
      ...prev,
      options: [...prev.options, { optionText: '', isCorrect: false, orderIndex: prev.options.length }],
    }));
  };

  const removeOption = (index) => {
    setFormData((prev) => ({
      ...prev,
      options: prev.options.filter((_, i) => i !== index),
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    let options = [];
    if (formData.questionType === 1) {
      options = formData.options;
    } else if (formData.questionType === 2) {
      options = [
        { optionText: 'Doğru', isCorrect: formData.correctAnswer === true, orderIndex: 0 },
        { optionText: 'Yanlış', isCorrect: formData.correctAnswer === false, orderIndex: 1 }
      ];
    } else if (formData.questionType === 3) {
      options = [
        { optionText: formData.correctAnswer, isCorrect: true, orderIndex: 0 }
      ];
    }
    const payload = {
      id,
      questionText: formData.questionText,
      questionType: formData.questionType,
      points: formData.points,
      orderIndex: formData.orderIndex,
      explanation: formData.explanation,
      imageUrl: formData.imageUrl,
      isActive: formData.isActive,
      questionRepoId: formData.questionRepoId || null,
      options,
    };
    try {
      await questionService.updateQuestion(payload);
      showNotification('Soru başarıyla güncellendi', 'success');
      navigate('/questions');
    } catch (error) {
      showNotification('Soru güncellenirken bir hata oluştu', 'error');
    }
  };

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ py: 4, textAlign: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Soruyu Düzenle
        </Typography>

        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Soru Metni"
                name="questionText"
                value={formData.questionText}
                onChange={handleChange}
                required
                multiline
                rows={3}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <FormControl fullWidth required>
                <InputLabel>Soru Tipi</InputLabel>
                <Select
                  name="questionType"
                  value={formData.questionType}
                  onChange={handleChange}
                  label="Soru Tipi"
                >
                  <MenuItem value={1}>Çoktan Seçmeli</MenuItem>
                  <MenuItem value={2}>Doğru/Yanlış</MenuItem>
                  <MenuItem value={3}>Kısa Cevap</MenuItem>
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12} md={6}>
              <FormControl fullWidth required>
                <InputLabel>Kategori</InputLabel>
                <Select
                  name="categoryId"
                  value={formData.categoryId}
                  onChange={handleChange}
                  label="Kategori"
                >
                  {categories.map((category) => (
                    <MenuItem key={category.id} value={category.id}>
                      {category.name}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            {formData.questionType === 1 && (
              <>
                <Grid item xs={12}>
                  <Typography variant="h6" gutterBottom>
                    Seçenekler
                  </Typography>
                  {formData.options.map((option, index) => (
                    <Box key={index} sx={{ display: 'flex', gap: 2, mb: 2 }}>
                      <TextField
                        fullWidth
                        label={`Seçenek ${index + 1}`}
                        value={option.optionText}
                        onChange={(e) => handleOptionChange(index, 'optionText', e.target.value)}
                        required
                      />
                      <FormControl sx={{ minWidth: 120 }}>
                        <InputLabel>Doğru mu?</InputLabel>
                        <Select
                          value={option.isCorrect}
                          onChange={(e) => handleOptionChange(index, 'isCorrect', e.target.value)}
                          label="Doğru mu?"
                        >
                          <MenuItem value={true}>Evet</MenuItem>
                          <MenuItem value={false}>Hayır</MenuItem>
                        </Select>
                      </FormControl>
                      {formData.options.length > 2 && (
                        <IconButton
                          color="error"
                          onClick={() => removeOption(index)}
                        >
                          <DeleteIcon />
                        </IconButton>
                      )}
                    </Box>
                  ))}
                  <Button
                    startIcon={<AddIcon />}
                    onClick={addOption}
                    variant="outlined"
                    sx={{ mt: 1 }}
                  >
                    Seçenek Ekle
                  </Button>
                </Grid>
              </>
            )}

            {formData.questionType === 2 && (
              <Grid item xs={12}>
                <FormControl fullWidth required>
                  <InputLabel>Doğru Cevap</InputLabel>
                  <Select
                    name="correctAnswer"
                    value={formData.correctAnswer}
                    onChange={handleChange}
                    label="Doğru Cevap"
                  >
                    <MenuItem value={true}>Doğru</MenuItem>
                    <MenuItem value={false}>Yanlış</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
            )}

            {formData.questionType === 3 && (
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Doğru Cevap"
                  name="correctAnswer"
                  value={formData.correctAnswer}
                  onChange={handleChange}
                  required
                />
              </Grid>
            )}

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Açıklama"
                name="explanation"
                value={formData.explanation}
                onChange={handleChange}
                multiline
                rows={2}
              />
            </Grid>

            <Grid item xs={12}>
              <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                <Button
                  variant="outlined"
                  onClick={() => navigate('/questions')}
                >
                  İptal
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  disabled={saving}
                >
                  {saving ? 'Kaydediliyor...' : 'Kaydet'}
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Paper>
    </Container>
  );
};

export default EditQuestion; 