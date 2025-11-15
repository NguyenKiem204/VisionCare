import api from "../utils/api";

export async function getBanners(place = "home_hero") {
  try {
    const res = await api.get(`/banner`, { params: { place } });
    // Backend trả về list trực tiếp hoặc wrap trong ApiResponse
    // Nếu là ApiResponse, lấy res.data.data, nếu không thì lấy res.data
    if (res.data && Array.isArray(res.data)) {
      return res.data;
    }
    if (res.data?.data && Array.isArray(res.data.data)) {
      return res.data.data;
    }
    if (res.data?.items && Array.isArray(res.data.items)) {
      return res.data.items;
    }
    return [];
  } catch (error) {
    console.error("Error fetching banners:", error);
    return [];
  }
}


