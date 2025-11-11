import React, { useState, useEffect } from "react";
import { X, Clock, Calendar, User } from "lucide-react";
import { updateWeeklySchedule } from "../../../services/adminWeeklyScheduleAPI";
import api from "../../../utils/api";
import toast from "react-hot-toast";

const dayNames = [
  { value: 1, label: "Thứ hai" },
  { value: 2, label: "Thứ ba" },
  { value: 3, label: "Thứ tư" },
  { value: 4, label: "Thứ năm" },
  { value: 5, label: "Thứ sáu" },
  { value: 6, label: "Thứ bảy" },
  { value: 0, label: "Chủ nhật" },
];

const EditWeeklyScheduleModal = ({
  open,
  onClose,
  schedule,
  onSuccess,
}) => {
  const [formData, setFormData] = useState({
    dayOfWeek: 1,
    slotId: "",
    isActive: true,
  });
  const [slots, setSlots] = useState([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (open && schedule) {
      setFormData({
        dayOfWeek: schedule.dayOfWeek ?? 1,
        slotId: schedule.slotId || schedule.slot?.slotId || "",
        isActive: schedule.isActive ?? true,
      });
      loadSlots();
    }
  }, [open, schedule]);

  const loadSlots = async () => {
    try {
      setLoading(true);
      const response = await api.get("/schedules/slots");
      const slotsData = response.data?.data || response.data || [];
      setSlots(slotsData);
    } catch (error) {
      console.error("Error loading slots:", error);
      toast.error("Không thể tải danh sách khung giờ");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!schedule?.id || !formData.slotId) {
      toast.error("Vui lòng điền đầy đủ thông tin");
      return;
    }

    setSubmitting(true);
    try {
      await updateWeeklySchedule(schedule.id, formData);
      toast.success("Cập nhật lịch tuần thành công!");
      onSuccess?.();
      onClose();
    } catch (error) {
      console.error("Error updating weekly schedule:", error);
      const message =
        error.response?.data?.message || "Không thể cập nhật lịch tuần";
      toast.error(message);
    } finally {
      setSubmitting(false);
    }
  };

  if (!open || !schedule) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
            <Calendar className="h-6 w-6 text-blue-600" />
            Chỉnh sửa lịch làm việc
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
          {/* Day of Week */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              <Calendar className="h-4 w-4 inline mr-2" />
              Ngày trong tuần
            </label>
            <select
              value={formData.dayOfWeek}
              onChange={(e) =>
                setFormData({ ...formData, dayOfWeek: Number(e.target.value) })
              }
              className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              required
            >
              {dayNames.map((day) => (
                <option key={day.value} value={day.value}>
                  {day.label}
                </option>
              ))}
            </select>
          </div>

          {/* Slot */}
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
              <Clock className="h-4 w-4 inline mr-2" />
              Khung giờ
            </label>
            {loading ? (
              <div className="text-center py-4 text-gray-500">
                Đang tải khung giờ...
              </div>
            ) : (
              <select
                value={formData.slotId}
                onChange={(e) =>
                  setFormData({ ...formData, slotId: Number(e.target.value) })
                }
                className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                required
              >
                <option value="">-- Chọn khung giờ --</option>
                {slots.map((slot) => (
                  <option key={slot.slotId || slot.id} value={slot.slotId || slot.id}>
                    {slot.startTime} - {slot.endTime}
                  </option>
                ))}
              </select>
            )}
          </div>

          {/* Is Active */}
          <div className="flex items-center">
            <input
              type="checkbox"
              id="isActive"
              checked={formData.isActive}
              onChange={(e) =>
                setFormData({ ...formData, isActive: e.target.checked })
              }
              className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
            />
            <label
              htmlFor="isActive"
              className="ml-2 block text-sm text-gray-700 dark:text-gray-300"
            >
              Kích hoạt
            </label>
          </div>

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
              className="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {submitting ? "Đang cập nhật..." : "Cập nhật"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditWeeklyScheduleModal;

