import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { questionService } from '../../services/questionService';
import { categoryService } from '../../services/categoryService';
import { Button } from '../../components/ui/button';
import { Input } from '../../components/ui/input';
import { Select } from '../../components/ui/select';
import { Textarea } from '../../components/ui/textarea';
import { Card } from '../../components/ui/card';
import { toast } from 'react-hot-toast';

const QuestionForm = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [formData, setFormData] = useState({
    questionText: '',
    questionType: 'MULTIPLE_CHOICE',
    categoryId: '',
    isActive: true,
    options: [{ text: '', isCorrect: false }],
  });

  useEffect(() => {
    loadCategories();
    if (id) {
      loadQuestion();
    }
  }, [id]);

  const loadCategories = async () => {
    try {
      const response = await categoryService.getAllCategories();
      setCategories(response.data);
    } catch (error) {
      toast.error('Kategoriler yüklenirken bir hata oluştu');
      console.error('Error loading categories:', error);
    }
  };

  const loadQuestion = async () => {
    try {
      setLoading(true);
      const response = await questionService.getQuestion(id);
      setFormData(response.data);
    } catch (error) {
      toast.error('Soru yüklenirken bir hata oluştu');
      console.error('Error loading question:', error);
      navigate('/questions');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      if (id) {
        await questionService.updateQuestion(id, formData);
        toast.success('Soru başarıyla güncellendi');
      } else {
        await questionService.createQuestion(formData);
        toast.success('Soru başarıyla oluşturuldu');
      }
      navigate('/questions');
    } catch (error) {
      toast.error('Soru kaydedilirken bir hata oluştu');
      console.error('Error saving question:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleOptionChange = (index, field, value) => {
    const newOptions = [...formData.options];
    newOptions[index] = { ...newOptions[index], [field]: value };
    setFormData({ ...formData, options: newOptions });
  };

  const addOption = () => {
    setFormData({
      ...formData,
      options: [...formData.options, { text: '', isCorrect: false }],
    });
  };

  const removeOption = (index) => {
    const newOptions = formData.options.filter((_, i) => i !== index);
    setFormData({ ...formData, options: newOptions });
  };

  if (loading && id) {
    return <div>Yükleniyor...</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <Card className="p-6">
        <h1 className="text-2xl font-bold mb-6">
          {id ? 'Soruyu Düzenle' : 'Yeni Soru Ekle'}
        </h1>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <label className="block text-sm font-medium mb-2">Soru Metni</label>
            <Textarea
              value={formData.questionText}
              onChange={(e) => setFormData({ ...formData, questionText: e.target.value })}
              required
              className="w-full"
              rows={3}
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium mb-2">Soru Tipi</label>
              <Select
                value={formData.questionType}
                onChange={(e) => setFormData({ ...formData, questionType: e.target.value })}
                required
              >
                <option value="MULTIPLE_CHOICE">Çoktan Seçmeli</option>
                <option value="TRUE_FALSE">Doğru/Yanlış</option>
                <option value="SHORT_ANSWER">Kısa Cevap</option>
              </Select>
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">Kategori</label>
              <Select
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                required
              >
                <option value="">Kategori Seçin</option>
                {categories.map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.name}
                  </option>
                ))}
              </Select>
            </div>
          </div>

          {formData.questionType === 'MULTIPLE_CHOICE' && (
            <div className="space-y-4">
              <div className="flex justify-between items-center">
                <label className="block text-sm font-medium">Seçenekler</label>
                <Button type="button" onClick={addOption}>
                  Seçenek Ekle
                </Button>
              </div>

              {formData.options.map((option, index) => (
                <div key={index} className="flex gap-4 items-center">
                  <Input
                    value={option.text}
                    onChange={(e) => handleOptionChange(index, 'text', e.target.value)}
                    placeholder="Seçenek metni"
                    required
                    className="flex-1"
                  />
                  <div className="flex items-center gap-2">
                    <input
                      type="checkbox"
                      checked={option.isCorrect}
                      onChange={(e) => handleOptionChange(index, 'isCorrect', e.target.checked)}
                      className="h-4 w-4"
                    />
                    <span className="text-sm">Doğru Cevap</span>
                  </div>
                  {formData.options.length > 1 && (
                    <Button
                      type="button"
                      variant="destructive"
                      size="sm"
                      onClick={() => removeOption(index)}
                    >
                      Sil
                    </Button>
                  )}
                </div>
              ))}
            </div>
          )}

          <div className="flex items-center gap-2">
            <input
              type="checkbox"
              id="isActive"
              checked={formData.isActive}
              onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
              className="h-4 w-4"
            />
            <label htmlFor="isActive" className="text-sm">
              Aktif
            </label>
          </div>

          <div className="flex gap-4">
            <Button type="submit" disabled={loading}>
              {id ? 'Güncelle' : 'Kaydet'}
            </Button>
            <Button
              type="button"
              variant="outline"
              onClick={() => navigate('/questions')}
            >
              İptal
            </Button>
          </div>
        </form>
      </Card>
    </div>
  );
};

export default QuestionForm; 