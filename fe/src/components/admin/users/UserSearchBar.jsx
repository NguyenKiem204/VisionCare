import React from "react";

const UserSearchBar = ({ value, onChange, inputClassName }) => (
  <input
    type="text"
    value={value}
    onChange={(e) => onChange(e.target.value)}
    placeholder="Tìm kiếm theo email hoặc role..."
    className={`w-full px-4 py-2 rounded-xl border border-gray-300 dark:border-gray-700 bg-white dark:bg-gray-900 text-gray-900 dark:text-white placeholder-gray-400 dark:placeholder-gray-400 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition-colors duration-200 ${inputClassName || ''}`}
  />
);

export default UserSearchBar;
