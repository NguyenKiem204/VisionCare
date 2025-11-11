import api from "../utils/api";

export const fetchWorkShifts = async () => {
  const response = await api.get("/workshifts");
  return response;
};

export const getActiveWorkShifts = async () => {
  const response = await api.get("/workshifts/active");
  return response;
};

export const getWorkShiftById = async (id) => {
  const response = await api.get(`/workshifts/${id}`);
  return response;
};

export const searchWorkShifts = async (params = {}) => {
  const response = await api.get("/workshifts/search", { params });
  return response;
};

export const createWorkShift = async (workShiftData) => {
  const response = await api.post("/workshifts", workShiftData);
  return response;
};

export const updateWorkShift = async (id, workShiftData) => {
  const response = await api.put(`/workshifts/${id}`, workShiftData);
  return response;
};

export const deleteWorkShift = async (id) => {
  const response = await api.delete(`/workshifts/${id}`);
  return response;
};
