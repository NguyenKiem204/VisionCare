import api from "../utils/api";

// Get published blogs (public)
export const getPublishedBlogs = async (params = {}) => {
  const {
    keyword = "",
    page = 1,
    pageSize = 10,
    publishedFrom = null,
    publishedTo = null,
  } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
    status: "Published",
  });

  if (keyword) queryParams.append("keyword", keyword);
  if (publishedFrom) queryParams.append("publishedFrom", publishedFrom);
  if (publishedTo) queryParams.append("publishedTo", publishedTo);

  const response = await api.get(`/blog?${queryParams}`);
  return response.data;
};

// Get blog by ID
export const getBlogById = async (id) => {
  const response = await api.get(`/blog/${id}`);
  return response.data;
};

// Get blog by slug (public)
export const getBlogBySlug = async (slug) => {
  const response = await api.get(`/blog/slug/${slug}`);
  return response.data;
};

// Get current user's blogs
export const getMyBlogs = async (params = {}) => {
  const {
    keyword = "",
    status = null,
    page = 1,
    pageSize = 10,
    publishedFrom = null,
    publishedTo = null,
  } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
  });

  if (keyword) queryParams.append("keyword", keyword);
  if (status) queryParams.append("status", status);
  if (publishedFrom) queryParams.append("publishedFrom", publishedFrom);
  if (publishedTo) queryParams.append("publishedTo", publishedTo);

  const response = await api.get(`/blog/me?${queryParams}`);
  return response.data;
};

// Get all blogs (Admin only)
export const getAllBlogs = async (params = {}) => {
  const {
    keyword = "",
    authorId = null,
    status = null,
    page = 1,
    pageSize = 10,
    publishedFrom = null,
    publishedTo = null,
  } = params;

  const queryParams = new URLSearchParams({
    page: page.toString(),
    pageSize: pageSize.toString(),
  });

  if (keyword) queryParams.append("keyword", keyword);
  if (authorId) queryParams.append("authorId", authorId.toString());
  if (status) queryParams.append("status", status);
  if (publishedFrom) queryParams.append("publishedFrom", publishedFrom);
  if (publishedTo) queryParams.append("publishedTo", publishedTo);

  const response = await api.get(`/blog/admin?${queryParams}`);
  return response.data;
};

// Create blog
export const createBlog = async (blogData, featuredImageFile = null) => {
  const formData = new FormData();
  formData.append("title", blogData.title);
  formData.append("content", blogData.content);
  if (blogData.slug) formData.append("slug", blogData.slug);
  if (blogData.excerpt) formData.append("excerpt", blogData.excerpt);
  if (blogData.featuredImage && !featuredImageFile) {
    formData.append("featuredImage", blogData.featuredImage);
  }
  if (blogData.status) formData.append("status", blogData.status);
  if (featuredImageFile) {
    formData.append("featuredImage", featuredImageFile);
  }

  const response = await api.post("/blog", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return response.data;
};

// Update blog
export const updateBlog = async (id, blogData, featuredImageFile = null) => {
  const formData = new FormData();
  formData.append("title", blogData.title);
  formData.append("content", blogData.content);
  if (blogData.slug) formData.append("slug", blogData.slug);
  if (blogData.excerpt) formData.append("excerpt", blogData.excerpt);
  if (blogData.featuredImage && !featuredImageFile) {
    formData.append("featuredImage", blogData.featuredImage);
  }
  if (blogData.status) formData.append("status", blogData.status);
  if (featuredImageFile) {
    formData.append("featuredImage", featuredImageFile);
  }

  const response = await api.put(`/blog/${id}`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return response.data;
};

// Delete blog
export const deleteBlog = async (id) => {
  const response = await api.delete(`/blog/${id}`);
  return response.data;
};

// Publish blog
export const publishBlog = async (id) => {
  const response = await api.post(`/blog/${id}/publish`);
  return response.data;
};

// Unpublish blog
export const unpublishBlog = async (id) => {
  const response = await api.post(`/blog/${id}/unpublish`);
  return response.data;
};

// Increment view count
export const incrementViewCount = async (id) => {
  const response = await api.post(`/blog/${id}/view`);
  return response.data;
};

// Check if slug exists
export const checkSlugExists = async (slug, excludeBlogId = null) => {
  const queryParams = new URLSearchParams({ slug });
  if (excludeBlogId) queryParams.append("excludeBlogId", excludeBlogId.toString());

  const response = await api.get(`/blog/check-slug?${queryParams}`);
  return response.data;
};

