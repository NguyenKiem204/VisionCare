import React from "react";
import { X } from "lucide-react";

const RoomDetailModal = ({ isOpen, onClose, room }) => {
  if (!isOpen || !room) return null;

  const getStatusColor = (status) => {
    switch (status?.toLowerCase()) {
      case "active":
        return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200";
      case "inactive":
        return "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300";
      case "maintenance":
        return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200";
      default:
        return "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300";
    }
  };

  const getStatusText = (status) => {
    switch (status?.toLowerCase()) {
      case "active":
        return "Hoạt động";
      case "inactive":
        return "Không hoạt động";
      case "maintenance":
        return "Bảo trì";
      default:
        return status || "Không xác định";
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[600px] max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-lg font-bold text-gray-900 dark:text-white">
            Chi tiết phòng
          </h2>
          <button
            onClick={onClose}
            className="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200"
          >
            <X size={24} />
          </button>
        </div>

        <div className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                ID
              </label>
              <p className="text-gray-900 dark:text-white">{room.id}</p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                Trạng thái
              </label>
              <span
                className={`px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(
                  room.status
                )}`}
              >
                {getStatusText(room.status)}
              </span>
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
              Tên phòng
            </label>
            <p className="text-gray-900 dark:text-white">{room.roomName}</p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
              Mã phòng
            </label>
            <p className="text-gray-900 dark:text-white">{room.roomCode || "N/A"}</p>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                Sức chứa
              </label>
              <p className="text-gray-900 dark:text-white">{room.capacity}</p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                Vị trí
              </label>
              <p className="text-gray-900 dark:text-white">{room.location || "N/A"}</p>
            </div>
          </div>

          {room.notes && (
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                Ghi chú
              </label>
              <p className="text-gray-900 dark:text-white">{room.notes}</p>
            </div>
          )}

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                Ngày tạo
              </label>
              <p className="text-gray-900 dark:text-white">
                {room.created ? new Date(room.created).toLocaleString("vi-VN") : "N/A"}
              </p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">
                Cập nhật lần cuối
              </label>
              <p className="text-gray-900 dark:text-white">
                {room.lastModified
                  ? new Date(room.lastModified).toLocaleString("vi-VN")
                  : "N/A"}
              </p>
            </div>
          </div>
        </div>

        <div className="flex justify-end mt-6">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition"
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default RoomDetailModal;

