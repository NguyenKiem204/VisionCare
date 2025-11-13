import api from "../utils/api";

export const getMyProfile = async () => {
  const res = await api.get("/admin/me/profile");
  return res.data?.data || res.data;
};

export const updateMyProfile = async (data) => {
  const res = await api.put("/admin/me/profile", data);
  return res.data?.data || res.data;
};

