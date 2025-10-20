import React, { useEffect, useMemo, useState } from "react";
import { Users, Calendar, CheckCircle, Clock, AlertCircle } from "lucide-react";
import {
  getDoctorSummary,
  getDoctorAppointmentSeries,
  getUpcomingAppointments,
} from "../../services/doctorDashboardAPI";
import DateRangePicker from "../../components/admin/dashboard/DateRangePicker";
import KpiCard from "../../components/admin/dashboard/KpiCard";
import ChartCard from "../../components/admin/dashboard/ChartCard";

const DoctorDashboard = () => {
  const [loading, setLoading] = useState(false);
  const [summary, setSummary] = useState(null);
  const [series, setSeries] = useState([]);
  const [upcomingAppointments, setUpcomingAppointments] = useState([]);
  const [range, setRange] = useState({
    from: new Date(new Date().getFullYear(), 0, 1),
    to: new Date(),
  });

  const presets = [
    { label: "Hôm nay", value: "today", active: false },
    { label: "7 ngày", value: "7d", active: false },
    { label: "30 ngày", value: "30d", active: false },
    { label: "Năm nay", value: "ytd", active: true },
  ];

  const params = useMemo(
    () => ({
      from: range.from.toISOString(),
      to: range.to.toISOString(),
    }),
    [range]
  );

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      try {
        const [s, ts, upcoming] = await Promise.all([
          getDoctorSummary(params),
          getDoctorAppointmentSeries({ ...params, bucket: "month" }),
          getUpcomingAppointments({ ...params, limit: 10 }),
        ]);
        setSummary(s);
        setSeries(ts);
        setUpcomingAppointments(upcoming);
      } catch (e) {
        console.error("Doctor Dashboard load error", e);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [params]);

  const stats = [
    {
      name: "Lịch hẹn hôm nay",
      value: summary?.todayAppointments ?? 0,
      icon: Calendar,
    },
    {
      name: "Hoàn thành tuần này",
      value: summary?.completedThisWeek ?? 0,
      icon: CheckCircle,
    },
    {
      name: "Tỷ lệ hủy",
      value: `${summary?.cancelRate ?? 0}%`,
      icon: AlertCircle,
    },
    {
      name: "Bệnh nhân mới",
      value: summary?.newPatients ?? 0,
      icon: Users,
    },
  ];

  const getStatusColor = (status) => {
    switch (status) {
      case "confirmed":
        return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200";
      case "pending":
        return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200";
      case "completed":
        return "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200";
      default:
        return "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200";
    }
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case "confirmed":
        return <CheckCircle className="h-4 w-4" />;
      case "pending":
        return <Clock className="h-4 w-4" />;
      case "completed":
        return <CheckCircle className="h-4 w-4" />;
      default:
        return <AlertCircle className="h-4 w-4" />;
    }
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
            Dashboard Bác sĩ
          </h1>
          <p className="text-gray-600 dark:text-gray-300">
            Tổng quan lịch làm việc và hiệu suất
          </p>
        </div>
        <DateRangePicker value={range} onChange={setRange} presets={presets} />
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        {stats.map((stat) => (
          <KpiCard
            key={stat.name}
            title={stat.name}
            value={stat.value}
            icon={stat.icon}
            loading={loading}
          />
        ))}
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <ChartCard
          title="Lịch hẹn của tôi theo thời gian"
          data={series}
          loading={loading}
        />
        <div className="bg-white dark:bg-gray-800 shadow rounded-lg border border-gray-200 dark:border-gray-700 p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Lịch hẹn sắp tới
          </h3>
          <div className="flow-root">
            <ul className="-my-5 divide-y divide-gray-200 dark:divide-gray-700">
              {(upcomingAppointments ?? []).map((appointment) => (
                <li key={appointment.appointmentId} className="py-4">
                  <div className="flex items-center space-x-4">
                    <div className="flex-shrink-0">
                      <div className="h-10 w-10 rounded-full bg-gray-200 dark:bg-gray-600 flex items-center justify-center">
                        <Users className="h-5 w-5 text-gray-500 dark:text-gray-300" />
                      </div>
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-gray-900 dark:text-white truncate">
                        {appointment.patientName ||
                          `Bệnh nhân #${appointment.patientId}`}
                      </p>
                      <p className="text-sm text-gray-500 dark:text-gray-400 truncate">
                        {appointment.serviceName || "Dịch vụ"}
                      </p>
                    </div>
                    <div className="flex items-center space-x-2">
                      <span className="text-sm text-gray-500 dark:text-gray-400">
                        {new Date(appointment.appointmentDate).toLocaleString(
                          "vi-VN"
                        )}
                      </span>
                      <span
                        className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(
                          appointment.appointmentStatus
                        )}`}
                      >
                        {getStatusIcon(appointment.appointmentStatus)}
                        <span className="ml-1">
                          {appointment.appointmentStatus === "Scheduled" &&
                            "Đã lên lịch"}
                          {appointment.appointmentStatus === "Confirmed" &&
                            "Xác nhận"}
                          {appointment.appointmentStatus === "Completed" &&
                            "Hoàn thành"}
                          {appointment.appointmentStatus === "Canceled" &&
                            "Hủy"}
                        </span>
                      </span>
                    </div>
                  </div>
                </li>
              ))}
              {(!upcomingAppointments || upcomingAppointments.length === 0) && (
                <li className="py-3 text-sm text-gray-500 dark:text-gray-400">
                  Không có lịch hẹn sắp tới
                </li>
              )}
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DoctorDashboard;
