import React, { useState, useEffect } from "react";

const WorkShiftModal = ({ open, workShift, onClose, onSave }) => {
  const [form, setForm] = useState({
    shiftName: "",
    startTime: "",
    endTime: "",
    isActive: true,
    description: "",
  });

  useEffect(() => {
    if (workShift) {
      setForm({
        shiftName: workShift.shiftName || "",
        startTime: typeof workShift.startTime === 'string' ? workShift.startTime.substring(0, 5) : workShift.startTime || "",
        endTime: typeof workShift.endTime === 'string' ? workShift.endTime.substring(0, 5) : workShift.endTime || "",
        isActive: workShift.isActive ?? true,
        description: workShift.description || "",
      });
    } else {
      setForm({ shiftName: "", startTime: "", endTime: "", isActive: true, description: "" });
    }
  }, [workShift]);

  if (!open) return null;

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave(form);
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[500px]">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          {workShift ? "Sửa ca làm việc" : "Thêm ca làm việc mới"}
        </h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Tên ca *</label>
            <input
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.shiftName}
              onChange={(e) => setForm((f) => ({ ...f, shiftName: e.target.value }))}
              required
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Giờ bắt đầu *</label>
              <input
                type="time"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.startTime}
                onChange={(e) => setForm((f) => ({ ...f, startTime: e.target.value }))}
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Giờ kết thúc *</label>
              <input
                type="time"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.endTime}
                onChange={(e) => setForm((f) => ({ ...f, endTime: e.target.value }))}
                required
              />
            </div>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Mô tả</label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.description}
              onChange={(e) => setForm((f) => ({ ...f, description: e.target.value }))}
              rows={3}
            />
          </div>
          <div>
            <label className="flex items-center">
              <input
                type="checkbox"
                className="mr-2"
                checked={form.isActive}
                onChange={(e) => setForm((f) => ({ ...f, isActive: e.target.checked }))}
              />
              <span className="text-sm text-gray-700 dark:text-gray-300">Hoạt động</span>
            </label>
          </div>
          <div className="flex justify-end gap-2 mt-6">
            <button type="button" onClick={onClose} className="px-4 py-2 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-200 rounded-md hover:bg-gray-300">
              Hủy
            </button>
            <button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700">
              {workShift ? "Cập nhật" : "Tạo mới"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default WorkShiftModal;

