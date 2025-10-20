import React, { useState, useEffect } from "react";
import { updateWeeklySchedule } from "../../../services/adminWeeklyScheduleAPI";
import toast from "react-hot-toast";
import {
  getISOWeeksInYear,
  startOfISOWeek,
  addWeeks,
  format as formatFns,
} from "date-fns";

const daysOfWeek = [
  "Thứ hai",
  "Thứ ba",
  "Thứ tư",
  "Thứ năm",
  "Thứ sáu",
  "Thứ bảy",
  "Chủ nhật",
];

function getWeeksOfYear(year) {
  const weeks = getISOWeeksInYear(new Date(year, 0, 1));
  let arr = [];
  for (let i = 1; i <= weeks; i++) {
    const start = startOfISOWeek(addWeeks(new Date(year, 0, 1), i - 1));
    const end = addWeeks(start, 1);
    arr.push({
      label: `Tuần ${i} (${formatFns(start, "dd/MM/yyyy")} - ${formatFns(
        end,
        "dd/MM/yyyy"
      )})`,
      value: formatFns(start, "yyyy-MM-dd"),
      start,
      weekNumber: i,
    });
  }
  return arr;
}

const currentYear = new Date().getFullYear();
const yearOptions = [currentYear - 1, currentYear, currentYear + 1];

function getMonday(dateStr) {
  const d = new Date(dateStr);
  const day = d.getDay();
  const diff = (day === 0 ? -6 : 1) - day;
  d.setDate(d.getDate() + diff);
  return d;
}

function getWeekDates(mondayDate) {
  const arr = [];
  for (let i = 0; i < 7; i++) {
    const d = new Date(mondayDate);
    d.setDate(d.getDate() + i);
    arr.push(d);
  }
  return arr;
}

const emptyMass = () => ({
  time: "",
  type: "",
  celebrant: "",
  specialName: "",
  note: "",
  isSolemn: false,
});

