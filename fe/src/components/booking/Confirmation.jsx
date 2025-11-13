import React, { useEffect, useState, useRef } from "react";
import Button from "../common/Button";
import { getDoctorById } from "../../services/adminDoctorAPI";
import { getServiceDetailById } from "../../services/serviceDetailAPI";

const Confirmation = ({ data, back, onSubmit, isSubmitting, submitError }) => {
  const [doctorInfo, setDoctorInfo] = useState(null);
  const [serviceInfo, setServiceInfo] = useState(null);
  const [loading, setLoading] = useState(true);
  const errorRef = useRef(null);

  useEffect(() => {
    const loadDetails = async () => {
      try {
        setLoading(true);
        const promises = [];

        if (data.doctorId) {
          promises.push(
            getDoctorById(data.doctorId)
              .then((res) => {
                // API returns doctor directly, not wrapped in ApiResponse
                const doctorData = res.data?.data || res.data || res;
                console.log("[Confirmation] Loaded doctor:", doctorData);
                return {
                  type: "doctor",
                  data: doctorData,
                };
              })
              .catch((err) => {
                console.error("Error loading doctor:", err);
                return { type: "doctor", data: null };
              })
          );
        }

        if (data.serviceDetailId) {
          promises.push(
            getServiceDetailById(data.serviceDetailId)
              .then((res) => ({
                type: "service",
                data: res.data?.data || res.data || res,
              }))
              .catch((err) => {
                console.error("Error loading service detail:", err);
                return { type: "service", data: null };
              })
          );
        }

        const results = await Promise.all(promises);
        results.forEach((result) => {
          if (result.type === "doctor") {
            setDoctorInfo(result.data);
          } else if (result.type === "service") {
            setServiceInfo(result.data);
          }
        });
      } catch (error) {
        console.error("Error loading details:", error);
      } finally {
        setLoading(false);
      }
    };

    loadDetails();
  }, [data.doctorId, data.serviceDetailId]);

  // Scroll to error when it appears
  useEffect(() => {
    if (submitError && errorRef.current) {
      errorRef.current.scrollIntoView({ 
        behavior: 'smooth', 
        block: 'center' 
      });
    }
  }, [submitError]);

  const handleSubmit = async () => {
    // Validate service cost
    if (!serviceInfo || !serviceInfo.cost || serviceInfo.cost <= 0) {
      alert("Dịch vụ này chưa có giá. Vui lòng liên hệ admin để được hỗ trợ hoặc chọn dịch vụ khác.");
      return;
    }

    try {
      await onSubmit();
    } catch (error) {
      // Error is handled by the hook
    }
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return "N/A";
    try {
      const date = new Date(dateStr + "T00:00:00");
      return date.toLocaleDateString("vi-VN", {
        weekday: "long",
        year: "numeric",
        month: "long",
        day: "numeric",
      });
    } catch {
      return dateStr;
    }
  };

  const formatPrice = (price) => {
    if (!price && price !== 0) return "Liên hệ";
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(price);
  };

  const formatTime = (timeStr) => {
    if (!timeStr) return "N/A";
    // Handle both "HH:mm" and "HH:mm:ss" formats
    return timeStr.split(":").slice(0, 2).join(":");
  };

  // Calculate total (service cost)
  const totalAmount = serviceInfo?.cost || 0;

  return (
    <div>
      <h3 className="text-xl font-bold mb-6 text-gray-900">Xác nhận thông tin đặt lịch</h3>

      {loading ? (
        <div className="text-center py-8 text-gray-600">Đang tải thông tin...</div>
      ) : (
        <div className="bg-white border-2 border-gray-200 rounded-lg shadow-lg overflow-hidden">
          {/* Header - Invoice style */}
          <div className="bg-gradient-to-r from-blue-600 to-indigo-600 text-white p-6">
            <div className="flex justify-between items-start">
              <div>
                <h2 className="text-2xl font-bold mb-2">VisionCare</h2>
                <p className="text-blue-100">Hóa đơn đặt lịch khám</p>
              </div>
              <div className="text-right">
                <p className="text-sm text-blue-100">Ngày tạo</p>
                <p className="font-semibold">{new Date().toLocaleDateString("vi-VN")}</p>
              </div>
            </div>
          </div>

          {/* Customer Info */}
          <div className="p-6 border-b border-gray-200">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Thông tin khách hàng</h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-gray-600 mb-1">Họ và tên</p>
                <p className="font-medium text-gray-900">{data.fullName || "N/A"}</p>
              </div>
              <div>
                <p className="text-sm text-gray-600 mb-1">Số điện thoại</p>
                <p className="font-medium text-gray-900">{data.phone || "N/A"}</p>
              </div>
              <div className="md:col-span-2">
                <p className="text-sm text-gray-600 mb-1">Email</p>
                <p className="font-medium text-gray-900">{data.email || "N/A"}</p>
              </div>
            </div>
          </div>

          {/* Service & Doctor Info */}
          <div className="p-6 border-b border-gray-200 bg-gray-50">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Thông tin dịch vụ</h3>
            <div className="space-y-4">
              <div className="flex justify-between items-start">
                <div className="flex-1">
                  <p className="text-sm text-gray-600 mb-1">Dịch vụ</p>
                  <p className="font-semibold text-gray-900 text-lg">
                    {serviceInfo?.serviceName || serviceInfo?.name || "N/A"}
                  </p>
                  {serviceInfo?.serviceTypeName && (
                    <p className="text-sm text-gray-600 mt-1">Loại: {serviceInfo.serviceTypeName}</p>
                  )}
                </div>
                <div className="text-right ml-4">
                  <p className="text-sm text-gray-600 mb-1">Giá dịch vụ</p>
                  <p className="font-bold text-lg text-blue-600">
                    {formatPrice(serviceInfo?.cost)}
                  </p>
                  {(!serviceInfo?.cost || serviceInfo.cost === 0) && (
                    <p className="text-xs text-red-600 mt-1">⚠️ Chưa có giá</p>
                  )}
                </div>
              </div>

              <div className="pt-4 border-t border-gray-200">
                <p className="text-sm text-gray-600 mb-1">Bác sĩ</p>
                <p className="font-semibold text-gray-900">
                  {doctorInfo?.fullName || doctorInfo?.name || doctorInfo?.doctorName || (data.doctorId ? `Bác sĩ ID: ${data.doctorId}` : "Chưa chọn bác sĩ")}
                </p>
                {doctorInfo?.specialization && (
                  <p className="text-sm text-gray-600 mt-1">
                    Chuyên khoa: {doctorInfo.specialization.name || doctorInfo.specialization}
                  </p>
                )}
              </div>
            </div>
          </div>

          {/* Appointment Time */}
          <div className="p-6 border-b border-gray-200">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Thời gian khám</h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-gray-600 mb-1">Ngày khám</p>
                <p className="font-medium text-gray-900">{formatDate(data.date)}</p>
              </div>
              <div>
                <p className="text-sm text-gray-600 mb-1">Giờ khám</p>
                <p className="font-medium text-gray-900">{formatTime(data.time)}</p>
              </div>
            </div>
          </div>

          {/* Notes */}
          {data.notes && (
            <div className="p-6 border-b border-gray-200">
              <h3 className="text-lg font-semibold text-gray-900 mb-2">Ghi chú</h3>
              <p className="text-gray-700">{data.notes}</p>
            </div>
          )}

          {/* Total */}
          <div className="p-6 bg-gray-50">
            <div className="flex justify-between items-center">
              <span className="text-lg font-semibold text-gray-900">Tổng cộng</span>
              <span className="text-2xl font-bold text-blue-600">
                {formatPrice(totalAmount)}
              </span>
            </div>
            {totalAmount === 0 || !totalAmount ? (
              <p className="text-sm text-gray-600 mt-2 text-right">
                (Vui lòng liên hệ để biết giá chính xác)
              </p>
            ) : null}
          </div>
        </div>
      )}

      {submitError && (
        <div ref={errorRef} className="mt-4 p-4 bg-red-50 border-2 border-red-300 rounded-lg text-red-800 shadow-lg">
          <div className="flex items-start">
            <div className="flex-shrink-0">
              <svg className="h-5 w-5 text-red-600 mt-0.5" fill="currentColor" viewBox="0 0 20 20">
                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
              </svg>
            </div>
            <div className="ml-3 flex-1">
              <h3 className="text-sm font-bold text-red-800 mb-2">Không thể đặt lịch</h3>
              <div className="text-sm text-red-700">
                {submitError.includes('\n') ? (
                  <ul className="list-disc list-inside space-y-1">
                    {submitError.split('\n').map((error, index) => (
                      <li key={index}>{error}</li>
                    ))}
                  </ul>
                ) : (
                  <p>{submitError}</p>
                )}
              </div>
              <p className="mt-2 text-xs text-red-600">
                Vui lòng kiểm tra lại thông tin và thử lại.
              </p>
            </div>
          </div>
        </div>
      )}

      <div className="flex justify-between mt-6">
        <Button variant="secondary" onClick={back} disabled={isSubmitting}>
          Quay lại
        </Button>
        <Button 
          variant="accent" 
          onClick={handleSubmit} 
          disabled={isSubmitting || loading || !serviceInfo || !serviceInfo.cost || serviceInfo.cost <= 0}
        >
          {isSubmitting ? "Đang xử lý..." : "Xác nhận & đặt lịch"}
        </Button>
      </div>
      {(!serviceInfo || !serviceInfo.cost || serviceInfo.cost <= 0) && (
        <div className="mt-4 p-3 bg-yellow-50 border border-yellow-200 rounded-lg text-yellow-800 text-sm">
          <p className="font-semibold mb-1">⚠️ Không thể đặt lịch</p>
          <p>Dịch vụ này chưa có giá. Vui lòng liên hệ admin để được hỗ trợ hoặc chọn dịch vụ khác.</p>
        </div>
      )}
    </div>
  );
};

export default Confirmation;
