import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  Grid,
  Typography,
  Button,
  TextField,
  Avatar,
  Stack,
  Divider,
  Alert,
  CircularProgress,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
  Chip,
} from '@mui/material';
import {
  Person as PersonIcon,
  Email as EmailIcon,
  Lock as LockIcon,
  School as SchoolIcon,
  Quiz as QuizIcon,
  Assessment as AssessmentIcon,
} from '@mui/icons-material';
import { useToken } from '../../context/TokenContext';
import { useNotification } from '../../context/NotificationContext';
import { userService } from '../../services/userService';
import Loading from '../../components/common/Loading';
import Error from '../../components/common/Error';
import Layout from '../../components/layout/Layout';

const Profile = (props) => {
  const { user } = useToken();
  const { showNotification } = useNotification();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [profileData, setProfileData] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });
  const [saving, setSaving] = useState(false);
  const userRole = Array.isArray(user?.roles) && user.roles.length > 0 ? user.roles[0].toLowerCase() : undefined;

  useEffect(() => {
    fetchProfileData();
  }, []);

  const fetchProfileData = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await userService.getCurrentUser();
      setProfileData(data);
      setFormData({
        fullName: data.fullName || '',
        email: data.email || '',
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
      });
    } catch (err) {
      console.error('Error fetching profile:', err);
      setError(err.message || 'Failed to load profile');
      showNotification('Error loading profile', 'error');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (formData.newPassword !== formData.confirmPassword) {
      showNotification('New passwords do not match', 'error');
      return;
    }

    try {
      setSaving(true);
      await userService.updateProfile({
        fullName: formData.fullName,
        email: formData.email,
        currentPassword: formData.currentPassword || null,
        newPassword: formData.newPassword || null,
      });
      showNotification('Profile updated successfully', 'success');
      setEditMode(false);
      fetchProfileData();
    } catch (err) {
      console.error('Error updating profile:', err);
      showNotification(err.message || 'Failed to update profile', 'error');
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <Loading />;
  if (error) return <Error message={error} />;

  return (
    <Layout>
      <Box sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>
          Profil
        </Typography>

        <Grid container spacing={3}>
          <Grid item xs={12} md={4}>
            <Card>
              <CardContent>
                <Stack spacing={3} alignItems="center">
                  <Avatar
                    sx={{
                      width: 100,
                      height: 100,
                      bgcolor: 'primary.main',
                      fontSize: '2.5rem',
                    }}
                  >
                    {profileData?.fullName?.charAt(0) || 'U'}
                  </Avatar>
                  <Box textAlign="center">
                    <Typography variant="h6">
                      {profileData?.fullName}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {profileData?.email}
                    </Typography>
                  </Box>
                  <Box width="100%">
                    <Stack direction="row" spacing={1} flexWrap="wrap" justifyContent="center">
                      {profileData?.roles?.map((role) => (
                        <Chip
                          key={role}
                          label={role}
                          color={role?.toLowerCase() === 'admin' ? 'error' : 'primary'}
                          size="small"
                        />
                      ))}
                    </Stack>
                  </Box>
                </Stack>
              </CardContent>
            </Card>
          </Grid>

          <Grid item xs={12} md={8}>
            <Card>
              <CardContent>
                {editMode ? (
                  <form onSubmit={handleSubmit}>
                    <Stack spacing={3}>
                      <Typography variant="h6">Profil Düzenle</Typography>
                      <TextField
                        fullWidth
                        label="Ad Soyad"
                        name="fullName"
                        value={formData.fullName}
                        onChange={handleInputChange}
                      />
                      <TextField
                        fullWidth
                        label="E-posta"
                        name="email"
                        type="email"
                        value={formData.email}
                        onChange={handleInputChange}
                      />
                      <Divider />
                      <Typography variant="subtitle2">Şifre Değiştir</Typography>
                      <TextField
                        fullWidth
                        label="Mevcut Şifre"
                        name="currentPassword"
                        type="password"
                        value={formData.currentPassword}
                        onChange={handleInputChange}
                      />
                      <TextField
                        fullWidth
                        label="Yeni Şifre"
                        name="newPassword"
                        type="password"
                        value={formData.newPassword}
                        onChange={handleInputChange}
                      />
                      <TextField
                        fullWidth
                        label="Yeni Şifre (Tekrar)"
                        name="confirmPassword"
                        type="password"
                        value={formData.confirmPassword}
                        onChange={handleInputChange}
                      />
                      <Stack direction="row" spacing={2} justifyContent="flex-end">
                        <Button
                          variant="outlined"
                          onClick={() => setEditMode(false)}
                          disabled={saving}
                        >
                          İptal
                        </Button>
                        <Button
                          type="submit"
                          variant="contained"
                          disabled={saving}
                        >
                          {saving ? <CircularProgress size={24} /> : 'Kaydet'}
                        </Button>
                      </Stack>
                    </Stack>
                  </form>
                ) : (
                  <Stack spacing={3}>
                    <Stack direction="row" justifyContent="space-between" alignItems="center">
                      <Typography variant="h6">Profil Bilgileri</Typography>
                      <Button
                        variant="outlined"
                        onClick={() => setEditMode(true)}
                      >
                        Düzenle
                      </Button>
                    </Stack>
                    <List>
                      <ListItem>
                        <ListItemIcon>
                          <PersonIcon />
                        </ListItemIcon>
                        <ListItemText
                          primary="Ad Soyad"
                          secondary={profileData?.fullName}
                        />
                      </ListItem>
                      <ListItem>
                        <ListItemIcon>
                          <EmailIcon />
                        </ListItemIcon>
                        <ListItemText
                          primary="E-posta"
                          secondary={profileData?.email}
                        />
                      </ListItem>
                      <ListItem>
                        <ListItemIcon>
                          <SchoolIcon />
                        </ListItemIcon>
                        <ListItemText
                          primary="Roller"
                          secondary={profileData?.roles?.join(', ')}
                        />
                      </ListItem>
                    </List>
                    <Divider />
                    <Typography variant="h6">İstatistikler</Typography>
                    <Grid container spacing={2}>
                      <Grid item xs={12} sm={6}>
                        <Card>
                          <CardContent>
                            <Stack direction="row" spacing={2} alignItems="center">
                              <Avatar sx={{ bgcolor: 'primary.main' }}>
                                <QuizIcon />
                              </Avatar>
                              <Box>
                                <Typography variant="h4">
                                  {profileData?.quizCount || 0}
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                  Toplam Sınav
                                </Typography>
                              </Box>
                            </Stack>
                          </CardContent>
                        </Card>
                      </Grid>
                      <Grid item xs={12} sm={6}>
                        <Card>
                          <CardContent>
                            <Stack direction="row" spacing={2} alignItems="center">
                              <Avatar sx={{ bgcolor: 'success.main' }}>
                                <AssessmentIcon />
                              </Avatar>
                              <Box>
                                <Typography variant="h4">
                                  {profileData?.averageScore?.toFixed(1) || 0}%
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                  Ortalama Puan
                                </Typography>
                              </Box>
                            </Stack>
                          </CardContent>
                        </Card>
                      </Grid>
                    </Grid>
                  </Stack>
                )}
              </CardContent>
            </Card>
          </Grid>
        </Grid>
      </Box>
    </Layout>
  );
};

export default Profile; 