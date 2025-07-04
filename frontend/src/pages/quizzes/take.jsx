import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { quizService } from '../../services/quizService';
import { quizResultService } from '../../services/quizResultService';
import { Container, Card, Button, ProgressBar, Alert, Spinner } from 'react-bootstrap';

const TakeQuiz = () => {
  const { quizId } = useParams();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState(null);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [answers, setAnswers] = useState({});
  const [timeLeft, setTimeLeft] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    loadQuiz();
  }, [quizId]);

  useEffect(() => {
    if (timeLeft > 0) {
      const timer = setInterval(() => {
        setTimeLeft(prev => prev - 1);
      }, 1000);
      return () => clearInterval(timer);
    } else if (timeLeft === 0 && quiz) {
      handleSubmit();
    }
  }, [timeLeft]);

  const loadQuiz = async () => {
    try {
      setLoading(true);
      const data = await quizService.getQuiz(quizId);
      setQuiz(data);
      setTimeLeft(data.timeLimit * 60); // Convert minutes to seconds
      setAnswers({});
    } catch (err) {
      setError('Sınav yüklenirken bir hata oluştu.');
      console.error('Quiz loading error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleAnswer = (questionId, optionId) => {
    setAnswers(prev => ({
      ...prev,
      [questionId]: optionId
    }));
  };

  const handleNext = () => {
    if (currentQuestion < quiz.questions.length - 1) {
      setCurrentQuestion(prev => prev + 1);
    }
  };

  const handlePrevious = () => {
    if (currentQuestion > 0) {
      setCurrentQuestion(prev => prev - 1);
    }
  };

  const handleSubmit = async () => {
    try {
      setSubmitting(true);
      const result = await quizResultService.createQuizResult({
        quizId,
        answers: Object.entries(answers).map(([questionId, optionId]) => ({
          questionId,
          selectedOptionId: optionId
        }))
      });
      navigate(`/quizzes/${quizId}/results/${result.id}`);
    } catch (err) {
      setError('Sınav sonuçları kaydedilirken bir hata oluştu.');
      console.error('Quiz submission error:', err);
    } finally {
      setSubmitting(false);
    }
  };

  const formatTime = (seconds) => {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
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

  const question = quiz.questions[currentQuestion];
  const progress = ((currentQuestion + 1) / quiz.questions.length) * 100;

  return (
    <Container className="py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>{quiz.title}</h2>
        <div className="text-end">
          <div className="h4 mb-0">{formatTime(timeLeft)}</div>
          <small>Kalan Süre</small>
        </div>
      </div>

      {error && (
        <Alert variant="danger" className="mb-4">
          {error}
        </Alert>
      )}

      <ProgressBar now={progress} className="mb-4" label={`${Math.round(progress)}%`} />

      <Card className="mb-4">
        <Card.Body>
          <Card.Title>
            Soru {currentQuestion + 1} / {quiz.questions.length}
          </Card.Title>
          <Card.Text className="mb-4">{question.text}</Card.Text>

          <div className="d-flex flex-column gap-2">
            {question.options.map(option => (
              <Button
                key={option.id}
                variant={answers[question.id] === option.id ? 'primary' : 'outline-primary'}
                className="text-start"
                onClick={() => handleAnswer(question.id, option.id)}
              >
                {option.text}
              </Button>
            ))}
          </div>
        </Card.Body>
      </Card>

      <div className="d-flex justify-content-between">
        <Button
          variant="secondary"
          onClick={handlePrevious}
          disabled={currentQuestion === 0}
        >
          Önceki Soru
        </Button>
        {currentQuestion < quiz.questions.length - 1 ? (
          <Button variant="primary" onClick={handleNext}>
            Sonraki Soru
          </Button>
        ) : (
          <Button
            variant="success"
            onClick={handleSubmit}
            disabled={submitting}
          >
            {submitting ? 'Gönderiliyor...' : 'Sınavı Bitir'}
          </Button>
        )}
      </div>
    </Container>
  );
};

export default TakeQuiz; 