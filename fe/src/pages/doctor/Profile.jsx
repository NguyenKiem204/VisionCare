import React, { useEffect, useState } from "react";
import { getMyProfile, updateMyProfile, getMyCertificates, createMyCertificate, deleteMyCertificate } from "../../services/doctorProfileAPI";

const Profile = () => {
  const [profile, setProfile] = useState(null);
  const [certs, setCerts] = useState([]);
  const [form, setForm] = useState({ doctorName: "", phone: "", experienceYears: 0, address: "" });
  const [newCert, setNewCert] = useState({ certificateId: "", issuedDate: "", issuedBy: "", expiryDate: "" });

  const load = async () => {
    const p = await getMyProfile();
    setProfile(p);
    setForm({
      doctorName: p?.doctorName || p?.fullName || "",
      phone: p?.phone || "",
      experienceYears: p?.experienceYears || 0,
      address: p?.address || "",
    });
    const c = await getMyCertificates();
    setCerts(c || []);
  };

  useEffect(() => { load(); }, []);

  const onSave = async (e) => {
    e.preventDefault();
    await updateMyProfile({
      DoctorName: form.doctorName,
      Phone: form.phone,
      ExperienceYears: Number(form.experienceYears) || 0,
      Address: form.address,
    });
    await load();
  };

  const onAddCert = async (e) => {
    e.preventDefault();
    if (!newCert.certificateId) return;
    await createMyCertificate({
      certificateId: Number(newCert.certificateId),
      issuedDate: newCert.issuedDate || null,
      issuedBy: newCert.issuedBy || null,
      expiryDate: newCert.expiryDate || null,
    });
    setNewCert({ certificateId: "", issuedDate: "", issuedBy: "", expiryDate: "" });
    await load();
  };

  const onDeleteCert = async (id) => {
    await deleteMyCertificate(id);
    await load();
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Hồ sơ bác sĩ</h1>
        <p className="text-gray-600">Cập nhật thông tin cá nhân và chứng chỉ</p>
      </div>

      <form onSubmit={onSave} className="bg-white shadow rounded-lg p-6 space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Họ tên</label>
            <input className="mt-1 block w-full border rounded-md p-2" value={form.doctorName} onChange={(e) => setForm({ ...form, doctorName: e.target.value })} />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">SĐT</label>
            <input className="mt-1 block w-full border rounded-md p-2" value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Kinh nghiệm (năm)</label>
            <input type="number" className="mt-1 block w-full border rounded-md p-2" value={form.experienceYears} onChange={(e) => setForm({ ...form, experienceYears: e.target.value })} />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Địa chỉ</label>
            <input className="mt-1 block w-full border rounded-md p-2" value={form.address} onChange={(e) => setForm({ ...form, address: e.target.value })} />
          </div>
        </div>
        <div>
          <button className="px-4 py-2 bg-indigo-600 text-white rounded-md">Lưu</button>
        </div>
      </form>

      <div className="bg-white shadow rounded-lg p-6 space-y-4">
        <h3 className="text-lg font-medium text-gray-900">Chứng chỉ</h3>
        <form onSubmit={onAddCert} className="grid grid-cols-1 md:grid-cols-5 gap-4">
          <input className="border rounded-md p-2" placeholder="CertificateId" value={newCert.certificateId} onChange={(e) => setNewCert({ ...newCert, certificateId: e.target.value })} />
          <input type="date" className="border rounded-md p-2" value={newCert.issuedDate} onChange={(e) => setNewCert({ ...newCert, issuedDate: e.target.value })} />
          <input className="border rounded-md p-2" placeholder="Issued by" value={newCert.issuedBy} onChange={(e) => setNewCert({ ...newCert, issuedBy: e.target.value })} />
          <input type="date" className="border rounded-md p-2" value={newCert.expiryDate} onChange={(e) => setNewCert({ ...newCert, expiryDate: e.target.value })} />
          <button className="px-4 py-2 bg-green-600 text-white rounded-md">Thêm</button>
        </form>

        <ul className="divide-y divide-gray-200">
          {(certs || []).map((c) => (
            <li key={c.id} className="py-3 flex items-center justify-between">
              <div>
                <div className="text-sm font-medium text-gray-900">#{c.certificateId} · {c.issuedBy || ""}</div>
                <div className="text-sm text-gray-500">{c.issuedDate} → {c.expiryDate || ""}</div>
              </div>
              <button onClick={() => onDeleteCert(c.certificateId)} className="px-2 py-1 text-xs bg-red-50 text-red-700 rounded">Xóa</button>
            </li>
          ))}
          {(!certs || certs.length === 0) && <li className="py-3 text-sm text-gray-500">Chưa có chứng chỉ</li>}
        </ul>
      </div>
    </div>
  );
};

export default Profile;


