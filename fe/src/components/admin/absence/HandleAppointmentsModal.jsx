import React, { useState, useEffect } from "react";
import { X, UserCheck, UserX, AlertCircle, RefreshCw, CheckCircle, XCircle } from "lucide-react";
import { handleAbsenceAppointments } from "../../../services/adminDoctorAbsenceAPI";
import { searchAppointments } from "../../../services/adminAppointmentAPI";
import { searchDoctors } from "../../../services/adminDoctorAPI";
import toast from "react-hot-toast";

const HandleAppointmentsModal = ({
  open,
  onClose,
  absence,
  onSuccess,
}) => {
  const [appointments, setAppointments] = useState([]);
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [autoAssign, setAutoAssign] = useState(true);
  const [manualAssignments, setManualAssignments] = useState({});
  const [appointmentsToCancel, setAppointmentsToCancel] = useState([]);

  useEffect(() => {
    if (open && absence) {
      loadAffectedAppointments();
      loadDoctors();
    }
  }, [open, absence]);

  const loadDoctors = async () => {
    try {
      const response = await searchDoctors({ page: 1, pageSize: 100 });
      const doctorsList = response.data?.data || response.data?.items || [];
      setDoctors(doctorsList);
    } catch (error) {
      console.error("Error loading doctors:", error);
    }
  };

  const loadAffectedAppointments = async () => {
    if (!absence) return;
    setLoading(true);
    try {
      const startDate = absence.startDate + "T00:00:00";
      const endDate = absence.endDate + "T23:59:59";

      const response = await searchAppointments({
        doctorId: absence.doctorId,
        startDate: startDate,
        endDate: endDate,
        status: null, // Get all statuses except cancelled/completed
        page: 1,
        pageSize: 100,
      });

      const appointmentsData = response.data?.data?.items || response.data?.items || [];
      // Filter out cancelled and completed
      const activeAppointments = appointmentsData.filter(
        (apt) =>
          apt.status !== "Cancelled" &&
          apt.status !== "Completed" &&
          apt.status?.toLowerCase() !== "cancelled" &&
          apt.status?.toLowerCase() !== "completed"
      );
      setAppointments(activeAppointments);
    } catch (error) {
      console.error("Error loading appointments:", error);
      toast.error("Không thể tải danh sách lịch hẹn bị ảnh hưởng");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    if (!absence) return;

    setSubmitting(true);
    try {
      const request = {
        autoAssignSubstitute: autoAssign,
        manualSubstituteAssignments: Object.keys(manualAssignments).length > 0
          ? manualAssignments
          : undefined,
        appointmentIdsToCancel: appointmentsToCancel.length > 0
          ? appointmentsToCancel
          : undefined,
      };

      const response = await handleAbsenceAppointments(absence.id, request);
      const result = response.data?.data || {};
      
      toast.success(
        `Đã xử lý: ${result.transferred || 0} chuyển bác sĩ, ${result.cancelled || 0} hủy`
      );
      onSuccess?.();
      onClose();
    } catch (error) {
      console.error("Error handling appointments:", error);
      const message =
        error.response?.data?.message || "Không thể xử lý lịch hẹn";
      toast.error(message);
    } finally {
      setSubmitting(false);
    }
  };

  const toggleManualAssignment = (appointmentId, doctorId) => {
    setManualAssignments((prev) => {
      const newAssignments = { ...prev };
      if (newAssignments[appointmentId] === doctorId) {
        delete newAssignments[appointmentId];
      } else {
        newAssignments[appointmentId] = doctorId;
      }
      return newAssignments;
    });
    // Remove from cancel list if assigned
    setAppointmentsToCancel((prev) =>
      prev.filter((id) => id !== appointmentId)
    );
  };

  const toggleCancel = (appointmentId) => {
    setAppointmentsToCancel((prev) => {
      if (prev.includes(appointmentId)) {
        return prev.filter((id) => id !== appointmentId);
      } else {
        return [...prev, appointmentId];
      }
    });
    // Remove from manual assignments if cancelling
    setManualAssignments((prev) => {
      const newAssignments = { ...prev };
      delete newAssignments[appointmentId];
      return newAssignments;
    });
  };

  if (!open || !absence) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-xl shadow-xl max-w-4xl w-full max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
          <div>
            <h2 className="text-2xl font-bold text-gray-900 dark:text-white flex items-center gap-2">
              <UserCheck className="h-6 w-6 text-orange-600" />
              Xử lý lịch hẹn bị ảnh hưởng
            </h2>
            <p className="text-sm text-gray-600 dark:text-gray-400 mt-1">
              Bác sĩ: {absence.doctorName} |{" "}
              {new Date(absence.startDate).toLocaleDateString("vi-VN")} -{" "}
              {new Date(absence.endDate).toLocaleDateString("vi-VN")}
            </p>
          </div>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
          >
            <X className="h-6 w-6" />
          </button>
        </div>

        {/* Content */}
        {loading ? (
          <div className="p-12 text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            <p className="mt-4 text-gray-600 dark:text-gray-400">
              Đang tải lịch hẹn...
            </p>
          </div>
        ) : appointments.length === 0 ? (
          <div className="p-12 text-center">
            <CheckCircle className="h-16 w-16 text-green-500 mx-auto mb-4" />
            <p className="text-gray-600 dark:text-gray-400">
              Không có lịch hẹn nào bị ảnh hưởng trong thời gian này
            </p>
          </div>
        ) : (
          <div className="p-6 space-y-4">
            {/* Warning */}
            <div className="bg-yellow-50 dark:bg-yellow-900/20 border border-yellow-200 dark:border-yellow-800 rounded-lg p-4">
              <div className="flex items-start gap-3">
                <AlertCircle className="h-5 w-5 text-yellow-600 mt-0.5" />
                <div className="flex-1">
                  <p className="text-sm font-medium text-yellow-800 dark:text-yellow-200">
                    Có {appointments.length} lịch hẹn bị ảnh hưởng
                  </p>
                  <p className="text-xs text-yellow-700 dark:text-yellow-300 mt-1">
                    Vui lòng chọn cách xử lý cho từng lịch hẹn: chuyển sang bác
                    sĩ khác hoặc hủy lịch.
                  </p>
                </div>
              </div>
            </div>

            {/* Auto Assign Option */}
            <div className="flex items-center gap-3 p-4 bg-blue-50 dark:bg-blue-900/20 rounded-lg border border-blue-200 dark:border-blue-800">
              <input
                type="checkbox"
                id="autoAssign"
                checked={autoAssign}
                onChange={(e) => setAutoAssign(e.target.checked)}
                className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <label
                htmlFor="autoAssign"
                className="text-sm font-medium text-blue-900 dark:text-blue-100"
              >
                Tự động chuyển sang bác sĩ thay thế (cùng chuyên khoa, có sẵn)
              </label>
            </div>

            {/* Appointments List */}
            <div className="space-y-3">
              {appointments.map((apt) => {
                const isManuallyAssigned =
                  manualAssignments[apt.appointmentId || apt.id];
                const isCancelled = appointmentsToCancel.includes(
                  apt.appointmentId || apt.id
                );
                const assignedDoctor = isManuallyAssigned
                  ? doctors.find(
                      (d) =>
                        (d.doctorId || d.id) === manualAssignments[apt.appointmentId || apt.id]
                    )
                  : null;

                return (
                  <div
                    key={apt.appointmentId || apt.id}
                    className={`p-4 rounded-lg border-2 ${
                      isCancelled
                        ? "bg-red-50 dark:bg-red-900/20 border-red-300 dark:border-red-700"
                        : isManuallyAssigned
                        ? "bg-green-50 dark:bg-green-900/20 border-green-300 dark:border-green-700"
                        : "bg-gray-50 dark:bg-gray-700 border-gray-200 dark:border-gray-600"
                    }`}
                  >
                    <div className="flex items-start justify-between gap-4">
                      <div className="flex-1">
                        <div className="flex items-center gap-3 mb-2">
                          <p className="font-semibold text-gray-900 dark:text-white">
                            Lịch hẹn #{apt.appointmentCode || apt.appointmentId || apt.id}
                          </p>
                          <span className="px-2 py-1 bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-200 rounded text-xs">
                            {apt.status}
                          </span>
                        </div>
                        <div className="space-y-1 text-sm text-gray-600 dark:text-gray-400">
                          <p>
                            <strong>Ngày giờ:</strong>{" "}
                            {new Date(
                              apt.appointmentDate || apt.appointmentDatetime
                            ).toLocaleString("vi-VN")}
                          </p>
                          {apt.patientName && (
                            <p>
                              <strong>Bệnh nhân:</strong> {apt.patientName}
                            </p>
                          )}
                          {apt.serviceName && (
                            <p>
                              <strong>Dịch vụ:</strong> {apt.serviceName}
                            </p>
                          )}
                        </div>

                        {/* Manual Doctor Selection */}
                        {!autoAssign && !isCancelled && (
                          <div className="mt-3">
                            <label className="block text-xs font-medium text-gray-700 dark:text-gray-300 mb-1">
                              Chọn bác sĩ thay thế:
                            </label>
                            <select
                              value={
                                isManuallyAssigned
                                  ? manualAssignments[apt.appointmentId || apt.id]
                                  : ""
                              }
                              onChange={(e) =>
                                toggleManualAssignment(
                                  apt.appointmentId || apt.id,
                                  Number(e.target.value)
                                )
                              }
                              className="w-full px-3 py-2 text-sm border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                            >
                              <option value="">-- Chọn bác sĩ --</option>
                              {doctors
                                .filter(
                                  (d) =>
                                    (d.doctorId || d.id) !== absence.doctorId
                                )
                                .map((doctor) => (
                                  <option
                                    key={doctor.doctorId || doctor.id}
                                    value={doctor.doctorId || doctor.id}
                                  >
                                    {doctor.doctorName ||
                                      doctor.fullName ||
                                      doctor.name}{" "}
                                    - {doctor.specializationName || ""}
                                  </option>
                                ))}
                            </select>
                          </div>
                        )}

                        {/* Assigned Doctor Display */}
                        {isManuallyAssigned && assignedDoctor && (
                          <div className="mt-2 flex items-center gap-2 text-sm text-green-700 dark:text-green-300">
                            <UserCheck className="h-4 w-4" />
                            <span>
                              Sẽ chuyển sang:{" "}
                              <strong>
                                {assignedDoctor.doctorName ||
                                  assignedDoctor.fullName ||
                                  assignedDoctor.name}
                              </strong>
                            </span>
                          </div>
                        )}
                      </div>

                      {/* Actions */}
                      <div className="flex flex-col gap-2">
                        {!isCancelled && (
                          <button
                            onClick={() => toggleCancel(apt.appointmentId || apt.id)}
                            className={`px-3 py-1 text-xs rounded-lg transition-colors ${
                              isCancelled
                                ? "bg-red-600 text-white"
                                : "bg-gray-200 dark:bg-gray-600 text-gray-700 dark:text-gray-300 hover:bg-red-100 dark:hover:bg-red-900/20"
                            }`}
                          >
                            <XCircle className="h-3 w-3 inline mr-1" />
                            Hủy
                          </button>
                        )}
                        {isCancelled && (
                          <span className="px-3 py-1 text-xs rounded-lg bg-red-100 dark:bg-red-900 text-red-800 dark:text-red-200">
                            Sẽ hủy
                          </span>
                        )}
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>

            {/* Summary */}
            <div className="mt-6 p-4 bg-gray-100 dark:bg-gray-700 rounded-lg">
              <div className="grid grid-cols-3 gap-4 text-center">
                <div>
                  <p className="text-xs text-gray-600 dark:text-gray-400">
                    Tổng lịch hẹn
                  </p>
                  <p className="text-xl font-bold text-gray-900 dark:text-white">
                    {appointments.length}
                  </p>
                </div>
                <div>
                  <p className="text-xs text-gray-600 dark:text-gray-400">
                    Sẽ chuyển
                  </p>
                  <p className="text-xl font-bold text-green-600">
                    {Object.keys(manualAssignments).length +
                      (autoAssign && appointmentsToCancel.length < appointments.length
                        ? appointments.length - appointmentsToCancel.length - Object.keys(manualAssignments).length
                        : 0)}
                  </p>
                </div>
                <div>
                  <p className="text-xs text-gray-600 dark:text-gray-400">
                    Sẽ hủy
                  </p>
                  <p className="text-xl font-bold text-red-600">
                    {appointmentsToCancel.length}
                  </p>
                </div>
              </div>
            </div>
          </div>
        )}

        {/* Footer */}
        <div className="flex items-center justify-end gap-3 p-6 border-t border-gray-200 dark:border-gray-700">
          <button
            onClick={onClose}
            className="px-6 py-2 border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors font-medium"
          >
            Hủy
          </button>
          {appointments.length > 0 && (
            <button
              onClick={handleSubmit}
              disabled={submitting || loading}
              className="px-6 py-2 bg-orange-600 hover:bg-orange-700 text-white rounded-lg transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
            >
              {submitting ? (
                <>
                  <RefreshCw className="h-4 w-4 animate-spin" />
                  Đang xử lý...
                </>
              ) : (
                <>
                  <UserCheck className="h-4 w-4" />
                  Xác nhận xử lý
                </>
              )}
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default HandleAppointmentsModal;

