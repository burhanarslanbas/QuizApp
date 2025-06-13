import React from 'react';
import {
  Home as HomeIcon,
  Description as DocumentTextIcon,
  Add as AddIcon,
  Category as CategoryIcon,
  AddBox as AddBoxIcon,
  Visibility as VisibilityIcon,
  Group as GroupIcon,
  SupervisorAccount as SupervisorAccountIcon,
  BarChart as ChartBarIcon,
  ListAlt as ListAltIcon,
  NotificationsActive as NotificationsActiveIcon,
  ManageAccounts as ManageAccountsIcon,
  Settings as CogIcon,
  Person as PersonIcon,
  LibraryBooks as LibraryBooksIcon,
  ChevronLeft as ChevronLeftIcon,
  ExpandLess,
  ExpandMore
} from '@mui/icons-material';
import {
  Drawer,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  IconButton,
  Typography,
  Box,
  ListItemButton,
  Collapse
} from '@mui/material';
import { useLocation, useNavigate } from 'react-router-dom';
import { useToken } from '../../context/TokenContext';

// Menü öğelerinin tanımlandığı array (rol bazlı)
const menuItems = [
  {
    id: 'dashboard',
    label: 'Ana Sayfa',
    icon: HomeIcon,
    path: '/dashboard',
    roles: ['admin', 'teacher', 'student'],
  },
  {
    id: 'quizzes',
    label: 'Sınavlar',
    icon: DocumentTextIcon,
    roles: ['admin', 'teacher', 'student'],
    children: [
      {
        id: 'quizzes-list',
        label: 'Sınavları Görüntüle',
        path: '/quizzes',
        icon: VisibilityIcon,
        roles: ['admin', 'teacher', 'student'],
      },
      {
        id: 'quizzes-create',
        label: 'Sınav Oluştur',
        path: '/quizzes/create',
        icon: AddIcon,
        roles: ['admin', 'teacher'],
      },
      {
        id: 'quizzes-join',
        label: 'Sınava Katıl',
        path: '/quizzes/join',
        icon: AddBoxIcon,
        roles: ['admin', 'teacher', 'student'],
      },
    ],
  },
  {
    id: 'categories',
    label: 'Kategoriler',
    icon: CategoryIcon,
    path: '/categories',
    roles: ['admin', 'teacher', 'student'],
  },
  {
    id: 'questions',
    label: 'Sorular',
    icon: LibraryBooksIcon,
    path: '/questions',
    roles: ['admin', 'teacher'],
  },
  {
    id: 'question-pool',
    label: 'Soru Havuzları',
    icon: AddBoxIcon,
    path: '/question-pool',
    roles: ['admin', 'teacher'],
  },
  {
    id: 'users',
    label: 'Kullanıcılar',
    icon: GroupIcon,
    path: '/users',
    roles: ['admin'],
  },
  {
    id: 'reports',
    label: 'Raporlar',
    icon: ChartBarIcon,
    path: '/reports',
    roles: ['admin', 'teacher'],
  },
  {
    id: 'my-results',
    label: 'Sonuçlarım',
    icon: ListAltIcon,
    path: '/my-results',
    roles: ['admin', 'teacher', 'student'],
  },
  {
    id: 'notifications',
    label: 'Bildirimler',
    icon: NotificationsActiveIcon,
    path: '/notifications',
    roles: ['admin', 'teacher', 'student'],
  },
  {
    id: 'settings',
    label: 'Ayarlar',
    icon: CogIcon,
    path: '/settings',
    roles: ['admin', 'teacher', 'student'],
  },
];

const Sidebar = ({ isOpen, setIsOpen }) => {
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useToken();
  const drawerWidth = isOpen ? 220 : 60;
  const [openMenus, setOpenMenus] = React.useState({});

  const handleMenuClick = (id) => {
    setOpenMenus((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  // Helper: check if user has any of the required roles
  const hasRequiredRole = (requiredRoles) => {
    if (!user?.roles || !requiredRoles) return false;
    return user.roles.some(role => requiredRoles.includes(role.toLowerCase()));
  };

  // Helper: filter menu items by user roles
  const filterMenu = (items) =>
    items
      .filter(item => !item.roles || hasRequiredRole(item.roles))
      .map(item => ({
        ...item,
        children: item.children ? filterMenu(item.children) : undefined,
      }));

  const filteredMenuItems = filterMenu(menuItems);

  const renderMenuItems = (items, depth = 0) =>
    items.map(item => {
      const Icon = item.icon;
      if (item.children && item.children.length > 0) {
        const isOpen = openMenus[item.id] || false;
        return (
          <React.Fragment key={item.id}>
            <ListItemButton
              onClick={() => handleMenuClick(item.id)}
              selected={location.pathname === item.path}
              sx={{
                pl: 2 + depth * 2,
                borderRadius: 1,
                mx: 1,
                mb: 0.5,
              }}
            >
              <ListItemIcon sx={{ minWidth: 0, mr: isOpen ? 3 : 'auto', justifyContent: 'center' }}>
                <Icon />
              </ListItemIcon>
              <ListItemText primary={item.label} sx={{ opacity: isOpen ? 1 : 0.9 }} />
              {isOpen ? <ExpandLess /> : <ExpandMore />}
            </ListItemButton>
            <Collapse in={isOpen} timeout="auto" unmountOnExit>
              <List component="div" disablePadding>
                {renderMenuItems(item.children, depth + 1)}
              </List>
            </Collapse>
          </React.Fragment>
        );
      } else {
        return (
          <ListItemButton
            key={item.id}
            onClick={() => navigate(item.path)}
            selected={location.pathname === item.path}
            sx={{
              pl: 2 + depth * 2,
              borderRadius: 1,
              mx: 1,
              mb: 0.5,
            }}
          >
            <ListItemIcon sx={{ minWidth: 0, mr: 'auto', justifyContent: 'center' }}>
              <Icon />
            </ListItemIcon>
            <ListItemText primary={item.label} sx={{ opacity: 1 }} />
          </ListItemButton>
        );
      }
    });

  return (
    <Drawer
      variant="permanent"
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: drawerWidth,
          boxSizing: 'border-box',
          transition: 'width 0.2s',
          overflowX: 'hidden',
          backgroundColor: 'background.paper',
          borderRight: '1px solid',
          borderColor: 'divider'
        },
      }}
    >
      <Box sx={{ 
        display: 'flex', 
        alignItems: 'center', 
        justifyContent: 'space-between',
        p: 1,
        minHeight: 64
      }}>
        {isOpen && (
          <Typography variant="h6" noWrap component="div" sx={{ ml: 2 }}>
            Quiz App
          </Typography>
        )}
        <IconButton onClick={() => setIsOpen(!isOpen)}>
          <ChevronLeftIcon sx={{ 
            transform: isOpen ? 'rotate(0deg)' : 'rotate(180deg)',
            transition: 'transform 0.2s'
          }} />
        </IconButton>
      </Box>
      <List>
        {renderMenuItems(filteredMenuItems)}
      </List>
    </Drawer>
  );
};

export default Sidebar; 