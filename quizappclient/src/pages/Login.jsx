import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: '',
    password: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    // Burada API isteği yapılabilir
    setTimeout(() => {
      setLoading(false);
      navigate('/');
    }, 1000);
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', justifyContent: 'center', alignItems: 'center', background: '#f7f7f7' }}>
      <div style={{ width: 350, background: '#fff', padding: 32, borderRadius: 12, boxShadow: '0 2px 8px rgba(0,0,0,0.08)' }}>
        <h2 style={{ textAlign: 'center', marginBottom: 24 }}>Giriş Yap</h2>
        {error && <div style={{ color: 'red', marginBottom: 16 }}>{error}</div>}
        <form onSubmit={handleSubmit}>
          <input
            type="email"
            name="email"
            placeholder="E-posta"
            value={formData.email}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: 8, marginBottom: 12, borderRadius: 6, border: '1px solid #ccc' }}
          />
          <input
            type="password"
            name="password"
            placeholder="Şifre"
            value={formData.password}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: 8, marginBottom: 16, borderRadius: 6, border: '1px solid #ccc' }}
          />
          <button
            type="submit"
            disabled={loading}
            style={{ width: '100%', padding: 10, borderRadius: 6, background: '#1976d2', color: '#fff', border: 'none', fontWeight: 600, fontSize: 16, cursor: 'pointer' }}
          >
            {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
          </button>
        </form>
        <div style={{ marginTop: 16, textAlign: 'center' }}>
          Hesabınız yok mu?{' '}
          <button
            onClick={() => navigate('/register')}
            style={{ background: 'none', color: '#1976d2', border: 'none', cursor: 'pointer', textDecoration: 'underline', fontWeight: 600 }}
          >
            Kayıt Ol
          </button>
        </div>
      </div>
    </div>
  );
};

export default Login; 