import api from "../utils/api";

// Get all certificates
export const fetchCertificates = async () => {
  const response = await api.get("/certificates");
  return response;
};

// Get all degrees
export const fetchDegrees = async () => {
  const response = await api.get("/degrees");
  return response;
};

// Get all doctor certificates
export const fetchDoctorCertificates = async () => {
  const response = await api.get("/doctorcertificates");
  return response;
};

// Get certificates by doctor ID
export const getCertificatesByDoctor = async (doctorId) => {
  const response = await api.get(`/doctorcertificates/doctor/${doctorId}`);
  return response;
};

// Get doctor certificate by ID
export const getDoctorCertificateById = async (id) => {
  const response = await api.get(`/doctorcertificates/${id}`);
  return response;
};

// Create new doctor certificate
export const createDoctorCertificate = async (doctorCertificateData) => {
  const response = await api.post("/doctorcertificates", doctorCertificateData);
  return response;
};

// Update doctor certificate
export const updateDoctorCertificate = async (id, doctorCertificateData) => {
  const response = await api.put(`/doctorcertificates/${id}`, doctorCertificateData);
  return response;
};

// Delete doctor certificate
export const deleteDoctorCertificate = async (id) => {
  const response = await api.delete(`/doctorcertificates/${id}`);
  return response;
};

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
