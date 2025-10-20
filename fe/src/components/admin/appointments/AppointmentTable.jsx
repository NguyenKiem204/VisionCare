import React from "react";
import {
  CheckCircle,
  XCircle,
  Clock,
  Calendar,
  User,
  Stethoscope,
} from "lucide-react";

const AppointmentTable = ({
  appointments,
  loading,
  onEdit,
  onDelete,
  onConfirm,
  onCancel,
  onComplete,
  onReschedule,
}) => (
  <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700 rounded-xl overflow-hidden shadow-lg">
    <thead className="bg-gray-100 dark:bg-gray-800">
      <tr>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          ID
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Khách hàng
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Bác sĩ
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Ngày giờ
        </th>
        <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
          Lý do
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
          <td colSpan="7" className="text-center py-6">
            Đang tải...
          </td>
        </tr>
      ) : appointments.length === 0 ? (
        <tr>
          <td colSpan="7" className="text-center py-6 text-gray-400">
            Không có dữ liệu
          </td>
        </tr>
      ) : (
        appointments.map((appointment, idx) => (
          <tr
            key={appointment.id}
            className={`text-sm ${
              idx % 2 === 0
                ? "bg-white dark:bg-gray-900"
                : "bg-gray-50 dark:bg-gray-800"
            } border-b border-gray-100 dark:border-gray-800 hover:bg-blue-50 dark:hover:bg-gray-700 transition-colors duration-200`}
          >
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              {appointment.id}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <div className="flex items-center gap-2">
                <User className="w-4 h-4 text-blue-500" />
                <span>
                  {appointment.patientName || appointment.customerName || "-"}
                </span>
              </div>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <div className="flex items-center gap-2">
                <Stethoscope className="w-4 h-4 text-green-500" />
                <span>{appointment.doctorName || "-"}</span>
              </div>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <div className="flex items-center gap-2">
                <Calendar className="w-4 h-4 text-purple-500" />
                <span>
                  {appointment.appointmentDateTime
                    ? new Date(appointment.appointmentDateTime).toLocaleString(
                        "vi-VN"
                      )
                    : appointment.appointmentDate
                    ? new Date(appointment.appointmentDate).toLocaleString(
                        "vi-VN"
                      )
                    : "-"}
                </span>
              </div>
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100 max-w-xs truncate">
              {appointment.notes || appointment.reason || "-"}
            </td>
            <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
              <span
                className={`px-2 py-1 rounded-full text-xs font-medium ${
                  appointment.status === "Confirmed"
                    ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200"
                    : appointment.status === "Pending"
                    ? "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200"
                    : appointment.status === "Cancelled"
                    ? "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200"
                    : appointment.status === "Completed"
                    ? "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200"
                    : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200"
                }`}
              >
                {appointment.status === "Confirmed" ? (
                  <>
                    <CheckCircle className="w-3 h-3 inline mr-1" />
                    Đã xác nhận
                  </>
                ) : appointment.status === "Pending" ? (
                  <>
                    <Clock className="w-3 h-3 inline mr-1" />
                    Chờ xác nhận
                  </>
                ) : appointment.status === "Cancelled" ? (
                  <>
                    <XCircle className="w-3 h-3 inline mr-1" />
                    Đã hủy
                  </>
                ) : appointment.status === "Completed" ? (
                  <>
                    <CheckCircle className="w-3 h-3 inline mr-1" />
                    Hoàn thành
                  </>
                ) : (
                  appointment.status || appointment.appointmentStatus
                )}
              </span>
            </td>
            <td className="px-4 py-2 flex gap-1 flex-wrap">
              <button
                className="bg-blue-600 hover:bg-blue-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onEdit(appointment)}
              >
                Sửa
              </button>
              <button
                className="bg-gray-200 dark:bg-gray-700 dark:text-white hover:bg-red-500 hover:text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                onClick={() => onDelete(appointment.id)}
              >
                Xóa
              </button>
              {appointment.appointmentStatus === "Pending" && (
                <button
                  className="bg-green-600 hover:bg-green-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                  onClick={() => onConfirm(appointment.id)}
                >
                  Xác nhận
                </button>
              )}
              {(appointment.appointmentStatus === "Pending" ||
                appointment.appointmentStatus === "Confirmed") && (
                <button
                  className="bg-red-600 hover:bg-red-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                  onClick={() => onCancel(appointment.id)}
                >
                  Hủy
                </button>
              )}
              {appointment.appointmentStatus === "Confirmed" && (
                <button
                  className="bg-blue-600 hover:bg-blue-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                  onClick={() => onComplete(appointment.id)}
                >
                  Hoàn thành
                </button>
              )}
              {(appointment.appointmentStatus === "Pending" ||
                appointment.appointmentStatus === "Confirmed") && (
                <button
                  className="bg-purple-600 hover:bg-purple-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                  onClick={() => onReschedule(appointment.id)}
                >
                  Đổi lịch
                </button>
              )}
            </td>
          </tr>
        ))
      )}
    </tbody>
  </table>
);

export default AppointmentTable;
