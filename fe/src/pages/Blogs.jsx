import React, { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import { getPublishedBlogs } from "../services/blogService";
import BlogCard from "../components/blog/BlogCard";

export default function Blogs() {
  const [blogs, setBlogs] = useState([]);
  const [pagination, setPagination] = useState({ page: 1, pageSize: 12, total: 0 });
  const [loading, setLoading] = useState(false);
  const [keyword, setKeyword] = useState("");

  const loadBlogs = useCallback(async () => {
    setLoading(true);
    try {
      const response = await getPublishedBlogs({
        page: pagination.page,
        pageSize: pagination.pageSize,
        keyword,
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
    } finally {
      setLoading(false);
    }
  }, [pagination.page, pagination.pageSize, keyword]);

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      loadBlogs();
    }, 300);
    return () => clearTimeout(timeoutId);
  }, [keyword, loadBlogs]);

  useEffect(() => {
    loadBlogs();
  }, [pagination.page, pagination.pageSize]);

  const totalPages = Math.ceil(pagination.total / pagination.pageSize);

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-7xl mx-auto px-4 py-8">
        {/* Header */}
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold text-gray-900 dark:text-white mb-4">
            Blog VisionCare
          </h1>
          <p className="text-lg text-gray-600 dark:text-gray-400">
            Chia sẻ kiến thức và cập nhật mới nhất về chăm sóc mắt
          </p>
        </div>

        {/* Search */}
        <div className="mb-8">
          <input
            type="text"
            value={keyword}
            onChange={(e) => {
              setKeyword(e.target.value);
              setPagination((prev) => ({ ...prev, page: 1 }));
            }}
            placeholder="Tìm kiếm blog..."
            className="w-full max-w-2xl mx-auto block px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white"
          />
        </div>

        {/* Blog List */}
        {loading ? (
          <div className="text-center py-12">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600 dark:text-gray-400">Đang tải...</p>
          </div>
        ) : blogs.length === 0 ? (
          <div className="text-center py-12 bg-white dark:bg-gray-800 rounded-lg shadow-md">
            <p className="text-gray-600 dark:text-gray-400">Không tìm thấy blog nào</p>
          </div>
        ) : (
          <>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
              {blogs.map((blog) => (
                <Link key={blog.blogId} to={`/blogs/${blog.slug}`}>
                  <BlogCard blog={blog} />
                </Link>
              ))}
            </div>

            {/* Pagination */}
            {totalPages > 1 && (
              <div className="flex items-center justify-center gap-2">
                <button
                  onClick={() =>
                    setPagination((prev) => ({ ...prev, page: prev.page - 1 }))
                  }
                  disabled={pagination.page === 1}
                  className="px-4 py-2 bg-white dark:bg-gray-800 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed text-gray-700 dark:text-gray-300"
                >
                  Trước
                </button>
                <span className="px-4 py-2 text-gray-700 dark:text-gray-300">
                  Trang {pagination.page} / {totalPages}
                </span>
                <button
                  onClick={() =>
                    setPagination((prev) => ({ ...prev, page: prev.page + 1 }))
                  }
                  disabled={pagination.page >= totalPages}
                  className="px-4 py-2 bg-white dark:bg-gray-800 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed text-gray-700 dark:text-gray-300"
                >
                  Sau
                </button>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
}

