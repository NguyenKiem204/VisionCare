import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import Loading from "../../components/common/Loading";
import ModernLogin from "../../components/auth/ModernLogin";

const Login = () => {
  const { isAuthenticated, loading } = useAuth();
  const location = useLocation();

  const from = location.state?.from?.pathname || "/staff";

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <Loading />
      </div>
    );
  }

  if (isAuthenticated) {
    return <Navigate to={from} replace />;
  }

  return <ModernLogin role="staff" redirectPath={from} />;
};

export default Login;

