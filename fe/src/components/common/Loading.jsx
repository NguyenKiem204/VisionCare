import React from "react";

const Loading = ({ label = "Đang tải..." }) => {
  return (
    <div className="flex items-center justify-center gap-3 py-10 text-gray-600">
      <svg
        className="w-6 h-6 animate-spin"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
      >
        <circle cx="12" cy="12" r="9" strokeWidth="2" className="opacity-25" />
        <path d="M21 12a9 9 0 0 1-9 9" strokeWidth="2" className="opacity-75" />
      </svg>
      <span className="font-medium">{label}</span>
    </div>
  );
};

export default Loading;
