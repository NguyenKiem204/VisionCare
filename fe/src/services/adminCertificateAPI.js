import api from "../utils/api";

// Get all certificates
export const fetchCertificates = async () => {
  const response = await api.get("/certificates");
  return response;
};

// Get certificate by ID
export const getCertificateById = async (id) => {
  const response = await api.get(`/certificates/${id}`);
  return response;
};

// Create new certificate
export const createCertificate = async (certificateData) => {
  const response = await api.post("/certificates", certificateData);
  return response;
};

// Update certificate
export const updateCertificate = async (id, certificateData) => {
  const response = await api.put(`/certificates/${id}`, certificateData);
  return response;
};

// Delete certificate
export const deleteCertificate = async (id) => {
  const response = await api.delete(`/certificates/${id}`);
  return response;
};
