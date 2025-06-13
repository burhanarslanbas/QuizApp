import React, { useState, useEffect } from 'react';
import { useToken } from '../../context/TokenContext';
import { Box, Typography } from '@mui/material';

const TokenTimer = () => {
    const { tokenExpiresAt, refreshTokenExpiresAt } = useToken();
    const [remainingTime, setRemainingTime] = useState(null);
    const [refreshRemaining, setRefreshRemaining] = useState('');

    useEffect(() => {
        const calculateRemainingTime = () => {
            if (!tokenExpiresAt) {
                setRemainingTime(null);
                return;
            }
            const now = new Date();
            const expirationDate = new Date(tokenExpiresAt);
            const diff = expirationDate - now;
            if (diff <= 0) {
                setRemainingTime(null);
                return;
            }
            const minutes = Math.floor(diff / 60000);
            const seconds = Math.floor((diff % 60000) / 1000);
            setRemainingTime({ minutes, seconds });
        };
        calculateRemainingTime();
        const interval = setInterval(calculateRemainingTime, 1000);
        return () => clearInterval(interval);
    }, [tokenExpiresAt]);

    useEffect(() => {
        if (!refreshTokenExpiresAt) {
            setRefreshRemaining('');
            return;
        }
        const updateRefreshRemainingTime = () => {
            const now = new Date();
            const exp = new Date(refreshTokenExpiresAt);
            const diff = exp - now;
            if (diff > 0) {
                const hours = Math.floor(diff / (1000 * 60 * 60));
                const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
                const seconds = Math.floor((diff % (1000 * 60)) / 1000);
                if (hours > 0) {
                    setRefreshRemaining(`${hours} saat ${minutes} dakika`);
                } else {
                    setRefreshRemaining(`${minutes} dakika ${seconds} saniye`);
                }
            } else {
                setRefreshRemaining('Refresh token süresi doldu.');
            }
        };
        updateRefreshRemainingTime();
        const interval = setInterval(updateRefreshRemainingTime, 1000);
        return () => clearInterval(interval);
    }, [refreshTokenExpiresAt]);

    if (!remainingTime) {
        return <span>Oturum süresi alınamıyor.</span>;
    }

    return (
        <Box sx={{ 
            display: 'flex', 
            flexDirection: 'column',
            gap: 1,
            p: 2,
            bgcolor: 'background.paper',
            borderRadius: 1,
            boxShadow: 1
        }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <Typography variant="body2" color="text.secondary">
                    Oturum Süresi:
                </Typography>
                <Typography variant="body2" color="primary.main" fontWeight="medium">
                    {remainingTime.minutes} dakika {remainingTime.seconds} saniye
                </Typography>
            </Box>
            {refreshRemaining && (
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Typography variant="body2" color="text.secondary">
                        Refresh Token:
                    </Typography>
                    <Typography variant="body2" color="primary.main" fontWeight="medium">
                        {refreshRemaining}
                    </Typography>
                </Box>
            )}
        </Box>
    );
};

export default TokenTimer; 