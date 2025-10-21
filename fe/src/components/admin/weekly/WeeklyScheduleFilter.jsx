import React from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./datepicker-dark.css";
import { registerLocale } from "react-datepicker";
import vi from "date-fns/locale/vi";
registerLocale("vi", vi);
import { format } from "date-fns";

const WeeklyScheduleFilter = ({ filter, setFilter, loading, onSearch }) => {
  return (
    <div className="flex flex-col md:flex-row md:items-end md:justify-between gap-4 mb-4">
      <div className="flex flex-wrap gap-2 items-end">
        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Từ ngày
          </label>
          <DatePicker
            selected={filter.startDate ? new Date(filter.startDate) : null}
            onChange={(date) =>
              setFilter((f) => ({
                ...f,
                startDate: date ? format(date, "yyyy-MM-dd") : "",
              }))
            }
            dateFormat="dd/MM/yyyy"
            locale="vi"
            placeholderText="dd/MM/yyyy"
            className="border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700 w-[150px]"
            calendarClassName="dark:bg-gray-900 dark:text-white"
            isClearable
          />
        </div>
        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Đến ngày
          </label>
          <DatePicker
            selected={filter.endDate ? new Date(filter.endDate) : null}
            onChange={(date) =>
              setFilter((f) => ({
                ...f,
                endDate: date ? format(date, "yyyy-MM-dd") : "",
              }))
            }
            dateFormat="dd/MM/yyyy"
            locale="vi"
            placeholderText="dd/MM/yyyy"
            className="border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700 w-[150px]"
            calendarClassName="dark:bg-gray-900 dark:text-white"
            isClearable
          />
        </div>
        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Năm
          </label>
          <input
            type="number"
            min="2000"
            max="2100"
            className="border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
            value={filter.year || ""}
            onChange={(e) => setFilter((f) => ({ ...f, year: e.target.value }))}
          />
        </div>
        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Số tuần
          </label>
          <input
            type="number"
            min="1"
            max="53"
            className="border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
            value={filter.weekNumber || ""}
            onChange={(e) =>
              setFilter((f) => ({ ...f, weekNumber: e.target.value }))
            }
          />
        </div>
        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Trạng thái
          </label>
          <select
            className="border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
            value={filter.status || ""}
            onChange={(e) =>
              setFilter((f) => ({ ...f, status: e.target.value }))
            }
          >
            <option value="">Tất cả</option>
            <option value="DRAFT">Nháp</option>
            <option value="PUBLISHED">Đã xuất bản</option>
            <option value="ARCHIVED">Lưu trữ</option>
          </select>
        </div>
        <div className="flex flex-col">
          <label className="text-sm font-medium mb-1 dark:text-gray-300">
            Người tạo
          </label>
          <input
            type="text"
            className="border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
            value={filter.createdBy || ""}
            onChange={(e) =>
              setFilter((f) => ({ ...f, createdBy: e.target.value }))
            }
          />
        </div>
      </div>
      <div className="flex gap-2 mt-2 md:mt-0">
        <button
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow transition"
          onClick={onSearch}
          disabled={loading}
        >
          Tìm kiếm
        </button>
        <button
          className="bg-gray-300 hover:bg-gray-400 text-gray-800 px-4 py-2 rounded-lg shadow transition"
          onClick={() => setFilter({})}
          disabled={loading}
        >
          Làm mới
        </button>
      </div>
    </div>
  );
};

export default WeeklyScheduleFilter;
