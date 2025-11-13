import api from "../utils/api";

export const getAppointmentById = async (id) => {
  const res = await api.get(`/appointments/${id}`);
  return res?.data ?? res;
};

export const getMedicalHistoryByAppointment = async (appointmentId) => {
  const res = await api.get(`/medical-records/appointment/${appointmentId}`);
  return res?.data?.data ?? res?.data ?? null;
};

export const createMedicalHistory = async (data) => {
  const res = await api.post("/medical-records", data);
  return res?.data?.data ?? res?.data ?? null;
};

export const updateMedicalHistory = async (id, data) => {
  const res = await api.put(`/medical-records/${id}`, data);
  return res?.data?.data ?? res?.data ?? null;
};

