import React from "react";
import { formatDateTime } from "../../../utils/formatDate";

const MedicalHistoryTimeline = ({ histories, loading, error, onRefresh }) => {
  const renderSkeleton = () => (
    <div className="space-y-6">
      {Array.from({ length: 3 }).map((_, index) => (
        <div
          key={`skeleton-${index}`}
          className="relative border-l-2 border-slate-200 pl-8 dark:border-slate-700"
        >
          <span className="absolute -left-[9px] top-0 h-4 w-4 rounded-full bg-slate-200 dark:bg-slate-700" />
          <div className="rounded-2xl border border-slate-100 bg-white p-6 shadow-sm dark:border-slate-800 dark:bg-slate-900">
            <div className="h-4 w-40 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
            <div className="mt-3 h-3 w-3/5 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
            <div className="mt-2 h-3 w-1/2 animate-pulse rounded bg-slate-200 dark:bg-slate-700" />
          </div>
        </div>
      ))}
    </div>
  );

  const renderEmpty = () => (
    <div className="flex flex-col items-center justify-center rounded-3xl border border-dashed border-slate-200 bg-white py-16 text-center dark:border-slate-700 dark:bg-slate-900">
      <h3 className="text-lg font-semibold text-slate-800 dark:text-white">
        Chưa có hồ sơ điều trị
      </h3>
      <p className="mt-2 max-w-md text-sm text-slate-500 dark:text-slate-400">
        Sau mỗi lần thăm khám, bác sĩ sẽ cập nhật chẩn đoán, phác đồ điều trị và
        kết quả xét nghiệm tại đây.
      </p>
      <button
        onClick={onRefresh}
        className="mt-6 inline-flex items-center rounded-full border border-slate-200 px-5 py-2.5 text-sm font-medium text-slate-600 transition hover:border-yellow-400 hover:text-yellow-600 dark:border-slate-700 dark:text-slate-300"
      >
        Làm mới dữ liệu
      </button>
    </div>
  );

  return (
    <div className="space-y-6">
      {error && (
        <div className="rounded-xl border border-rose-200 bg-rose-50 p-4 text-sm text-rose-700 dark:border-rose-900 dark:bg-rose-950/70 dark:text-rose-300">
          {error}
        </div>
      )}

      {loading
        ? renderSkeleton()
        : histories.length === 0
        ? renderEmpty()
        : histories.map((item) => (
            <div
              key={item.medicalHistoryId}
              className="relative border-l-2 border-slate-200 pl-8 dark:border-slate-700"
            >
              <span className="absolute -left-[9px] top-0 flex h-4 w-4 items-center justify-center rounded-full border-2 border-yellow-400 bg-white dark:bg-slate-900" />
              <div className="rounded-2xl border border-slate-100 bg-white p-6 shadow-sm transition hover:shadow-lg dark:border-slate-800 dark:bg-slate-900">
                <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
                  <div>
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Ngày khám
                    </p>
                    <p className="text-sm font-semibold text-slate-800 dark:text-white">
                      {formatDateTime(item.appointmentDate)}
                    </p>
                  </div>
                  <div className="text-xs text-slate-500 dark:text-slate-400">
                    <span className="font-medium text-slate-700 dark:text-slate-200">
                      Bác sĩ: {item.doctorName || "—"}
                    </span>
                    <span className="ml-3">Mã hồ sơ #{item.medicalHistoryId}</span>
                  </div>
                </div>

                <div className="mt-4 grid gap-4 md:grid-cols-2">
                  <div className="rounded-xl bg-slate-50 p-4 dark:bg-slate-800/60">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Chẩn đoán
                    </p>
                    <p className="mt-1 text-sm text-slate-600 dark:text-slate-300">
                      {item.diagnosis || "Bác sĩ chưa cập nhật."}
                    </p>
                  </div>
                  <div className="rounded-xl bg-slate-50 p-4 dark:bg-slate-800/60">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Điều trị
                    </p>
                    <p className="mt-1 text-sm text-slate-600 dark:text-slate-300">
                      {item.treatment || "Bác sĩ chưa cập nhật."}
                    </p>
                  </div>
                </div>

                <div className="mt-4 grid gap-4 md:grid-cols-2">
                  <div className="rounded-xl border border-slate-100 bg-white p-4 dark:border-slate-700 dark:bg-slate-900/80">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Thị lực
                    </p>
                    <div className="mt-2 flex gap-3 text-sm text-slate-600 dark:text-slate-300">
                      <span>Mắt trái: {item.visionLeft ?? "—"}</span>
                      <span>Mắt phải: {item.visionRight ?? "—"}</span>
                    </div>
                  </div>
                  <div className="rounded-xl border border-slate-100 bg-white p-4 dark:border-slate-700 dark:bg-slate-900/80">
                    <p className="text-xs uppercase tracking-wide text-slate-400">
                      Xét nghiệm & ghi chú
                    </p>
                    <p className="mt-2 text-sm text-slate-600 dark:text-slate-300">
                      {item.additionalTests || item.notes || "Không có ghi chú thêm."}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          ))}
    </div>
  );
};

export default MedicalHistoryTimeline;

