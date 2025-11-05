import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import Loading from "../components/common/Loading";
import { bookingAPI } from "../services/bookingAPI";
import toast from "react-hot-toast";

const BookingPaymentCallback = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [status, setStatus] = useState("processing"); // processing, success, failed
  const [booking, setBooking] = useState(null);

  useEffect(() => {
    const processCallback = async () => {
      try {
        setStatus("processing");

        // VNPay returns query params, we need to send them to backend
        // However, the backend PaymentCallback endpoint handles this via GET request
        // So we need to check the current URL params

        const appointmentCode = searchParams.get("vnp_TxnRef");
        if (!appointmentCode) {
          setStatus("failed");
          toast.error("Không tìm thấy thông tin giao dịch");
          return;
        }

        // The backend should have already processed the payment
        // We just need to fetch the updated booking status
        setTimeout(async () => {
          try {
            const bookingData = await bookingAPI.searchBooking(
              appointmentCode,
              null
            );

            if (bookingData) {
              setBooking(bookingData);
              if (bookingData.paymentStatus === "Paid") {
                setStatus("success");
                toast.success("Thanh toán thành công!");
              } else {
                setStatus("failed");
                toast.error("Thanh toán thất bại");
              }
            } else {
              setStatus("failed");
              toast.error("Không tìm thấy thông tin đặt lịch");
            }
          } catch (error) {
            console.error("Error fetching booking:", error);
            setStatus("failed");
            toast.error("Không thể kiểm tra trạng thái thanh toán");
          }
        }, 2000); // Wait 2 seconds for backend to process
      } catch (error) {
        console.error("Error processing callback:", error);
        setStatus("failed");
        toast.error("Có lỗi xảy ra khi xử lý thanh toán");
      }
    };

    processCallback();
  }, [searchParams]);

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

  if (status === "processing") {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="bg-white rounded-2xl shadow-lg p-8 max-w-md w-full text-center">
          <Loading />
          <p className="mt-4 text-gray-600">Đang xử lý thanh toán...</p>
        </div>
      </div>
    );
  }

  if (status === "success") {
    return (
      <div className="min-h-screen bg-gray-50 py-12">
        <div className="container mx-auto px-4 max-w-2xl">
          <div className="bg-white rounded-2xl shadow-lg p-8 text-center">
            <div className="mx-auto w-20 h-20 rounded-full bg-green-100 text-green-600 flex items-center justify-center mb-6">
              <svg
                className="w-12 h-12"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M5 13l4 4L19 7"
                />
              </svg>
            </div>

            <h1 className="text-3xl font-bold text-gray-900 mb-4">
              Thanh toán thành công!
            </h1>
            <p className="text-gray-600 mb-8">
              Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi
            </p>

            {booking && (
              <div className="bg-blue-50 rounded-lg p-6 mb-6 text-left">
                <h2 className="text-lg font-semibold mb-4">
                  Thông tin đặt lịch
                </h2>
                <div className="space-y-2 text-sm">
                  <p>
                    <strong>Mã đặt lịch:</strong> {booking.appointmentCode}
                  </p>
                  <p>
                    <strong>Thời gian khám:</strong>{" "}
                    {formatDate(booking.appointmentDate)}
                  </p>
                  {booking.doctorName && (
                    <p>
                      <strong>Bác sĩ:</strong> {booking.doctorName}
                    </p>
                  )}
                  {booking.serviceName && (
                    <p>
                      <strong>Dịch vụ:</strong> {booking.serviceName}
                    </p>
                  )}
                </div>
              </div>
            )}

            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <button
                onClick={() => navigate(`/booking/status?code=${booking?.appointmentCode || ""}`)}
                className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
              >
                Xem chi tiết đặt lịch
              </button>
              <button
                onClick={() => navigate("/")}
                className="px-6 py-3 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 transition-colors"
              >
                Về trang chủ
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

  // Failed
  return (
    <div className="min-h-screen bg-gray-50 py-12">
      <div className="container mx-auto px-4 max-w-2xl">
        <div className="bg-white rounded-2xl shadow-lg p-8 text-center">
          <div className="mx-auto w-20 h-20 rounded-full bg-red-100 text-red-600 flex items-center justify-center mb-6">
            <svg
              className="w-12 h-12"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </div>

          <h1 className="text-3xl font-bold text-gray-900 mb-4">
            Thanh toán thất bại
          </h1>
          <p className="text-gray-600 mb-8">
            Đã xảy ra lỗi trong quá trình thanh toán. Vui lòng thử lại.
          </p>

          {booking && (
            <div className="bg-yellow-50 rounded-lg p-6 mb-6 text-left">
              <h2 className="text-lg font-semibold mb-4">
                Thông tin đặt lịch
              </h2>
              <div className="space-y-2 text-sm">
                <p>
                  <strong>Mã đặt lịch:</strong> {booking.appointmentCode}
                </p>
                <p>
                  <strong>Trạng thái:</strong>{" "}
                  <span className="text-yellow-700 font-medium">
                    {booking.paymentStatus === "Unpaid"
                      ? "Chưa thanh toán"
                      : booking.paymentStatus}
                  </span>
                </p>
              </div>
            </div>
          )}

          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            {booking && (
              <button
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
                className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
              >
                Thử thanh toán lại
              </button>
            )}
            <button
              onClick={() => navigate(`/booking/status?code=${booking?.appointmentCode || ""}`)}
              className="px-6 py-3 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 transition-colors"
            >
              Xem chi tiết đặt lịch
            </button>
            <button
              onClick={() => navigate("/")}
              className="px-6 py-3 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 transition-colors"
            >
              Về trang chủ
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default BookingPaymentCallback;
