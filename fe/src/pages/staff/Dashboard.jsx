import React from "react";

const StaffDashboard = () => {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">
          Dashboard Nhân viên
        </h1>
        <p className="text-gray-600">Tổng quan hoạt động của nhân viên</p>
      </div>

      <div className="bg-white shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 mb-4">
          Công việc hôm nay
        </h3>
        <p className="text-gray-500">Tính năng đang được phát triển...</p>
      </div>
    </div>
  );
};

export default StaffDashboard;
