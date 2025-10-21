import React from "react";
import { statusConfig } from "./weeklyScheduleStatusConfig";

const WeeklyScheduleStatusBadge = ({ status }) => {
  const info = statusConfig[status] || {
    label: status,
    color: "bg-gray-200 text-gray-800",
  };
  return (
    <span className={`px-2 py-1 rounded text-xs font-bold ${info.color}`}>
      {info.label}
    </span>
  );
};

export default WeeklyScheduleStatusBadge;
