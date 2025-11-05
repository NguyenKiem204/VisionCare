import React, { useMemo } from "react";

const dayNames = ["", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật"];

export default function WeeklyDoctorScheduleView({ schedules = [] }) {
  // Build distinct shifts from data (by shiftId if available), sorted by start time
  const shifts = useMemo(() => {
    const map = new Map();
    schedules.forEach((s) => {
      const key = s.shiftId ?? `${s.shiftName}-${s.startTime}-${s.endTime}`;
      if (!map.has(key)) {
        map.set(key, {
          key,
          shiftId: s.shiftId,
          name: s.shiftName || "Ca",
          start: s.startTime,
          end: s.endTime,
        });
      }
    });
    return Array.from(map.values()).sort((a, b) => {
      const at = (a.start || "23:59").substring(0, 5);
      const bt = (b.start || "23:59").substring(0, 5);
      return at.localeCompare(bt);
    });
  }, [schedules]);

  // Index schedules by day + shift for O(1) lookup
  const byDayShift = useMemo(() => {
    const idx = new Map();
    schedules.forEach((s) => {
      if (!s.dayOfWeek) return; // ignore unknown day in grid
      const key = `${s.dayOfWeek}|${s.shiftId ?? `${s.shiftName}-${s.startTime}-${s.endTime}`}`;
      idx.set(key, s);
    });
    return idx;
  }, [schedules]);

  return (
    <div className="w-full overflow-x-auto">
      <div className="min-w-[1000px]">
        <div className="grid grid-cols-8 rounded-t-lg">
          <div className="sticky left-0 z-10 bg-gray-50 dark:bg-gray-800 p-3 font-semibold text-gray-700 dark:text-gray-200 rounded-tl-lg border border-gray-200 dark:border-gray-700">
            Ca / Giờ
          </div>
          {dayNames.slice(1).map((n) => (
            <div
              key={n}
              className="bg-gray-50 dark:bg-gray-800 p-3 font-semibold text-gray-700 dark:text-gray-200 text-center border border-gray-200 dark:border-gray-700"
            >
              {n}
            </div>
          ))}
        </div>

        {shifts.length === 0 ? (
          <div className="border border-gray-200 dark:border-gray-700 rounded-b-lg p-6 text-center text-gray-500 dark:text-gray-400">
            Chưa có ca để hiển thị
          </div>
        ) : (
          shifts.map((shift, rowIdx) => (
            <div key={shift.key} className="grid grid-cols-8">
              {/* Shift label column */}
              <div className={`sticky left-0 z-10 p-3 bg-white dark:bg-gray-900 border border-gray-200 dark:border-gray-700 ${rowIdx === shifts.length - 1 ? "rounded-bl-lg" : ""}`}>
                <div className="font-semibold text-gray-800 dark:text-gray-100 truncate">{shift.name}</div>
                <div className="text-xs text-gray-500 dark:text-gray-400">
                  {(shift.start || "--:--").substring(0, 5)} - {(shift.end || "--:--").substring(0, 5)}
                </div>
              </div>

              {/* Day cells */}
              {[1, 2, 3, 4, 5, 6, 7].map((dow) => {
                const key = `${dow}|${shift.shiftId ?? `${shift.name}-${shift.start}-${shift.end}`}`;
                const s = byDayShift.get(key);
                return (
                  <div
                    key={key}
                    className={`p-2 min-h-[84px] border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-900 ${rowIdx === shifts.length - 1 ? "last:rounded-br-lg" : ""}`}
                  >
                    {s ? (
                      <div className="h-full border border-emerald-300 dark:border-emerald-800 bg-emerald-50 dark:bg-emerald-900/30 rounded p-2 text-xs text-gray-800 dark:text-gray-100">
                        <div className="font-medium truncate">{s.shiftName || "Ca"}</div>
                        <div className="opacity-80">
                          {(s.startTime || "").substring(0, 5)} - {(s.endTime || "").substring(0, 5)}
                        </div>
                        <div className="opacity-80 truncate">Phòng: {s.roomName || "-"}</div>
                        <div className="opacity-80 truncate">Thiết bị: {s.equipmentName || "-"}</div>
                      </div>
                    ) : (
                      <div className="h-full flex items-center justify-center text-xs text-gray-300 dark:text-gray-600">—</div>
                    )}
                  </div>
                );
              })}
            </div>
          ))
        )}
      </div>
    </div>
  );
}
