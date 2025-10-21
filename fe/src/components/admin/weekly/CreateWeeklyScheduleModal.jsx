import React, { useState } from "react";
import { createWeeklySchedule } from "../../../services/adminWeeklyScheduleAPI";
// Giả sử đã có hàm toast
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

const emptyMass = () => ({
  time: "",
  type: "",
  celebrant: "",
  specialName: "",
  note: "",
  isSolemn: false,
});

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

const CreateWeeklyScheduleModal = ({ open, onClose, onSuccess }) => {
  const [selectedYear, setSelectedYear] = useState(currentYear);
  const [weekOptions, setWeekOptions] = useState(getWeeksOfYear(currentYear));
  const [selectedWeek, setSelectedWeek] = useState(null);
  const [weekStartDate, setWeekStartDate] = useState("");
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dailySchedules, setDailySchedules] = useState([]);
  const [expanded, setExpanded] = useState(Array(7).fill(false));
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [massErrors, setMassErrors] = useState({});

  // Tính toán ngày đầu tuần, danh sách ngày trong tuần, ngày cuối tuần
  let monday = weekStartDate ? getMonday(weekStartDate) : null;
  let weekDates = monday ? getWeekDates(monday) : [];

  React.useEffect(() => {
    if (weekStartDate) {
      setDailySchedules(
        weekDates.map((date, idx) => ({
          dayOfWeek: daysOfWeek[idx],
          date: date.toISOString().slice(0, 10),
          masses: [],
        }))
      );
    } else {
      setDailySchedules([]);
    }
  }, [weekStartDate]);

  // Khi chọn năm, cập nhật lại danh sách tuần
  React.useEffect(() => {
    setWeekOptions(getWeeksOfYear(selectedYear));
    setSelectedWeek(null);
    setWeekStartDate("");
    setDailySchedules([]);
  }, [selectedYear]);

  // Khi chọn tuần, cập nhật ngày bắt đầu và dailySchedules
  React.useEffect(() => {
    if (selectedWeek) {
      setWeekStartDate(selectedWeek.value);
      // Tính dailySchedules
      const weekDates = [];
      for (let i = 0; i < 7; i++) {
        const d = new Date(selectedWeek.start);
        d.setDate(d.getDate() + i);
        weekDates.push(d);
      }
      setDailySchedules(
        weekDates.map((date, idx) => ({
          dayOfWeek: daysOfWeek[idx],
          date: date.toISOString().slice(0, 10),
          masses: [],
        }))
      );
    } else {
      setWeekStartDate("");
      setDailySchedules([]);
    }
  }, [selectedWeek]);

  // Tính toán weekEndDate từ selectedWeek
  const weekEndDate = selectedWeek
    ? (() => {
        // Tính 7 ngày trong tuần từ ngày thứ 2
        const weekDates = [];
        for (let i = 0; i < 7; i++) {
          const d = new Date(selectedWeek.value);
          d.setDate(d.getDate() + i);
          weekDates.push(d);
        }
        // Ngày cuối tuần là ngày thứ 7 (index 6)
        const result = weekDates[6].toISOString().slice(0, 10);
        console.log("Debug weekEndDate:", {
          selectedWeekStart: selectedWeek.start,
          selectedWeekValue: selectedWeek.value,
          weekDates: weekDates.map((d) => d.toISOString().slice(0, 10)),
          endDate: result,
          startDate: selectedWeek.value,
        });
        return result;
      })()
    : "";

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
    let hasError = false;
    let newMassErrors = {};
    dailySchedules.forEach((day, dayIdx) => {
      day.masses.forEach((mass, massIdx) => {
        const key = `${dayIdx}-${massIdx}`;
        let err = {};
        if (!mass.time) err.time = true;
        if (!mass.type) err.type = true;
        if (!mass.celebrant) err.celebrant = true;
        if (Object.keys(err).length > 0) {
          newMassErrors[key] = err;
          hasError = true;
        }
      });
    });
    setMassErrors(newMassErrors);
    if (!weekStartDate || !title) {
      setError("Vui lòng nhập đầy đủ thông tin bắt buộc.");
      hasError = true;
    }
    if (hasError) {
      setError("Vui lòng điền đầy đủ thông tin các thánh lễ.");
      setTimeout(() => {
        const el = document.querySelector(".border-red-500");
        if (el) el.scrollIntoView({ behavior: "smooth", block: "center" });
      }, 100);
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
          masses: d.masses.filter((m) => m.time && m.type && m.celebrant),
        })),
      };
      await createWeeklySchedule(payload);
      toast.success("Tạo lịch tuần thành công!");
      setLoading(false);
      onSuccess && onSuccess();
      onClose && onClose();
    } catch (err) {
      setLoading(false);
      toast.error("Tạo lịch tuần thất bại!");
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
              Tạo lịch thánh lễ tuần mới
            </h2>

            {/* Ẩn 2 trường ngày nhưng vẫn có value để gửi lên BE */}
            <input type="hidden" value={weekStartDate} />
            <input type="hidden" value={weekEndDate} />

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
              <div className="space-y-2 overflow-y-auto max-h-[40vh] pr-1">
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
                      <span className="text-xs dark:text-gray-300">
                        {day.date}
                      </span>
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
                            value={day.date}
                            readOnly
                          />
                        </div>
                        <div className="space-y-2">
                          {day.masses.map((mass, massIdx) => {
                            const key = `${dayIdx}-${massIdx}`;
                            const err = massErrors[key] || {};
                            return (
                              <div
                                key={massIdx}
                                className="border rounded p-2 bg-white dark:bg-gray-800 flex flex-col gap-2"
                              >
                                <div className="flex flex-wrap gap-2">
                                  <input
                                    type="time"
                                    className={`border px-2 py-1 rounded-lg w-28 dark:bg-gray-900 dark:text-white dark:border-gray-700${
                                      err.time ? " border-red-500" : ""
                                    }`}
                                    value={mass.time}
                                    onChange={(e) =>
                                      handleMassChange(
                                        dayIdx,
                                        massIdx,
                                        "time",
                                        e.target.value
                                      )
                                    }
                                    required
                                  />
                                  <input
                                    type="text"
                                    className={`border px-2 py-1 rounded-lg w-36 dark:bg-gray-900 dark:text-white dark:border-gray-700${
                                      err.type ? " border-red-500" : ""
                                    }`}
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
                                    className={`border px-2 py-1 rounded-lg w-36 dark:bg-gray-900 dark:text-white dark:border-gray-700${
                                      err.celebrant ? " border-red-500" : ""
                                    }`}
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
                                  <label className="flex items-center gap-1 text-xs dark:text-white">
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
                                <div className="flex flex-wrap gap-2">
                                  {err.time && (
                                    <span className="text-xs text-red-500">
                                      * Chưa nhập giờ
                                    </span>
                                  )}
                                  {err.type && (
                                    <span className="text-xs text-red-500">
                                      * Chưa nhập loại thánh lễ
                                    </span>
                                  )}
                                  {err.celebrant && (
                                    <span className="text-xs text-red-500">
                                      * Chưa nhập chủ tế
                                    </span>
                                  )}
                                </div>
                              </div>
                            );
                          })}
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
              <div className="text-red-600 mb-2 text-center dark:text-red-400">
                {error}
              </div>
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

export default CreateWeeklyScheduleModal;
