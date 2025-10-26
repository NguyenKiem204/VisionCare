import React, { useEffect, useState, useCallback } from "react";
import {
  fetchCustomers,
  searchCustomers,
  createCustomer,
  updateCustomer,
  deleteCustomer,
} from "../../services/adminCustomerAPI";
import CustomerTable from "../../components/admin/customers/CustomerTable";
import CustomerSearchBar from "../../components/admin/customers/CustomerSearchBar";
import CustomerModal from "../../components/admin/customers/CustomerModal";

const sortFields = [
  { value: "id", label: "ID" },
  { value: "fullName", label: "Tên đầy đủ" },
  { value: "email", label: "Email" },
  { value: "gender", label: "Giới tính" },
  { value: "dateOfBirth", label: "Ngày sinh" },
];

const pageSizes = [5, 10, 20, 50, 100];

const CustomersManagementPage = () => {
  const [customers, setCustomers] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "asc" });
  const [search, setSearch] = useState("");
  const [genderFilter, setGenderFilter] = useState("");
  const [ageRangeFilter, setAgeRangeFilter] = useState({
    minAge: "",
    maxAge: "",
  });
  const [loading, setLoading] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);

  const totalPages = Math.ceil(pagination.total / pagination.size);

  const loadCustomers = useCallback(async () => {
    setLoading(true);
    try {
      // Always use search API for consistency and pagination support
      const res = await searchCustomers({
        keyword: search || "",
        gender: genderFilter || null,
        fromDob: ageRangeFilter.minAge
          ? new Date().getFullYear() -
            parseInt(ageRangeFilter.maxAge) +
            "-01-01"
          : null,
        toDob: ageRangeFilter.maxAge
          ? new Date().getFullYear() -
            parseInt(ageRangeFilter.minAge) +
            "-12-31"
          : null,
        page: pagination.page + 1, // Backend uses 1-based pagination
        pageSize: pagination.size,
        sortBy: sort.sortBy,
        desc: sort.sortDir === "desc",
      });

      // Backend returns PagedResponse structure
      if (res.data.success) {
        setCustomers(res.data.items || []);
        setPagination((prev) => ({
          ...prev,
          total: res.data.totalCount || 0,
        }));
      }
    } catch (error) {
      console.error("Failed to load customers:", error);
    }
    setLoading(false);
  }, [
    pagination.page,
    pagination.size,
    sort,
    search,
    genderFilter,
    ageRangeFilter,
  ]);

  useEffect(() => {
    loadCustomers();
  }, [loadCustomers]);

  const handleSave = async (data) => {
    try {
      if (selectedCustomer) {
        await updateCustomer(selectedCustomer.id, data);
      } else {
        await createCustomer(data);
      }
      setModalOpen(false);
      loadCustomers();
    } catch (error) {
      console.error("Failed to save customer:", error);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa khách hàng này?")) {
      try {
        await deleteCustomer(id);
        loadCustomers();
      } catch (error) {
        console.error("Failed to delete customer:", error);
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

  const handleGenderFilter = (gender) => {
    setGenderFilter(gender);
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  const handleAgeRangeFilter = (minAge, maxAge) => {
    setAgeRangeFilter({ minAge, maxAge });
    setPagination((prev) => ({ ...prev, page: 0 }));
  };

  return (
    <div className="max-w-6xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý khách hàng
      </h1>

      <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
        <div className="flex-1 min-w-[200px] flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Tìm kiếm:
          </label>
          <CustomerSearchBar
            value={search}
            onChange={setSearch}
            onGenderFilter={handleGenderFilter}
            onAgeRangeFilter={handleAgeRangeFilter}
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
            setSelectedCustomer(null);
            setModalOpen(true);
          }}
        >
          + Tạo khách hàng mới
        </button>
      </div>

      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <CustomerTable
          customers={customers}
          loading={loading}
          onEdit={(customer) => {
            setSelectedCustomer(customer);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
        />
      </div>

      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số: <span className="font-semibold">{pagination.total}</span>{" "}
          khách hàng
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

      <CustomerModal
        open={modalOpen}
        customer={selectedCustomer}
        onClose={() => setModalOpen(false)}
        onSave={handleSave}
      />
    </div>
  );
};

export default CustomersManagementPage;
