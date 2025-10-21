import React from "react";
import { Outlet, useLocation } from "react-router-dom";
import Header from "../components/common/Header";
import Footer from "../components/common/Footer";
import ChatWidget from "../components/chatbot/ChatWidget";

const PublicLayout = () => {
  const location = useLocation();

  // Hide ChatWidget on admin, doctor, and staff pages
  const isAdminPage = location.pathname.startsWith("/admin");
  const isDoctorPage = location.pathname.startsWith("/doctor");
  const isStaffPage = location.pathname.startsWith("/staff");

  return (
    <div className="min-h-screen">
      <Header />
      <main>
        <Outlet />
      </main>
      <Footer />
      {!(isAdminPage || isDoctorPage || isStaffPage) && <ChatWidget />}
    </div>
  );
};

export default PublicLayout;
