import React from "react";
import { Star, Award, User } from "lucide-react";

const DoctorTable = ({
  doctors,
  loading,
  onEdit,
  onDelete,
  onUpdateRating,
  onUpdateStatus,
}) => (
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
          Đánh giá
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Kinh nghiệm
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Giới tính
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Trạng thái
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Actions
        </th>
      </tr>
    </thead>
    <tbody>
      {loading ? (
        <tr>
          <td colSpan="9" className="text-center py-6">
            Đang tải...
          </td>
        </tr>
      ) : doctors.length === 0 ? (
        <tr>
          <td colSpan="9" className="text-center py-6 text-gray-400">
            Không có dữ liệu
          </td>
        </tr>
      ) : (
        doctors.map((doctor, idx) => (
          <tr
            key={doctor.id}
            className={`text-sm ${
              idx % 2 === 0
                ? "bg-white dark:bg-gray-900"
                : "bg-gray-50 dark:bg-gray-800"
            } border-b border-gray-100 dark:border-gray-800 hover:bg-blue-50 dark:hover:bg-gray-700 transition-colors duration-200`}
          >
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {doctor.id}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {doctor.doctorName || doctor.fullName}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {doctor.email}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {doctor.phone || "Chưa có"}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <div className="flex items-center gap-1">
                <Star className="w-4 h-4 text-yellow-500 fill-current" />
                <span className="font-medium">{doctor.rating || 0}</span>
              </div>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <div className="flex items-center gap-1">
                <Award className="w-4 h-4 text-green-500" />
                <span>
                  {doctor.experienceYears || doctor.experience || 0} năm
                </span>
              </div>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <span
                className={`px-2 py-1 rounded-full text-xs font-medium ${
                  doctor.gender === "Male"
                    ? "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200"
                    : doctor.gender === "Female"
                    ? "bg-pink-100 text-pink-800 dark:bg-pink-900 dark:text-pink-200"
                    : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200"
                }`}
              >
                {doctor.gender === "Male"
                  ? "Nam"
                  : doctor.gender === "Female"
                  ? "Nữ"
                  : doctor.gender || "Chưa có"}
              </span>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <span
                className={`px-2 py-1 rounded-full text-xs font-medium ${
                  doctor.doctorStatus === "Active" || doctor.status === "Active"
                    ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200"
                    : doctor.doctorStatus === "Inactive" ||
                      doctor.status === "Inactive"
                    ? "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200"
                    : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200"
                }`}
              >
                {doctor.doctorStatus === "Active" || doctor.status === "Active"
                  ? "Hoạt động"
                  : doctor.doctorStatus === "Inactive" ||
                    doctor.status === "Inactive"
                  ? "Không hoạt động"
                  : doctor.doctorStatus || doctor.status}
              </span>
            </td>
            <td className="px-4 py-2 flex gap-1 flex-wrap">
              <button
                className="bg-blue-600 hover:bg-blue-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onEdit(doctor)}
              >
                Sửa
              </button>
              <button
                className="bg-gray-200 dark:bg-gray-700 dark:text-white hover:bg-red-500 hover:text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onDelete(doctor.id)}
              >
                Xóa
              </button>
              <button
                className="bg-green-600 hover:bg-green-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => {
                  const currentStatus = doctor.doctorStatus || doctor.status;
                  const newStatus =
                    currentStatus === "Active" ? "Inactive" : "Active";
                  onUpdateStatus(doctor.id, newStatus);
                }}
              >
                {(doctor.doctorStatus || doctor.status) === "Active"
                  ? "Vô hiệu"
                  : "Kích hoạt"}
              </button>
            </td>
          </tr>
        ))
      )}
    </tbody>
  </table>
);

export default DoctorTable;
