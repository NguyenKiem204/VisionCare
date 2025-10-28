import React from "react";
import { X } from "lucide-react";

const EquipmentDetailModal = ({ equipment, isOpen, onClose }) => {
  if (!isOpen || !equipment) return null;

  const formatPrice = (price) => {
    if (!price) return "N/A";
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(price);
  };

  const formatDate = (date) => {
    if (!date) return "N/A";
    return new Date(date).toLocaleDateString("vi-VN");
  };

  const getStatusColor = (status) => {
    switch (status?.toLowerCase()) {
      case "active":
        return "bg-green-100 text-green-800";
      case "inactive":
        return "bg-gray-100 text-gray-800";
      case "maintenance":
        return "bg-yellow-100 text-yellow-800";
      case "broken":
        return "bg-red-100 text-red-800";
      default:
        return "bg-gray-100 text-gray-800";
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
      case "broken":
        return "Hỏng";
      default:
        return status || "Không xác định";
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-xl max-w-4xl w-full mx-4 max-h-[90vh] overflow-y-auto">
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
          <h2 className="text-xl font-semibold text-gray-900 dark:text-white">
            Chi tiết thiết bị
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
          >
            <X size={24} />
          </button>
        </div>

        <div className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Thông tin cơ bản */}
            <div className="space-y-4">
              <h3 className="text-lg font-medium text-gray-900 dark:text-white border-b border-gray-200 dark:border-gray-700 pb-2">
                Thông tin cơ bản
              </h3>
              
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  ID
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.id}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Tên thiết bị
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.name}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Mô tả
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.description || "Không có mô tả"}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Model
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.model || "N/A"}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Serial Number
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.serialNumber || "N/A"}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Nhà sản xuất
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.manufacturer || "N/A"}
                </p>
              </div>
            </div>

            {/* Thông tin vận hành */}
            <div className="space-y-4">
              <h3 className="text-lg font-medium text-gray-900 dark:text-white border-b border-gray-200 dark:border-gray-700 pb-2">
                Thông tin vận hành
              </h3>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Trạng thái
                </label>
                <div className="mt-1">
                  <span
                    className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(
                      equipment.status
                    )}`}
                  >
                    {getStatusText(equipment.status)}
                  </span>
                </div>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Vị trí
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {equipment.location || "N/A"}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Giá mua
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {formatPrice(equipment.purchasePrice)}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Ngày mua
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {formatDate(equipment.purchaseDate)}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Ngày tạo
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {formatDate(equipment.createdAt)}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300">
                  Cập nhật lần cuối
                </label>
                <p className="mt-1 text-sm text-gray-900 dark:text-white">
                  {formatDate(equipment.updatedAt)}
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="flex justify-end p-6 border-t border-gray-200 dark:border-gray-700">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-500"
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default EquipmentDetailModal;
