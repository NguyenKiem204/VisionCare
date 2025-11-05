import React, { useEffect, useState } from "react";
import { Calendar, Clock, MapPin, Wrench } from "lucide-react";
import { getMyDoctorSchedules } from "../../services/doctorMeAPI";

const MyDoctorSchedules = () => {
  const [schedules, setSchedules] = useState([]);
  const [loading, setLoading] = useState(false);

  const loadSchedules = async () => {
    setLoading(true);
    try {
      const res = await getMyDoctorSchedules();
      setSchedules(Array.isArray(res) ? res : []);
    } catch (error) {
      console.error("Failed to load doctor schedules:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadSchedules();
  }, []);

  const getDayOfWeekName = (dayOfWeek) => {
    const days = ["", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật"];
    return days[dayOfWeek] || "Tất cả";
  };

  const getRecurrenceText = (rule, dayOfWeek) => {
    if (rule === "WEEKLY" && dayOfWeek) {
      return `Hàng tuần - ${getDayOfWeekName(dayOfWeek)}`;
    }
    if (rule === "DAILY") return "Hàng ngày";
    if (rule === "MONTHLY") return "Hàng tháng";
    return rule || "Tùy chỉnh";
  };

  const formatDate = (date) => {
    if (!date) return "Không giới hạn";
    return new Date(date).toLocaleDateString("vi-VN");
  };

  const formatTime = (time) => {
    if (!time) return "N/A";
    if (typeof time === 'string') return time.substring(0, 5);
    return time;
  };

  return (
    <div className="max-w-6xl mx-auto px-4 py-6">
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Lịch làm việc định kỳ</h1>
            <p className="text-gray-600 dark:text-gray-300 mt-1">Xem lịch làm việc định kỳ của bạn</p>
          </div>
          <button
            onClick={loadSchedules}
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition flex items-center gap-2"
          >
            <Calendar className="h-4 w-4" />
            Làm mới
          </button>
        </div>

        {loading && (
          <div className="text-center py-8 text-gray-500 dark:text-gray-400">Đang tải...</div>
        )}

        {!loading && schedules.length === 0 && (
          <div className="text-center py-8 text-gray-500 dark:text-gray-400">
            Chưa có lịch làm việc định kỳ nào
          </div>
        )}

        {!loading && schedules.length > 0 && (
          <div className="grid gap-4">
            {schedules.map((schedule) => (
              <div
                key={schedule.id}
                className="border border-gray-200 dark:border-gray-700 rounded-lg p-4 hover:shadow-md transition"
              >
                <div className="flex justify-between items-start">
                  <div className="flex-1">
                    <div className="flex items-center gap-3 mb-2">
                      <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                        {schedule.shiftName || "Ca làm việc"}
                      </h3>
                      <span
                        className={`px-2 py-1 text-xs font-semibold rounded-full ${
                          schedule.isActive
                            ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200"
                            : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300"
                        }`}
                      >
                        {schedule.isActive ? "Đang hoạt động" : "Tạm dừng"}
                      </span>
                    </div>

                    <div className="grid grid-cols-2 md:grid-cols-3 gap-4 mt-3">
                      <div className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-300">
                        <Clock className="h-4 w-4" />
                        <span>
                          {formatTime(schedule.startTime)} - {formatTime(schedule.endTime)}
                        </span>
                      </div>

                      <div className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-300">
                        <Calendar className="h-4 w-4" />
                        <span>{getRecurrenceText(schedule.recurrenceRule, schedule.dayOfWeek)}</span>
                      </div>

                      {schedule.roomName && (
                        <div className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-300">
                          <MapPin className="h-4 w-4" />
                          <span>Phòng: {schedule.roomName}</span>
                        </div>
                      )}

                      {schedule.equipmentName && (
                        <div className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-300">
                          <Wrench className="h-4 w-4" />
                          <span>Thiết bị: {schedule.equipmentName}</span>
                        </div>
                      )}
                    </div>

                    <div className="mt-3 text-sm text-gray-500 dark:text-gray-400">
                      <div>Thời gian: {formatDate(schedule.startDate)} - {formatDate(schedule.endDate)}</div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default MyDoctorSchedules;

