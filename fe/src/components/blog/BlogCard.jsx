import { Link } from "react-router-dom";
import { format } from "date-fns";

const BlogCard = ({ blog, className = "" }) => {
  const formatDate = (date) => {
    if (!date) return "";
    try {
      return format(new Date(date), "dd/MM/yyyy");
    } catch {
      return "";
    }
  };

  const image = blog.featuredImage;
  const excerpt = (blog.excerpt || "").trim();

  return (
    <article
      className={`group flex flex-col h-full rounded-2xl border border-slate-100 bg-white shadow-lg transition-all duration-300 hover:-translate-y-1 hover:shadow-2xl dark:border-slate-700 dark:bg-slate-800 ${className}`}
    >
      <div className="relative h-52 overflow-hidden bg-slate-100 dark:bg-slate-700">
        {image && (
          <img
            src={image}
            alt={blog.title || "Hình ảnh blog"}
            className="absolute inset-0 h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        )}
      </div>

      <div className="flex flex-col flex-1 p-6">
        <Link to={`/blogs/${blog.slug}`}>
          <h3 className="text-lg font-semibold leading-snug text-slate-900 transition-colors duration-200 hover:text-blue-600 line-clamp-2 min-h-[3.25rem] dark:text-slate-100">
            {blog.title}
          </h3>
        </Link>

        <div className="mt-3 flex flex-wrap items-center gap-4 text-sm text-slate-500 dark:text-slate-400">
          <div className="flex items-center gap-2">
            <svg className="w-4 h-4 text-yellow-500" fill="currentColor" viewBox="0 0 20 20">
              <path d="M10 12a2 2 0 100-4 2 2 0 000 4z" />
              <path
                fillRule="evenodd"
                d="M.458 10C1.732 5.943 5.522 3 10 3s8.268 2.943 9.542 7c-1.274 4.057-5.064 7-9.542 7S1.732 14.057.458 10zM14 10a4 4 0 11-8 0 4 4 0 018 0z"
                clipRule="evenodd"
              />
            </svg>
            <span>{blog.authorName || "hangvt-admin"}</span>
          </div>
          <div className="flex items-center gap-2">
            <svg className="w-4 h-4 text-yellow-500" fill="currentColor" viewBox="0 0 20 20">
              <path
                fillRule="evenodd"
                d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z"
                clipRule="evenodd"
              />
            </svg>
            <span>{formatDate(blog.publishedAt || blog.createdAt)}</span>
          </div>
        </div>

        <p className="mt-4 flex-1 text-sm leading-relaxed text-slate-600 line-clamp-3 min-h-[4.2rem] dark:text-slate-300">
          {excerpt || "Tìm hiểu thêm trong bài viết chi tiết."}
        </p>

        <div className="mt-6">
          <Link
            to={`/blogs/${blog.slug}`}
            className="inline-flex items-center gap-2 rounded-full bg-yellow-500 px-6 py-2.5 text-sm font-semibold text-white transition-colors duration-200 hover:bg-yellow-600"
          >
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
            </svg>
            ĐỌC THÊM
          </Link>
        </div>
      </div>
    </article>
  );
};

export default BlogCard;

