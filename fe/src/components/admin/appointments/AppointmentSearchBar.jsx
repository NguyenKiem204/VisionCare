import React from "react";

const AppointmentSearchBar = ({
  value,
  onChange,
  onStatusFilter,
  onDateRangeFilter,
}) => {
  const [showFilters, setShowFilters] = React.useState(false);
  const [statusFilter, setStatusFilter] = React.useState("");
  const [startDate, setStartDate] = React.useState("");
  const [endDate, setEndDate] = React.useState("");

  const handleStatusChange = (e) => {
    const status = e.target.value;
    setStatusFilter(status);
    onStatusFilter(status);
  };

  const handleDateRangeApply = () => {
    onDateRangeFilter(startDate, endDate);
  };

  const handleDateRangeClear = () => {
    setStartDate("");
    setEndDate("");
    onDateRangeFilter("", "");
  };

  return (
    <div className="space-y-2">
      <div className="flex gap-2">
        <input
          type="text"
          className="flex-1 border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          value={value}
          onChange={(e) => onChange(e.target.value)}
          placeholder="Tìm kiếm theo ID, tên khách hàng, bác sĩ..."
        />
        <button
          className="px-4 py-2 bg-gray-100 dark:bg-gray-600 hover:bg-gray-200 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 rounded-md transition"
          onClick={() => setShowFilters(!showFilters)}
        >
          Bộ lọc
        </button>
      </div>

      {showFilters && (
        <div className="bg-gray-50 dark:bg-gray-700 p-4 rounded-md space-y-3">
          <div className="flex gap-4">
            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Trạng thái
              </label>
              <select
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={statusFilter}
                onChange={handleStatusChange}
              >
                <option value="">Tất cả</option>
                <option value="Pending">Chờ xác nhận</option>
                <option value="Confirmed">Đã xác nhận</option>
                <option value="Cancelled">Đã hủy</option>
                <option value="Completed">Hoàn thành</option>
              </select>
            </div>

            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Khoảng thời gian
              </label>
              <div className="flex gap-2">
                <input
                  type="date"
                  className="border border-gray-300 dark:border-gray-600 px-2 py-1 rounded-md w-32 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                  placeholder="Từ ngày"
                />
                <span className="text-gray-500 dark:text-gray-400 self-center">
                  -
                </span>
                <input
                  type="date"
                  className="border border-gray-300 dark:border-gray-600 px-2 py-1 rounded-md w-32 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                  placeholder="Đến ngày"
                />
                <button
                  className="px-3 py-1 bg-blue-600 hover:bg-blue-700 text-white rounded-md text-sm transition"
                  onClick={handleDateRangeApply}
                >
                  Áp dụng
                </button>
                <button
                  className="px-3 py-1 bg-gray-300 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 rounded-md text-sm transition"
                  onClick={handleDateRangeClear}
                >
                  Xóa
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default AppointmentSearchBar;
