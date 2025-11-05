import React, { useState } from "react";
import { X, Calendar, FileText, User } from "lucide-react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { registerLocale } from "react-datepicker";
import vi from "date-fns/locale/vi";
import { format } from "date-fns";
import { createDoctorAbsence } from "../../../services/adminDoctorAbsenceAPI";
import toast from "react-hot-toast";

registerLocale("vi", vi);

const CreateAbsenceModal = ({ open, onClose, onSuccess, doctors }) => {
  const [formData, setFormData] = useState({
    doctorId: "",
    startDate: null,
    endDate: null,
    absenceType: "Leave",
    reason: "",
  });
  const [submitting, setSubmitting] = useState(false);

  const absenceTypes = [
    { value: "Leave", label: "Nghỉ phép" },
    { value: "Emergency", label: "Khẩn cấp" },
    { value: "Sick", label: "Ốm đau" },
    { value: "Other", label: "Khác" },
  ];

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!formData.doctorId || !formData.startDate || !formData.endDate) {
      toast.error("Vui lòng điền đầy đủ thông tin");
      return;
    }

    if (formData.endDate < formData.startDate) {
      toast.error("Ngày kết thúc phải sau ngày bắt đầu");
      return;
    }

    setSubmitting(true);
    try {
      await createDoctorAbsence({
        doctorId: Number(formData.doctorId),
        startDate: format(formData.startDate, "yyyy-MM-dd"),
        endDate: format(formData.endDate, "yyyy-MM-dd"),
        absenceType: formData.absenceType,
        reason: formData.reason,
      });
      toast.success("Tạo đơn nghỉ phép thành công!");
      onSuccess?.();
      onClose();
      setFormData({
        doctorId: "",
        startDate: null,
        endDate: null,
        absenceType: "Leave",
        reason: "",
      });
    } catch (error) {
      console.error("Error creating absence:", error);
      const message =
        error.response?.data?.message || "Không thể tạo đơn nghỉ phép";
      toast.error(message);
    } finally {
      setSubmitting(false);
    }
  };

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
            <Calendar className="h-6 w-6 text-red-600" />
            Tạo đơn nghỉ phép
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="p-6 space-y-6">
          {/* Doctor */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              <User className="h-4 w-4 inline mr-2" />
              Bác sĩ <span className="text-red-500">*</span>
            </label>
            <select
              value={formData.doctorId}
              onChange={(e) =>
                setFormData({ ...formData, doctorId: e.target.value })
              }
              className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              required
            >
              <option value="">-- Chọn bác sĩ --</option>
              {doctors.map((doctor) => (
                <option
                  key={doctor.doctorId || doctor.id}
                  value={doctor.doctorId || doctor.id}
                >
                  {doctor.doctorName || doctor.fullName || doctor.name} -{" "}
                  {doctor.specializationName || "Không có chuyên khoa"}
                </option>
              ))}
            </select>
          </div>

          {/* Date Range */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                <Calendar className="h-4 w-4 inline mr-2" />
                Ngày bắt đầu <span className="text-red-500">*</span>
              </label>
              <DatePicker
                selected={formData.startDate}
                onChange={(date) =>
                  setFormData({ ...formData, startDate: date })
                }
                dateFormat="dd/MM/yyyy"
                locale="vi"
                minDate={new Date()}
                placeholderText="Chọn ngày bắt đầu"
                className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                <Calendar className="h-4 w-4 inline mr-2" />
                Ngày kết thúc <span className="text-red-500">*</span>
              </label>
              <DatePicker
                selected={formData.endDate}
                onChange={(date) =>
                  setFormData({ ...formData, endDate: date })
                }
                dateFormat="dd/MM/yyyy"
                locale="vi"
                minDate={formData.startDate || new Date()}
                placeholderText="Chọn ngày kết thúc"
                className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                required
              />
            </div>
          </div>

          {/* Absence Type */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              Loại nghỉ phép
            </label>
            <select
              value={formData.absenceType}
              onChange={(e) =>
                setFormData({ ...formData, absenceType: e.target.value })
              }
              className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
            >
              {absenceTypes.map((type) => (
                <option key={type.value} value={type.value}>
                  {type.label}
                </option>
              ))}
            </select>
          </div>

          {/* Reason */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              <FileText className="h-4 w-4 inline mr-2" />
              Lý do nghỉ phép
            </label>
            <textarea
              value={formData.reason}
              onChange={(e) =>
                setFormData({ ...formData, reason: e.target.value })
              }
              rows={4}
              className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              placeholder="Nhập lý do nghỉ phép..."
            />
          </div>

          {/* Days count */}
          {formData.startDate && formData.endDate && (
            <div className="bg-blue-50 dark:bg-blue-900/20 border border-blue-200 dark:border-blue-800 rounded-lg p-4">
              <p className="text-sm text-blue-800 dark:text-blue-200">
                <strong>Số ngày nghỉ:</strong>{" "}
                {Math.ceil(
                  (formData.endDate - formData.startDate) / (1000 * 60 * 60 * 24)
                ) + 1}{" "}
                ngày
              </p>
            </div>
          )}

          {/* Actions */}
          <div className="flex items-center justify-end gap-3 pt-4 border-t border-gray-200 dark:border-gray-700">
            <button
              type="button"
              onClick={onClose}
              className="px-6 py-2 border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors font-medium"
            >
              Hủy
            </button>
            <button
              type="submit"
              disabled={submitting}
              className="px-6 py-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {submitting ? "Đang tạo..." : "Tạo đơn nghỉ phép"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateAbsenceModal;

