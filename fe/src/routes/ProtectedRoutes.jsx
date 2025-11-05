import React from "react";
import { Routes, Route } from "react-router-dom";
import Booking from "../pages/Booking";
import BookingStatus from "../pages/BookingStatus";
import BookingPaymentCallback from "../pages/BookingPaymentCallback";
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
      {/* Booking status is public (no login required) */}
      <Route path="/booking/status" element={<BookingStatus />} />
      {/* Payment callback is public */}
      <Route
        path="/booking/payment/callback"
        element={<BookingPaymentCallback />}
      />
      <Route path="/booking/success" element={<BookingPaymentCallback />} />
      <Route path="/booking/failed" element={<BookingPaymentCallback />} />
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
