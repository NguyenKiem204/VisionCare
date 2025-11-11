import React, { useState, useEffect } from "react";
import {
  Calendar,
  Clock,
  User,
  Plus,
  Search,
  Filter,
  CheckCircle,
  XCircle,
  AlertCircle,
  Eye,
  Edit,
  Trash2,
  MoreVertical,
  CalendarX,
  UserX,
  AlertTriangle,
} from "lucide-react";
import {
  getAllDoctorAbsences,
  getPendingAbsences,
  getAbsencesByDoctor,
  approveAbsence,
  rejectAbsence,
  deleteDoctorAbsence,
} from "../../services/adminDoctorAbsenceAPI";
import { searchDoctors } from "../../services/adminDoctorAPI";
import toast from "react-hot-toast";
import CreateAbsenceModal from "../../components/admin/absence/CreateAbsenceModal";
import AbsenceDetailModal from "../../components/admin/absence/AbsenceDetailModal";
import HandleAppointmentsModal from "../../components/admin/absence/HandleAppointmentsModal";

const DoctorAbsenceManagement = () => {
  const [absences, setAbsences] = useState([]);
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filter, setFilter] = useState({
    status: "all", // all, pending, approved, rejected
    doctorId: null,
    searchTerm: "",
  });
  const [createModalOpen, setCreateModalOpen] = useState(false);
  const [detailModalOpen, setDetailModalOpen] = useState(false);
  const [handleModalOpen, setHandleModalOpen] = useState(false);
  const [selectedAbsence, setSelectedAbsence] = useState(null);

  useEffect(() => {
    loadAbsences();
    loadDoctors();
  }, [filter]);

  const loadDoctors = async () => {
    try {
      const response = await searchDoctors({ page: 1, pageSize: 100 });
      const doctorsList = response.data?.data || response.data?.items || [];
      setDoctors(doctorsList);
    } catch (error) {
      console.error("Error loading doctors:", error);
    }
  };

  const loadAbsences = async () => {
    setLoading(true);
    try {
      let response;
      if (filter.doctorId) {
        response = await getAbsencesByDoctor(filter.doctorId);
      } else if (filter.status === "pending") {
        response = await getPendingAbsences();
      } else {
        response = await getAllDoctorAbsences();
      }

      let absencesData = response.data?.data || [];

      // Filter by search term
      if (filter.searchTerm) {
        absencesData = absencesData.filter((absence) => {
          const doctorName = absence.doctorName?.toLowerCase() || "";
          const reason = absence.reason?.toLowerCase() || "";
          const search = filter.searchTerm.toLowerCase();
          return doctorName.includes(search) || reason.includes(search);
        });
      }

      // Filter by status
      if (filter.status !== "all") {
        absencesData = absencesData.filter(
          (absence) => absence.status?.toLowerCase() === filter.status
        );
      }

      setAbsences(absencesData);
    } catch (error) {
      console.error("Error loading absences:", error);
      toast.error("Không thể tải danh sách nghỉ phép");
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (id) => {
    if (
      !window.confirm(
        "Duyệt nghỉ phép này sẽ tự động xử lý các lịch hẹn bị ảnh hưởng. Bạn có chắc chắn?"
      )
    ) {
      return;
    }

    try {
      await approveAbsence(id);
      toast.success("Đã duyệt nghỉ phép và xử lý lịch hẹn");
      loadAbsences();
    } catch (error) {
      console.error("Error approving absence:", error);
      toast.error("Không thể duyệt nghỉ phép");
    }
  };

  const handleReject = async (id) => {
    if (!window.confirm("Bạn có chắc muốn từ chối nghỉ phép này?")) {
      return;
    }

    try {
      await rejectAbsence(id);
      toast.success("Đã từ chối nghỉ phép");
      loadAbsences();
    } catch (error) {
      console.error("Error rejecting absence:", error);
      toast.error("Không thể từ chối nghỉ phép");
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Bạn có chắc muốn xóa nghỉ phép này?")) {
      return;
    }

    try {
      await deleteDoctorAbsence(id);
      toast.success("Đã xóa nghỉ phép");
      loadAbsences();
    } catch (error) {
      console.error("Error deleting absence:", error);
      toast.error("Không thể xóa nghỉ phép");
    }
  };

  const handleViewDetail = (absence) => {
    setSelectedAbsence(absence);
    setDetailModalOpen(true);
  };

  const handleManualHandle = (absence) => {
    if (absence.status !== "Approved") {
      toast.error("Chỉ có thể xử lý lịch hẹn cho nghỉ phép đã được duyệt");
      return;
    }
    setSelectedAbsence(absence);
    setHandleModalOpen(true);
  };

  const getStatusBadge = (status, isResolved) => {
    const statusLower = status?.toLowerCase() || "";
    if (statusLower === "approved") {
      return (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200">
          <CheckCircle className="h-3 w-3 mr-1" />
          Đã duyệt {isResolved ? "(Đã xử lý)" : "(Chưa xử lý)"}
        </span>
      );
    } else if (statusLower === "pending") {
      return (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200">
          <AlertCircle className="h-3 w-3 mr-1" />
          Chờ duyệt
        </span>
      );
    } else if (statusLower === "rejected") {
      return (
        <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200">
          <XCircle className="h-3 w-3 mr-1" />
          Đã từ chối
        </span>
      );
    }
    return (
      <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300">
        {status}
      </span>
    );
  };

  const getAbsenceTypeBadge = (type) => {
    const typeLower = type?.toLowerCase() || "";
    const colors = {
      leave: "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200",
      emergency: "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200",
      sick: "bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-200",
      other: "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300",
    };

    const labels = {
      leave: "Nghỉ phép",
      emergency: "Khẩn cấp",
      sick: "Ốm đau",
      other: "Khác",
    };

    return (
      <span
        className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${
          colors[typeLower] || colors.other
        }`}
      >
        {labels[typeLower] || type}
      </span>
    );
  };

  return (
    <div className="space-y-6 p-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white flex items-center gap-3">
            <CalendarX className="h-8 w-8 text-red-600" />
            Quản lý nghỉ phép bác sĩ
          </h1>
          <p className="text-gray-600 dark:text-gray-400 mt-2">
            Quản lý đơn nghỉ phép và xử lý lịch hẹn khi bác sĩ nghỉ đột xuất
          </p>
        </div>
        <button
          onClick={() => setCreateModalOpen(true)}
          className="px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-medium transition-all flex items-center gap-2 shadow-lg hover:shadow-xl"
        >
          <Plus className="h-5 w-5" />
          Tạo đơn nghỉ phép
        </button>
      </div>

      {/* Filters */}
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          {/* Search */}
          <div className="md:col-span-2">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
              <input
                type="text"
                placeholder="Tìm kiếm theo tên bác sĩ, lý do..."
                className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                value={filter.searchTerm}
                onChange={(e) =>
                  setFilter({ ...filter, searchTerm: e.target.value })
                }
              />
            </div>
          </div>

          {/* Status Filter */}
          <div>
            <select
              className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              value={filter.status}
              onChange={(e) => setFilter({ ...filter, status: e.target.value })}
            >
              <option value="all">Tất cả trạng thái</option>
              <option value="pending">Chờ duyệt</option>
              <option value="approved">Đã duyệt</option>
              <option value="rejected">Đã từ chối</option>
            </select>
          </div>

          {/* Doctor Filter */}
          <div>
            <select
              className="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              value={filter.doctorId || ""}
              onChange={(e) =>
                setFilter({
                  ...filter,
                  doctorId: e.target.value ? Number(e.target.value) : null,
                })
              }
            >
              <option value="">Tất cả bác sĩ</option>
              {doctors.map((doctor) => (
                <option
                  key={doctor.doctorId || doctor.id}
                  value={doctor.doctorId || doctor.id}
                >
                  {doctor.doctorName || doctor.fullName || doctor.name}
                </option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {/* Statistics Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700 p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-600 dark:text-gray-400">Tổng cộng</p>
              <p className="text-2xl font-bold text-gray-900 dark:text-white mt-1">
                {absences.length}
              </p>
            </div>
            <CalendarX className="h-8 w-8 text-gray-400" />
          </div>
        </div>

        <div className="bg-yellow-50 dark:bg-yellow-900/20 rounded-xl shadow-sm border border-yellow-200 dark:border-yellow-800 p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-yellow-600 dark:text-yellow-400">Chờ duyệt</p>
              <p className="text-2xl font-bold text-yellow-900 dark:text-yellow-200 mt-1">
                {absences.filter((a) => a.status?.toLowerCase() === "pending").length}
              </p>
            </div>
            <AlertCircle className="h-8 w-8 text-yellow-600" />
          </div>
        </div>

        <div className="bg-green-50 dark:bg-green-900/20 rounded-xl shadow-sm border border-green-200 dark:border-green-800 p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-green-600 dark:text-green-400">Đã duyệt</p>
              <p className="text-2xl font-bold text-green-900 dark:text-green-200 mt-1">
                {absences.filter((a) => a.status?.toLowerCase() === "approved").length}
              </p>
            </div>
            <CheckCircle className="h-8 w-8 text-green-600" />
          </div>
        </div>

        <div className="bg-red-50 dark:bg-red-900/20 rounded-xl shadow-sm border border-red-200 dark:border-red-800 p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-red-600 dark:text-red-400">Chưa xử lý</p>
              <p className="text-2xl font-bold text-red-900 dark:text-red-200 mt-1">
                {
                  absences.filter(
                    (a) =>
                      a.status?.toLowerCase() === "approved" && !a.isResolved
                  ).length
                }
              </p>
            </div>
            <AlertTriangle className="h-8 w-8 text-red-600" />
          </div>
        </div>
      </div>

      {/* Absences Table */}
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-sm border border-gray-200 dark:border-gray-700 overflow-hidden">
        <div className="px-6 py-4 border-b border-gray-200 dark:border-gray-700">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
            Danh sách đơn nghỉ phép ({absences.length})
          </h3>
        </div>

        {loading ? (
          <div className="p-12 text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600 dark:text-gray-400">Đang tải...</p>
          </div>
        ) : absences.length === 0 ? (
          <div className="p-12 text-center">
            <CalendarX className="h-16 w-16 text-gray-400 mx-auto mb-4" />
            <p className="text-gray-600 dark:text-gray-400">
              Không có đơn nghỉ phép nào
            </p>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
              <thead className="bg-gray-50 dark:bg-gray-900">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                    Bác sĩ
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                    Thời gian nghỉ
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                    Loại
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                    Lý do
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                    Trạng thái
                  </th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
                    Thao tác
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
                {absences.map((absence) => (
                  <tr
                    key={absence.id}
                    className="hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
                  >
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <User className="h-5 w-5 text-gray-400 mr-2" />
                        <div>
                          <div className="text-sm font-medium text-gray-900 dark:text-white">
                            {absence.doctorName || "Không rõ"}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900 dark:text-white">
                        {new Date(absence.startDate).toLocaleDateString("vi-VN")}
                      </div>
                      <div className="text-sm text-gray-500 dark:text-gray-400 flex items-center">
                        <Clock className="h-3 w-3 mr-1" />
                        đến{" "}
                        {new Date(absence.endDate).toLocaleDateString("vi-VN")}
                      </div>
                      <div className="text-xs text-gray-400 mt-1">
                        (
                        {Math.ceil(
                          (new Date(absence.endDate) -
                            new Date(absence.startDate)) /
                            (1000 * 60 * 60 * 24)
                        ) + 1}{" "}
                        ngày)
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {getAbsenceTypeBadge(absence.absenceType)}
                    </td>
                    <td className="px-6 py-4">
                      <div className="text-sm text-gray-900 dark:text-white max-w-xs truncate">
                        {absence.reason || "Không có lý do"}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      {getStatusBadge(absence.status, absence.isResolved)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <div className="flex items-center justify-end gap-2">
                        <button
                          onClick={() => handleViewDetail(absence)}
                          className="text-blue-600 hover:text-blue-900 dark:text-blue-400 dark:hover:text-blue-300 p-2 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded"
                          title="Xem chi tiết"
                        >
                          <Eye className="h-4 w-4" />
                        </button>

                        {absence.status?.toLowerCase() === "pending" && (
                          <>
                            <button
                              onClick={() => handleApprove(absence.id)}
                              className="text-green-600 hover:text-green-900 dark:text-green-400 dark:hover:text-green-300 p-2 hover:bg-green-50 dark:hover:bg-green-900/20 rounded"
                              title="Duyệt"
                            >
                              <CheckCircle className="h-4 w-4" />
                            </button>
                            <button
                              onClick={() => handleReject(absence.id)}
                              className="text-red-600 hover:text-red-900 dark:text-red-400 dark:hover:text-red-300 p-2 hover:bg-red-50 dark:hover:bg-red-900/20 rounded"
                              title="Từ chối"
                            >
                              <XCircle className="h-4 w-4" />
                            </button>
                          </>
                        )}

                        {absence.status?.toLowerCase() === "approved" &&
                          !absence.isResolved && (
                            <button
                              onClick={() => handleManualHandle(absence)}
                              className="text-orange-600 hover:text-orange-900 dark:text-orange-400 dark:hover:text-orange-300 p-2 hover:bg-orange-50 dark:hover:bg-orange-900/20 rounded"
                              title="Xử lý lịch hẹn"
                            >
                              <UserX className="h-4 w-4" />
                            </button>
                          )}

                        <button
                          onClick={() => handleDelete(absence.id)}
                          className="text-red-600 hover:text-red-900 dark:text-red-400 dark:hover:text-red-300 p-2 hover:bg-red-50 dark:hover:bg-red-900/20 rounded"
                          title="Xóa"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Modals */}
      <CreateAbsenceModal
        open={createModalOpen}
        onClose={() => setCreateModalOpen(false)}
        onSuccess={loadAbsences}
        doctors={doctors}
      />

      <AbsenceDetailModal
        open={detailModalOpen}
        absence={selectedAbsence}
        onClose={() => {
          setDetailModalOpen(false);
          setSelectedAbsence(null);
        }}
      />

      <HandleAppointmentsModal
        open={handleModalOpen}
        absence={selectedAbsence}
        onClose={() => {
          setHandleModalOpen(false);
          setSelectedAbsence(null);
        }}
        onSuccess={loadAbsences}
      />
    </div>
  );
};

export default DoctorAbsenceManagement;

