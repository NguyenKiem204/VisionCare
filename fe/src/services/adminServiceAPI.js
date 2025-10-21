import api from "../utils/api";

// Get all services
export const fetchServices = async (params = {}) => {
  const { page = 0, size = 10, sortBy = "id", sortDir = "asc" } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: size.toString(),
    sortBy,
    desc: sortDir === "desc",
  });

  const response = await api.get(`/servicesmanagement?${queryParams}`);
  return response;
};

// Search services with filters
export const searchServices = async (params = {}) => {
  const {
    keyword = "",
    specializationId = null,
    isActive = null,
    page = 0,
    pageSize = 10,
    sortBy = "id",
    desc = false,
  } = params;

  const searchRequest = {
    keyword,
    specializationId,
    isActive,
    page,
    pageSize,
    sortBy,
    desc,
  };

  const response = await api.post("/servicesmanagement/search", searchRequest);
  return response;
};

// Get service by ID
export const getServiceById = async (id) => {
  const response = await api.get(`/servicesmanagement/${id}`);
  return response;
};

// Get service by name
export const getServiceByName = async (name) => {
  const response = await api.get(`/servicesmanagement/name/${name}`);
  return response;
};

// Get services by specialization
export const getServicesBySpecialization = async (specializationId) => {
  const response = await api.get(
    `/servicesmanagement/by-specialization/${specializationId}`
  );
  return response;
};

// Get active services only
export const getActiveServices = async () => {
  const response = await api.get("/servicesmanagement/active");
  return response;
};

// Create new service
export const createService = async (serviceData) => {
  const response = await api.post("/servicesmanagement", serviceData);
  return response;
};

// Update service
export const updateService = async (id, serviceData) => {
  const response = await api.put(`/servicesmanagement/${id}`, serviceData);
  return response;
};

// Delete service
export const deleteService = async (id) => {
  const response = await api.delete(`/servicesmanagement/${id}`);
  return response;
};

// Activate service
export const activateService = async (id) => {
  const response = await api.put(`/servicesmanagement/${id}/activate`);
  return response;
};

// Deactivate service
export const deactivateService = async (id) => {
  const response = await api.put(`/servicesmanagement/${id}/deactivate`);
  return response;
};

// Get total services count
export const getTotalServicesCount = async () => {
  const response = await api.get("/servicesmanagement/count");
  return response;
};
