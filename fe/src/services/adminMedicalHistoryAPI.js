import api from "../utils/api";

export const fetchMedicalHistories = async () => {
  const response = await api.get("/medicalhistory");
  return response;
};

export const getMedicalHistoryById = async (id) => {
  const response = await api.get(`/medicalhistory/${id}`);
  return response;
};

export const getMedicalHistoryByAppointment = async (appointmentId) => {
  const response = await api.get(`/medicalhistory/appointment/${appointmentId}`);
  return response;
};

export const getMedicalHistoriesByPatient = async (patientId) => {
  const response = await api.get(`/medicalhistory/patient/${patientId}`);
  return response;
};

export const getMedicalHistoriesByDoctor = async (doctorId) => {
  const response = await api.get(`/medicalhistory/doctor/${doctorId}`);
  return response;
};

export const searchMedicalHistories = async (params = {}) => {
  const response = await api.get("/medicalhistory/search", { params });
  return response;
};

export const createMedicalHistory = async (medicalHistoryData) => {
  const response = await api.post("/medicalhistory", medicalHistoryData);
  return response;
};

export const updateMedicalHistory = async (id, medicalHistoryData) => {
  const response = await api.put(`/medicalhistory/${id}`, medicalHistoryData);
  return response;
};

export const deleteMedicalHistory = async (id) => {
  const response = await api.delete(`/medicalhistory/${id}`);
  return response;
};
