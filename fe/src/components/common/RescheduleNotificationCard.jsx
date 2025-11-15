import React from "react";
import { Clock, User, MessageSquare } from "lucide-react";
import { formatDateTime } from "../../utils/formatDate";

const RescheduleNotificationCard = ({
  appointment,
  requestedBy,
  proposedDateTime,
  reason,
  onApprove,
  onReject,
  onCounterPropose,
  isLoading = false,
  currentUserRole = null, // "Doctor" or "Customer"
}) => {
  const isFromDoctor = requestedBy === "Doctor";
  const isFromCustomer = requestedBy === "Customer";
  
  // Chỉ hiển thị nút approve/reject nếu người dùng hiện tại KHÔNG phải người request
  // Staff có thể approve/reject cho cả 2 bên
  const canApproveReject = 
    currentUserRole === "Staff" ||
    currentUserRole === "Admin" ||
    (isFromCustomer && currentUserRole === "Doctor") ||
    (isFromDoctor && currentUserRole === "Customer");

  return (
    <div className="rounded-lg border-2 border-yellow-400 bg-yellow-50 p-4 dark:border-yellow-600 dark:bg-yellow-900/20">
      <div className="flex items-start gap-3">
        <div className="rounded-full bg-yellow-100 p-2 dark:bg-yellow-900/40">
          <Clock className="h-5 w-5 text-yellow-600 dark:text-yellow-400" />
        </div>
        <div className="flex-1">
          <h3 className="font-semibold text-yellow-900 dark:text-yellow-100">
            Đề xuất đổi lịch
          </h3>
          <p className="mt-1 text-sm text-yellow-800 dark:text-yellow-200">
            {isFromDoctor && "Bác sĩ"} {isFromCustomer && "Khách hàng"} {requestedBy === "Counter" && "Bên kia"} đề xuất đổi lịch khám
            {isFromDoctor && " - Khách hàng có thể chấp nhận, từ chối hoặc đề xuất thời gian khác"}
            {isFromCustomer && " - Bác sĩ có thể chấp nhận, từ chối hoặc đề xuất thời gian khác"}
          </p>

          <div className="mt-3 space-y-2">
            <div className="flex items-center gap-2 text-sm">
              <Clock className="h-4 w-4 text-yellow-600 dark:text-yellow-400" />
              <span className="text-yellow-900 dark:text-yellow-100">
                Thời gian đề xuất: <strong>{formatDateTime(proposedDateTime)}</strong>
              </span>
            </div>

            {reason && (
              <div className="flex items-start gap-2 text-sm">
                <MessageSquare className="h-4 w-4 text-yellow-600 dark:text-yellow-400 mt-0.5" />
                <span className="text-yellow-800 dark:text-yellow-200">
                  Lý do: {reason}
                </span>
              </div>
            )}
          </div>

          <div className="mt-4 flex flex-wrap gap-2">
            {canApproveReject && (
              <>
                <button
                  onClick={onApprove}
                  disabled={isLoading}
                  className="rounded-md bg-green-600 px-4 py-2 text-sm font-medium text-white hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Chấp nhận
                </button>
                <button
                  onClick={onReject}
                  disabled={isLoading}
                  className="rounded-md bg-red-600 px-4 py-2 text-sm font-medium text-white hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Từ chối
                </button>
              </>
            )}
            {canApproveReject && (
              <button
                onClick={onCounterPropose}
                disabled={isLoading}
                className="rounded-md border border-yellow-600 bg-white px-4 py-2 text-sm font-medium text-yellow-700 hover:bg-yellow-50 dark:border-yellow-500 dark:bg-yellow-900/40 dark:text-yellow-200 dark:hover:bg-yellow-900/60 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                Đề xuất thời gian khác
              </button>
            )}
            {!canApproveReject && (
              <div className="text-sm text-yellow-800 dark:text-yellow-200 italic">
                Đang chờ {isFromDoctor ? "khách hàng" : "bác sĩ"} phê duyệt...
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default RescheduleNotificationCard;

