import api from "../utils/api";

// Get all doctor absences
export const getAllDoctorAbsences = async () => {
  const response = await api.get("/doctor-absences");
  return response;
};

// Get pending absences
export const getPendingAbsences = async () => {
  const response = await api.get("/doctor-absences/pending");
  return response;
};

// Get absences by doctor
export const getAbsencesByDoctor = async (doctorId) => {
  const response = await api.get(`/doctor-absences/doctor/${doctorId}`);
  return response;
};

// Get absence by ID
export const getAbsenceById = async (id) => {
  const response = await api.get(`/doctor-absences/${id}`);
  return response;
};

// Create doctor absence
export const createDoctorAbsence = async (data) => {
  const response = await api.post("/doctor-absences", data);
  return response;
};

// Update doctor absence
export const updateDoctorAbsence = async (id, data) => {
  const response = await api.put(`/doctor-absences/${id}`, data);
  return response;
};

// Approve absence
export const approveAbsence = async (id) => {
  const response = await api.post(`/doctor-absences/${id}/approve`);
  return response;
};

// Reject absence
export const rejectAbsence = async (id) => {
  const response = await api.post(`/doctor-absences/${id}/reject`);
  return response;
};

// Handle appointments for absence
export const handleAbsenceAppointments = async (id, data) => {
  const response = await api.post(`/doctor-absences/${id}/handle-appointments`, data);
  return response;
};

// Delete absence
export const deleteDoctorAbsence = async (id) => {
  const response = await api.delete(`/doctor-absences/${id}`);
  return response;
};

