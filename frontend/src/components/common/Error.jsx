import React from 'react';
import { Box, Typography, Button } from '@mui/material';
import { Error as ErrorIcon } from '@mui/icons-material';

const Error = ({ message = 'An error occurred', onRetry }) => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        minHeight: '200px',
        p: 3,
      }}
    >
      <ErrorIcon
        color="error"
        sx={{
          fontSize: 48,
          mb: 2,
        }}
      />
      <Typography
        variant="h6"
        color="error"
        align="center"
        sx={{ mb: 2 }}
      >
        {message}
      </Typography>
      {onRetry && (
        <Button
          variant="contained"
          color="primary"
          onClick={onRetry}
        >
          Try Again
        </Button>
      )}
    </Box>
  );
};

export default Error; 