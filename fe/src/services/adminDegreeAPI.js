import api from "../utils/api";

// Get all degrees
export const fetchDegrees = async () => {
  const response = await api.get("/degrees");
  return response;
};

// Get degree by ID
export const getDegreeById = async (id) => {
  const response = await api.get(`/degrees/${id}`);
  return response;
};

// Create new degree
export const createDegree = async (degreeData) => {
  const response = await api.post("/degrees", degreeData);
  return response;
};

// Update degree
export const updateDegree = async (id, degreeData) => {
  const response = await api.put(`/degrees/${id}`, degreeData);
  return response;
};

// Delete degree
export const deleteDegree = async (id) => {
  const response = await api.delete(`/degrees/${id}`);
  return response;
};
