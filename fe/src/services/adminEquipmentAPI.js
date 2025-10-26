import api from "../utils/api";

export const fetchEquipment = async () => {
  const response = await api.get("/equipment");
  return response;
};

export const getEquipmentById = async (id) => {
  const response = await api.get(`/equipment/${id}`);
  return response;
};

export const searchEquipment = async (params = {}) => {
  const response = await api.get("/equipment/search", { params });
  return response;
};

export const createEquipment = async (equipmentData) => {
  const response = await api.post("/equipment", equipmentData);
  return response;
};

export const updateEquipment = async (id, equipmentData) => {
  const response = await api.put(`/equipment/${id}`, equipmentData);
  return response;
};

export const deleteEquipment = async (id) => {
  const response = await api.delete(`/equipment/${id}`);
  return response;
};
