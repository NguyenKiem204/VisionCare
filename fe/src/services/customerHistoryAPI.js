import api from "../utils/api";

const unwrap = (response) => response.data?.data || response.data;

export const customerHistoryAPI = {
  getBookings: async (params = {}) => {
    const response = await api.get("/customer/me/bookings", {
      params,
    });
    return unwrap(response);
  },

  getPrescriptions: async (encounterId = null) => {
    const response = await api.get("/customer/me/prescriptions", {
      params: encounterId ? { encounterId } : undefined,
    });
    return unwrap(response);
  },

  getMedicalHistory: async () => {
    const response = await api.get("/customer/me/medical-history");
    return unwrap(response);
  },
};

