import api from "../utils/api";

export const fetchFollowUps = async () => {
  const response = await api.get("/followup");
  return response;
};

export const getFollowUpById = async (id) => {
  const response = await api.get(`/followup/${id}`);
  return response;
};

export const getFollowUpByAppointment = async (appointmentId) => {
  const response = await api.get(`/followup/appointment/${appointmentId}`);
  return response;
};

export const getFollowUpsByPatient = async (patientId) => {
  const response = await api.get(`/followup/patient/${patientId}`);
  return response;
};

export const getFollowUpsByDoctor = async (doctorId) => {
  const response = await api.get(`/followup/doctor/${doctorId}`);
  return response;
};

export const searchFollowUps = async (params = {}) => {
  const response = await api.get("/followup/search", { params });
  return response;
};

export const createFollowUp = async (followUpData) => {
  const response = await api.post("/followup", followUpData);
  return response;
};

export const updateFollowUp = async (id, followUpData) => {
  const response = await api.put(`/followup/${id}`, followUpData);
  return response;
};

export const completeFollowUp = async (id) => {
  const response = await api.put(`/followup/${id}/complete`);
  return response;
};

export const cancelFollowUp = async (id, reason = null) => {
  const response = await api.put(`/followup/${id}/cancel`, reason);
  return response;
};

export const rescheduleFollowUp = async (id, newDate) => {
  const response = await api.put(`/followup/${id}/reschedule`, newDate);
  return response;
};

export const deleteFollowUp = async (id) => {
  const response = await api.delete(`/followup/${id}`);
  return response;
};
