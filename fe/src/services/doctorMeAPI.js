import api from "../utils/api";

export const getWeeklySchedules = async () => {
  const res = await api.get("/doctor/me/weekly-schedules");
  return res?.data?.data ?? res?.data ?? [];
};

export const getMyDoctorSchedules = async () => {
  const res = await api.get("/doctor/me/doctor-schedules");
  return res?.data?.data ?? res?.data ?? [];
};

export const getSchedules = async (params = {}) => {
  const res = await api.get("/doctor/me/schedules", { params });
  return res?.data?.data ?? res?.data ?? [];
};

export const getUpcomingAppointments = async (limit = 10) => {
  const res = await api.get("/doctor/me/upcoming-appointments", { params: { limit } });
  return res?.data?.data ?? res?.data ?? [];
};

export const getMyAbsences = async () => {
  const res = await api.get("/doctor/me/absences");
  return res?.data?.data ?? res?.data ?? [];
};

export const createMyAbsence = async (data) => {
  const res = await api.post("/doctor/me/absences", data);
  return res?.data?.data ?? res?.data ?? null;
};

export const confirmMyAppointment = async (id) => {
  const res = await api.put(`/doctor/me/appointments/${id}/confirm`);
  return res?.data?.data ?? res?.data ?? true;
};

export const completeMyAppointment = async (id, notes) => {
  const res = await api.put(`/doctor/me/appointments/${id}/complete`, notes ?? "");
  return res?.data?.data ?? res?.data ?? true;
};

export const cancelMyAppointment = async (id, reason) => {
  const res = await api.put(`/doctor/me/appointments/${id}/cancel`, reason ?? "");
  return res?.data?.data ?? res?.data ?? true;
};


