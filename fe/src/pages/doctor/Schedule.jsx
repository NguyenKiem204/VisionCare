import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getSchedules, getUpcomingAppointments, confirmMyAppointment, completeMyAppointment, cancelMyAppointment } from "../../services/doctorMeAPI";
import { Calendar, CheckCircle, XCircle, Clock, Eye } from "lucide-react";

const DoctorSchedule = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [todaySchedules, setTodaySchedules] = useState([]);
  const [upcoming, setUpcoming] = useState([]);
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
    } catch (e) {
      console.error("Load schedules error", e);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

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

  const StatusBadge = ({ status }) => {
    let cls = "bg-gray-100 text-gray-800";
    if (status === "Scheduled" || status === "Pending") cls = "bg-yellow-100 text-yellow-800";
    if (status === "Confirmed") cls = "bg-blue-100 text-blue-800";
    if (status === "Completed") cls = "bg-green-100 text-green-800";
    if (status === "Canceled" || status === "NoShow") cls = "bg-red-100 text-red-800";
    return <span className={`px-2 py-0.5 rounded text-xs font-medium ${cls}`}>{status}</span>;
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Lịch làm việc</h1>
          <p className="text-gray-600 dark:text-gray-300">Quản lý lịch làm việc của bác sĩ</p>
        </div>
        <button
          onClick={load}
          className="inline-flex items-center px-3 py-2 rounded-md bg-indigo-600 text-white hover:bg-indigo-700"
        >
          <Calendar className="h-4 w-4 mr-2" /> Làm mới
        </button>
      </div>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">Lịch hôm nay</h3>
        {loading && <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>}
        {!loading && (
          <ul className="divide-y divide-gray-200 dark:divide-gray-700">
            {(todaySchedules || []).map((s, idx) => (
              <li key={idx} className="py-3 flex items-center justify-between">
                <div>
                  <div className="text-sm font-medium text-gray-900 dark:text-white">
                    {s?.PatientName || "Bệnh nhân"}
                  </div>
                  <div className="text-sm text-gray-500 dark:text-gray-300">
                    {s?.ServiceName || "Dịch vụ"} · {s?.StartTime} - {s?.EndTime}
                  </div>
                </div>
                <div className="flex items-center space-x-3">
                  <StatusBadge status={s?.Status || s?.ScheduleStatus || "Scheduled"} />
                  <button
                    onClick={() => navigate(`/doctor/appointments/${s?.AppointmentId || s?.Id}`)}
                    className="inline-flex items-center px-2 py-1 rounded bg-indigo-50 text-indigo-700 hover:bg-indigo-100 text-xs dark:bg-indigo-900/30 dark:text-indigo-300"
                  >
                    <Eye className="h-3 w-3 mr-1" /> Chi tiết
                  </button>
                  <button
                    onClick={() => onConfirm(s?.AppointmentId || s?.Id)}
                    className="inline-flex items-center px-2 py-1 rounded bg-blue-50 text-blue-700 hover:bg-blue-100 text-xs dark:bg-blue-900/30 dark:text-blue-300"
                  >
                    <Clock className="h-3 w-3 mr-1" /> Xác nhận
                  </button>
                  <button
                    onClick={() => onComplete(s?.AppointmentId || s?.Id)}
                    className="inline-flex items-center px-2 py-1 rounded bg-green-50 text-green-700 hover:bg-green-100 text-xs dark:bg-green-900/30 dark:text-green-300"
                  >
                    <CheckCircle className="h-3 w-3 mr-1" /> Hoàn thành
                  </button>
                  <button
                    onClick={() => onCancel(s?.AppointmentId || s?.Id)}
                    className="inline-flex items-center px-2 py-1 rounded bg-red-50 text-red-700 hover:bg-red-100 text-xs dark:bg-red-900/30 dark:text-red-300"
                  >
                    <XCircle className="h-3 w-3 mr-1" /> Hủy
                  </button>
                </div>
              </li>
            ))}
            {(!todaySchedules || todaySchedules.length === 0) && (
              <li className="py-3 text-sm text-gray-500 dark:text-gray-300">Không có lịch hẹn hôm nay</li>
            )}
          </ul>
        )}
      </div>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">Sắp tới</h3>
        <ul className="divide-y divide-gray-200 dark:divide-gray-700">
          {(upcoming || []).map((s, idx) => (
            <li key={idx} className="py-3 flex items-center justify-between">
              <div>
                <div className="text-sm font-medium text-gray-900 dark:text-white">
                  {s?.PatientName || "Bệnh nhân"}
                </div>
                <div className="text-sm text-gray-500 dark:text-gray-300">
                  {new Date(s?.AppointmentDate || s?.ScheduleDate).toLocaleString("vi-VN")}
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <StatusBadge status={s?.Status || s?.ScheduleStatus || "Scheduled"} />
                <button
                  onClick={() => navigate(`/doctor/appointments/${s?.AppointmentId || s?.Id}`)}
                  className="inline-flex items-center px-2 py-1 rounded bg-indigo-50 text-indigo-700 hover:bg-indigo-100 text-xs dark:bg-indigo-900/30 dark:text-indigo-300"
                >
                  <Eye className="h-3 w-3 mr-1" /> Chi tiết
                </button>
              </div>
            </li>
          ))}
          {(!upcoming || upcoming.length === 0) && (
            <li className="py-3 text-sm text-gray-500 dark:text-gray-300">Không có lịch sắp tới</li>
          )}
        </ul>
      </div>
    </div>
  );
};

export default DoctorSchedule;
