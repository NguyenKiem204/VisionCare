import React from "react";
import { Calendar, ChevronDown } from "lucide-react";

const DateRangePicker = ({ value, onChange, presets = [] }) => {
  const formatDate = (date) => {
    return date.toLocaleDateString("vi-VN");
  };

  const handlePresetClick = (preset) => {
    const now = new Date();
    let from, to;

    switch (preset.value) {
      case "today":
        from = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        to = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1);
        break;
      case "7d":
        from = new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000);
        to = now;
        break;
      case "30d":
        from = new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000);
        to = now;
        break;
      case "ytd":
        from = new Date(now.getFullYear(), 0, 1);
        to = now;
        break;
      default:
        return;
    }

    onChange({ from, to });
  };

  return (
    <div className="flex items-center space-x-2">
      {/* Preset buttons */}
      <div className="flex space-x-1">
        {presets.map((preset) => (
          <button
            key={preset.value}
            onClick={() => handlePresetClick(preset)}
            className={`px-3 py-1 text-xs font-medium rounded-md transition-colors ${
              preset.active
                ? "bg-blue-100 text-blue-700 dark:bg-blue-900 dark:text-blue-200"
                : "bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-300 hover:bg-gray-200 dark:hover:bg-gray-600"
            }`}
          >
            {preset.label}
          </button>
        ))}
      </div>

      {/* Date range display */}
      <div className="flex items-center space-x-2 px-3 py-2 bg-white dark:bg-gray-800 border border-gray-300 dark:border-gray-600 rounded-md">
        <Calendar className="h-4 w-4 text-gray-500 dark:text-gray-400" />
        <span className="text-sm text-gray-700 dark:text-gray-300">
          {formatDate(value.from)} - {formatDate(value.to)}
        </span>
        <ChevronDown className="h-4 w-4 text-gray-500 dark:text-gray-400" />
      </div>
    </div>
  );
};

export default DateRangePicker;
