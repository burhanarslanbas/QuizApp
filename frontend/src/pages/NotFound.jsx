import React from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Button,
  Container,
} from '@mui/material';
import { Home as HomeIcon } from '@mui/icons-material';

const NotFound = () => {
  const navigate = useNavigate();

  return (
    <Container maxWidth="md">
      <Box
        sx={{
          minHeight: '100vh',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          textAlign: 'center',
          py: 8,
        }}
      >
        <Typography
          variant="h1"
          component="h1"
          sx={{
            fontSize: { xs: '6rem', md: '8rem' },
            fontWeight: 'bold',
            color: 'primary.main',
            mb: 2,
          }}
        >
          404
        </Typography>
        <Typography
          variant="h4"
          component="h2"
          gutterBottom
          sx={{ mb: 2 }}
        >
          Sayfa Bulunamadı
        </Typography>
        <Typography
          variant="body1"
          color="text.secondary"
          sx={{ mb: 4, maxWidth: 400 }}
        >
          Aradığınız sayfa mevcut değil veya taşınmış olabilir.
        </Typography>
        <Button
          variant="contained"
          size="large"
          startIcon={<HomeIcon />}
          onClick={() => navigate('/')}
        >
          Ana Sayfaya Dön
        </Button>
      </Box>
    </Container>
  );
};

export default NotFound; 