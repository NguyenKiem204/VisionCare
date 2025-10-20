import React, { useState, useEffect } from "react";
import ReactQuill from "react-quill-new";
import "react-quill-new/dist/quill.snow.css";
import "./NewsModal.css";

const modules = {
  toolbar: [
    [{ header: [1, 2, 3, false] }],
    ["bold", "italic", "underline", "strike"],
    [{ list: "ordered" }, { list: "bullet" }],
    [{ color: [] }, { background: [] }],
    [{ align: [] }],
    ["link", "image"],
    ["clean"],
  ],
};

const formats = [
  "header",
  "bold",
  "italic",
  "underline",
  "strike",
  "list",
  "bullet",
  "color",
  "background",
  "align",
  "link",
  "image",
];

export default function NewsModal({
  form,
  setForm,
  isEdit,
  onSubmit,
  onCancel,
  loading,
  hideTitle,
}) {
  const [errors, setErrors] = useState({});

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm({ ...form, [name]: value });
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors((prev) => ({ ...prev, [name]: "" }));
    }
  };

  const handleContentChange = (value) => {
    setForm({ ...form, content: value });
    if (errors.content) {
      setErrors((prev) => ({ ...prev, content: "" }));
    }
  };

  const validateForm = () => {
    const newErrors = {};
    if (!form.title?.trim()) {
      newErrors.title = "Tiêu đề là bắt buộc";
    }
    if (!form.content?.trim()) {
      newErrors.content = "Nội dung là bắt buộc";
    }
    if (!form.categoryId?.trim()) {
      newErrors.categoryId = "Danh mục là bắt buộc";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validateForm()) {
      onSubmit(e);
    }
  };

  return (
    <div className="w-full max-w-2xl mx-auto">
      {!hideTitle && (
        <h2 className="text-2xl font-bold mb-8 text-gray-900 dark:text-white text-center">
          {isEdit ? "Chỉnh sửa tin tức" : "Tạo tin tức mới"}
        </h2>
      )}
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Title */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            Tiêu đề <span className="text-red-500">*</span>
          </label>
          <input
            name="title"
            value={form.title || ""}
            onChange={handleChange}
            placeholder="Nhập tiêu đề tin tức"
            className={`w-full px-4 py-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white dark:border-gray-600 ${
              errors.title
                ? "border-red-500"
                : "border-gray-300 dark:border-gray-600"
            } text-base`}
            required
          />
          {errors.title && (
            <p className="mt-1 text-sm text-red-500">{errors.title}</p>
          )}
        </div>
        {/* Slug */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            Slug
          </label>
          <input
            name="slug"
            value={form.slug || ""}
            onChange={handleChange}
            placeholder="tin-tuc-moi"
            className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white text-base"
          />
        </div>
        {/* Summary */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            Tóm tắt
          </label>
          <textarea
            name="summary"
            value={form.summary || ""}
            onChange={handleChange}
            placeholder="Nhập tóm tắt tin tức"
            rows={3}
            className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white resize-none text-base"
          />
        </div>
        {/* Content */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            Nội dung <span className="text-red-500">*</span>
          </label>
          <div
            className={`w-full border rounded-lg ${
              errors.content
                ? "border-red-500"
                : "border-gray-300 dark:border-gray-600"
            }`}
          >
            <ReactQuill
              value={form.content || ""}
              onChange={handleContentChange}
              modules={modules}
              formats={formats}
              placeholder="Nhập nội dung tin tức..."
              className="dark:text-white w-full"
            />
          </div>
          {errors.content && (
            <p className="mt-1 text-sm text-red-500">{errors.content}</p>
          )}
        </div>
        {/* Image URL */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            Ảnh đại diện
          </label>
          <input
            name="imageUrl"
            value={form.imageUrl || ""}
            onChange={handleChange}
            placeholder="https://example.com/image.jpg"
            className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white text-base"
          />
        </div>
        {/* Status */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            Trạng thái
          </label>
          <select
            name="status"
            value={form.status || "DRAFT"}
            onChange={handleChange}
            className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white text-base"
          >
            <option value="DRAFT">Nháp</option>
            <option value="PUBLISHED">Công khai</option>
            <option value="ARCHIVED">Lưu trữ</option>
          </select>
        </div>
        {/* Category ID */}
        <div className="w-full">
          <label className="block text-base font-medium text-gray-700 dark:text-gray-300 mb-2">
            ID Danh mục <span className="text-red-500">*</span>
          </label>
          <input
            name="categoryId"
            value={form.categoryId || ""}
            onChange={handleChange}
            className={`w-full px-4 py-3 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-white dark:border-gray-600 ${
              errors.categoryId
                ? "border-red-500"
                : "border-gray-300 dark:border-gray-600"
            } text-base`}
            required
          />
          {errors.categoryId && (
            <p className="mt-1 text-sm text-red-500">{errors.categoryId}</p>
          )}
        </div>
        {/* Buttons */}
        <div className="flex justify-end gap-3 pt-6">
          <button
            type="button"
            onClick={onCancel}
            className="px-5 py-2.5 text-gray-700 dark:text-gray-300 bg-gray-200 dark:bg-gray-700 rounded-lg hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors text-base"
          >
            Huỷ
          </button>
          <button
            type="submit"
            disabled={loading}
            className="px-5 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors text-base font-semibold"
          >
            {loading ? "Đang xử lý..." : isEdit ? "Cập nhật" : "Tạo mới"}
          </button>
        </div>
      </form>
    </div>
  );
}
