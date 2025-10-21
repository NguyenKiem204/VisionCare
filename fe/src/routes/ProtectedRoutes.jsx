import React from "react";
import { Routes, Route } from "react-router-dom";
import Booking from "../pages/Booking";
import Profile from "../pages/Profile";
import ProtectedRoute from "../components/auth/ProtectedRoute";

const ProtectedRoutes = () => {
  return (
    <Routes>
      <Route
        path="/booking"
        element={
          <ProtectedRoute>
            <Booking />
          </ProtectedRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <ProtectedRoute>
            <Profile />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default ProtectedRoutes;
