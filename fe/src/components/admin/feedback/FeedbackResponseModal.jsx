import React, { useState, useEffect } from "react";

const FeedbackResponseModal = ({ open, feedback, onClose, onSave }) => {
  const [responseText, setResponseText] = useState("");

  useEffect(() => {
    if (feedback) {
      setResponseText(feedback.responseText || "");
    } else {
      setResponseText("");
    }
  }, [feedback]);

  if (!open) return null;

  const getRatingStars = (rating) => {
    return Array.from({ length: 5 }, (_, index) => (
      <span
        key={index}
        className={`text-lg ${
          index < rating ? "text-yellow-400" : "text-gray-300"
        }`}
      >
        ★
      </span>
    ));
  };

  const formatDate = (date) => {
    if (!date) return "N/A";
    return new Date(date).toLocaleDateString("vi-VN", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[700px] max-h-[90vh] overflow-y-auto">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          Phản hồi phản hồi
        </h2>

        {feedback && (
          <div className="mb-6 p-4 bg-gray-50 dark:bg-gray-700 rounded-lg">
            <div className="grid grid-cols-2 gap-4 mb-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Bệnh nhân:
                </label>
                <p className="text-sm text-gray-900 dark:text-white">
                  {feedback.patientName || "N/A"}
                </p>
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Đánh giá:
                </label>
                <div className="flex items-center">
                  <div className="flex">
                    {getRatingStars(feedback.rating)}
                  </div>
                  <span className="ml-2 text-sm font-medium">
                    {feedback.rating}/5
                  </span>
                </div>
              </div>
            </div>

            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Nội dung phản hồi:
              </label>
              <p className="text-sm text-gray-900 dark:text-white bg-white dark:bg-gray-600 p-3 rounded border">
                {feedback.feedbackText || "Không có nội dung"}
              </p>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Ngày phản hồi:
              </label>
              <p className="text-sm text-gray-900 dark:text-white">
                {formatDate(feedback.feedbackDate)}
              </p>
            </div>
          </div>
        )}

        <div className="mb-6">
          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
            Phản hồi của bạn *
          </label>
          <textarea
            className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
            value={responseText}
            onChange={(e) => setResponseText(e.target.value)}
            placeholder="Nhập phản hồi của bạn..."
            rows={4}
            required
          />
        </div>

        <div className="flex justify-end gap-2">
          <button
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium transition"
            onClick={() => onSave(responseText)}
          >
            Gửi phản hồi
          </button>
          <button
            className="bg-gray-300 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 px-4 py-2 rounded-md font-medium transition"
            onClick={onClose}
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default FeedbackResponseModal;
