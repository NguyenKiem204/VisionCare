import React from "react";
import { BOOKING_STEPS } from "../../utils/constants";

const StepIndicator = ({ step = 1 }) => {
  return (
    <div className="flex items-center justify-center gap-3 py-6 flex-wrap">
      {BOOKING_STEPS.map((s) => (
        <div key={s.id} className="flex items-center gap-2">
          <div
            className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold ${
              s.id <= step
                ? "bg-blue-600 text-white"
                : "bg-gray-200 text-gray-600"
            }`}
          >
            {s.id}
          </div>
          <span
            className={`hidden md:inline text-xs ${
              s.id <= step ? "text-blue-700" : "text-gray-500"
            }`}
          >
            {s.name}
          </span>
        </div>
      ))}
    </div>
  );
};

export default StepIndicator;
