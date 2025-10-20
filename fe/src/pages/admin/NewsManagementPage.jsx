import React, { useEffect, useState, useCallback } from "react";
import {
  getAllNews,
  createNews,
  updateNews,
  deleteNews,
  changeNewsStatus,
  searchNews,
} from "../../services/adminNewsAPI";
import NewsTable from "../../components/admin/news/NewsTable";
import NewsModal from "../../components/admin/news/NewsModal";
import NewsSearchBar from "../../components/admin/news/NewsSearchBar";
import NewsFilter from "../../components/admin/news/NewsFilter";
import { useNavigate } from "react-router-dom";

const sortFields = [
  { value: "id", label: "ID" },
  { value: "title", label: "Tiêu đề" },
  { value: "status", label: "Trạng thái" },
  { value: "categoryId", label: "Danh mục" },
  { value: "authorId", label: "Tác giả" },
  { value: "createdAt", label: "Ngày tạo" },
];

const pageSizes = [5, 10, 20, 50, 100];

const initialForm = {
  title: "",
  slug: "",
  summary: "",
  content: "",
  imageUrl: "",
  status: "DRAFT",
  categoryId: "",
};

export default function NewsManagementPage() {
  const [newsList, setNewsList] = useState([]);
  const [pagination, setPagination] = useState({ page: 0, size: 10, total: 0 });
  const [sort, setSort] = useState({ sortBy: "id", sortDir: "desc" });
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(false);
  const [selectedNews, setSelectedNews] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [showDeleteId, setShowDeleteId] = useState(null);
  const [formData, setFormData] = useState(initialForm);
  const navigate = useNavigate();

  const totalPages = Math.ceil(pagination.total / pagination.size);

  const loadNews = useCallback(async () => {
    setLoading(true);
    try {
      let res;
      if (search && search.trim() !== "") {
        res = await searchNews({
          keyword: search,
          page: pagination.page,
          size: pagination.size,
          sortBy: sort.sortBy,
          sortDir: sort.sortDir,
        });
      } else {
        res = await getAllNews({
          page: pagination.page,
          size: pagination.size,
          sortBy: sort.sortBy,
          sortDir: sort.sortDir,
        });
      }
      setNewsList(res.data.data.content || res.data.data || []);
      setPagination((prev) => ({
        ...prev,
        total: res.data.data.totalElements,
      }));
    } catch {
      setNewsList([]);
    }
    setLoading(false);
  }, [pagination.page, pagination.size, sort, search]);

  useEffect(() => {
    const handler = setTimeout(() => {
      loadNews();
    }, 400);
    return () => clearTimeout(handler);
  }, [search, pagination.page, pagination.size, sort, loadNews]);
  
  useEffect(() => {
    loadNews();
  }, [loadNews]);

  const handleSave = async (data) => {
    if (selectedNews) await updateNews(selectedNews.id, data);
    else await createNews(data);
    setModalOpen(false);
    setSelectedNews(null);
    setFormData(initialForm);
    loadNews();
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa tin này?")) {
      await deleteNews(id);
      loadNews();
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

  return (
    <div className="max-w-6xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">
        Quản lý tin tức
      </h1>
      <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
        <div className="flex-1 min-w-[200px] flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Tìm kiếm:
          </label>
          <NewsSearchBar
            value={search}
            onChange={setSearch}
            inputClassName="dark:bg-gray-900 dark:text-white dark:border-gray-700 dark:placeholder-gray-400"
          />
        </div>
        <div className="flex flex-wrap gap-2 items-end min-w-[320px]">
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1 dark:text-gray-300">
              Sắp xếp:
            </label>
            <select
              className="border px-2 py-2 rounded-lg focus:outline-gray-400 min-w-[100px] dark:bg-gray-900 dark:text-white dark:border-gray-700"
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
              className="border px-2 py-2 rounded-lg focus:outline-gray-400 min-w-[80px] dark:bg-gray-900 dark:text-white dark:border-gray-700"
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
              className="border px-2 py-2 rounded-lg focus:outline-gray-400 min-w-[60px] dark:bg-gray-900 dark:text-white dark:border-gray-700"
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
          onClick={() => navigate("/admin/news/create")}
        >
          + Tạo tin mới
        </button>
      </div>
      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <NewsTable
          newsList={newsList}
          loading={loading}
          onEdit={(news) => {
            setSelectedNews(news);
            setFormData(news);
            setModalOpen(true);
          }}
          onDelete={handleDelete}
          onChangeStatus={async (id, status) => {
            await changeNewsStatus(id, status);
            loadNews();
          }}
          showDeleteId={showDeleteId}
          setShowDeleteId={setShowDeleteId}
        />
      </div>
      {/* Pagination */}
      <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
        <div className="text-sm text-gray-600 dark:text-gray-300">
          Tổng số: <span className="font-semibold">{pagination.total}</span> tin
        </div>
        <div className="flex items-center gap-2">
          <button
            className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => handlePageChange(pagination.page - 1)}
            disabled={pagination.page === 0}
          >
            Trước
          </button>
          <span className="text-gray-900 dark:text-white">
            Trang <span className="font-semibold">{pagination.page + 1}</span> /{" "}
            {totalPages || 1}
          </span>
          <button
            className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
            onClick={() => handlePageChange(pagination.page + 1)}
            disabled={pagination.page + 1 >= totalPages}
          >
            Sau
          </button>
        </div>
      </div>
      {/* Modal */}
      {modalOpen && (
        <div
          className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40"
          onClick={(e) => {
            if (e.target === e.currentTarget) setModalOpen(false);
          }}
        >
          <div className="bg-white dark:bg-gray-900 rounded-2xl shadow-2xl p-0 w-full max-w-3xl relative mx-2 animate-fadeIn max-h-[90vh] overflow-y-auto flex flex-col">
            <button
              className="sticky top-0 right-0 mt-4 mr-4 self-end text-gray-500 hover:text-gray-900 dark:hover:text-white text-3xl font-bold z-10 focus:outline-none bg-transparent"
              onClick={() => setModalOpen(false)}
              aria-label="Đóng"
              type="button"
            >
              ×
            </button>
            <div className="p-8 sm:p-10 pt-2 sm:pt-4">
              <NewsModal
                form={formData}
                setForm={setFormData}
                isEdit={!!selectedNews}
                onSubmit={async (e) => {
                  e.preventDefault();
                  await handleSave(formData);
                }}
                onCancel={() => setModalOpen(false)}
                loading={loading}
              />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
