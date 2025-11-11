import api from "../utils/api";

// Get comments for a blog
export const getComments = async (blogId) => {
  const response = await api.get(`/blog/${blogId}/comments`);
  return response.data;
};

// Get comment by ID
export const getCommentById = async (blogId, commentId) => {
  const response = await api.get(`/blog/${blogId}/comments/${commentId}`);
  return response.data;
};

// Create comment
export const createComment = async (blogId, commentData) => {
  const response = await api.post(`/blog/${blogId}/comments`, commentData);
  return response.data;
};

// Update comment
export const updateComment = async (blogId, commentId, commentData) => {
  const response = await api.put(`/blog/${blogId}/comments/${commentId}`, commentData);
  return response.data;
};

// Delete comment
export const deleteComment = async (blogId, commentId) => {
  const response = await api.delete(`/blog/${blogId}/comments/${commentId}`);
  return response.data;
};

