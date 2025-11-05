import api from "../utils/api";

// Get all doctors with pagination and sorting
export const fetchDoctors = async (params = {}) => {
  const { page = 0, size = 10, sortBy = "id", sortDir = "asc" } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: size.toString(),
    sortBy,
    desc: sortDir === "desc",
  });

  const response = await api.get(`/doctors?${queryParams}`);
  return response;
};

// Search doctors with filters
export const searchDoctors = async (params = {}) => {
  const {
    keyword = "",
    specializationId = null,
    minRating = null,
    page = 1,
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

  if (specializationId)
    queryParams.append("specializationId", specializationId);
  if (minRating) queryParams.append("minRating", minRating);

  const response = await api.get(`/doctors/search?${queryParams}`);
  return response;
};

// Get doctor by ID
export const getDoctorById = async (id) => {
  const response = await api.get(`/doctors/${id}`);
  return response;
};

// Create new doctor
export const createDoctor = async (doctorData) => {
  const response = await api.post("/doctors", doctorData);
  return response;
};

// Update doctor
export const updateDoctor = async (id, doctorData) => {
  const response = await api.put(`/doctors/${id}`, doctorData);
  return response;
};

// Delete doctor
export const deleteDoctor = async (id) => {
  const response = await api.delete(`/doctors/${id}`);
  return response;
};

// Get doctors by specialization
export const getDoctorsBySpecialization = async (specializationId) => {
  const response = await api.get(`/doctors/specialization/${specializationId}`);
  return response;
};

// Get doctors by service (serviceDetailId)
export const getDoctorsByService = async (serviceDetailId) => {
  const response = await api.get(`/doctors/service/${serviceDetailId}`);
  return response;
};

// Get available doctors
export const getAvailableDoctors = async (date) => {
  const response = await api.get(`/doctors/available?date=${date}`);
  return response;
};

// Update doctor rating
export const updateDoctorRating = async (id, rating) => {
  const response = await api.put(`/doctors/${id}/rating`, rating);
  return response;
};

// Update doctor status
export const updateDoctorStatus = async (id, status) => {
  const response = await api.put(`/doctors/${id}/status`, status);
  return response;
};

// Get top rated doctors
export const getTopRatedDoctors = async (count = 5) => {
  const response = await api.get(`/doctors/top-rated?count=${count}`);
  return response;
};

// Get doctor statistics
export const getDoctorStatistics = async () => {
  const response = await api.get("/doctors/statistics");
  return response;
};
