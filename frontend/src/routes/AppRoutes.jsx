import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { useToken } from '../context/TokenContext';
import Loading from '../components/common/Loading';
import RoleBasedRoute from './RoleBasedRoute';

// Auth pages
import Login from "../pages/auth/Login";
import Register from "../pages/auth/Register";

// Question pages
import QuestionList from "../pages/questions/QuestionList";
import CreateQuestion from "../pages/questions/CreateQuestion";
import EditQuestion from "../pages/questions/EditQuestion";

// Category pages
import Categories from "../pages/categories/Categories";
import CreateCategory from "../pages/categories/CreateCategory";
import CategoryDetail from "../pages/categories/CategoryDetail";

// Profile pages
import Profile from "../pages/profile/Profile";

// Other pages
import Home from "../pages/home/Home";
import Dashboard from "../pages/dashboard/Dashboard";
import NotFound from "../pages/NotFound";

const AppRoutes = () => {
  const { user, isLoading, checking } = useToken();

  if (isLoading || checking) {
    return <Loading />;
  }

  return (
    <Routes>
      {/* Public routes */}
      <Route path="/" element={<Home />} />
      <Route path="/login" element={!user ? <Login /> : <Navigate to="/dashboard" />} />
      <Route path="/register" element={!user ? <Register /> : <Navigate to="/login" />} />

      {/* Protected routes */}
      <Route path="/dashboard" element={<RoleBasedRoute allowedRoles={['admin','teacher','student']} redirectTo="/login"><Dashboard /></RoleBasedRoute>} />

      <Route path="/questions" element={<RoleBasedRoute allowedRoles={['admin','teacher']} redirectTo="/dashboard"><QuestionList /></RoleBasedRoute>} />
      <Route path="/questions/create" element={<RoleBasedRoute allowedRoles={['admin','teacher']} redirectTo="/questions"><CreateQuestion /></RoleBasedRoute>} />
      <Route path="/questions/edit/:id" element={<RoleBasedRoute allowedRoles={['admin','teacher']} redirectTo="/questions"><EditQuestion /></RoleBasedRoute>} />

      <Route path="/categories" element={<RoleBasedRoute allowedRoles={['admin','teacher','student']} redirectTo="/login"><Categories /></RoleBasedRoute>} />
      <Route path="/categories/create" element={<RoleBasedRoute allowedRoles={['admin','teacher']} redirectTo="/categories"><CreateCategory /></RoleBasedRoute>} />
      <Route path="/categories/:id" element={<RoleBasedRoute allowedRoles={['admin','teacher','student']} redirectTo="/categories"><CategoryDetail /></RoleBasedRoute>} />
      <Route path="/categories/:id/edit" element={<RoleBasedRoute allowedRoles={['admin','teacher']} redirectTo="/categories"><CreateCategory /></RoleBasedRoute>} />

      <Route path="/profile" element={<RoleBasedRoute allowedRoles={['admin','teacher','student']} redirectTo="/login"><Profile /></RoleBasedRoute>} />

      {/* 404 route */}
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes; 