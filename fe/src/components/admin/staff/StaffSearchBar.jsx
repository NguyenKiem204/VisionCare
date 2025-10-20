import React from "react";

const StaffSearchBar = ({ value, onChange, onGenderFilter }) => {
  const [showFilters, setShowFilters] = React.useState(false);
  const [genderFilter, setGenderFilter] = React.useState("");

  const handleGenderChange = (e) => {
    const gender = e.target.value;
    setGenderFilter(gender);
    onGenderFilter(gender);
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
          </div>
        </div>
      )}
    </div>
  );
};

export default StaffSearchBar;
