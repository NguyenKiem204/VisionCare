import api from "../utils/api";

// Get weekly schedules by doctor
export const getWeeklySchedulesByDoctor = async (doctorId) => {
  const response = await api.get(`/weekly-schedules/doctor/${doctorId}`);
  return response;
};

// Get active weekly schedules by doctor
export const getActiveWeeklySchedulesByDoctor = async (doctorId) => {
  const response = await api.get(`/weekly-schedules/doctor/${doctorId}/active`);
  return response;
};

// Get weekly schedule by ID
export const getWeeklyScheduleById = async (id) => {
  const response = await api.get(`/weekly-schedules/${id}`);
  return response;
};

// Create weekly schedule
export const createWeeklySchedule = async (data) => {
  const response = await api.post("/weekly-schedules", data);
  return response;
};

// Update weekly schedule
export const updateWeeklySchedule = async (id, data) => {
  const response = await api.put(`/weekly-schedules/${id}`, data);
  return response;
};

// Delete weekly schedule
export const deleteWeeklySchedule = async (id) => {
  const response = await api.delete(`/weekly-schedules/${id}`);
  return response;
};

// Search weekly schedules
// Note: Backend may need to implement a search endpoint
// For now, this returns empty results or uses available endpoints
export const searchWeeklySchedules = async (params = {}) => {
  try {
    // If doctorId is provided, use the doctor endpoint
    if (params.doctorId) {
      const response = await api.get(`/weekly-schedules/doctor/${params.doctorId}`);
      // Transform to match expected format
      const items = response.data?.data || [];
      const page = params.page || 0;
      const size = params.size || 10;
      const start = page * size;
      const end = start + size;
      const paginatedItems = items.slice(start, end);
      
      return {
        data: {
          content: paginatedItems,
          totalElements: items.length,
          totalPages: Math.ceil(items.length / size),
          size: size,
          number: page,
        },
      };
    }
    
    // Otherwise, return empty results
    return {
      data: {
        content: [],
        totalElements: 0,
        totalPages: 0,
        size: params.size || 10,
        number: params.page || 0,
      },
    };
  } catch (error) {
    console.error("Error searching weekly schedules:", error);
    return {
      data: {
        content: [],
        totalElements: 0,
        totalPages: 0,
        size: params.size || 10,
        number: params.page || 0,
      },
    };
  }
};

// Publish weekly schedule (activate it)
export const publishWeeklySchedule = async (id) => {
  const response = await api.put(`/weekly-schedules/${id}`, {
    isActive: true,
  });
  return response;
};

// Archive weekly schedule (deactivate it)
export const archiveWeeklySchedule = async (id) => {
  const response = await api.put(`/weekly-schedules/${id}`, {
    isActive: false,
  });
  return response;
};

// Update weekly schedule status
export const updateWeeklyScheduleStatus = async (id, status) => {
  const response = await api.put(`/weekly-schedules/${id}`, {
    isActive: status === "active" || status === true,
  });
  return response;
};
