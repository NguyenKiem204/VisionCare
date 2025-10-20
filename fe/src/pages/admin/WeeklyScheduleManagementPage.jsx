import React, { useState, useEffect, useCallback } from "react";
import WeeklyScheduleFilter from "../../components/admin/weekly/WeeklyScheduleFilter";
import WeeklyScheduleTable from "../../components/admin/weekly/WeeklyScheduleTable";
import CreateWeeklyScheduleModal from "../../components/admin/weekly/CreateWeeklyScheduleModal";
import EditWeeklyScheduleModal from "../../components/admin/weekly/EditWeeklyScheduleModal";
import ViewWeeklyScheduleModal from "../../components/admin/weekly/ViewWeeklyScheduleModal";
import {
  searchWeeklySchedules,
  deleteWeeklySchedule,
  publishWeeklySchedule,
  archiveWeeklySchedule,
  updateWeeklyScheduleStatus,
} from "../../services/adminWeeklyScheduleAPI";
import toast from "react-hot-toast";

const WeeklyScheduleManagementPage = () => {
  const [schedules, setSchedules] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "desc" });
  const [filter, setFilter] = useState({});
  const [loading, setLoading] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [viewModalOpen, setViewModalOpen] = useState(false);
  const [selectedSchedule, setSelectedSchedule] = useState(null);

  const loadSchedules = useCallback(async () => {
    setLoading(true);
    try {
      const params = {
        ...filter,
        page: pagination.page,
        size: pagination.size,
        sortBy: sort.sortBy,
        sortDir: sort.sortDir,
      };
      Object.keys(params).forEach(
        (k) => (params[k] == null || params[k] === "") && delete params[k]
      );
      console.log("[WeeklySchedule] Gọi searchWeeklySchedules", params);
      const res = await searchWeeklySchedules(params);
      setSchedules(res.data.content || []);
      setPagination((prev) => ({
        ...prev,
        total: res.data.totalElements,
      }));
    } catch {
      setSchedules([]);
    }
    setLoading(false);
  }, [filter, pagination.page, pagination.size, sort]);

  useEffect(() => {
    loadSchedules();
  }, [loadSchedules]);

  const handleCreate = () => setModalOpen(true);
  const handleEdit = (schedule) => {
    setSelectedSchedule(schedule);
    setEditModalOpen(true);
  };
  const handleView = (schedule) => {
    setSelectedSchedule(schedule);
    setViewModalOpen(true);
  };
  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa lịch tuần này?")) {
      try {
        await deleteWeeklySchedule(id);
        toast.success("Xóa lịch tuần thành công!");
        loadSchedules();
      } catch {
        toast.error("Xóa lịch tuần thất bại!");
      }
    }
  };
  const handlePublish = async (id) => {
    await publishWeeklySchedule(id);
    loadSchedules();
  };
  const handleArchive = async (id) => {
    await archiveWeeklySchedule(id);
    loadSchedules();
  };

  const handleChangeStatus = async (id, status) => {
    await updateWeeklyScheduleStatus(id, status);
    loadSchedules();
  };

  return (
    <div className="max-w-6xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý lịch thánh lễ tuần
      </h1>
      <WeeklyScheduleFilter
        filter={filter}
        setFilter={setFilter}
        loading={loading}
        onSearch={loadSchedules}
      />
      <div className="flex justify-end mb-2 gap-2">
        <button
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow transition"
          onClick={handleCreate}
        >
          + Tạo lịch tuần mới
        </button>
        <button
          className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg shadow transition"
          // onClick={handleExportExcel}
        >
          Xuất Excel
        </button>
      </div>
      <WeeklyScheduleTable
        schedules={schedules}
        loading={loading}
        onEdit={handleEdit}
        onView={handleView}
        onDelete={handleDelete}
        onArchive={handleArchive}
        onChangeStatus={handleChangeStatus}
        pagination={pagination}
        setPagination={setPagination}
        sort={sort}
        setSort={setSort}
      />
      <CreateWeeklyScheduleModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        onSuccess={loadSchedules}
      />
      <EditWeeklyScheduleModal
        open={editModalOpen}
        schedule={selectedSchedule}
        onClose={() => setEditModalOpen(false)}
        onSuccess={loadSchedules}
      />
      <ViewWeeklyScheduleModal
        open={viewModalOpen}
        schedule={selectedSchedule}
        onClose={() => setViewModalOpen(false)}
      />
    </div>
  );
};

export default WeeklyScheduleManagementPage;
