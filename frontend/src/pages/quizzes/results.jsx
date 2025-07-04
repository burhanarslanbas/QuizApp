import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { quizService } from '../../services/quizService';
import { quizResultService } from '../../services/quizResultService';
import { Container, Card, Table, Button, Alert, Spinner, ProgressBar } from 'react-bootstrap';

const QuizResults = () => {
  const { quizId } = useParams();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState(null);
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadData();
  }, [quizId]);

  const loadData = async () => {
    try {
      setLoading(true);
      const [quizData, resultsData] = await Promise.all([
        quizService.getQuiz(quizId),
        quizResultService.getResultsByQuiz(quizId)
      ]);
      setQuiz(quizData);
      setResults(resultsData);
      setError(null);
    } catch (err) {
      setError('Veriler yüklenirken bir hata oluştu.');
      console.error('Data loading error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleViewDetail = (resultId) => {
    navigate(`/quizzes/${quizId}/results/${resultId}`);
  };

  const handleBack = () => {
    navigate('/quizzes');
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

  if (!quiz) {
    return (
      <Container className="py-4">
        <Alert variant="danger">Sınav bulunamadı.</Alert>
      </Container>
    );
  }

  const calculateAverageScore = () => {
    if (results.length === 0) return 0;
    const total = results.reduce((sum, result) => sum + result.score, 0);
    return total / results.length;
  };

  const calculatePassRate = () => {
    if (results.length === 0) return 0;
    const passed = results.filter(result => result.isPassed).length;
    return (passed / results.length) * 100;
  };

  return (
    <Container className="py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>{quiz.title} - Sonuçlar</h2>
        <Button variant="secondary" onClick={handleBack}>
          Geri Dön
        </Button>
      </div>

      {error && (
        <Alert variant="danger" className="mb-4">
          {error}
        </Alert>
      )}

      <Row className="mb-4">
        <Col md={4}>
          <Card>
            <Card.Body>
              <Card.Title>Toplam Katılım</Card.Title>
              <Card.Text className="h3">{results.length}</Card.Text>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card>
            <Card.Body>
              <Card.Title>Ortalama Puan</Card.Title>
              <Card.Text className="h3">{calculateAverageScore().toFixed(1)}</Card.Text>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card>
            <Card.Body>
              <Card.Title>Geçme Oranı</Card.Title>
              <Card.Text className="h3">{calculatePassRate().toFixed(1)}%</Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <Card>
        <Card.Body>
          <Card.Title>Sonuç Listesi</Card.Title>
          <Table responsive hover>
            <thead>
              <tr>
                <th>Öğrenci</th>
                <th>Puan</th>
                <th>Durum</th>
                <th>Süre</th>
                <th>Tarih</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              {results.map(result => (
                <tr key={result.id}>
                  <td>{result.studentName}</td>
                  <td>
                    <ProgressBar
                      now={result.score}
                      max={100}
                      label={`${result.score}%`}
                      variant={result.isPassed ? 'success' : 'danger'}
                    />
                  </td>
                  <td>
                    <span className={`badge ${result.isPassed ? 'bg-success' : 'bg-danger'}`}>
                      {result.isPassed ? 'Geçti' : 'Kaldı'}
                    </span>
                  </td>
                  <td>{result.timeSpent} dakika</td>
                  <td>{new Date(result.createdDate).toLocaleString()}</td>
                  <td>
                    <Button
                      variant="outline-primary"
                      size="sm"
                      onClick={() => handleViewDetail(result.id)}
                    >
                      Detay
                    </Button>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        </Card.Body>
      </Card>
    </Container>
  );
};

export default QuizResults; 