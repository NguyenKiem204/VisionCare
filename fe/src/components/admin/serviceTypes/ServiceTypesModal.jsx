import React, { useState, useEffect } from "react";

const ServiceTypesModal = ({ open, serviceType, onClose, onSave }) => {
  const [form, setForm] = useState({
    name: "",
    durationMinutes: "",
  });

  useEffect(() => {
    if (serviceType) {
      setForm({
        name: serviceType.name || "",
        durationMinutes: serviceType.durationMinutes || "",
      });
    } else {
      setForm({
        name: "",
        durationMinutes: "",
      });
    }
  }, [serviceType]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[500px] max-h-[90vh] overflow-y-auto">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          {serviceType ? "Sửa loại dịch vụ" : "Thêm loại dịch vụ mới"}
        </h2>

        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Tên loại dịch vụ *
            </label>
            <input
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.name}
              onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))}
              placeholder="Nhập tên loại dịch vụ"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Thời gian (phút) *
            </label>
            <input
              type="number"
              min="1"
              max="1440"
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.durationMinutes}
              onChange={(e) => setForm((f) => ({ ...f, durationMinutes: e.target.value }))}
              placeholder="Nhập thời gian (phút)"
              required
            />
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">
              Thời gian từ 1 đến 1440 phút (24 giờ)
            </p>
          </div>

          <div className="bg-blue-50 dark:bg-blue-900/20 p-3 rounded-lg">
            <h4 className="text-sm font-medium text-blue-800 dark:text-blue-200 mb-2">
              Thông tin bổ sung:
            </h4>
            <ul className="text-xs text-blue-700 dark:text-blue-300 space-y-1">
              <li>• Thời gian được tính bằng phút</li>
              <li>• 1 giờ = 60 phút</li>
              <li>• Thời gian tối đa: 24 giờ (1440 phút)</li>
              <li>• Ví dụ: 30 phút, 60 phút, 90 phút</li>
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

export default ServiceTypesModal;
