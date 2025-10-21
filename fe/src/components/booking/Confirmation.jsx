import React from "react";
import Button from "../common/Button";

const Confirmation = ({ data, back, onSubmit, isSubmitting, submitError }) => {
  const handleSubmit = async () => {
    try {
      await onSubmit();
    } catch (error) {
      // Error is handled by the hook
    }
  };

  // This component is for confirmation step, success is handled in Success component

  return (
    <div>
      <h3 className="text-lg font-semibold mb-4">Xác nhận thông tin</h3>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
        <div className="p-4 rounded-xl border bg-white">
          <p className="font-semibold mb-2">Thông tin cá nhân</p>
          <p>Họ tên: {data.fullName}</p>
          <p>SĐT: {data.phone}</p>
          <p>Email: {data.email}</p>
        </div>
        <div className="p-4 rounded-xl border bg-white">
          <p className="font-semibold mb-2">Dịch vụ</p>
          <ul className="list-disc list-inside text-gray-700">
            {(data.selectedServices || []).map((s) => (
              <li key={s}>{s}</li>
            ))}
          </ul>
        </div>
        <div className="p-4 rounded-xl border bg-white">
          <p className="font-semibold mb-2">Bác sĩ & thời gian</p>
          <p>Ngày: {data.date}</p>
          <p>Giờ: {data.time}</p>
        </div>
      </div>

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
