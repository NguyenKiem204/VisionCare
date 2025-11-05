import React from "react";
import { Edit, Trash2, Eye } from "lucide-react";

const WorkShiftTable = ({ workShifts, loading, onEdit, onDelete, onView }) => {
  const formatTime = (time) => {
    if (!time) return "N/A";
    if (typeof time === 'string') {
      return time.substring(0, 5); // HH:mm
    }
    return time;
  };

  if (loading) {
    return (
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <div className="p-8 text-center text-gray-500 dark:text-gray-400">Đang tải...</div>
      </div>
    );
  }

  if (!workShifts || workShifts.length === 0) {
    return (
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow">
        <div className="p-8 text-center text-gray-500 dark:text-gray-400">Không có dữ liệu</div>
      </div>
    );
  }

  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow overflow-hidden">
      <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
        <thead className="bg-gray-50 dark:bg-gray-900">
          <tr>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">ID</th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">Tên ca</th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">Giờ bắt đầu</th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">Giờ kết thúc</th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">Thời lượng</th>
            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">Trạng thái</th>
            <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase">Thao tác</th>
          </tr>
        </thead>
        <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
          {workShifts.map((shift) => (
            <tr key={shift.id} className="hover:bg-gray-50 dark:hover:bg-gray-700">
              <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">{shift.id}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900 dark:text-gray-100">{shift.shiftName}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">{formatTime(shift.startTime)}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">{formatTime(shift.endTime)}</td>
              <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">{shift.durationMinutes} phút</td>
              <td className="px-6 py-4 whitespace-nowrap">
                <span className={`px-2 py-1 text-xs font-semibold rounded-full ${shift.isActive ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200' : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'}`}>
                  {shift.isActive ? "Hoạt động" : "Không hoạt động"}
                </span>
              </td>
              <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                <div className="flex justify-end gap-2">
                  <button onClick={() => onView(shift)} className="text-blue-600 hover:text-blue-900 dark:text-blue-400" title="Xem"><Eye size={18} /></button>
                  <button onClick={() => onEdit(shift)} className="text-yellow-600 hover:text-yellow-900 dark:text-yellow-400" title="Sửa"><Edit size={18} /></button>
                  <button onClick={() => onDelete(shift.id)} className="text-red-600 hover:text-red-900 dark:text-red-400" title="Xóa"><Trash2 size={18} /></button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default WorkShiftTable;

