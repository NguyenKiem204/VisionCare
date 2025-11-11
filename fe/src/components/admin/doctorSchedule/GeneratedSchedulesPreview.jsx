import React, { useMemo, useState } from "react";

// shifts: [{ shiftName, startTime, endTime }] start/end as "HH:mm"
export default function GeneratedSchedulesPreview({ open, onClose, items = [], title = "Lịch cụ thể" }) {
  const [statusFilter, setStatusFilter] = useState("all"); // 'all' | 'Available' | 'Booked' | 'Blocked'
  const [compact, setCompact] = useState(true); // true: gộp theo ngày, false: chi tiết slot

  const filteredItems = useMemo(() => {
    if (statusFilter === "all") return items;
    return items.filter(s => (s.status || "").toLowerCase() === statusFilter.toLowerCase());
  }, [items, statusFilter]);
  if (!open) return null;
  return (
    <div className="fixed inset-0 bg-black/40 z-50 flex items-center justify-center p-4">
      <div className="w-full max-w-3xl bg-white dark:bg-gray-800 rounded-xl shadow-xl overflow-hidden">
        <div className="flex items-center justify-between px-5 py-3 border-b dark:border-gray-700">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white">{title}</h3>
          <button onClick={onClose} className="text-gray-500 hover:text-gray-700 dark:text-gray-300">×</button>
        </div>
        <div className="max-h-[70vh] overflow-y-auto custom-scrollbar">
          <div className="px-5 py-3 flex flex-wrap gap-3 items-center text-sm border-b dark:border-gray-700 bg-white/60 dark:bg-gray-900/60 sticky top-0 z-10 backdrop-blur">
            <div className="flex items-center gap-2">
              <span className="text-gray-600 dark:text-gray-300">Hiển thị:</span>
              <button onClick={() => setCompact(true)} className={`px-3 py-1 rounded ${compact ? 'bg-blue-600 text-white' : 'bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300'}`}>Tổng hợp theo ngày</button>
              <button onClick={() => setCompact(false)} className={`px-3 py-1 rounded ${!compact ? 'bg-blue-600 text-white' : 'bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300'}`}>Chi tiết slot</button>
            </div>
            <div className="flex items-center gap-2">
              <span className="text-gray-600 dark:text-gray-300">Trạng thái:</span>
              <select value={statusFilter} onChange={(e)=>setStatusFilter(e.target.value)} className="px-2 py-1 rounded border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200">
                <option value="all">Tất cả</option>
                <option value="Available">Available</option>
                <option value="Booked">Booked</option>
                <option value="Blocked">Blocked</option>
              </select>
            </div>
            <div className="ml-auto flex items-center gap-2 text-xs">
              <span className="px-2 py-0.5 rounded bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200">Available</span>
              <span className="px-2 py-0.5 rounded bg-amber-100 text-amber-800 dark:bg-amber-900 dark:text-amber-200">Booked</span>
              <span className="px-2 py-0.5 rounded bg-gray-200 text-gray-800 dark:bg-gray-700 dark:text-gray-200">Blocked</span>
            </div>
          </div>
          {filteredItems.length === 0 ? (
            <div className="p-6 text-center text-gray-500 dark:text-gray-400">Không có dữ liệu</div>
          ) : compact ? (
            <table className="w-full text-sm">
              <thead className="bg-gray-50 dark:bg-gray-900/60 text-gray-700 dark:text-gray-300 sticky top-[52px] z-10">
                <tr>
                  <th className="text-left px-4 py-2">Ngày</th>
                  <th className="text-left px-4 py-2">Khoảng giờ (min–max)</th>
                  <th className="text-left px-4 py-2">Số slot</th>
                </tr>
              </thead>
              <tbody>
                {Object.values(
                  filteredItems.reduce((acc, s) => {
                    const key = new Date(s.scheduleDate).toISOString().substring(0,10);
                    const start = (s.startTime || "").substring(0,5);
                    const end = (s.endTime || "").substring(0,5);
                    if (!acc[key]) acc[key] = { day: key, starts: [], ends: [], count: 0 };
                    acc[key].starts.push(start);
                    acc[key].ends.push(end);
                    acc[key].count += 1;
                    return acc;
                  }, {})
                ).sort((a,b)=>a.day.localeCompare(b.day)).map(g => {
                  const starts = g.starts.filter(Boolean).sort();
                  const ends = g.ends.filter(Boolean).sort();
                  const min = starts[0] || "--:--";
                  const max = ends[ends.length-1] || "--:--";
                  return (
                    <tr key={g.day} className="border-t dark:border-gray-700 odd:bg-white even:bg-gray-50 dark:odd:bg-gray-900 dark:even:bg-gray-800/60 text-gray-900 dark:text-gray-100">
                      <td className="px-4 py-2">{new Date(g.day).toLocaleDateString("vi-VN")}</td>
                      <td className="px-4 py-2">{min} - {max}</td>
                      <td className="px-4 py-2">{g.count}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          ) : (
            <table className="w-full text-sm">
              <thead className="bg-gray-50 dark:bg-gray-900/60 text-gray-700 dark:text-gray-300 sticky top-[52px] z-10">
                <tr>
                  <th className="text-left px-4 py-2">Ngày</th>
                  <th className="text-left px-4 py-2">Giờ</th>
                  <th className="text-left px-4 py-2">Trạng thái</th>
                </tr>
              </thead>
              <tbody>
                {filteredItems.map((s) => {
                  const status = (s.status || "").toLowerCase();
                  const badgeCls = status === 'available' ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'
                                   : status === 'booked' ? 'bg-amber-100 text-amber-800 dark:bg-amber-900 dark:text-amber-200'
                                   : 'bg-gray-200 text-gray-800 dark:bg-gray-700 dark:text-gray-200';
                  return (
                  <tr key={s.id} className="border-t dark:border-gray-700 odd:bg-white even:bg-gray-50 dark:odd:bg-gray-900 dark:even:bg-gray-800/60 text-gray-900 dark:text-gray-100">
                    <td className="px-4 py-2 text-gray-900 dark:text-gray-100">{new Date(s.scheduleDate).toLocaleDateString("vi-VN")}</td>
                    <td className="px-4 py-2 text-gray-900 dark:text-gray-100">{(s.startTime || "").substring(0,5)} - {(s.endTime || "").substring(0,5)}</td>
                    <td className="px-4 py-2"><span className={`px-2 py-0.5 rounded text-xs ${badgeCls}`}>{s.status}</span></td>
                  </tr>
                )})}
              </tbody>
            </table>
          )}
        </div>
        <div className="px-5 py-3 border-t dark:border-gray-700 flex justify-end">
          <button onClick={onClose} className="px-4 py-2 bg-blue-600 text-white rounded-lg">Đóng</button>
        </div>
      </div>
    </div>
  );
}


