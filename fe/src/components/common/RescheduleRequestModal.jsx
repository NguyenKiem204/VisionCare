import React, { useState, useEffect, useCallback } from "react";
import { X, Calendar, Clock, MessageSquare } from "lucide-react";
import { bookingAPI } from "../../services/bookingAPI";
import { formatDateTime } from "../../utils/formatDate";

const RescheduleRequestModal = ({
  isOpen,
  onClose,
  appointment,
  onSubmit,
  isLoading = false,
  isDoctorRequest = false, // Nếu true, cho phép bác sĩ chọn bất kỳ thời gian nào
}) => {
  const [selectedDate, setSelectedDate] = useState("");
  const [selectedTime, setSelectedTime] = useState("");
  const [selectedDateTime, setSelectedDateTime] = useState(""); // Cho bác sĩ chọn tự do
  const [reason, setReason] = useState("");
  const [availableSlots, setAvailableSlots] = useState([]);
  const [loadingSlots, setLoadingSlots] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (isOpen && appointment) {
      // Set default date to tomorrow
      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      const dateStr = tomorrow.toISOString().split("T")[0];
      setSelectedDate(dateStr);
      
      // Nếu là bác sĩ, set default datetime
      if (isDoctorRequest) {
        const defaultDateTime = new Date(tomorrow);
        defaultDateTime.setHours(9, 0, 0, 0); // 9:00 AM
        // Format: YYYY-MM-DDTHH:mm
        const year = defaultDateTime.getFullYear();
        const month = String(defaultDateTime.getMonth() + 1).padStart(2, '0');
        const day = String(defaultDateTime.getDate()).padStart(2, '0');
        const hours = String(defaultDateTime.getHours()).padStart(2, '0');
        const minutes = String(defaultDateTime.getMinutes()).padStart(2, '0');
        setSelectedDateTime(`${year}-${month}-${day}T${hours}:${minutes}`);
      }
    }
  }, [isOpen, appointment, isDoctorRequest]);

  const loadAvailableSlots = useCallback(async () => {
    const doctorId = appointment?.doctorId || appointment?.DoctorId;
    if (!selectedDate || !doctorId) return;

    setLoadingSlots(true);
    setError("");
    try {
      const slots = await bookingAPI.getAvailableSlots(
        doctorId,
        selectedDate
      );
      setAvailableSlots(Array.isArray(slots) ? slots : []);
    } catch (err) {
      setError("Không thể tải danh sách khung giờ");
      console.error("Error loading slots:", err);
    } finally {
      setLoadingSlots(false);
    }
  }, [selectedDate, appointment]);

  useEffect(() => {
    // Chỉ load slots nếu không phải là bác sĩ đề xuất
    if (isDoctorRequest) {
      setAvailableSlots([]);
      setSelectedTime("");
      return;
    }
    
    const doctorId = appointment?.doctorId || appointment?.DoctorId;
    if (selectedDate && doctorId) {
      loadAvailableSlots();
    } else {
      setAvailableSlots([]);
      setSelectedTime("");
    }
  }, [selectedDate, appointment, loadAvailableSlots, isDoctorRequest]);

  const handleSubmit = (e) => {
    e.preventDefault();
    setError("");

    let proposedDateTime;

    if (isDoctorRequest) {
      // Bác sĩ chọn tự do thời gian
      if (!selectedDateTime) {
        setError("Vui lòng chọn ngày và giờ");
        return;
      }
      
      // Parse từ datetime-local input (YYYY-MM-DDTHH:mm)
      const [datePart, timePart] = selectedDateTime.split('T');
      const [year, month, day] = datePart.split("-").map(Number);
      const [hours, minutes] = timePart.split(":").map(Number);
      
      proposedDateTime = new Date(year, month - 1, day, hours, minutes, 0);
      if (isNaN(proposedDateTime.getTime())) {
        setError("Thời gian không hợp lệ");
        return;
      }
    } else {
      // Customer chọn từ slot có sẵn
      if (!selectedDate || !selectedTime) {
        setError("Vui lòng chọn ngày và giờ");
        return;
      }

      // Handle time format - could be "HH:mm" or "HH:mm:ss"
      let timeStr = selectedTime;
      if (timeStr.length === 5) {
        // Format: "HH:mm"
        timeStr = `${timeStr}:00`;
      }

      // Create date in local timezone to avoid timezone conversion issues
      // Parse date and time components separately
      const [year, month, day] = selectedDate.split("-").map(Number);
      const [hours, minutes, seconds = 0] = timeStr.split(":").map(Number);
      
      // Create date object using local time components (not UTC)
      proposedDateTime = new Date(year, month - 1, day, hours, minutes, seconds);
      if (isNaN(proposedDateTime.getTime())) {
        setError("Thời gian không hợp lệ");
        return;
      }
    }

    if (proposedDateTime <= new Date()) {
      setError("Thời gian đề xuất phải trong tương lai");
      return;
    }

    // Validate: Must be at least 24 hours in the future
    const hoursUntil = (proposedDateTime - new Date()) / (1000 * 60 * 60);
    if (hoursUntil < 24) {
      setError("Thời gian đề xuất phải cách ít nhất 24 giờ");
      return;
    }

    // Validate: Cannot be more than 3 months in advance
    const daysUntil = (proposedDateTime - new Date()) / (1000 * 60 * 60 * 24);
    if (daysUntil > 90) {
      setError("Thời gian đề xuất không được quá 3 tháng");
      return;
    }

    // Validate: Reason is required and at least 5 characters
    if (!reason || reason.trim().length < 5) {
      setError("Lý do phải có ít nhất 5 ký tự");
      return;
    }

    onSubmit(proposedDateTime, reason.trim());
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div className="w-full max-w-md rounded-lg bg-white p-6 shadow-xl dark:bg-gray-800">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-xl font-bold text-gray-900 dark:text-white">
            Đề xuất đổi lịch
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        {appointment && (
          <div className="mb-4 rounded-lg bg-gray-50 p-3 dark:bg-gray-700">
            <p className="text-sm text-gray-600 dark:text-gray-300">
              Lịch hiện tại: <strong>{formatDateTime(appointment.appointmentDate || appointment.AppointmentDate)}</strong>
            </p>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              <Calendar className="inline h-4 w-4 mr-1" />
              Chọn ngày
            </label>
            <input
              type="date"
              value={selectedDate}
              onChange={(e) => setSelectedDate(e.target.value)}
              min={new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString().split("T")[0]}
              required
              className="w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-white"
            />
          </div>

          {isDoctorRequest ? (
            // Bác sĩ: Chọn tự do ngày và giờ
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                <Clock className="inline h-4 w-4 mr-1" />
                Chọn ngày và giờ <span className="text-gray-500 text-xs">(Bác sĩ có thể chọn bất kỳ thời gian nào)</span>
              </label>
              <input
                type="datetime-local"
                value={selectedDateTime}
                onChange={(e) => setSelectedDateTime(e.target.value)}
                min={new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString().slice(0, 16)}
                max={new Date(Date.now() + 90 * 24 * 60 * 60 * 1000).toISOString().slice(0, 16)}
                required
                className="w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-white"
              />
              <p className="mt-1 text-xs text-gray-500">
                Thời gian đề xuất phải cách ít nhất 24 giờ và không quá 3 tháng
              </p>
            </div>
          ) : (
            // Customer: Chọn từ slot có sẵn
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                <Clock className="inline h-4 w-4 mr-1" />
                Chọn giờ
              </label>
              {loadingSlots ? (
                <div className="text-sm text-gray-500">Đang tải khung giờ...</div>
              ) : availableSlots.length === 0 ? (
                <div className="text-sm text-red-500">
                  Không có khung giờ trống trong ngày này
                </div>
              ) : (
                <select
                  value={selectedTime}
                  onChange={(e) => setSelectedTime(e.target.value)}
                  required
                  className="w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-white"
                >
                  <option value="">-- Chọn giờ --</option>
                  {availableSlots
                    .filter((slot) => slot.isAvailable)
                    .map((slot) => {
                      const timeValue = typeof slot.startTime === 'string' 
                        ? slot.startTime 
                        : slot.startTime?.toString() || '';
                      return (
                        <option key={slot.slotId} value={timeValue}>
                          {timeValue} - {slot.endTime || ''}
                        </option>
                      );
                    })}
                </select>
              )}
            </div>
          )}

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              <MessageSquare className="inline h-4 w-4 mr-1" />
              Lý do <span className="text-red-500">*</span>
            </label>
            <textarea
              value={reason}
              onChange={(e) => setReason(e.target.value)}
              rows={3}
              minLength={5}
              maxLength={500}
              required
              placeholder="Nhập lý do đề xuất đổi lịch (tối thiểu 5 ký tự)..."
              className="w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none dark:border-gray-600 dark:bg-gray-700 dark:text-white"
            />
            <p className="mt-1 text-xs text-gray-500">
              {reason.length}/500 ký tự (tối thiểu 5 ký tự)
            </p>
          </div>

          {error && (
            <div className="rounded-md bg-red-50 p-3 text-sm text-red-600 dark:bg-red-900/20 dark:text-red-400">
              {error}
            </div>
          )}

          <div className="flex gap-2 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 rounded-md border border-gray-300 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-700"
            >
              Hủy
            </button>
            <button
              type="submit"
              disabled={
                isLoading || 
                !reason || 
                reason.trim().length < 5 ||
                (isDoctorRequest ? !selectedDateTime : (!selectedDate || !selectedTime))
              }
              className="flex-1 rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isLoading ? "Đang gửi..." : "Gửi đề xuất"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default RescheduleRequestModal;

