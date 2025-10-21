import api from "../utils/api";

// Get all users with pagination and sorting
export const fetchUsers = async (params = {}) => {
  const { page = 0, size = 10, sortBy = "id", sortDir = "asc" } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: size.toString(),
    sortBy,
    desc: sortDir === "desc",
  });

  const response = await api.get(`/users?${queryParams}`);
  return response;
};

// Search users with filters
export const searchUsers = async (params = {}) => {
  const {
    keyword = "",
    roleId = null,
    status = null,
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

  if (roleId) queryParams.append("roleId", roleId);
  if (status) queryParams.append("status", status);

  const response = await api.get(`/users/search?${queryParams}`);
  return response;
};

// Get user by ID
export const getUserById = async (id) => {
  const response = await api.get(`/users/${id}`);
  return response;
};

// Create new user
export const createUser = async (userData) => {
  const response = await api.post("/users", userData);
  return response;
};

// Update user
export const updateUser = async (id, userData) => {
  const response = await api.put(`/users/${id}`, userData);
  return response;
};

// Delete user
export const deleteUser = async (id) => {
  const response = await api.delete(`/users/${id}`);
  return response;
};

// Get users by role
export const getUsersByRole = async (roleId) => {
  const response = await api.get(`/users/role/${roleId}`);
  return response;
};

// Get active users
export const getActiveUsers = async () => {
  const response = await api.get("/users/active");
  return response;
};

// Get inactive users
export const getInactiveUsers = async () => {
  const response = await api.get("/users/inactive");
  return response;
};

// Activate user
export const activateUser = async (id) => {
  const response = await api.put(`/users/${id}/activate`);
  return response;
};

// Deactivate user
export const deactivateUser = async (id) => {
  const response = await api.put(`/users/${id}/deactivate`);
  return response;
};

// Change user password
export const changePassword = async (id, newPassword) => {
  const response = await api.put(`/users/${id}/password`, {
    newPassword,
  });
  return response;
};

// Update user role
export const updateUserRole = async (id, roleId) => {
  const response = await api.put(`/users/${id}/role`, {
    roleId,
  });
  return response;
};

// Get user statistics
export const getUserStatistics = async () => {
  const response = await api.get("/users/statistics");
  return response;
};
