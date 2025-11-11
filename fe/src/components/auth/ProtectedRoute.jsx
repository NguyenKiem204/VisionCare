import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import Loading from "../common/Loading";

const ProtectedRoute = ({
  children,
  requiredRole = null,
  requiredPermission = null,
}) => {
  const { isAuthenticated, loading, hasRole, hasPermission, user } = useAuth();
  const location = useLocation();

  console.log("🔒 ProtectedRoute check:", {
    isAuthenticated,
    loading,
    requiredRole,
    userRole: user?.roleName,
    hasRequiredRole: requiredRole ? hasRole(requiredRole) : true,
  });

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <Loading />
      </div>
    );
  }

  if (!isAuthenticated) {
    // Redirect to role-specific login page
    let loginPath = "/login";
    if (requiredRole) {
      const role = requiredRole.toLowerCase();
      if (role === "admin") {
        loginPath = "/admin/login";
      } else if (role === "doctor") {
        loginPath = "/doctor/login";
      } else if (role === "staff") {
        loginPath = "/staff/login";
      }
    } else {
      // Check current path to determine login page
      if (location.pathname.startsWith("/admin")) {
        loginPath = "/admin/login";
      } else if (location.pathname.startsWith("/doctor")) {
        loginPath = "/doctor/login";
      } else if (location.pathname.startsWith("/staff")) {
        loginPath = "/staff/login";
      }
    }
    return <Navigate to={loginPath} state={{ from: location }} replace />;
  }

  // Check role requirement
  if (requiredRole && !hasRole(requiredRole)) {
    console.log("❌ Missing required role:", requiredRole);
    return <Navigate to="unauthorized" replace />;
  }

  // Check permission requirement
  if (requiredPermission && !hasPermission(requiredPermission)) {
    console.log("❌ Missing required permission:", requiredPermission);
    return <Navigate to="unauthorized" replace />;
  }

  console.log("✅ Access granted");
  return children;
};

export default ProtectedRoute;
