import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";

const Profile = () => {
  const { user } = useAuth();

  // Redirect to role-specific profile page
  const getProfilePath = () => {
    const role = user?.roleName?.toLowerCase();
    if (role === "admin") return "/admin/profile";
    if (role === "doctor") return "/doctor/profile";
    if (role === "staff") return "/staff/profile";
    return "/customer/profile"; // Default to customer
  };

  return <Navigate to={getProfilePath()} replace />;
};

export default Profile;
