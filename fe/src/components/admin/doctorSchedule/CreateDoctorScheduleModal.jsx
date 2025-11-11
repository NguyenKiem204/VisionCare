import React, { useState, useEffect } from "react";
import { X } from "lucide-react";
import { searchDoctors } from "../../../services/adminDoctorAPI";
import { getActiveWorkShifts } from "../../../services/adminWorkShiftAPI";
import { getActiveRooms } from "../../../services/adminRoomAPI";
import { fetchEquipment } from "../../../services/adminEquipmentAPI";
import toast from "react-hot-toast";

const CreateDoctorScheduleModal = ({ open, onClose, onSave, doctorId: initialDoctorId }) => {
  const [form, setForm] = useState({
    doctorId: initialDoctorId || "",
    shiftId: "",
    roomId: "",
    equipmentId: "",
    startDate: "",
    endDate: "",
    dayOfWeek: "",
    recurrenceRule: "WEEKLY",
    isActive: true,
  });

  const [doctors, setDoctors] = useState([]);
  const [workShifts, setWorkShifts] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [equipment, setEquipment] = useState([]);
  const [loading, setLoading] = useState(false);
  const [batchItems, setBatchItems] = useState([]);

  useEffect(() => {
    if (open) {
      loadOptions();
      if (initialDoctorId) {
        setForm((prev) => ({ ...prev, doctorId: initialDoctorId }));
      }
    }
  }, [open, initialDoctorId]);

  const loadOptions = async () => {
    setLoading(true);
    try {
      const [doctorsRes, shiftsRes, roomsRes, equipmentRes] = await Promise.all([
        searchDoctors({ page: 1, pageSize: 100 }),
        getActiveWorkShifts(),
        getActiveRooms(),
        fetchEquipment(),
      ]);

      setDoctors(doctorsRes?.data?.data || doctorsRes?.data?.items || []);
      setWorkShifts(shiftsRes?.data?.data || shiftsRes?.data || []);
      setRooms(roomsRes?.data?.data || roomsRes?.data || []);
      setEquipment(equipmentRes?.data?.data || equipmentRes?.data || []);
    } catch (error) {
      console.error("Failed to load options:", error);
    } finally {
      setLoading(false);
    }
  };

  const dayOptions = [
    { value: "1", label: "Thứ 2" },
    { value: "2", label: "Thứ 3" },
    { value: "3", label: "Thứ 4" },
    { value: "4", label: "Thứ 5" },
    { value: "5", label: "Thứ 6" },
    { value: "6", label: "Thứ 7" },
    { value: "7", label: "Chủ nhật" },
  ];

  const recurrenceOptions = [
    { value: "DAILY", label: "Hàng ngày" },
    { value: "WEEKLY", label: "Hàng tuần" },
    { value: "MONTHLY", label: "Hàng tháng" },
  ];

  const handleSubmit = (e) => {
    e.preventDefault();
    // If batch has items, allow submit even if current form incomplete
    if (batchItems.length > 0) {
      onSave(batchItems);
      setBatchItems([]);
      return;
    }
    // Otherwise validate single payload
    if (form.recurrenceRule === "WEEKLY" && !form.dayOfWeek) {
      alert("Vui lòng chọn Thứ trong tuần cho lịch Hàng tuần.");
      return;
    }
    const payload = {
      ...form,
      doctorId: Number(form.doctorId),
      shiftId: Number(form.shiftId),
      roomId: form.roomId ? Number(form.roomId) : null,
      equipmentId: form.equipmentId ? Number(form.equipmentId) : null,
      dayOfWeek: form.dayOfWeek ? Number(form.dayOfWeek) : null,
      endDate: form.endDate ? form.endDate : null,
    };
    onSave(payload);
  };

  const addToBatch = () => {
    // Validate inputs before adding
    if (!form.doctorId) {
      toast.error("Vui lòng chọn bác sĩ");
      return;
    }
    if (!form.shiftId) {
      toast.error("Vui lòng chọn ca làm việc");
      return;
    }
    if (!form.startDate) {
      toast.error("Vui lòng chọn ngày bắt đầu");
      return;
    }
    if (form.recurrenceRule === "WEEKLY" && !form.dayOfWeek) {
      toast.error("Vui lòng chọn Thứ trong tuần");
      return;
    }
    const item = {
      doctorId: Number(form.doctorId),
      shiftId: Number(form.shiftId),
      roomId: form.roomId ? Number(form.roomId) : null,
      equipmentId: form.equipmentId ? Number(form.equipmentId) : null,
      startDate: form.startDate,
      endDate: form.endDate ? form.endDate : null,
      dayOfWeek: form.dayOfWeek ? Number(form.dayOfWeek) : null,
      recurrenceRule: form.recurrenceRule,
      isActive: form.isActive,
    };
    setBatchItems((prev) => [...prev, item]);
    toast.success("Đã thêm vào danh sách");
    // Reset a few fields for quick add next item
    setForm((f) => ({
      ...f,
      shiftId: "",
      roomId: "",
      equipmentId: "",
      dayOfWeek: f.recurrenceRule === "WEEKLY" ? "" : f.dayOfWeek,
    }));
  };

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[600px] max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-lg font-bold text-gray-900 dark:text-white">
            Tạo lịch làm việc định kỳ
          </h2>
          <button
            onClick={onClose}
            className="text-gray-500 hover:text-gray-700 dark:text-gray-400"
          >
            <X size={24} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Bác sĩ *
            </label>
            <select
              required={!batchItems.length}
              value={form.doctorId}
              onChange={(e) => setForm((f) => ({ ...f, doctorId: e.target.value }))}
              className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              disabled={!!initialDoctorId}
            >
              <option value="">-- Chọn bác sĩ --</option>
              {doctors.map((d) => (
                <option key={d.doctorId || d.id} value={d.doctorId || d.id}>
                  {d.doctorName || d.fullName} - {d.specializationName || ""}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Ca làm việc *
            </label>
            <select
              required={!batchItems.length}
              value={form.shiftId}
              onChange={(e) => setForm((f) => ({ ...f, shiftId: e.target.value }))}
              className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="">-- Chọn ca làm việc --</option>
              {workShifts.map((s) => (
                <option key={s.id} value={s.id}>
                  {s.shiftName} ({s.startTime?.substring(0, 5)} - {s.endTime?.substring(0, 5)})
                </option>
              ))}
            </select>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Phòng (tùy chọn)
              </label>
              <select
                value={form.roomId}
                onChange={(e) => setForm((f) => ({ ...f, roomId: e.target.value }))}
                className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="">-- Không chọn --</option>
                {rooms.map((r) => (
                  <option key={r.id} value={r.id}>
                    {r.roomName}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Thiết bị (tùy chọn)
              </label>
              <select
                value={form.equipmentId}
                onChange={(e) => setForm((f) => ({ ...f, equipmentId: e.target.value }))}
                className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="">-- Không chọn --</option>
                {equipment.map((e) => (
                  <option key={e.id} value={e.id}>
                    {e.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Ngày bắt đầu *
              </label>
              <input
                type="date"
                required={!batchItems.length}
                value={form.startDate}
                onChange={(e) => setForm((f) => ({ ...f, startDate: e.target.value }))}
                className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Ngày kết thúc (tùy chọn)
              </label>
              <input
                type="date"
                value={form.endDate}
                onChange={(e) => setForm((f) => ({ ...f, endDate: e.target.value }))}
                className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Chu kỳ lặp lại *
              </label>
              <select
                required={!batchItems.length}
                value={form.recurrenceRule}
                onChange={(e) => setForm((f) => ({ ...f, recurrenceRule: e.target.value }))}
                className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                {recurrenceOptions.map((opt) => (
                  <option key={opt.value} value={opt.value}>
                    {opt.label}
                  </option>
                ))}
              </select>
            </div>

            {form.recurrenceRule === "WEEKLY" && (
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Thứ trong tuần
                </label>
                <select
                  required={form.recurrenceRule === "WEEKLY" && !batchItems.length}
                  value={form.dayOfWeek}
                  onChange={(e) => setForm((f) => ({ ...f, dayOfWeek: e.target.value }))}
                  className="w-full border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">-- Chọn thứ --</option>
                  {dayOptions.map((opt) => (
                    <option key={opt.value} value={opt.value}>
                      {opt.label}
                    </option>
                  ))}
                </select>
                <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">Chọn cụ thể một ngày trong tuần để tránh trùng lịch.</p>
              </div>
            )}
          </div>

          {batchItems.length > 0 && (
            <div className="border border-gray-200 dark:border-gray-700 rounded p-3 text-sm">
              <div className="font-medium mb-2">Danh sách sẽ tạo ({batchItems.length}):</div>
              <ul className="list-disc pl-5 space-y-1">
                {batchItems.map((b, idx) => (
                  <li key={idx} className="text-gray-700 dark:text-gray-300">
                    Ca #{idx + 1}: shift {b.shiftId}, {b.recurrenceRule}
                    {b.dayOfWeek ? ` - Thứ ${b.dayOfWeek}` : ""} | {b.startDate}
                    {b.endDate ? ` → ${b.endDate}` : ""}
                  </li>
                ))}
              </ul>
            </div>
          )}

          <div className="text-xs text-gray-600 dark:text-gray-400 bg-gray-50 dark:bg-gray-900/40 border border-gray-200 dark:border-gray-700 rounded p-3">
            - Mỗi mục bên dưới tương ứng với 1 ca lặp lại. Bác sĩ có thể làm nhiều ca bằng cách tạo NHIỀU mục khác nhau (ví dụ: Thứ 2 ca sáng, Thứ 4 ca chiều...).<br />
            - Hệ thống sẽ không cho phép các ca trùng nhau trong cùng khoảng ngày.
          </div>

          <div>
            <label className="flex items-center">
              <input
                type="checkbox"
                checked={form.isActive}
                onChange={(e) => setForm((f) => ({ ...f, isActive: e.target.checked }))}
                className="mr-2"
              />
              <span className="text-sm text-gray-700 dark:text-gray-300">Kích hoạt ngay</span>
            </label>
          </div>

          <div className="flex justify-end gap-2 mt-6">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-200 rounded-md hover:bg-gray-300 dark:hover:bg-gray-600 transition"
            >
              Hủy
            </button>
            <button
              type="button"
              onClick={addToBatch}
              className="px-4 py-2 bg-emerald-600 text-white rounded-md hover:bg-emerald-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
              disabled={!form.doctorId || !form.shiftId || !form.startDate || (form.recurrenceRule === "WEEKLY" && !form.dayOfWeek)}
            >
              Thêm vào danh sách{batchItems.length > 0 ? ` (${batchItems.length})` : ""}
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition"
              disabled={loading}
            >
              {loading ? "Đang tải..." : batchItems.length > 0 ? "Tạo tất cả" : "Tạo mới"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateDoctorScheduleModal;

