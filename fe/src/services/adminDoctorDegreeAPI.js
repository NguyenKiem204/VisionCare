import api from "../utils/api";

// Get all doctor degrees
export const fetchDoctorDegrees = async () => {
  const response = await api.get("/doctordegrees");
  return response;
};

// Get degrees by doctor ID
export const getDegreesByDoctor = async (doctorId) => {
  const response = await api.get(`/doctordegrees/doctor/${doctorId}`);
  return response;
};

// Get doctor degree by ID
export const getDoctorDegreeById = async (id) => {
  const response = await api.get(`/doctordegrees/${id}`);
  return response;
};

// Create new doctor degree
export const createDoctorDegree = async (doctorDegreeData) => {
  const response = await api.post("/doctordegrees", doctorDegreeData);
  return response;
};

// Update doctor degree
export const updateDoctorDegree = async (id, doctorDegreeData) => {
  const response = await api.put(`/doctordegrees/${id}`, doctorDegreeData);
  return response;
};

// Delete doctor degree
export const deleteDoctorDegree = async (id) => {
  const response = await api.delete(`/doctordegrees/${id}`);
  return response;
};
