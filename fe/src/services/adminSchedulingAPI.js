import api from "../utils/api";

export const searchSchedules = async (payload) => {
  // payload: { doctorId?, fromDate?, toDate?, status?, page?, pageSize? }
  return await api.post("/Scheduling/schedules/search", payload);
};


