import api from "../utils/api";

// Doctor Feedback APIs
export const fetchDoctorFeedbacks = async () => {
  const response = await api.get("/feedback/doctors");
  return response;
};

export const getDoctorFeedbackById = async (id) => {
  const response = await api.get(`/feedback/doctors/${id}`);
  return response;
};

export const searchDoctorFeedbacks = async (params = {}) => {
  const response = await api.get("/feedback/doctors/search", { params });
  return response;
};

export const createDoctorFeedback = async (feedbackData) => {
  const response = await api.post("/feedback/doctors", feedbackData);
  return response;
};

export const updateDoctorFeedback = async (id, feedbackData) => {
  const response = await api.put(`/feedback/doctors/${id}`, feedbackData);
  return response;
};

export const respondToDoctorFeedback = async (id, responseData) => {
  const response = await api.post(`/feedback/doctors/${id}/respond`, responseData);
  return response;
};

export const deleteDoctorFeedback = async (id) => {
  const response = await api.delete(`/feedback/doctors/${id}`);
  return response;
};

// Service Feedback APIs
export const fetchServiceFeedbacks = async () => {
  const response = await api.get("/feedback/services");
  return response;
};

export const getServiceFeedbackById = async (id) => {
  const response = await api.get(`/feedback/services/${id}`);
  return response;
};

export const searchServiceFeedbacks = async (params = {}) => {
  const response = await api.get("/feedback/services/search", { params });
  return response;
};

export const createServiceFeedback = async (feedbackData) => {
  const response = await api.post("/feedback/services", feedbackData);
  return response;
};

export const updateServiceFeedback = async (id, feedbackData) => {
  const response = await api.put(`/feedback/services/${id}`, feedbackData);
  return response;
};

export const respondToServiceFeedback = async (id, responseData) => {
  const response = await api.post(`/feedback/services/${id}/respond`, responseData);
  return response;
};

export const deleteServiceFeedback = async (id) => {
  const response = await api.delete(`/feedback/services/${id}`);
  return response;
};
