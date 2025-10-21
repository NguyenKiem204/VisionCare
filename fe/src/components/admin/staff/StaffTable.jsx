import React from "react";

const StaffTable = ({ staff, loading, onEdit, onDelete }) => (
  <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700 rounded-xl overflow-hidden shadow-lg">
    <thead className="bg-gray-100 dark:bg-gray-800">
      <tr>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          ID
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Tên đầy đủ
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Email
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Số điện thoại
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Giới tính
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Ngày sinh
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Actions
        </th>
      </tr>
    </thead>
    <tbody>
      {loading ? (
        <tr>
          <td colSpan="7" className="text-center py-6">
            Đang tải...
          </td>
        </tr>
      ) : staff.length === 0 ? (
        <tr>
          <td colSpan="7" className="text-center py-6 text-gray-400">
            Không có dữ liệu
          </td>
        </tr>
      ) : (
        staff.map((staffMember, idx) => (
          <tr
            key={staffMember.id}
            className={`text-sm ${
              idx % 2 === 0
                ? "bg-white dark:bg-gray-900"
                : "bg-gray-50 dark:bg-gray-800"
            } border-b border-gray-100 dark:border-gray-800 hover:bg-blue-50 dark:hover:bg-gray-700 transition-colors duration-200`}
          >
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {staffMember.id}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {staffMember.staffName || staffMember.fullName}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {staffMember.email}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {staffMember.phone}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <span
                className={`px-2 py-1 rounded-full text-xs font-medium ${
                  staffMember.gender === "Male"
                    ? "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200"
                    : staffMember.gender === "Female"
                    ? "bg-pink-100 text-pink-800 dark:bg-pink-900 dark:text-pink-200"
                    : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200"
                }`}
              >
                {staffMember.gender === "Male"
                  ? "Nam"
                  : staffMember.gender === "Female"
                  ? "Nữ"
                  : staffMember.gender}
              </span>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {staffMember.dob
                ? new Date(staffMember.dob).toLocaleDateString("vi-VN")
                : staffMember.dateOfBirth
                ? new Date(staffMember.dateOfBirth).toLocaleDateString("vi-VN")
                : "-"}
            </td>
            <td className="px-4 py-2 flex gap-2">
              <button
                className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onEdit(staffMember)}
              >
                Sửa
              </button>
              <button
                className="bg-gray-200 dark:bg-gray-700 dark:text-white hover:bg-red-500 hover:text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onDelete(staffMember.id)}
              >
                Xóa
              </button>
            </td>
          </tr>
        ))
      )}
    </tbody>
  </table>
);

export default StaffTable;
