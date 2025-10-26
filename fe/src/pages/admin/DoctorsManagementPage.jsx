import React, { useEffect, useState, useCallback } from "react";
import {
  fetchDoctors,
  searchDoctors,
  createDoctor,
  updateDoctor,
  deleteDoctor,
  updateDoctorRating,
  updateDoctorStatus,
} from "../../services/adminDoctorAPI";
import DoctorTable from "../../components/admin/doctors/DoctorTable";
import DoctorSearchBar from "../../components/admin/doctors/DoctorSearchBar";
import DoctorModal from "../../components/admin/doctors/DoctorModal";

const sortFields = [
  { value: "id", label: "ID" },
  { value: "fullName", label: "Tên đầy đủ" },
  { value: "email", label: "Email" },
  { value: "specializationId", label: "Chuyên khoa" },
  { value: "rating", label: "Đánh giá" },
  { value: "experience", label: "Kinh nghiệm" },
];

const pageSizes = [5, 10, 20, 50, 100];

const DoctorsManagementPage = () => {
  const [doctors, setDoctors] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "asc" });
  const [search, setSearch] = useState("");
  const [specializationFilter, setSpecializationFilter] = useState("");
  const [minRatingFilter, setMinRatingFilter] = useState("");
  const [loading, setLoading] = useState(false);
  const [selectedDoctor, setSelectedDoctor] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);

  const totalPages = Math.ceil(pagination.total / pagination.size);

  const loadDoctors = useCallback(async () => {
    setLoading(true);
    try {
      let res;
      if (search && search.trim() !== "") {
        res = await searchDoctors({
          keyword: search,
          specializationId: specializationFilter || null,
          minRating: minRatingFilter || null,
          page: pagination.page + 1, // Backend uses 1-based pagination
          pageSize: pagination.size,
          sortBy: sort.sortBy,
          desc: sort.sortDir === "desc",
        });
      } else {
        res = await fetchDoctors({
          page: pagination.page,
          size: pagination.size,
          ...sort,
        });
      }

      if (res.data) {
        // Handle PagedResponse for search results
        if (res.data.items && res.data.totalCount !== undefined) {
          setDoctors(res.data.items);
          setPagination((prev) => ({
            ...prev,
            total: res.data.totalCount,
          }));
        } else {
          // Handle regular array response for fetchDoctors
          setDoctors(res.data);
          setPagination((prev) => ({
            ...prev,
            total: res.data.length,
          }));
        }
      }
    } catch (error) {
      console.error("Failed to load doctors:", error);
    }
    setLoading(false);
  }, [
    pagination.page,
    pagination.size,
    sort,
    search,
    specializationFilter,
    minRatingFilter,
  ]);

  useEffect(() => {
    loadDoctors();
  }, [loadDoctors]);

  const handleSave = async (data) => {
    try {
      if (selectedDoctor) {
        await updateDoctor(selectedDoctor.id, data);
      } else {
        await createDoctor(data);
      }
      setModalOpen(false);
      loadDoctors();
    } catch (error) {
      console.error("Failed to save doctor:", error);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa bác sĩ này?")) {
      try {
        await deleteDoctor(id);
        loadDoctors();
      } catch (error) {
        console.error("Failed to delete doctor:", error);
      }
    }
  };

  const handleUpdateRating = async (id, rating) => {
    try {
      await updateDoctorRating(id, rating);
      loadDoctors();
    } catch (error) {
      console.error("Failed to update rating:", error);
    }
  };

  const handleUpdateStatus = async (id, status) => {
    try {
      await updateDoctorStatus(id, status);
      loadDoctors();
    } catch (error) {
      console.error("Failed to update status:", error);
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

  const handleSpecializationFilter = (specializationId) => {
    setSpecializationFilter(specializationId);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleMinRatingFilter = (minRating) => {
    setMinRatingFilter(minRating);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  return (
    <div className="max-w-6xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý bác sĩ
      </h1>

      <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
        <div className="flex-1 min-w-[200px] flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Tìm kiếm:
          </label>
          <DoctorSearchBar
            value={search}
            onChange={setSearch}
            onSpecializationFilter={handleSpecializationFilter}
            onMinRatingFilter={handleMinRatingFilter}
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
            setSelectedDoctor(null);
            setModalOpen(true);
          }}
        >
          + Tạo bác sĩ mới
        </button>
      </div>

      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <DoctorTable
          doctors={doctors}
          loading={loading}
          onEdit={(doctor) => {
            setSelectedDoctor(doctor);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
          onUpdateRating={handleUpdateRating}
          onUpdateStatus={handleUpdateStatus}
        />
      </div>

      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số: <span className="font-semibold">{pagination.total}</span> bác
          sĩ
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

      <DoctorModal
        open={modalOpen}
        doctor={selectedDoctor}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
      />
    </div>
  );
};

export default DoctorsManagementPage;
