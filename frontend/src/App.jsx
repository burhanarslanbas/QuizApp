import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import AppRoutes from './routes/AppRoutes';
import { TokenProvider } from './context/TokenContext';
import './App.css';

function App() {
  return (
    <TokenProvider>
      <BrowserRouter>
        <Toaster position="top-right" />
        <AppRoutes />
      </BrowserRouter>
    </TokenProvider>
  );
}

export default App;
