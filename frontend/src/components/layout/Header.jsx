import { Notifications as NotificationsIcon } from '@mui/icons-material';
import { 
  AppBar, 
  Toolbar, 
  Typography, 
  IconButton, 
  Menu, 
  MenuItem, 
  Avatar,
  Box
} from '@mui/material';
import { useState } from 'react';
import { useLocation } from 'react-router-dom';
import { useToken } from '../../context/TokenContext';

// Sayfa başlıklarını tanımlayan obje
const pageTitles = {
  '/': 'Ana Sayfa',
  '/dashboard': 'Dashboard',
  '/users': 'Kullanıcılar',
  '/quizzes': 'Sınavlar',
  '/reports': 'Raporlar',
  '/settings': 'Ayarlar'
};

// Baş harfleri üretmek için yardımcı fonksiyon
const getInitials = (user) => {
  const name = user?.fullName || user?.name || user?.userName || user?.email || '';
  const parts = name.trim().split(' ');
  if (parts.length === 1) return parts[0][0]?.toUpperCase() || '';
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
};

// Üst kısımdaki header bileşeni
const Header = () => {
  const [anchorEl, setAnchorEl] = useState(null);
  const location = useLocation();
  const { user, logout } = useToken();
  // Kullanıcının ilk rolünü al (dizi olarak geliyor)
  const userRole = Array.isArray(user?.roles) && user.roles.length > 0
    ? user.roles[0].toLowerCase()
    : undefined;
  console.log('Header user:', user);
  console.log('Header userRole:', userRole);
  
  // Menü açma/kapama işlemleri
  const handleMenu = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  // Çıkış yapma fonksiyonu
  const handleLogout = () => {
    logout();
    handleClose();
  };

  return (
    <AppBar position="static" color="default" elevation={0}>
      <Toolbar>
        {/* Sol kısım - Sayfa başlığı */}
        <Typography variant="h6" color="inherit" sx={{ flexGrow: 1 }}>
          {pageTitles[location.pathname] || 'QuizApp'}
        </Typography>
        
        {/* Sağ kısım - Bildirim ve kullanıcı menüsü */}
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          {/* Bildirim ikonu */}
          <IconButton color="inherit">
            <NotificationsIcon />
          </IconButton>
          
          {/* Kullanıcı menüsü */}
          <IconButton
            onClick={handleMenu}
            sx={{ p: 0 }}
          >
            <Avatar
              alt={user?.fullName || user?.name || 'User'}
              src={user?.avatar || ''}
              sx={{ width: 32, height: 32 }}
            >
              {!user?.avatar && getInitials(user)}
            </Avatar>
          </IconButton>
          
          <Menu
            anchorEl={anchorEl}
            open={Boolean(anchorEl)}
            onClose={handleClose}
            onClick={handleClose}
            transformOrigin={{ horizontal: 'right', vertical: 'top' }}
            anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
          >
            <MenuItem onClick={handleLogout}>Çıkış Yap</MenuItem>
          </Menu>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header; 