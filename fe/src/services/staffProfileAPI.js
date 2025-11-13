import api from "../utils/api";

export const getMyProfile = async () => {
  const res = await api.get("/staff/me/profile");
  return res.data?.data || res.data;
};

export const updateMyProfile = async (data, avatarFile = null) => {
  const formData = new FormData();
  formData.append("staffName", data.staffName || "");
  if (data.gender) formData.append("gender", data.gender);
  if (data.dob) formData.append("dob", data.dob);
  if (data.address) formData.append("address", data.address);
  if (data.phone) formData.append("phone", data.phone);
  if (avatarFile) {
    formData.append("avatar", avatarFile);
  }

  const res = await api.put("/staff/me/profile", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data?.data || res.data;
};

