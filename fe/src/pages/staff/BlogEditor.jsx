import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createBlog, updateBlog, getBlogById } from "../../services/blogService";
import TipTapEditor from "../../components/common/TipTapEditor";
import SlugInput from "../../components/blog/SlugInput";
import toast from "react-hot-toast";

export default function StaffBlogEditor() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = !!id;

  const [formData, setFormData] = useState({
    title: "",
    slug: "",
    content: "",
    excerpt: "",
    featuredImage: "",
    status: "Draft",
  });
  const [imageFile, setImageFile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (isEdit) {
      loadBlog();
    }
  }, [id, isEdit]);

  const loadBlog = async () => {
    setLoading(true);
    try {
      const response = await getBlogById(id);
      if (response.success && response.data) {
        const blog = response.data;
        setFormData({
          title: blog.title || "",
          slug: blog.slug || "",
          content: blog.content || "",
          excerpt: blog.excerpt || "",
          featuredImage: blog.featuredImage || "",
          status: "Draft", // Staff can only have Draft
        });
      }
    } catch (error) {
      console.error("Error loading blog:", error);
      toast.error("Không thể tải blog");
      navigate("/staff/blogs");
    } finally {
      setLoading(false);
    }
  };

  const handleImagePick = (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    if (!file.type.startsWith("image/")) {
      toast.error("Vui lòng chọn file ảnh");
      return;
    }
    setImageFile(file);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!formData.title.trim() || !formData.content.trim()) {
      toast.error("Vui lòng điền đầy đủ thông tin");
      return;
    }

    // Force status to Draft for Staff
    const submitData = { ...formData, status: "Draft" };

    setSaving(true);
    try {
      if (isEdit) {
        await updateBlog(id, submitData, imageFile);
        toast.success("Cập nhật blog thành công");
      } else {
        const response = await createBlog(submitData, imageFile);
        toast.success("Tạo blog thành công");
        if (response.success && response.data) {
          navigate(`/staff/blogs/${response.data.blogId}/edit`);
          return;
        }
      }
      navigate("/staff/blogs");
    } catch (error) {
      console.error("Error saving blog:", error);
      toast.error(isEdit ? "Không thể cập nhật blog" : "Không thể tạo blog");
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="p-6 bg-gray-50 dark:bg-gray-900 min-h-screen">
      <div className="max-w-4xl mx-auto">
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6">
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white mb-6">
            {isEdit ? "Chỉnh sửa Blog" : "Tạo Blog Mới"}
          </h1>

          <div className="bg-yellow-50 dark:bg-yellow-900/20 border border-yellow-200 dark:border-yellow-800 rounded-lg p-4 mb-6">
            <p className="text-sm text-yellow-800 dark:text-yellow-200">
              ⚠️ Lưu ý: Blog của bạn sẽ ở trạng thái Draft và cần được Admin hoặc Doctor duyệt để xuất bản.
            </p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Tiêu đề *
              </label>
              <input
                type="text"
                value={formData.title}
                onChange={(e) => setFormData((prev) => ({ ...prev, title: e.target.value }))}
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
                required
                maxLength={500}
              />
            </div>

            <SlugInput
              value={formData.slug}
              onChange={(slug) => setFormData((prev) => ({ ...prev, slug }))}
              title={formData.title}
              blogId={isEdit ? parseInt(id) : null}
            />

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Tóm tắt
              </label>
              <textarea
                value={formData.excerpt}
                onChange={(e) => setFormData((prev) => ({ ...prev, excerpt: e.target.value }))}
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
                rows="3"
                maxLength={1000}
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Hình ảnh đại diện
              </label>
              <div className="flex items-start gap-4 mb-2">
                {(imageFile || formData.featuredImage) && (
                  <div className="flex-shrink-0">
                    <img
                      src={imageFile ? URL.createObjectURL(imageFile) : formData.featuredImage}
                      alt="Preview"
                      className="w-28 h-16 object-cover rounded-md border border-gray-200 dark:border-gray-700"
                    />
                  </div>
                )}
                {!imageFile && !formData.featuredImage && (
                  <div className="w-28 h-16 rounded-md border border-dashed border-gray-300 dark:border-gray-700 flex items-center justify-center text-xs text-gray-400 dark:text-gray-300 flex-shrink-0">
                    Preview
                  </div>
                )}
                <div className="flex-1 space-y-2">
                  <div className="flex items-center gap-2">
                    <label className="inline-flex items-center px-3 py-2 rounded-lg bg-gray-100 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 cursor-pointer text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-200 dark:hover:bg-gray-700">
                      Chọn ảnh
                      <input
                        type="file"
                        accept="image/*"
                        className="hidden"
                        onChange={handleImagePick}
                      />
                    </label>
                    {imageFile && (
                      <button
                        type="button"
                        onClick={() => setImageFile(null)}
                        className="px-3 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 text-sm"
                      >
                        Xóa
                      </button>
                    )}
                  </div>
                  <input
                    type="text"
                    value={imageFile ? imageFile.name : (formData.featuredImage || "")}
                    readOnly={!!imageFile}
                    onChange={(e) => setFormData((prev) => ({ ...prev, featuredImage: e.target.value }))}
                    className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white text-sm"
                    placeholder={imageFile ? "File sẽ được upload khi lưu" : "Hoặc nhập URL ảnh (tùy chọn)"}
                  />
                </div>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Nội dung *
              </label>
              <TipTapEditor
                value={formData.content}
                onChange={(content) => setFormData((prev) => ({ ...prev, content }))}
              />
            </div>

            <div className="flex gap-4">
              <button
                type="submit"
                disabled={saving}
                className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
              >
                {saving ? "Đang lưu..." : isEdit ? "Cập nhật" : "Tạo mới"}
              </button>
              <button
                type="button"
                onClick={() => navigate("/staff/blogs")}
                className="px-4 py-2 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
              >
                Hủy
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

