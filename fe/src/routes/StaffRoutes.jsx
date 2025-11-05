import React from "react";
import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "../components/auth/ProtectedRoute";
import StaffLayout from "../layouts/StaffLayout";
import StaffDashboard from "../pages/staff/Dashboard";
import StaffPatients from "../pages/staff/Patients";
import StaffBookings from "../pages/staff/Bookings";

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
        <Route path="patients" element={<StaffPatients />} />
        <Route path="bookings" element={<StaffBookings />} />
      </Route>
    </Routes>
  );
};

export default StaffRoutes;


