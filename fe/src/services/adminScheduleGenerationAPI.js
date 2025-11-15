import api from "../utils/api";

export const generateSchedulesForDoctor = async (doctorId, daysAhead = 14) => {
  const response = await api.post(
    `/ScheduleGeneration/doctor/${doctorId}?daysAhead=${daysAhead}`
  );
  return response;
};

export const generateSchedulesForAll = async (daysAhead = 14) => {
  const response = await api.post(`/ScheduleGeneration/all?daysAhead=${daysAhead}`);
  return response;
};

export const generateSchedulesForDateRange = async (doctorId, startDate, endDate) => {
  const response = await api.post(
    `/ScheduleGeneration/doctor/${doctorId}/range?startDate=${startDate}&endDate=${endDate}`
  );
  return response;
};

