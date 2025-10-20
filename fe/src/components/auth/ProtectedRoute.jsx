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
    // Build absolute admin login path if inside admin section
    const isAdminSection = location.pathname.startsWith("/admin");
    const loginPath = isAdminSection ? "/admin/login" : "/login";
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
