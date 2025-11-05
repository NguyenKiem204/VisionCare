import React, { useEffect, useState } from "react";
import Button from "../common/Button";
import { getDoctorById } from "../../services/adminDoctorAPI";
import { getServiceById } from "../../services/adminServiceAPI";

const Confirmation = ({ data, back, onSubmit, isSubmitting, submitError }) => {
  const [doctorInfo, setDoctorInfo] = useState(null);
  const [serviceInfo, setServiceInfo] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadDetails = async () => {
      try {
        setLoading(true);
        const promises = [];

        if (data.doctorId) {
          promises.push(
            getDoctorById(data.doctorId).then((res) => ({
              type: "doctor",
              data: res.data?.data || res.data,
            }))
          );
        }

        if (data.serviceDetailId) {
          promises.push(
            getServiceById(data.serviceDetailId).then((res) => ({
              type: "service",
              data: res.data?.data || res.data,
            }))
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

  const handleSubmit = async () => {
    try {
      await onSubmit();
    } catch (error) {
      // Error is handled by the hook
    }
  };

  const formatDate = (dateStr) => {
    if (!dateStr) return "N/A";
    const date = new Date(dateStr + "T00:00:00");
    return date.toLocaleDateString("vi-VN", {
      weekday: "long",
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  const formatPrice = (price) => {
    if (!price) return "Liên hệ";
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(price);
  };

  return (
    <div>
      <h3 className="text-lg font-semibold mb-4">Xác nhận thông tin đặt lịch</h3>

      {loading ? (
        <div className="text-center py-8">Đang tải...</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
          <div className="p-4 rounded-xl border bg-white">
            <p className="font-semibold mb-2 text-blue-600">Thông tin cá nhân</p>
            <div className="space-y-1 text-sm">
              <p>
                <strong>Họ tên:</strong> {data.fullName || "N/A"}
              </p>
              <p>
                <strong>SĐT:</strong> {data.phone || "N/A"}
              </p>
              <p>
                <strong>Email:</strong> {data.email || "N/A"}
              </p>
            </div>
          </div>

          <div className="p-4 rounded-xl border bg-white">
            <p className="font-semibold mb-2 text-blue-600">Dịch vụ</p>
            <div className="space-y-1 text-sm">
              <p>
                <strong>Tên dịch vụ:</strong>{" "}
                {serviceInfo?.serviceDetailName || serviceInfo?.name || "N/A"}
              </p>
              <p>
                <strong>Giá:</strong> {formatPrice(serviceInfo?.cost)}
              </p>
            </div>
          </div>

          <div className="p-4 rounded-xl border bg-white">
            <p className="font-semibold mb-2 text-blue-600">Bác sĩ & thời gian</p>
            <div className="space-y-1 text-sm">
              <p>
                <strong>Bác sĩ:</strong>{" "}
                {doctorInfo?.fullName || doctorInfo?.name || "N/A"}
              </p>
              <p>
                <strong>Ngày:</strong> {formatDate(data.date)}
              </p>
              <p>
                <strong>Giờ:</strong> {data.time || "N/A"}
              </p>
            </div>
          </div>

          {data.notes && (
            <div className="p-4 rounded-xl border bg-white">
              <p className="font-semibold mb-2 text-blue-600">Ghi chú</p>
              <p className="text-sm">{data.notes}</p>
            </div>
          )}
        </div>
      )}

      {submitError && (
        <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-lg text-red-700 text-sm">
          {submitError}
        </div>
      )}

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button variant="accent" onClick={handleSubmit} disabled={isSubmitting}>
          {isSubmitting ? "Đang gửi..." : "Xác nhận & đặt lịch"}
        </Button>
      </div>
    </div>
  );
};

export default Confirmation;
