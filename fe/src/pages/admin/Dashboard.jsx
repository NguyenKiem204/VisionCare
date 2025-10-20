import React, { useEffect, useMemo, useState } from "react";
import {
  Users,
  Calendar,
  Stethoscope,
  TrendingUp,
  Clock,
  CheckCircle,
  AlertCircle,
} from "lucide-react";
import {
  getAdminSummary,
  getAdminAppointmentSeries,
  getTopServices,
  getDoctorKpis,
  getRecentAppointments,
} from "../../services/adminDashboardAPI";
import DateRangePicker from "../../components/admin/dashboard/DateRangePicker";
import KpiCard from "../../components/admin/dashboard/KpiCard";
import ChartCard from "../../components/admin/dashboard/ChartCard";

const Dashboard = () => {
  const [loading, setLoading] = useState(false);
  const [summary, setSummary] = useState(null);
  const [series, setSeries] = useState([]);
  const [topServices, setTopServices] = useState([]);
  const [doctorKpis, setDoctorKpis] = useState([]);
  const [recentAppointments, setRecentAppointments] = useState([]);
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
        const [s, ts, tops, dk, recent] = await Promise.all([
          getAdminSummary(params),
          getAdminAppointmentSeries({ ...params, bucket: "month" }),
          getTopServices({ ...params, top: 5 }),
          getDoctorKpis(params),
          getRecentAppointments({ limit: 10 }),
        ]);
        setSummary(s);
        setSeries(ts);
        setTopServices(tops);
        setDoctorKpis(dk);
        setRecentAppointments(recent);
      } catch (e) {
        console.error("Dashboard load error", e);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [params]);

  const stats = [
    {
      name: "Tổng lịch hẹn",
      value: summary?.totalAppointments ?? 0,
      icon: Calendar,
    },
    {
      name: "Hoàn thành",
      value: summary?.completedAppointments ?? 0,
      icon: CheckCircle,
    },
    {
      name: "Hủy",
      value: summary?.canceledAppointments ?? 0,
      icon: AlertCircle,
    },
    { name: "Bệnh nhân mới", value: summary?.newPatients ?? 0, icon: Users },
  ];

  // Use real data from API
  const recentBookings = recentAppointments.map((apt) => ({
    id: apt.appointmentId,
    patient: apt.patientName,
    service: apt.serviceName,
    time: new Date(apt.appointmentDate).toLocaleTimeString("vi-VN", {
      hour: "2-digit",
      minute: "2-digit",
    }),
    status: apt.status.toLowerCase(),
  }));

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
            Dashboard
          </h1>
          <p className="text-gray-600 dark:text-gray-300">
            Tổng quan hoạt động phòng khám
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
          title="Lịch hẹn theo thời gian"
          data={series}
          loading={loading}
        />
        <ChartCard
          title="Dịch vụ phổ biến"
          data={topServices.map((s) => ({
            label: s.serviceName,
            count: s.completedCount,
          }))}
          loading={loading}
        />
      </div>

      {/* Top Services */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg border border-gray-200 dark:border-gray-700">
        <div className="px-4 py-5 sm:p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Dịch vụ hàng đầu
          </h3>
          <div className="flow-root">
            <ul className="-my-5 divide-y divide-gray-200 dark:divide-gray-700">
              {(topServices ?? []).map((s) => (
                <li
                  key={s.serviceId}
                  className="py-3 flex items-center justify-between"
                >
                  <span className="text-sm text-gray-900 dark:text-gray-200">
                    {s.serviceName}
                  </span>
                  <span className="text-sm text-gray-600 dark:text-gray-400">
                    {s.completedCount}
                  </span>
                </li>
              ))}
              {(!topServices || topServices.length === 0) && (
                <li className="py-3 text-sm text-gray-500 dark:text-gray-400">
                  Không có dữ liệu
                </li>
              )}
            </ul>
          </div>
        </div>
      </div>

      {/* Doctor KPIs */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg border border-gray-200 dark:border-gray-700">
        <div className="px-4 py-5 sm:p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Hiệu suất bác sĩ
          </h3>
          <div className="flow-root">
            <ul className="-my-5 divide-y divide-gray-200 dark:divide-gray-700">
              {(doctorKpis ?? []).map((d) => (
                <li
                  key={d.doctorId}
                  className="py-3 flex items-center justify-between"
                >
                  <span className="text-sm text-gray-900 dark:text-gray-200">
                    {d.doctorName}
                  </span>
                  <span className="text-sm text-gray-600 dark:text-gray-400">
                    {d.completedCount}
                  </span>
                </li>
              ))}
              {(!doctorKpis || doctorKpis.length === 0) && (
                <li className="py-3 text-sm text-gray-500 dark:text-gray-400">
                  Không có dữ liệu
                </li>
              )}
            </ul>
          </div>
        </div>
      </div>

      {/* Recent Bookings */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg border border-gray-200 dark:border-gray-700">
        <div className="px-4 py-5 sm:p-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
            Lịch hẹn gần đây
          </h3>
          <div className="flow-root">
            <ul className="-my-5 divide-y divide-gray-200 dark:divide-gray-700">
              {recentBookings.map((booking) => (
                <li key={booking.id} className="py-4">
                  <div className="flex items-center space-x-4">
                    <div className="flex-shrink-0">
                      <div className="h-10 w-10 rounded-full bg-gray-200 dark:bg-gray-600 flex items-center justify-center">
                        <Users className="h-5 w-5 text-gray-500 dark:text-gray-300" />
                      </div>
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-gray-900 dark:text-white truncate">
                        {booking.patient}
                      </p>
                      <p className="text-sm text-gray-500 dark:text-gray-400 truncate">
                        {booking.service}
                      </p>
                    </div>
                    <div className="flex items-center space-x-2">
                      <span className="text-sm text-gray-500 dark:text-gray-400">
                        {booking.time}
                      </span>
                      <span
                        className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(
                          booking.status
                        )}`}
                      >
                        {getStatusIcon(booking.status)}
                        <span className="ml-1">
                          {booking.status === "confirmed" && "Xác nhận"}
                          {booking.status === "pending" && "Chờ xác nhận"}
                          {booking.status === "completed" && "Hoàn thành"}
                        </span>
                      </span>
                    </div>
                  </div>
                </li>
              ))}
            </ul>
          </div>
          <div className="mt-6">
            <a
              href="/admin/appointments"
              className="w-full flex justify-center items-center px-4 py-2 border border-gray-300 dark:border-gray-600 shadow-sm text-sm font-medium rounded-md text-gray-700 dark:text-gray-200 bg-white dark:bg-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600 transition"
            >
              Xem tất cả lịch hẹn
            </a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
