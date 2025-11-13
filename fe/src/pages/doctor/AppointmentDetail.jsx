import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  getAppointmentById,
  getMedicalHistoryByAppointment,
  createMedicalHistory,
  updateMedicalHistory,
} from "../../services/doctorAppointmentAPI";
import {
  getEncounterByAppointment,
  createEncounter,
  updateEncounter,
  createPrescription,
  getPrescriptionsByEncounter,
  createOrder,
  getOrdersByEncounter,
} from "../../services/doctorEhrAPI";
import {
  confirmMyAppointment,
  completeMyAppointment,
  cancelMyAppointment,
} from "../../services/doctorMeAPI";
import { Calendar, User, Clock, FileText, Pill, ClipboardList, ArrowLeft, CheckCircle, XCircle } from "lucide-react";

const AppointmentDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [appointment, setAppointment] = useState(null);
  const [encounter, setEncounter] = useState(null);
  const [medicalHistory, setMedicalHistory] = useState(null);
  const [prescriptions, setPrescriptions] = useState([]);
  const [orders, setOrders] = useState([]);
  const [activeTab, setActiveTab] = useState("info");

  // Encounter form
  const [encounterForm, setEncounterForm] = useState({
    subjective: "",
    objective: "",
    assessment: "",
    plan: "",
  });

  // Prescription form
  const [prescriptionForm, setPrescriptionForm] = useState({
    drugName: "",
    drugCode: "",
    dosage: "",
    frequency: "",
    duration: "",
    instructions: "",
  });

  // Medical History form
  const [medicalHistoryForm, setMedicalHistoryForm] = useState({
    diagnosis: "",
    symptoms: "",
    treatment: "",
    prescription: "",
    visionLeft: "",
    visionRight: "",
    additionalTests: "",
    notes: "",
  });

  useEffect(() => {
    loadData();
  }, [id]);

  const loadData = async () => {
    setLoading(true);
    try {
      const [appt, enc, mh] = await Promise.all([
        getAppointmentById(id),
        getEncounterByAppointment(id).catch(() => null),
        getMedicalHistoryByAppointment(id).catch(() => null),
      ]);

      setAppointment(appt);
      setEncounter(enc);
      setMedicalHistory(mh);

      if (enc) {
        setEncounterForm({
          subjective: enc.subjective || "",
          objective: enc.objective || "",
          assessment: enc.assessment || "",
          plan: enc.plan || "",
        });

        // Load prescriptions and orders
        const [rxList, orderList] = await Promise.all([
          getPrescriptionsByEncounter(enc.id).catch(() => []),
          getOrdersByEncounter(enc.id).catch(() => []),
        ]);
        setPrescriptions(Array.isArray(rxList) ? rxList : []);
        setOrders(Array.isArray(orderList) ? orderList : []);
      }

      if (mh) {
        setMedicalHistoryForm({
          diagnosis: mh.diagnosis || "",
          symptoms: mh.symptoms || "",
          treatment: mh.treatment || "",
          prescription: mh.prescription || "",
          visionLeft: mh.visionLeft || "",
          visionRight: mh.visionRight || "",
          additionalTests: mh.additionalTests || "",
          notes: mh.notes || "",
        });
      }
    } catch (error) {
      console.error("Error loading appointment data:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreateEncounter = async (e) => {
    e.preventDefault();
    if (!appointment) return;

    try {
      const created = await createEncounter(
        {
          appointmentId: parseInt(id),
          subjective: encounterForm.subjective,
          objective: encounterForm.objective,
          assessment: encounterForm.assessment,
          plan: encounterForm.plan,
        },
        appointment.patientId
      );
      setEncounter(created);
      alert("Tạo hồ sơ khám thành công!");
    } catch (error) {
      console.error("Error creating encounter:", error);
      alert("Lỗi khi tạo hồ sơ khám: " + (error.response?.data?.message || error.message));
    }
  };

  const handleUpdateEncounter = async (e) => {
    e.preventDefault();
    if (!encounter) return;

    try {
      const updated = await updateEncounter(encounter.id, {
        subjective: encounterForm.subjective,
        objective: encounterForm.objective,
        assessment: encounterForm.assessment,
        plan: encounterForm.plan,
      });
      setEncounter(updated);
      alert("Cập nhật hồ sơ khám thành công!");
    } catch (error) {
      console.error("Error updating encounter:", error);
      alert("Lỗi khi cập nhật hồ sơ khám: " + (error.response?.data?.message || error.message));
    }
  };

  const handleAddPrescription = async (e) => {
    e.preventDefault();
    if (!encounter) {
      alert("Vui lòng tạo hồ sơ khám (Encounter) trước!");
      return;
    }

    try {
      await createPrescription({
        encounterId: encounter.id,
        notes: null,
        lines: [
          {
            drugName: prescriptionForm.drugName,
            drugCode: prescriptionForm.drugCode || "",
            dosage: prescriptionForm.dosage,
            frequency: prescriptionForm.frequency,
            duration: prescriptionForm.duration,
            instructions: prescriptionForm.instructions,
          },
        ],
      });

      setPrescriptionForm({
        drugName: "",
        drugCode: "",
        dosage: "",
        frequency: "",
        duration: "",
        instructions: "",
      });

      // Reload prescriptions
      const rxList = await getPrescriptionsByEncounter(encounter.id);
      setPrescriptions(Array.isArray(rxList) ? rxList : []);
      alert("Thêm đơn thuốc thành công!");
    } catch (error) {
      console.error("Error creating prescription:", error);
      alert("Lỗi khi thêm đơn thuốc: " + (error.response?.data?.message || error.message));
    }
  };

  const handleSaveMedicalHistory = async (e) => {
    e.preventDefault();
    if (!appointment) return;

    try {
      if (medicalHistory) {
        // Update existing
        await updateMedicalHistory(medicalHistory.id, {
          appointmentId: parseInt(id),
          ...medicalHistoryForm,
          visionLeft: medicalHistoryForm.visionLeft ? parseFloat(medicalHistoryForm.visionLeft) : null,
          visionRight: medicalHistoryForm.visionRight ? parseFloat(medicalHistoryForm.visionRight) : null,
        });
        alert("Cập nhật hồ sơ bệnh án thành công!");
      } else {
        // Create new
        const created = await createMedicalHistory({
          appointmentId: parseInt(id),
          ...medicalHistoryForm,
          visionLeft: medicalHistoryForm.visionLeft ? parseFloat(medicalHistoryForm.visionLeft) : null,
          visionRight: medicalHistoryForm.visionRight ? parseFloat(medicalHistoryForm.visionRight) : null,
        });
        setMedicalHistory(created);
        alert("Tạo hồ sơ bệnh án thành công!");
      }
      await loadData();
    } catch (error) {
      console.error("Error saving medical history:", error);
      alert("Lỗi khi lưu hồ sơ bệnh án: " + (error.response?.data?.message || error.message));
    }
  };

  const handleStatusChange = async (action) => {
    if (!appointment) return;

    try {
      if (action === "confirm") {
        await confirmMyAppointment(id);
      } else if (action === "complete") {
        await completeMyAppointment(id);
      } else if (action === "cancel") {
        const reason = window.prompt("Lý do hủy:");
        if (reason !== null) {
          await cancelMyAppointment(id, reason);
        } else {
          return;
        }
      }
      await loadData();
      alert(`Cập nhật trạng thái thành công!`);
    } catch (error) {
      console.error("Error updating status:", error);
      alert("Lỗi khi cập nhật trạng thái: " + (error.response?.data?.message || error.message));
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-gray-600 dark:text-gray-300">Đang tải...</div>
      </div>
    );
  }

  if (!appointment) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <p className="text-gray-600 dark:text-gray-300 mb-4">Không tìm thấy cuộc hẹn</p>
          <button
            onClick={() => navigate("/doctor/schedule")}
            className="px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700"
          >
            Quay lại
          </button>
        </div>
      </div>
    );
  }

  const StatusBadge = ({ status }) => {
    const statusMap = {
      Pending: "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-300",
      Scheduled: "bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300",
      Confirmed: "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-300",
      Completed: "bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300",
      Canceled: "bg-red-100 text-red-800 dark:bg-red-900/30 dark:text-red-300",
    };
    return (
      <span className={`px-3 py-1 rounded-full text-sm font-medium ${statusMap[status] || statusMap.Pending}`}>
        {status}
      </span>
    );
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <button
            onClick={() => navigate("/doctor/schedule")}
            className="p-2 text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 rounded-md"
          >
            <ArrowLeft className="h-5 w-5" />
          </button>
          <div>
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Chi tiết cuộc hẹn</h1>
            <p className="text-gray-600 dark:text-gray-300">Mã: {appointment.appointmentCode || `#${id}`}</p>
          </div>
        </div>
        <div className="flex items-center space-x-3">
          <StatusBadge status={appointment.status || appointment.appointmentStatus} />
          {appointment.status === "Pending" || appointment.status === "Scheduled" ? (
            <button
              onClick={() => handleStatusChange("confirm")}
              className="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
              <CheckCircle className="h-4 w-4 mr-2" />
              Xác nhận
            </button>
          ) : null}
          {appointment.status === "Confirmed" ? (
            <button
              onClick={() => handleStatusChange("complete")}
              className="inline-flex items-center px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
            >
              <CheckCircle className="h-4 w-4 mr-2" />
              Hoàn thành
            </button>
          ) : null}
          {appointment.status !== "Completed" && appointment.status !== "Canceled" ? (
            <button
              onClick={() => handleStatusChange("cancel")}
              className="inline-flex items-center px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700"
            >
              <XCircle className="h-4 w-4 mr-2" />
              Hủy
            </button>
          ) : null}
        </div>
      </div>

      {/* Appointment Info Card */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Thông tin cuộc hẹn</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="flex items-start space-x-3">
            <User className="h-5 w-5 text-gray-400 mt-0.5" />
            <div>
              <p className="text-sm text-gray-500 dark:text-gray-400">Bệnh nhân</p>
              <p className="font-medium text-gray-900 dark:text-white">
                {appointment.patientName || `Bệnh nhân #${appointment.patientId}`}
              </p>
            </div>
          </div>
          <div className="flex items-start space-x-3">
            <Calendar className="h-5 w-5 text-gray-400 mt-0.5" />
            <div>
              <p className="text-sm text-gray-500 dark:text-gray-400">Ngày giờ</p>
              <p className="font-medium text-gray-900 dark:text-white">
                {new Date(appointment.appointmentDate || appointment.appointmentDatetime).toLocaleString("vi-VN")}
              </p>
            </div>
          </div>
          <div className="flex items-start space-x-3">
            <FileText className="h-5 w-5 text-gray-400 mt-0.5" />
            <div>
              <p className="text-sm text-gray-500 dark:text-gray-400">Dịch vụ</p>
              <p className="font-medium text-gray-900 dark:text-white">
                {appointment.serviceName || appointment.serviceDetailName || "N/A"}
              </p>
            </div>
          </div>
          <div className="flex items-start space-x-3">
            <Clock className="h-5 w-5 text-gray-400 mt-0.5" />
            <div>
              <p className="text-sm text-gray-500 dark:text-gray-400">Trạng thái thanh toán</p>
              <p className="font-medium text-gray-900 dark:text-white">
                {appointment.paymentStatus || "Chưa thanh toán"}
              </p>
            </div>
          </div>
        </div>
        {appointment.notes && (
          <div className="mt-4 pt-4 border-t border-gray-200 dark:border-gray-700">
            <p className="text-sm text-gray-500 dark:text-gray-400">Ghi chú</p>
            <p className="text-gray-900 dark:text-white">{appointment.notes}</p>
          </div>
        )}
      </div>

      {/* Tabs */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg">
        <div className="border-b border-gray-200 dark:border-gray-700">
          <nav className="flex space-x-8 px-6" aria-label="Tabs">
            {[
              { id: "info", label: "Thông tin", icon: FileText },
              { id: "encounter", label: "Hồ sơ khám (SOAP)", icon: ClipboardList },
              { id: "prescription", label: "Đơn thuốc", icon: Pill },
              { id: "medical", label: "Hồ sơ bệnh án", icon: FileText },
            ].map((tab) => {
              const Icon = tab.icon;
              return (
                <button
                  key={tab.id}
                  onClick={() => setActiveTab(tab.id)}
                  className={`${
                    activeTab === tab.id
                      ? "border-indigo-500 text-indigo-600 dark:text-indigo-400"
                      : "border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300"
                  } flex items-center space-x-2 py-4 px-1 border-b-2 font-medium text-sm`}
                >
                  <Icon className="h-4 w-4" />
                  <span>{tab.label}</span>
                </button>
              );
            })}
          </nav>
        </div>

        <div className="p-6">
          {/* Encounter Tab */}
          {activeTab === "encounter" && (
            <div className="space-y-6">
              <div>
                <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">
                  Hồ sơ khám (SOAP Notes)
                </h3>
                {encounter ? (
                  <div className="mb-4 p-4 bg-green-50 dark:bg-green-900/20 rounded-md">
                    <p className="text-sm text-green-800 dark:text-green-300">
                      Hồ sơ khám đã được tạo. Trạng thái: <strong>{encounter.status}</strong>
                    </p>
                  </div>
                ) : (
                  <div className="mb-4 p-4 bg-yellow-50 dark:bg-yellow-900/20 rounded-md">
                    <p className="text-sm text-yellow-800 dark:text-yellow-300">
                      Chưa có hồ sơ khám. Vui lòng tạo mới.
                    </p>
                  </div>
                )}

                <form onSubmit={encounter ? handleUpdateEncounter : handleCreateEncounter} className="space-y-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                      Subjective (Chủ quan)
                    </label>
                    <textarea
                      rows={3}
                      value={encounterForm.subjective}
                      onChange={(e) => setEncounterForm({ ...encounterForm, subjective: e.target.value })}
                      className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                      placeholder="Triệu chứng, lời kể của bệnh nhân..."
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                      Objective (Khách quan)
                    </label>
                    <textarea
                      rows={3}
                      value={encounterForm.objective}
                      onChange={(e) => setEncounterForm({ ...encounterForm, objective: e.target.value })}
                      className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                      placeholder="Kết quả khám, xét nghiệm..."
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                      Assessment (Đánh giá)
                    </label>
                    <textarea
                      rows={3}
                      value={encounterForm.assessment}
                      onChange={(e) => setEncounterForm({ ...encounterForm, assessment: e.target.value })}
                      className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                      placeholder="Chẩn đoán..."
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                      Plan (Kế hoạch)
                    </label>
                    <textarea
                      rows={3}
                      value={encounterForm.plan}
                      onChange={(e) => setEncounterForm({ ...encounterForm, plan: e.target.value })}
                      className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                      placeholder="Kế hoạch điều trị..."
                    />
                  </div>
                  <button
                    type="submit"
                    className="px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700"
                  >
                    {encounter ? "Cập nhật" : "Tạo"} hồ sơ khám
                  </button>
                </form>
              </div>
            </div>
          )}

          {/* Prescription Tab */}
          {activeTab === "prescription" && (
            <div className="space-y-6">
              <div>
                <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Đơn thuốc</h3>
                {!encounter ? (
                  <div className="p-4 bg-yellow-50 dark:bg-yellow-900/20 rounded-md">
                    <p className="text-sm text-yellow-800 dark:text-yellow-300">
                      Vui lòng tạo hồ sơ khám (Encounter) trước khi thêm đơn thuốc.
                    </p>
                  </div>
                ) : (
                  <>
                    <form onSubmit={handleAddPrescription} className="bg-gray-50 dark:bg-gray-700/50 p-4 rounded-md mb-6">
                      <h4 className="font-medium text-gray-900 dark:text-white mb-4">Thêm thuốc mới</h4>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div>
                          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Tên thuốc *
                          </label>
                          <input
                            type="text"
                            required
                            value={prescriptionForm.drugName}
                            onChange={(e) => setPrescriptionForm({ ...prescriptionForm, drugName: e.target.value })}
                            className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Mã thuốc
                          </label>
                          <input
                            type="text"
                            value={prescriptionForm.drugCode}
                            onChange={(e) => setPrescriptionForm({ ...prescriptionForm, drugCode: e.target.value })}
                            className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Liều lượng *
                          </label>
                          <input
                            type="text"
                            required
                            value={prescriptionForm.dosage}
                            onChange={(e) => setPrescriptionForm({ ...prescriptionForm, dosage: e.target.value })}
                            className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                            placeholder="VD: 500mg"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Tần suất *
                          </label>
                          <input
                            type="text"
                            required
                            value={prescriptionForm.frequency}
                            onChange={(e) => setPrescriptionForm({ ...prescriptionForm, frequency: e.target.value })}
                            className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                            placeholder="VD: 2 lần/ngày"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Thời gian *
                          </label>
                          <input
                            type="text"
                            required
                            value={prescriptionForm.duration}
                            onChange={(e) => setPrescriptionForm({ ...prescriptionForm, duration: e.target.value })}
                            className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                            placeholder="VD: 7 ngày"
                          />
                        </div>
                        <div>
                          <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                            Hướng dẫn
                          </label>
                          <input
                            type="text"
                            value={prescriptionForm.instructions}
                            onChange={(e) => setPrescriptionForm({ ...prescriptionForm, instructions: e.target.value })}
                            className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                            placeholder="VD: Uống sau ăn"
                          />
                        </div>
                      </div>
                      <button
                        type="submit"
                        className="mt-4 px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700"
                      >
                        Thêm thuốc
                      </button>
                    </form>

                    <div>
                      <h4 className="font-medium text-gray-900 dark:text-white mb-4">Danh sách đơn thuốc</h4>
                      {prescriptions.length === 0 ? (
                        <p className="text-gray-500 dark:text-gray-400">Chưa có đơn thuốc nào</p>
                      ) : (
                        <div className="space-y-4">
                          {prescriptions.map((rx) => (
                            <div
                              key={rx.id}
                              className="border border-gray-200 dark:border-gray-700 rounded-md p-4"
                            >
                              <div className="flex items-center justify-between mb-2">
                                <span className="font-medium text-gray-900 dark:text-white">
                                  Đơn thuốc #{rx.id}
                                </span>
                                <span className="text-sm text-gray-500 dark:text-gray-400">
                                  {new Date(rx.createdAt).toLocaleDateString("vi-VN")}
                                </span>
                              </div>
                              {rx.lines && rx.lines.length > 0 ? (
                                <div className="space-y-2">
                                  {rx.lines.map((line, idx) => (
                                    <div key={idx} className="text-sm text-gray-700 dark:text-gray-300">
                                      <strong>{line.drugName}</strong> - {line.dosage} - {line.frequency} -{" "}
                                      {line.duration}
                                      {line.instructions && ` - ${line.instructions}`}
                                    </div>
                                  ))}
                                </div>
                              ) : null}
                            </div>
                          ))}
                        </div>
                      )}
                    </div>
                  </>
                )}
              </div>
            </div>
          )}

          {/* Medical History Tab */}
          {activeTab === "medical" && (
            <div className="space-y-6">
              <div>
                <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Hồ sơ bệnh án</h3>
                <form onSubmit={handleSaveMedicalHistory} className="space-y-4">
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Chẩn đoán
                      </label>
                      <textarea
                        rows={3}
                        value={medicalHistoryForm.diagnosis}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, diagnosis: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="Chẩn đoán bệnh..."
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Triệu chứng
                      </label>
                      <textarea
                        rows={3}
                        value={medicalHistoryForm.symptoms}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, symptoms: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="Triệu chứng bệnh nhân..."
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Điều trị
                      </label>
                      <textarea
                        rows={3}
                        value={medicalHistoryForm.treatment}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, treatment: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="Phương pháp điều trị..."
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Đơn thuốc (text)
                      </label>
                      <textarea
                        rows={3}
                        value={medicalHistoryForm.prescription}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, prescription: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="Ghi chú đơn thuốc..."
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Thị lực trái
                      </label>
                      <input
                        type="number"
                        step="0.1"
                        min="0"
                        max="2"
                        value={medicalHistoryForm.visionLeft}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, visionLeft: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="0.0 - 2.0"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Thị lực phải
                      </label>
                      <input
                        type="number"
                        step="0.1"
                        min="0"
                        max="2"
                        value={medicalHistoryForm.visionRight}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, visionRight: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="0.0 - 2.0"
                      />
                    </div>
                    <div className="md:col-span-2">
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Xét nghiệm bổ sung
                      </label>
                      <textarea
                        rows={2}
                        value={medicalHistoryForm.additionalTests}
                        onChange={(e) =>
                          setMedicalHistoryForm({ ...medicalHistoryForm, additionalTests: e.target.value })
                        }
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="Các xét nghiệm đã thực hiện..."
                      />
                    </div>
                    <div className="md:col-span-2">
                      <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                        Ghi chú
                      </label>
                      <textarea
                        rows={3}
                        value={medicalHistoryForm.notes}
                        onChange={(e) => setMedicalHistoryForm({ ...medicalHistoryForm, notes: e.target.value })}
                        className="w-full border border-gray-300 dark:border-gray-600 rounded-md px-3 py-2 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
                        placeholder="Ghi chú thêm..."
                      />
                    </div>
                  </div>
                  <button
                    type="submit"
                    className="px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700"
                  >
                    {medicalHistory ? "Cập nhật" : "Tạo"} hồ sơ bệnh án
                  </button>
                </form>
              </div>
            </div>
          )}

          {/* Info Tab */}
          {activeTab === "info" && (
            <div className="space-y-4">
              <div>
                <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Thông tin chi tiết</h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400">Mã cuộc hẹn</p>
                    <p className="font-medium text-gray-900 dark:text-white">
                      {appointment.appointmentCode || `#${id}`}
                    </p>
                  </div>
                  <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400">Trạng thái</p>
                    <StatusBadge status={appointment.status || appointment.appointmentStatus} />
                  </div>
                  <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400">Bệnh nhân</p>
                    <p className="font-medium text-gray-900 dark:text-white">
                      {appointment.patientName || `Bệnh nhân #${appointment.patientId}`}
                    </p>
                  </div>
                  <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400">Ngày giờ</p>
                    <p className="font-medium text-gray-900 dark:text-white">
                      {new Date(appointment.appointmentDate || appointment.appointmentDatetime).toLocaleString("vi-VN")}
                    </p>
                  </div>
                  <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400">Dịch vụ</p>
                    <p className="font-medium text-gray-900 dark:text-white">
                      {appointment.serviceName || appointment.serviceDetailName || "N/A"}
                    </p>
                  </div>
                  <div>
                    <p className="text-sm text-gray-500 dark:text-gray-400">Thanh toán</p>
                    <p className="font-medium text-gray-900 dark:text-white">
                      {appointment.paymentStatus || "Chưa thanh toán"}
                    </p>
                  </div>
                  {appointment.totalAmount && (
                    <div>
                      <p className="text-sm text-gray-500 dark:text-gray-400">Tổng tiền</p>
                      <p className="font-medium text-gray-900 dark:text-white">
                        {new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(
                          appointment.totalAmount
                        )}
                      </p>
                    </div>
                  )}
                </div>
                {appointment.notes && (
                  <div className="mt-4 pt-4 border-t border-gray-200 dark:border-gray-700">
                    <p className="text-sm text-gray-500 dark:text-gray-400 mb-2">Ghi chú</p>
                    <p className="text-gray-900 dark:text-white">{appointment.notes}</p>
                  </div>
                )}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AppointmentDetail;

