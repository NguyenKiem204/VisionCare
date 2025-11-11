import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import DoctorHeader from "../components/doctor/common/DoctorHeader";
import DoctorSidebar from "../components/doctor/common/DoctorSidebar";

const DoctorLayout = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <DoctorSidebar isOpen={sidebarOpen} setOpen={setSidebarOpen} />
      <div
        className={`transition-all duration-300 ${
          sidebarOpen ? "ml-64" : "ml-20"
        }`}
      >
        <DoctorHeader onMenuClick={() => setSidebarOpen(!sidebarOpen)} />
        <main className="p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default DoctorLayout;
