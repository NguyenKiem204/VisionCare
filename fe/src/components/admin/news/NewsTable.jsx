import React from "react";
import { formatDateTime } from "../../../utils/formatDate";

export default function NewsTable({
  newsList,
  onEdit,
  onDelete,
  onChangeStatus,
  showDeleteId,
  setShowDeleteId,
  loading,
}) {
  return (
    <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700 rounded-xl overflow-hidden shadow-lg">
      <thead className="bg-gray-100 dark:bg-gray-800">
        <tr>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            ID
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Tiêu đề
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Slug
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Trạng thái
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Danh mục
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Tác giả
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Ngày tạo
          </th>
          <th className="px-4 py-3 text-left text-xs font-semibold text-gray-700 dark:text-gray-200 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
            Hành động
          </th>
        </tr>
      </thead>
      <tbody>
        {loading ? (
          <tr>
            <td colSpan="8" className="text-center py-6">
              Đang tải...
            </td>
          </tr>
        ) : newsList.length === 0 ? (
          <tr>
            <td colSpan="8" className="text-center py-6 text-gray-400">
              Không có dữ liệu
            </td>
          </tr>
        ) : (
          newsList.map((news, idx) => (
            <React.Fragment key={news.id}>
              <tr
                className={`text-sm ${
                  idx % 2 === 0
                    ? "bg-white dark:bg-gray-900"
                    : "bg-gray-50 dark:bg-gray-800"
                } border-b border-gray-100 dark:border-gray-800 hover:bg-blue-50 dark:hover:bg-gray-700 transition-colors duration-200`}
              >
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                  {news.id}
                </td>
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100 font-semibold">
                  {news.title}
                </td>
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                  {news.slug}
                </td>
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                  {news.status}
                </td>
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                  {news.categoryName || news.categoryId}
                </td>
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                  {news.authorName || news.authorId}
                </td>
                <td className="px-4 py-2 text-gray-800 dark:text-gray-100">
                  {formatDateTime(news.createdAt)}
                </td>
                <td className="px-4 py-2 flex gap-2">
                  <button
                    className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                    onClick={() => onEdit(news)}
                  >
                    Sửa
                  </button>
                  <button
                    className="bg-gray-200 dark:bg-gray-700 dark:text-white hover:bg-red-500 hover:text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                    onClick={() => setShowDeleteId(news.id)}
                  >
                    Xóa
                  </button>
                  <button
                    className="bg-yellow-500 hover:bg-yellow-600 text-white px-3 py-1 rounded shadow text-xs font-semibold transition"
                    onClick={() =>
                      onChangeStatus(
                        news.id,
                        news.status === "PUBLISHED" ? "ARCHIVED" : "PUBLISHED"
                      )
                    }
                  >
                    {news.status === "PUBLISHED" ? "Lưu trữ" : "Công khai"}
                  </button>
                </td>
              </tr>
              {showDeleteId === news.id && (
                <tr>
                  <td
                    colSpan="8"
                    className="bg-white dark:bg-gray-900 text-center py-4 border-b border-gray-200 dark:border-gray-800"
                  >
                    <div className="inline-block p-4 border border-red-400 bg-red-50 dark:bg-gray-800 rounded">
                      <div className="mb-2 text-red-700 dark:text-red-300">
                        Xác nhận xoá tin "{news.title}"?
                      </div>
                      <button
                        className="bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded shadow text-xs font-semibold transition mr-2"
                        onClick={() => onDelete(news.id)}
                      >
                        Xoá
                      </button>
                      <button
                        className="bg-gray-300 hover:bg-gray-400 text-gray-800 px-3 py-1 rounded shadow text-xs font-semibold transition"
                        onClick={() => setShowDeleteId(null)}
                      >
                        Huỷ
                      </button>
                    </div>
                  </td>
                </tr>
              )}
            </React.Fragment>
          ))
        )}
      </tbody>
    </table>
  );
}
