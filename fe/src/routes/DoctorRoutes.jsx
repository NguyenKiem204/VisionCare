import React from "react";
import { Routes, Route, Link } from "react-router-dom";
import ProtectedRoute from "../components/auth/ProtectedRoute";
import DoctorLayout from "../layouts/DoctorLayout";
import DoctorLogin from "../pages/doctor/Login";
import DoctorDashboard from "../pages/doctor/Dashboard";
import DoctorPatients from "../pages/doctor/Patients";
import DoctorSchedule from "../pages/doctor/Schedule";
import DoctorAbsences from "../pages/doctor/Absences";
import EncounterPage from "../pages/doctor/Encounter";
import AppointmentDetail from "../pages/doctor/AppointmentDetail";
import Profile from "../pages/doctor/Profile";
import Analytics from "../pages/doctor/Analytics";
import MyDoctorSchedules from "../pages/doctor/MyDoctorSchedules";
import BlogsManagement from "../pages/doctor/BlogsManagement";
import BlogEditor from "../pages/doctor/BlogEditor";

const DoctorRoutes = () => {
  return (
    <Routes>
      {/* Doctor Login - No authentication required */}
      <Route path="login" element={<DoctorLogin />} />

      {/* Unauthorized page */}
      <Route
        path="unauthorized"
        element={
          <div className="min-h-screen flex items-center justify-center">
            <div className="text-center">
              <h1 className="text-2xl font-bold text-red-600 mb-4">
                Unauthorized
              </h1>
              <p className="text-gray-600 mb-4">
                You don't have permission to access this page.
              </p>
              <Link to="/" className="text-blue-600 hover:underline">
                Go back to home
              </Link>
            </div>
          </div>
        }
      />

      {/* Doctor Dashboard - Requires doctor role */}
      <Route
        path=""
        element={
          <ProtectedRoute requiredRole="doctor">
            <DoctorLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<DoctorDashboard />} />
        <Route path="patients" element={<DoctorPatients />} />
        <Route path="schedule" element={<DoctorSchedule />} />
        <Route path="appointments/:id" element={<AppointmentDetail />} />
        <Route path="doctor-schedules" element={<MyDoctorSchedules />} />
        <Route path="absences" element={<DoctorAbsences />} />
        <Route path="ehr" element={<EncounterPage />} />
        <Route path="profile" element={<Profile />} />
        <Route path="analytics" element={<Analytics />} />
        <Route path="blogs" element={<BlogsManagement />} />
        <Route path="blogs/new" element={<BlogEditor />} />
        <Route path="blogs/:id/edit" element={<BlogEditor />} />
      </Route>
    </Routes>
  );
};

export default DoctorRoutes;
