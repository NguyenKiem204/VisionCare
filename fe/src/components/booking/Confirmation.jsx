import React from "react";
import Button from "../common/Button";
import { api } from "../../utils/api";

const Confirmation = ({ data, back }) => {
  const [loading, setLoading] = React.useState(false);
  const [result, setResult] = React.useState(null);

  const submit = async () => {
    setLoading(true);
    const res = await api.createBooking(data);
    setResult(res);
    setLoading(false);
  };

  if (result?.ok) {
    return (
      <div className="text-center">
        <div className="mx-auto w-16 h-16 rounded-full bg-emerald-100 text-emerald-600 flex items-center justify-center mb-4">
          <svg
            className="w-8 h-8"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M5 13l4 4L19 7"
            />
          </svg>
        </div>
        <h3 className="text-2xl font-semibold mb-2">Đặt lịch thành công!</h3>
        <p className="text-gray-700 mb-4">
          Mã đặt lịch: <span className="font-semibold">{result.code}</span>
        </p>
        <a href="/" className="underline text-blue-600">
          Về trang chủ
        </a>
      </div>
    );
  }

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

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button variant="accent" onClick={submit} disabled={loading}>
          {loading ? "Đang gửi..." : "Xác nhận & đặt lịch"}
        </Button>
      </div>
    </div>
  );
};

export default Confirmation;
