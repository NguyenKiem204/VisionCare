import React from "react";
import { Link } from "react-router-dom";
import { formatCurrency } from "../../../utils/helpers";
import { formatDateTime, formatDate } from "../../../utils/formatDate";
import { classNames } from "../../../utils/helpers";

const STATUS_STYLES = {
  Pending: "bg-blue-100 text-blue-700",
  Confirmed: "bg-emerald-100 text-emerald-700",
  Completed: "bg-teal-100 text-teal-700",
  Cancelled: "bg-rose-100 text-rose-700",
};

const BookingHistoryTab = ({
  bookings,
  loading,
  statusFilter,
  timeFilter,
  onStatusChange,
  onTimeChange,
  onRefresh,
  page,
  pageCount,
  onPageChange,
  error,
}) => {
  const renderSkeleton = () => (
    <div className="grid gap-4 sm:grid-cols-2">
      {Array.from({ length: 4 }).map((_, index) => (
        <div
          key={`skeleton-${index}`}
          className="rounded-2xl border border-slate-100 bg-white p-6 shadow-sm dark:border-slate-800 dark:bg-slate-900 animate-pulse"
        >
          <div className="flex items-center justify-between mb-4">
            <div className="h-4 w-32 rounded bg-slate-200 dark:bg-slate-700" />
            <div className="h-6 w-20 rounded-full bg-slate-200 dark:bg-slate-700" />
          </div>
          <div className="space-y-3">
            <div className="h-5 w-2/3 rounded bg-slate-200 dark:bg-slate-700" />
            <div className="h-4 w-1/2 rounded bg-slate-200 dark:bg-slate-700" />
            <div className="h-4 w-1/3 rounded bg-slate-200 dark:bg-slate-700" />
          </div>
        </div>
      ))}
    </div>
  );

  const renderEmpty = () => (
    <div className="flex flex-col items-center justify-center rounded-3xl border border-dashed border-slate-200 bg-white py-16 text-center dark:border-slate-700 dark:bg-slate-900">
      <h3 className="text-lg font-semibold text-slate-800 dark:text-white">
        Chưa có lịch sử cuộc hẹn
      </h3>
      <p className="mt-2 max-w-md text-sm text-slate-500 dark:text-slate-400">
        Hãy đặt lịch khám đầu tiên của bạn ngay hôm nay để VisionCare đồng hành
        chăm sóc sức khỏe đôi mắt.
      </p>
      <div className="mt-6 flex flex-wrap justify-center gap-3">
        <Link
          to="/booking"
          className="inline-flex items-center rounded-full bg-gradient-to-r from-yellow-400 to-orange-500 px-5 py-2.5 text-sm font-semibold text-white shadow-md transition hover:from-yellow-500 hover:to-orange-600"
        >
          Đặt lịch ngay
        </Link>
        <button
          onClick={onRefresh}
          className="inline-flex items-center rounded-full border border-slate-200 px-5 py-2.5 text-sm font-medium text-slate-600 transition hover:border-slate-300 hover:text-slate-800 dark:border-slate-700 dark:text-slate-300 dark:hover:border-slate-600"
        >
          Thử tải lại
        </button>
      </div>
    </div>
  );

  const renderBookings = () => (
    <div className="grid gap-5 md:grid-cols-2">
      {bookings.map((booking) => {
        const status = booking.status || "Pending";
        const statusClass =
          STATUS_STYLES[status] || "bg-slate-100 text-slate-600";

        return (
          <div
            key={booking.appointmentId}
            className="group flex h-full flex-col justify-between rounded-2xl border border-slate-100 bg-white p-6 shadow-sm transition hover:-translate-y-1 hover:shadow-xl dark:border-slate-800 dark:bg-slate-900"
          >
            <div>
              <div className="mb-4 flex items-start justify-between gap-3">
                <div>
                  <p className="text-xs uppercase tracking-wide text-slate-400">
                    Mã đặt lịch
                  </p>
                  <p className="text-sm font-semibold text-slate-800 dark:text-white">
                    {booking.appointmentCode || "—"}
                  </p>
                </div>
                <span
                  className={classNames(
                    "rounded-full px-3 py-1 text-xs font-semibold",
                    statusClass
                  )}
                >
                  {status === "Pending"
                    ? "Chờ xác nhận"
                    : status === "Confirmed"
                    ? "Đã xác nhận"
                    : status === "Completed"
                    ? "Hoàn thành"
                    : status === "Cancelled"
                    ? "Đã hủy"
                    : status}
                </span>
              </div>

              <div className="mb-5 space-y-3 text-sm text-slate-600 dark:text-slate-300">
                <div className="flex items-start gap-3 rounded-xl bg-slate-50 p-3 dark:bg-slate-800/60">
                  <div className="flex-1">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Thời gian khám
                    </p>
                    <p className="font-semibold text-slate-800 dark:text-white">
                      {formatDateTime(booking.appointmentDate)}
                    </p>
                  </div>
                </div>

                <div className="grid grid-cols-1 gap-3 lg:grid-cols-2">
                  <div className="rounded-xl bg-slate-50 p-3 dark:bg-slate-800/60">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Bác sĩ
                    </p>
                    <p className="font-medium text-slate-700 dark:text-slate-200">
                      {booking.doctorName || "Đang cập nhật"}
                    </p>
                    {booking.doctorSpecialization && (
                      <p className="text-xs text-slate-500 dark:text-slate-400">
                        Chuyên khoa: {booking.doctorSpecialization}
                      </p>
                    )}
                  </div>
                  <div className="rounded-xl bg-slate-50 p-3 dark:bg-slate-800/60">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Dịch vụ
                    </p>
                    <p className="font-medium text-slate-700 dark:text-slate-200">
                      {booking.serviceName}
                    </p>
                    {booking.serviceTypeName && (
                      <p className="text-xs text-slate-500 dark:text-slate-400">
                        Gói: {booking.serviceTypeName}
                      </p>
                    )}
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-3 text-xs">
                  <div>
                    <p className="uppercase tracking-wide text-slate-400">
                      Trạng thái thanh toán
                    </p>
                    <p className="mt-1 text-sm font-semibold text-slate-700 dark:text-slate-200">
                      {booking.paymentStatus === "Paid"
                        ? "Đã thanh toán"
                        : booking.paymentStatus === "Refunded"
                        ? "Đã hoàn tiền"
                        : "Chưa thanh toán"}
                    </p>
                  </div>
                  <div>
                    <p className="uppercase tracking-wide text-slate-400">
                      Chi phí
                    </p>
                    <p className="mt-1 text-sm font-semibold text-slate-700 dark:text-slate-200">
                      {booking.totalAmount
                        ? formatCurrency(booking.totalAmount)
                        : "—"}
                    </p>
                  </div>
                </div>

                {booking.notes && (
                  <div className="rounded-xl border border-slate-100 bg-white p-3 text-sm italic text-slate-500 shadow-inner dark:border-slate-700 dark:bg-slate-900/80 dark:text-slate-400">
                    Ghi chú: {booking.notes}
                  </div>
                )}
              </div>
            </div>

            <div className="flex items-center justify-between border-t border-slate-100 pt-4 text-xs text-slate-500 dark:border-slate-800 dark:text-slate-400">
              <span>
                Tạo lúc {formatDate(booking.createdAt)} • Mã dịch vụ #
                {booking.serviceDetailId}
              </span>
              <Link
                to="/booking"
                className="text-sm font-semibold text-yellow-600 transition hover:text-yellow-500"
              >
                Đặt lại lịch
              </Link>
            </div>
          </div>
        );
      })}
    </div>
  );

  const totalPages = Math.max(pageCount, 1);

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
        <div className="flex flex-wrap items-center gap-2">
          {["all", "pending", "confirmed", "completed", "cancelled"].map(
            (option) => (
              <button
                key={option}
                onClick={() => onStatusChange(option)}
                className={classNames(
                  "rounded-full border px-4 py-2 text-sm font-medium transition",
                  statusFilter === option
                    ? "border-yellow-500 bg-yellow-500 text-white shadow"
                    : "border-slate-200 bg-white text-slate-600 hover:border-yellow-400 hover:text-yellow-600 dark:border-slate-700 dark:bg-slate-900 dark:text-slate-300"
                )}
              >
                {option === "all"
                  ? "Tất cả"
                  : option === "pending"
                  ? "Chờ xác nhận"
                  : option === "confirmed"
                  ? "Đã xác nhận"
                  : option === "completed"
                  ? "Hoàn thành"
                  : "Đã hủy"}
              </button>
            )
          )}
        </div>

        <div className="flex items-center gap-2">
          {["all", "upcoming", "past"].map((option) => (
            <button
              key={option}
              onClick={() => onTimeChange(option)}
              className={classNames(
                "rounded-full border px-4 py-2 text-sm font-medium transition",
                timeFilter === option
                  ? "border-slate-900 bg-slate-900 text-white shadow dark:border-yellow-500 dark:bg-yellow-500 dark:text-slate-900"
                  : "border-slate-200 bg-white text-slate-600 hover:border-slate-300 hover:text-slate-900 dark:border-slate-700 dark:bg-slate-900 dark:text-slate-300"
              )}
            >
              {option === "all"
                ? "Tất cả thời gian"
                : option === "upcoming"
                ? "Sắp tới"
                : "Đã khám"}
            </button>
          ))}
          <button
            onClick={onRefresh}
            className="ml-2 rounded-full border border-slate-200 px-4 py-2 text-sm font-semibold text-slate-600 transition hover:border-yellow-400 hover:text-yellow-600 dark:border-slate-700 dark:text-slate-300"
          >
            Làm mới
          </button>
        </div>
      </div>

      {error && (
        <div className="rounded-xl border border-rose-200 bg-rose-50 p-4 text-sm text-rose-700 dark:border-rose-900 dark:bg-rose-950/70 dark:text-rose-300">
          {error}
        </div>
      )}

      {loading ? renderSkeleton() : bookings.length === 0 ? renderEmpty() : renderBookings()}

      {bookings.length > 0 && (
        <div className="flex items-center justify-between rounded-2xl border border-slate-100 bg-white px-4 py-3 text-sm shadow-sm dark:border-slate-800 dark:bg-slate-900">
          <span className="text-slate-500 dark:text-slate-400">
            Trang {page} / {totalPages}
          </span>
          <div className="flex items-center gap-2">
            <button
              onClick={() => onPageChange(Math.max(1, page - 1))}
              disabled={page <= 1}
              className="rounded-full border border-slate-200 px-3 py-1.5 text-xs font-semibold text-slate-500 transition enabled:hover:border-yellow-500 enabled:hover:text-yellow-600 disabled:opacity-50 dark:border-slate-700 dark:text-slate-300"
            >
              Trước
            </button>
            <button
              onClick={() => onPageChange(Math.min(totalPages, page + 1))}
              disabled={page >= totalPages}
              className="rounded-full border border-slate-200 px-3 py-1.5 text-xs font-semibold text-slate-500 transition enabled:hover:border-yellow-500 enabled:hover:text-yellow-600 disabled:opacity-50 dark:border-slate-700 dark:text-slate-300"
            >
              Sau
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default BookingHistoryTab;

