import React from "react";
import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "../components/auth/ProtectedRoute";
import StaffLayout from "../layouts/StaffLayout";
import StaffDashboard from "../pages/staff/Dashboard";
import StaffBookings from "../pages/staff/Bookings";
import StaffPatients from "../pages/staff/Patients";

const StaffRoutes = () => {
  return (
    <Routes>
      <Route
        path="/staff"
        element={
          <ProtectedRoute requiredRole="staff">
            <StaffLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<StaffDashboard />} />
        <Route path="bookings" element={<StaffBookings />} />
        <Route path="patients" element={<StaffPatients />} />
      </Route>
    </Routes>
  );
};

export default StaffRoutes;
