import React, { useEffect, useState } from "react";
import { listEncounters, createEncounter, updateEncounter, createPrescription, getPrescriptionsByEncounter, createOrder, getOrdersByEncounter } from "../../services/doctorEhrAPI";

const EncounterPage = () => {
  const [encounters, setEncounters] = useState([]);
  const [selected, setSelected] = useState(null);
  const [form, setForm] = useState({ appointmentId: "", subjective: "", objective: "", assessment: "", plan: "" });
  const [rx, setRx] = useState({ drugName: "", dosage: "", frequency: "", duration: "", instructions: "" });
  const [ord, setOrd] = useState({ orderType: "Test", name: "", notes: "" });
  const [rxList, setRxList] = useState([]);
  const [orderList, setOrderList] = useState([]);

  const load = async () => {
    const res = await listEncounters();
    setEncounters(Array.isArray(res) ? res : []);
  };

  useEffect(() => { load(); }, []);

  const onCreate = async (e) => {
    e.preventDefault();
    if (!form.appointmentId) return;
    // customerId should match appointment's patient id; ask user input minimal
    const customerId = window.prompt("Nhập CustomerId (account_id) của bệnh nhân theo appointment:") || "";
    await createEncounter({
      appointmentId: Number(form.appointmentId),
      subjective: form.subjective,
      objective: form.objective,
      assessment: form.assessment,
      plan: form.plan,
    }, Number(customerId));
    setForm({ appointmentId: "", subjective: "", objective: "", assessment: "", plan: "" });
    await load();
  };

  const onSelect = async (e) => {
    setSelected(e);
    const rx = await getPrescriptionsByEncounter(e.id);
    setRxList(Array.isArray(rx) ? rx : []);
    const orders = await getOrdersByEncounter(e.id);
    setOrderList(Array.isArray(orders) ? orders : []);
  };

  const addRx = async (e) => {
    e.preventDefault();
    if (!selected) return;
    await createPrescription({
      encounterId: selected.id,
      notes: null,
      lines: [{
        drugName: rx.drugName,
        dosage: rx.dosage,
        frequency: rx.frequency,
        duration: rx.duration,
        instructions: rx.instructions,
      }]
    });
    setRx({ drugName: "", dosage: "", frequency: "", duration: "", instructions: "" });
    await onSelect(selected);
  };

  const addOrder = async (e) => {
    e.preventDefault();
    if (!selected) return;
    await createOrder({
      encounterId: selected.id,
      orderType: ord.orderType,
      name: ord.name,
      notes: ord.notes,
    });
    setOrd({ orderType: "Test", name: "", notes: "" });
    await onSelect(selected);
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Hồ sơ khám (Encounter)</h1>
        <p className="text-gray-600 dark:text-gray-300">Tạo và cập nhật encounter, đơn thuốc, chỉ định</p>
      </div>

      <form onSubmit={onCreate} className="bg-white dark:bg-gray-800 shadow rounded-lg p-6 space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-5 gap-4">
          <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="AppointmentId" value={form.appointmentId} onChange={(e) => setForm({ ...form, appointmentId: e.target.value })} />
          <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Subjective" value={form.subjective} onChange={(e) => setForm({ ...form, subjective: e.target.value })} />
          <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Objective" value={form.objective} onChange={(e) => setForm({ ...form, objective: e.target.value })} />
          <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Assessment" value={form.assessment} onChange={(e) => setForm({ ...form, assessment: e.target.value })} />
          <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Plan" value={form.plan} onChange={(e) => setForm({ ...form, plan: e.target.value })} />
        </div>
        <button className="px-4 py-2 bg-indigo-600 text-white rounded-md">Tạo Encounter</button>
      </form>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">Danh sách Encounter</h3>
        <ul className="divide-y divide-gray-200 dark:divide-gray-700">
          {(encounters || []).map((e) => (
            <li key={e.id} className="py-3 flex items-center justify-between">
              <div>
                <div className="text-sm font-medium text-gray-900 dark:text-white">#{e.id} · Appt {e.appointmentId} · {e.status}</div>
                <div className="text-sm text-gray-500 dark:text-gray-300">{e.subjective || ""}</div>
              </div>
              <button onClick={() => onSelect(e)} className="px-2 py-1 text-xs bg-gray-100 dark:bg-gray-700 dark:text-gray-100 rounded">Chi tiết</button>
            </li>
          ))}
          {(!encounters || encounters.length === 0) && <li className="py-3 text-sm text-gray-500 dark:text-gray-300">Chưa có encounter</li>}
        </ul>
      </div>

      {selected && (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6 space-y-4">
            <h3 className="text-lg font-medium text-gray-900 dark:text-white">Đơn thuốc</h3>
            <form onSubmit={addRx} className="grid grid-cols-1 md:grid-cols-5 gap-2">
              <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Thuốc" value={rx.drugName} onChange={(e)=>setRx({ ...rx, drugName: e.target.value })} />
              <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Liều" value={rx.dosage} onChange={(e)=>setRx({ ...rx, dosage: e.target.value })} />
              <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Tần suất" value={rx.frequency} onChange={(e)=>setRx({ ...rx, frequency: e.target.value })} />
              <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Thời gian" value={rx.duration} onChange={(e)=>setRx({ ...rx, duration: e.target.value })} />
              <button className="px-4 py-2 bg-green-600 text-white rounded-md">Thêm</button>
            </form>
            <ul className="divide-y divide-gray-200 dark:divide-gray-700">
              {(rxList || []).map((p) => (
                <li key={p.id} className="py-2">
                  <div className="text-sm font-medium text-gray-900 dark:text-white">Đơn #{p.id}</div>
                  <div className="text-sm text-gray-500 dark:text-gray-300">{(p.lines||[]).map(l=>l.drugName).join(", ")}</div>
                </li>
              ))}
            </ul>
          </div>
          <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6 space-y-4">
            <h3 className="text-lg font-medium text-gray-900 dark:text-white">Chỉ định</h3>
            <form onSubmit={addOrder} className="grid grid-cols-1 md:grid-cols-4 gap-2">
              <select className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" value={ord.orderType} onChange={(e)=>setOrd({ ...ord, orderType: e.target.value })}>
                <option value="Test">Test</option>
                <option value="Procedure">Procedure</option>
              </select>
              <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Tên" value={ord.name} onChange={(e)=>setOrd({ ...ord, name: e.target.value })} />
              <input className="border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-gray-100 rounded-md p-2" placeholder="Ghi chú" value={ord.notes} onChange={(e)=>setOrd({ ...ord, notes: e.target.value })} />
              <button className="px-4 py-2 bg-indigo-600 text-white rounded-md">Chỉ định</button>
            </form>
            <ul className="divide-y divide-gray-200 dark:divide-gray-700">
              {(orderList || []).map((o) => (
                <li key={o.id} className="py-2">
                  <div className="text-sm font-medium text-gray-900 dark:text-white">{o.orderType} · {o.name}</div>
                  <div className="text-sm text-gray-500 dark:text-gray-300">{o.status} {o.resultUrl ? `· ${o.resultUrl}` : ""}</div>
                </li>
              ))}
            </ul>
          </div>
        </div>
      )}
    </div>
  );
};

export default EncounterPage;


