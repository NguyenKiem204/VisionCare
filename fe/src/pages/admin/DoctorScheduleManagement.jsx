import React, { useState, useEffect } from "react";
import {
  Calendar,
  Clock,
  User,
  Plus,
  RefreshCw,
  Filter,
  Search,
  Grid,
  List,
  Settings,
  AlertCircle,
  CheckCircle,
  XCircle,
} from "lucide-react";
import { getWeeklySchedulesByDoctor, updateWeeklySchedule, deleteWeeklySchedule } from "../../services/adminWeeklyScheduleAPI";
import { searchDoctors } from "../../services/adminDoctorAPI";
import { generateSchedulesForDoctor } from "../../services/adminScheduleGenerationAPI";
import toast from "react-hot-toast";
import WeeklyScheduleCalendar from "../../components/admin/schedule/WeeklyScheduleCalendar";
import WeeklyScheduleTable from "../../components/admin/schedule/WeeklyScheduleTable";
import CreateWeeklyScheduleModal from "../../components/admin/schedule/CreateWeeklyScheduleModal";
import EditWeeklyScheduleModal from "../../components/admin/schedule/EditWeeklyScheduleModal";
import ScheduleGenerationModal from "../../components/admin/schedule/ScheduleGenerationModal";

const DoctorScheduleManagement = () => {
  const [viewMode, setViewMode] = useState("calendar"); // 'calendar' or 'table'
  const [selectedDoctor, setSelectedDoctor] = useState(null);
  const [doctors, setDoctors] = useState([]);
  const [weeklySchedules, setWeeklySchedules] = useState([]);
  const [loading, setLoading] = useState(false);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [generateModalOpen, setGenerateModalOpen] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [selectedSchedule, setSelectedSchedule] = useState(null);

  // Load doctors
  useEffect(() => {
    const loadDoctors = async () => {
      try {
        const response = await searchDoctors({ page: 1, pageSize: 100 });
        const doctorsList = response.data?.data || response.data?.items || [];
        setDoctors(doctorsList);
        if (doctorsList.length > 0 && !selectedDoctor) {
          setSelectedDoctor(doctorsList[0].doctorId || doctorsList[0].id);
        }
      } catch (error) {
        console.error("Error loading doctors:", error);
      }
    };
    loadDoctors();
  }, []);

  // Load weekly schedules when doctor selected
  useEffect(() => {
    if (selectedDoctor) {
      loadWeeklySchedules();
    }
  }, [selectedDoctor]);

  const loadWeeklySchedules = async () => {
    if (!selectedDoctor) return;
    setLoading(true);
    try {
      const response = await getWeeklySchedulesByDoctor(selectedDoctor);
      const schedules = response.data?.data || [];
      setWeeklySchedules(schedules);
    } catch (error) {
      console.error("Error loading weekly schedules:", error);
      toast.error("Không thể tải lịch làm việc");
    } finally {
      setLoading(false);
    }
  };

  const handleGenerateSchedules = async (daysAhead) => {
    if (!selectedDoctor) {
      toast.error("Vui lòng chọn bác sĩ");
      return;
    }
    try {
      const response = await generateSchedulesForDoctor(selectedDoctor, daysAhead);
      const count = response.data?.data || 0;
      toast.success(`Đã tạo ${count} lịch hẹn từ template`);
      setGenerateModalOpen(false);
    } catch (error) {
      console.error("Error generating schedules:", error);
      toast.error("Không thể tạo lịch tự động");
    }
  };

  const handleEditSchedule = (schedule) => {
    setSelectedSchedule(schedule);
    setEditModalOpen(true);
  };

  const handleDeleteSchedule = async (id) => {
    if (!window.confirm("Bạn có chắc muốn xóa lịch này?")) {
      return;
    }
    try {
      await deleteWeeklySchedule(id);
      toast.success("Đã xóa lịch làm việc");
      loadWeeklySchedules();
    } catch (error) {
      console.error("Error deleting schedule:", error);
      toast.error("Không thể xóa lịch làm việc");
    }
  };

  const selectedDoctorInfo = doctors.find(
    (d) => (d.doctorId || d.id) === selectedDoctor
  );

  return (
    <div className="space-y-6 p-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white flex items-center gap-3">
            <Calendar className="h-8 w-8 text-blue-600" />
            Quản lý lịch làm việc bác sĩ
          </h1>
          <p className="text-gray-600 dark:text-gray-400 mt-2">
            Thiết lập và quản lý lịch làm việc theo tuần cho bác sĩ
          </p>
        </div>
      </div>

      {/* Doctor Selector */}
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
        <div className="flex items-center gap-4 flex-wrap">
          <label className="text-sm font-semibold text-gray-700 dark:text-gray-300 flex items-center gap-2">
            <User className="h-4 w-4" />
            Chọn bác sĩ:
          </label>
          <select
            value={selectedDoctor || ""}
            onChange={(e) => setSelectedDoctor(Number(e.target.value))}
            className="flex-1 min-w-[200px] px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
          >
            <option value="">-- Chọn bác sĩ --</option>
            {doctors.map((doctor) => (
              <option
                key={doctor.doctorId || doctor.id}
                value={doctor.doctorId || doctor.id}
              >
                {doctor.doctorName || doctor.fullName || doctor.name} -{" "}
                {doctor.specializationName || "Không có chuyên khoa"}
              </option>
            ))}
          </select>
          {selectedDoctorInfo && (
            <div className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-400">
              <span className="font-medium">Chuyên khoa:</span>
              <span className="px-2 py-1 bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-200 rounded">
                {selectedDoctorInfo.specializationName || "N/A"}
              </span>
            </div>
          )}
        </div>
      </div>

      {/* Action Bar */}
      {selectedDoctor && (
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700 p-4">
          <div className="flex items-center justify-between flex-wrap gap-4">
            <div className="flex items-center gap-2">
              <button
                onClick={() => setViewMode("calendar")}
                className={`px-4 py-2 rounded-lg font-medium transition-all flex items-center gap-2 ${
                  viewMode === "calendar"
                    ? "bg-blue-600 text-white shadow-md"
                    : "bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-600"
                }`}
              >
                <Grid className="h-4 w-4" />
                Lịch tuần
              </button>
              <button
                onClick={() => setViewMode("table")}
                className={`px-4 py-2 rounded-lg font-medium transition-all flex items-center gap-2 ${
                  viewMode === "table"
                    ? "bg-blue-600 text-white shadow-md"
                    : "bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-600"
                }`}
              >
                <List className="h-4 w-4" />
                Danh sách
              </button>
            </div>

            <div className="flex items-center gap-2">
              <button
                onClick={() => setGenerateModalOpen(true)}
                className="px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded-lg font-medium transition-all flex items-center gap-2 shadow-sm"
              >
                <RefreshCw className="h-4 w-4" />
                Tạo lịch tự động
              </button>
              <button
                onClick={() => setCreateModalOpen(true)}
                className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-medium transition-all flex items-center gap-2 shadow-sm"
              >
                <Plus className="h-4 w-4" />
                Thêm lịch mới
              </button>
              <button
                onClick={loadWeeklySchedules}
                disabled={loading}
                className="px-4 py-2 bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 text-gray-700 dark:text-gray-300 rounded-lg font-medium transition-all flex items-center gap-2"
              >
                <RefreshCw
                  className={`h-4 w-4 ${loading ? "animate-spin" : ""}`}
                />
                Làm mới
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Content */}
      {selectedDoctor ? (
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700">
          {viewMode === "calendar" ? (
            <WeeklyScheduleCalendar
              schedules={weeklySchedules}
              loading={loading}
              onRefresh={loadWeeklySchedules}
            />
          ) : (
            <WeeklyScheduleTable
              schedules={weeklySchedules}
              loading={loading}
              onRefresh={loadWeeklySchedules}
              onEdit={handleEditSchedule}
              onDelete={handleDeleteSchedule}
            />
          )}
        </div>
      ) : (
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700 p-12 text-center">
          <Calendar className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <p className="text-gray-500 dark:text-gray-400 text-lg">
            Vui lòng chọn bác sĩ để xem và quản lý lịch làm việc
          </p>
        </div>
      )}

      {/* Modals */}
      <CreateWeeklyScheduleModal
        open={createModalOpen}
        onClose={() => setCreateModalOpen(false)}
        doctorId={selectedDoctor}
        onSuccess={loadWeeklySchedules}
      />

      <ScheduleGenerationModal
        open={generateModalOpen}
        onClose={() => setGenerateModalOpen(false)}
        doctorId={selectedDoctor}
        onGenerate={handleGenerateSchedules}
      />

      <EditWeeklyScheduleModal
        open={editModalOpen}
        onClose={() => {
          setEditModalOpen(false);
          setSelectedSchedule(null);
        }}
        schedule={selectedSchedule}
        onSuccess={loadWeeklySchedules}
      />
    </div>
  );
};

export default DoctorScheduleManagement;

