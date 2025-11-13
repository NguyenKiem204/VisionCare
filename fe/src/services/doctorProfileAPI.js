import api from "../utils/api";

export const getMyProfile = async () => {
  const res = await api.get("/doctor/me/profile");
  return res.data?.data || res.data;
};

export const updateMyProfile = async (data, avatarFile = null) => {
  const formData = new FormData();
  formData.append("doctorName", data.doctorName || "");
  if (data.phone) formData.append("phone", data.phone);
  if (data.experienceYears !== undefined) formData.append("experienceYears", data.experienceYears);
  if (data.specializationId) formData.append("specializationId", data.specializationId);
  if (data.gender) formData.append("gender", data.gender);
  if (data.dob) formData.append("dob", data.dob);
  if (data.address) formData.append("address", data.address);
  if (avatarFile) {
    formData.append("avatar", avatarFile);
  }

  const res = await api.put("/doctor/me/profile", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data?.data || res.data;
};

export const getMyCertificates = async () => {
  const res = await api.get("/doctor/me/certificates");
  return res.data?.data || res.data;
};

export const createMyCertificate = async (data, certificateImageFile = null) => {
  const formData = new FormData();
  formData.append("certificateId", data.certificateId || "");
  if (data.issuedDate) formData.append("issuedDate", data.issuedDate);
  if (data.issuedBy) formData.append("issuedBy", data.issuedBy);
  if (data.expiryDate) formData.append("expiryDate", data.expiryDate);
  if (data.status) formData.append("status", data.status);
  if (certificateImageFile) {
    formData.append("certificateImage", certificateImageFile);
  }

  const res = await api.post("/doctor/me/certificates", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data?.data || res.data;
};

export const deleteMyCertificate = async (certificateId) => {
  const res = await api.delete(`/doctor/me/certificates/${certificateId}`);
  return res;
};

export const getMyDegrees = async () => {
  const res = await api.get("/doctor/me/degrees");
  return res.data?.data || res.data;
};

export const createMyDegree = async (data, certificateImageFile = null) => {
  const formData = new FormData();
  formData.append("degreeId", data.degreeId || "");
  if (data.issuedDate) formData.append("issuedDate", data.issuedDate);
  if (data.issuedBy) formData.append("issuedBy", data.issuedBy);
  if (data.status) formData.append("status", data.status);
  if (certificateImageFile) {
    formData.append("certificateImage", certificateImageFile);
  }

  const res = await api.post("/doctor/me/degrees", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data?.data || res.data;
};

export const deleteMyDegree = async (degreeId) => {
  const res = await api.delete(`/doctor/me/degrees/${degreeId}`);
  return res;
};


