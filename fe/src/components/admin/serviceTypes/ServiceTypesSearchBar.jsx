import React from "react";
import { Search } from "lucide-react";

const ServiceTypesSearchBar = ({ 
  value, 
  onChange, 
  onDurationFilter, 
  durationFilter 
}) => {
  return (
    <div className="space-y-3">
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="flex-1">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={16} />
            <input
              type="text"
              placeholder="Tìm kiếm theo tên loại dịch vụ..."
              className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              value={value}
              onChange={(e) => onChange(e.target.value)}
            />
          </div>
        </div>
      </div>

      {/* Duration Range Filter */}
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="flex-1">
          <label className="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
            Thời gian tối thiểu (phút):
          </label>
          <input
            type="number"
            min="0"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            value={durationFilter.min}
            onChange={(e) => onDurationFilter("min", e.target.value)}
            placeholder="0"
          />
        </div>
        
        <div className="flex-1">
          <label className="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
            Thời gian tối đa (phút):
          </label>
          <input
            type="number"
            min="0"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            value={durationFilter.max}
            onChange={(e) => onDurationFilter("max", e.target.value)}
            placeholder="999"
          />
        </div>
      </div>
    </div>
  );
};

export default ServiceTypesSearchBar;
