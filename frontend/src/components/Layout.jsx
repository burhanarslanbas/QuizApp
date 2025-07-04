import React from 'react';
import { Outlet, Link, useNavigate } from 'react-router-dom';
import { Navbar, Nav, Container, Button } from 'react-bootstrap';
import { useAuth } from '../contexts/AuthContext';

const Layout = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <>
      <Navbar bg="dark" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand as={Link} to="/">Quiz App</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link as={Link} to="/">Ana Sayfa</Nav.Link>
              <Nav.Link as={Link} to="/quizzes">Sınavlar</Nav.Link>
              {user?.roles?.includes('Teacher') && (
                <Nav.Link as={Link} to="/quizzes/create">Yeni Sınav</Nav.Link>
              )}
              <Nav.Link as={Link} to="/dashboard">Dashboard</Nav.Link>
            </Nav>
            <Nav>
              {user ? (
                <>
                  <Nav.Link as={Link} to="/profile">Profil</Nav.Link>
                  <Button variant="outline-light" onClick={handleLogout}>
                    Çıkış Yap
                  </Button>
                </>
              ) : (
                <>
                  <Nav.Link as={Link} to="/login">Giriş Yap</Nav.Link>
                  <Nav.Link as={Link} to="/register">Kayıt Ol</Nav.Link>
                </>
              )}
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>

      <main>
        <Outlet />
      </main>

      <footer className="bg-dark text-light py-4 mt-auto">
        <Container>
          <div className="text-center">
            <p className="mb-0">&copy; 2024 Quiz App. Tüm hakları saklıdır.</p>
          </div>
        </Container>
      </footer>
    </>
  );
};

export default Layout; 