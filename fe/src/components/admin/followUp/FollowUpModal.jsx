import React, { useState, useEffect } from "react";

const FollowUpModal = ({ open, followUp, onClose, onSave }) => {
  const [form, setForm] = useState({
    appointmentId: "",
    nextAppointmentDate: "",
    description: "",
    status: "Pending",
  });

  const statusOptions = [
    { value: "Pending", label: "Chờ xử lý" },
    { value: "Scheduled", label: "Đã lên lịch" },
    { value: "Completed", label: "Hoàn thành" },
    { value: "Cancelled", label: "Đã hủy" },
    { value: "Rescheduled", label: "Đã dời lịch" },
  ];

  useEffect(() => {
    if (followUp) {
      setForm({
        appointmentId: followUp.appointmentId || "",
        nextAppointmentDate: followUp.nextAppointmentDate ? 
          new Date(followUp.nextAppointmentDate).toISOString().split('T')[0] : "",
        description: followUp.description || "",
        status: followUp.status || "Pending",
      });
    } else {
      setForm({
        appointmentId: "",
        nextAppointmentDate: "",
        description: "",
        status: "Pending",
      });
    }
  }, [followUp]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[600px] max-h-[90vh] overflow-y-auto">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          {followUp ? "Sửa lịch tái khám" : "Thêm lịch tái khám mới"}
        </h2>

        <div className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                ID Lịch hẹn *
              </label>
              <input
                type="number"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.appointmentId}
                onChange={(e) => setForm((f) => ({ ...f, appointmentId: e.target.value }))}
                placeholder="Nhập ID lịch hẹn"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Trạng thái
              </label>
              <select
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.status}
                onChange={(e) => setForm((f) => ({ ...f, status: e.target.value }))}
              >
                {statusOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Ngày tái khám *
            </label>
            <input
              type="date"
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.nextAppointmentDate}
              onChange={(e) => setForm((f) => ({ ...f, nextAppointmentDate: e.target.value }))}
              required
            />
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">
              Ngày tái khám phải trong tương lai
            </p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Mô tả
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.description}
              onChange={(e) => setForm((f) => ({ ...f, description: e.target.value }))}
              placeholder="Nhập mô tả lịch tái khám"
              rows={4}
            />
          </div>

          <div className="bg-blue-50 dark:bg-blue-900/20 p-3 rounded-lg">
            <h4 className="text-sm font-medium text-blue-800 dark:text-blue-200 mb-2">
              Thông tin bổ sung:
            </h4>
            <ul className="text-xs text-blue-700 dark:text-blue-300 space-y-1">
              <li>• <strong>Chờ xử lý:</strong> Lịch tái khám mới được tạo</li>
              <li>• <strong>Đã lên lịch:</strong> Đã xác nhận lịch tái khám</li>
              <li>• <strong>Hoàn thành:</strong> Đã hoàn thành tái khám</li>
              <li>• <strong>Đã hủy:</strong> Hủy lịch tái khám</li>
              <li>• <strong>Đã dời lịch:</strong> Dời lịch tái khám</li>
            </ul>
          </div>
        </div>

        <div className="flex justify-end gap-2 mt-6">
          <button
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium transition"
            onClick={() => onSave(form)}
          >
            Lưu
          </button>
          <button
            className="bg-gray-300 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 px-4 py-2 rounded-md font-medium transition"
            onClick={onClose}
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default FollowUpModal;
