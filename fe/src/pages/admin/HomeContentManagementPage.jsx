import React, { useEffect, useMemo, useState } from "react";
import { getAllSections, putWhyUs, putAbout } from "../../services/sectionContentAPI";
import { getBanners } from "../../services/bannerAPI";
import api from "../../utils/api";
// Upload ảnh sẽ do BE xử lý trong chính endpoint POST/PUT banner/section

export default function HomeContentManagementPage() {
  const [activeTab, setActiveTab] = useState("sections");
  const [sections, setSections] = useState([]);
  const [banners, setBanners] = useState([]);
  const [loading, setLoading] = useState(false);

  const sectionKeys = useMemo(() => ["why_us", "about", "background_image"], []);

  const load = async () => {
    setLoading(true);
    try {
      const [sec, ban] = await Promise.all([
        getAllSections(),
        getBanners("home_hero"),
      ]);
      setSections(sec || []);
      setBanners(ban || []);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const upsertSection = async (payload, file) => {
    const form = new FormData();
    form.append("sectionKey", payload.sectionKey);
    if (payload.content != null) form.append("content", payload.content);
    if (payload.moreData != null) form.append("moreData", payload.moreData);
    if (payload.imageUrl) form.append("imageUrl", payload.imageUrl);
    if (file) form.append("image", file);
    const exists = sections.find((s) => s.sectionKey === payload.sectionKey);
    if (exists) {
      await api.put(`/sectioncontent/${payload.sectionKey}`, form, { headers: { "Content-Type": "multipart/form-data" } });
    } else {
      await api.post(`/sectioncontent`, form, { headers: { "Content-Type": "multipart/form-data" } });
    }
    await load();
  };

  const deleteSection = async (key) => {
    await api.delete(`/sectioncontent/${key}`);
    await load();
  };

  const createBanner = async (payload, file) => {
    const form = new FormData();
    Object.entries(payload).forEach(([k,v])=>{ if(v!==undefined && v!==null) form.append(k, v); });
    if (file) form.append("image", file);
    await api.post(`/banner`, form, { headers: { "Content-Type": "multipart/form-data" } });
    await load();
  };

  const updateBanner = async (id, payload, file) => {
    const form = new FormData();
    Object.entries(payload).forEach(([k,v])=>{ if(v!==undefined && v!==null) form.append(k, v); });
    if (file) form.append("image", file);
    await api.put(`/banner/${id}`, form, { headers: { "Content-Type": "multipart/form-data" } });
    await load();
  };

  const deleteBanner = async (id) => {
    await api.delete(`/banner/${id}`);
    await load();
  };

  const onUpload = async () => {};

  return (
    <div className="p-6">
      <div className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Quản lý nội dung Home</h1>
          <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">Cập nhật các khối nội dung và slideshow trang chủ.</p>
        </div>
      </div>

      <div className="inline-flex rounded-full bg-gray-100 dark:bg-gray-800 p-1 mb-6">
        <button
          className={`px-4 py-2 rounded-full text-sm transition ${
            activeTab === "sections"
              ? "bg-white dark:bg-gray-900 shadow text-blue-600 dark:text-blue-300"
              : "text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white"
          }`}
          onClick={() => setActiveTab("sections")}
        >
          Sections
        </button>
        <button
          className={`px-4 py-2 rounded-full text-sm transition ${
            activeTab === "banners"
              ? "bg-white dark:bg-gray-900 shadow text-blue-600 dark:text-blue-300"
              : "text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white"
          }`}
          onClick={() => setActiveTab("banners")}
        >
          Banners (Hero Slider)
        </button>
      </div>

      {loading && (
        <div className="grid gap-4 md:grid-cols-2">
          {[...Array(4)].map((_, i) => (
            <div key={i} className="animate-pulse h-40 rounded-xl bg-gray-200 dark:bg-gray-800" />
          ))}
        </div>
      )}

      {!loading && activeTab === "sections" && (
        <div className="grid gap-6 md:grid-cols-2">
          {sectionKeys.map((key) => {
            const data = sections.find((s) => s.sectionKey === key) || {
              sectionKey: key,
              content: "",
              imageUrl: "",
              moreData: "",
            };
            if (key === "why_us") {
              return (
                <WhyUsEditor key={key} data={data} onSaved={load} />
              );
            }
            if (key === "about") {
              return (
                <AboutEditor key={key} data={data} onSaved={load} />
              );
            }
            return (
              <SectionEditor
                key={key}
                data={data}
                onSave={upsertSection}
                onDelete={deleteSection}
              />
            );
          })}
        </div>
      )}

      {!loading && activeTab === "banners" && (
        <BannerManager
          items={banners}
          onCreate={createBanner}
          onUpdate={updateBanner}
          onDelete={deleteBanner}
          onUpload={onUpload}
        />
      )}
    </div>
  );
}

function SectionEditor({ data, onSave, onDelete }) {
  const [form, setForm] = useState({ ...data });
  const [file, setFile] = useState(null);
  useEffect(() => setForm({ ...data }), [data.sectionKey]);

  const pickImage = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setFile(file);
  };

  return (
    <div className="rounded-2xl border border-gray-200 dark:border-gray-800 bg-white dark:bg-gray-900 p-5 shadow-sm">
      <div className="flex items-start justify-between mb-3">
        <div>
          <h3 className="font-semibold text-gray-900 dark:text-white">Section: {form.sectionKey}</h3>
          <p className="text-xs text-gray-500 dark:text-gray-400 mt-0.5">Chỉnh sửa nội dung và ảnh hiển thị.</p>
        </div>
        {(file || form.imageUrl) ? (
          <img src={file ? URL.createObjectURL(file) : form.imageUrl} alt="preview" className="w-20 h-12 object-cover rounded-md border border-gray-200 dark:border-gray-800" />
        ) : (
          <div className="w-20 h-12 rounded-md border border-dashed border-gray-300 dark:border-gray-700 flex items-center justify-center text-xs text-gray-400 dark:text-gray-300">No Image</div>
        )}
      </div>
      <div className="grid gap-3">
        <input
          className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100 focus:outline-none focus:ring-2 focus:ring-blue-500"
          placeholder="Content"
          value={form.content || ""}
          onChange={(e) => setForm({ ...form, content: e.target.value })}
        />
        <div className="flex items-center gap-2">
          <input
            className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100 flex-1 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Image URL"
            value={form.imageUrl || ""}
            onChange={(e) => setForm({ ...form, imageUrl: e.target.value })}
          />
          <label className="inline-flex items-center px-3 py-2 rounded-lg bg-gray-100 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 cursor-pointer text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-200 dark:hover:bg-gray-700">
            Tải ảnh
            <input type="file" className="hidden" onChange={pickImage} />
          </label>
        </div>
        <textarea
          className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100 font-mono text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          placeholder="MoreData (JSON)"
          rows={4}
          value={form.moreData || ""}
          onChange={(e) => setForm({ ...form, moreData: e.target.value })}
        />
      </div>
      <div className="mt-4 flex gap-2">
        <button
          onClick={() => onSave(form, file)}
          className="inline-flex items-center px-4 py-2 rounded-lg bg-blue-600 hover:bg-blue-700 text-white shadow-sm"
        >
          Lưu
        </button>
        {data?.content || data?.imageUrl || data?.moreData ? (
          <button
            onClick={() => onDelete(form.sectionKey)}
            className="inline-flex items-center px-4 py-2 rounded-lg bg-red-600 hover:bg-red-700 text-white"
          >
            Xóa
          </button>
        ) : null}
      </div>
    </div>
  );
}

function WhyUsEditor({ data, onSaved }) {
  const initial = useMemo(() => {
    let md = {};
    try { md = data.moreData ? JSON.parse(data.moreData) : {}; } catch {}
    // Hỗ trợ cả key viết hoa (dữ liệu cũ)
    const title = md.title ?? md.Title ?? "Tại sao VisionCare là sự lựa chọn hàng đầu?";
    const subtitle = md.subtitle ?? md.Subtitle ?? data.content ?? "";
    const bullets = md.bullets ?? md.Bullets ?? ["", "", "", ""];
    const images = md.images ?? md.Images ?? ["", "", "", ""];
    return {
      title,
      subtitle,
      bullets,
      images,
    };
  }, [data.moreData, data.content]);
  const [form, setForm] = useState(initial);
  const [files, setFiles] = useState([null, null, null, null]);

  // Đồng bộ lại form khi dữ liệu từ API thay đổi
  useEffect(() => {
    setForm(initial);
    setFiles([null, null, null, null]);
  }, [initial]);

  const updateBullet = (i, v) => setForm((f)=>({ ...f, bullets: f.bullets.map((b,idx)=> idx===i? v : b) }));
  const updateImage = (i, v) => setForm((f)=>({ ...f, images: f.images.map((u,idx)=> idx===i? v : u) }));
  const updateImageFile = (i, file) => {
    setFiles((arr)=> arr.map((f,idx)=> idx===i? file : f));
    if (file) {
      const blobUrl = URL.createObjectURL(file);
      setForm((f)=> ({ ...f, images: f.images.map((u,idx)=> idx===i? blobUrl : u) }));
    }
  };

  const save = async () => {
    await putWhyUs(form, files);
    onSaved && onSaved();
  };

  return (
    <div className="rounded-2xl border border-gray-200 dark:border-gray-800 bg-white dark:bg-gray-900 p-5 shadow-sm">
      <h3 className="font-semibold text-gray-900 dark:text-white mb-3">Why Us</h3>
      <div className="grid gap-3">
        <input className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700" placeholder="Title" value={form.title} onChange={(e)=>setForm({...form,title:e.target.value})} />
        <input className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700" placeholder="Subtitle" value={form.subtitle} onChange={(e)=>setForm({...form,subtitle:e.target.value})} />
        <div className="grid md:grid-cols-2 gap-2">
          {form.bullets.map((b, i)=> (
            <input key={i} className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700" placeholder={`Bullet ${i+1}`} value={b} onChange={(e)=>updateBullet(i, e.target.value)} />
          ))}
        </div>
        <div className="grid md:grid-cols-2 gap-3">
          {form.images.map((u, i)=> (
            <div key={i} className="flex items-center gap-2">
              {/* Chỉ chọn file, không nhập URL thủ công */}
              <label className="btn-ghost">
                {files[i]?.name || `Tải ảnh ${i+1}`}
                <input type="file" className="hidden" onChange={(e)=>updateImageFile(i, e.target.files?.[0]||null)} />
              </label>
              {(files[i] || u) && (
                <img src={files[i] ? URL.createObjectURL(files[i]) : u} alt={`preview-${i+1}`} className="w-16 h-10 object-cover rounded border border-gray-200 dark:border-gray-700" />
              )}
            </div>
          ))}
        </div>
      </div>
      <div className="mt-4">
        <button onClick={save} className="btn-primary">Lưu Why Us</button>
      </div>
    </div>
  );
}

function AboutEditor({ data, onSaved }) {
  const initial = useMemo(()=>{
    let md = {};
    try { md = data.moreData ? JSON.parse(data.moreData) : {}; } catch {}
    return {
      title: md.title || "Giới thiệu",
      content: md.content || data.content || "",
      image: md.image || data.imageUrl || "",
    };
  }, [data.moreData, data.content, data.imageUrl]);
  const [form, setForm] = useState(initial);
  const [file, setFile] = useState(null);

  useEffect(() => {
    setForm(initial);
    setFile(null);
  }, [initial]);
  const save = async () => {
    await putAbout({ title: form.title, content: form.content, image: form.image }, file);
    onSaved && onSaved();
  };
  return (
    <div className="rounded-2xl border border-gray-200 dark:border-gray-800 bg-white dark:bg-gray-900 p-5 shadow-sm">
      <h3 className="font-semibold text-gray-900 dark:text-white mb-3">About</h3>
      <div className="grid gap-3">
        <input className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700" placeholder="Title" value={form.title} onChange={(e)=>setForm({...form,title:e.target.value})} />
        <textarea className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700" rows={4} placeholder="Content" value={form.content} onChange={(e)=>setForm({...form,content:e.target.value})} />
        <div className="flex items-center gap-2">
          <input className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 flex-1" placeholder="Image URL" value={form.image} onChange={(e)=>setForm({...form,image:e.target.value})} />
          <label className="btn-ghost">
            Tải ảnh
            <input type="file" className="hidden" onChange={(e)=>setFile(e.target.files?.[0]||null)} />
          </label>
        </div>
      </div>
      <div className="mt-4">
        <button onClick={save} className="btn-primary">Lưu About</button>
      </div>
    </div>
  );
}

function BannerManager({ items, onCreate, onUpdate, onDelete }) {
  const [newItem, setNewItem] = useState({
    title: "",
    description: "",
    imageUrl: "",
    linkUrl: "",
    displayOrder: items?.length || 0,
    status: "Active",
  });
  const [newFile, setNewFile] = useState(null);

  const pickNewImage = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setNewFile(file);
  };

  return (
    <div className="space-y-6">
      <div className="rounded-2xl border border-gray-200 dark:border-gray-800 bg-white dark:bg-gray-900 p-5 shadow-sm">
        <h3 className="font-semibold text-gray-900 dark:text-white mb-2">Thêm banner</h3>
        <div className="grid gap-3 md:grid-cols-2">
          <input
            className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100"
            placeholder="Title"
            value={newItem.title}
            onChange={(e) => setNewItem({ ...newItem, title: e.target.value })}
          />
          <input
            className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100"
            placeholder="Description"
            value={newItem.description}
            onChange={(e) => setNewItem({ ...newItem, description: e.target.value })}
          />
          <div className="flex items-center gap-2 md:col-span-2">
            <input
              className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100 flex-1"
              placeholder="Image URL"
              value={newItem.imageUrl}
              onChange={(e) => setNewItem({ ...newItem, imageUrl: e.target.value })}
            />
            <label className="inline-flex items-center px-3 py-2 rounded-lg bg-gray-100 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 cursor-pointer text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-200 dark:hover:bg-gray-700">
              Tải ảnh
              <input type="file" className="hidden" onChange={pickNewImage} />
            </label>
          </div>
          <input
            className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100"
            placeholder="Link URL"
            value={newItem.linkUrl}
            onChange={(e) => setNewItem({ ...newItem, linkUrl: e.target.value })}
          />
          <input
            type="number"
            className="rounded-lg px-3 py-2 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 text-gray-900 dark:text-gray-100"
            placeholder="Display Order"
            value={newItem.displayOrder}
            onChange={(e) => setNewItem({ ...newItem, displayOrder: Number(e.target.value) })}
          />
        </div>
        <div className="mt-4">
          <button
            onClick={() => onCreate(newItem, newFile)}
            className="inline-flex items-center px-4 py-2 rounded-lg bg-blue-600 hover:bg-blue-700 text-white shadow-sm"
          >
            Thêm banner
          </button>
        </div>
      </div>

      <div className="rounded-2xl border border-gray-200 dark:border-gray-800 bg-white dark:bg-gray-900 p-5 shadow-sm">
        <h3 className="font-semibold text-gray-900 dark:text-white mb-4">Danh sách banner</h3>
        <div className="space-y-4">
          {items?.map((b) => (
            <BannerRow key={b.bannerId} item={b} onUpdate={onUpdate} onDelete={onDelete} />
          ))}
          {(!items || items.length === 0) && (
            <p className="text-sm text-gray-500 dark:text-gray-400">Chưa có banner nào.</p>
          )}
        </div>
      </div>
    </div>
  );
}

