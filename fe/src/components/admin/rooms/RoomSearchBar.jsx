import React from "react";
import { Search } from "lucide-react";

const RoomSearchBar = ({
  value,
  onChange,
  onStatusFilter,
  onLocationFilter,
  locationValue,
}) => {
  const statusOptions = [
    { value: "", label: "Tất cả trạng thái" },
    { value: "Active", label: "Hoạt động" },
    { value: "Inactive", label: "Không hoạt động" },
    { value: "Maintenance", label: "Bảo trì" },
  ];

  return (
    <div className="flex flex-wrap gap-2">
      <div className="flex-1 min-w-[200px] relative">
        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
        <input
          type="text"
          value={value}
          onChange={(e) => onChange(e.target.value)}
          placeholder="Tìm kiếm theo tên, mã phòng..."
          className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-900 dark:text-white"
        />
      </div>
      <select
        value={onStatusFilter ? "" : ""}
        onChange={(e) => onStatusFilter && onStatusFilter(e.target.value)}
        className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none dark:bg-gray-900 dark:text-white dark:border-gray-700 min-w-[150px]"
      >
        {statusOptions.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
      <input
        type="text"
        value={locationValue || ""}
        onChange={(e) => onLocationFilter && onLocationFilter(e.target.value)}
        placeholder="Lọc theo vị trí..."
        className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none dark:bg-gray-900 dark:text-white dark:border-gray-700 min-w-[150px]"
      />
    </div>
  );
};

export default RoomSearchBar;

