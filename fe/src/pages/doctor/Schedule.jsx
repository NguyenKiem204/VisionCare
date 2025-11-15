import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getSchedules,
  getUpcomingAppointments,
  getMyAppointments,
  confirmMyAppointment,
  completeMyAppointment,
  cancelMyAppointment,
  requestReschedule,
  approveReschedule,
  rejectReschedule,
} from "../../services/doctorMeAPI";
import {
  Calendar,
  CheckCircle,
  XCircle,
  Clock,
  Eye,
  CalendarClock,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";
import RescheduleNotificationCard from "../../components/common/RescheduleNotificationCard";
import RescheduleRequestModal from "../../components/common/RescheduleRequestModal";
import WeeklyAppointmentCalendar from "../../components/doctor/WeeklyAppointmentCalendar";
import toast from "react-hot-toast";
import { formatDateTime } from "../../utils/formatDate";
import { useAuth } from "../../contexts/AuthContext";

const DoctorSchedule = () => {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [loading, setLoading] = useState(false);
  const [todaySchedules, setTodaySchedules] = useState([]);
  const [upcoming, setUpcoming] = useState([]);
  const [allAppointments, setAllAppointments] = useState([]);
  const [weekSchedules, setWeekSchedules] = useState([]);
  const [viewMode, setViewMode] = useState("today"); // "today", "week", "month"
  const [currentDate, setCurrentDate] = useState(new Date());
  const [showRescheduleModal, setShowRescheduleModal] = useState(null);
  const [rescheduleLoading, setRescheduleLoading] = useState(false);
  const today = useMemo(() => new Date(), []);

  const load = async () => {
    setLoading(true);
    try {
      const dateStr = today.toISOString().slice(0, 10);
      const [schedulesRes, upcomingRes] = await Promise.all([
        getSchedules({ date: dateStr }),
        getUpcomingAppointments(10),
      ]);
      setTodaySchedules(Array.isArray(schedulesRes) ? schedulesRes : []);
      setUpcoming(Array.isArray(upcomingRes) ? upcomingRes : []);

      // Load appointments based on view mode
      await loadAppointmentsByViewMode();
    } catch (e) {
      console.error("Load schedules error", e);
    } finally {
      setLoading(false);
    }
  };

  const loadAppointmentsByViewMode = async () => {
    try {
      let from, to;
      const start = new Date(currentDate);
      start.setHours(0, 0, 0, 0);

      if (viewMode === "today") {
        from = start;
        to = new Date(start);
        to.setHours(23, 59, 59, 999);
      } else if (viewMode === "week") {
        from = new Date(start);
        // Adjust to Monday
        const dayOfWeek = from.getDay();
        const diff = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
        from.setDate(from.getDate() + diff);
        from.setHours(0, 0, 0, 0); // Start of Monday
        to = new Date(from);
        to.setDate(from.getDate() + 6); // Sunday
        to.setHours(23, 59, 59, 999); // End of Sunday

        // Load schedules for the week (7 days: Mon to Sun)
        const schedulesPromises = [];
        // Loop through 7 days explicitly
        for (let i = 0; i < 7; i++) {
          const d = new Date(from);
          d.setDate(from.getDate() + i);
          // Use local date to avoid timezone issues
          const year = d.getFullYear();
          const month = String(d.getMonth() + 1).padStart(2, "0");
          const day = String(d.getDate()).padStart(2, "0");
          const dateStr = `${year}-${month}-${day}`;
          schedulesPromises.push(
            getSchedules({ date: dateStr }).catch(() => [])
          );
        }
        const weekSchedulesResults = await Promise.all(schedulesPromises);
        const allSchedules = weekSchedulesResults.flat();
        setWeekSchedules(allSchedules);
      } else if (viewMode === "month") {
        from = new Date(start.getFullYear(), start.getMonth(), 1);
        to = new Date(start.getFullYear(), start.getMonth() + 1, 0);
        to.setHours(23, 59, 59, 999);
      }

      if (from && to) {
        const appointments = await getMyAppointments(
          from.toISOString(),
          to.toISOString()
        );
        setAllAppointments(Array.isArray(appointments) ? appointments : []);
      }
    } catch (e) {
      console.error("Load appointments error", e);
    }
  };

  useEffect(() => {
    load();
  }, []);

  useEffect(() => {
    if (viewMode !== "today") {
      loadAppointmentsByViewMode();
    }
  }, [viewMode, currentDate]);

  const onConfirm = async (id) => {
    await confirmMyAppointment(id);
    await load();
  };

  const onComplete = async (id) => {
    await completeMyAppointment(id);
    await load();
  };

  const onCancel = async (id) => {
    await cancelMyAppointment(id);
    await load();
  };

  const extractProposedDateTime = (notes) => {
    if (!notes) return null;
    // Match ALL reschedule requests and get the LAST one (most recent)
    const matches = [...notes.matchAll(/\[(?:Doctor|Customer|Counter)\]\s*Đề xuất đổi lịch:\s*(\d{2}\/\d{2}\/\d{4} \d{2}:\d{2})/g)];
    if (matches.length === 0) return null;
    
    // Get the last match (most recent request)
    const lastMatch = matches[matches.length - 1];
    const [date, time] = lastMatch[1].split(" ");
    const [day, month, year] = date.split("/");
    const [hours, minutes] = time.split(":");
    // Create date in local timezone (not UTC)
    const dateObj = new Date();
    dateObj.setFullYear(parseInt(year), parseInt(month) - 1, parseInt(day));
    dateObj.setHours(parseInt(hours), parseInt(minutes), 0, 0);
    return dateObj;
  };

  const extractReason = (notes) => {
    if (!notes) return null;
    // Find the last reschedule request and extract its reason
    const matches = [...notes.matchAll(/\[(?:Doctor|Customer|Counter)\]\s*Đề xuất đổi lịch:.*?Lý do:\s*(.+?)(?:\n|\[|$)/g)];
    if (matches.length === 0) return null;
    
    // Get the last match (most recent request)
    const lastMatch = matches[matches.length - 1];
    return lastMatch[1] ? lastMatch[1].trim() : null;
  };

  const canReschedule = (appointment) => {
    const status = appointment.status || appointment.AppointmentStatus;
    const paymentStatus =
      appointment.paymentStatus || appointment.PaymentStatus;
    
    // Không cho phép reschedule nếu đã hoàn thành hoặc đã hủy
    if (status === "Completed" || status === "Canceled" || status === "Cancelled") return false;
    
    // Không cho phép reschedule nếu đang chờ phê duyệt đổi lịch
    if (status === "PendingReschedule") return false;
    
    // Phải đã thanh toán
    if (paymentStatus !== "Paid") return false;
    
    // Cho phép reschedule khi status là Pending, Confirmed, Scheduled, hoặc Rescheduled (đã đổi lịch nhưng có thể đổi lại)
    if (status !== "Confirmed" && status !== "Scheduled" && status !== "Pending" && status !== "Rescheduled") {
      return false;
    }

    if (appointment.appointmentDate || appointment.AppointmentDate) {
      const appointmentDate = new Date(
        appointment.appointmentDate || appointment.AppointmentDate
      );
      const hoursUntil = (appointmentDate - new Date()) / (1000 * 60 * 60);
      if (hoursUntil < 24) return false;
    }

    return true;
  };

  const handleRequestReschedule = async (
    appointmentId,
    proposedDateTime,
    reason
  ) => {
    setRescheduleLoading(true);
    try {
      await requestReschedule(
        appointmentId,
        proposedDateTime.toISOString(),
        reason
      );
      toast.success("Đã gửi đề xuất đổi lịch");
      setShowRescheduleModal(null);
      await load();
      await loadAppointmentsByViewMode();
    } catch (error) {
      const errorMessage = error.response?.data?.message || 
                          error.response?.data?.error ||
                          error.message || 
                          "Lỗi khi gửi đề xuất đổi lịch";
      toast.error(errorMessage);
      console.error("Reschedule error:", error.response?.data || error);
    } finally {
      setRescheduleLoading(false);
    }
  };

  const handleApproveReschedule = async (appointmentId) => {
    setRescheduleLoading(true);
    try {
      await approveReschedule(appointmentId);
      toast.success("Đã chấp nhận đổi lịch");
      await load();
      await loadAppointmentsByViewMode();
    } catch (error) {
      toast.error(
        error.response?.data?.message || "Lỗi khi chấp nhận đổi lịch"
      );
    } finally {
      setRescheduleLoading(false);
    }
  };

  const handleRejectReschedule = async (appointmentId) => {
    const reason = window.prompt("Lý do từ chối (tùy chọn):");
    setRescheduleLoading(true);
    try {
      await rejectReschedule(appointmentId, reason || null);
      toast.success("Đã từ chối đề xuất đổi lịch");
      await load();
      await loadAppointmentsByViewMode();
    } catch (error) {
      toast.error(error.response?.data?.message || "Lỗi khi từ chối đổi lịch");
    } finally {
      setRescheduleLoading(false);
    }
  };

  const handleCounterReschedule = (appointmentId) => {
    setShowRescheduleModal(appointmentId);
  };

  const StatusBadge = ({ status }) => {
    let cls = "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200";
    if (status === "Scheduled" || status === "Pending")
      cls =
        "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-300";
    if (status === "Confirmed")
      cls = "bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300";
    if (status === "Completed")
      cls =
        "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300";
    if (status === "Cancelled" || status === "Canceled" || status === "NoShow")
      cls = "bg-red-100 text-red-800 dark:bg-red-900/30 dark:text-red-300";
    if (status === "PendingReschedule")
      cls =
        "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-300";
    if (status === "Rescheduled")
      cls = "bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300";
    return (
      <span className={`px-2 py-0.5 rounded text-xs font-medium ${cls}`}>
        {status}
      </span>
    );
  };

  const navigateDate = (direction) => {
    const newDate = new Date(currentDate);
    if (viewMode === "week") {
      newDate.setDate(newDate.getDate() + (direction === "next" ? 7 : -7));
    } else if (viewMode === "month") {
      newDate.setMonth(newDate.getMonth() + (direction === "next" ? 1 : -1));
    }
    setCurrentDate(newDate);
  };

  const renderAppointments = (appointments) => {
    if (!appointments || appointments.length === 0) {
      return (
        <li className="py-3 text-sm text-gray-500 dark:text-gray-300">
          Không có lịch hẹn
        </li>
      );
    }

    return appointments.map((appt, idx) => {
      const appointmentId =
        appt.appointmentId || appt.id || appt.Id || appt.AppointmentId;
      const status = appt.status || appt.AppointmentStatus || "Scheduled";
      const appointmentDate = appt.appointmentDate || appt.AppointmentDate;
      const patientName = appt.patientName || appt.PatientName || "Bệnh nhân";
      const serviceName = appt.serviceName || appt.ServiceName || "Dịch vụ";
      const notes = appt.notes || appt.Notes;

      return (
        <li key={idx} className="py-3 space-y-3">
          <div className="flex items-center justify-between">
            <div className="flex-1">
              <div className="text-sm font-medium text-gray-900 dark:text-white">
                {patientName}
              </div>
              <div className="text-sm text-gray-500 dark:text-gray-300">
                {appointmentDate
                  ? formatDateTime(appointmentDate)
                  : "Chưa có thời gian"}{" "}
                · {serviceName}
              </div>
              {appointmentId && (
                <div className="text-xs text-gray-400 dark:text-gray-500 mt-1">
                  Mã:{" "}
                  {appt.appointmentCode ||
                    appt.AppointmentCode ||
                    `#${appointmentId}`}
                </div>
              )}
            </div>
            <div className="flex items-center space-x-2">
              <StatusBadge status={status} />
              <button
                onClick={() =>
                  navigate(`/doctor/appointments/${appointmentId}`)
                }
                className="inline-flex items-center px-2 py-1 rounded bg-indigo-50 text-indigo-700 hover:bg-indigo-100 text-xs dark:bg-indigo-900/30 dark:text-indigo-300"
              >
                <Eye className="h-3 w-3 mr-1" /> Chi tiết
              </button>
              {/* Button Đề xuất đổi lịch - Hiển thị khi có thể reschedule */}
              {(() => {
                const canRescheduleAppt = canReschedule(appt);
                const status = appt.status || appt.AppointmentStatus || "Scheduled";
                const paymentStatus = appt.paymentStatus || appt.PaymentStatus;
                
                // Hiển thị button nếu canReschedule trả về true
                // Hoặc nếu status hợp lệ và đã paid (fallback logic)
                const shouldShow = canRescheduleAppt || (
                  (status === "Confirmed" || status === "Scheduled" || status === "Pending") &&
                  paymentStatus === "Paid" &&
                  status !== "Completed" &&
                  status !== "Canceled" &&
                  status !== "Cancelled" &&
                  status !== "PendingReschedule" &&
                  status !== "Rescheduled"
                );
                
                return shouldShow ? (
                  <button
                    type="button"
                    onClick={(e) => {
                      e.preventDefault();
                      e.stopPropagation();
                      setShowRescheduleModal(appointmentId);
                    }}
                    className="inline-flex items-center px-2 py-1 rounded bg-yellow-50 text-yellow-700 hover:bg-yellow-100 text-xs dark:bg-yellow-900/30 dark:text-yellow-300 border border-yellow-300 dark:border-yellow-600 cursor-pointer"
                    title="Đề xuất đổi lịch khám cho khách hàng"
                  >
                    <CalendarClock className="h-3 w-3 mr-1" /> Đề xuất đổi lịch
                  </button>
                ) : null;
              })()}
              {status === "Scheduled" || status === "Pending" ? (
                <button
                  onClick={() => onConfirm(appointmentId)}
                  className="inline-flex items-center px-2 py-1 rounded bg-blue-50 text-blue-700 hover:bg-blue-100 text-xs dark:bg-blue-900/30 dark:text-blue-300"
                >
                  <Clock className="h-3 w-3 mr-1" /> Xác nhận
                </button>
              ) : null}
              {status === "Confirmed" && (
                <>
                  <button
                    onClick={() => onComplete(appointmentId)}
                    className="inline-flex items-center px-2 py-1 rounded bg-green-50 text-green-700 hover:bg-green-100 text-xs dark:bg-green-900/30 dark:text-green-300"
                  >
                    <CheckCircle className="h-3 w-3 mr-1" /> Hoàn thành
                  </button>
                  <button
                    onClick={() => onCancel(appointmentId)}
                    className="inline-flex items-center px-2 py-1 rounded bg-red-50 text-red-700 hover:bg-red-100 text-xs dark:bg-red-900/30 dark:text-red-300"
                  >
                    <XCircle className="h-3 w-3 mr-1" /> Hủy
                  </button>
                </>
              )}
            </div>
          </div>

          {status === "PendingReschedule" && (
            <RescheduleNotificationCard
              appointment={appt}
              requestedBy={
                notes?.includes("[Customer]") ? "Customer" : "Doctor"
              }
              proposedDateTime={extractProposedDateTime(notes)}
              reason={extractReason(notes)}
              onApprove={() => handleApproveReschedule(appointmentId)}
              onReject={() => handleRejectReschedule(appointmentId)}
              onCounterPropose={() => handleCounterReschedule(appointmentId)}
              isLoading={rescheduleLoading}
              currentUserRole="Doctor"
            />
          )}
        </li>
      );
    });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
            Lịch làm việc
          </h1>
          <p className="text-gray-600 dark:text-gray-300">
            Quản lý lịch làm việc của bác sĩ
          </p>
        </div>
        <button
          onClick={load}
          className="inline-flex items-center px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700"
        >
          <Calendar className="h-4 w-4 mr-2" /> Làm mới
        </button>
      </div>

      {/* View Mode Selector */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-2">
                  <button
              onClick={() => setViewMode("today")}
              className={`px-4 py-2 rounded-md text-sm font-medium ${
                viewMode === "today"
                  ? "bg-indigo-600 text-white"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-300"
              }`}
            >
              Hôm nay
                  </button>
                  <button
              onClick={() => setViewMode("week")}
              className={`px-4 py-2 rounded-md text-sm font-medium ${
                viewMode === "week"
                  ? "bg-indigo-600 text-white"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-300"
              }`}
            >
              Tuần này
                  </button>
                  <button
              onClick={() => setViewMode("month")}
              className={`px-4 py-2 rounded-md text-sm font-medium ${
                viewMode === "month"
                  ? "bg-indigo-600 text-white"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200 dark:bg-gray-700 dark:text-gray-300"
              }`}
            >
              Tháng này
            </button>
          </div>
          {(viewMode === "week" || viewMode === "month") && (
            <div className="flex items-center space-x-2">
              <button
                onClick={() => navigateDate("prev")}
                className="p-2 rounded-md bg-gray-100 hover:bg-gray-200 dark:bg-gray-700 dark:hover:bg-gray-600"
              >
                <ChevronLeft className="h-4 w-4" />
                  </button>
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300">
                {viewMode === "week"
                  ? `Tuần ${currentDate.toLocaleDateString("vi-VN", {
                      week: "numeric",
                      year: "numeric",
                    })}`
                  : currentDate.toLocaleDateString("vi-VN", {
                      month: "long",
                      year: "numeric",
                    })}
              </span>
                  <button
                onClick={() => navigateDate("next")}
                className="p-2 rounded-md bg-gray-100 hover:bg-gray-200 dark:bg-gray-700 dark:hover:bg-gray-600"
                  >
                <ChevronRight className="h-4 w-4" />
                  </button>
                </div>
          )}
        </div>
      </div>

      {/* Today's Schedule */}
      {viewMode === "today" && (
        <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Lịch hôm nay
          </h3>
          {loading && (
            <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>
          )}
          {!loading && (
            <ul className="divide-y divide-gray-200 dark:divide-gray-700">
              {renderAppointments(todaySchedules)}
          </ul>
        )}
      </div>
      )}

      {/* Week View - Calendar */}
      {viewMode === "week" && (
        <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Lịch tuần
          </h3>
          {loading && (
            <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>
          )}
          {!loading && (
            <WeeklyAppointmentCalendar
              appointments={allAppointments}
              schedules={weekSchedules}
              currentWeekStart={currentDate}
              onAppointmentClick={(appt) => {
                const appointmentId =
                  appt.appointmentId ||
                  appt.id ||
                  appt.Id ||
                  appt.AppointmentId;
                navigate(`/doctor/appointments/${appointmentId}`);
              }}
              onRescheduleClick={(appt) => {
                const appointmentId =
                  appt.appointmentId ||
                  appt.id ||
                  appt.Id ||
                  appt.AppointmentId;
                // Không cần check canReschedule nữa vì button chỉ hiển thị khi đã đáp ứng điều kiện
                // Cho phép mở modal để đề xuất đổi lịch
                setShowRescheduleModal(appointmentId);
              }}
              canReschedule={canReschedule}
            />
          )}
        </div>
      )}

      {/* Month View - List */}
      {viewMode === "month" && (
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Lịch tháng
          </h3>
          {loading && (
            <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>
          )}
          {!loading && (
        <ul className="divide-y divide-gray-200 dark:divide-gray-700">
              {renderAppointments(allAppointments)}
            </ul>
          )}
              </div>
      )}

      {/* Upcoming Appointments */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
          Sắp tới
        </h3>
        <ul className="divide-y divide-gray-200 dark:divide-gray-700">
          {renderAppointments(upcoming)}
        </ul>
      </div>

      {/* Reschedule Request Modal */}
      {showRescheduleModal && (
        <RescheduleRequestModal
          isOpen={!!showRescheduleModal}
          onClose={() => setShowRescheduleModal(null)}
          appointment={[
            ...todaySchedules,
            ...upcoming,
            ...allAppointments,
          ].find(
            (a) =>
              (a.appointmentId || a.id || a.Id || a.AppointmentId) ===
              showRescheduleModal
          )}
          onSubmit={(proposedDateTime, reason) =>
            handleRequestReschedule(
              showRescheduleModal,
              proposedDateTime,
              reason
            )
          }
          isLoading={rescheduleLoading}
          isDoctorRequest={true} // Bác sĩ có thể chọn bất kỳ thời gian nào
        />
      )}
    </div>
  );
};

export default DoctorSchedule;
