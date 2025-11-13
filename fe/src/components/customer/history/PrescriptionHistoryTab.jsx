import React, { useState } from "react";
import { formatDateTime } from "../../../utils/formatDate";

const PrescriptionHistoryTab = ({ prescriptions, loading, error, onRefresh }) => {
  const [expandedId, setExpandedId] = useState(null);

  const toggleExpand = (id) => {
    setExpandedId((current) => (current === id ? null : id));
  };

  const renderSkeleton = () => (
    <div className="space-y-4">
      {Array.from({ length: 3 }).map((_, index) => (
        <div
          key={`skeleton-${index}`}
          className="rounded-2xl border border-slate-100 bg-white p-6 shadow-sm dark:border-slate-800 dark:bg-slate-900"
        >
          <div className="mb-4 flex items-center justify-between">
            <div className="h-4 w-40 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
            <div className="h-4 w-24 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
          </div>
          <div className="space-y-3">
            <div className="h-3 w-3/5 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
            <div className="h-3 w-2/5 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
          </div>
        </div>
      ))}
    </div>
  );

  const renderEmpty = () => (
    <div className="flex flex-col items-center justify-center rounded-3xl border border-dashed border-slate-200 bg-white py-16 text-center dark:border-slate-700 dark:bg-slate-900">
      <h3 className="text-lg font-semibold text-slate-800 dark:text-white">
        Chưa có đơn thuốc nào
      </h3>
      <p className="mt-2 max-w-md text-sm text-slate-500 dark:text-slate-400">
        Khi bác sĩ kê đơn thuốc cho bạn, nội dung đơn sẽ hiển thị ở đây để bạn
        dễ dàng tra cứu và sử dụng đúng cách.
      </p>
      <button
        onClick={onRefresh}
        className="mt-6 inline-flex items-center rounded-full border border-slate-200 px-5 py-2.5 text-sm font-medium text-slate-600 transition hover:border-yellow-400 hover:text-yellow-600 dark:border-slate-700 dark:text-slate-300"
      >
        Làm mới danh sách
      </button>
    </div>
  );

  return (
    <div className="space-y-5">
      {error && (
        <div className="rounded-xl border border-rose-200 bg-rose-50 p-4 text-sm text-rose-700 dark:border-rose-900 dark:bg-rose-950/70 dark:text-rose-300">
          {error}
        </div>
      )}

      {loading
        ? renderSkeleton()
        : prescriptions.length === 0
        ? renderEmpty()
        : prescriptions.map((prescription) => {
            const isExpanded = expandedId === prescription.prescriptionId;
            const hasLines = (prescription.lines || []).length > 0;

            return (
              <div
                key={prescription.prescriptionId}
                className="rounded-2xl border border-slate-100 bg-white shadow-sm transition hover:shadow-lg dark:border-slate-800 dark:bg-slate-900"
              >
                <button
                  onClick={() => toggleExpand(prescription.prescriptionId)}
                  className="flex w-full items-center justify-between gap-4 rounded-t-2xl bg-slate-50 px-6 py-4 text-left transition dark:bg-slate-800/60"
                >
                  <div>
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Ngày kê đơn
                    </p>
                    <p className="text-sm font-semibold text-slate-800 dark:text-white">
                      {formatDateTime(prescription.createdAt)}
                    </p>
                    <p className="text-xs text-slate-500 dark:text-slate-400">
                      Bác sĩ:{" "}
                      <span className="font-medium text-slate-700 dark:text-slate-200">
                        {prescription.doctorName || "Đang cập nhật"}
                      </span>
                    </p>
                  </div>
                  <div className="flex flex-col items-end text-xs text-slate-500 dark:text-slate-400">
                    <span>Mã đơn: #{prescription.prescriptionId}</span>
                    <span>Trạng thái: {prescription.encounterStatus}</span>
                  </div>
                </button>

                <div className="px-6 py-4">
                  <div className="mb-4 text-sm text-slate-600 dark:text-slate-300">
                    {prescription.notes ? (
                      <p>{prescription.notes}</p>
                    ) : (
                      <p className="italic text-slate-400">
                        Bác sĩ không để lại ghi chú.
                      </p>
                    )}
                  </div>

                  {hasLines ? (
                    <div className="overflow-hidden rounded-xl border border-slate-100 dark:border-slate-800">
                      <div className="hidden bg-slate-50 text-xs font-semibold uppercase tracking-wide text-slate-500 dark:bg-slate-800/60 dark:text-slate-300 sm:grid sm:grid-cols-[2fr,1fr,1fr,1.5fr]">
                        <span className="px-4 py-3">Tên thuốc</span>
                        <span className="px-4 py-3">Liều dùng</span>
                        <span className="px-4 py-3">Tần suất</span>
                        <span className="px-4 py-3">Hướng dẫn</span>
                      </div>
                      <div className="divide-y divide-slate-100 dark:divide-slate-800">
                        {(isExpanded ? prescription.lines : prescription.lines.slice(0, 2)).map(
                          (line) => (
                            <div
                              key={line.lineId}
                              className="grid grid-cols-1 gap-3 px-4 py-4 text-sm text-slate-600 dark:text-slate-300 sm:grid-cols-[2fr,1fr,1fr,1.5fr]"
                            >
                              <div>
                                <p className="font-semibold text-slate-800 dark:text-white">
                                  {line.drugName}
                                </p>
                                {line.drugCode && (
                                  <p className="text-xs text-slate-400">
                                    Mã thuốc: {line.drugCode}
                                  </p>
                                )}
                              </div>
                              <div>{line.dosage || "—"}</div>
                              <div>{line.frequency || "—"}</div>
                              <div>{line.instructions || line.duration || "—"}</div>
                            </div>
                          )
                        )}
                      </div>
                    </div>
                  ) : (
                    <div className="rounded-xl border border-dashed border-slate-200 bg-slate-50 p-4 text-sm text-slate-500 dark:border-slate-700 dark:bg-slate-900/60 dark:text-slate-400">
                      Đơn thuốc chưa có thông tin chi tiết về các loại thuốc.
                    </div>
                  )}

                  {hasLines && prescription.lines.length > 2 && (
                    <div className="mt-4 flex justify-center">
                      <button
                        onClick={() => toggleExpand(prescription.prescriptionId)}
                        className="text-sm font-semibold text-yellow-600 transition hover:text-yellow-500"
                      >
                        {isExpanded ? "Thu gọn" : "Xem thêm thuốc"}
                      </button>
                    </div>
                  )}
                </div>
              </div>
            );
          })}
    </div>
  );
};

export default PrescriptionHistoryTab;

