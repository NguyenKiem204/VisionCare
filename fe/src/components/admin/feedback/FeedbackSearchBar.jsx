import React from "react";
import { Search } from "lucide-react";

const FeedbackSearchBar = ({ 
  value, 
  onChange, 
  onRatingFilter, 
  onHasResponseFilter, 
  onDateRangeChange, 
  dateRange 
}) => {
  const ratingOptions = [
    { value: "", label: "Tất cả đánh giá" },
    { value: "5", label: "5 sao" },
    { value: "4", label: "4 sao" },
    { value: "3", label: "3 sao" },
    { value: "2", label: "2 sao" },
    { value: "1", label: "1 sao" },
  ];

  const hasResponseOptions = [
    { value: "", label: "Tất cả" },
    { value: "true", label: "Đã phản hồi" },
    { value: "false", label: "Chưa phản hồi" },
  ];

  return (
    <div className="space-y-3">
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="flex-1">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={16} />
            <input
              type="text"
              placeholder="Tìm kiếm theo bệnh nhân, bác sĩ, dịch vụ..."
              className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
              value={value}
              onChange={(e) => onChange(e.target.value)}
            />
          </div>
        </div>
        
        <div className="flex gap-2">
          <select
            className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white min-w-[140px]"
            onChange={(e) => onRatingFilter(e.target.value)}
          >
            {ratingOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
          
          <select
            className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white min-w-[140px]"
            onChange={(e) => onHasResponseFilter(e.target.value)}
          >
            {hasResponseOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
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

export default FeedbackSearchBar;
