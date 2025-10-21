import React from "react";
import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "../components/auth/ProtectedRoute";
import DoctorLayout from "../layouts/DoctorLayout";
import DoctorDashboard from "../pages/doctor/Dashboard";
import DoctorPatients from "../pages/doctor/Patients";
import DoctorSchedule from "../pages/doctor/Schedule";

const DoctorRoutes = () => {
  return (
    <Routes>
      <Route
        path="/doctor"
        element={
          <ProtectedRoute requiredRole="doctor">
            <DoctorLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<DoctorDashboard />} />
        <Route path="patients" element={<DoctorPatients />} />
        <Route path="schedule" element={<DoctorSchedule />} />
      </Route>
    </Routes>
  );
};

export default DoctorRoutes;
