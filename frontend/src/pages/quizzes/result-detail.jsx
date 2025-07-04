import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { quizService } from '../../services/quizService';
import { quizResultService } from '../../services/quizResultService';
import { Container, Card, Button, Alert, Spinner, ProgressBar, ListGroup } from 'react-bootstrap';

const QuizResultDetail = () => {
  const { quizId, resultId } = useParams();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState(null);
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadData();
  }, [quizId, resultId]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [quizData, resultData] = await Promise.all([
        quizService.getQuiz(quizId),
        quizResultService.getQuizResult(resultId)
      ]);
      setQuiz(quizData);
      setResult(resultData);
      setError(null);
    } catch (err) {
      setError('Veriler yüklenirken bir hata oluştu.');
      console.error('Data loading error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleBack = () => {
    navigate(`/quizzes/${quizId}/results`);
  };

  if (loading) {
    return (
      <Container className="d-flex justify-content-center align-items-center" style={{ minHeight: '60vh' }}>
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Yükleniyor...</span>
        </Spinner>
      </Container>
    );
  }

  if (!quiz || !result) {
    return (
      <Container className="py-4">
        <Alert variant="danger">Sonuç bulunamadı.</Alert>
      </Container>
    );
  }

  return (
    <Container className="py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>{quiz.title} - Sonuç Detayı</h2>
        <Button variant="secondary" onClick={handleBack}>
          Geri Dön
        </Button>
      </div>

      {error && (
        <Alert variant="danger" className="mb-4">
          {error}
        </Alert>
      )}

      <Card className="mb-4">
        <Card.Body>
          <Card.Title>Genel Bilgiler</Card.Title>
          <div className="row">
            <div className="col-md-6">
              <p><strong>Öğrenci:</strong> {result.studentName}</p>
              <p><strong>Tarih:</strong> {new Date(result.createdDate).toLocaleString()}</p>
              <p><strong>Süre:</strong> {result.timeSpent} dakika</p>
            </div>
            <div className="col-md-6">
              <p><strong>Puan:</strong></p>
              <ProgressBar
                now={result.score}
                max={100}
                label={`${result.score}%`}
                variant={result.isPassed ? 'success' : 'danger'}
                className="mb-2"
              />
              <p>
                <strong>Durum:</strong>{' '}
                <span className={`badge ${result.isPassed ? 'bg-success' : 'bg-danger'}`}>
                  {result.isPassed ? 'Geçti' : 'Kaldı'}
                </span>
              </p>
            </div>
          </div>
        </Card.Body>
      </Card>

      <Card>
        <Card.Body>
          <Card.Title>Soru Detayları</Card.Title>
          <ListGroup variant="flush">
            {result.answers.map((answer, index) => {
              const question = quiz.questions.find(q => q.id === answer.questionId);
              const selectedOption = question?.options.find(o => o.id === answer.selectedOptionId);
              const isCorrect = selectedOption?.isCorrect;

              return (
                <ListGroup.Item key={answer.questionId}>
                  <div className="d-flex justify-content-between align-items-start mb-2">
                    <h5 className="mb-0">Soru {index + 1}</h5>
                    <span className={`badge ${isCorrect ? 'bg-success' : 'bg-danger'}`}>
                      {isCorrect ? 'Doğru' : 'Yanlış'}
                    </span>
                  </div>
                  <p className="mb-2">{question?.text}</p>
                  <div className="ms-3">
                    <p className="mb-1">
                      <strong>Seçilen Cevap:</strong>{' '}
                      <span className={isCorrect ? 'text-success' : 'text-danger'}>
                        {selectedOption?.text}
                      </span>
                    </p>
                    {!isCorrect && (
                      <p className="mb-0">
                        <strong>Doğru Cevap:</strong>{' '}
                        <span className="text-success">
                          {question?.options.find(o => o.isCorrect)?.text}
                        </span>
                      </p>
                    )}
                  </div>
                </ListGroup.Item>
              );
            })}
          </ListGroup>
        </Card.Body>
      </Card>
    </Container>
  );
};

export default QuizResultDetail; 