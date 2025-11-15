import React, { useEffect, useState, useCallback } from "react";
import { Plus } from "lucide-react";
import {
  fetchAppointments,
  searchAppointments,
  createAppointment,
  updateAppointment,
  deleteAppointment,
  confirmAppointment,
  cancelAppointment,
  completeAppointment,
  rescheduleAppointment,
  approveReschedule,
  rejectReschedule,
} from "../../services/adminAppointmentAPI";
import RescheduleNotificationCard from "../../components/common/RescheduleNotificationCard";
import { useAuth } from "../../contexts/AuthContext";
import toast from "react-hot-toast";
import AppointmentTable from "../../components/admin/appointments/AppointmentTable";
import AppointmentSearchBar from "../../components/admin/appointments/AppointmentSearchBar";
import AppointmentModal from "../../components/admin/appointments/AppointmentModal";

const sortFields = [
  { value: "id", label: "ID" },
  { value: "appointmentDate", label: "Ngày giờ" },
  { value: "appointmentStatus", label: "Trạng thái" },
  { value: "patientId", label: "Bệnh nhân" },
  { value: "doctorId", label: "Bác sĩ" },
];

const pageSizes = [5, 10, 20, 50, 100];

const AppointmentsManagementPage = () => {
  const { user } = useAuth();
  const [appointments, setAppointments] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "asc" });
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("");
  const [dateRangeFilter, setDateRangeFilter] = useState({
    startDate: "",
    endDate: "",
  });
  const [loading, setLoading] = useState(false);
  const [selectedAppointment, setSelectedAppointment] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);

  const totalPages = Math.ceil(pagination.total / pagination.size);

  const loadAppointments = useCallback(async () => {
    setLoading(true);
    try {
      // Always use search API for consistency and pagination support
      const res = await searchAppointments({
        keyword: search || "",
        status: statusFilter || null,
        startDate: dateRangeFilter.startDate || null,
        endDate: dateRangeFilter.endDate || null,
        page: pagination.page + 1, // Backend uses 1-based pagination
        pageSize: pagination.size,
        sortBy: sort.sortBy,
        desc: sort.sortDir === "desc",
      });

      // Backend returns PagedResponse structure
      if (res.data.success) {
        setAppointments(res.data.items || []);
        setPagination((prev) => ({
          ...prev,
          total: res.data.totalCount || 0,
        }));
      } else {
        console.warn("No appointments data found:", res);
        setAppointments([]);
        setPagination((prev) => ({ ...prev, total: 0 }));
      }
    } catch (error) {
      console.error("Failed to load appointments:", error);
      setAppointments([]);
      setPagination((prev) => ({ ...prev, total: 0 }));
    }
    setLoading(false);
  }, [
    pagination.page,
    pagination.size,
    sort,
    search,
    statusFilter,
    dateRangeFilter,
  ]);

  useEffect(() => {
    loadAppointments();
  }, [loadAppointments]);

  const handleSave = async (data) => {
    try {
      if (selectedAppointment) {
        await updateAppointment(selectedAppointment.id, data);
      } else {
        await createAppointment(data);
      }
      setModalOpen(false);
      loadAppointments();
    } catch (error) {
      console.error("Failed to save appointment:", error);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa lịch hẹn này?")) {
      try {
        await deleteAppointment(id);
        loadAppointments();
      } catch (error) {
        console.error("Failed to delete appointment:", error);
      }
    }
  };

  const handleConfirm = async (id) => {
    try {
      await confirmAppointment(id);
      loadAppointments();
    } catch (error) {
      console.error("Failed to confirm appointment:", error);
    }
  };

  const handleCancel = async (id) => {
    if (window.confirm("Bạn có chắc muốn hủy lịch hẹn này?")) {
      try {
        await cancelAppointment(id);
        loadAppointments();
      } catch (error) {
        console.error("Failed to cancel appointment:", error);
      }
    }
  };

  const handleComplete = async (id) => {
    try {
      await completeAppointment(id);
      loadAppointments();
    } catch (error) {
      console.error("Failed to complete appointment:", error);
    }
  };

  const handleReschedule = async (id) => {
    const newDateTime = prompt(
      "Nhập ngày giờ mới (YYYY-MM-DDTHH:MM):",
      new Date().toISOString().slice(0, 16)
    );
    if (newDateTime) {
      try {
        await rescheduleAppointment(id, newDateTime);
        loadAppointments();
      } catch (error) {
        console.error("Failed to reschedule appointment:", error);
      }
    }
  };

  const handleApproveReschedule = async (id) => {
    try {
      await approveReschedule(id);
      toast.success("Đã chấp nhận đề xuất đổi lịch");
      loadAppointments();
    } catch (error) {
      const errorMessage = error.response?.data?.message || error.response?.data?.error || error.message || "Không thể chấp nhận đề xuất đổi lịch";
      console.error("Approve reschedule error:", error.response?.data || error);
      toast.error(errorMessage);
    }
  };

  const handleRejectReschedule = async (id) => {
    const reason = prompt("Nhập lý do từ chối (tùy chọn):");
    if (reason === null) return; // User cancelled
    
    try {
      await rejectReschedule(id, reason || null);
      toast.success("Đã từ chối đề xuất đổi lịch");
      loadAppointments();
    } catch (error) {
      toast.error(error.response?.data?.message || "Không thể từ chối đề xuất đổi lịch");
    }
  };

  const handleCounterReschedule = async (id) => {
    // TODO: Implement counter reschedule modal
    toast.info("Chức năng đề xuất thời gian khác đang được phát triển");
  };

  // Helper functions to extract data from notes
  const extractProposedDateTime = (notes) => {
    if (!notes) return null;
    // Match ALL reschedule requests and get the LAST one (most recent)
    const matches = [...notes.matchAll(/\[(?:Doctor|Customer|Counter)\]\s*Đề xuất đổi lịch:\s*(\d{2}\/\d{2}\/\d{4} \d{2}:\d{2})/g)];
    if (matches.length === 0) {
      return null;
    }
    
    // Get the last match (most recent request)
    const lastMatch = matches[matches.length - 1];
    const [date, time] = lastMatch[1].split(" ");
    const [day, month, year] = date.split("/");
    const [hours, minutes] = time.split(":");
    
    // Create date in local timezone (not UTC)
    // Use local date components to avoid timezone conversion
    const dateObj = new Date();
    dateObj.setFullYear(parseInt(year), parseInt(month) - 1, parseInt(day));
    dateObj.setHours(parseInt(hours), parseInt(minutes), 0, 0);
    
    return dateObj;
  };

  const extractReason = (notes) => {
    if (!notes) return null;
    // Find the last reschedule request and extract its reason
    // Match pattern: [Customer/Doctor] Đề xuất đổi lịch: ... Lý do: {reason}
    const matches = [...notes.matchAll(/\[(?:Doctor|Customer|Counter)\]\s*Đề xuất đổi lịch:.*?Lý do:\s*(.+?)(?:\n|\[|$)/g)];
    if (matches.length === 0) return null;
    
    // Get the last match (most recent request)
    const lastMatch = matches[matches.length - 1];
    return lastMatch[1] ? lastMatch[1].trim() : null;
  };

  const extractRequestedBy = (notes) => {
    if (!notes) return null;
    // Find the last reschedule request to get the requester
    const matches = [...notes.matchAll(/\[(Customer|Doctor|Counter)\]\s*Đề xuất đổi lịch:/g)];
    if (matches.length === 0) return null;
    
    // Get the last match (most recent request)
    const lastMatch = matches[matches.length - 1];
    return lastMatch[1] || null;
  };

  const handleSortChange = (e) => {
    setSort((prev) => ({ ...prev, sortBy: e.target.value }));
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleSortDirChange = (e) => {
    setSort((prev) => ({ ...prev, sortDir: e.target.value }));
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 0 && newPage < totalPages) {
      setPagination((prev) => ({ ...prev, page: newPage }));
    }
  };

  const handlePageSizeChange = (e) => {
    setPagination((prev) => ({
      ...prev,
      size: Number(e.target.value),
      page: 0,
    }));
  };

  const handleStatusFilter = (status) => {
    setStatusFilter(status);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleDateRangeFilter = (startDate, endDate) => {
    setDateRangeFilter({ startDate, endDate });
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  return (
    <div className="max-w-6xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý lịch hẹn
      </h1>

      <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
        <div className="flex-1 min-w-[200px] flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Tìm kiếm:
          </label>
          <AppointmentSearchBar
            value={search}
            onChange={setSearch}
            onStatusFilter={handleStatusFilter}
            onDateRangeFilter={handleDateRangeFilter}
          />
        </div>

        <div className="flex flex-wrap gap-2 items-end min-w-[320px]">
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1 dark:text-gray-300">
              Sắp xếp:
            </label>
            <select
              className="border border-gray-300 dark:border-gray-600 px-2 py-2 rounded-lg focus:outline-gray-400 min-w-[100px] dark:bg-gray-900 dark:text-white dark:border-gray-700"
              value={sort.sortBy}
              onChange={handleSortChange}
            >
              {sortFields.map((f) => (
                <option key={f.value} value={f.value}>
                  {f.label}
                </option>
              ))}
            </select>
          </div>
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1 dark:text-gray-300">
              Chiều:
            </label>
            <select
              className="border border-gray-300 dark:border-gray-600 px-2 py-2 rounded-lg focus:outline-gray-400 min-w-[80px] dark:bg-gray-900 dark:text-white dark:border-gray-700"
              value={sort.sortDir}
              onChange={handleSortDirChange}
            >
              <option value="asc">Tăng dần</option>
              <option value="desc">Giảm dần</option>
            </select>
          </div>
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1 dark:text-gray-300">
              Bản ghi/trang:
            </label>
            <select
              className="border border-gray-300 dark:border-gray-600 px-2 py-2 rounded-lg focus:outline-gray-400 min-w-[60px] dark:bg-gray-900 dark:text-white dark:border-gray-700"
              value={pagination.size}
              onChange={handlePageSizeChange}
            >
              {pageSizes.map((size) => (
                <option key={size} value={size}>
                  {size}
                </option>
              ))}
            </select>
          </div>
        </div>
      </div>

      <div className="flex justify-end mb-2">
        <button
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow transition"
          onClick={() => {
            setSelectedAppointment(null);
            setModalOpen(true);
          }}
        >
          + Tạo lịch hẹn mới
        </button>
      </div>

      {/* Display reschedule notifications */}
      {appointments
        .filter((apt) => apt.appointmentStatus === "PendingReschedule" || apt.status === "PendingReschedule")
        .map((appointment) => {
          const proposedDateTime = extractProposedDateTime(appointment.notes);
          const reason = extractReason(appointment.notes);
          const requestedBy = extractRequestedBy(appointment.notes);
          
          return (
            <div key={appointment.id} className="mb-4">
              <RescheduleNotificationCard
                appointment={appointment}
                requestedBy={requestedBy || "Unknown"}
                proposedDateTime={proposedDateTime}
                reason={reason}
                onApprove={() => handleApproveReschedule(appointment.id)}
                onReject={() => handleRejectReschedule(appointment.id)}
                onCounterPropose={() => handleCounterReschedule(appointment.id)}
                currentUserRole={user?.role || "Admin"}
              />
            </div>
          );
        })}

      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <AppointmentTable
          appointments={appointments}
          loading={loading}
          onEdit={(appointment) => {
            setSelectedAppointment(appointment);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
          onConfirm={handleConfirm}
          onCancel={handleCancel}
          onComplete={handleComplete}
          onReschedule={handleReschedule}
        />
      </div>

      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số: <span className="font-semibold">{pagination.total}</span>{" "}
          lịch hẹn
        </div>
        <div className="flex items-center gap-2">
          <button
            className="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => handlePageChange(0)}
            disabled={pagination.page === 0}
          >
            Đầu
          </button>
          <button
            className="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => handlePageChange(pagination.page - 1)}
            disabled={pagination.page === 0}
          >
            Trước
          </button>
          <span className="mx-2 dark:text-gray-200">
            Trang <span className="font-semibold">{pagination.page + 1}</span> /{" "}
            {totalPages || 1}
          </span>
          <button
            className="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => handlePageChange(pagination.page + 1)}
            disabled={pagination.page + 1 >= totalPages}
          >
            Sau
          </button>
          <button
            className="px-3 py-1 border border-gray-300 dark:border-gray-600 rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => handlePageChange(totalPages - 1)}
            disabled={pagination.page + 1 >= totalPages}
          >
            Cuối
          </button>
        </div>
      </div>

      <AppointmentModal
        open={modalOpen}
        appointment={selectedAppointment}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
      />
    </div>
  );
};

export default AppointmentsManagementPage;