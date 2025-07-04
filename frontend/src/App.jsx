import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import PrivateRoute from './components/PrivateRoute';
import Layout from './components/Layout';
import Login from './pages/auth/Login';
import Register from './pages/auth/Register';
import Home from './pages/home/Home';
import Dashboard from './pages/dashboard/Dashboard';
import Profile from './pages/profile/Profile';
import NotFound from './pages/NotFound';

// Quiz pages
import QuizList from './pages/quizzes';
import CreateQuiz from './pages/quizzes/create';
import TakeQuiz from './pages/quizzes/take';
import QuizResults from './pages/quizzes/results';
import QuizResultDetail from './pages/quizzes/result-detail';

function App() {
  return (
    <Router>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          
          <Route path="/" element={<Layout />}>
            <Route index element={<Home />} />
            <Route path="dashboard" element={<PrivateRoute><Dashboard /></PrivateRoute>} />
            <Route path="profile" element={<PrivateRoute><Profile /></PrivateRoute>} />
            
            {/* Quiz routes */}
            <Route path="quizzes">
              <Route index element={<PrivateRoute><QuizList /></PrivateRoute>} />
              <Route path="create" element={<PrivateRoute><CreateQuiz /></PrivateRoute>} />
              <Route path=":quizId/take" element={<PrivateRoute><TakeQuiz /></PrivateRoute>} />
              <Route path=":quizId/results" element={<PrivateRoute><QuizResults /></PrivateRoute>} />
              <Route path=":quizId/results/:resultId" element={<PrivateRoute><QuizResultDetail /></PrivateRoute>} />
            </Route>

            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      </AuthProvider>
    </Router>
  );
}

export default App;
