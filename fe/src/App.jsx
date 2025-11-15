import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";
import PublicLayout from "./layouts/PublicLayout";
import Home from "./pages/Home";
import Services from "./pages/Services";
import Equipment from "./pages/Equipment";
import Contact from "./pages/Contact";
import About from "./pages/About";
import Booking from "./pages/Booking";
import BookingStatus from "./pages/BookingStatus";
import BookingPaymentCallback from "./pages/BookingPaymentCallback";
import Profile from "./pages/Profile";
import Login from "./pages/Login";
import Register from "./pages/Register";
import ForgotPassword from "./pages/ForgotPassword";
import Blogs from "./pages/Blogs";
import BlogDetail from "./pages/BlogDetail";
import Doctors from "./pages/Doctors";
import DoctorDetail from "./pages/DoctorDetail";
import AdminRoutes from "./routes/AdminRoutes";
import DoctorRoutes from "./routes/DoctorRoutes";
import StaffRoutes from "./routes/StaffRoutes";
import ProtectedRoute from "./components/auth/ProtectedRoute";
import CustomerProfile from "./pages/customer/Profile";
import CustomerHistory from "./pages/customer/History";
import "./index.css";

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* Public Routes */}
          <Route path="/*" element={<PublicLayout />}>
            <Route index element={<Home />} />
            <Route path="about" element={<About />} />
            <Route path="services" element={<Services />} />
            <Route path="equipment" element={<Equipment />} />
            <Route path="contact" element={<Contact />} />
            <Route path="booking" element={<Booking />} />
            {/* Booking Status - Public (no auth required) */}
            <Route path="booking/status" element={<BookingStatus />} />
            {/* Payment Callback - Public */}
            <Route
              path="booking/payment/callback"
              element={<BookingPaymentCallback />}
            />
            <Route
              path="booking/success"
              element={<BookingPaymentCallback />}
            />
            <Route
              path="booking/failed"
              element={<BookingPaymentCallback />}
            />
            <Route path="profile" element={<Profile />} />
            <Route
              path="customer/profile"
              element={
                <ProtectedRoute>
                  <CustomerProfile />
                </ProtectedRoute>
              }
            />
            <Route
              path="customer/history"
              element={
                <ProtectedRoute>
                  <CustomerHistory />
                </ProtectedRoute>
              }
            />
            <Route path="blogs" element={<Blogs />} />
            <Route path="blogs/:slug" element={<BlogDetail />} />
            <Route path="doctors" element={<Doctors />} />
            <Route path="doctors/:id" element={<DoctorDetail />} />
          </Route>

          {/* Auth Routes */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/forgot-password" element={<ForgotPassword />} />

          {/* Admin Routes */}
          <Route path="/admin/*" element={<AdminRoutes />} />

          {/* Doctor Routes */}
          <Route path="/doctor/*" element={<DoctorRoutes />} />

          {/* Staff Routes */}
          <Route path="/staff/*" element={<StaffRoutes />} />

          {/* Redirects */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
