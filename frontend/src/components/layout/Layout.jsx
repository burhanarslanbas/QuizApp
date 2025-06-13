import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
  Box,
  Drawer,
  AppBar,
  Toolbar,
  List,
  Typography,
  Divider,
  IconButton,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  useTheme,
  Avatar,
  Menu,
  MenuItem,
  Tooltip,
} from '@mui/material';
import {
  Menu as MenuIcon,
  ChevronLeft as ChevronLeftIcon,
  Dashboard as DashboardIcon,
  Quiz as QuizIcon,
  Category as CategoryIcon,
  QuestionAnswer as QuestionIcon,
  People as PeopleIcon,
  Assessment as AssessmentIcon,
  Notifications as NotificationsIcon,
  Settings as SettingsIcon,
  Person as PersonIcon,
  Brightness4 as DarkModeIcon,
  Brightness7 as LightModeIcon,
} from '@mui/icons-material';
import { useToken } from '../../context/TokenContext';
import { useNotification } from '../../context/NotificationContext';
import Sidebar from './Sidebar';

const drawerWidth = 240;

const menuItems = [
  { text: 'Ana Sayfa', icon: <DashboardIcon />, path: '/dashboard', roles: ['admin', 'teacher', 'student'] },
  { text: 'Sınavlar', icon: <QuizIcon />, path: '/quizzes', roles: ['admin', 'teacher', 'student'] },
  { text: 'Kategoriler', icon: <CategoryIcon />, path: '/categories', roles: ['admin', 'teacher', 'student'] },
  { text: 'Sorular', icon: <QuestionIcon />, path: '/questions', roles: ['admin', 'teacher'] },
  { text: 'Soru Havuzları', icon: <QuestionIcon />, path: '/question-pool', roles: ['admin', 'teacher'] },
  { text: 'Kullanıcılar', icon: <PeopleIcon />, path: '/users', roles: ['admin'] },
  { text: 'Raporlar', icon: <AssessmentIcon />, path: '/reports', roles: ['admin', 'teacher'] },
  { text: 'Sonuçlarım', icon: <AssessmentIcon />, path: '/my-results', roles: ['admin', 'teacher', 'student'] },
  { text: 'Bildirimler', icon: <NotificationsIcon />, path: '/notifications', roles: ['admin', 'teacher', 'student'] },
  { text: 'Ayarlar', icon: <SettingsIcon />, path: '/settings', roles: ['admin', 'teacher', 'student'] },
];

const Layout = ({ children }) => {
  const [open, setOpen] = useState(true);
  const [anchorEl, setAnchorEl] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const { user, logout } = useToken();
  const { showNotification } = useNotification();

  // Kullanıcının ilk rolünü al (dizi olarak geliyor)
  const userRole = Array.isArray(user?.roles) && user.roles.length > 0
    ? user.roles[0].toLowerCase()
    : undefined;

  const handleDrawerToggle = () => {
    setOpen(!open);
  };

  const handleProfileMenuOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleProfileMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    logout();
    showNotification('Successfully logged out', 'success');
    navigate('/login');
  };

  const handleProfileClick = () => {
    handleProfileMenuClose();
    navigate('/profile');
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar
        position="fixed"
        sx={{
          zIndex: theme.zIndex.drawer + 1,
          transition: theme.transitions.create(['width', 'margin'], {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
          }),
          ...(open && {
            marginLeft: 220,
            width: `calc(100% - 220px)`,
            transition: theme.transitions.create(['width', 'margin'], {
              easing: theme.transitions.easing.sharp,
              duration: theme.transitions.duration.enteringScreen,
            }),
          }),
        }}
      >
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            onClick={() => setOpen(!open)}
            edge="start"
            sx={{ marginRight: 5 }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
            Quiz App
          </Typography>
          <IconButton
            onClick={handleProfileMenuOpen}
            size="small"
            sx={{ ml: 2 }}
            aria-controls="menu-appbar"
            aria-haspopup="true"
          >
            <Avatar sx={{ width: 32, height: 32 }}>
              {user?.name?.charAt(0) || 'U'}
            </Avatar>
          </IconButton>
          <Menu
            id="menu-appbar"
            anchorEl={anchorEl}
            anchorOrigin={{
              vertical: 'bottom',
              horizontal: 'right',
            }}
            keepMounted
            transformOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            open={Boolean(anchorEl)}
            onClose={handleProfileMenuClose}
          >
            <MenuItem onClick={handleProfileClick}>
              <ListItemIcon>
                <PersonIcon fontSize="small" />
              </ListItemIcon>
              Profile
            </MenuItem>
            <MenuItem onClick={handleLogout}>Logout</MenuItem>
          </Menu>
        </Toolbar>
      </AppBar>
      <Sidebar isOpen={open} setIsOpen={setOpen} />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          width: { sm: `calc(100% - 220px)` },
          mt: 8,
        }}
      >
        {children}
      </Box>
    </Box>
  );
};

export default Layout; 