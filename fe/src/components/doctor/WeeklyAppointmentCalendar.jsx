import React, { useMemo } from "react";
import { Clock, User, CalendarClock } from "lucide-react";

const dayNames = ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"];

const generateTimeSlots = () => {
  const slots = [];
  
  slots.push("08:00");
  slots.push("09:15");
  slots.push("10:30");
  
  slots.push("13:00");
  slots.push("14:15");
  slots.push("15:30");
  
  slots.push("17:00");
  slots.push("18:15");
  slots.push("19:30");
  
  return slots;
};

const timeSlots = generateTimeSlots();

const WeeklyAppointmentCalendar = ({
  appointments = [],
  schedules = [],
  currentWeekStart,
  onAppointmentClick,
  onRescheduleClick,
  canReschedule,
}) => {
  const weekDays = useMemo(() => {
    const days = [];
    const start = new Date(currentWeekStart);
    const dayOfWeek = start.getDay();
    const diff = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
    start.setDate(start.getDate() + diff);
    
    for (let i = 0; i < 7; i++) {
      const day = new Date(start);
      day.setDate(start.getDate() + i);
      days.push(day);
    }
    return days;
  }, [currentWeekStart]);


  const appointmentsBySlot = useMemo(() => {
    const map = new Map();
    appointments.forEach((appt) => {
      const date = new Date(appt.appointmentDate || appt.AppointmentDate);
      if (isNaN(date.getTime())) return;
      
      const dateKey = date.toISOString().split("T")[0];
      const timeKey = `${date.getHours().toString().padStart(2, "0")}:${date.getMinutes().toString().padStart(2, "0")}`;
      const slotKey = `${dateKey}_${timeKey}`;
      
      if (!map.has(slotKey)) {
        map.set(slotKey, []);
      }
      map.get(slotKey).push(appt);
    });
    return map;
  }, [appointments]);

  const schedulesByDate = useMemo(() => {
    const map = new Map();
    schedules.forEach((schedule) => {
      const date = schedule.scheduleDate || schedule.ScheduleDate;
      const startTime = schedule.startTime || schedule.StartTime;
      const endTime = schedule.endTime || schedule.EndTime;
      
      if (!date || !startTime) {
        return;
      }
      
      // Handle different date formats
      let dateStr = "";
      if (typeof date === "string") {
        // Could be "2025-11-16" or "2025-11-16T00:00:00" or "2025-11-16T00:00:00Z"
        dateStr = date.split("T")[0];
      } else if (date instanceof Date) {
        dateStr = date.toISOString().split("T")[0];
      } else if (date && typeof date === "object" && date.year) {
        // DateOnly format from backend: { year: 2025, month: 11, day: 16 }
        dateStr = `${date.year}-${String(date.month).padStart(2, "0")}-${String(date.day).padStart(2, "0")}`;
      } else {
        dateStr = String(date);
      }
      
      if (!map.has(dateStr)) {
        map.set(dateStr, []);
      }
      map.get(dateStr).push({
        ...schedule,
        startTime: startTime,
        endTime: endTime,
      });
    });

    return map;
  }, [schedules]);

  const timeToMinutes = (timeStr) => {
    if (!timeStr) return 0;
    const str = typeof timeStr === "string" ? timeStr.substring(0, 5) : String(timeStr);
    const [hours, minutes] = str.split(":").map(Number);
    return (hours || 0) * 60 + (minutes || 0);
  };

  const formatDateKey = (date) => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
  };

  const getAppointmentsForSlot = (day, timeSlot) => {
    const dateKey = formatDateKey(day);
    const slotStartMinutes = timeToMinutes(timeSlot);
    
    return Array.from(appointmentsBySlot.values())
      .flat()
      .filter((appt) => {
        const apptDate = new Date(appt.appointmentDate || appt.AppointmentDate);
        if (isNaN(apptDate.getTime())) return false;
        const apptDateKey = formatDateKey(apptDate);
        if (apptDateKey !== dateKey) return false;
        
        const apptMinutes = apptDate.getHours() * 60 + apptDate.getMinutes();
        return Math.abs(apptMinutes - slotStartMinutes) <= 15;
      });
  };

  const findBestScheduleForSlot = (daySchedules, timeSlot) => {
    if (!daySchedules || daySchedules.length === 0) {
      return null;
    }
    
    const slotStartMinutes = timeToMinutes(timeSlot);
    
    // Tìm schedule mà time slot nằm trong khoảng của nó
    for (const schedule of daySchedules) {
      const scheduleStart = timeToMinutes(schedule.startTime);
      const scheduleEnd = timeToMinutes(schedule.endTime);
      
      // Chỉ match nếu slot nằm trong khoảng schedule
      if (scheduleStart <= slotStartMinutes && scheduleEnd > slotStartMinutes) {
        return schedule;
      }
      
      // Hoặc match nếu slot start time trùng với schedule start time (trong vòng 15 phút)
      if (Math.abs(scheduleStart - slotStartMinutes) <= 15) {
        return schedule;
      }
    }
    
    return null;
  };

  const getStatusColor = (status) => {
    switch (status) {
      case "Confirmed":
        return "bg-blue-100 border-blue-300 text-blue-800 dark:bg-blue-900/30 dark:border-blue-700 dark:text-blue-300";
      case "Scheduled":
      case "Pending":
        return "bg-yellow-100 border-yellow-300 text-yellow-800 dark:bg-yellow-900/30 dark:border-yellow-700 dark:text-yellow-300";
      case "Completed":
        return "bg-green-100 border-green-300 text-green-800 dark:bg-green-900/30 dark:border-green-700 dark:text-green-300";
      case "PendingReschedule":
        return "bg-orange-100 border-orange-300 text-orange-800 dark:bg-orange-900/30 dark:border-orange-700 dark:text-orange-300";
      case "Cancelled":
      case "Canceled":
        return "bg-red-100 border-red-300 text-red-800 dark:bg-red-900/30 dark:border-red-700 dark:text-red-300";
      default:
        return "bg-gray-100 border-gray-300 text-gray-800 dark:bg-gray-700 dark:border-gray-600 dark:text-gray-300";
    }
  };

  const formatScheduleTime = (schedule) => {
    if (!schedule) return "";
    const start = typeof schedule.startTime === "string" 
      ? schedule.startTime.substring(0, 5) 
      : schedule.startTime;
    const end = typeof schedule.endTime === "string"
      ? schedule.endTime.substring(0, 5)
      : schedule.endTime;
    return `${start} - ${end}`;
  };

  return (
    <div className="overflow-x-auto bg-white dark:bg-gray-800 rounded-lg shadow [&::-webkit-scrollbar]:hidden [-ms-overflow-style:none] [scrollbar-width:none]">
      <div className="min-w-[900px]">
        <div className="grid grid-cols-8 gap-1.5 p-1.5 border-b border-gray-200 dark:border-gray-700">
          <div className="font-semibold text-gray-700 dark:text-gray-300 p-1.5 bg-gray-50 dark:bg-gray-900 rounded text-sm">
            Giờ
          </div>
          {weekDays.map((day, index) => (
            <div
              key={index}
              className="font-semibold text-gray-700 dark:text-gray-300 p-1.5 bg-blue-50 dark:bg-blue-900/30 rounded text-center text-sm"
            >
              <div>{dayNames[day.getDay()]}</div>
              <div className="text-xs text-gray-500 dark:text-gray-400">
                {day.getDate()}/{day.getMonth() + 1}
              </div>
            </div>
          ))}
        </div>

        <div className="max-h-[500px] overflow-y-auto [&::-webkit-scrollbar]:hidden [-ms-overflow-style:none] [scrollbar-width:none]">
          {timeSlots.map((timeSlot, slotIndex) => (
            <div key={slotIndex} className="grid grid-cols-8 gap-1.5 p-1.5 border-b border-gray-100 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700/50">
              <div className="p-1.5 bg-gray-50 dark:bg-gray-900 rounded flex items-center justify-center text-xs text-gray-600 dark:text-gray-400">
                <Clock className="h-3 w-3 mr-1" />
                {timeSlot}
              </div>

              {weekDays.map((day, dayIndex) => {
                const dateKey = formatDateKey(day);
                const appointments = getAppointmentsForSlot(day, timeSlot);
                const daySchedules = schedulesByDate.get(dateKey) || [];
                const schedule = findBestScheduleForSlot(daySchedules, timeSlot);
                
                const hasAppointment = appointments.length > 0;
                const isAvailable = schedule && (schedule.status === "Available" || schedule.Status === "Available");
                const isBooked = schedule && (schedule.status === "Booked" || schedule.Status === "Booked");

                return (
                  <div
                    key={dayIndex}
                    className="min-h-[50px] p-0.5 rounded border-2 border-transparent"
                  >
                    {hasAppointment ? (
                      <div className="space-y-1">
                        {appointments.map((appt, idx) => {
                          const status = appt.status || appt.AppointmentStatus || "Scheduled";
                          const patientName = appt.patientName || appt.PatientName || "Bệnh nhân";
                          const apptDate = new Date(appt.appointmentDate || appt.AppointmentDate);
                          const apptTime = `${apptDate.getHours().toString().padStart(2, "0")}:${apptDate.getMinutes().toString().padStart(2, "0")}`;
                          // Kiểm tra điều kiện hiển thị button
                          const paymentStatus = appt.paymentStatus || appt.PaymentStatus;
                          const canRescheduleAppt = canReschedule && canReschedule(appt);
                          // Hiển thị button nếu canReschedule trả về true (bao gồm cả Rescheduled nếu đã paid và còn cách 24h)
                          const shouldShowReschedule = canRescheduleAppt || (
                            (status === "Confirmed" || status === "Scheduled" || status === "Pending" || status === "Rescheduled") &&
                            paymentStatus === "Paid" &&
                            status !== "Completed" &&
                            status !== "Canceled" &&
                            status !== "Cancelled" &&
                            status !== "PendingReschedule"
                          );
                          
                          return (
                            <div
                              key={idx}
                              className={`p-2 rounded text-xs border hover:shadow-md transition ${getStatusColor(status)}`}
                            >
                              <div 
                                className="cursor-pointer"
                                onClick={() => onAppointmentClick && onAppointmentClick(appt)}
                              >
                                <div className="font-semibold truncate flex items-center gap-1">
                                  <User className="h-3 w-3" />
                                  {patientName}
                                </div>
                                <div className="text-xs opacity-80 mt-1 truncate">
                                  {appt.serviceName || appt.ServiceName || "Dịch vụ"}
                                </div>
                                <div className="text-xs opacity-70 mt-0.5">
                                  {apptTime}
                                </div>
                                {status === "PendingReschedule" && (
                                  <div className="mt-1 flex items-center gap-1 text-xs text-yellow-600 dark:text-yellow-400">
                                    <CalendarClock className="h-3 w-3" />
                                    <span>Chờ đổi lịch</span>
                                  </div>
                                )}
                              </div>
                              {/* Button Đề xuất đổi lịch - Tách ra ngoài để tránh conflict với onClick của parent */}
                              {shouldShowReschedule && onRescheduleClick && (
                                <button
                                  type="button"
                                  onClick={(e) => {
                                    e.preventDefault();
                                    e.stopPropagation();
                                    onRescheduleClick(appt);
                                  }}
                                  onMouseDown={(e) => {
                                    e.preventDefault();
                                    e.stopPropagation();
                                  }}
                                  className="mt-1 w-full text-[10px] px-1.5 py-0.5 rounded bg-yellow-50 text-yellow-700 hover:bg-yellow-100 dark:bg-yellow-900/30 dark:text-yellow-300 flex items-center justify-center gap-1 border border-yellow-300 dark:border-yellow-600 cursor-pointer relative z-50"
                                  style={{ pointerEvents: 'auto' }}
                                  title="Đề xuất đổi lịch khám cho khách hàng"
                                >
                                  <CalendarClock className="h-2.5 w-2.5" />
                                  <span>Đề xuất đổi lịch</span>
                                </button>
                              )}
                            </div>
                          );
                        })}
                      </div>
                    ) : schedule ? (
                      <div className={`h-full flex flex-col items-center justify-center text-xs rounded p-1 border-2 ${
                        isBooked 
                          ? "text-gray-600 dark:text-gray-400 border-dashed border-gray-300 dark:border-gray-600 bg-gray-50 dark:bg-gray-800"
                          : isAvailable
                          ? "text-green-600 dark:text-green-400 border-dashed border-green-300 dark:border-green-600 bg-green-50 dark:bg-green-900/20"
                          : "text-gray-400 dark:text-gray-600 border-dashed border-gray-200 dark:border-gray-700"
                      }`}>
                        <div className="font-medium text-[10px]">
                          {isBooked ? "Đã đặt" : isAvailable ? "Trống" : "N/A"}
                        </div>
                        <div className="text-[9px] opacity-75 mt-0.5">
                          {formatScheduleTime(schedule)}
                        </div>
                      </div>
                    ) : (
                      <div className="h-full flex items-center justify-center text-xs text-gray-300 dark:text-gray-700">
                        —
                      </div>
                    )}
                  </div>
                );
              })}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default WeeklyAppointmentCalendar;