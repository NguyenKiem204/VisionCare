import React, { useState, useEffect } from "react";
import { Calendar, Plus, RefreshCw, User, Search, Grid, List } from "lucide-react";
import {
  fetchDoctorSchedules,
  getDoctorSchedulesByDoctorId,
  createDoctorSchedule,
  deleteDoctorSchedule,
} from "../../services/adminDoctorScheduleAPI";
import { searchDoctors } from "../../services/adminDoctorAPI";
import toast from "react-hot-toast";
import CreateDoctorScheduleModal from "../../components/admin/doctorSchedule/CreateDoctorScheduleModal";
import WeeklyDoctorScheduleView from "../../components/admin/doctorSchedule/WeeklyDoctorScheduleView";
import { generateSchedulesForDoctor, generateSchedulesForAll } from "../../services/adminScheduleGenerationAPI";
import { getActiveDoctorSchedulesByDoctorId } from "../../services/adminDoctorScheduleAPI";
import { searchSchedules } from "../../services/adminSchedulingAPI";
import GeneratedSchedulesPreview from "../../components/admin/doctorSchedule/GeneratedSchedulesPreview";
import ScheduleGenerationModal from "../../components/admin/schedule/ScheduleGenerationModal";

const DoctorSchedulesManagementPage = () => {
  const [doctorSchedules, setDoctorSchedules] = useState([]);
  const [doctors, setDoctors] = useState([]);
  const [selectedDoctor, setSelectedDoctor] = useState(null);
  const [loading, setLoading] = useState(false);
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [generateModalOpen, setGenerateModalOpen] = useState(false);
  const [searchKeyword, setSearchKeyword] = useState("");
  const [viewMode, setViewMode] = useState("list"); // 'list' | 'week'
  const [previewOpen, setPreviewOpen] = useState(false);
  const [previewItems, setPreviewItems] = useState([]);
  const [previewShifts, setPreviewShifts] = useState([]);

  // If switching to all doctors, force list view (weekly grid is per-doctor only)
  useEffect(() => {
    if (!selectedDoctor && viewMode === "week") {
      setViewMode("list");
    }
  }, [selectedDoctor, viewMode]);

  useEffect(() => {
    loadDoctors();
  }, []);

  useEffect(() => {
    if (selectedDoctor) {
      loadDoctorSchedules();
    } else {
      loadAllDoctorSchedules();
    }
  }, [selectedDoctor]);

  const loadDoctors = async () => {
    try {
      const res = await searchDoctors({ page: 1, pageSize: 100 });
      setDoctors(res?.data?.data || res?.data?.items || []);
    } catch {
      // Silently fail - doctors list is not critical
    }
  };

  const loadAllDoctorSchedules = async () => {
    setLoading(true);
    try {
      const res = await fetchDoctorSchedules();
      setDoctorSchedules(res?.data?.data || res?.data || []);
    } catch {
      toast.error("Không thể tải lịch làm việc");
    } finally {
      setLoading(false);
    }
  };

  const loadDoctorSchedules = async () => {
    if (!selectedDoctor) return;
    setLoading(true);
    try {
      const res = await getDoctorSchedulesByDoctorId(selectedDoctor);
      setDoctorSchedules(res?.data?.data || res?.data || []);
    } catch {
      toast.error("Không thể tải lịch làm việc");
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = async (data) => {
    try {
      if (Array.isArray(data)) {
        await Promise.all(data.map((item) => createDoctorSchedule(item)));
        toast.success(`Đã tạo ${data.length} lịch làm việc định kỳ`);
      } else {
        await createDoctorSchedule(data);
        toast.success("Đã tạo lịch làm việc định kỳ");
      }
      setCreateModalOpen(false);
      if (selectedDoctor) {
        loadDoctorSchedules();
      } else {
        loadAllDoctorSchedules();
      }
    } catch (error) {
      toast.error(error?.response?.data?.message || "Không thể tạo lịch làm việc");
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Bạn có chắc muốn xóa lịch làm việc này?")) return;
    try {
      await deleteDoctorSchedule(id);
      toast.success("Đã xóa lịch làm việc");
      if (selectedDoctor) {
        loadDoctorSchedules();
      } else {
        loadAllDoctorSchedules();
      }
    } catch {
      toast.error("Không thể xóa lịch làm việc");
    }
  };

  const handleGenerateSchedules = async (daysAhead) => {
    try {
      if (selectedDoctor) {
        const active = await getActiveDoctorSchedulesByDoctorId(selectedDoctor);
        const items = active?.data?.data || active?.data || [];
        if (!items.length) {
          toast.error("Bác sĩ chưa có lịch định kỳ nào đang hoạt động");
          return;
        }
        const res = await generateSchedulesForDoctor(selectedDoctor, daysAhead);
        const count = res?.data?.data ?? 0;
        
        if (count === 0) {
          const message = res?.data?.message || "Không có lịch mới nào được tạo (có thể đã tồn tại)";
          toast(message, { icon: "ℹ️", duration: 5000 });
        } else {
          toast.success(`Đã tạo ${count} lịch hẹn từ lịch định kỳ`);
        }
        
        const today = new Date();
        const fromDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 1).toISOString().substring(0,10);
        const toDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() + daysAhead).toISOString().substring(0,10);
        const searchRes = await searchSchedules({ doctorId: selectedDoctor, fromDate, toDate, page: 1, pageSize: 200 });
        const previewListDoctor = searchRes?.data?.data || searchRes?.data || [];
        setPreviewItems(previewListDoctor);
        
        const activeRecur = await getActiveDoctorSchedulesByDoctorId(selectedDoctor);
        const recurItems = activeRecur?.data?.data || activeRecur?.data || [];
        const shifts = recurItems.map(r => ({ shiftName: r.shiftName, startTime: r.startTime, endTime: r.endTime }));
        setPreviewOpen(true);
        setPreviewShifts(shifts);
      } else {
        const res = await generateSchedulesForAll(daysAhead);
        const count = res?.data?.data ?? 0;
        
        if (count === 0) {
          const message = res?.data?.message || "Không có lịch mới nào được tạo (có thể đã tồn tại)";
          toast(message, { icon: "ℹ️", duration: 5000 });
        } else {
          toast.success(`Đã tạo ${count} lịch hẹn cho tất cả bác sĩ`);
        }
      }
      setGenerateModalOpen(false);
    } catch (error) {
      const msg = error?.response?.data?.message || error?.response?.data?.Message || error?.message || "Không thể tạo lịch tự động";
      toast.error(msg);
    }
  };

  const handlePreviewNext30 = async () => {
    if (!selectedDoctor) {
      toast.error("Vui lòng chọn bác sĩ");
      return;
    }
    try {
      const today = new Date();
      const fromDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 1).toISOString().substring(0,10);
      const toDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() + 30).toISOString().substring(0,10);
      const res = await searchSchedules({ doctorId: selectedDoctor, fromDate, toDate, page: 1, pageSize: 300 });
      const items = res?.data?.data || res?.data || [];
      setPreviewItems(items);
      const activeRecur = await getActiveDoctorSchedulesByDoctorId(selectedDoctor);
      const recurItems = activeRecur?.data?.data || activeRecur?.data || [];
      const shifts = recurItems.map(r => ({ shiftName: r.shiftName, startTime: r.startTime, endTime: r.endTime }));
      setPreviewShifts(shifts);
      setPreviewOpen(true);
  } catch {
      toast.error("Không thể tải lịch 30 ngày");
    }
  };

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

  const filteredSchedules = doctorSchedules.filter((schedule) => {
    if (!searchKeyword) return true;
    const keyword = searchKeyword.toLowerCase();
    return (
      schedule.doctorName?.toLowerCase().includes(keyword) ||
      schedule.shiftName?.toLowerCase().includes(keyword) ||
      schedule.roomName?.toLowerCase().includes(keyword)
    );
  });

  return (
    <div className="max-w-7xl mx-auto px-4 py-6">
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-lg p-6">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-3">
              <Calendar className="h-6 w-6" />
              Quản lý lịch làm việc định kỳ
            </h1>
            <p className="text-gray-600 dark:text-gray-300 mt-1">
              Đây là lịch ĐỊNH KỲ theo ca/chu kỳ (ví dụ: Thứ 2, 8:00–11:00). Mỗi đêm lúc 02:00, hệ thống sẽ DỰA TRÊN các lịch này để sinh ra lịch hẹn CỤ THỂ cho từng ngày chưa có.
            </p>
            <p className="text-gray-600 dark:text-gray-300 text-sm mt-2">
              Trường hợp tác vụ tự động lỗi, bạn có thể bấm “Tạo lịch tự động” để chạy thủ công cho bác sĩ đang chọn hoặc cho tất cả.
            </p>
          </div>
          <div className="flex gap-2">
            <button
              onClick={() => setViewMode("list")}
              className={`px-3 py-2 rounded-lg flex items-center gap-2 ${viewMode === "list" ? "bg-blue-600 text-white" : "bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300"}`}
            >
              <List size={16} /> Danh sách
            </button>
            <button
              onClick={() => selectedDoctor && setViewMode("week")}
              disabled={!selectedDoctor}
              title={!selectedDoctor ? "Lịch tuần chỉ hỗ trợ khi lọc theo 1 bác sĩ" : ""}
              className={`px-3 py-2 rounded-lg flex items-center gap-2 ${viewMode === "week" ? "bg-blue-600 text-white" : "bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300"} disabled:opacity-50 disabled:cursor-not-allowed`}
            >
              <Grid size={16} /> Lịch tuần
            </button>
          </div>
        </div>

        {/* Filters */}
        <div className="flex gap-4 mb-6 flex-wrap">
          <div className="flex-1 min-w-[200px]">
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Lọc theo bác sĩ:
            </label>
            <select
              value={selectedDoctor || ""}
              onChange={(e) => setSelectedDoctor(e.target.value ? Number(e.target.value) : null)}
              className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-lg dark:bg-gray-900 dark:text-white"
            >
              <option value="">Tất cả bác sĩ</option>
              {doctors.map((d) => (
                <option key={d.doctorId || d.id} value={d.doctorId || d.id}>
                  {d.doctorName || d.fullName} - {d.specializationName || ""}
                </option>
              ))}
            </select>
          </div>

          <div className="flex-1 min-w-[200px]">
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Tìm kiếm:
            </label>
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
              <input
                type="text"
                value={searchKeyword}
                onChange={(e) => setSearchKeyword(e.target.value)}
                placeholder="Tìm theo tên bác sĩ, ca làm việc..."
                className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg dark:bg-gray-900 dark:text-white"
              />
            </div>
          </div>

          <div className="flex items-end gap-2">
            <button
              onClick={() => setCreateModalOpen(true)}
              className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg flex items-center gap-2"
            >
              <Plus size={18} />
              Tạo lịch mới
            </button>
            <button
              onClick={handlePreviewNext30}
              disabled={!selectedDoctor}
              className="px-4 py-2 bg-indigo-600 hover:bg-indigo-700 disabled:opacity-50 disabled:cursor-not-allowed text-white rounded-lg flex items-center gap-2"
            >
              Xem 30 ngày sắp tới
            </button>
            <button
              onClick={() => setGenerateModalOpen(true)}
              className="px-4 py-2 bg-emerald-600 hover:bg-emerald-700 text-white rounded-lg flex items-center gap-2"
            >
              <RefreshCw size={18} />
              Tạo lịch tự động
            </button>
            <button
              onClick={selectedDoctor ? loadDoctorSchedules : loadAllDoctorSchedules}
              disabled={loading}
              className="px-4 py-2 bg-gray-100 dark:bg-gray-700 hover:bg-gray-200 dark:hover:bg-gray-600 text-gray-700 dark:text-gray-300 rounded-lg flex items-center gap-2"
            >
              <RefreshCw size={18} className={loading ? "animate-spin" : ""} />
              Làm mới
            </button>
          </div>
        </div>

        {/* Content */}
        {loading && (
          <div className="text-center py-8 text-gray-500 dark:text-gray-400">Đang tải...</div>
        )}

        {!loading && filteredSchedules.length === 0 && (
          <div className="text-center py-8 text-gray-500 dark:text-gray-400">
            {selectedDoctor ? "Chưa có lịch làm việc định kỳ cho bác sĩ này" : "Chưa có lịch làm việc định kỳ nào"}
          </div>
        )}

        {!loading && filteredSchedules.length > 0 && (
          <>
            {viewMode === "list" ? (
              <div className="grid gap-4">
                {filteredSchedules.map((schedule) => (
                  <div
                    key={schedule.id}
                    className="border border-gray-200 dark:border-gray-700 rounded-lg p-4 hover:shadow-md transition"
                  >
                    <div className="flex justify-between items-start">
                      <div className="flex-1">
                        <div className="flex items-center gap-3 mb-2">
                          <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
                            {schedule.doctorName || "Bác sĩ"} - {schedule.shiftName || "Ca làm việc"}
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

                        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mt-3 text-sm text-gray-600 dark:text-gray-300">
                          <div>
                            <span className="font-medium">Thời gian:</span>{" "}
                            {formatTime(schedule.startTime)} - {formatTime(schedule.endTime)}
                          </div>
                          <div>
                            <span className="font-medium">Chu kỳ:</span>{" "}
                            {getRecurrenceText(schedule.recurrenceRule, schedule.dayOfWeek)}
                          </div>
                          <div>
                            <span className="font-medium">Phòng:</span>{" "}
                            {schedule.roomName || "Không có"}
                          </div>
                          <div>
                            <span className="font-medium">Thiết bị:</span>{" "}
                            {schedule.equipmentName || "Không có"}
                          </div>
                        </div>

                        <div className="mt-3 text-sm text-gray-500 dark:text-gray-400">
                          <span className="font-medium">Thời gian hiệu lực:</span> {formatDate(schedule.startDate)} - {formatDate(schedule.endDate)}
                        </div>
                      </div>

                      <button
                        onClick={() => handleDelete(schedule.id)}
                        className="ml-4 px-3 py-1 bg-red-100 dark:bg-red-900 text-red-700 dark:text-red-300 rounded hover:bg-red-200 dark:hover:bg-red-800 text-sm"
                      >
                        Xóa
                      </button>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <WeeklyDoctorScheduleView schedules={filteredSchedules} />
            )}
          </>
        )}
      </div>

      <CreateDoctorScheduleModal
        open={createModalOpen}
        onClose={() => setCreateModalOpen(false)}
        onSave={handleCreate}
        doctorId={selectedDoctor}
      />

      <ScheduleGenerationModal
        open={generateModalOpen}
        onClose={() => setGenerateModalOpen(false)}
        doctorId={selectedDoctor}
        onGenerate={handleGenerateSchedules}
      />

      <GeneratedSchedulesPreview
        open={previewOpen}
        onClose={() => setPreviewOpen(false)}
        items={previewItems}
        shifts={previewShifts}
        title="Lịch cụ thể (xem nhanh)"
      />
    </div>
  );
};

export default DoctorSchedulesManagementPage;

