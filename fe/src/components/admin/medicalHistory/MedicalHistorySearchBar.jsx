import React from "react";
import { Search } from "lucide-react";

const MedicalHistorySearchBar = ({ 
  value, 
  onChange, 
  onPatientFilter, 
  onDoctorFilter, 
  onDateRangeChange, 
  dateRange 
}) => {
  return (
    <div className="space-y-3">
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="flex-1">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={16} />
            <input
              type="text"
              placeholder="Tìm kiếm theo bệnh nhân, bác sĩ, chẩn đoán..."
              className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              value={value}
              onChange={(e) => onChange(e.target.value)}
            />
          </div>
        </div>
        
        <div className="flex gap-2">
          <input
            type="text"
            placeholder="ID bệnh nhân..."
            className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white min-w-[120px]"
            onChange={(e) => onPatientFilter(e.target.value)}
          />
          
          <input
            type="text"
            placeholder="ID bác sĩ..."
            className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white min-w-[120px]"
            onChange={(e) => onDoctorFilter(e.target.value)}
          />
        </div>
      </div>

      {/* Date Range Filter */}
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="flex-1">
          <label className="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
            Từ ngày:
          </label>
          <input
            type="date"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            value={dateRange.from}
            onChange={(e) => onDateRangeChange("from", e.target.value)}
          />
        </div>
        
        <div className="flex-1">
          <label className="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
            Đến ngày:
          </label>
          <input
            type="date"
            className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
            value={dateRange.to}
            onChange={(e) => onDateRangeChange("to", e.target.value)}
          />
        </div>
      </div>
    </div>
  );
};

export default MedicalHistorySearchBar;
