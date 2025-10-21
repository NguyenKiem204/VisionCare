import React from "react";
import WeeklyScheduleStatusBadge from "./WeeklyScheduleStatusBadge";
import { statusConfig } from "./weeklyScheduleStatusConfig";
import { FaEye, FaEdit, FaTrash } from "react-icons/fa";
import { formatDate } from "../../../utils/formatDate";

const WeeklyScheduleTable = ({
  schedules = [],
  loading,
  onEdit,
  onView,
  onDelete,
  pagination,
  setPagination,
  onChangeStatus, // thêm prop này
}) => {
  const totalPages = Math.ceil(
    (pagination?.total || 0) / (pagination?.size || 10)
  );
  return (
    <div className="overflow-x-auto rounded-2xl shadow-lg">
      <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700 text-sm">
        <thead className="bg-gray-100 dark:bg-gray-900">
          <tr>
            <th className="px-3 py-2 text-center font-bold dark:text-white border-b">
              ID
            </th>
            <th className="px-3 py-2 text-center font-bold dark:text-white border-b">
              Tuần
            </th>
            <th className="px-3 py-2 text-center font-bold dark:text-white border-b">
              Ngày bắt đầu - Ngày kết thúc
            </th>
            <th className="px-3 py-2 text-left font-bold dark:text-white border-b">
              Tiêu đề
            </th>
            <th className="px-3 py-2 text-center font-bold dark:text-white border-b">
              Trạng thái
            </th>
            <th className="px-3 py-2 text-left font-bold dark:text-white border-b">
              Người tạo
            </th>
            <th className="px-3 py-2 text-center font-bold dark:text-white border-b">
              Ngày tạo
            </th>
            <th className="px-3 py-2 text-center font-bold dark:text-white border-b">
              Thao tác
            </th>
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={8} className="text-center py-8 dark:text-white">
                Đang tải...
              </td>
            </tr>
          ) : schedules.length === 0 ? (
            <tr>
              <td colSpan={8} className="text-center py-8 dark:text-white">
                Không có dữ liệu
              </td>
            </tr>
          ) : (
            schedules.map((s) => (
              <tr
                key={s.id}
                className="hover:bg-gray-50 dark:hover:bg-gray-800 transition border-b last:border-b-0"
              >
                <td className="px-3 py-2 text-center font-semibold dark:text-white">
                  {s.id}
                </td>
                <td className="px-3 py-2 text-center dark:text-white">
                  Tuần {s.weekNumber}, {s.year}
                </td>
                <td className="px-3 py-2 text-center dark:text-white">
                  {formatDate(s.weekStartDate)} - {formatDate(s.weekEndDate)}
                </td>
                <td className="px-3 py-2 text-left dark:text-white">
                  {s.title}
                </td>
                <td className="px-3 py-2 text-center">
                  <WeeklyScheduleStatusBadge status={s.status} />
                </td>
                <td className="px-3 py-2 text-left dark:text-white">
                  {s.createdBy}
                </td>
                <td className="px-3 py-2 text-center dark:text-white">
                  {formatDate(s.createdAt)}
                </td>
                <td className="px-3 py-2 flex flex-nowrap gap-2 justify-center items-center">
                  <button
                    className="flex items-center gap-1 px-2 py-1 rounded bg-blue-100 hover:bg-blue-200 text-blue-700 dark:bg-blue-900 dark:hover:bg-blue-800 dark:text-blue-300 transition"
                    title="Xem"
                    onClick={() => onView(s)}
                  >
                    <FaEye /> <span className="hidden sm:inline">Xem</span>
                  </button>
                  <button
                    className="flex items-center gap-1 px-2 py-1 rounded bg-yellow-100 hover:bg-yellow-200 text-yellow-700 dark:bg-yellow-900 dark:hover:bg-yellow-800 dark:text-yellow-300 transition"
                    title="Sửa"
                    onClick={() => onEdit(s)}
                  >
                    <FaEdit /> <span className="hidden sm:inline">Sửa</span>
                  </button>
                  <button
                    className="flex items-center gap-1 px-2 py-1 rounded bg-red-100 hover:bg-red-200 text-red-700 dark:bg-red-900 dark:hover:bg-red-800 dark:text-red-300 transition"
                    title="Xóa"
                    onClick={() => onDelete(s.id)}
                  >
                    <FaTrash /> <span className="hidden sm:inline">Xóa</span>
                  </button>
                  {statusConfig[s.status]?.button && (
                    <button
                      className={`w-24 flex items-center gap-1 px-2 py-1 rounded transition ${
                        statusConfig[s.status].button.color
                      }`}
                      title={statusConfig[s.status].button.label}
                      onClick={() =>
                        onChangeStatus &&
                        onChangeStatus(
                          s.id,
                          statusConfig[s.status].button.nextStatus
                        )
                      }
                    >
                      {React.createElement(statusConfig[s.status].button.icon)}
                      <span className="hidden sm:inline">
                        {statusConfig[s.status].button.label}
                      </span>
                    </button>
                  )}
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4 pb-4 px-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số:{" "}
          <span className="font-semibold dark:text-white">
            {pagination?.total || 0}
          </span>{" "}
          lịch tuần
        </div>
        <div className="flex items-center gap-2">
          <button
            className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => setPagination((p) => ({ ...p, page: 0 }))}
            disabled={pagination?.page === 0}
          >
            Đầu
          </button>
          <button
            className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() =>
              setPagination((p) => ({
                ...p,
                page: Math.max(0, (p.page || 0) - 1),
              }))
            }
            disabled={pagination?.page === 0}
          >
            Trước
          </button>
          <span className="mx-2 dark:text-gray-200">
            Trang{" "}
            <span className="font-semibold dark:text-white">
              {(pagination?.page || 0) + 1}
            </span>{" "}
            / {totalPages || 1}
          </span>
          <button
            className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() =>
              setPagination((p) => ({
                ...p,
                page: Math.min(totalPages - 1, (p.page || 0) + 1),
              }))
            }
            disabled={(pagination?.page || 0) + 1 >= totalPages}
          >
            Sau
          </button>
          <button
            className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() =>
              setPagination((p) => ({ ...p, page: totalPages - 1 }))
            }
            disabled={(pagination?.page || 0) + 1 >= totalPages}
          >
            Cuối
          </button>
        </div>
      </div>
    </div>
  );
};

export default WeeklyScheduleTable;
