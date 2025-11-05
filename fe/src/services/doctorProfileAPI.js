import api from "../utils/api";

export const getMyProfile = async () => {
  const res = await api.get("/doctor/me/profile");
  return res;
};

export const updateMyProfile = async (data) => {
  const res = await api.put("/doctor/me/profile", data);
  return res;
};

export const getMyCertificates = async () => {
  const res = await api.get("/doctor/me/certificates");
  return res;
};

export const createMyCertificate = async (data) => {
  const res = await api.post("/doctor/me/certificates", data);
  return res;
};

export const deleteMyCertificate = async (certificateId) => {
  const res = await api.delete(`/doctor/me/certificates/${certificateId}`);
  return res;
};


