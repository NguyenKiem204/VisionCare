import api from "../utils/api";

// Generate schedules for a doctor
export const generateSchedulesForDoctor = async (doctorId, daysAhead = 14) => {
  const response = await api.post(
    `/ScheduleGeneration/doctor/${doctorId}?daysAhead=${daysAhead}`
  );
  return response;
};

// Generate schedules for all doctors
export const generateSchedulesForAll = async (daysAhead = 14) => {
  const response = await api.post(`/ScheduleGeneration/all?daysAhead=${daysAhead}`);
  return response;
};

// Generate schedules for date range
export const generateSchedulesForDateRange = async (doctorId, startDate, endDate) => {
  const response = await api.post(
    `/ScheduleGeneration/doctor/${doctorId}/range?startDate=${startDate}&endDate=${endDate}`
  );
  return response;
};

