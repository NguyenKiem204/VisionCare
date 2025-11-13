import React, { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import { getPublishedBlogs } from "../services/blogService";
import BlogCard from "../components/blog/BlogCard";

export default function Blogs() {
  const [blogs, setBlogs] = useState([]);
  const [pagination, setPagination] = useState({
    page: 1,
    pageSize: 12,
    total: 0,
  });
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

  const totalPages = Math.ceil(pagination.total / pagination.pageSize);

  return (
    <div className="bg-white min-h-screen">
      <section className="relative">
        <div className="absolute inset-0">
          <img
            src="/images/blog-hero.jpg"
            alt="Bác sĩ VisionCare"
            className="w-full h-full object-cover"
            onError={(e) => {
              e.currentTarget.src =
                "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?auto=format&fit=crop&w=1600&q=80";
            }}
          />
          <div className="absolute inset-0 bg-slate-900/70" />
        </div>
        <div className="relative max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 pt-28 pb-20 text-white">
          <p className="text-sm uppercase tracking-[0.3em] text-white/70 mb-4">
            VisionCare Blog
          </p>
          <h1 className="text-4xl md:text-5xl font-extrabold mb-6">
            KIẾN THỨC NHÃN KHOA
          </h1>
          <div className="flex items-center gap-2 text-sm text-white/80">
            <Link to="/" className="hover:text-white transition-colors">
              Trang Chủ
            </Link>
            <span className="opacity-50">/</span>
            <span>Kiến Thức Nhãn Khoa</span>
          </div>
        </div>
      </section>

      <main className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 -mt-12 pb-16 mt-14">
        <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-6 mb-6">
          <div>
            <h2 className="text-2xl font-semibold text-slate-900 mb-2">
              Kho Kiến Thức VisionCare
            </h2>
            <p className="text-slate-500">
              Tìm kiếm bài viết theo chủ đề bạn quan tâm để hiểu rõ hơn về sức
              khỏe thị lực.
            </p>
          </div>
          <div className="relative w-full md:w-96">
            <input
              type="text"
              value={keyword}
              onChange={(e) => {
                setKeyword(e.target.value);
                setPagination((prev) => ({ ...prev, page: 1 }));
              }}
              placeholder="Tìm kiếm bài viết, chủ đề, tác giả..."
              className="w-full pl-4 pr-12 py-3 rounded-full !bg-white !border !border-gray-300 !text-gray-900 placeholder:!text-gray-400 shadow-md focus:outline-none focus:ring-2 focus:ring-blue-400 focus:border-transparent"
            />
            <div className="absolute inset-y-0 right-4 flex items-center pointer-events-none">
              <svg
                className="w-5 h-5 !text-gray-400"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M21 21l-4.35-4.35m0 0A7.5 7.5 0 103 10.5a7.5 7.5 0 0013.65 6.15z"
                />
              </svg>
            </div>
          </div>
        </div>

        {loading ? (
          <div className="text-center py-12">
            <div className="animate-spin rounded-full h-12 w-12 border-2 border-yellow-400 border-t-transparent mx-auto" />
            <p className="mt-4 text-slate-500">Đang tải bài viết...</p>
          </div>
        ) : blogs.length === 0 ? (
          <div className="text-center py-16 border border-dashed border-slate-200 rounded-2xl">
            <p className="text-slate-500 text-lg">
              Hiện chưa có bài viết nào phù hợp với từ khóa này.
            </p>
            <p className="text-slate-400 mt-2">
              Hãy thử tìm kiếm với từ khóa khác nhé!
            </p>
          </div>
        ) : (
          <>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {blogs.map((blog) => (
                <Link
                  key={blog.blogId}
                  to={`/blogs/${blog.slug}`}
                  className="group"
                >
                  <div className="bg-white rounded-2xl shadow-lg border border-slate-100 overflow-hidden transition-all duration-300 group-hover:-translate-y-2 group-hover:shadow-2xl">
                    <BlogCard blog={blog} />
                  </div>
                </Link>
              ))}
            </div>

            {totalPages > 1 && (
              <div className="flex items-center justify-center gap-3 mt-10">
                <button
                  onClick={() =>
                    setPagination((prev) => ({ ...prev, page: prev.page - 1 }))
                  }
                  disabled={pagination.page === 1}
                  className="px-4 py-2 rounded-full border border-slate-200 text-slate-700 hover:bg-slate-50 transition disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Trước
                </button>
                <span className="px-4 py-2 text-slate-700 font-semibold">
                  Trang {pagination.page} / {totalPages}
                </span>
                <button
                  onClick={() =>
                    setPagination((prev) => ({ ...prev, page: prev.page + 1 }))
                  }
                  disabled={pagination.page >= totalPages}
                  className="px-4 py-2 rounded-full border border-slate-200 text-slate-700 hover:bg-slate-50 transition disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  Sau
                </button>
              </div>
            )}
          </>
        )}
      </main>
    </div>
  );
}
