import api from "../utils/api";

// Reschedule functions for customers
export const requestReschedule = async (appointmentId, proposedDateTime, reason) => {
  const res = await api.post(`/customer/me/appointments/${appointmentId}/request-reschedule`, {
    proposedDateTime,
    reason,
  });
  return res?.data?.data ?? res?.data;
};

export const approveReschedule = async (appointmentId) => {
  const res = await api.put(`/customer/me/appointments/${appointmentId}/approve-reschedule`);
  return res?.data?.data ?? res?.data;
};

export const rejectReschedule = async (appointmentId, reason) => {
  const res = await api.put(`/customer/me/appointments/${appointmentId}/reject-reschedule`, {
    reason: reason ?? null,
  });
  return res?.data?.data ?? res?.data;
};

export const counterReschedule = async (appointmentId, proposedDateTime, reason) => {
  const res = await api.post(`/customer/me/appointments/${appointmentId}/counter-reschedule`, {
    proposedDateTime,
    reason,
  });
  return res?.data?.data ?? res?.data;
};

