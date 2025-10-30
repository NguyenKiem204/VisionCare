import api from "../utils/api";

export async function getBanners(place = "home_hero") {
  const res = await api.get(`/banner`, { params: { place } });
  return res.data;
}


