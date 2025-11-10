import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import {
  getMyBlogs,
  deleteBlog,
  publishBlog,
  unpublishBlog,
} from "../../services/blogService";
import BlogCard from "../../components/blog/BlogCard";
import toast from "react-hot-toast";

const pageSizes = [5, 10, 20, 50, 100];

export default function DoctorBlogsManagement() {
  const [blogs, setBlogs] = useState([]);
  const [pagination, setPagination] = useState({ page: 1, pageSize: 10, total: 0 });
  const [loading, setLoading] = useState(false);
  const [filters, setFilters] = useState({
    keyword: "",
    status: "",
  });

  const navigate = useNavigate();

  const loadBlogs = useCallback(async () => {
    setLoading(true);
    try {
      const response = await getMyBlogs({
        page: pagination.page,
        pageSize: pagination.pageSize,
        ...filters,
      });
      
      if (response.success) {
        setBlogs(response.Items || response.items || []);
        setPagination((prev) => ({
          ...prev,
          total: response.TotalCount || response.totalCount || 0,
        }));
      }
    } catch (error) {
      console.error("Error loading blogs:", error);
      toast.error("Không thể tải danh sách blog");
    } finally {
      setLoading(false);
    }
  }, [pagination.page, pagination.pageSize, filters]);

  useEffect(() => {
    loadBlogs();
  }, [loadBlogs]);

  const handleDelete = async (id) => {
    if (!window.confirm("Bạn có chắc chắn muốn xóa blog này?")) return;

    try {
      await deleteBlog(id);
      toast.success("Xóa blog thành công");
      loadBlogs();
    } catch (error) {
      console.error("Error deleting blog:", error);
      toast.error("Không thể xóa blog");
    }
  };

  const handlePublish = async (id) => {
    try {
      await publishBlog(id);
      toast.success("Xuất bản blog thành công");
      loadBlogs();
    } catch (error) {
      console.error("Error publishing blog:", error);
      toast.error("Không thể xuất bản blog");
    }
  };

  const handleUnpublish = async (id) => {
    try {
      await unpublishBlog(id);
      toast.success("Gỡ xuất bản blog thành công");
      loadBlogs();
    } catch (error) {
      console.error("Error unpublishing blog:", error);
      toast.error("Không thể gỡ xuất bản blog");
    }
  };

  const totalPages = Math.ceil(pagination.total / pagination.pageSize);

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setPagination((prev) => ({ ...prev, page: newPage }));
    }
  };

  const handlePageSizeChange = (e) => {
    setPagination((prev) => ({
      ...prev,
      pageSize: Number(e.target.value),
      page: 1,
    }));
  };

  return (
    <div className="p-6 bg-gray-50 dark:bg-gray-900 min-h-screen">
      <div className="max-w-7xl mx-auto">
        <div className="flex items-center justify-between mb-6">
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
            Quản lý Blog của tôi
          </h1>
          <button
            onClick={() => navigate("/doctor/blogs/new")}
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
          >
            + Tạo Blog Mới
          </button>
        </div>

        {/* Filters */}
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-4 mb-6">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Tìm kiếm
              </label>
              <input
                type="text"
                value={filters.keyword}
                onChange={(e) =>
                  setFilters((prev) => ({ ...prev, keyword: e.target.value }))
                }
                placeholder="Nhập từ khóa..."
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Trạng thái
              </label>
              <select
                value={filters.status}
                onChange={(e) =>
                  setFilters((prev) => ({ ...prev, status: e.target.value }))
                }
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              >
                <option value="">Tất cả</option>
                <option value="Draft">Draft</option>
                <option value="Published">Published</option>
                <option value="Archived">Archived</option>
              </select>
            </div>
            <div className="flex flex-col">
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Bản ghi/trang:
              </label>
              <select
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
                value={pagination.pageSize}
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

        {/* Blog List */}
        {loading ? (
          <div className="text-center py-12">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600 dark:text-gray-400">Đang tải...</p>
          </div>
        ) : blogs.length === 0 ? (
          <div className="text-center py-12 bg-white dark:bg-gray-800 rounded-lg shadow-md">
            <p className="text-gray-600 dark:text-gray-400">Không có blog nào</p>
          </div>
        ) : (
          <>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-6">
              {blogs.map((blog) => (
                <div key={blog.blogId} className="relative">
                  <BlogCard blog={blog} />
                  <div className="absolute top-4 right-4 flex gap-2">
                    <button
                      onClick={() => navigate(`/doctor/blogs/${blog.blogId}/edit`)}
                      className="p-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
                      title="Chỉnh sửa"
                    >
                      ✏️
                    </button>
                    {blog.status === "Published" ? (
                      <button
                        onClick={() => handleUnpublish(blog.blogId)}
                        className="p-2 bg-yellow-600 text-white rounded-lg hover:bg-yellow-700 transition-colors"
                        title="Gỡ xuất bản"
                      >
                        👁️‍🗨️
                      </button>
                    ) : (
                      <button
                        onClick={() => handlePublish(blog.blogId)}
                        className="p-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
                        title="Xuất bản"
                      >
                        📢
                      </button>
                    )}
                    <button
                      onClick={() => handleDelete(blog.blogId)}
                      className="p-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors"
                      title="Xóa"
                    >
                      🗑️
                    </button>
                  </div>
                </div>
              ))}
            </div>

            {/* Pagination */}
            <div className="flex flex-col sm:flex-row justify-center sm:justify-between items-center gap-2 mt-4">
              <div className="text-sm text-gray-600 dark:text-gray-300">
                Tổng số: <span className="font-semibold">{pagination.total}</span> blog
              </div>
              {totalPages > 1 && (
                <div className="flex items-center gap-2">
                  <button
                    className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
                    onClick={() => handlePageChange(1)}
                    disabled={pagination.page === 1}
                  >
                    Đầu
                  </button>
                  <button
                    className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
                    onClick={() => handlePageChange(pagination.page - 1)}
                    disabled={pagination.page === 1}
                  >
                    Trước
                  </button>
                  <span className="mx-2 dark:text-gray-200">
                    Trang <span className="font-semibold">{pagination.page}</span> /{" "}
                    {totalPages || 1}
                  </span>
                  <button
                    className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
                    onClick={() => handlePageChange(pagination.page + 1)}
                    disabled={pagination.page >= totalPages}
                  >
                    Sau
                  </button>
                  <button
                    className="px-3 py-1 border rounded-lg disabled:opacity-50 bg-gray-100 dark:bg-gray-900 dark:text-white dark:border-gray-700 hover:bg-gray-200 dark:hover:bg-gray-700 transition"
                    onClick={() => handlePageChange(totalPages)}
                    disabled={pagination.page >= totalPages}
                  >
                    Cuối
                  </button>
                </div>
              )}
            </div>
          </>
        )}
      </div>
    </div>
  );
}

