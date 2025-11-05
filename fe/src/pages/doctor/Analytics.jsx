import React, { useEffect, useMemo, useState } from "react";
import api from "../../utils/api";

const Analytics = () => {
  const [summary, setSummary] = useState(null);
  const [range, setRange] = useState({
    from: new Date(new Date().getFullYear(), 0, 1),
    to: new Date(),
  });

  const params = useMemo(() => ({
    from: range.from.toISOString(),
    to: range.to.toISOString(),
  }), [range]);

  useEffect(() => {
    const load = async () => {
      const res = await api.get("/doctor/me/analytics/summary", { params });
      setSummary(res?.data || res);
    };
    load();
  }, [params]);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Báo cáo</h1>
        <p className="text-gray-600 dark:text-gray-300">Tổng quan hiệu suất theo thời gian</p>
      </div>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">Tổng kết</h3>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="p-4 border border-gray-200 dark:border-gray-700 rounded">
            <div className="text-sm text-gray-500 dark:text-gray-300">Tổng cuộc hẹn</div>
            <div className="text-2xl font-semibold text-gray-900 dark:text-white">{summary?.data?.Total ?? 0}</div>
          </div>
          <div className="p-4 border border-gray-200 dark:border-gray-700 rounded">
            <div className="text-sm text-gray-500 dark:text-gray-300">Hôm nay</div>
            <div className="text-2xl font-semibold text-gray-900 dark:text-white">{summary?.data?.TodayAppointments ?? 0}</div>
          </div>
          <div className="p-4 border border-gray-200 dark:border-gray-700 rounded">
            <div className="text-sm text-gray-500 dark:text-gray-300">Theo trạng thái</div>
            <div className="text-sm text-gray-900 dark:text-white">{Object.entries(summary?.data?.Status || {}).map(([k,v]) => `${k}:${v}`).join(" · ")}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Analytics;


