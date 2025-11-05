import React, { useEffect, useState } from "react";
import api from "../../utils/api";

const DoctorPatients = () => {
  const [patients, setPatients] = useState([]);
  const [loading, setLoading] = useState(false);
  const load = async () => {
    setLoading(true);
    try {
      const res = await api.get("/doctor/me/patients");
      setPatients(res?.data || res);
    } catch (e) {
      console.error("Load patients error", e);
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => { load(); }, []);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Bệnh nhân</h1>
        <p className="text-gray-600 dark:text-gray-300">Quản lý bệnh nhân của bác sĩ</p>
      </div>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">Danh sách bệnh nhân</h3>
        {loading && <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>}
        {!loading && (
          <ul className="divide-y divide-gray-200 dark:divide-gray-700">
            {(patients?.data || patients || []).map((p) => (
              <li key={p.id} className="py-3">
                <div className="text-sm font-medium text-gray-900 dark:text-white">{p.customerName || p.fullName || `Bệnh nhân #${p.id}`}</div>
              </li>
            ))}
            {(!patients || (patients.data||patients).length === 0) && (
              <li className="py-3 text-sm text-gray-500 dark:text-gray-300">Chưa có bệnh nhân</li>
            )}
          </ul>
        )}
      </div>
    </div>
  );
};

export default DoctorPatients;
