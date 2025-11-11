import { Link } from "react-router-dom";
import { format } from "date-fns";

const BlogCard = ({ blog }) => {
  const formatDate = (date) => {
    if (!date) return "";
    try {
      return format(new Date(date), "dd/MM/yyyy");
    } catch {
      return "";
    }
  };

  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md hover:shadow-lg transition-shadow overflow-hidden">
      {blog.featuredImage && (
        <div className="h-48 overflow-hidden">
          <img
            src={blog.featuredImage}
            alt={blog.title}
            className="w-full h-full object-cover"
          />
        </div>
      )}
      <div className="p-6">
        <div className="flex items-center justify-between mb-2">
          <span className="text-xs text-gray-500 dark:text-gray-400">
            {formatDate(blog.publishedAt || blog.createdAt)}
          </span>
          {blog.status && (
            <span
              className={`px-2 py-1 text-xs rounded ${
                blog.status === "Published"
                  ? "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200"
                  : blog.status === "Draft"
                  ? "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200"
                  : "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200"
              }`}
            >
              {blog.status}
            </span>
          )}
        </div>
        <Link to={`/blogs/${blog.slug}`}>
          <h3 className="text-xl font-bold text-gray-900 dark:text-white mb-2 hover:text-blue-600 dark:hover:text-blue-400 transition-colors">
            {blog.title}
          </h3>
        </Link>
        {blog.excerpt && (
          <p className="text-gray-600 dark:text-gray-300 mb-4 line-clamp-3">
            {blog.excerpt}
          </p>
        )}
        <div className="flex items-center justify-between text-sm text-gray-500 dark:text-gray-400">
          <div className="flex items-center gap-4">
            {blog.authorName && (
              <span>Tác giả: {blog.authorName}</span>
            )}
            <span>👁️ {blog.viewCount || 0}</span>
            <span>💬 {blog.commentCount || 0}</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default BlogCard;

