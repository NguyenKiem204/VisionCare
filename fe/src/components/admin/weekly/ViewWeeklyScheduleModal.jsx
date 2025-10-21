import React, { useState } from "react";
import QuickAddMassModal from "./QuickAddMassModal";

const ViewWeeklyScheduleModal = ({
  open,
  schedule,
  onClose,
  onEditMass,
  onDeleteMass,
  onAddMass,
}) => {
  const [openQuickAdd, setOpenQuickAdd] = useState(false);
  const [selectedDay, setSelectedDay] = useState(null);

  if (!open || !schedule) return null;
  const {
    weekStartDate,
    weekEndDate,
    weekNumber,
    year,
    title,
    description,
    dailySchedules = [],
  } = schedule;

  const handleOpenQuickAdd = (day) => {
    setSelectedDay(day);
    setOpenQuickAdd(true);
  };
  const handleCloseQuickAdd = () => {
    setOpenQuickAdd(false);
    setSelectedDay(null);
  };
  const handleSaveQuickAdd = (massData) => {
    onAddMass && onAddMass(selectedDay, massData);
    handleCloseQuickAdd();
  };

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40"
      onClick={onClose}
    >
      <div
        className="bg-white dark:bg-gray-800 rounded-xl shadow-lg w-full max-w-4xl p-6 relative max-h-[90vh] overflow-y-auto border border-gray-200 dark:border-gray-700"
        onClick={(e) => e.stopPropagation()}
      >
        <h2 className="text-xl font-bold mb-2 text-center dark:text-white">
          Chi tiết lịch thánh lễ tuần
        </h2>
        <div className="mb-2 text-center text-gray-700 dark:text-gray-200">
          <span className="font-semibold">
            Tuần {weekNumber}, {year}
          </span>{" "}
          | {weekStartDate} - {weekEndDate}
        </div>
        <div className="mb-2 text-center text-lg font-semibold dark:text-white">
          {title}
        </div>
        {description && (
          <div className="mb-4 text-center text-gray-600 dark:text-gray-300">
            {description}
          </div>
        )}
        <div className="space-y-4">
          {dailySchedules.map((day, idx) => (
            <div
              key={day.dayOfWeek}
              className="border border-gray-200 dark:border-gray-700 rounded-lg p-3 bg-gray-50 dark:bg-gray-900"
            >
              <div className="flex items-center justify-between mb-2">
                <div className="font-bold text-blue-700 dark:text-blue-300">
                  {day.dayOfWeek}, {day.date}
                </div>
                <button
                  type="button"
                  className="bg-blue-100 hover:bg-blue-200 dark:bg-blue-900 dark:hover:bg-blue-800 text-blue-700 dark:text-blue-200 px-2 py-1 rounded text-xs"
                  onClick={() => handleOpenQuickAdd(day)}
                >
                  + Thêm thánh lễ
                </button>
              </div>
              {!day.masses || day.masses.length === 0 ? (
                <div className="text-gray-500 dark:text-gray-400 italic">
                  Chưa có thánh lễ nào
                </div>
              ) : (
                <ul className="space-y-2">
                  {day.masses.map((mass, massIdx) => (
                    <li
                      key={mass.id || massIdx}
                      className="flex flex-col md:flex-row md:items-center md:gap-2 bg-white dark:bg-gray-800 rounded p-2 border border-gray-200 dark:border-gray-700"
                    >
                      <div className="flex-1 flex flex-wrap gap-2 items-center">
                        <span className="font-semibold text-blue-600 dark:text-blue-300">
                          {mass.time}
                        </span>
                        <span className="dark:text-gray-200">
                          - {mass.type}
                        </span>
                        <span className="dark:text-gray-200">
                          - {mass.celebrant}
                        </span>
                        {mass.specialName && (
                          <span className="italic dark:text-gray-300">
                            ({mass.specialName})
                          </span>
                        )}
                        {mass.isSolemn && (
                          <span className="bg-yellow-200 dark:bg-yellow-700 text-yellow-800 dark:text-yellow-200 px-2 py-0.5 rounded text-xs ml-2 font-semibold">
                            Trọng
                          </span>
                        )}
                        {mass.note && (
                          <span className="text-gray-500 dark:text-gray-400 ml-2">
                            {mass.note}
                          </span>
                        )}
                      </div>
                      <div className="flex gap-1 mt-2 md:mt-0">
                        <button
                          className="text-yellow-600 dark:text-yellow-300 hover:underline text-xs"
                          onClick={() => onEditMass && onEditMass(day, mass)}
                        >
                          Sửa
                        </button>
                        <button
                          className="text-red-600 dark:text-red-400 hover:underline text-xs"
                          onClick={() =>
                            onDeleteMass && onDeleteMass(day, mass)
                          }
                        >
                          Xóa
                        </button>
                      </div>
                    </li>
                  ))}
                </ul>
              )}
            </div>
          ))}
        </div>
        <div className="flex justify-end gap-2 mt-6">
          <button
            type="button"
            className="px-4 py-2 rounded-lg bg-gray-300 hover:bg-gray-400 text-gray-800 dark:bg-gray-700 dark:hover:bg-gray-600 dark:text-gray-100"
            onClick={onClose}
          >
            Đóng
          </button>
        </div>
        <QuickAddMassModal
          open={openQuickAdd}
          onClose={handleCloseQuickAdd}
          onSave={handleSaveQuickAdd}
          day={selectedDay}
        />
      </div>
    </div>
  );
};

export default ViewWeeklyScheduleModal;
