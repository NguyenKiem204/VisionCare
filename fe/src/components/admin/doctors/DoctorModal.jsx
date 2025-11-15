import React, { useState, useEffect } from "react";
import DoctorQualifications from "./DoctorQualifications";

const DoctorModal = ({ open, doctor, onClose, onSave }) => {
  const [form, setForm] = useState({
    doctorName: "",
    email: "",
    phone: "",
    gender: "",
    dob: "",
    address: "",
    specializationId: "",
    specializationName: "",
    experienceYears: "",
    rating: "",
    doctorStatus: "",
    biography: "",
  });
  const [activeTab, setActiveTab] = useState("basic");
  const [avatarFile, setAvatarFile] = useState(null);
  const [avatarPreview, setAvatarPreview] = useState(null);

  useEffect(() => {
    if (doctor) {
      setForm({
        doctorName: doctor.doctorName || doctor.fullName || "",
        email: doctor.email || "",
        phone: doctor.phone || "",
        gender: doctor.gender || "",
        dob: doctor.dob
          ? new Date(doctor.dob).toISOString().split("T")[0]
          : doctor.dateOfBirth
          ? new Date(doctor.dateOfBirth).toISOString().split("T")[0]
          : "",
        address: doctor.address || "",
        specializationId: doctor.specializationId || "",
        specializationName: doctor.specializationName || "",
        experienceYears: doctor.experienceYears || doctor.experience || "",
        rating: doctor.rating || "",
        doctorStatus: doctor.doctorStatus || doctor.status || "",
        biography: doctor.biography || "",
      });
      setAvatarPreview(doctor.avatar || doctor.profileImage || null);
    } else {
      setForm({
        doctorName: "",
        email: "",
        phone: "",
        gender: "",
        dob: "",
        address: "",
        specializationId: "",
        specializationName: "",
        experienceYears: "",
        rating: "",
        doctorStatus: "",
        biography: "",
      });
      setAvatarPreview(null);
    }
    setAvatarFile(null);
  }, [doctor]);

  const handleAvatarChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (file.size > 5 * 1024 * 1024) {
        alert("File ảnh không được vượt quá 5MB");
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

  if (!open) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div className="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-[800px] max-h-[90vh] overflow-y-auto">
        <h2 className="text-lg font-bold mb-4 text-gray-900 dark:text-white">
          {doctor ? "Sửa thông tin bác sĩ" : "Tạo bác sĩ mới"}
        </h2>

        {/* Tabs */}
        <div className="flex border-b border-gray-200 dark:border-gray-700 mb-6">
          <button
            onClick={() => setActiveTab("basic")}
            className={`px-4 py-2 text-sm font-medium ${
              activeTab === "basic"
                ? "text-blue-600 border-b-2 border-blue-600"
                : "text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300"
            }`}
          >
            Thông tin cơ bản
          </button>
          {doctor && (
            <button
              onClick={() => setActiveTab("qualifications")}
              className={`px-4 py-2 text-sm font-medium ${
                activeTab === "qualifications"
                  ? "text-blue-600 border-b-2 border-blue-600"
                  : "text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300"
              }`}
            >
              Bằng cấp & Chứng chỉ
            </button>
          )}
        </div>

        {activeTab === "basic" && (
          <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Ảnh đại diện
            </label>
            <div className="flex items-center gap-4">
              <div className="relative">
                <img
                  src={
                    avatarPreview ||
                    doctor?.avatar ||
                    doctor?.profileImage ||
                    `https://ui-avatars.com/api/?name=${encodeURIComponent(
                      form.doctorName || "D"
                    )}&background=ffffff&color=10b981&size=128&bold=true`
                  }
                  alt="Avatar"
                  className="w-24 h-24 rounded-full object-cover border-2 border-gray-300 dark:border-gray-600"
                />
              </div>
              <label className="cursor-pointer">
                <div className="px-4 py-2 border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-lg hover:border-blue-500 transition-colors text-center text-sm text-gray-600 dark:text-gray-400">
                  {avatarFile ? avatarFile.name : "Chọn ảnh đại diện"}
                </div>
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleAvatarChange}
                  className="hidden"
                />
              </label>
            </div>
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">
              Kích thước tối đa: 5MB
            </p>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Tên đầy đủ *
              </label>
              <input
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.doctorName}
                onChange={(e) =>
                  setForm((f) => ({ ...f, doctorName: e.target.value }))
                }
                placeholder="Nhập tên đầy đủ"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Email *
              </label>
              <input
                type="email"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.email}
                onChange={(e) =>
                  setForm((f) => ({ ...f, email: e.target.value }))
                }
                placeholder="Nhập email"
                required
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Số điện thoại
              </label>
              <input
                type="tel"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.phone}
                onChange={(e) =>
                  setForm((f) => ({ ...f, phone: e.target.value }))
                }
                placeholder="Nhập số điện thoại"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Giới tính
              </label>
              <select
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.gender}
                onChange={(e) =>
                  setForm((f) => ({ ...f, gender: e.target.value }))
                }
                required={!doctor} // Chỉ bắt buộc khi tạo mới
              >
                <option value="">Chọn giới tính</option>
                <option value="Male">Nam</option>
                <option value="Female">Nữ</option>
                <option value="Other">Khác</option>
              </select>
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Ngày sinh
            </label>
            <input
              type="date"
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.dob}
              onChange={(e) => setForm((f) => ({ ...f, dob: e.target.value }))}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Địa chỉ
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.address}
              onChange={(e) =>
                setForm((f) => ({ ...f, address: e.target.value }))
              }
              placeholder="Nhập địa chỉ"
              rows={2}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Chuyên khoa *
              </label>
              <input
                type="text"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.specializationName || ""}
                onChange={(e) =>
                  setForm((f) => ({ ...f, specializationName: e.target.value }))
                }
                placeholder="Nhập tên chuyên khoa"
                required
              />
              <input
                type="hidden"
                value={form.specializationId}
                onChange={(e) =>
                  setForm((f) => ({ ...f, specializationId: e.target.value }))
                }
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Trạng thái
              </label>
              <select
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.doctorStatus}
                onChange={(e) =>
                  setForm((f) => ({ ...f, doctorStatus: e.target.value }))
                }
              >
                <option value="">Chọn trạng thái</option>
                <option value="Active">Hoạt động</option>
                <option value="Inactive">Không hoạt động</option>
              </select>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Kinh nghiệm (năm)
              </label>
              <input
                type="number"
                min="0"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.experienceYears}
                onChange={(e) =>
                  setForm((f) => ({ ...f, experienceYears: e.target.value }))
                }
                placeholder="Nhập số năm kinh nghiệm"
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Đánh giá (0-5)
              </label>
              <input
                type="number"
                min="0"
                max="5"
                step="0.1"
                className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
                value={form.rating}
                onChange={(e) =>
                  setForm((f) => ({ ...f, rating: e.target.value }))
                }
                placeholder="Nhập đánh giá"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Giới thiệu bác sĩ
            </label>
            <textarea
              className="border border-gray-300 dark:border-gray-600 px-3 py-2 rounded-md w-full bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={form.biography}
              onChange={(e) =>
                setForm((f) => ({ ...f, biography: e.target.value }))
              }
              placeholder="Nhập giới thiệu về bác sĩ (bằng khen, chức vụ, kinh nghiệm...)"
              rows={6}
            />
            <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">
              Ví dụ: Bằng khen của Bộ trưởng Bộ Y Tế về đóng góp cho ngành Nhãn khoa. Trưởng khoa Khám và Điều trị theo yêu cầu - Bệnh viện Mắt Trung Ương. Hơn 30 năm kinh nghiệm...
            </p>
          </div>
        </div>
        )}

        {activeTab === "qualifications" && doctor && (
          <DoctorQualifications doctorId={doctor.id} doctorName={doctor.doctorName || doctor.fullName} />
        )}

        <div className="flex justify-end gap-2 mt-6">
          <button
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium transition"
            onClick={() => onSave(form, avatarFile)}
          >
            Lưu
          </button>
          <button
            className="bg-gray-300 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-gray-700 dark:text-gray-200 px-4 py-2 rounded-md font-medium transition"
            onClick={onClose}
          >
            Đóng
          </button>
        </div>
      </div>
    </div>
  );
};

export default DoctorModal;
