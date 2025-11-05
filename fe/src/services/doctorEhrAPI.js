import api from "../utils/api";

export const listEncounters = async (params = {}) => {
  const res = await api.get("/doctor/me/ehr/encounters", { params });
  return res?.data?.data ?? res?.data ?? [];
};

export const createEncounter = async (data, customerId) => {
  const res = await api.post(`/doctor/me/ehr/encounters?customerId=${customerId}`, data);
  return res?.data?.data ?? res?.data ?? null;
};

export const updateEncounter = async (id, data) => {
  const res = await api.put(`/doctor/me/ehr/encounters/${id}`, data);
  return res?.data?.data ?? res?.data ?? null;
};

export const createPrescription = async (data) => {
  const res = await api.post("/doctor/me/ehr/prescriptions", data);
  return res?.data?.data ?? res?.data ?? null;
};

export const getPrescriptionsByEncounter = async (encounterId) => {
  const res = await api.get(`/doctor/me/ehr/prescriptions/by-encounter/${encounterId}`);
  return res?.data?.data ?? res?.data ?? [];
};

export const createOrder = async (data) => {
  const res = await api.post("/doctor/me/ehr/orders", data);
  return res?.data?.data ?? res?.data ?? null;
};

export const getOrdersByEncounter = async (encounterId) => {
  const res = await api.get(`/doctor/me/ehr/orders/by-encounter/${encounterId}`);
  return res?.data?.data ?? res?.data ?? [];
};

export const updateOrder = async (orderId, data) => {
  const res = await api.put(`/doctor/me/ehr/orders/${orderId}`, data);
  return res?.data?.data ?? res?.data ?? null;
};


