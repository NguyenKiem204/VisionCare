import React, { useState, useEffect } from "react";

const QuickAddMassModal = ({ open, onClose, onSave, day }) => {
  const [time, setTime] = useState("");
  const [type, setType] = useState("");
  const [celebrant, setCelebrant] = useState("");
  const [specialName, setSpecialName] = useState("");
  const [note, setNote] = useState("");
  const [isSolemn, setIsSolemn] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (open) {
      setTime("");
      setType("");
      setCelebrant("");
      setSpecialName("");
      setNote("");
      setIsSolemn(false);
      setError("");
    }
  }, [open]);

  const handleSubmit = (e) => {
    e.preventDefault();
    setError("");
    if (!time || !type || !celebrant) {
      setError("Vui lòng nhập đầy đủ thông tin bắt buộc.");
      return;
    }
    onSave && onSave({ time, type, celebrant, specialName, note, isSolemn });
    onClose && onClose();
  };

  if (!open) return null;

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40"
      onClick={onClose}
    >
      <div
        className="bg-white dark:bg-gray-800 rounded-xl shadow-lg w-full max-w-md p-6 relative"
        onClick={(e) => e.stopPropagation()}
      >
        <h2 className="text-lg font-bold mb-4 text-center dark:text-white">
          Thêm thánh lễ cho {day?.dayOfWeek}, {day?.date}
        </h2>
        <form onSubmit={handleSubmit}>
          <div className="space-y-3 mb-4">
            <div>
              <label className="block text-sm font-medium mb-1">Giờ *</label>
              <input
                type="time"
                className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                value={time}
                onChange={(e) => setTime(e.target.value)}
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium mb-1">
                Loại thánh lễ *
              </label>
              <input
                type="text"
                className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                value={type}
                onChange={(e) => setType(e.target.value)}
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium mb-1">Chủ tế *</label>
              <input
                type="text"
                className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                value={celebrant}
                onChange={(e) => setCelebrant(e.target.value)}
                required
              />
            </div>
            <div>
              <label className="block text-sm font-medium mb-1">
                Tên đặc biệt
              </label>
              <input
                type="text"
                className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                value={specialName}
                onChange={(e) => setSpecialName(e.target.value)}
              />
            </div>
            <div>
              <label className="block text-sm font-medium mb-1">Ghi chú</label>
              <textarea
                className="w-full border px-2 py-2 rounded-lg dark:bg-gray-900 dark:text-white dark:border-gray-700"
                rows={2}
                value={note}
                onChange={(e) => setNote(e.target.value)}
              />
            </div>
            <div className="flex items-center gap-2">
              <input
                type="checkbox"
                checked={isSolemn}
                onChange={(e) => setIsSolemn(e.target.checked)}
                id="isSolemn"
              />
              <label htmlFor="isSolemn" className="text-sm">
                Thánh lễ trọng
              </label>
            </div>
          </div>
          {error && (
            <div className="text-red-600 mb-2 text-center">{error}</div>
          )}
          <div className="flex justify-end gap-2 mt-4">
            <button
              type="button"
              className="px-4 py-2 rounded-lg bg-gray-300 hover:bg-gray-400 text-gray-800"
              onClick={onClose}
            >
              Đóng
            </button>
            <button
              type="submit"
              className="px-4 py-2 rounded-lg bg-blue-600 hover:bg-blue-700 text-white font-semibold"
            >
              Lưu
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default QuickAddMassModal;
