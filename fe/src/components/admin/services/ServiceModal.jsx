import React, { useState, useEffect } from "react";
import api from "../../../utils/api";

const ServiceModal = ({ open, service, onClose, onSave }) => {
  const [form, setForm] = useState({
    serviceName: "",
    description: "",
    price: "",
    duration: "",
    specializationId: "",
    isActive: true,
  });

  const [specializations, setSpecializations] = useState([]);

  useEffect(() => {
    const fetchSpecializations = async () => {
      try {
        const res = await api.get("/specializations");
        const rows = Array.isArray(res.data)
          ? res.data
          : res?.data?.data ?? res?.data?.items ?? [];
        setSpecializations(rows);
      } catch (e) {
        // ignore silently
      }
    };
    fetchSpecializations();
  }, []);

  useEffect(() => {
    if (service) {
      setForm({
        serviceName: service.name || service.serviceName || "",
        description: service.description || "",
        price: service.minPrice || service.price || "",
        duration: service.minDuration || service.duration || "",
        specializationId: service.specializationId || "",
        isActive: service.isActive !== undefined ? service.isActive : true,
      });
    } else {
      setForm({
        serviceName: "",
        description: "",
        price: "",
        duration: "",
        specializationId: "",
        isActive: true,
      });
    }
  }, [service]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[500px] max-h-[90vh] overflow-y-auto">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          {service ? "Sửa dịch vụ" : "Tạo dịch vụ mới"}
        </h2>

        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Tên dịch vụ *
            </label>
            <input
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.serviceName}
              onChange={(e) =>
                setForm((f) => ({ ...f, serviceName: e.target.value }))
              }
              placeholder="Nhập tên dịch vụ"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Mô tả
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.description}
              onChange={(e) =>
                setForm((f) => ({ ...f, description: e.target.value }))
              }
              placeholder="Nhập mô tả dịch vụ"
              rows={3}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Giá (VND)
              </label>
              <input
                type="number"
                min="0"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.price}
                onChange={(e) =>
                  setForm((f) => ({ ...f, price: e.target.value }))
                }
                placeholder="Nhập giá dịch vụ"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Thời gian (phút)
              </label>
              <input
                type="number"
                min="0"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.duration}
                onChange={(e) =>
                  setForm((f) => ({ ...f, duration: e.target.value }))
                }
                placeholder="Nhập thời gian"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Chuyên khoa *
            </label>
            <select
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.specializationId}
              onChange={(e) =>
                setForm((f) => ({ ...f, specializationId: e.target.value }))
              }
              required
            >
              <option value="">-- Chọn chuyên khoa --</option>
              {specializations.map((s) => (
                <option key={s.id} value={s.id}>
                  {s.specializationName || s.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="flex items-center space-x-2">
              <input
                type="checkbox"
                className="rounded border-gray-300 dark:border-gray-600 text-blue-600 focus:ring-blue-500 dark:bg-gray-700"
                checked={form.isActive}
                onChange={(e) =>
                  setForm((f) => ({ ...f, isActive: e.target.checked }))
                }
              />
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300">
                Dịch vụ hoạt động
              </span>
            </label>
          </div>
        </div>

        <div className="flex justify-end gap-2 mt-6">
          <button
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium transition"
            onClick={() => onSave(form)}
          >
            Lưu
          </button>
          <button
            className="bg-gray-300 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 px-4 py-2 rounded-md font-medium transition"
            onClick={onClose}
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default ServiceModal;
