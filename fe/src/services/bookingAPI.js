import api from "../utils/api";

/**
 * Booking API Service
 * Handles all booking-related API calls
 */
export const bookingAPI = {
  /**
   * Get available slots for a doctor on a specific date
   * @param {number} doctorId - Doctor ID
   * @param {string} date - Date in YYYY-MM-DD format
   * @param {number} [serviceTypeId] - Optional service type ID
   * @returns {Promise<Array<AvailableSlot>>}
   */
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

  /**
   * Hold a slot temporarily (10 minutes)
   * @param {Object} request - HoldSlotRequest
   * @param {number} request.doctorId - Doctor ID
   * @param {number} request.slotId - Slot ID
   * @param {string} request.scheduleDate - Date in YYYY-MM-DD format
   * @param {number} [request.customerId] - Optional customer ID if logged in
   * @returns {Promise<HoldSlotResponse>}
   */
  holdSlot: async (request) => {
    const response = await api.post("/booking/hold-slot", request);
    return response.data;
  },

  /**
   * Release a held slot immediately
   * @param {{holdToken:string, doctorId:number, slotId:number, scheduleDate:string}} request
   */
  releaseHold: async (request) => {
    const response = await api.post("/booking/release-hold", request);
    return response.data;
  },

  /**
   * Create a new booking
   * @param {Object} request - CreateBookingRequest
   * @returns {Promise<BookingResponse>}
   */
  createBooking: async (request) => {
    const response = await api.post("/booking/create", request);
    return response.data;
  },

  /**
   * Get booking by ID
   * @param {number} appointmentId - Appointment ID
   * @returns {Promise<BookingDto>}
   */
  getBookingById: async (appointmentId) => {
    const response = await api.get(`/booking/${appointmentId}`);
    return response.data;
  },

  /**
   * Search booking by appointment code or phone number
   * @param {string} [appointmentCode] - Appointment code (e.g., VC-20250101-123456)
   * @param {string} [phone] - Phone number
   * @returns {Promise<BookingDto>}
   */
  searchBooking: async (appointmentCode = null, phone = null) => {
    const params = new URLSearchParams();
    if (appointmentCode) params.append("appointmentCode", appointmentCode);
    if (phone) params.append("phone", phone);

    const response = await api.get(`/booking/search?${params}`);
    return response.data;
  },

  /**
   * Cancel a booking
   * @param {number} appointmentId - Appointment ID
   * @param {Object} request - CancelBookingRequest
   * @param {string} [request.reason] - Cancellation reason
   * @param {boolean} [request.requestRefund] - Whether to request refund
   * @returns {Promise<boolean>}
   */
  cancelBooking: async (appointmentId, request) => {
    const response = await api.put(`/booking/${appointmentId}/cancel`, request);
    return response.data;
  },

  /**
   * Initiate payment for an appointment (redirects to VNPay)
   * @param {number} appointmentId - Appointment ID
   * @returns {Promise<string>} Payment URL
   */
  initiatePayment: async (appointmentId) => {
    const response = await api.post(`/booking/${appointmentId}/payment/initiate`);
    return response.data.paymentUrl;
  },
};

// Type definitions (for reference):
/**
 * @typedef {Object} AvailableSlot
 * @property {number} slotId - Slot ID
 * @property {string} startTime - Start time (HH:mm format)
 * @property {string} endTime - End time (HH:mm format)
 * @property {number} durationMinutes - Duration in minutes
 * @property {number} availableCount - Available count (usually 1 or 0)
 * @property {boolean} isAvailable - Whether slot is available
 * @property {boolean} isOnHold - Whether slot is currently on hold
 */

/**
 * @typedef {Object} HoldSlotRequest
 * @property {number} doctorId - Doctor ID
 * @property {number} slotId - Slot ID
 * @property {string} scheduleDate - Date in YYYY-MM-DD format
 * @property {number} [customerId] - Optional customer ID
 */

/**
 * @typedef {Object} HoldSlotResponse
 * @property {string} holdToken - Hold token (UUID)
 * @property {string} expiresAt - Expiration time (ISO string)
 */

/**
 * @typedef {Object} CreateBookingRequest
 * @property {string} [holdToken] - Optional hold token
 * @property {number} doctorId - Doctor ID
 * @property {number} serviceDetailId - Service detail ID
 * @property {number} slotId - Slot ID
 * @property {string} scheduleDate - Date in YYYY-MM-DD format
 * @property {string} startTime - Start time (HH:mm format)
 * @property {number} [customerId] - Customer ID if logged in
 * @property {string} [customerName] - Customer name (if not logged in)
 * @property {string} [phone] - Phone number (if not logged in)
 * @property {string} [email] - Email (if not logged in)
 * @property {string} [notes] - Optional notes
 * @property {number} [discountId] - Optional discount ID
 */

/**
 * @typedef {Object} BookingResponse
 * @property {number} appointmentId - Appointment ID
 * @property {string} appointmentCode - Appointment code
 * @property {string} paymentStatus - Payment status (Unpaid, Paid, etc.)
 * @property {number} totalAmount - Total amount
 * @property {string} [paymentUrl] - Payment URL (if payment needed)
 */

/**
 * @typedef {Object} BookingDto
 * @property {number} id - Appointment ID
 * @property {string} appointmentCode - Appointment code
 * @property {string} appointmentDate - Appointment date (ISO string)
 * @property {string} status - Appointment status
 * @property {string} paymentStatus - Payment status
 * @property {number} doctorId - Doctor ID
 * @property {string} [doctorName] - Doctor name
 * @property {number} patientId - Patient ID
 * @property {string} [patientName] - Patient name
 * @property {number} serviceDetailId - Service detail ID
 * @property {string} [serviceName] - Service name
 * @property {number} [actualCost] - Actual cost
 * @property {string} [notes] - Notes
 * @property {string} createdAt - Creation date (ISO string)
 */
