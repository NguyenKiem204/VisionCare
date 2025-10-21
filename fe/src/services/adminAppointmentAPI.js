import api from "../utils/api";

// Get all appointments with pagination and sorting
export const fetchAppointments = async (params = {}) => {
  const { page = 0, size = 10, sortBy = "id", sortDir = "asc" } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: size.toString(),
    sortBy,
    desc: sortDir === "desc",
  });

  const response = await api.get(`/appointments?${queryParams}`);
  return response;
};

// Search appointments with filters
export const searchAppointments = async (params = {}) => {
  const {
    keyword = "",
    status = null,
    doctorId = null,
    customerId = null,
    startDate = null,
    endDate = null,
    page = 0,
    pageSize = 10,
    sortBy = "id",
    desc = false,
  } = params;

  const queryParams = new URLSearchParams({
    keyword,
    page: page.toString(),
    pageSize: pageSize.toString(),
    sortBy,
    desc: desc.toString(),
  });

  if (status) queryParams.append("status", status);
  if (doctorId) queryParams.append("doctorId", doctorId);
  if (customerId) queryParams.append("customerId", customerId);
  if (startDate) queryParams.append("startDate", startDate);
  if (endDate) queryParams.append("endDate", endDate);

  const response = await api.get(`/appointments/search?${queryParams}`);
  return response;
};

// Get appointment by ID
export const getAppointmentById = async (id) => {
  const response = await api.get(`/appointments/${id}`);
  return response;
};

// Create new appointment
export const createAppointment = async (appointmentData) => {
  const response = await api.post("/appointments", appointmentData);
  return response;
};

// Update appointment
export const updateAppointment = async (id, appointmentData) => {
  const response = await api.put(`/appointments/${id}`, appointmentData);
  return response;
};

// Delete appointment
export const deleteAppointment = async (id) => {
  const response = await api.delete(`/appointments/${id}`);
  return response;
};

// Get appointments by doctor
export const getAppointmentsByDoctor = async (doctorId, date = null) => {
  const queryParams = new URLSearchParams({ doctorId: doctorId.toString() });
  if (date) queryParams.append("date", date);

  const response = await api.get(
    `/appointments/doctor/${doctorId}?${queryParams}`
  );
  return response;
};

// Get appointments by customer
export const getAppointmentsByCustomer = async (customerId, date = null) => {
  const queryParams = new URLSearchParams({
    customerId: customerId.toString(),
  });
  if (date) queryParams.append("date", date);

  const response = await api.get(
    `/appointments/customer/${customerId}?${queryParams}`
  );
  return response;
};

// Get appointments by date
export const getAppointmentsByDate = async (date) => {
  const response = await api.get(`/appointments/date/${date}`);
  return response;
};

// Get appointments by date range
export const getAppointmentsByDateRange = async (startDate, endDate) => {
  const response = await api.get(
    `/appointments/date-range?startDate=${startDate}&endDate=${endDate}`
  );
  return response;
};

// Get upcoming appointments
export const getUpcomingAppointments = async (
  doctorId = null,
  customerId = null
) => {
  const queryParams = new URLSearchParams();
  if (doctorId) queryParams.append("doctorId", doctorId);
  if (customerId) queryParams.append("customerId", customerId);

  const response = await api.get(`/appointments/upcoming?${queryParams}`);
  return response;
};

// Confirm appointment
export const confirmAppointment = async (id) => {
  const response = await api.put(`/appointments/${id}/confirm`);
  return response;
};

// Cancel appointment
export const cancelAppointment = async (id, reason = null) => {
  const response = await api.put(`/appointments/${id}/cancel`, reason);
  return response;
};

// Complete appointment
export const completeAppointment = async (id, notes = null) => {
  const response = await api.put(`/appointments/${id}/complete`, notes);
  return response;
};

// Reschedule appointment
export const rescheduleAppointment = async (id, newDateTime) => {
  const response = await api.put(`/appointments/${id}/reschedule`, newDateTime);
  return response;
};

// Check if doctor is available
export const checkDoctorAvailability = async (doctorId, dateTime) => {
  const response = await api.get(
    `/appointments/availability?doctorId=${doctorId}&dateTime=${dateTime}`
  );
  return response;
};

// Get available time slots for a doctor on a specific date
export const getAvailableTimeSlots = async (doctorId, date) => {
  const response = await api.get(
    `/appointments/available-slots?doctorId=${doctorId}&date=${date}`
  );
  return response;
};

// Get overdue appointments
export const getOverdueAppointments = async () => {
  const response = await api.get("/appointments/overdue");
  return response;
};

// Get appointment statistics
export const getAppointmentStatistics = async () => {
  const response = await api.get("/appointments/statistics");
  return response;
};
