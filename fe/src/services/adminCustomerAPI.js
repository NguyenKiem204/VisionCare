import api from "../utils/api";

// Get all customers with pagination and sorting
export const fetchCustomers = async (params = {}) => {
  const { page = 0, size = 10, sortBy = "id", sortDir = "asc" } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: size.toString(),
    sortBy,
    desc: sortDir === "desc",
  });

  const response = await api.get(`/customers?${queryParams}`);
  return response;
};

// Search customers with filters
export const searchCustomers = async (params = {}) => {
  const {
    keyword = "",
    gender = null,
    fromDob = null,
    toDob = null,
    page = 0,
    pageSize = 10,
    sortBy = "id",
    desc = false,
  } = params;

  const queryParams = new URLSearchParams({
    keyword,
    page: page.toString(),
    pageSize: pageSize.toString(),
    sortBy,
    desc: desc.toString(),
  });

  if (gender) queryParams.append("gender", gender);
  if (fromDob) queryParams.append("fromDob", fromDob);
  if (toDob) queryParams.append("toDob", toDob);

  const response = await api.get(`/customers/search?${queryParams}`);
  return response;
};

// Get customer by ID
export const getCustomerById = async (id) => {
  const response = await api.get(`/customers/${id}`);
  return response;
};

// Get customer by account ID
export const getCustomerByAccountId = async (accountId) => {
  const response = await api.get(`/customers/account/${accountId}`);
  return response;
};

// Create new customer
export const createCustomer = async (customerData) => {
  const response = await api.post("/customers", customerData);
  return response;
};

// Update customer
export const updateCustomer = async (id, customerData) => {
  const response = await api.put(`/customers/${id}`, customerData);
  return response;
};

// Delete customer
export const deleteCustomer = async (id) => {
  const response = await api.delete(`/customers/${id}`);
  return response;
};

// Get customers by gender
export const getCustomersByGender = async (gender) => {
  const response = await api.get(`/customers/gender/${gender}`);
  return response;
};

// Get customers by age range
export const getCustomersByAgeRange = async (minAge, maxAge) => {
  const response = await api.get(
    `/customers/age-range?minAge=${minAge}&maxAge=${maxAge}`
  );
  return response;
};

// Update customer profile
export const updateCustomerProfile = async (id, profileData) => {
  const response = await api.put(`/customers/${id}/profile`, profileData);
  return response;
};

// Get customer statistics
export const getCustomerStatistics = async () => {
  const response = await api.get("/customers/statistics");
  return response;
};
