import api from "../utils/api";

export const bookingAPI = {
  getAvailableSlots: async (doctorId, date, serviceTypeId = null) => {
    const params = new URLSearchParams({
      doctorId: doctorId.toString(),
      date: date,
    });
    if (serviceTypeId) {
      params.append("serviceTypeId", serviceTypeId.toString());
    }

    const response = await api.get(`/booking/available-slots?${params}`);
    return response.data;
  },

  holdSlot: async (request) => {
    const response = await api.post("/booking/hold-slot", request);
    return response.data;
  },

  releaseHold: async (request) => {
    const response = await api.post("/booking/release-hold", request);
    return response.data;
  },

  createBooking: async (request) => {
    const response = await api.post("/booking/create", request);
    return response.data;
  },

  getBookingById: async (appointmentId) => {
    const response = await api.get(`/booking/${appointmentId}`);
    return response.data;
  },

  searchBooking: async (appointmentCode = null, phone = null) => {
    const params = new URLSearchParams();
    if (appointmentCode) params.append("appointmentCode", appointmentCode);
    if (phone) params.append("phone", phone);

    const response = await api.get(`/booking/search?${params}`);
    return response.data;
  },

  cancelBooking: async (appointmentId, request) => {
    const response = await api.put(`/booking/${appointmentId}/cancel`, request);
    return response.data;
  },

  initiatePayment: async (appointmentId) => {
    const response = await api.post(`/booking/${appointmentId}/payment/initiate`);
    return response.data.paymentUrl;
  },
};







