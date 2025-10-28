import api from "../utils/api";

export const fetchWeeklySchedules = async () => {
  const response = await api.get("/scheduling/weekly");
  return response;
};

export const getWeeklyScheduleById = async (id) => {
  const response = await api.get(`/scheduling/weekly/${id}`);
  return response;
};

export const searchWeeklySchedules = async (params = {}) => {
  const response = await api.get("/scheduling/weekly/search", { params });
  return response;
};

export const createWeeklySchedule = async (scheduleData) => {
  const response = await api.post("/scheduling/weekly", scheduleData);
  return response;
};

export const updateWeeklySchedule = async (id, scheduleData) => {
  const response = await api.put(`/scheduling/weekly/${id}`, scheduleData);
  return response;
};

export const deleteWeeklySchedule = async (id) => {
  const response = await api.delete(`/scheduling/weekly/${id}`);
  return response;
};

export const publishWeeklySchedule = async (id) => {
  const response = await api.post(`/scheduling/weekly/${id}/publish`);
  return response;
};

export const archiveWeeklySchedule = async (id) => {
  const response = await api.post(`/scheduling/weekly/${id}/archive`);
  return response;
};

export const updateWeeklyScheduleStatus = async (id, status) => {
  const response = await api.put(`/scheduling/weekly/${id}/status`, { status });
  return response;
};
