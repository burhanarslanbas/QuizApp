import React from 'react';
import { Navigate } from 'react-router-dom';
import { useToken } from '../context/TokenContext';

const RoleBasedRoute = ({ allowedRoles, children, redirectTo = "/login" }) => {
  const { user } = useToken();
  const userRoles = Array.isArray(user?.roles) ? user.roles.map(r => r.toLowerCase()) : [];
  const hasAccess = userRoles.some(role => allowedRoles.includes(role));

  if (!hasAccess) {
    return <Navigate to={redirectTo} replace />;
  }
  return children;
};

export default RoleBasedRoute; 