import React, { useState, useEffect } from "react";

const UserModal = ({ open, user, onClose, onSave }) => {
  const [form, setForm] = useState({
    email: "",
    role: "",
    status: "",
    residentId: "",
  });

  useEffect(() => {
    if (user) setForm(user);
    else setForm({ email: "", role: "", status: "", residentId: "" });
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
          value={form.email}
          onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))}
          placeholder="Email"
        />
        <input
          className="border px-2 py-1 mb-2 w-full"
          value={form.residentId}
          onChange={(e) =>
            setForm((f) => ({ ...f, residentId: e.target.value }))
          }
          placeholder="Resident ID"
        />
        <select
          className="border px-2 py-1 mb-2 w-full"
          value={form.role}
          onChange={(e) => setForm((f) => ({ ...f, role: e.target.value }))}
        >
          <option value="">Chọn role</option>
          <option value="ADMIN">ADMIN</option>
          <option value="USER">USER</option>
        </select>
        <select
          className="border px-2 py-1 mb-2 w-full"
          value={form.status}
          onChange={(e) => setForm((f) => ({ ...f, status: e.target.value }))}
        >
          <option value="">Chọn status</option>
          <option value="ACTIVE">ACTIVE</option>
          <option value="INACTIVE">INACTIVE</option>
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
