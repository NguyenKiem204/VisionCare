import React from "react";

const KpiCard = ({
  title,
  value,
  icon: Icon,
  loading = false,
  trend = null,
}) => {
  return (
    <div className="relative overflow-hidden rounded-lg bg-white dark:bg-gray-800 px-4 py-5 shadow sm:px-6 sm:py-6 border border-gray-200 dark:border-gray-700">
      <dt>
        <div className="absolute rounded-md bg-blue-500 p-3">
          <Icon className="h-6 w-6 text-white" />
        </div>
        <p className="ml-16 truncate text-sm font-medium text-gray-500 dark:text-gray-400">
          {title}
        </p>
      </dt>
      <dd className="ml-16 flex items-baseline">
        <p className="text-2xl font-semibold text-gray-900 dark:text-white">
          {loading ? "..." : value}
        </p>
        {trend && (
          <p
            className={`ml-2 flex items-baseline text-sm font-semibold ${
              trend.type === "positive"
                ? "text-green-600 dark:text-green-400"
                : "text-red-600 dark:text-red-400"
            }`}
          >
            {trend.value}
          </p>
        )}
      </dd>
    </div>
  );
};

export default KpiCard;
