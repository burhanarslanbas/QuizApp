import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
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
  Checkbox,
  FormControlLabel,
  RadioGroup,
  Radio,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { questionService } from '@/services/questionService';
import { categoryService } from '@/services/categoryService';
import { useNotification } from '@/context/NotificationContext';

const CreateQuestion = () => {
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    questionText: '',
    questionType: 1,
    categoryId: '',
    points: 1,
    orderIndex: 0,
    explanation: '',
    imageUrl: '',
    isActive: true,
    questionRepoId: '',
    options: [{ optionText: '', isCorrect: false, orderIndex: 0 }, { optionText: '', isCorrect: false, orderIndex: 1 }],
  });

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const response = await categoryService.getAllCategories();
      setCategories(response.data);
    } catch (error) {
      showNotification('Kategoriler yüklenirken bir hata oluştu', 'error');
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
      options: [...prev.options, { text: '', isCorrect: false }],
    }));
  };

  const removeOption = (index) => {
    setFormData((prev) => ({
      ...prev,
      options: prev.options.filter((_, i) => i !== index),
    }));
  };

  const handleQuestionTypeChange = (e) => {
    const value = Number(e.target.value);
    setFormData((prev) => ({
      ...prev,
      questionType: value,
      options: value === 1 ? [{ text: '' }, { text: '' }] : [],
      correctAnswers: [],
      correctAnswer: value === 2 ? false : ''
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
      await questionService.createQuestion(payload);
      showNotification('Soru başarıyla oluşturuldu', 'success');
      navigate('/questions');
    } catch (error) {
      showNotification('Soru oluşturulurken bir hata oluştu', 'error');
    }
  };

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Yeni Soru Oluştur
        </Typography>

        <Box component="form" onSubmit={handleSubmit}>
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
                  onChange={handleQuestionTypeChange}
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
                  {formData.options.map((option, idx) => (
                    <Box key={idx} display="flex" alignItems="center" gap={1} mb={1}>
                      <TextField label={`Seçenek ${idx + 1}`} value={option.optionText} onChange={e => {
                        const newOptions = [...formData.options];
                        newOptions[idx].optionText = e.target.value;
                        setFormData({ ...formData, options: newOptions });
                      }} required />
                      <Checkbox checked={option.isCorrect} onChange={e => {
                        const newOptions = [...formData.options];
                        newOptions[idx].isCorrect = e.target.checked;
                        setFormData({ ...formData, options: newOptions });
                      }} />
                      <TextField label="Sıra" type="number" value={option.orderIndex} onChange={e => {
                        const newOptions = [...formData.options];
                        newOptions[idx].orderIndex = Number(e.target.value);
                        setFormData({ ...formData, options: newOptions });
                      }} sx={{ width: 80 }} />
                      <IconButton onClick={() => {
                        const newOptions = formData.options.filter((_, i) => i !== idx);
                        setFormData({ ...formData, options: newOptions });
                      }} disabled={formData.options.length <= 2}><DeleteIcon /></IconButton>
                    </Box>
                  ))}
                  <Button onClick={() => setFormData({ ...formData, options: [...formData.options, { optionText: '', isCorrect: false, orderIndex: formData.options.length }] })}>Seçenek Ekle</Button>
                </Grid>
              </>
            )}

            {formData.questionType === 2 && (
              <Grid item xs={12}>
                <FormControl fullWidth required>
                  <FormControlLabel value={true} control={<Radio />} label="Doğru" />
                  <FormControlLabel value={false} control={<Radio />} label="Yanlış" />
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
                label="Puan"
                type="number"
                value={formData.points}
                onChange={e => setFormData({ ...formData, points: Number(e.target.value) })}
                required
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Sıra"
                type="number"
                value={formData.orderIndex}
                onChange={e => setFormData({ ...formData, orderIndex: Number(e.target.value) })}
                required
              />
            </Grid>

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
              <TextField
                fullWidth
                label="Resim URL"
                name="imageUrl"
                value={formData.imageUrl}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12}>
              <FormControlLabel control={<Checkbox checked={formData.isActive} onChange={handleChange} name="isActive" />} label="Aktif mi?" />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Soru Havuzu ID (opsiyonel)"
                name="questionRepoId"
                value={formData.questionRepoId}
                onChange={handleChange}
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
                  disabled={loading}
                >
                  {loading ? 'Kaydediliyor...' : 'Kaydet'}
                </Button>
              </Box>
            </Grid>
          </Grid>
        </Box>
      </Paper>
    </Container>
  );
};

export default CreateQuestion; 