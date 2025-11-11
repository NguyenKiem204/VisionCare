import React, { useEffect, useState } from "react";
import { getMyAbsences, createMyAbsence } from "../../services/doctorMeAPI";

const DoctorAbsences = () => {
  const [list, setList] = useState([]);
  const [form, setForm] = useState({ startDate: "", endDate: "", absenceType: "Leave", reason: "" });
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const res = await getMyAbsences();
      setList(res);
    } catch (e) {
      console.error("Load absences error", e);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const onSubmit = async (e) => {
    e.preventDefault();
    await createMyAbsence({
      startDate: form.startDate,
      endDate: form.endDate,
      absenceType: form.absenceType,
      reason: form.reason,
    });
    setForm({ startDate: "", endDate: "", absenceType: "Leave", reason: "" });
    await load();
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Đơn nghỉ phép</h1>
        <p className="text-gray-600 dark:text-gray-300">Tạo và xem các đơn nghỉ của bạn</p>
      </div>

      <form onSubmit={onSubmit} className="bg-white dark:bg-gray-800 shadow rounded-lg p-6 space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-200">Từ ngày</label>
            <input type="date" className="mt-1 block w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" value={form.startDate} onChange={(e) => setForm({ ...form, startDate: e.target.value })} required />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-200">Đến ngày</label>
            <input type="date" className="mt-1 block w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" value={form.endDate} onChange={(e) => setForm({ ...form, endDate: e.target.value })} required />
          </div>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-200">Loại</label>
            <select className="mt-1 block w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" value={form.absenceType} onChange={(e) => setForm({ ...form, absenceType: e.target.value })}>
              <option value="Leave">Leave</option>
              <option value="Sick">Sick</option>
              <option value="Emergency">Emergency</option>
              <option value="Other">Other</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-200">Lý do</label>
            <input className="mt-1 block w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" value={form.reason} onChange={(e) => setForm({ ...form, reason: e.target.value })} placeholder="Lý do" />
          </div>
        </div>
        <div>
          <button className="px-4 py-2 bg-indigo-600 text-white rounded-md">Gửi đơn</button>
        </div>
      </form>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">Danh sách đơn nghỉ</h3>
        {loading && <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>}
        {!loading && (
          <ul className="divide-y divide-gray-200 dark:divide-gray-700">
            {(list || []).map((a) => (
              <li key={a.id} className="py-3 flex items-center justify-between">
                <div>
                  <div className="text-sm font-medium text-gray-900 dark:text-white">{a.absenceType} · {a.startDate} → {a.endDate}</div>
                  <div className="text-sm text-gray-500 dark:text-gray-300">{a.reason}</div>
                </div>
                <span className="text-xs px-2 py-0.5 rounded bg-gray-100 dark:bg-gray-700 text-gray-800 dark:text-gray-100">{a.status}</span>
              </li>
            ))}
            {(!list || list.length === 0) && (
              <li className="py-3 text-sm text-gray-500 dark:text-gray-300">Chưa có đơn nghỉ</li>
            )}
          </ul>
        )}
      </div>
    </div>
  );
};

export default DoctorAbsences;