function BannerRow({ item, onUpdate, onDelete }) {
  const [f, setF] = useState({ ...item });
  const [file, setFile] = useState(null);
  const pick = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setFile(file);
  };
  return (
    <div className="grid gap-3 rounded-xl border border-gray-200 dark:border-gray-800 p-4 bg-gray-50 dark:bg-gray-900/60">
      <div className="flex items-start justify-between gap-3">
        <div className="grid md:grid-cols-2 gap-2 flex-1">
          <input className="rounded-lg px-3 py-2 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700" value={f.title || ""} onChange={(e)=>setF({...f,title:e.target.value})} placeholder="Title" />
          <input className="rounded-lg px-3 py-2 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700" value={f.description || ""} onChange={(e)=>setF({...f,description:e.target.value})} placeholder="Description" />
          <div className="flex items-center gap-2 md:col-span-2">
            <input className="rounded-lg px-3 py-2 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700 flex-1" value={f.imageUrl || ""} onChange={(e)=>setF({...f,imageUrl:e.target.value})} placeholder="Image URL" />
            <label className="inline-flex items-center px-3 py-2 rounded-lg bg-gray-100 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 cursor-pointer text-sm text-gray-700 dark:text-gray-200 hover:bg-gray-200 dark:hover:bg-gray-700">
              Tải ảnh
              <input type="file" className="hidden" onChange={pick} />
            </label>
          </div>
          <input className="rounded-lg px-3 py-2 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700" value={f.linkUrl || ""} onChange={(e)=>setF({...f,linkUrl:e.target.value})} placeholder="Link URL" />
          <input type="number" className="rounded-lg px-3 py-2 bg-white dark:bg-gray-800 border border-gray-200 dark:border-gray-700" value={f.displayOrder || 0} onChange={(e)=>setF({...f,displayOrder:Number(e.target.value)})} placeholder="Display Order" />
        </div>
        {(file || f.imageUrl) ? (
          <img src={file ? URL.createObjectURL(file) : f.imageUrl} alt="preview" className="w-28 h-16 object-cover rounded-md border border-gray-200 dark:border-gray-700" />
        ) : (
          <div className="w-28 h-16 rounded-md border border-dashed border-gray-300 dark:border-gray-700 flex items-center justify-center text-xs text-gray-400 dark:text-gray-300">Preview</div>
        )}
      </div>
      <div className="flex gap-2">
        <button onClick={()=>onUpdate(f.bannerId, f, file)} className="inline-flex items-center px-4 py-2 rounded-lg bg-blue-600 hover:bg-blue-700 text-white">Lưu</button>
        <button onClick={()=>onDelete(f.bannerId)} className="inline-flex items-center px-4 py-2 rounded-lg bg-red-600 hover:bg-red-700 text-white">Xóa</button>
      </div>
    </div>
  );
}


