import React from "react";
import { CheckCircle, XCircle, Clock } from "lucide-react";

const ServiceTable = ({
  services,
  loading,
  onEdit,
  onDelete,
  onActivate,
  onDeactivate,
}) => {
  const rows = Array.isArray(services)
    ? services
    : services?.data?.data ?? services?.data ?? services?.items ?? [];

  return (
    <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700 rounded-xl overflow-hidden shadow-lg">
      <thead className="bg-gray-100 dark:bg-gray-800">
        <tr>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            ID
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Tên dịch vụ
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Giá
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Thời gian
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Chuyên khoa
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
        ) : rows.length === 0 ? (
          <tr>
            <td colSpan="7" className="text-center py-6 text-gray-400">
              Không có dữ liệu
            </td>
          </tr>
        ) : (
          rows.map((service, idx) => (
            <tr
              key={service.id}
              className={`text-sm ${
                idx % 2 === 0
                  ? "bg-white dark:bg-gray-900"
                  : "bg-gray-50 dark:bg-gray-800"
              } border-b border-gray-100 dark:border-gray-800 hover:bg-blue-50 dark:hover:bg-gray-700 transition-colors duration-200`}
            >
              <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                {service.id}
              </td>
              <td className="px-4 py-2 text-gray-800 dark:text-gray-100 font-medium">
                {service.name || service.serviceName}
              </td>
              <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                {(() => {
                  const min = service.minPrice ?? service.price;
                  const max = service.maxPrice ?? service.price;
                  if (min == null && max == null) return "-";
                  const fmt = (v) =>
                    new Intl.NumberFormat("vi-VN", {
                      style: "currency",
                      currency: "VND",
                    }).format(v);
                  if (min != null && max != null && min !== max) {
                    return `${fmt(min)} - ${fmt(max)}`;
                  }
                  const val = min ?? max;
                  return fmt(val);
                })()}
              </td>
              <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                <div className="flex items-center gap-1">
                  <Clock className="w-4 h-4 text-blue-500" />
                  <span>
                    {(() => {
                      const min = service.minDuration ?? service.duration;
                      const max = service.maxDuration ?? service.duration;
                      if (min == null && max == null) return "-";
                      if (min != null && max != null && min !== max) {
                        return `${min} - ${max} phút`;
                      }
                      const val = min ?? max ?? 0;
                      return `${val} phút`;
                    })()}
                  </span>
                </div>
              </td>
              <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                {service.specializationName || "Chưa có chuyên khoa"}
              </td>
              <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                <span
                  className={`px-2 py-1 rounded-full text-xs font-medium ${
                    service.isActive
                      ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200"
                      : "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200"
                  }`}
                >
                  {service.isActive ? (
                    <>
                      <CheckCircle className="w-3 h-3 inline mr-1" />
                      Hoạt động
                    </>
                  ) : (
                    <>
                      <XCircle className="w-3 h-3 inline mr-1" />
                      Không hoạt động
                    </>
                  )}
                </span>
              </td>
              <td className="px-4 py-2 flex gap-1 flex-wrap">
                <button
                  className="bg-blue-600 hover:bg-blue-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                  onClick={() => onEdit(service)}
                >
                  Sửa
                </button>
                <button
                  className="bg-gray-200 dark:bg-gray-700 dark:text-white hover:bg-red-500 hover:text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                  onClick={() => onDelete(service.id)}
                >
                  Xóa
                </button>
                {service.isActive ? (
                  <button
                    className="bg-orange-600 hover:bg-orange-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                    onClick={() => onDeactivate(service.id)}
                  >
                    Vô hiệu
                  </button>
                ) : (
                  <button
                    className="bg-green-600 hover:bg-green-700 text-white px-2 py-1 rounded shadow text-xs font-semibold transition"
                    onClick={() => onActivate(service.id)}
                  >
                    Kích hoạt
                  </button>
                )}
              </td>
            </tr>
          ))
        )}
      </tbody>
    </table>
  );
};

export default ServiceTable;
