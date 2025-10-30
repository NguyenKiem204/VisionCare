import api from "../utils/api";

export async function getSectionByKey(key) {
  const res = await api.get(`/sectioncontent/${key}`);
  return res.data;
}

export async function getAllSections() {
  const res = await api.get(`/sectioncontent`);
  return res.data;
}

// Strongly-typed endpoints
export async function putWhyUs(payload, files) {
  // payload: { title, subtitle, bullets: string[4], images: string[4] }
  // files: (optional) File[] length <= 4 mapped by index
  const form = new FormData();
  form.append("title", payload.title || "");
  form.append("subtitle", payload.subtitle || "");
  const bullets = Array.from({ length: 4 }, (_, i) => (payload.bullets?.[i] ?? ""));
  // Gửi theo dạng lặp tên trường để ASP.NET bind List<string>
  bullets.forEach((b) => form.append("bullets", b));
  // Chỉ chọn file, kèm index để BE map chính xác vị trí
  for (let i = 0; i < 4; i++) {
    const f = files?.[i] || null;
    if (f) {
      form.append("imageFiles", f);
      form.append("imageIndexes", String(i));
    }
  }
  await api.put(`/sectioncontent/why_us`, form, {
    headers: { "Content-Type": "multipart/form-data" },
  });
}

export async function putAbout(payload, file) {
  // payload: { title, content, image? }
  const form = new FormData();
  form.append("title", payload.title || "");
  form.append("content", payload.content || "");
  if (payload.image) form.append("image", payload.image);
  if (file) form.append("image", file);
  await api.put(`/sectioncontent/about`, form, {
    headers: { "Content-Type": "multipart/form-data" },
  });
}


