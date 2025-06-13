import React from 'react';
import { Box, Typography, Button } from '@mui/material';
import { Inbox as InboxIcon } from '@mui/icons-material';

const EmptyState = ({
  title = 'No Data',
  message = 'There is no data to display at the moment.',
  action,
  actionText,
}) => {
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
      <InboxIcon
        sx={{
          fontSize: 48,
          mb: 2,
          color: 'text.secondary',
        }}
      />
      <Typography
        variant="h6"
        color="text.secondary"
        align="center"
        sx={{ mb: 1 }}
      >
        {title}
      </Typography>
      <Typography
        variant="body1"
        color="text.secondary"
        align="center"
        sx={{ mb: 2 }}
      >
        {message}
      </Typography>
      {action && actionText && (
        <Button
          variant="contained"
          color="primary"
          onClick={action}
        >
          {actionText}
        </Button>
      )}
    </Box>
  );
};

export default EmptyState; 