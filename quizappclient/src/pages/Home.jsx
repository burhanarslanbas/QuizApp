import React from 'react';
import { useNavigate } from 'react-router-dom';

const Home = () => {
  const navigate = useNavigate();

  return (
    <div style={{ minHeight: '100vh', background: '#f7f7f7', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
      <div style={{ background: '#fff', padding: 40, borderRadius: 16, boxShadow: '0 2px 8px rgba(0,0,0,0.08)', maxWidth: 420, width: '100%', textAlign: 'center' }}>
        <h1 style={{ fontSize: 32, marginBottom: 16 }}>QuizApp'e Hoş Geldiniz</h1>
        <p style={{ color: '#555', marginBottom: 32 }}>
          Modern ve sade bir quiz platformu ile öğrenmeye hemen başlayın.
        </p>
        <button
          onClick={() => navigate('/register')}
          style={{ width: '100%', padding: 12, borderRadius: 8, background: '#1976d2', color: '#fff', border: 'none', fontWeight: 600, fontSize: 18, cursor: 'pointer', marginBottom: 12 }}
        >
          Kayıt Ol
        </button>
        <button
          onClick={() => navigate('/login')}
          style={{ width: '100%', padding: 12, borderRadius: 8, background: '#fff', color: '#1976d2', border: '2px solid #1976d2', fontWeight: 600, fontSize: 18, cursor: 'pointer' }}
        >
          Giriş Yap
        </button>
      </div>
    </div>
  );
};

export default Home; 