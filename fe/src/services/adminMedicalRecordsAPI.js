import api from "../utils/api";

export const fetchMedicalRecords = async () => {
  const response = await api.get("/medicalhistory");
  return response;
};

export const getMedicalRecordById = async (id) => {
  const response = await api.get(`/medicalhistory/${id}`);
  return response;
};

export const getMedicalRecordByAppointment = async (appointmentId) => {
  const response = await api.get(`/medicalhistory/appointment/${appointmentId}`);
  return response;
};

export const getMedicalRecordsByPatient = async (patientId) => {
  const response = await api.get(`/medicalhistory/patient/${patientId}`);
  return response;
};

export const searchMedicalRecords = async (params = {}) => {
  const response = await api.get("/medicalhistory/search", { params });
  return response;
};

export const createMedicalRecord = async (medicalRecordData) => {
  const response = await api.post("/medicalhistory", medicalRecordData);
  return response;
};

export const updateMedicalRecord = async (id, medicalRecordData) => {
  const response = await api.put(`/medicalhistory/${id}`, medicalRecordData);
  return response;
};

export const deleteMedicalRecord = async (id) => {
  const response = await api.delete(`/medicalhistory/${id}`);
  return response;
};
