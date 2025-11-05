import api from "../utils/api";

export const fetchDoctorSchedules = async () => {
  const response = await api.get("/doctorschedules");
  return response;
};

export const getDoctorScheduleById = async (id) => {
  const response = await api.get(`/doctorschedules/${id}`);
  return response;
};

export const getDoctorSchedulesByDoctorId = async (doctorId) => {
  const response = await api.get(`/doctorschedules/doctor/${doctorId}`);
  return response;
};

export const getActiveDoctorSchedulesByDoctorId = async (doctorId) => {
  const response = await api.get(`/doctorschedules/doctor/${doctorId}/active`);
  return response;
};

export const createDoctorSchedule = async (scheduleData) => {
  const response = await api.post("/doctorschedules", scheduleData);
  return response;
};

export const updateDoctorSchedule = async (id, scheduleData) => {
  const response = await api.put(`/doctorschedules/${id}`, scheduleData);
  return response;
};

export const deleteDoctorSchedule = async (id) => {
  const response = await api.delete(`/doctorschedules/${id}`);
  return response;
};

