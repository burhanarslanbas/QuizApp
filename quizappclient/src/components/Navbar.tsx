import React from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  Container,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import QuizIcon from '@mui/icons-material/Quiz';
import LoginIcon from '@mui/icons-material/Login';

const Navbar: React.FC = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  return (
    <AppBar 
      position="fixed" 
      elevation={0}
      sx={{ 
        backgroundColor: 'rgba(255, 255, 255, 0.98)',
        borderBottom: '1px solid rgba(0, 0, 0, 0.1)',
      }}
    >
      <Container maxWidth="lg">
        <Toolbar disableGutters sx={{ minHeight: 70 }}>
          <Box sx={{ display: 'flex', alignItems: 'center', flexGrow: 1 }}>
            <QuizIcon sx={{ fontSize: 28, color: '#000', mr: 1 }} />
            <Typography
              variant="h6"
              component="div"
              sx={{
                color: '#000',
                fontWeight: 600,
                letterSpacing: '0.5px',
                fontSize: '1.2rem',
              }}
            >
              QuizApp
            </Typography>
          </Box>
          
          <Box sx={{ display: 'flex', gap: 2 }}>
            <Button
              variant="outlined"
              startIcon={<LoginIcon />}
              sx={{
                borderRadius: '30px',
                textTransform: 'none',
                px: 3,
                py: 1,
                borderColor: '#000',
                color: '#000',
                '&:hover': {
                  backgroundColor: '#000',
                  color: '#fff',
                  borderColor: '#000',
                },
              }}
            >
              {isMobile ? 'Giriş' : 'Giriş Yap'}
            </Button>
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default Navbar; 