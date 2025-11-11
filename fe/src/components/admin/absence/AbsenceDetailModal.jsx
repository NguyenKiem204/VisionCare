import React, { useState, useEffect } from "react";
import { X, Calendar, FileText, User, Clock, AlertCircle, CheckCircle, XCircle } from "lucide-react";
import { getAbsenceById } from "../../../services/adminDoctorAbsenceAPI";

const AbsenceDetailModal = ({ open, onClose, absence: initialAbsence }) => {
  const [absence, setAbsence] = useState(initialAbsence);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (open && initialAbsence?.id) {
      loadAbsence();
    } else if (initialAbsence) {
      setAbsence(initialAbsence);
    }
  }, [open, initialAbsence]);

  const loadAbsence = async () => {
    if (!initialAbsence?.id) return;
    setLoading(true);
    try {
      const response = await getAbsenceById(initialAbsence.id);
      setAbsence(response.data?.data || initialAbsence);
    } catch (error) {
      console.error("Error loading absence:", error);
    } finally {
      setLoading(false);
    }
  };

  if (!open || !absence) return null;

  const getStatusBadge = (status, isResolved) => {
    const statusLower = status?.toLowerCase() || "";
    if (statusLower === "approved") {
      return (
        <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200">
          <CheckCircle className="h-4 w-4 mr-1" />
          Đã duyệt {isResolved ? "(Đã xử lý)" : "(Chưa xử lý)"}
        </span>
      );
    } else if (statusLower === "pending") {
      return (
        <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200">
          <AlertCircle className="h-4 w-4 mr-1" />
          Chờ duyệt
        </span>
      );
    } else if (statusLower === "rejected") {
      return (
        <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200">
          <XCircle className="h-4 w-4 mr-1" />
          Đã từ chối
        </span>
      );
    }
    return null;
  };

  const getAbsenceTypeLabel = (type) => {
    const labels = {
      Leave: "Nghỉ phép",
      Emergency: "Khẩn cấp",
      Sick: "Ốm đau",
      Other: "Khác",
    };
    return labels[type] || type;
  };

  const daysCount = absence.startDate && absence.endDate
    ? Math.ceil(
        (new Date(absence.endDate) - new Date(absence.startDate)) /
          (1000 * 60 * 60 * 24)
      ) + 1
    : 0;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
            <FileText className="h-6 w-6 text-blue-600" />
            Chi tiết đơn nghỉ phép
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        {/* Content */}
        {loading ? (
          <div className="p-12 text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600 dark:text-gray-400">Đang tải...</p>
          </div>
        ) : (
          <div className="p-6 space-y-6">
            {/* Status */}
            <div className="flex items-center justify-between">
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300">
                Trạng thái:
              </span>
              {getStatusBadge(absence.status, absence.isResolved)}
            </div>

            {/* Doctor */}
            <div className="flex items-start gap-4 p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
              <User className="h-5 w-5 text-gray-400 mt-0.5" />
              <div className="flex-1">
                <p className="text-sm font-medium text-gray-700 dark:text-gray-300">
                  Bác sĩ
                </p>
                <p className="text-lg font-semibold text-gray-900 dark:text-white mt-1">
                  {absence.doctorName || "Không rõ"}
                </p>
              </div>
            </div>

            {/* Date Range */}
            <div className="grid grid-cols-2 gap-4">
              <div className="p-4 bg-blue-50 dark:bg-blue-900/20 rounded-lg border border-blue-200 dark:border-blue-800">
                <div className="flex items-center gap-2 mb-2">
                  <Calendar className="h-4 w-4 text-blue-600" />
                  <span className="text-sm font-medium text-blue-800 dark:text-blue-200">
                    Ngày bắt đầu
                  </span>
                </div>
                <p className="text-lg font-semibold text-blue-900 dark:text-blue-100">
                  {new Date(absence.startDate).toLocaleDateString("vi-VN", {
                    weekday: "long",
                    year: "numeric",
                    month: "long",
                    day: "numeric",
                  })}
                </p>
              </div>

              <div className="p-4 bg-red-50 dark:bg-red-900/20 rounded-lg border border-red-200 dark:border-red-800">
                <div className="flex items-center gap-2 mb-2">
                  <Calendar className="h-4 w-4 text-red-600" />
                  <span className="text-sm font-medium text-red-800 dark:text-red-200">
                    Ngày kết thúc
                  </span>
                </div>
                <p className="text-lg font-semibold text-red-900 dark:text-red-100">
                  {new Date(absence.endDate).toLocaleDateString("vi-VN", {
                    weekday: "long",
                    year: "numeric",
                    month: "long",
                    day: "numeric",
                  })}
                </p>
              </div>
            </div>

            {/* Days Count */}
            <div className="p-4 bg-purple-50 dark:bg-purple-900/20 rounded-lg border border-purple-200 dark:border-purple-800">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Clock className="h-5 w-5 text-purple-600" />
                  <span className="text-sm font-medium text-purple-800 dark:text-purple-200">
                    Tổng số ngày nghỉ
                  </span>
                </div>
                <span className="text-2xl font-bold text-purple-900 dark:text-purple-100">
                  {daysCount} ngày
                </span>
              </div>
            </div>

            {/* Absence Type */}
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Loại nghỉ phép
              </label>
              <div className="px-4 py-2 bg-gray-50 dark:bg-gray-700 rounded-lg">
                <p className="text-gray-900 dark:text-white">
                  {getAbsenceTypeLabel(absence.absenceType)}
                </p>
              </div>
            </div>

            {/* Reason */}
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                <FileText className="h-4 w-4 inline mr-2" />
                Lý do
              </label>
              <div className="px-4 py-3 bg-gray-50 dark:bg-gray-700 rounded-lg min-h-[100px]">
                <p className="text-gray-900 dark:text-white whitespace-pre-wrap">
                  {absence.reason || "Không có lý do"}
                </p>
              </div>
            </div>

            {/* Metadata */}
            <div className="grid grid-cols-2 gap-4 pt-4 border-t border-gray-200 dark:border-gray-700">
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400">Ngày tạo</p>
                <p className="text-sm font-medium text-gray-900 dark:text-white mt-1">
                  {new Date(absence.created).toLocaleString("vi-VN")}
                </p>
              </div>
              {absence.lastModified && (
                <div>
                  <p className="text-xs text-gray-500 dark:text-gray-400">
                    Cập nhật lần cuối
                  </p>
                  <p className="text-sm font-medium text-gray-900 dark:text-white mt-1">
                    {new Date(absence.lastModified).toLocaleString("vi-VN")}
                  </p>
                </div>
              )}
            </div>
          </div>
        )}

        {/* Footer */}
        <div className="flex items-center justify-end p-6 border-t border-gray-200 dark:border-gray-700">
          <button
            onClick={onClose}
            className="px-6 py-2 bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 text-gray-700 dark:text-gray-300 rounded-lg transition-colors font-medium"
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default AbsenceDetailModal;

