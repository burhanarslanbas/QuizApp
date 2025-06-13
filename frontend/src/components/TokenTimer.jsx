import React, { useState, useEffect } from 'react';
import { Box, Typography } from '@mui/material';

const TokenTimer = () => {
  const [accessTokenTime, setAccessTokenTime] = useState(null);
  const [refreshTokenTime, setRefreshTokenTime] = useState(null);

  useEffect(() => {
    const updateTimes = () => {
      const accessTokenExpiration = localStorage.getItem('tokenExpiration');
      const refreshTokenExpiration = localStorage.getItem('refreshTokenExpiration');

      const getRemainingTime = (expiration) => {
        if (!expiration) return null;
        const expirationDate = new Date(expiration);
        const diff = expirationDate - new Date();
        if (diff <= 0) return 'Süresi doldu';
        const min = Math.floor(diff / 60000);
        const sec = Math.floor((diff % 60000) / 1000);
        return `${min} dk ${sec} sn`;
      };

      setAccessTokenTime(getRemainingTime(accessTokenExpiration));
      setRefreshTokenTime(getRemainingTime(refreshTokenExpiration));
    };

    updateTimes();
    const interval = setInterval(updateTimes, 1000);
    return () => clearInterval(interval);
  }, []);

  return (
    <Box sx={{ p: 2, bgcolor: 'background.paper', borderRadius: 1 }}>
      <Typography variant="body2" color="text.secondary">
        Access Token Süresi: {accessTokenTime || 'Yok'}
      </Typography>
      <Typography variant="body2" color="text.secondary">
        Refresh Token Süresi: {refreshTokenTime || 'Yok'}
      </Typography>
    </Box>
  );
};

export default TokenTimer; 