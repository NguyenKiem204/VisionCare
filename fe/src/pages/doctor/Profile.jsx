import React, { useEffect, useState } from "react";
import { getMyProfile, updateMyProfile, getMyCertificates, createMyCertificate, deleteMyCertificate, getMyDegrees, createMyDegree, deleteMyDegree } from "../../services/doctorProfileAPI";
import toast from "react-hot-toast";
import { User, Phone, MapPin, Calendar, Camera, Award, Plus, Trash2 } from "lucide-react";

const Profile = () => {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [certs, setCerts] = useState([]);
  const [degrees, setDegrees] = useState([]);
  const [activeTab, setActiveTab] = useState("certificates"); // "certificates" or "degrees"
  const [form, setForm] = useState({ doctorName: "", phone: "", experienceYears: 0, address: "", gender: "", dob: "" });
  const [newCert, setNewCert] = useState({ certificateId: "", issuedDate: "", issuedBy: "", expiryDate: "" });
  const [newDegree, setNewDegree] = useState({ degreeId: "", issuedDate: "", issuedBy: "" });
  const [certImageFile, setCertImageFile] = useState(null);
  const [degreeImageFile, setDegreeImageFile] = useState(null);
  const [avatarFile, setAvatarFile] = useState(null);
  const [avatarPreview, setAvatarPreview] = useState(null);

  const load = async () => {
    try {
      setLoading(true);
    const p = await getMyProfile();
    setProfile(p);
    setForm({
      doctorName: p?.doctorName || p?.fullName || "",
      phone: p?.phone || "",
      experienceYears: p?.experienceYears || 0,
      address: p?.address || "",
        gender: p?.gender || "",
        dob: p?.dob ? p.dob.split("T")[0] : "",
    });
      setAvatarPreview(p?.avatar || p?.profileImage || null);
    const c = await getMyCertificates();
    setCerts(c || []);
      const d = await getMyDegrees();
      setDegrees(d || []);
    } catch (error) {
      toast.error("Không thể tải thông tin profile");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const handleAvatarChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) {
        toast.error("File ảnh không được vượt quá 5MB");
        return;
      }
      setAvatarFile(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setAvatarPreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const onSave = async (e) => {
    e.preventDefault();
    try {
      setSaving(true);
    await updateMyProfile({
        doctorName: form.doctorName,
        phone: form.phone,
        experienceYears: Number(form.experienceYears) || 0,
        address: form.address,
        gender: form.gender,
        dob: form.dob,
      }, avatarFile);
      toast.success("Cập nhật profile thành công");
    await load();
      setAvatarFile(null);
    } catch (error) {
      toast.error(error?.response?.data?.message || "Không thể cập nhật profile");
    } finally {
      setSaving(false);
    }
  };

  const onAddCert = async (e) => {
    e.preventDefault();
    if (!newCert.certificateId) {
      toast.error("Vui lòng nhập mã chứng chỉ");
      return;
    }
    try {
    await createMyCertificate({
      certificateId: Number(newCert.certificateId),
      issuedDate: newCert.issuedDate || null,
      issuedBy: newCert.issuedBy || null,
      expiryDate: newCert.expiryDate || null,
      }, certImageFile);
      toast.success("Thêm chứng chỉ thành công");
    setNewCert({ certificateId: "", issuedDate: "", issuedBy: "", expiryDate: "" });
      setCertImageFile(null);
    await load();
    } catch (error) {
      toast.error(error?.response?.data?.message || "Không thể thêm chứng chỉ");
    }
  };

  const onDeleteCert = async (id) => {
    if (!window.confirm("Bạn có chắc chắn muốn xóa chứng chỉ này?")) return;
    try {
    await deleteMyCertificate(id);
      toast.success("Xóa chứng chỉ thành công");
      await load();
    } catch (error) {
      toast.error("Không thể xóa chứng chỉ");
    }
  };

  const onAddDegree = async (e) => {
    e.preventDefault();
    if (!newDegree.degreeId) {
      toast.error("Vui lòng nhập mã bằng cấp");
      return;
    }
    try {
      await createMyDegree({
        degreeId: Number(newDegree.degreeId),
        issuedDate: newDegree.issuedDate || null,
        issuedBy: newDegree.issuedBy || null,
      }, degreeImageFile);
      toast.success("Thêm bằng cấp thành công");
      setNewDegree({ degreeId: "", issuedDate: "", issuedBy: "" });
      setDegreeImageFile(null);
      await load();
    } catch (error) {
      toast.error(error?.response?.data?.message || "Không thể thêm bằng cấp");
    }
  };

  const onDeleteDegree = async (id) => {
    if (!window.confirm("Bạn có chắc chắn muốn xóa bằng cấp này?")) return;
    try {
      await deleteMyDegree(id);
      toast.success("Xóa bằng cấp thành công");
    await load();
    } catch (error) {
      toast.error("Không thể xóa bằng cấp");
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-green-50 via-white to-emerald-50 dark:from-gray-900 dark:via-gray-800 dark:to-gray-900 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-green-600 mx-auto mb-4"></div>
          <div className="text-lg text-gray-600 dark:text-gray-400">Đang tải...</div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-green-50 via-white to-emerald-50 dark:from-gray-900 dark:via-gray-800 dark:to-gray-900 py-8 px-4 sm:px-6 lg:px-8">
      <div className="max-w-4xl mx-auto space-y-6">
        {/* Header */}
      <div>
          <h1 className="text-4xl font-bold text-gray-900 dark:text-white mb-2">
            Hồ sơ bác sĩ
          </h1>
          <p className="text-gray-600 dark:text-gray-400">
            Quản lý thông tin cá nhân và chứng chỉ
          </p>
        </div>

        {/* Profile Card */}
        <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-xl overflow-hidden">
          {/* Avatar Section */}
          <div className="bg-gradient-to-r from-green-500 to-emerald-600 px-8 py-12">
            <div className="flex flex-col sm:flex-row items-center gap-6">
              <div className="relative group">
                <div className="absolute inset-0 bg-white rounded-full opacity-0 group-hover:opacity-20 transition-opacity"></div>
                <img
                  src={
                    avatarPreview ||
                    profile?.avatar ||
                    profile?.profileImage ||
                    `https://ui-avatars.com/api/?name=${encodeURIComponent(
                      form.doctorName || "D"
                    )}&background=ffffff&color=10b981&size=128&bold=true`
                  }
                  alt="Avatar"
                  className="w-32 h-32 rounded-full object-cover border-4 border-white shadow-2xl"
                />
                <label className="absolute bottom-0 right-0 bg-green-600 hover:bg-green-700 text-white p-3 rounded-full cursor-pointer shadow-lg transition-colors">
                  <Camera className="w-5 h-5" />
                  <input
                    type="file"
                    accept="image/*"
                    onChange={handleAvatarChange}
                    className="hidden"
                  />
                </label>
              </div>
              <div className="flex-1 text-center sm:text-left">
                <h2 className="text-3xl font-bold text-white mb-2">
                  {form.doctorName || "Bác sĩ"}
                </h2>
                <p className="text-green-100">Bác sĩ VisionCare</p>
              </div>
            </div>
      </div>

          {/* Form Section */}
          <form onSubmit={onSave} className="p-8 space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="md:col-span-2">
                <label className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-300 mb-2">
                  <User className="w-4 h-4 text-green-600" />
                  Họ tên <span className="text-red-500">*</span>
                </label>
                <input
                  required
                  className="w-full px-4 py-3 border-2 border-gray-200 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all dark:bg-gray-700 dark:text-white"
                  value={form.doctorName}
                  onChange={(e) => setForm({ ...form, doctorName: e.target.value })}
                  placeholder="Nhập họ tên"
                />
              </div>
              <div>
                <label className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-300 mb-2">
                  <Phone className="w-4 h-4 text-green-600" />
                  SĐT
                </label>
                <input className="w-full px-4 py-3 border-2 border-gray-200 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all dark:bg-gray-700 dark:text-white" value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} placeholder="Nhập số điện thoại" />
              </div>
          <div>
                <label className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-300 mb-2">
                  <Award className="w-4 h-4 text-green-600" />
                  Kinh nghiệm (năm)
                </label>
                <input type="number" className="w-full px-4 py-3 border-2 border-gray-200 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all dark:bg-gray-700 dark:text-white" value={form.experienceYears} onChange={(e) => setForm({ ...form, experienceYears: e.target.value })} placeholder="0" />
          </div>
          <div>
                <label className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-300 mb-2">
                  <User className="w-4 h-4 text-green-600" />
                  Giới tính
                </label>
                <select
                  className="w-full px-4 py-3 border-2 border-gray-200 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all dark:bg-gray-700 dark:text-white"
                  value={form.gender}
                  onChange={(e) => setForm({ ...form, gender: e.target.value })}
                >
                  <option value="">Chọn giới tính</option>
                  <option value="Male">Nam</option>
                  <option value="Female">Nữ</option>
                  <option value="Other">Khác</option>
                </select>
          </div>
          <div>
                <label className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-300 mb-2">
                  <Calendar className="w-4 h-4 text-green-600" />
                  Ngày sinh
                </label>
                <input type="date" className="w-full px-4 py-3 border-2 border-gray-200 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all dark:bg-gray-700 dark:text-white" value={form.dob} onChange={(e) => setForm({ ...form, dob: e.target.value })} />
          </div>
          <div>
                <label className="flex items-center gap-2 text-sm font-semibold text-gray-700 dark:text-gray-300 mb-2">
                  <MapPin className="w-4 h-4 text-green-600" />
                  Địa chỉ
                </label>
                <textarea
                  rows={3}
                  className="w-full px-4 py-3 border-2 border-gray-200 dark:border-gray-700 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all resize-none dark:bg-gray-700 dark:text-white"
                  value={form.address}
                  onChange={(e) => setForm({ ...form, address: e.target.value })}
                  placeholder="Nhập địa chỉ"
                />
          </div>
        </div>
            <div className="flex justify-end gap-4 pt-6 border-t border-gray-200 dark:border-gray-700">
              <button
                type="button"
                onClick={() => load()}
                disabled={saving}
                className="px-6 py-3 border-2 border-gray-300 dark:border-gray-600 rounded-lg text-gray-700 dark:text-gray-300 font-medium hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors disabled:opacity-50"
              >
                Hủy
              </button>
              <button type="submit" disabled={saving} className="px-6 py-3 bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 text-white rounded-lg font-semibold shadow-lg hover:shadow-xl transition-all disabled:opacity-50 disabled:cursor-not-allowed">
                {saving ? "Đang lưu..." : "Lưu thay đổi"}
              </button>
            </div>
          </form>
        </div>

        {/* Certificates & Degrees Card */}
        <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-xl p-8">
          <div className="flex items-center gap-3 mb-6">
            <Award className="w-6 h-6 text-green-600" />
            <h3 className="text-2xl font-bold text-gray-900 dark:text-white">Chứng chỉ & Bằng cấp</h3>
          </div>
          
          {/* Tabs */}
          <div className="flex gap-2 mb-6 border-b border-gray-200 dark:border-gray-700">
            <button
              onClick={() => setActiveTab("certificates")}
              className={`px-6 py-3 font-semibold transition-colors ${
                activeTab === "certificates"
                  ? "text-green-600 border-b-2 border-green-600"
                  : "text-gray-500 dark:text-gray-400 hover:text-gray-700 dark:hover:text-gray-300"
              }`}
            >
              Chứng chỉ
            </button>
            <button
              onClick={() => setActiveTab("degrees")}
              className={`px-6 py-3 font-semibold transition-colors ${
                activeTab === "degrees"
                  ? "text-green-600 border-b-2 border-green-600"
                  : "text-gray-500 dark:text-gray-400 hover:text-gray-700 dark:hover:text-gray-300"
              }`}
            >
              Bằng cấp
            </button>
          </div>

          {/* Certificates Tab */}
          {activeTab === "certificates" && (
            <>
          <form onSubmit={onAddCert} className="space-y-4 mb-6 p-4 bg-gray-50 dark:bg-gray-700/50 rounded-lg">
            <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
              <input className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" placeholder="Mã chứng chỉ *" value={newCert.certificateId} onChange={(e) => setNewCert({ ...newCert, certificateId: e.target.value })} required />
              <input type="date" className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" value={newCert.issuedDate} onChange={(e) => setNewCert({ ...newCert, issuedDate: e.target.value })} />
              <input className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" placeholder="Cơ quan cấp" value={newCert.issuedBy} onChange={(e) => setNewCert({ ...newCert, issuedBy: e.target.value })} />
              <input type="date" className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" value={newCert.expiryDate} onChange={(e) => setNewCert({ ...newCert, expiryDate: e.target.value })} />
            </div>
            <div className="flex items-center gap-4">
              <label className="flex-1 cursor-pointer">
                <input
                  type="file"
                  accept="image/*"
                  onChange={(e) => {
                    const file = e.target.files[0];
                    if (file) {
                      if (file.size > 10 * 1024 * 1024) {
                        toast.error("File ảnh không được vượt quá 10MB");
                        return;
                      }
                      setCertImageFile(file);
                    }
                  }}
                  className="hidden"
                />
                <div className="px-4 py-3 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-green-500 transition-colors text-center text-sm text-gray-600 dark:text-gray-400">
                  {certImageFile ? certImageFile.name : "Chọn ảnh chứng chỉ (tùy chọn)"}
                </div>
              </label>
              <button type="submit" className="px-6 py-3 bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 text-white rounded-lg font-semibold shadow-lg hover:shadow-xl transition-all flex items-center justify-center gap-2">
                <Plus className="w-5 h-5" />
                Thêm
              </button>
        </div>
      </form>

          <div className="space-y-3">
            {(certs || []).map((c) => (
              <div key={c.id} className="flex items-center gap-4 p-4 bg-gray-50 dark:bg-gray-700/50 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors">
                {c.certificateImage && (
                  <img src={c.certificateImage} alt="Certificate" className="w-20 h-20 object-cover rounded-lg border-2 border-gray-200 dark:border-gray-600" />
                )}
                <div className="flex-1">
                  <div className="text-sm font-semibold text-gray-900 dark:text-white">#{c.certificateId} · {c.certificateName || ""}</div>
                  <div className="text-sm text-gray-600 dark:text-gray-400">{c.issuedBy || ""}</div>
                  <div className="text-sm text-gray-500 dark:text-gray-400">{c.issuedDate ? new Date(c.issuedDate).toLocaleDateString("vi-VN") : ""} → {c.expiryDate ? new Date(c.expiryDate).toLocaleDateString("vi-VN") : "Không hết hạn"}</div>
                </div>
                <button onClick={() => onDeleteCert(c.certificateId)} className="p-2 text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-colors">
                  <Trash2 className="w-5 h-5" />
                </button>
              </div>
            ))}
            {(!certs || certs.length === 0) && (
              <div className="text-center py-8 text-gray-500 dark:text-gray-400">
                Chưa có chứng chỉ nào
              </div>
            )}
          </div>
            </>
          )}

          {/* Degrees Tab */}
          {activeTab === "degrees" && (
            <>
          <form onSubmit={onAddDegree} className="space-y-4 mb-6 p-4 bg-gray-50 dark:bg-gray-700/50 rounded-lg">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <input className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" placeholder="Mã bằng cấp *" value={newDegree.degreeId} onChange={(e) => setNewDegree({ ...newDegree, degreeId: e.target.value })} required />
              <input type="date" className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" value={newDegree.issuedDate} onChange={(e) => setNewDegree({ ...newDegree, issuedDate: e.target.value })} />
              <input className="px-4 py-3 border-2 border-gray-200 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent dark:bg-gray-700 dark:text-white" placeholder="Cơ quan cấp" value={newDegree.issuedBy} onChange={(e) => setNewDegree({ ...newDegree, issuedBy: e.target.value })} />
            </div>
            <div className="flex items-center gap-4">
              <label className="flex-1 cursor-pointer">
                <input
                  type="file"
                  accept="image/*"
                  onChange={(e) => {
                    const file = e.target.files[0];
                    if (file) {
                      if (file.size > 10 * 1024 * 1024) {
                        toast.error("File ảnh không được vượt quá 10MB");
                        return;
                      }
                      setDegreeImageFile(file);
                    }
                  }}
                  className="hidden"
                />
                <div className="px-4 py-3 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-green-500 transition-colors text-center text-sm text-gray-600 dark:text-gray-400">
                  {degreeImageFile ? degreeImageFile.name : "Chọn ảnh bằng cấp (tùy chọn)"}
                </div>
              </label>
              <button type="submit" className="px-6 py-3 bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 text-white rounded-lg font-semibold shadow-lg hover:shadow-xl transition-all flex items-center justify-center gap-2">
                <Plus className="w-5 h-5" />
                Thêm
              </button>
            </div>
        </form>

          <div className="space-y-3">
            {(degrees || []).map((d) => (
              <div key={d.id} className="flex items-center gap-4 p-4 bg-gray-50 dark:bg-gray-700/50 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors">
                {d.certificateImage && (
                  <img src={d.certificateImage} alt="Degree" className="w-20 h-20 object-cover rounded-lg border-2 border-gray-200 dark:border-gray-600" />
                )}
                <div className="flex-1">
                  <div className="text-sm font-semibold text-gray-900 dark:text-white">#{d.degreeId} · {d.degreeName || ""}</div>
                  <div className="text-sm text-gray-600 dark:text-gray-400">{d.issuedBy || ""}</div>
                  <div className="text-sm text-gray-500 dark:text-gray-400">{d.issuedDate ? new Date(d.issuedDate).toLocaleDateString("vi-VN") : ""}</div>
                </div>
                <button onClick={() => onDeleteDegree(d.degreeId)} className="p-2 text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition-colors">
                  <Trash2 className="w-5 h-5" />
                </button>
              </div>
            ))}
            {(!degrees || degrees.length === 0) && (
              <div className="text-center py-8 text-gray-500 dark:text-gray-400">
                Chưa có bằng cấp nào
              </div>
            )}
          </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;
