import api from "../utils/api";

export const getDoctorSummary = async ({ from, to }) => {
  const res = await api.get(`/dashboard/doctor/summary`, {
    params: { from, to },
  });
  return res?.data?.data ?? res?.data ?? {};
};

export const getDoctorAppointmentSeries = async ({
  from,
  to,
  bucket = "day",
}) => {
  const res = await api.get(`/dashboard/doctor/appointments/series`, {
    params: { from, to, bucket },
  });
  return res?.data?.data ?? [];
};

export const getUpcomingAppointments = async ({ from, to, limit = 10 }) => {
  const res = await api.get(`/dashboard/doctor/appointments/upcoming`, {
    params: { from, to, limit },
  });
  return res?.data?.data ?? [];
};
