import React, { useEffect, useState, useCallback } from "react";
import {
  fetchMedicalRecords,
  searchMedicalRecords,
  createMedicalRecord,
  updateMedicalRecord,
  deleteMedicalRecord,
} from "../../services/adminMedicalRecordsAPI";
import MedicalRecordsTable from "../../components/admin/medicalRecords/MedicalRecordsTable";
import MedicalRecordsSearchBar from "../../components/admin/medicalRecords/MedicalRecordsSearchBar";
import MedicalRecordsModal from "../../components/admin/medicalRecords/MedicalRecordsModal";

const sortFields = [
  { value: "id", label: "ID" },
  { value: "patientName", label: "Bệnh nhân" },
  { value: "doctorName", label: "Bác sĩ" },
  { value: "appointmentDate", label: "Ngày khám" },
  { value: "diagnosis", label: "Chẩn đoán" },
];

const pageSizes = [5, 10, 20, 50, 100];

const MedicalRecordsManagementPage = () => {
  const [medicalRecords, setMedicalRecords] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "desc" });
  const [search, setSearch] = useState("");
  const [patientFilter, setPatientFilter] = useState("");
  const [doctorFilter, setDoctorFilter] = useState("");
  const [dateRange, setDateRange] = useState({ from: "", to: "" });
  const [loading, setLoading] = useState(false);
  const [selectedMedicalRecord, setSelectedMedicalRecord] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);

  const totalPages = Math.ceil(pagination.total / pagination.size);

  const loadMedicalRecords = useCallback(async () => {
    setLoading(true);
    try {
      let res;
      if (search && search.trim() !== "") {
        res = await searchMedicalRecords({
          keyword: search,
          patientId: patientFilter || null,
          doctorId: doctorFilter || null,
          fromDate: dateRange.from || null,
          toDate: dateRange.to || null,
          page: pagination.page + 1, // Backend uses 1-based pagination
          pageSize: pagination.size,
          sortBy: sort.sortBy,
          desc: sort.sortDir === "desc",
        });
      } else {
        res = await fetchMedicalRecords();
      }

      if (res.data) {
        // Handle PagedResponse for search results
        if (res.data.items && res.data.totalCount !== undefined) {
          setMedicalRecords(res.data.items);
          setPagination((prev) => ({
            ...prev,
            total: res.data.totalCount,
          }));
        } else {
          // Handle regular array response for fetchMedicalRecords
          setMedicalRecords(res.data.data || res.data);
          setPagination((prev) => ({
            ...prev,
            total: res.data.data?.length || res.data.length || 0,
          }));
        }
      }
    } catch (error) {
      console.error("Failed to load medical records:", error);
    }
    setLoading(false);
  }, [
    pagination.page,
    pagination.size,
    sort,
    search,
    patientFilter,
    doctorFilter,
    dateRange,
  ]);

  useEffect(() => {
    loadMedicalRecords();
  }, [loadMedicalRecords]);

  const handleSave = async (data) => {
    try {
      if (selectedMedicalRecord) {
        await updateMedicalRecord(selectedMedicalRecord.id, data);
      } else {
        await createMedicalRecord(data);
      }
      setModalOpen(false);
      loadMedicalRecords();
    } catch (error) {
      console.error("Failed to save medical record:", error);
      alert("Có lỗi xảy ra khi lưu hồ sơ bệnh án. Vui lòng thử lại.");
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa hồ sơ bệnh án này?")) {
      try {
        await deleteMedicalRecord(id);
        loadMedicalRecords();
      } catch (error) {
        console.error("Failed to delete medical record:", error);
      }
    }
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

  const handlePatientFilter = (patientId) => {
    setPatientFilter(patientId);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleDoctorFilter = (doctorId) => {
    setDoctorFilter(doctorId);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleDateRangeChange = (field, value) => {
    setDateRange((prev) => ({ ...prev, [field]: value }));
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  return (
    <div className="max-w-7xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý hồ sơ bệnh án
      </h1>

      <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
        <div className="flex-1 min-w-[200px] flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Tìm kiếm:
          </label>
          <MedicalRecordsSearchBar
            value={search}
            onChange={setSearch}
            onPatientFilter={handlePatientFilter}
            onDoctorFilter={handleDoctorFilter}
            onDateRangeChange={handleDateRangeChange}
            dateRange={dateRange}
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
            setSelectedMedicalRecord(null);
            setModalOpen(true);
          }}
        >
          + Thêm hồ sơ mới
        </button>
      </div>

      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <MedicalRecordsTable
          medicalRecords={medicalRecords}
          loading={loading}
          onEdit={(medicalRecord) => {
            setSelectedMedicalRecord(medicalRecord);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
        />
      </div>

      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số: <span className="font-semibold">{pagination.total}</span> hồ sơ bệnh án
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

      <MedicalRecordsModal
        open={modalOpen}
        medicalRecord={selectedMedicalRecord}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
      />
    </div>
  );
};

export default MedicalRecordsManagementPage;
