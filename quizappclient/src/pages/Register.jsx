import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Register = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    password: '',
    confirmPassword: '',
    userType: 'student',
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
    if (formData.password !== formData.confirmPassword) {
      setError('Şifreler eşleşmiyor.');
      setLoading(false);
      return;
    }
    // Burada API isteği yapılabilir
    setTimeout(() => {
      setLoading(false);
      navigate('/login');
    }, 1000);
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', justifyContent: 'center', alignItems: 'center', background: '#f7f7f7' }}>
      <div style={{ width: 350, background: '#fff', padding: 32, borderRadius: 12, boxShadow: '0 2px 8px rgba(0,0,0,0.08)' }}>
        <h2 style={{ textAlign: 'center', marginBottom: 24 }}>Kayıt Ol</h2>
        {error && <div style={{ color: 'red', marginBottom: 16 }}>{error}</div>}
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="firstName"
            placeholder="Ad"
            value={formData.firstName}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: 8, marginBottom: 12, borderRadius: 6, border: '1px solid #ccc' }}
          />
          <input
            type="text"
            name="lastName"
            placeholder="Soyad"
            value={formData.lastName}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: 8, marginBottom: 12, borderRadius: 6, border: '1px solid #ccc' }}
          />
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
            type="text"
            name="phone"
            placeholder="Telefon"
            value={formData.phone}
            onChange={handleChange}
            style={{ width: '100%', padding: 8, marginBottom: 12, borderRadius: 6, border: '1px solid #ccc' }}
          />
          <select
            name="userType"
            value={formData.userType}
            onChange={handleChange}
            style={{ width: '100%', padding: 8, marginBottom: 12, borderRadius: 6, border: '1px solid #ccc' }}
          >
            <option value="student">Öğrenci</option>
            <option value="teacher">Öğretmen</option>
          </select>
          <input
            type="password"
            name="password"
            placeholder="Şifre"
            value={formData.password}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: 8, marginBottom: 12, borderRadius: 6, border: '1px solid #ccc' }}
          />
          <input
            type="password"
            name="confirmPassword"
            placeholder="Şifre Tekrar"
            value={formData.confirmPassword}
            onChange={handleChange}
            required
            style={{ width: '100%', padding: 8, marginBottom: 16, borderRadius: 6, border: '1px solid #ccc' }}
          />
          <button
            type="submit"
            disabled={loading}
            style={{ width: '100%', padding: 10, borderRadius: 6, background: '#1976d2', color: '#fff', border: 'none', fontWeight: 600, fontSize: 16, cursor: 'pointer' }}
          >
            {loading ? 'Kaydediliyor...' : 'Kayıt Ol'}
          </button>
        </form>
        <div style={{ marginTop: 16, textAlign: 'center' }}>
          Zaten hesabınız var mı?{' '}
          <button
            onClick={() => navigate('/login')}
            style={{ background: 'none', color: '#1976d2', border: 'none', cursor: 'pointer', textDecoration: 'underline', fontWeight: 600 }}
          >
            Giriş Yap
          </button>
        </div>
      </div>
    </div>
  );
};

export default Register; 