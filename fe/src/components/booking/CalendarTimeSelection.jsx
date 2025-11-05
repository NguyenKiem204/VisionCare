import React, { useState, useEffect, useMemo } from "react";
import Button from "../common/Button";
import Loading from "../common/Loading";
import { bookingAPI } from "../../services/bookingAPI";
import { signalRService } from "../../services/signalRService";
import toast from "react-hot-toast";
import { ChevronLeft, ChevronRight, X } from "lucide-react";

const CalendarTimeSelection = ({ data, update, next, back, holdSlot }) => {
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState(
    data.date ? new Date(data.date + "T00:00:00") : null
  );
  const [slots, setSlots] = useState([]);
  const [selectedSlot, setSelectedSlot] = useState(data.slotId || null);
  const [loading, setLoading] = useState(false);
  const [holdingSlot, setHoldingSlot] = useState(false);

  const doctorId = data.doctorId;
  const serviceTypeId = data.serviceTypeId || null;

  // Join SignalR group ngay khi có selectedDate và doctorId để nhận real-time updates
  useEffect(() => {
    if (!selectedDate || !doctorId) return;

    const y = selectedDate.getFullYear();
    const m = (selectedDate.getMonth() + 1).toString().padStart(2, "0");
    const d = selectedDate.getDate().toString().padStart(2, "0");
    const dateStr = `${y}-${m}-${d}`;

    // Join SignalR group ngay để nhận real-time updates
    signalRService.joinSlotsGroup(doctorId, dateStr).catch((error) => {
      console.error("Failed to join SignalR group:", error);
    });

    // Update data.date để useBooking hook cũng join group (backup)
    if (data.date !== dateStr) {
      update({ date: dateStr });
    }

    return () => {
      signalRService.leaveSlotsGroup(doctorId, dateStr).catch(console.error);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedDate, doctorId]);

  // Generate calendar days
  const calendarDays = useMemo(() => {
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();

    const firstDay = new Date(year, month, 1);
    // const lastDay = new Date(year, month + 1, 0);
    const startDate = new Date(firstDay);
    startDate.setDate(startDate.getDate() - firstDay.getDay()); // Start from Monday

    const days = [];
    const current = new Date(startDate);

    // Generate 6 weeks worth of days (42 days)
    for (let i = 0; i < 42; i++) {
      days.push(new Date(current));
      current.setDate(current.getDate() + 1);
    }

    return days;
  }, [currentMonth]);

  // Fetch slots when date is selected
  useEffect(() => {
    if (!selectedDate || !doctorId) return;

    const loadSlots = async () => {
      try {
        setLoading(true);
        // Build local date string YYYY-MM-DD to avoid timezone shifting
        const y = selectedDate.getFullYear();
        const m = (selectedDate.getMonth() + 1).toString().padStart(2, "0");
        const d = selectedDate.getDate().toString().padStart(2, "0");
        const dateStr = `${y}-${m}-${d}`;
        
        // Update data.date ngay để SignalR join group sớm
        if (data.date !== dateStr) {
          update({ date: dateStr });
        }
        
        let slotsData = await bookingAPI.getAvailableSlots(
          doctorId,
          dateStr,
          serviceTypeId
        );
        // Fallback: if filter theo serviceTypeId không có, thử bỏ lọc
        if (Array.isArray(slotsData) && slotsData.length === 0 && serviceTypeId) {
          slotsData = await bookingAPI.getAvailableSlots(doctorId, dateStr, null);
        }
        setSlots(slotsData);
        // Chỉ reset selectedSlot khi thay đổi ngày/BS/serviceType, không reset khi refresh do SignalR
        if (!data.refreshSlots) {
          setSelectedSlot(null);
        } else {
          // Khi reload do SignalR: giữ lại selectedSlot nếu vẫn tồn tại và do mình hold
          setSelectedSlot(prev => {
            if (prev) {
              const slot = slotsData.find(s => s.slotId === prev);
              // Giữ lại nếu slot vẫn tồn tại và (không bị hold hoặc do mình hold)
              if (slot && (!slot.isOnHold || (slot.isOnHold && slot.holdByCustomerId === (data.customerId || null)))) {
                return prev;
              }
            }
            return null; // Slot không còn tồn tại hoặc bị người khác hold
          });
        }
      } catch (error) {
        console.error("Error loading slots:", error);
        toast.error("Không thể tải lịch khả dụng");
        setSlots([]);
      } finally {
        setLoading(false);
      }
    };

    loadSlots();
  }, [selectedDate, doctorId, serviceTypeId, data.refreshSlots, data.customerId]);

  const handleDateSelect = (date) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const dateToCheck = new Date(date);
    dateToCheck.setHours(0, 0, 0, 0);

    // Don't allow selecting past dates
    if (dateToCheck < today) {
      return;
    }

    setSelectedDate(date);
    setSelectedSlot(null);
  };

  const handleSlotSelect = async (slot) => {
    // Check xem slot này có phải do mình hold không
    const isHeldByMe = slot.isOnHold && slot.holdByCustomerId === (data.customerId || null);
    
    // Nếu slot đang do mình hold → hủy hold (không cần check selectedSlot)
    if (isHeldByMe) {
      try {
        setHoldingSlot(true);
        const y = selectedDate.getFullYear();
        const m = (selectedDate.getMonth() + 1).toString().padStart(2, "0");
        const d = selectedDate.getDate().toString().padStart(2, "0");
        const dateStr = `${y}-${m}-${d}`;
        
        // Lấy holdToken từ localStorage nếu có
        const holdKey = `hold:${doctorId}:${slot.slotId}:${dateStr}`;
        const savedHold = localStorage.getItem(holdKey);
        let holdToken = null;
        if (savedHold) {
          try {
            const holdData = JSON.parse(savedHold);
            holdToken = holdData.holdToken;
          } catch (e) {
            console.warn("Failed to parse saved hold data", e);
          }
        }
        
        // Release hold với thông tin từ slot hiện tại
        // Backend sẽ xử lý bằng slotKey nếu holdToken không có
        await bookingAPI.releaseHold({
          holdToken: holdToken || "",
          doctorId: doctorId,
          slotId: slot.slotId,
          scheduleDate: dateStr,
        });
        
        // Clear localStorage
        localStorage.removeItem(holdKey);
        
        // Update state trong useBooking hook
        update({
          slotId: null,
          date: dateStr, // Đảm bảo date được set để releaseHold trong hook có thể hoạt động
        });
        setSelectedSlot(null);
        
        // Refresh slots để cập nhật UI
        const slotsData = await bookingAPI.getAvailableSlots(doctorId, dateStr, serviceTypeId);
        setSlots(slotsData);
        
        toast.success("Đã hủy giữ slot");
      } catch (error) {
        console.error("Error releasing hold:", error);
        toast.error("Không thể hủy giữ slot");
      } finally {
        setHoldingSlot(false);
      }
      return;
    }

    // Nếu slot không khả dụng hoặc đang được giữ bởi người khác
    if (!slot.isAvailable || (slot.isOnHold && !isHeldByMe)) {
      toast.error("Slot này không khả dụng");
      return;
    }

    try {
      setHoldingSlot(true);
      const y = selectedDate.getFullYear();
      const m = (selectedDate.getMonth() + 1).toString().padStart(2, "0");
      const d = selectedDate.getDate().toString().padStart(2, "0");
      const dateStr = `${y}-${m}-${d}`;

      // Hold the slot
      await holdSlot(doctorId, slot.slotId, dateStr);

      setSelectedSlot(slot.slotId);
      update({
        doctorId: doctorId,
        slotId: slot.slotId,
        date: dateStr,
        time: slot.startTime,
      });

      toast.success("Đã giữ slot này trong 10 phút");
    } catch (error) {
      console.error("Error holding slot:", error);
      toast.error("Không thể giữ slot này");
    } finally {
      setHoldingSlot(false);
    }
  };

  const handleNext = () => {
    if (!selectedDate) {
      toast.error("Vui lòng chọn ngày");
      return;
    }
    if (!selectedSlot) {
      toast.error("Vui lòng chọn thời gian khám");
      return;
    }
    next();
  };

  const formatTime = (timeStr) => {
    if (!timeStr) return "";
    // Ensure HH:mm format
    return timeStr.slice(0, 5);
  };

  const formatDuration = (minutes) => {
    if (minutes < 60) return `${minutes} phút`;
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return mins > 0 ? `${hours}h${mins}` : `${hours} giờ`;
  };

  // Compute valid starting slots. Nếu có serviceTypeId, hiển thị trực tiếp slots BE trả về
  // no-op placeholder to keep previous logic simple
  const BUFFER_MIN = 15; // must match server buffer

  const parseHM = (hm) => {
    const [h, m] = (hm || "00:00").split(":").map(Number);
    return h * 60 + m;
  };
  // removed unused toHM

  const validStarts = useMemo(() => {
    if (!Array.isArray(slots) || slots.length === 0) return [];
    // Nếu đã chọn dịch vụ → slots đã đúng duration, trả nguyên
    if (serviceTypeId) return [...slots].sort((a, b) => parseHM(a.startTime) - parseHM(b.startTime));
    // Chưa chọn dịch vụ → dùng lưới 60'+15' (mặc định)
    const byStart = [...slots].sort((a, b) => parseHM(a.startTime) - parseHM(b.startTime));
    const results = [];
    for (let i = 0; i < byStart.length; i++) {
      const start = byStart[i];
      // chỉ cần điểm bắt đầu hợp lệ
      results.push(start);
      // bỏ lọc chain nâng cao để tăng tốc
    }
    return results;
  }, [slots, serviceTypeId]);

  const monthNames = [
    "Tháng 1",
    "Tháng 2",
    "Tháng 3",
    "Tháng 4",
    "Tháng 5",
    "Tháng 6",
    "Tháng 7",
    "Tháng 8",
    "Tháng 9",
    "Tháng 10",
    "Tháng 11",
    "Tháng 12",
  ];

  const dayNames = ["MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"];

  const navigateMonth = (direction) => {
    setCurrentMonth((prev) => {
      const newDate = new Date(prev);
      newDate.setMonth(prev.getMonth() + direction);
      return newDate;
    });
  };

  const isToday = (date) => {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  };

  const isSelected = (date) => {
    if (!selectedDate) return false;
    return (
      date.getDate() === selectedDate.getDate() &&
      date.getMonth() === selectedDate.getMonth() &&
      date.getFullYear() === selectedDate.getFullYear()
    );
  };

  const isPast = (date) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const dateToCheck = new Date(date);
    dateToCheck.setHours(0, 0, 0, 0);
    return dateToCheck < today;
  };

  const isCurrentMonth = (date) => {
    return date.getMonth() === currentMonth.getMonth();
  };

  const clearSelection = () => {
    setSelectedDate(null);
    setSelectedSlot(null);
    setSlots([]);
  };

  return (
    <div>
      <h3 className="text-lg font-semibold mb-6">Chọn ngày và giờ</h3>

      {/* Calendar */}
      <div className="mb-8">
        <div className="flex items-center justify-between mb-4">
          <button
            onClick={() => navigateMonth(-1)}
            className="p-2 rounded-lg hover:bg-gray-100 transition-colors"
          >
            <ChevronLeft className="w-5 h-5" />
          </button>
          <h4 className="text-xl font-semibold">
            {monthNames[currentMonth.getMonth()]} {currentMonth.getFullYear()}
          </h4>
          <button
            onClick={() => navigateMonth(1)}
            className="p-2 rounded-lg hover:bg-gray-100 transition-colors"
          >
            <ChevronRight className="w-5 h-5" />
          </button>
        </div>

        {/* Day names */}
        <div className="grid grid-cols-7 gap-1 mb-2">
          {dayNames.map((day) => (
            <div
              key={day}
              className="text-center text-sm font-semibold text-gray-600 py-2"
            >
              {day}
            </div>
          ))}
        </div>

        {/* Calendar grid */}
        <div className="grid grid-cols-7 gap-1">
          {calendarDays.map((date, idx) => {
            const isDatePast = isPast(date);
            const isDateSelected = isSelected(date);
            const isDateToday = isToday(date);
            const isDateCurrentMonth = isCurrentMonth(date);

            return (
              <button
                key={idx}
                onClick={() => !isDatePast && handleDateSelect(date)}
                disabled={isDatePast}
                className={`
                  aspect-square rounded-lg border-2 transition-all text-sm font-medium
                  ${
                    isDatePast
                      ? "bg-gray-100 text-gray-400 border-gray-200 cursor-not-allowed"
                      : isDateSelected
                      ? "bg-blue-600 text-white border-blue-600 shadow-md"
                      : isDateToday
                      ? "bg-blue-50 text-blue-600 border-blue-300 hover:border-blue-400"
                      : isDateCurrentMonth
                      ? "bg-white text-gray-900 border-gray-200 hover:border-blue-300 hover:bg-blue-50"
                      : "bg-gray-50 text-gray-400 border-gray-100"
                  }
                `}
              >
                {date.getDate()}
              </button>
            );
          })}
        </div>
      </div>

      {/* Thời lượng ràng buộc theo dịch vụ đã chọn (ẩn chọn tay) */}

      {/* Time slots */}
      {selectedDate && (
        <div className="mb-6">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-lg font-semibold">
              Giờ khả dụng -{" "}
              {selectedDate.toLocaleDateString("vi-VN", {
                weekday: "long",
                day: "numeric",
                month: "long",
                year: "numeric",
              })}
            </h3>
            <button
              onClick={clearSelection}
              className="p-1 rounded-lg hover:bg-gray-100 transition-colors"
              title="Hủy chọn ngày"
            >
              <X className="w-5 h-5 text-gray-500" />
            </button>
          </div>

          {loading ? (
            <Loading />
          ) : slots.length === 0 ? (
            <p className="text-gray-500 text-center py-8">
              Không có slot khả dụng cho ngày này
            </p>
          ) : (
            <div className="grid grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-3">
              {validStarts.map((slot) => {
                const isSlotSelected = selectedSlot === slot.slotId;
                // Check xem slot này có phải do mình hold không
                const isHeldByMe = slot.isOnHold && slot.holdByCustomerId === (data.customerId || null);
                // Disable nếu: 
                // - Đang loading (holdingSlot)
                // - Không available VÀ không phải do mình hold (để có thể click hủy hold)
                // - Đang được giữ bởi người khác (không phải mình, không phải slot đã chọn)
                const isDisabled = holdingSlot || 
                  (!slot.isAvailable && !isHeldByMe) || 
                  (slot.isOnHold && !isHeldByMe && !isSlotSelected);

                return (
                  <button
                    key={slot.slotId}
                    onClick={() => !isDisabled && handleSlotSelect(slot)}
                    disabled={isDisabled || holdingSlot}
                    className={`
                      px-4 py-3 rounded-xl border-2 text-sm font-medium transition-all
                      ${
                        isSlotSelected
                          ? "bg-blue-600 text-white border-blue-600 shadow-md"
                          : isHeldByMe
                          ? "bg-blue-50 text-blue-700 border-blue-300 hover:border-blue-500 hover:bg-blue-100 cursor-pointer"
                          : isDisabled
                          ? "bg-gray-100 text-gray-400 border-gray-200 cursor-not-allowed"
                          : "bg-white text-gray-900 border-gray-200 hover:border-blue-500 hover:bg-blue-50 hover:text-blue-600"
                      }
                    `}
                    title={
                      isHeldByMe
                        ? "Click để hủy giữ slot này"
                        : slot.isOnHold
                        ? "Slot đang được giữ bởi người khác"
                        : !slot.isAvailable
                        ? "Slot không khả dụng"
                        : `${formatTime(slot.startTime)} - ${formatTime(slot.endTime)} (${formatDuration(slot.durationMinutes)})`
                    }
                  >
                    <div>
                      <div className="font-semibold">
                        {formatTime(slot.startTime)} - {formatTime(slot.endTime)}
                      </div>
                      {slot.isOnHold && !isHeldByMe && !isSlotSelected && (
                        <div className="text-xs mt-1 opacity-75">Đang giữ</div>
                      )}
                      {slot.isOnHold && isHeldByMe && (
                        <div className="text-xs mt-1 text-blue-600">Đang giữ bởi bạn</div>
                      )}
                    </div>
                  </button>
                );
              })}
            </div>
          )}
        </div>
      )}

      {selectedSlot && (
        <div className="mb-6 p-4 bg-blue-50 border border-blue-200 rounded-lg">
          <p className="text-sm text-blue-800">
            <strong>Đã chọn:</strong>{" "}
            {selectedDate?.toLocaleDateString("vi-VN", {
              day: "numeric",
              month: "long",
              year: "numeric",
            })}{" "}
            - {formatTime(data.time)} ({formatDuration(
              slots.find((s) => s.slotId === selectedSlot)?.durationMinutes || 30
            )})
          </p>
        </div>
      )}

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button
          variant="accent"
          onClick={handleNext}
          disabled={!selectedDate || !selectedSlot || loading || holdingSlot}
        >
          Tiếp tục
        </Button>
      </div>
    </div>
  );
};

export default CalendarTimeSelection;

