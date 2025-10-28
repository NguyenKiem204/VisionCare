import React from "react";
import { Search } from "lucide-react";

const EquipmentSearchBar = ({ value, onChange, onStatusFilter, onLocationFilter, locationValue }) => {
  const statusOptions = [
    { value: "", label: "Tất cả trạng thái" },
    { value: "Active", label: "Hoạt động" },
    { value: "Inactive", label: "Không hoạt động" },
    { value: "Maintenance", label: "Bảo trì" },
    { value: "Broken", label: "Hỏng" },
  ];

  return (
    <div className="flex flex-col sm:flex-row gap-2">
      <div className="flex-1">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={16} />
          <input
            type="text"
            placeholder="Tìm kiếm theo tên, model, serial, nhà sản xuất..."
            className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            value={value}
            onChange={(e) => onChange(e.target.value)}
          />
        </div>
      </div>
      
      <div className="flex gap-2">
        <select
          className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white min-w-[140px]"
          onChange={(e) => onStatusFilter(e.target.value)}
        >
          {statusOptions.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
        
        <input
          type="text"
          placeholder="Vị trí..."
          className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white min-w-[120px]"
          value={locationValue}
          onChange={(e) => onLocationFilter(e.target.value)}
        />
      </div>
    </div>
  );
};

export default EquipmentSearchBar;
