import React, { useState, useEffect } from "react";

const MedicalHistoryModal = ({ open, medicalHistory, onClose, onSave }) => {
  const [form, setForm] = useState({
    appointmentId: "",
    diagnosis: "",
    symptoms: "",
    treatment: "",
    prescription: "",
    visionLeft: "",
    visionRight: "",
    additionalTests: "",
    notes: "",
  });

  useEffect(() => {
    if (medicalHistory) {
      setForm({
        appointmentId: medicalHistory.appointmentId || "",
        diagnosis: medicalHistory.diagnosis || "",
        symptoms: medicalHistory.symptoms || "",
        treatment: medicalHistory.treatment || "",
        prescription: medicalHistory.prescription || "",
        visionLeft: medicalHistory.visionLeft || "",
        visionRight: medicalHistory.visionRight || "",
        additionalTests: medicalHistory.additionalTests || "",
        notes: medicalHistory.notes || "",
      });
    } else {
      setForm({
        appointmentId: "",
        diagnosis: "",
        symptoms: "",
        treatment: "",
        prescription: "",
        visionLeft: "",
        visionRight: "",
        additionalTests: "",
        notes: "",
      });
    }
  }, [medicalHistory]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[800px] max-h-[90vh] overflow-y-auto">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          {medicalHistory ? "Sửa lịch sử khám bệnh" : "Thêm lịch sử khám bệnh mới"}
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
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Chẩn đoán
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.diagnosis}
              onChange={(e) => setForm((f) => ({ ...f, diagnosis: e.target.value }))}
              placeholder="Nhập chẩn đoán"
              rows={3}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Triệu chứng
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.symptoms}
              onChange={(e) => setForm((f) => ({ ...f, symptoms: e.target.value }))}
              placeholder="Nhập triệu chứng"
              rows={3}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Điều trị
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.treatment}
              onChange={(e) => setForm((f) => ({ ...f, treatment: e.target.value }))}
              placeholder="Nhập phương pháp điều trị"
              rows={3}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Đơn thuốc
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.prescription}
              onChange={(e) => setForm((f) => ({ ...f, prescription: e.target.value }))}
              placeholder="Nhập đơn thuốc"
              rows={3}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Thị lực trái (0.0 - 2.0)
              </label>
              <input
                type="number"
                step="0.1"
                min="0"
                max="2"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.visionLeft}
                onChange={(e) => setForm((f) => ({ ...f, visionLeft: e.target.value }))}
                placeholder="0.0 - 2.0"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Thị lực phải (0.0 - 2.0)
              </label>
              <input
                type="number"
                step="0.1"
                min="0"
                max="2"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.visionRight}
                onChange={(e) => setForm((f) => ({ ...f, visionRight: e.target.value }))}
                placeholder="0.0 - 2.0"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Xét nghiệm bổ sung
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.additionalTests}
              onChange={(e) => setForm((f) => ({ ...f, additionalTests: e.target.value }))}
              placeholder="Nhập các xét nghiệm bổ sung"
              rows={2}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Ghi chú
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.notes}
              onChange={(e) => setForm((f) => ({ ...f, notes: e.target.value }))}
              placeholder="Nhập ghi chú bổ sung"
              rows={3}
            />
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

export default MedicalHistoryModal;