const EditWeeklyScheduleModal = ({ open, schedule, onClose, onSuccess }) => {
  const [selectedYear, setSelectedYear] = useState(currentYear);
  const [weekOptions, setWeekOptions] = useState(getWeeksOfYear(currentYear));
  const [selectedWeek, setSelectedWeek] = useState(null);
  const [weekStartDate, setWeekStartDate] = useState("");
  const [weekEndDate, setWeekEndDate] = useState("");
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dailySchedules, setDailySchedules] = useState([]);
  const [expanded, setExpanded] = useState(Array(7).fill(false));
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (open && schedule) {
      const weekStart = schedule.weekStartDate;
      const year = weekStart ? new Date(weekStart).getFullYear() : currentYear;
      setSelectedYear(year);
      setWeekOptions(getWeeksOfYear(year));
      const week = getWeeksOfYear(year).find((w) => w.value === weekStart);
      setSelectedWeek(week || null);
      setTitle(schedule.title || "");
      setDescription(schedule.description || "");
      let daily = daysOfWeek.map((day, idx) => {
        const found = (schedule.dailySchedules || []).find(
          (d) => d.dayOfWeek === day
        );
        return found
          ? { ...found, masses: found.masses ? [...found.masses] : [] }
          : {
              dayOfWeek: day,
              date:
                week && week.start
                  ? new Date(week.start).toISOString().slice(0, 10)
                  : "",
              masses: [],
            };
      });
      setDailySchedules(daily);
      setExpanded(Array(7).fill(false));
    }
  }, [open, schedule]);

  useEffect(() => {
    setWeekOptions(getWeeksOfYear(selectedYear));
    setSelectedWeek(null);
    setDailySchedules([]);
  }, [selectedYear]);

  useEffect(() => {
    if (selectedWeek) {
      setWeekStartDate(selectedWeek.value);
      const weekDates = [];
      for (let i = 0; i < 7; i++) {
        const d = new Date(selectedWeek.start);
        d.setDate(d.getDate() + i);
        weekDates.push(d);
      }
      setWeekEndDate(weekDates[6].toISOString().slice(0, 10));
      setDailySchedules(
        weekDates.map((date, idx) => ({
          dayOfWeek: daysOfWeek[idx],
          date: date.toISOString().slice(0, 10),
          masses: dailySchedules[idx]?.masses || [],
        }))
      );
    } else {
      setWeekStartDate("");
      setWeekEndDate("");
      setDailySchedules([]);
    }
  }, [selectedWeek]);

  const handleAddMass = (dayIdx) => {
    setDailySchedules((prev) => {
      const copy = [...prev];
      copy[dayIdx].masses.push(emptyMass());
      return copy;
    });
  };

  const handleRemoveMass = (dayIdx, massIdx) => {
    setDailySchedules((prev) => {
      const copy = [...prev];
      copy[dayIdx].masses.splice(massIdx, 1);
      return copy;
    });
  };

  const handleMassChange = (dayIdx, massIdx, field, value) => {
    setDailySchedules((prev) => {
      const copy = [...prev];
      copy[dayIdx].masses[massIdx][field] = value;
      return copy;
    });
  };

  const handleDateChange = (dayIdx, value) => {
    setDailySchedules((prev) => {
      const copy = [...prev];
      copy[dayIdx].date = value;
      return copy;
    });
  };

  const handleExpand = (idx) => {
    setExpanded((prev) => {
      const copy = [...prev];
      copy[idx] = !copy[idx];
      return copy;
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    if (!weekStartDate || !weekEndDate || !title) {
      setError("Vui lòng nhập đầy đủ thông tin bắt buộc.");
      return;
    }
    setLoading(true);
    try {
      const payload = {
        weekStartDate,
        weekEndDate,
        title,
        description,
        dailySchedules: dailySchedules.map((d) => ({
          ...d,
          masses: d.masses
            .filter((m) => m.time && m.type && m.celebrant)
            .map((m) => ({
              ...m,
              time: m.time ? m.time.match(/^\d{2}:\d{2}/)?.[0] || m.time : "",
            })),
        })),
      };
      await updateWeeklySchedule(schedule.id, payload);
      toast.success("Cập nhật lịch tuần thành công!");
      setLoading(false);
      onSuccess && onSuccess();
      onClose && onClose();
    } catch (err) {
      setLoading(false);
      toast.error("Cập nhật lịch tuần thất bại!");
      setError(err?.response?.data?.message || "Có lỗi xảy ra");
    }
  };

  if (!open) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40"
      onClick={onClose}
    >
      <div
        className="bg-white dark:bg-gray-800 rounded-xl shadow-lg w-full max-w-3xl p-0 relative max-h-[90vh] flex flex-col"
        onClick={(e) => e.stopPropagation()}
      >
        <form onSubmit={handleSubmit} className="flex flex-col h-full">
          <div className="p-6 overflow-y-auto flex-1">
            <h2 className="text-xl font-bold mb-4 text-center dark:text-white">
              Chỉnh sửa lịch thánh lễ tuần
            </h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
              <div>
                <label className="block text-sm font-medium mb-1 dark:text-white">
                  Năm *
                </label>
                <select
                  className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                  value={selectedYear}
                  onChange={(e) => setSelectedYear(Number(e.target.value))}
                  required
                >
                  {yearOptions.map((y) => (
                    <option key={y} value={y}>
                      {y}
                    </option>
                  ))}
                </select>
              </div>
              <div>
                <label className="block text-sm font-medium mb-1 dark:text-white">
                  Chọn tuần *
                </label>
                <select
                  className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                  value={selectedWeek ? selectedWeek.value : ""}
                  onChange={(e) => {
                    const week = weekOptions.find(
                      (w) => w.value === e.target.value
                    );
                    setSelectedWeek(week);
                  }}
                  required
                >
                  <option value="">-- Chọn tuần --</option>
                  {weekOptions.map((w) => (
                    <option key={w.value} value={w.value}>
                      {w.label}
                    </option>
                  ))}
                </select>
              </div>
              <input type="hidden" value={weekStartDate} />
              <input type="hidden" value={weekEndDate} />
              <div className="md:col-span-2">
                <label className="block text-sm font-medium mb-1 dark:text-white">
                  Tiêu đề *
                </label>
                <input
                  type="text"
                  className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  required
                />
              </div>
              <div className="md:col-span-2">
                <label className="block text-sm font-medium mb-1 dark:text-white">
                  Mô tả
                </label>
                <textarea
                  className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                  rows={2}
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                />
              </div>
            </div>
            <div className="mb-4">
              <h3 className="font-semibold mb-2 dark:text-white">
                Danh sách 7 ngày trong tuần
              </h3>
              <div className="space-y-2">
                {dailySchedules.map((day, dayIdx) => (
                  <div
                    key={day.dayOfWeek}
                    className="border rounded-lg p-2 bg-gray-50 dark:bg-gray-900"
                  >
                    <div
                      className="flex items-center justify-between cursor-pointer"
                      onClick={() => handleExpand(dayIdx)}
                    >
                      <div className="font-bold dark:text-white">
                        {day.dayOfWeek}
                      </div>
                      <button
                        type="button"
                        className="text-blue-600 text-xs underline dark:text-blue-400 dark:hover:text-blue-300"
                      >
                        {expanded[dayIdx] ? "Ẩn" : "Hiện"}
                      </button>
                    </div>
                    {expanded[dayIdx] && (
                      <div className="mt-2 space-y-2">
                        <div className="flex flex-col md:flex-row gap-2 mb-2">
                          <label className="text-sm font-medium dark:text-white">
                            Ngày
                          </label>
                          <input
                            type="text"
                            className="border px-2 py-1 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                            value={formatFns(new Date(day.date), "dd/MM/yyyy")}
                            readOnly
                          />
                        </div>
                        <div className="space-y-2">
                          {day.masses.map((mass, massIdx) => (
                            <div
                              key={mass.id || massIdx}
                              className="border rounded p-2 bg-white dark:bg-gray-800 flex flex-col gap-2"
                            >
                              <div className="flex flex-wrap gap-2">
                                <input
                                  type="time"
                                  className="border px-2 py-1 rounded-lg w-28 dark:bg-gray-900 dark:text-white dark:border-gray-700"
                                  value={mass.time}
                                  onChange={(e) =>
                                    handleMassChange(
                                      dayIdx,
                                      massIdx,
                                      "time",
                                      e.target.value
                                    )
                                  }
                                  placeholder="Giờ"
                                  required
                                />
                                <input
                                  type="text"
                                  className="border px-2 py-1 rounded-lg w-36 dark:bg-gray-900 dark:text-white dark:border-gray-700"
                                  value={mass.type}
                                  onChange={(e) =>
                                    handleMassChange(
                                      dayIdx,
                                      massIdx,
                                      "type",
                                      e.target.value
                                    )
                                  }
                                  placeholder="Loại thánh lễ"
                                  required
                                />
                                <input
                                  type="text"
                                  className="border px-2 py-1 rounded-lg w-36 dark:bg-gray-900 dark:text-white dark:border-gray-700"
                                  value={mass.celebrant}
                                  onChange={(e) =>
                                    handleMassChange(
                                      dayIdx,
                                      massIdx,
                                      "celebrant",
                                      e.target.value
                                    )
                                  }
                                  placeholder="Chủ tế"
                                  required
                                />
                                <input
                                  type="text"
                                  className="border px-2 py-1 rounded-lg w-36 dark:bg-gray-900 dark:text-white dark:border-gray-700"
                                  value={mass.specialName}
                                  onChange={(e) =>
                                    handleMassChange(
                                      dayIdx,
                                      massIdx,
                                      "specialName",
                                      e.target.value
                                    )
                                  }
                                  placeholder="Tên đặc biệt"
                                />
                                <input
                                  type="text"
                                  className="border px-2 py-1 rounded-lg w-36 dark:bg-gray-900 dark:text-white dark:border-gray-700"
                                  value={mass.note}
                                  onChange={(e) =>
                                    handleMassChange(
                                      dayIdx,
                                      massIdx,
                                      "note",
                                      e.target.value
                                    )
                                  }
                                  placeholder="Ghi chú"
                                />
                                <label className="flex items-center gap-1 text-xs">
                                  <input
                                    type="checkbox"
                                    checked={mass.isSolemn}
                                    onChange={(e) =>
                                      handleMassChange(
                                        dayIdx,
                                        massIdx,
                                        "isSolemn",
                                        e.target.checked
                                      )
                                    }
                                  />
                                  Thánh lễ trọng
                                </label>
                                <button
                                  type="button"
                                  className="text-red-600 text-xs underline ml-2 dark:text-red-400 dark:hover:text-red-300"
                                  onClick={() =>
                                    handleRemoveMass(dayIdx, massIdx)
                                  }
                                >
                                  Xóa
                                </button>
                              </div>
                            </div>
                          ))}
                          <button
                            type="button"
                            className="bg-blue-100 hover:bg-blue-200 text-blue-700 px-2 py-1 rounded text-xs dark:bg-blue-900 dark:hover:bg-blue-800 dark:text-blue-300"
                            onClick={() => handleAddMass(dayIdx)}
                          >
                            + Thêm thánh lễ
                          </button>
                        </div>
                      </div>
                    )}
                  </div>
                ))}
              </div>
            </div>
            {error && (
              <div className="text-red-600 mb-2 text-center">{error}</div>
            )}
          </div>
          <div className="flex justify-end gap-2 p-6 pt-0 bg-white dark:bg-gray-800 sticky bottom-0 z-10 border-t dark:border-gray-700">
            <button
              type="button"
              className="px-4 py-2 rounded-lg bg-gray-300 hover:bg-gray-400 text-gray-800 dark:bg-gray-700 dark:hover:bg-gray-600 dark:text-white"
              onClick={onClose}
              disabled={loading}
            >
              Đóng
            </button>
            <button
              type="submit"
              className="px-4 py-2 rounded-lg bg-blue-600 hover:bg-blue-700 text-white font-semibold dark:bg-blue-700 dark:hover:bg-blue-800 dark:text-white"
              disabled={loading}
            >
              {loading ? "Đang lưu..." : "Lưu"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditWeeklyScheduleModal;
