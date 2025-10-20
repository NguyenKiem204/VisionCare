import React from "react";

export default function NewsFilter({ filter, setFilter, onFilter }) {
  return (
    <form onSubmit={onFilter} style={{ marginBottom: 16 }}>
      <input
        name="categoryId"
        value={filter.categoryId}
        onChange={(e) =>
          setFilter((f) => ({ ...f, categoryId: e.target.value }))
        }
        placeholder="Lọc theo danh mục"
      />
      <select
        name="status"
        value={filter.status}
        onChange={(e) => setFilter((f) => ({ ...f, status: e.target.value }))}
      >
        <option value="">Tất cả trạng thái</option>
        <option value="DRAFT">Nháp</option>
        <option value="PUBLISHED">Công khai</option>
        <option value="ARCHIVED">Lưu trữ</option>
      </select>
      <button type="submit">Lọc</button>
    </form>
  );
}
