import React, { useState, useEffect } from "react";

const UserModal = ({ open, user, onClose, onSave }) => {
  const [form, setForm] = useState({
    username: "",
    email: "",
    password: "",
    roleId: "",
  });

  useEffect(() => {
    if (user) {
      setForm({
        username: user.username || "",
        email: user.email || "",
        password: "",
        roleId: user.roleId || "",
      });
    } else {
      setForm({ username: "", email: "", password: "", roleId: "" });
    }
  }, [user]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white p-6 rounded shadow w-80">
        <h2 className="text-lg font-bold mb-2">
          {user ? "Sửa user" : "Tạo user mới"}
        </h2>
        <input
          className="border px-2 py-1 mb-2 w-full"
          value={form.username}
          onChange={(e) => setForm((f) => ({ ...f, username: e.target.value }))}
          placeholder="Username"
          required
        />
        <input
          className="border px-2 py-1 mb-2 w-full"
          value={form.email}
          onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))}
          placeholder="Email"
          type="email"
          required
        />
        <input
          className="border px-2 py-1 mb-2 w-full"
          value={form.password}
          onChange={(e) => setForm((f) => ({ ...f, password: e.target.value }))}
          placeholder={user ? "Mật khẩu mới (để trống nếu không đổi)" : "Mật khẩu"}
          type="password"
          required={!user}
        />
        <select
          className="border px-2 py-1 mb-2 w-full"
          value={form.roleId}
          onChange={(e) => setForm((f) => ({ ...f, roleId: e.target.value }))}
          required
        >
          <option value="">Chọn role</option>
          <option value="1">ADMIN</option>
          <option value="2">USER</option>
        </select>
        <div className="flex justify-end gap-2 mt-2">
          <button
            className="bg-blue-600 text-white px-3 py-1 rounded"
            onClick={() => onSave(form)}
          >
            Lưu
          </button>
          <button className="bg-gray-300 px-3 py-1 rounded" onClick={onClose}>
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default UserModal;
