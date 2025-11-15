import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { listEncounters, createEncounter, updateEncounter, createPrescription, getPrescriptionsByEncounter, createOrder, getOrdersByEncounter } from "../../services/doctorEhrAPI";
import { getAppointmentById } from "../../services/doctorAppointmentAPI";
import { FileText, Pill, ClipboardList, Plus, Eye, Calendar } from "lucide-react";
import toast from "react-hot-toast";

const EncounterPage = () => {
  const navigate = useNavigate();
  const [encounters, setEncounters] = useState([]);
  const [selected, setSelected] = useState(null);
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({ appointmentId: "", subjective: "", objective: "", assessment: "", plan: "" });
  const [rx, setRx] = useState({ drugName: "", drugCode: "", dosage: "", frequency: "", duration: "", instructions: "" });
  const [ord, setOrd] = useState({ orderType: "Test", name: "", notes: "" });
  const [rxList, setRxList] = useState([]);
  const [orderList, setOrderList] = useState([]);

  const load = async () => {
    setLoading(true);
    try {
    const res = await listEncounters();
      const encountersData = Array.isArray(res) ? res : (Array.isArray(res?.data) ? res.data : []);
      setEncounters(encountersData);
    } catch (error) {
      console.error("Load encounters error", error);
      toast.error("Không thể tải danh sách hồ sơ khám");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const onCreate = async (e) => {
    e.preventDefault();
    if (!form.appointmentId) {
      toast.error("Vui lòng nhập mã lịch hẹn");
      return;
    }
    try {
      // Get appointment to find customerId
      let customerId = null;
      try {
        const appt = await getAppointmentById(Number(form.appointmentId));
        customerId = appt?.patientId || appt?.PatientId || appt?.data?.patientId || appt?.data?.PatientId;
      } catch (error) {
        console.warn("Could not fetch appointment, will prompt for customerId", error);
      }
      
      if (!customerId) {
        customerId = window.prompt("Nhập CustomerId (account_id) của bệnh nhân:") || "";
        if (!customerId) {
          toast.error("Vui lòng nhập CustomerId");
          return;
        }
      }

    await createEncounter({
      appointmentId: Number(form.appointmentId),
      subjective: form.subjective,
      objective: form.objective,
      assessment: form.assessment,
      plan: form.plan,
    }, Number(customerId));
      
      toast.success("Tạo hồ sơ khám thành công");
    setForm({ appointmentId: "", subjective: "", objective: "", assessment: "", plan: "" });
    await load();
    } catch (error) {
      console.error("Create encounter error", error);
      toast.error(error.response?.data?.message || "Lỗi khi tạo hồ sơ khám");
    }
  };

  const onSelect = async (e) => {
    setSelected(e);
    try {
      const [rx, orders] = await Promise.all([
        getPrescriptionsByEncounter(e.id || e.Id).catch(() => []),
        getOrdersByEncounter(e.id || e.Id).catch(() => [])
      ]);
    setRxList(Array.isArray(rx) ? rx : []);
    setOrderList(Array.isArray(orders) ? orders : []);
    } catch (error) {
      console.error("Load encounter details error", error);
      toast.error("Không thể tải chi tiết hồ sơ khám");
    }
  };

  const addRx = async (e) => {
    e.preventDefault();
    if (!selected) {
      toast.error("Vui lòng chọn một hồ sơ khám trước");
      return;
    }
    if (!rx.drugName) {
      toast.error("Vui lòng nhập tên thuốc");
      return;
    }
    try {
    await createPrescription({
        encounterId: selected.id || selected.Id,
      notes: null,
      lines: [{
        drugName: rx.drugName,
          drugCode: rx.drugCode || null,
        dosage: rx.dosage,
        frequency: rx.frequency,
        duration: rx.duration,
        instructions: rx.instructions,
      }]
    });
      toast.success("Thêm thuốc thành công");
      setRx({ drugName: "", drugCode: "", dosage: "", frequency: "", duration: "", instructions: "" });
    await onSelect(selected);
    } catch (error) {
      console.error("Add prescription error", error);
      toast.error(error.response?.data?.message || "Lỗi khi thêm thuốc");
    }
  };

  const addOrder = async (e) => {
    e.preventDefault();
    if (!selected) {
      toast.error("Vui lòng chọn một hồ sơ khám trước");
      return;
    }
    if (!ord.name) {
      toast.error("Vui lòng nhập tên chỉ định");
      return;
    }
    try {
    await createOrder({
        encounterId: selected.id || selected.Id,
      orderType: ord.orderType,
      name: ord.name,
      notes: ord.notes,
    });
      toast.success("Thêm chỉ định thành công");
    setOrd({ orderType: "Test", name: "", notes: "" });
    await onSelect(selected);
    } catch (error) {
      console.error("Add order error", error);
      toast.error(error.response?.data?.message || "Lỗi khi thêm chỉ định");
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Hồ sơ khám (SOAP Notes)</h1>
        <p className="text-gray-600 dark:text-gray-300">
          Quản lý hồ sơ khám chi tiết theo chuẩn SOAP, đơn thuốc và chỉ định cho bệnh nhân
        </p>
        <div className="mt-2 p-3 bg-blue-50 dark:bg-blue-900/20 border border-blue-200 dark:border-blue-800 rounded-lg">
          <p className="text-sm text-blue-800 dark:text-blue-300">
            <strong>Lưu ý:</strong> Đây là <strong>Hồ sơ khám (Encounter)</strong> - ghi chép chi tiết quá trình khám theo SOAP. 
            Để xem <strong>Hồ sơ bệnh án (Medical History)</strong> - lịch sử khám tổng quan, vui lòng vào trang <strong>Bệnh nhân</strong> và chọn "Xem hồ sơ bệnh án".
          </p>
        </div>
      </div>

      {/* Create Encounter Form */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center gap-2">
          <FileText className="h-5 w-5" />
          Tạo hồ sơ khám mới
        </h3>
        <form onSubmit={onCreate} className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Mã lịch hẹn (Appointment ID) *
              </label>
              <input
                type="number"
                required
                className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2"
                placeholder="Nhập mã lịch hẹn"
                value={form.appointmentId}
                onChange={(e) => setForm({ ...form, appointmentId: e.target.value })}
              />
            </div>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Subjective (Chủ quan) - Triệu chứng bệnh nhân mô tả
              </label>
              <textarea
                rows={3}
                className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2"
                placeholder="Bệnh nhân mô tả triệu chứng..."
                value={form.subjective}
                onChange={(e) => setForm({ ...form, subjective: e.target.value })}
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Objective (Khách quan) - Kết quả khám, đo lường
              </label>
              <textarea
                rows={3}
                className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2"
                placeholder="Kết quả khám, đo lường..."
                value={form.objective}
                onChange={(e) => setForm({ ...form, objective: e.target.value })}
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Assessment (Đánh giá) - Chẩn đoán
              </label>
              <textarea
                rows={3}
                className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2"
                placeholder="Chẩn đoán bệnh..."
                value={form.assessment}
                onChange={(e) => setForm({ ...form, assessment: e.target.value })}
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                Plan (Kế hoạch) - Phương án điều trị
              </label>
              <textarea
                rows={3}
                className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2"
                placeholder="Kế hoạch điều trị..."
                value={form.plan}
                onChange={(e) => setForm({ ...form, plan: e.target.value })}
              />
            </div>
        </div>
          <button
            type="submit"
            className="px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 flex items-center gap-2"
          >
            <Plus className="h-4 w-4" />
            Tạo hồ sơ khám
          </button>
      </form>
      </div>

      {/* Encounters List */}
      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center gap-2">
          <ClipboardList className="h-5 w-5" />
          Danh sách hồ sơ khám
        </h3>
        {loading && (
          <div className="text-center py-8">
            <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>
          </div>
        )}
        {!loading && encounters.length === 0 && (
          <div className="text-center py-8">
            <p className="text-gray-500 dark:text-gray-300">Chưa có hồ sơ khám nào</p>
            <p className="text-sm text-gray-400 dark:text-gray-500 mt-2">
              Tạo hồ sơ khám mới bằng form phía trên
            </p>
          </div>
        )}
        {!loading && encounters.length > 0 && (
          <div className="space-y-3">
            {encounters.map((e) => {
              const isSelected = selected && (selected.id === e.id || selected.Id === e.Id);
              return (
                <div
                  key={e.id || e.Id}
                  className={`border rounded-lg p-4 transition-all cursor-pointer ${
                    isSelected
                      ? "border-indigo-500 bg-indigo-50 dark:bg-indigo-900/20"
                      : "border-gray-200 dark:border-gray-700 hover:border-gray-300 dark:hover:border-gray-600"
                  }`}
                  onClick={() => onSelect(e)}
                >
                  <div className="flex items-center justify-between">
                    <div className="flex-1">
                      <div className="flex items-center gap-3 mb-2">
                        <span className="font-semibold text-gray-900 dark:text-white">
                          Hồ sơ #{e.id || e.Id}
                        </span>
                        <span className="text-xs px-2 py-1 rounded bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300">
                          {e.status || "Draft"}
                        </span>
                        <span className="text-sm text-gray-500 dark:text-gray-400">
                          Lịch hẹn: #{e.appointmentId || e.AppointmentId}
                        </span>
                      </div>
                      {e.subjective || e.Subjective ? (
                        <p className="text-sm text-gray-600 dark:text-gray-300 line-clamp-2">
                          <strong>Triệu chứng:</strong> {e.subjective || e.Subjective}
                        </p>
                      ) : null}
                      {e.assessment || e.Assessment ? (
                        <p className="text-sm text-gray-600 dark:text-gray-300 line-clamp-2 mt-1">
                          <strong>Chẩn đoán:</strong> {e.assessment || e.Assessment}
                        </p>
                      ) : null}
                    </div>
                    <button
                      onClick={(event) => {
                        event.stopPropagation();
                        navigate(`/doctor/appointments/${e.appointmentId || e.AppointmentId}`);
                      }}
                      className="ml-4 px-3 py-1.5 text-xs bg-gray-100 dark:bg-gray-700 text-gray-700 dark:text-gray-300 rounded hover:bg-gray-200 dark:hover:bg-gray-600 flex items-center gap-1"
                    >
                      <Eye className="h-3 w-3" />
                      Xem lịch hẹn
                    </button>
                  </div>
                </div>
              );
            })}
              </div>
        )}
      </div>

      {selected && (
        <div className="space-y-6">
          {/* Selected Encounter Details */}
          <div className="bg-indigo-50 dark:bg-indigo-900/20 border border-indigo-200 dark:border-indigo-800 rounded-lg p-6">
            <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center gap-2">
              <FileText className="h-5 w-5" />
              Chi tiết hồ sơ khám #{selected.id || selected.Id}
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">Subjective (Chủ quan)</label>
                <p className="text-sm text-gray-900 dark:text-white bg-white dark:bg-gray-800 p-3 rounded border border-gray-200 dark:border-gray-700 min-h-[60px]">
                  {selected.subjective || selected.Subjective || "—"}
                </p>
              </div>
              <div>
                <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">Objective (Khách quan)</label>
                <p className="text-sm text-gray-900 dark:text-white bg-white dark:bg-gray-800 p-3 rounded border border-gray-200 dark:border-gray-700 min-h-[60px]">
                  {selected.objective || selected.Objective || "—"}
                </p>
              </div>
              <div>
                <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">Assessment (Đánh giá)</label>
                <p className="text-sm text-gray-900 dark:text-white bg-white dark:bg-gray-800 p-3 rounded border border-gray-200 dark:border-gray-700 min-h-[60px]">
                  {selected.assessment || selected.Assessment || "—"}
                </p>
              </div>
              <div>
                <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">Plan (Kế hoạch)</label>
                <p className="text-sm text-gray-900 dark:text-white bg-white dark:bg-gray-800 p-3 rounded border border-gray-200 dark:border-gray-700 min-h-[60px]">
                  {selected.plan || selected.Plan || "—"}
                </p>
              </div>
            </div>
            <div className="mt-4 flex gap-2">
              <button
                onClick={() => navigate(`/doctor/appointments/${selected.appointmentId || selected.AppointmentId}`)}
                className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 flex items-center gap-2 text-sm"
              >
                <Calendar className="h-4 w-4" />
                Xem chi tiết lịch hẹn
              </button>
            </div>
          </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Prescriptions Section */}
            <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
              <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center gap-2">
                <Pill className="h-5 w-5" />
                Đơn thuốc
              </h3>
              <form onSubmit={addRx} className="bg-gray-50 dark:bg-gray-700/50 p-4 rounded-lg mb-4 space-y-3">
                <div className="grid grid-cols-2 gap-2">
                  <input
                    required
                    className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                    placeholder="Tên thuốc *"
                    value={rx.drugName}
                    onChange={(e) => setRx({ ...rx, drugName: e.target.value })}
                  />
                  <input
                    className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                    placeholder="Mã thuốc"
                    value={rx.drugCode}
                    onChange={(e) => setRx({ ...rx, drugCode: e.target.value })}
                  />
                </div>
                <div className="grid grid-cols-3 gap-2">
                  <input
                    required
                    className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                    placeholder="Liều dùng *"
                    value={rx.dosage}
                    onChange={(e) => setRx({ ...rx, dosage: e.target.value })}
                  />
                  <input
                    required
                    className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                    placeholder="Tần suất *"
                    value={rx.frequency}
                    onChange={(e) => setRx({ ...rx, frequency: e.target.value })}
                  />
                  <input
                    required
                    className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                    placeholder="Thời gian *"
                    value={rx.duration}
                    onChange={(e) => setRx({ ...rx, duration: e.target.value })}
                  />
                </div>
                <input
                  className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                  placeholder="Hướng dẫn sử dụng"
                  value={rx.instructions}
                  onChange={(e) => setRx({ ...rx, instructions: e.target.value })}
                />
                <button
                  type="submit"
                  className="w-full px-4 py-2 bg-green-600 text-white rounded-md hover:bg-green-700 flex items-center justify-center gap-2"
                >
                  <Plus className="h-4 w-4" />
                  Thêm thuốc
                </button>
            </form>
              {rxList.length === 0 ? (
                <p className="text-sm text-gray-500 dark:text-gray-400 text-center py-4">Chưa có đơn thuốc nào</p>
              ) : (
                <div className="space-y-3">
                  {rxList.map((p) => (
                    <div key={p.id || p.Id} className="border border-gray-200 dark:border-gray-700 rounded-lg p-4">
                      <div className="flex items-center justify-between mb-2">
                        <span className="font-semibold text-gray-900 dark:text-white">Đơn #{p.id || p.Id}</span>
                        <span className="text-xs text-gray-500 dark:text-gray-400">
                          {p.createdAt ? new Date(p.createdAt).toLocaleDateString("vi-VN") : ""}
                        </span>
                      </div>
                      {p.notes && (
                        <p className="text-sm text-gray-600 dark:text-gray-300 mb-2">{p.notes}</p>
                      )}
                      {p.lines && p.lines.length > 0 && (
                        <div className="space-y-2">
                          {p.lines.map((line, idx) => (
                            <div key={idx} className="text-sm bg-gray-50 dark:bg-gray-700/50 p-2 rounded">
                              <div className="font-medium text-gray-900 dark:text-white">
                                {line.drugName || line.DrugName}
                                {line.drugCode || line.DrugCode ? (
                                  <span className="text-xs text-gray-500 dark:text-gray-400 ml-2">
                                    (Mã: {line.drugCode || line.DrugCode})
                                  </span>
                                ) : null}
                              </div>
                              <div className="text-xs text-gray-600 dark:text-gray-400 mt-1">
                                {line.dosage || line.Dosage} · {line.frequency || line.Frequency} · {line.duration || line.Duration}
                                {line.instructions || line.Instructions ? ` · ${line.instructions || line.Instructions}` : ""}
                              </div>
                            </div>
                          ))}
                        </div>
                      )}
                    </div>
                  ))}
                </div>
              )}
          </div>

            {/* Orders Section */}
            <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
              <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-4 flex items-center gap-2">
                <ClipboardList className="h-5 w-5" />
                Chỉ định
              </h3>
              <form onSubmit={addOrder} className="bg-gray-50 dark:bg-gray-700/50 p-4 rounded-lg mb-4 space-y-3">
                <select
                  className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                  value={ord.orderType}
                  onChange={(e) => setOrd({ ...ord, orderType: e.target.value })}
                >
                  <option value="Test">Xét nghiệm</option>
                  <option value="Procedure">Thủ thuật</option>
              </select>
                <input
                  required
                  className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                  placeholder="Tên chỉ định *"
                  value={ord.name}
                  onChange={(e) => setOrd({ ...ord, name: e.target.value })}
                />
                <textarea
                  rows={2}
                  className="w-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md px-3 py-2 text-sm"
                  placeholder="Ghi chú"
                  value={ord.notes}
                  onChange={(e) => setOrd({ ...ord, notes: e.target.value })}
                />
                <button
                  type="submit"
                  className="w-full px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700 flex items-center justify-center gap-2"
                >
                  <Plus className="h-4 w-4" />
                  Thêm chỉ định
                </button>
            </form>
              {orderList.length === 0 ? (
                <p className="text-sm text-gray-500 dark:text-gray-400 text-center py-4">Chưa có chỉ định nào</p>
              ) : (
                <div className="space-y-3">
                  {orderList.map((o) => (
                    <div key={o.id || o.Id} className="border border-gray-200 dark:border-gray-700 rounded-lg p-4">
                      <div className="flex items-center justify-between mb-2">
                        <span className="font-semibold text-gray-900 dark:text-white">
                          {o.orderType || o.OrderType} · {o.name || o.Name}
                        </span>
                        <span className="text-xs px-2 py-1 rounded bg-blue-100 text-blue-800 dark:bg-blue-900/30 dark:text-blue-300">
                          {o.status || o.Status || "Pending"}
                        </span>
                      </div>
                      {o.notes || o.Notes ? (
                        <p className="text-sm text-gray-600 dark:text-gray-300">{o.notes || o.Notes}</p>
                      ) : null}
                      {o.resultUrl || o.ResultUrl ? (
                        <a
                          href={o.resultUrl || o.ResultUrl}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-xs text-blue-600 dark:text-blue-400 hover:underline mt-1 inline-block"
                        >
                          Xem kết quả
                        </a>
                      ) : null}
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default EncounterPage;


