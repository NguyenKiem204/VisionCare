import api from "../utils/api";

// Get service detail by ID
export const getServiceDetailById = async (id) => {
  const response = await api.get(`/servicedetails/${id}`);
  return response?.data?.data || response?.data || response;
};

// Get service details by service type ID
export const getServiceDetailsByServiceTypeId = async (serviceTypeId) => {
  const response = await api.get(`/servicedetails/by-service-type/${serviceTypeId}`);
  return response?.data?.data || response?.data || response;
};

