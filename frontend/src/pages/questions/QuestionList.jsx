import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Typography,
  IconButton,
  Paper,
  CircularProgress
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { questionService } from '../../services/questionService';
import { useNotification } from '../../context/NotificationContext';

const QuestionList = () => {
  const navigate = useNavigate();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(false);
  const [questions, setQuestions] = useState([]);

  const loadQuestions = async () => {
    try {
      setLoading(true);
      const response = await questionService.getAllQuestions();
      // Support both array and object (with items) response
      const questionsArray = Array.isArray(response.data)
        ? response.data
        : (Array.isArray(response.data?.items)
            ? response.data.items
            : (response.data?.data || []));
      setQuestions(questionsArray);
    } catch (error) {
      showNotification('Sorular yüklenirken bir hata oluştu', 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadQuestions();
    // eslint-disable-next-line
  }, []);

  const handleDelete = async (id) => {
    if (window.confirm('Bu soruyu silmek istediğinizden emin misiniz?')) {
      try {
        await questionService.deleteQuestion(id);
        showNotification('Soru başarıyla silindi', 'success');
        loadQuestions();
      } catch (error) {
        showNotification('Soru silinirken bir hata oluştu', 'error');
      }
    }
  };

  return (
    <Box>
      <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h5">Sorular</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => navigate('/questions/create')}
        >
          Yeni Soru
        </Button>
      </Box>
      <Paper sx={{ p: 2 }}>
        {loading ? (
          <Box display="flex" justifyContent="center" alignItems="center" minHeight={200}>
            <CircularProgress />
          </Box>
        ) : questions.length === 0 ? (
          <Typography align="center" color="text.secondary" sx={{ py: 4 }}>
            Hiç soru bulunamadı.
          </Typography>
        ) : (
          <Box sx={{ overflowX: 'auto' }}>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Soru</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Tip</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Puan</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Sıra</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Açıklama</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Resim</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Aktif</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Seçenekler</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Oluşturulma</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>Güncellenme</th>
                  <th style={{ padding: 8, borderBottom: '1px solid #eee' }}>İşlemler</th>
                </tr>
              </thead>
              <tbody>
                {questions.map((q) => (
                  <tr key={q.id}>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.questionText}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.questionType === 1 ? 'Çoktan Seçmeli' : q.questionType === 2 ? 'Doğru/Yanlış' : 'Kısa Cevap'}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.points}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.orderIndex}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.explanation}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.imageUrl ? <a href={q.imageUrl} target="_blank" rel="noopener noreferrer">Görüntüle</a> : '-'}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.isActive ? 'Evet' : 'Hayır'}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{(q.options || []).map(opt => opt.optionText).join(', ')}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.createdAt ? new Date(q.createdAt).toLocaleString() : '-'}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>{q.updatedAt ? new Date(q.updatedAt).toLocaleString() : '-'}</td>
                    <td style={{ padding: 8, borderBottom: '1px solid #eee' }}>
                      <IconButton size="small" onClick={() => navigate(`/questions/edit/${q.id}`)}><EditIcon /></IconButton>
                      <IconButton size="small" onClick={() => handleDelete(q.id)}><DeleteIcon /></IconButton>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Box>
        )}
      </Paper>
    </Box>
  );
};

export default QuestionList; 