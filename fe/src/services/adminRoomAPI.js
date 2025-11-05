import api from "../utils/api";

export const fetchRooms = async () => {
  const response = await api.get("/rooms");
  return response;
};

export const getActiveRooms = async () => {
  const response = await api.get("/rooms/active");
  return response;
};

export const getRoomById = async (id) => {
  const response = await api.get(`/rooms/${id}`);
  return response;
};

export const getRoomByCode = async (roomCode) => {
  const response = await api.get(`/rooms/code/${roomCode}`);
  return response;
};

export const searchRooms = async (params = {}) => {
  const response = await api.get("/rooms/search", { params });
  return response;
};

export const createRoom = async (roomData) => {
  const response = await api.post("/rooms", roomData);
  return response;
};

export const updateRoom = async (id, roomData) => {
  const response = await api.put(`/rooms/${id}`, roomData);
  return response;
};

export const deleteRoom = async (id) => {
  const response = await api.delete(`/rooms/${id}`);
  return response;
};

