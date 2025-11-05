import React from "react";
import { Clock, CheckCircle, XCircle } from "lucide-react";

const dayNames = [
  "Chủ nhật",
  "Thứ hai",
  "Thứ ba",
  "Thứ tư",
  "Thứ năm",
  "Thứ sáu",
  "Thứ bảy",
];

const timeSlots = [
  { start: "08:00", end: "08:30" },
  { start: "08:30", end: "09:00" },
  { start: "09:00", end: "09:30" },
  { start: "09:30", end: "10:00" },
  { start: "10:00", end: "10:30" },
  { start: "10:30", end: "11:00" },
  { start: "11:00", end: "11:30" },
  { start: "13:30", end: "14:00" },
  { start: "14:00", end: "14:30" },
  { start: "14:30", end: "15:00" },
  { start: "15:00", end: "15:30" },
  { start: "15:30", end: "16:00" },
  { start: "16:00", end: "16:30" },
];

const WeeklyScheduleCalendar = ({ schedules, loading, onRefresh }) => {
  // Group schedules by day of week
  const schedulesByDay = schedules.reduce((acc, schedule) => {
    const day = schedule.dayOfWeek;
    if (!acc[day]) acc[day] = [];
    acc[day].push(schedule);
    return acc;
  }, {});

  if (loading) {
    return (
      <div className="p-12 text-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
        <p className="mt-4 text-gray-600 dark:text-gray-400">Đang tải lịch làm việc...</p>
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className="mb-6">
        <h2 className="text-xl font-semibold text-gray-900 dark:text-white mb-2">
          Lịch làm việc theo tuần
        </h2>
        <p className="text-sm text-gray-600 dark:text-gray-400">
          Lịch này sẽ được sử dụng để tự động tạo lịch hẹn hàng ngày
        </p>
      </div>

      {/* Calendar Grid */}
      <div className="overflow-x-auto">
        <div className="min-w-[1200px]">
          {/* Header */}
          <div className="grid grid-cols-8 gap-2 mb-2">
            <div className="font-semibold text-gray-700 dark:text-gray-300 p-3 bg-gray-50 dark:bg-gray-900 rounded-lg">
              Giờ
            </div>
            {dayNames.map((day, index) => (
              <div
                key={index}
                className="font-semibold text-gray-700 dark:text-gray-300 p-3 bg-blue-50 dark:bg-blue-900/30 rounded-lg text-center"
              >
                {day}
              </div>
            ))}
          </div>

          {/* Time slots */}
          {timeSlots.map((slot, slotIndex) => (
            <div key={slotIndex} className="grid grid-cols-8 gap-2 mb-2">
              {/* Time label */}
              <div className="p-3 bg-gray-50 dark:bg-gray-900 rounded-lg flex items-center justify-center text-sm text-gray-600 dark:text-gray-400">
                <Clock className="h-4 w-4 mr-1" />
                {slot.start}
              </div>

              {/* Days */}
              {dayNames.map((_, dayIndex) => {
                const dayOfWeek = dayIndex === 0 ? 0 : dayIndex; // Sunday = 0
                const daySchedules = schedulesByDay[dayOfWeek] || [];
                const hasSchedule = daySchedules.some((s) => {
                  const scheduleTime = s.startTime?.slice(0, 5) || "";
                  return scheduleTime === slot.start;
                });

                return (
                  <div
                    key={dayIndex}
                    className={`p-3 rounded-lg border-2 transition-all ${
                      hasSchedule
                        ? "bg-green-50 dark:bg-green-900/30 border-green-300 dark:border-green-700"
                        : "bg-gray-50 dark:bg-gray-800 border-gray-200 dark:border-gray-700"
                    }`}
                  >
                    {hasSchedule ? (
                      <div className="flex items-center justify-center">
                        <CheckCircle className="h-5 w-5 text-green-600 dark:text-green-400" />
                      </div>
                    ) : (
                      <div className="flex items-center justify-center opacity-30">
                        <XCircle className="h-4 w-4 text-gray-400" />
                      </div>
                    )}
                  </div>
                );
              })}
            </div>
          ))}
        </div>
      </div>

      {/* Legend */}
      <div className="mt-6 flex items-center gap-6 text-sm">
        <div className="flex items-center gap-2">
          <CheckCircle className="h-4 w-4 text-green-600" />
          <span className="text-gray-700 dark:text-gray-300">Có lịch</span>
        </div>
        <div className="flex items-center gap-2">
          <XCircle className="h-4 w-4 text-gray-400" />
          <span className="text-gray-700 dark:text-gray-300">Trống</span>
        </div>
      </div>

      {/* Summary */}
      {schedules.length > 0 && (
        <div className="mt-6 p-4 bg-blue-50 dark:bg-blue-900/20 rounded-lg">
          <p className="text-sm text-gray-700 dark:text-gray-300">
            <strong>Tổng cộng:</strong> {schedules.length} slot trong tuần
          </p>
        </div>
      )}
    </div>
  );
};

export default WeeklyScheduleCalendar;

