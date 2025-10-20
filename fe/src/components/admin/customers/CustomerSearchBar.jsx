import React from "react";

const CustomerSearchBar = ({
  value,
  onChange,
  onGenderFilter,
  onAgeRangeFilter,
}) => {
  const [showFilters, setShowFilters] = React.useState(false);
  const [genderFilter, setGenderFilter] = React.useState("");
  const [minAge, setMinAge] = React.useState("");
  const [maxAge, setMaxAge] = React.useState("");

  const handleGenderChange = (e) => {
    const gender = e.target.value;
    setGenderFilter(gender);
    onGenderFilter(gender);
  };

  const handleAgeRangeApply = () => {
    onAgeRangeFilter(minAge, maxAge);
  };

  const handleAgeRangeClear = () => {
    setMinAge("");
    setMaxAge("");
    onAgeRangeFilter("", "");
  };

  return (
    <div className="space-y-2">
      <div className="flex gap-2">
        <input
          type="text"
          className="flex-1 border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          value={value}
          onChange={(e) => onChange(e.target.value)}
          placeholder="Tìm kiếm theo tên, email, số điện thoại..."
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
                Giới tính
              </label>
              <select
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={genderFilter}
                onChange={handleGenderChange}
              >
                <option value="">Tất cả</option>
                <option value="Male">Nam</option>
                <option value="Female">Nữ</option>
                <option value="Other">Khác</option>
              </select>
            </div>

            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Khoảng tuổi
              </label>
              <div className="flex gap-2">
                <input
                  type="number"
                  className="border border-gray-300 dark:border-gray-600 px-2 py-1 rounded-md w-20 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                  value={minAge}
                  onChange={(e) => setMinAge(e.target.value)}
                  placeholder="Từ"
                  min="0"
                />
                <span className="text-gray-500 dark:text-gray-400 self-center">
                  -
                </span>
                <input
                  type="number"
                  className="border border-gray-300 dark:border-gray-600 px-2 py-1 rounded-md w-20 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                  value={maxAge}
                  onChange={(e) => setMaxAge(e.target.value)}
                  placeholder="Đến"
                  min="0"
                />
                <button
                  className="px-3 py-1 bg-blue-600 hover:bg-blue-700 text-white rounded-md text-sm transition"
                  onClick={handleAgeRangeApply}
                >
                  Áp dụng
                </button>
                <button
                  className="px-3 py-1 bg-gray-300 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 rounded-md text-sm transition"
                  onClick={handleAgeRangeClear}
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

export default CustomerSearchBar;
