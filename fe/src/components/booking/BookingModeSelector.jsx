import React from "react";
import Button from "../common/Button";

const BookingModeSelector = ({ selectedMode, onModeChange, onNext }) => {
  return (
    <div className="space-y-6">
      <div>
        <h3 className="text-lg font-semibold mb-2">Chọn cách đặt lịch</h3>
        <p className="text-sm text-gray-600 mb-6">
          Bạn muốn đặt lịch theo bác sĩ hay theo dịch vụ?
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {/* Book by Doctor */}
        <button
          onClick={() => onModeChange("doctor")}
          className={`p-6 rounded-xl border-2 transition-all text-left ${
            selectedMode === "doctor"
              ? "border-blue-500 bg-blue-50 shadow-md"
              : "border-gray-200 bg-white hover:border-blue-300"
          }`}
        >
          <div className="flex items-start gap-4">
            <div
              className={`w-12 h-12 rounded-full flex items-center justify-center ${
                selectedMode === "doctor"
                  ? "bg-blue-600 text-white"
                  : "bg-gray-100 text-gray-600"
              }`}
            >
              <svg
                className="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
                />
              </svg>
            </div>
            <div className="flex-1">
              <h4 className="font-semibold text-gray-900 mb-1">
                Đặt theo Bác sĩ
              </h4>
              <p className="text-sm text-gray-600">
                Chọn bác sĩ bạn muốn khám, sau đó chọn dịch vụ và thời gian
              </p>
            </div>
            {selectedMode === "doctor" && (
              <svg
                className="w-6 h-6 text-blue-600"
                fill="currentColor"
                viewBox="0 0 20 20"
              >
                <path
                  fillRule="evenodd"
                  d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                  clipRule="evenodd"
                />
              </svg>
            )}
          </div>
        </button>

        {/* Book by Service */}
        <button
          onClick={() => onModeChange("service")}
          className={`p-6 rounded-xl border-2 transition-all text-left ${
            selectedMode === "service"
              ? "border-blue-500 bg-blue-50 shadow-md"
              : "border-gray-200 bg-white hover:border-blue-300"
          }`}
        >
          <div className="flex items-start gap-4">
            <div
              className={`w-12 h-12 rounded-full flex items-center justify-center ${
                selectedMode === "service"
                  ? "bg-blue-600 text-white"
                  : "bg-gray-100 text-gray-600"
              }`}
            >
              <svg
                className="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"
                />
              </svg>
            </div>
            <div className="flex-1">
              <h4 className="font-semibold text-gray-900 mb-1">
                Đặt theo Dịch vụ
              </h4>
              <p className="text-sm text-gray-600">
                Chọn dịch vụ bạn cần, sau đó chọn bác sĩ và thời gian
              </p>
            </div>
            {selectedMode === "service" && (
              <svg
                className="w-6 h-6 text-blue-600"
                fill="currentColor"
                viewBox="0 0 20 20"
              >
                <path
                  fillRule="evenodd"
                  d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                  clipRule="evenodd"
                />
              </svg>
            )}
          </div>
        </button>
      </div>

      <div className="flex justify-end mt-6">
        <Button
          variant="accent"
          onClick={onNext}
          disabled={!selectedMode}
        >
          Tiếp tục
        </Button>
      </div>
    </div>
  );
};

export default BookingModeSelector;

