import React from "react";
import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "../components/auth/ProtectedRoute";
import StaffLayout from "../layouts/StaffLayout";
import StaffLogin from "../pages/staff/Login";
import StaffDashboard from "../pages/staff/Dashboard";
import StaffPatients from "../pages/staff/Patients";
import StaffBookings from "../pages/staff/Bookings";
import BlogsManagement from "../pages/staff/BlogsManagement";
import BlogEditor from "../pages/staff/BlogEditor";
import Profile from "../pages/staff/Profile";

const StaffRoutes = () => {
  return (
    <Routes>
      {/* Staff Login - No authentication required */}
      <Route path="login" element={<StaffLogin />} />

      <Route
        path=""
        element={
          <ProtectedRoute requiredRole="staff">
            <StaffLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<StaffDashboard />} />
        <Route path="patients" element={<StaffPatients />} />
        <Route path="bookings" element={<StaffBookings />} />
        <Route path="blogs" element={<BlogsManagement />} />
        <Route path="blogs/new" element={<BlogEditor />} />
        <Route path="blogs/:id/edit" element={<BlogEditor />} />
        <Route path="profile" element={<Profile />} />
      </Route>
    </Routes>
  );
};

export default StaffRoutes;


