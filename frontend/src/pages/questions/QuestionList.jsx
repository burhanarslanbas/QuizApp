import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { questionService } from '@/services/questionService';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Select } from '@/components/ui/select';
import { DataTable } from '@/components/common/DataTable';
import { toast } from 'react-hot-toast';

const QuestionList = () => {
  const [questions, setQuestions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filters, setFilters] = useState({
    search: '',
    categoryId: '',
    questionType: '',
  });
  const navigate = useNavigate();

  useEffect(() => {
    loadQuestions();
  }, [filters]);

  const loadQuestions = async () => {
    try {
      setLoading(true);
      const response = await questionService.getAllQuestions(filters);
      setQuestions(response.data);
    } catch (error) {
      toast.error('Sorular yüklenirken bir hata oluştu');
      console.error('Error loading questions:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu soruyu silmek istediğinizden emin misiniz?')) {
      try {
        await questionService.deleteQuestion(id);
        toast.success('Soru başarıyla silindi');
        loadQuestions();
      } catch (error) {
        toast.error('Soru silinirken bir hata oluştu');
        console.error('Error deleting question:', error);
      }
    }
  };

  const columns = [
    {
      header: 'Soru Metni',
      accessorKey: 'questionText',
      cell: ({ row }) => (
        <div className="max-w-md truncate" title={row.original.questionText}>
          {row.original.questionText}
        </div>
      ),
    },
    {
      header: 'Soru Tipi',
      accessorKey: 'questionType',
      cell: ({ row }) => {
        const types = {
          MULTIPLE_CHOICE: 'Çoktan Seçmeli',
          TRUE_FALSE: 'Doğru/Yanlış',
          SHORT_ANSWER: 'Kısa Cevap',
        };
        return types[row.original.questionType] || row.original.questionType;
      },
    },
    {
      header: 'Kategori',
      accessorKey: 'categoryName',
    },
    {
      header: 'Durum',
      accessorKey: 'isActive',
      cell: ({ row }) => (
        <span className={`px-2 py-1 rounded-full text-xs ${
          row.original.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
        }`}>
          {row.original.isActive ? 'Aktif' : 'Pasif'}
        </span>
      ),
    },
    {
      header: 'İşlemler',
      cell: ({ row }) => (
        <div className="flex gap-2">
          <Button
            variant="outline"
            size="sm"
            onClick={() => navigate(`/questions/edit/${row.original.id}`)}
          >
            Düzenle
          </Button>
          <Button
            variant="destructive"
            size="sm"
            onClick={() => handleDelete(row.original.id)}
          >
            Sil
          </Button>
        </div>
      ),
    },
  ];

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Sorular</h1>
        <Button onClick={() => navigate('/questions/create')}>
          Yeni Soru Ekle
        </Button>
      </div>

      <div className="bg-white rounded-lg shadow p-4 mb-6">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <Input
            placeholder="Soru ara..."
            value={filters.search}
            onChange={(e) => setFilters({ ...filters, search: e.target.value })}
          />
          <Select
            value={filters.categoryId}
            onChange={(e) => setFilters({ ...filters, categoryId: e.target.value })}
          >
            <option value="">Tüm Kategoriler</option>
            {/* Kategoriler buraya eklenecek */}
          </Select>
          <Select
            value={filters.questionType}
            onChange={(e) => setFilters({ ...filters, questionType: e.target.value })}
          >
            <option value="">Tüm Soru Tipleri</option>
            <option value="MULTIPLE_CHOICE">Çoktan Seçmeli</option>
            <option value="TRUE_FALSE">Doğru/Yanlış</option>
            <option value="SHORT_ANSWER">Kısa Cevap</option>
          </Select>
        </div>
      </div>

      <DataTable
        columns={columns}
        data={questions}
        loading={loading}
        pagination
        pageSize={10}
      />
    </div>
  );
};

export default QuestionList; 