import React, { useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import Button from "../components/common/Button";
import Loading from "../components/common/Loading";
import { bookingAPI } from "../services/bookingAPI";
import toast from "react-hot-toast";

const BookingStatus = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [appointmentCode, setAppointmentCode] = useState(
    searchParams.get("code") || ""
  );
  const [phone, setPhone] = useState(searchParams.get("phone") || "");
  const [loading, setLoading] = useState(false);
  const [booking, setBooking] = useState(null);
  const [error, setError] = useState(null);

  const handleSearch = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setBooking(null);

    try {
      let result;
      if (appointmentCode.trim()) {
        result = await bookingAPI.searchBooking(appointmentCode.trim(), null);
      } else if (phone.trim()) {
        result = await bookingAPI.searchBooking(null, phone.trim());
      } else {
        toast.error("Vui lòng nhập mã đặt lịch hoặc số điện thoại");
        setLoading(false);
        return;
      }

      setBooking(result);
      toast.success("Tìm thấy thông tin đặt lịch");
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Không tìm thấy thông tin đặt lịch";
      setError(errorMessage);
      toast.error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  const handleCancelBooking = async () => {
    if (!booking || !confirm("Bạn có chắc chắn muốn hủy đặt lịch này?")) {
      return;
    }

    try {
      setLoading(true);
      await bookingAPI.cancelBooking(booking.id, {
        reason: "Khách hàng yêu cầu hủy",
        requestRefund: booking.paymentStatus === "Paid",
      });

      toast.success("Đã hủy đặt lịch thành công");
      // Refresh booking status
      if (booking.appointmentCode) {
        const result = await bookingAPI.searchBooking(booking.appointmentCode, null);
        setBooking(result);
      }
    } catch (error) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Không thể hủy đặt lịch";
      toast.error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return "N/A";
    const date = new Date(dateStr);
    return date.toLocaleString("vi-VN", {
      weekday: "long",
      year: "numeric",
      month: "long",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const formatPrice = (price) => {
    if (!price) return "Liên hệ";
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(price);
  };

  const getStatusBadge = (status) => {
    const statusColors = {
      Scheduled: "bg-blue-100 text-blue-800",
      Confirmed: "bg-green-100 text-green-800",
      Completed: "bg-gray-100 text-gray-800",
      Cancelled: "bg-red-100 text-red-800",
      Rescheduled: "bg-yellow-100 text-yellow-800",
    };

    const statusLabels = {
      Scheduled: "Đã đặt lịch",
      Confirmed: "Đã xác nhận",
      Completed: "Hoàn thành",
      Cancelled: "Đã hủy",
      Rescheduled: "Đã dời lịch",
    };

    const color = statusColors[status] || "bg-gray-100 text-gray-800";
    const label = statusLabels[status] || status;

    return (
      <span
        className={`px-3 py-1 rounded-full text-sm font-medium ${color}`}
      >
        {label}
      </span>
    );
  };

  const getPaymentStatusBadge = (paymentStatus) => {
    const statusColors = {
      Unpaid: "bg-yellow-100 text-yellow-800",
      Paid: "bg-green-100 text-green-800",
      Failed: "bg-red-100 text-red-800",
      Refunding: "bg-blue-100 text-blue-800",
      Refunded: "bg-gray-100 text-gray-800",
    };

    const statusLabels = {
      Unpaid: "Chưa thanh toán",
      Paid: "Đã thanh toán",
      Failed: "Thanh toán thất bại",
      Refunding: "Đang hoàn tiền",
      Refunded: "Đã hoàn tiền",
    };

    const color = statusColors[paymentStatus] || "bg-gray-100 text-gray-800";
    const label = statusLabels[paymentStatus] || paymentStatus;

    return (
      <span
        className={`px-3 py-1 rounded-full text-sm font-medium ${color}`}
      >
        {label}
      </span>
    );
  };

  return (
    <div className="min-h-screen bg-gray-50 py-12">
      <div className="container mx-auto px-4 max-w-4xl">
        <div className="bg-white rounded-2xl shadow-lg p-6 md:p-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            Tra cứu đặt lịch
          </h1>
          <p className="text-gray-600 mb-6">
            Nhập mã đặt lịch hoặc số điện thoại để tra cứu thông tin
          </p>

          {/* Search Form */}
          <form onSubmit={handleSearch} className="mb-8">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Mã đặt lịch (VD: VC-20250101-123456)
                </label>
                <input
                  type="text"
                  value={appointmentCode}
                  onChange={(e) => setAppointmentCode(e.target.value)}
                  placeholder="VC-YYYYMMDD-XXXXXX"
                  className="w-full border rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                  disabled={loading}
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Hoặc số điện thoại
                </label>
                <input
                  type="tel"
                  value={phone}
                  onChange={(e) => setPhone(e.target.value)}
                  placeholder="09xx xxx xxx"
                  className="w-full border rounded-lg px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                  disabled={loading}
                />
              </div>
            </div>
            <Button
              type="submit"
              variant="accent"
              disabled={loading}
              className="w-full md:w-auto"
            >
              {loading ? "Đang tìm kiếm..." : "Tra cứu"}
            </Button>
          </form>

          {/* Error Message */}
          {error && (
            <div className="mb-6 p-4 bg-red-50 border border-red-200 rounded-lg">
              <p className="text-red-700">{error}</p>
            </div>
          )}

          {/* Loading */}
          {loading && !booking && <Loading />}

          {/* Booking Details */}
          {booking && (
            <div className="space-y-6">
              <div className="border-t pt-6">
                <h2 className="text-xl font-semibold mb-4">
                  Thông tin đặt lịch
                </h2>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
                  <div className="p-4 bg-gray-50 rounded-lg">
                    <p className="text-sm text-gray-600 mb-1">Mã đặt lịch</p>
                    <p className="font-semibold text-lg">
                      {booking.appointmentCode}
                    </p>
                  </div>
                  <div className="p-4 bg-gray-50 rounded-lg">
                    <p className="text-sm text-gray-600 mb-1">Trạng thái</p>
                    {getStatusBadge(booking.status)}
                  </div>
                </div>

                <div className="space-y-4">
                  <div className="p-4 bg-blue-50 rounded-lg">
                    <p className="text-sm text-gray-600 mb-1">Thời gian khám</p>
                    <p className="font-semibold">{formatDate(booking.appointmentDate)}</p>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div className="p-4 bg-white border rounded-lg">
                      <p className="text-sm text-gray-600 mb-1">Bác sĩ</p>
                      <p className="font-semibold">
                        {booking.doctorName || "Chưa có thông tin"}
                      </p>
                    </div>

                    <div className="p-4 bg-white border rounded-lg">
                      <p className="text-sm text-gray-600 mb-1">Dịch vụ</p>
                      <p className="font-semibold">
                        {booking.serviceName || "Chưa có thông tin"}
                      </p>
                    </div>

                    <div className="p-4 bg-white border rounded-lg">
                      <p className="text-sm text-gray-600 mb-1">Bệnh nhân</p>
                      <p className="font-semibold">
                        {booking.patientName || "Chưa có thông tin"}
                      </p>
                    </div>

                    <div className="p-4 bg-white border rounded-lg">
                      <p className="text-sm text-gray-600 mb-1">
                        Trạng thái thanh toán
                      </p>
                      {getPaymentStatusBadge(booking.paymentStatus)}
                    </div>
                  </div>

                  {booking.actualCost && (
                    <div className="p-4 bg-white border rounded-lg">
                      <p className="text-sm text-gray-600 mb-1">Chi phí</p>
                      <p className="font-semibold text-lg text-blue-600">
                        {formatPrice(booking.actualCost)}
                      </p>
                    </div>
                  )}

                  {booking.notes && (
                    <div className="p-4 bg-white border rounded-lg">
                      <p className="text-sm text-gray-600 mb-1">Ghi chú</p>
                      <p className="text-gray-700">{booking.notes}</p>
                    </div>
                  )}

                  <div className="p-4 bg-gray-50 rounded-lg">
                    <p className="text-sm text-gray-600 mb-1">Ngày tạo</p>
                    <p className="text-gray-700">
                      {formatDate(booking.createdAt)}
                    </p>
                  </div>
                </div>
              </div>

              {/* Actions */}
              <div className="border-t pt-6 flex flex-col sm:flex-row gap-4">
                {booking.status !== "Cancelled" &&
                  booking.status !== "Completed" && (
                    <Button
                      variant="secondary"
                      onClick={handleCancelBooking}
                      disabled={loading}
                    >
                      Hủy đặt lịch
                    </Button>
                  )}

                {booking.paymentStatus === "Unpaid" &&
                  booking.status !== "Cancelled" && (
                    <Button
                      variant="accent"
                      onClick={async () => {
                        try {
                          const paymentUrl = await bookingAPI.initiatePayment(
                            booking.id
                          );
                          window.location.href = paymentUrl;
                        } catch (error) {
                          toast.error(
                            error.response?.data?.message ||
                              "Không thể khởi tạo thanh toán"
                          );
                        }
                      }}
                    >
                      Thanh toán ngay
                    </Button>
                  )}

                <Button
                  variant="secondary"
                  onClick={() => navigate("/booking")}
                >
                  Đặt lịch mới
                </Button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default BookingStatus;
