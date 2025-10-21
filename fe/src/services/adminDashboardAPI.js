import api from "../utils/api";

export const getAdminSummary = async ({ from, to }) => {
  const res = await api.get(`/dashboard/admin/summary`, {
    params: { from, to },
  });
  return res?.data?.data ?? res?.data ?? {};
};

export const getAdminAppointmentSeries = async ({
  from,
  to,
  bucket = "day",
}) => {
  const res = await api.get(`/dashboard/admin/appointments/series`, {
    params: { from, to, bucket },
  });
  return res?.data?.data ?? [];
};

export const getTopServices = async ({ from, to, top = 5 }) => {
  const res = await api.get(`/dashboard/admin/services/top`, {
    params: { from, to, top },
  });
  return res?.data?.data ?? [];
};

export const getDoctorKpis = async ({ from, to }) => {
  const res = await api.get(`/dashboard/admin/doctors/kpis`, {
    params: { from, to },
  });
  return res?.data?.data ?? [];
};

export const getRecentAppointments = async ({ limit = 10 } = {}) => {
  const res = await api.get(`/dashboard/admin/appointments/recent`, {
    params: { limit },
  });
  return res?.data?.data ?? [];
};
