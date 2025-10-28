import api from "../utils/api";

export const fetchServiceTypes = async () => {
  const response = await api.get("/servicetypes");
  return response;
};

export const getServiceTypeById = async (id) => {
  const response = await api.get(`/servicetypes/${id}`);
  return response;
};

export const searchServiceTypes = async (params = {}) => {
  const response = await api.get("/servicetypes/search", { params });
  return response;
};

export const createServiceType = async (serviceTypeData) => {
  const response = await api.post("/servicetypes", serviceTypeData);
  return response;
};

export const updateServiceType = async (id, serviceTypeData) => {
  const response = await api.put(`/servicetypes/${id}`, serviceTypeData);
  return response;
};

export const deleteServiceType = async (id) => {
  const response = await api.delete(`/servicetypes/${id}`);
  return response;
};
