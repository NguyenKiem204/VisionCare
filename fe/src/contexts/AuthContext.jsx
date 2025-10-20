import React, { createContext, useContext, useState, useEffect } from "react";
import api from "../utils/api";

const AuthContext = createContext();

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    checkAuthStatus();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const checkAuthStatus = async () => {
    try {
      const token = localStorage.getItem("accessToken");
      if (!token) return;

      // Proactively refresh if near expiry on app load
      try {
        // Proactive refresh handled inside api request interceptor on first call
        // We'll rely on 401-triggered refresh for background state
      } catch {
        clearAuth();
        return;
      }

      // Fetch user info
      const response = await api.get("/auth/me");
      if (response.data.success) {
        setUser(response.data.data);
        setIsAuthenticated(true);
      } else {
        clearAuth();
      }
    } catch (error) {
      console.error("Auth check failed:", error);
      clearAuth();
    } finally {
      setLoading(false);
    }
  };

  const login = async (credentials) => {
    try {
      setLoading(true);
      const response = await api.post("/auth/login", credentials);

      if (response.data.success) {
        const { accessToken, expiresAt } = response.data.data;

        // Store token
        localStorage.setItem("accessToken", accessToken);
        localStorage.setItem(
          "tokenExpiresAt",
          new Date(expiresAt).getTime().toString()
        );

        // Get user info after successful login
        try {
          const userResponse = await api.get("/auth/me");
          if (userResponse.data.success) {
            setUser(userResponse.data.data);
            setIsAuthenticated(true);
            return { success: true, data: userResponse.data.data };
          } else {
            clearAuth();
            return { success: false, message: "Failed to get user info" };
          }
        } catch (userError) {
          console.error("Failed to get user info:", userError);
          clearAuth();
          return { success: false, message: "Failed to get user info" };
        }
      } else {
        return { success: false, message: response.data.message };
      }
    } catch (error) {
      console.error("Login error:", error);
      return {
        success: false,
        message: "Đăng nhập thất bại. Vui lòng kiểm tra email và mật khẩu.",
      };
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    clearAuth();
    // Redirect to login page
    window.location.href = "/admin/login";
  };

  const clearAuth = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("tokenExpiresAt");
    setUser(null);
    setIsAuthenticated(false);
  };

  const hasRole = (role) => {
    return user?.roleName?.toLowerCase() === role.toLowerCase();
  };

  const hasPermission = (permission) => {
    return user?.permissions?.includes(permission);
  };

  const value = {
    user,
    isAuthenticated,
    loading,
    login,
    logout,
    hasRole,
    hasPermission,
    checkAuthStatus,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthContext;
