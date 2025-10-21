import api from "../utils/api";

// Get all staff with pagination and sorting
export const fetchStaff = async (params = {}) => {
  const { page = 0, size = 10, sortBy = "id", sortDir = "asc" } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: size.toString(),
    sortBy,
    desc: sortDir === "desc",
  });

  const response = await api.get(`/staff?${queryParams}`);
  return response;
};

// Search staff with filters
export const searchStaff = async (params = {}) => {
  const {
    keyword = "",
    gender = null,
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

  if (gender) queryParams.append("gender", gender);

  const response = await api.get(`/staff/search?${queryParams}`);
  return response;
};

// Get staff by ID
export const getStaffById = async (id) => {
  const response = await api.get(`/staff/${id}`);
  return response;
};

// Get staff by account ID
export const getStaffByAccountId = async (accountId) => {
  const response = await api.get(`/staff/account/${accountId}`);
  return response;
};

// Create new staff
export const createStaff = async (staffData) => {
  const response = await api.post("/staff", staffData);
  return response;
};

// Update staff
export const updateStaff = async (id, staffData) => {
  const response = await api.put(`/staff/${id}`, staffData);
  return response;
};

// Delete staff
export const deleteStaff = async (id) => {
  const response = await api.delete(`/staff/${id}`);
  return response;
};

// Get staff by gender
export const getStaffByGender = async (gender) => {
  const response = await api.get(`/staff/gender/${gender}`);
  return response;
};

// Update staff profile
export const updateStaffProfile = async (id, profileData) => {
  const response = await api.put(`/staff/${id}/profile`, profileData);
  return response;
};

// Get staff statistics
export const getStaffStatistics = async () => {
  const response = await api.get("/staff/statistics");
  return response;
};
