import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { quizService } from '../../services/quizService';
import { Button, Card, Container, Row, Col, Spinner, Alert } from 'react-bootstrap';
import { useAuth } from '../../contexts/AuthContext';

const QuizList = () => {
  const [quizzes, setQuizzes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const { user } = useAuth();

  useEffect(() => {
    loadQuizzes();
  }, []);

  const loadQuizzes = async () => {
    try {
      setLoading(true);
      const data = await quizService.getQuizzes();
      setQuizzes(data);
      setError(null);
    } catch (err) {
      setError('Sınavlar yüklenirken bir hata oluştu.');
      console.error('Quiz loading error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateQuiz = () => {
    navigate('/quizzes/create');
  };

  const handleStartQuiz = (quizId) => {
    navigate(`/quizzes/${quizId}/take`);
  };

  const handleViewResults = (quizId) => {
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

  return (
    <Container className="py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Sınavlar</h2>
        {user?.roles?.includes('Teacher') && (
          <Button variant="primary" onClick={handleCreateQuiz}>
            Yeni Sınav Oluştur
          </Button>
        )}
      </div>

      {error && (
        <Alert variant="danger" className="mb-4">
          {error}
        </Alert>
      )}

      <Row>
        {quizzes.map((quiz) => (
          <Col key={quiz.id} md={4} className="mb-4">
            <Card>
              <Card.Body>
                <Card.Title>{quiz.title}</Card.Title>
                <Card.Text>{quiz.description}</Card.Text>
                <div className="d-flex justify-content-between">
                  <Button variant="primary" onClick={() => handleStartQuiz(quiz.id)}>
                    Sınava Başla
                  </Button>
                  <Button variant="outline-secondary" onClick={() => handleViewResults(quiz.id)}>
                    Sonuçlar
                  </Button>
                </div>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </Container>
  );
};

export default QuizList; 