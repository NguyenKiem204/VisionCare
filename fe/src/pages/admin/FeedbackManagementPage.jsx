import React, { useEffect, useState, useCallback } from "react";
import {
  fetchDoctorFeedbacks,
  fetchServiceFeedbacks,
  searchDoctorFeedbacks,
  searchServiceFeedbacks,
  respondToDoctorFeedback,
  respondToServiceFeedback,
  deleteDoctorFeedback,
  deleteServiceFeedback,
} from "../../services/adminFeedbackAPI";
import FeedbackTable from "../../components/admin/feedback/FeedbackTable";
import FeedbackSearchBar from "../../components/admin/feedback/FeedbackSearchBar";
import FeedbackResponseModal from "../../components/admin/feedback/FeedbackResponseModal";

const sortFields = [
  { value: "id", label: "ID" },
  { value: "rating", label: "Đánh giá" },
  { value: "feedbackDate", label: "Ngày phản hồi" },
  { value: "patientName", label: "Bệnh nhân" },
  { value: "doctorName", label: "Bác sĩ" },
  { value: "serviceName", label: "Dịch vụ" },
];

const pageSizes = [5, 10, 20, 50, 100];

const FeedbackManagementPage = () => {
  const [feedbacks, setFeedbacks] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "desc" });
  const [search, setSearch] = useState("");
  const [feedbackType, setFeedbackType] = useState("doctor"); // "doctor" or "service"
  const [ratingFilter, setRatingFilter] = useState("");
  const [hasResponseFilter, setHasResponseFilter] = useState("");
  const [dateRange, setDateRange] = useState({ from: "", to: "" });
  const [loading, setLoading] = useState(false);
  const [selectedFeedback, setSelectedFeedback] = useState(null);
  const [responseModalOpen, setResponseModalOpen] = useState(false);

  const totalPages = Math.ceil(pagination.total / pagination.size);

  const loadFeedbacks = useCallback(async () => {
    setLoading(true);
    try {
      let res;
      if (search && search.trim() !== "") {
        const searchParams = {
          keyword: search,
          minRating: ratingFilter || null,
          hasResponse: hasResponseFilter !== "" ? hasResponseFilter === "true" : null,
          fromDate: dateRange.from || null,
          toDate: dateRange.to || null,
          page: pagination.page + 1,
          pageSize: pagination.size,
          sortBy: sort.sortBy,
          desc: sort.sortDir === "desc",
        };

        if (feedbackType === "doctor") {
          res = await searchDoctorFeedbacks(searchParams);
        } else {
          res = await searchServiceFeedbacks(searchParams);
        }
      } else {
        if (feedbackType === "doctor") {
          res = await fetchDoctorFeedbacks();
        } else {
          res = await fetchServiceFeedbacks();
        }
      }

      if (res.data) {
        // Handle PagedResponse for search results
        if (res.data.items && res.data.totalCount !== undefined) {
          setFeedbacks(res.data.items);
          setPagination((prev) => ({
            ...prev,
            total: res.data.totalCount,
          }));
        } else {
          // Handle regular array response
          setFeedbacks(res.data.data || res.data);
          setPagination((prev) => ({
            ...prev,
            total: res.data.data?.length || res.data.length || 0,
          }));
        }
      }
    } catch (error) {
      console.error("Failed to load feedbacks:", error);
    }
    setLoading(false);
  }, [
    pagination.page,
    pagination.size,
    sort,
    search,
    feedbackType,
    ratingFilter,
    hasResponseFilter,
    dateRange,
  ]);

  useEffect(() => {
    loadFeedbacks();
  }, [loadFeedbacks]);

  const handleRespond = async (responseText) => {
    try {
      if (feedbackType === "doctor") {
        await respondToDoctorFeedback(selectedFeedback.id, { responseText });
      } else {
        await respondToServiceFeedback(selectedFeedback.id, { responseText });
      }
      setResponseModalOpen(false);
      loadFeedbacks();
    } catch (error) {
      console.error("Failed to respond to feedback:", error);
      alert("Có lỗi xảy ra khi phản hồi. Vui lòng thử lại.");
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa phản hồi này?")) {
      try {
        if (feedbackType === "doctor") {
          await deleteDoctorFeedback(id);
        } else {
          await deleteServiceFeedback(id);
        }
        loadFeedbacks();
      } catch (error) {
        console.error("Failed to delete feedback:", error);
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

  const handleFeedbackTypeChange = (type) => {
    setFeedbackType(type);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleRatingFilter = (rating) => {
    setRatingFilter(rating);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleHasResponseFilter = (hasResponse) => {
    setHasResponseFilter(hasResponse);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleDateRangeChange = (field, value) => {
    setDateRange((prev) => ({ ...prev, [field]: value }));
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  return (
    <div className="max-w-7xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý phản hồi
      </h1>

      {/* Feedback Type Tabs */}
      <div className="flex border-b border-gray-200 dark:border-gray-700 mb-6">
        <button
          onClick={() => handleFeedbackTypeChange("doctor")}
          className={`px-4 py-2 text-sm font-medium ${
            feedbackType === "doctor"
              ? "text-blue-600 border-b-2 border-blue-600"
              : "text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300"
          }`}
        >
          Phản hồi bác sĩ
        </button>
        <button
          onClick={() => handleFeedbackTypeChange("service")}
          className={`px-4 py-2 text-sm font-medium ${
            feedbackType === "service"
              ? "text-blue-600 border-b-2 border-blue-600"
              : "text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300"
          }`}
        >
          Phản hồi dịch vụ
        </button>
      </div>

      <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
        <div className="flex-1 min-w-[200px] flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Tìm kiếm:
          </label>
          <FeedbackSearchBar
            value={search}
            onChange={setSearch}
            onRatingFilter={handleRatingFilter}
            onHasResponseFilter={handleHasResponseFilter}
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

      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <FeedbackTable
          feedbacks={feedbacks}
          loading={loading}
          feedbackType={feedbackType}
          onRespond={(feedback) => {
            setSelectedFeedback(feedback);
            setResponseModalOpen(true);
          }}
          onDelete={handleDelete}
        />
      </div>

      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số: <span className="font-semibold">{pagination.total}</span> phản hồi
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

      <FeedbackResponseModal
        open={responseModalOpen}
        feedback={selectedFeedback}
        onClose={() => setResponseModalOpen(false)}
        onSave={handleRespond}
      />
    </div>
  );
};

export default FeedbackManagementPage;
