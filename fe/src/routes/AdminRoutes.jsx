import React from "react";
import { Routes, Route, Link } from "react-router-dom";
import ProtectedRoute from "../components/auth/ProtectedRoute";
import AdminLayout from "../layouts/AdminLayout";
import AdminLogin from "../pages/admin/Login";
import AdminDashboard from "../pages/admin/Dashboard";
import AdminBookings from "../pages/admin/Bookings";
import AdminPatients from "../pages/admin/Patients";
import AdminUsers from "../pages/admin/UsersManagementPage";
import AdminCustomers from "../pages/admin/CustomersManagementPage";
import AdminDoctors from "../pages/admin/DoctorsManagementPage";
import AdminStaff from "../pages/admin/StaffManagementPage";
import AdminServices from "../pages/admin/ServicesManagementPage";
import AdminAppointments from "../pages/admin/AppointmentsManagementPage";
import AdminSettings from "../pages/admin/Settings";
import AdminEquipment from "../pages/admin/EquipmentManagementPage";
import AdminServiceTypes from "../pages/admin/ServiceTypesManagementPage";
import AdminFeedback from "../pages/admin/FeedbackManagementPage";
import AdminFollowUp from "../pages/admin/FollowUpManagementPage";
import AdminMedicalHistory from "../pages/admin/MedicalHistoryManagementPage";
import AdminMedicalRecords from "../pages/admin/MedicalRecordsManagementPage";
import AdminHomeContent from "../pages/admin/HomeContentManagementPage";
import DoctorSchedulesManagementPage from "../pages/admin/DoctorSchedulesManagementPage";
import DoctorAbsenceManagement from "../pages/admin/DoctorAbsenceManagement";
import RoomsManagementPage from "../pages/admin/RoomsManagementPage";
import WorkShiftsManagementPage from "../pages/admin/WorkShiftsManagementPage";

const AdminRoutes = () => {
  return (
    <Routes>
      {/* Admin Login - No authentication required */}
      <Route path="login" element={<AdminLogin />} />

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

      {/* Admin Dashboard - Requires admin role */}
      <Route
        path=""
        element={
          <ProtectedRoute requiredRole="admin">
            <AdminLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<AdminDashboard />} />
        <Route path="customers" element={<AdminCustomers />} />
        <Route path="doctors" element={<AdminDoctors />} />
        <Route path="staff" element={<AdminStaff />} />
        <Route path="services" element={<AdminServices />} />
        <Route path="appointments" element={<AdminAppointments />} />
        <Route path="users" element={<AdminUsers />} />
        <Route path="equipment" element={<AdminEquipment />} />
        <Route path="service-types" element={<AdminServiceTypes />} />
        <Route path="feedback" element={<AdminFeedback />} />
        <Route path="follow-up" element={<AdminFollowUp />} />
        <Route path="medical-history" element={<AdminMedicalHistory />} />
        <Route path="medical-records" element={<AdminMedicalRecords />} />
        <Route path="doctor-schedules" element={<DoctorSchedulesManagementPage />} />
        <Route path="doctor-absence" element={<DoctorAbsenceManagement />} />
        <Route path="rooms" element={<RoomsManagementPage />} />
        <Route path="work-shifts" element={<WorkShiftsManagementPage />} />
        <Route path="home-content" element={<AdminHomeContent />} />
        <Route path="settings" element={<AdminSettings />} />
      </Route>
    </Routes>
  );
};

export default AdminRoutes;
