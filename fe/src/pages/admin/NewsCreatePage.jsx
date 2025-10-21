import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createNews } from "../../services/adminNewsAPI";
import NewsModal from "../../components/admin/news/NewsModal";

const initialForm = {
  title: "",
  slug: "",
  summary: "",
  content: "",
  imageUrl: "",
  status: "DRAFT",
  categoryId: "",
};

export default function NewsCreatePage() {
  const [form, setForm] = useState(initialForm);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSave = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await createNews(form);
      navigate("/admin/news");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-6xl mx-auto px-4 py-8 bg-white dark:bg-gray-900 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-8">
      <div className="mb-6">
        <button
          className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition font-semibold flex items-center gap-2 shadow mb-3"
          onClick={() => navigate("/admin/news")}
        >
          <span className="text-lg">←</span> Quay lại
        </button>
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white text-center">
          Tạo tin tức mới
        </h1>
      </div>
      <NewsModal
        form={form}
        setForm={setForm}
        isEdit={false}
        onSubmit={handleSave}
        onCancel={() => navigate("/admin/news")}
        loading={loading}
        hideTitle={true}
      />
    </div>
  );
}
