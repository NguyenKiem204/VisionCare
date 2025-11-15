import api from "../utils/api";

export const register = async (request) => {
  const response = await api.post("/auth/register", {
    username: request.username,
    email: request.email,
    password: request.password,
    confirmPassword: request.confirmPassword,
  });
  return response.data;
};

