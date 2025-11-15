import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getDoctorById } from "../services/adminDoctorAPI";
import { getCertificatesByDoctor, getDegreesByDoctor } from "../services/adminDoctorCertificateAPI";
import { CheckCircle2, Star, MapPin, Phone, Mail, Calendar, Award, ArrowLeft, GraduationCap, FileText, X, ChevronRight } from "lucide-react";
import toast from "react-hot-toast";

const parseBiography = (biography) => {
  if (!biography) return [];
  
  let items = [];
  
  if (biography.includes('\n')) {
    items = biography.split('\n');
  } else {
    items = biography.split(/\.(?=\s+[A-ZĐ])/);
  }
  
  return items
    .map(item => item.trim().replace(/^\.\s*/, ''))
    .filter(item => item.length > 10)
    .map(item => {
      if (!item.endsWith('.') && !item.endsWith('!') && !item.endsWith('?')) {
        return item + '.';
      }
      return item;
    });
};

const DoctorDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [doctor, setDoctor] = useState(null);
  const [certificates, setCertificates] = useState([]);
  const [degrees, setDegrees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState(null);

  useEffect(() => {
    const loadDoctor = async () => {
      try {
        setLoading(true);
        const response = await getDoctorById(id);
        const doctorData = response?.data || response;
        setDoctor(doctorData);

        const doctorId = doctorData.id || doctorData.accountId || id;
        if (doctorId) {
          try {
            const certsResponse = await getCertificatesByDoctor(doctorId);
            const certsData = certsResponse?.data || certsResponse?.data?.data || certsResponse || [];
            setCertificates(Array.isArray(certsData) ? certsData : []);

            const degreesResponse = await getDegreesByDoctor(doctorId);
            const degreesData = degreesResponse?.data || degreesResponse?.data?.data || degreesResponse || [];
            setDegrees(Array.isArray(degreesData) ? degreesData : []);
          } catch (err) {
            console.error("Error loading certificates/degrees:", err);
          }
        }
      } catch (error) {
        console.error("Error loading doctor:", error);
        toast.error("Không thể tải thông tin bác sĩ");
        navigate("/");
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      loadDoctor();
    }
  }, [id, navigate]);

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-[#0c5a8a] mx-auto"></div>
          <p className="mt-4 text-gray-600">Đang tải thông tin bác sĩ...</p>
        </div>
      </div>
    );
  }

  if (!doctor) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <p className="text-gray-600 mb-4">Không tìm thấy thông tin bác sĩ</p>
          <button
            onClick={() => navigate("/")}
            className="px-6 py-2 bg-[#0c5a8a] text-white rounded-lg hover:bg-[#094a75] transition-colors"
          >
            Về trang chủ
          </button>
        </div>
      </div>
    );
  }

  const achievements = parseBiography(doctor.biography);
  const imageUrl = doctor.avatar || doctor.profileImage || "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80";
  const rating = doctor.rating || 0;
  const experienceYears = doctor.experienceYears || 0;

  const calculateAge = (dob) => {
    if (!dob) return null;
    const birthDate = new Date(dob);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  };

  const age = doctor.dob ? calculateAge(doctor.dob) : null;

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Hero Header with Background Image */}
      <div className="relative h-72 bg-gradient-to-r from-gray-700 to-gray-600 overflow-hidden">
        <div 
          className="absolute inset-0 bg-cover bg-center opacity-40"
          style={{
            backgroundImage: "url('https://images.unsplash.com/photo-1631217868264-e5b90bb7e133?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80')"
          }}
        ></div>
        <div className="absolute inset-0 bg-gradient-to-b from-transparent to-gray-900/50"></div>
        
        {/* Back button */}
        <div className="absolute top-6 left-6 z-10">
          <button
            onClick={() => navigate(-1)}
            className="flex items-center text-white hover:text-gray-200 transition-colors bg-black/30 px-4 py-2 rounded-lg backdrop-blur-sm"
          >
            <ArrowLeft className="w-5 h-5 mr-2" />
            Quay lại
          </button>
        </div>

        {/* Title and Breadcrumb */}
        <div className="relative h-full flex flex-col items-center justify-center text-white px-4">
          <h1 className="text-4xl md:text-5xl font-bold mb-4">
            {doctor.doctorName || doctor.fullName}
          </h1>
          
          {/* Breadcrumb */}
          <div className="flex items-center gap-2 text-sm">
            <button onClick={() => navigate("/")} className="hover:text-gray-200">
              Trang Chủ
            </button>
            <ChevronRight className="w-4 h-4" />
            <button onClick={() => navigate("/doctors")} className="hover:text-gray-200">
              Bác Sĩ
            </button>
            <ChevronRight className="w-4 h-4" />
            <span className="text-yellow-400">{doctor.doctorName || doctor.fullName}</span>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="container mx-auto px-4 -mt-16 relative z-10 pb-12">
        <div className="max-w-7xl mx-auto">
          {/* Doctor Info Card */}
          <div className="bg-white rounded-lg shadow-lg overflow-hidden mb-8">
            <div className="p-8">
              <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                {/* Left Column - Doctor Image */}
                <div className="lg:col-span-1">
                  <div className="bg-gray-100 rounded-lg overflow-hidden shadow-md">
                    <img
                      src={imageUrl}
                      alt={doctor.doctorName || doctor.fullName}
                      className="w-full h-auto object-cover"
                    />
                  </div>
                </div>

                {/* Right Column - Doctor Info */}
                <div className="lg:col-span-2">
                  <div className="mb-6">
                    <h2 className="text-3xl font-bold text-[#0c5a8a] mb-2">
                      {doctor.doctorName || doctor.fullName}
                    </h2>
                    <p className="text-lg text-gray-600 mb-4">
                      {doctor.specializationName || "Cố Vấn Chuyên Môn"}
                    </p>
                    
                    {/* Rating */}
                    {rating > 0 && (
                      <div className="flex items-center gap-2 mb-6">
                        <div className="flex items-center">
                          {[...Array(5)].map((_, i) => (
                            <Star
                              key={i}
                              className={`w-5 h-5 ${
                                i < Math.floor(rating)
                                  ? "fill-yellow-400 text-yellow-400"
                                  : "fill-gray-300 text-gray-300"
                              }`}
                            />
                          ))}
                        </div>
                        <span className="text-gray-700 font-medium">
                          {rating.toFixed(1)}
                        </span>
                      </div>
                    )}
                  </div>

                  {/* Info Grid */}
                  <div className="space-y-4">
                    {doctor.phone && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Điện thoại:</span>
                        <span className="text-gray-900">{doctor.phone}</span>
                      </div>
                    )}
                    
                    {doctor.email && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Email:</span>
                        <span className="text-gray-900">{doctor.email}</span>
                      </div>
                    )}

                    {doctor.specializationName && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Chuyên khoa:</span>
                        <span className="text-gray-900">{doctor.specializationName}</span>
                      </div>
                    )}

                    {experienceYears > 0 && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Kinh nghiệm:</span>
                        <span className="text-gray-900">{experienceYears} năm</span>
                      </div>
                    )}

                    {degrees.length > 0 && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Bằng cấp:</span>
                        <span className="text-gray-900">
                          {degrees.map(d => d.degreeName).join(", ")}
                        </span>
                      </div>
                    )}

                    {achievements.length > 0 && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Lĩnh vực chuyên môn:</span>
                        <span className="text-gray-900">{achievements[0]}</span>
                      </div>
                    )}

                    {doctor.address && (
                      <div className="flex items-start">
                        <span className="font-semibold text-gray-700 w-40">Tốt nghiệp:</span>
                        <span className="text-gray-900">{doctor.address}</span>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Description Section */}
          <div className="bg-white rounded-lg shadow-lg p-8 mb-8">
            <h3 className="text-2xl font-bold text-[#0c5a8a] mb-6">
              Chuyên gia hàng đầu về dịch kênh - vòng mạc | Hơn {experienceYears} năm kinh nghiệm | Gần 100.000 ca phẫu thuật thành công
            </h3>
            
            <div className="prose max-w-none text-gray-700">
              <p className="mb-4 leading-relaxed">
                {doctor.doctorName || doctor.fullName} là một trong những bác sĩ nhãn khoa hàng đầu tại Việt Nam, đặc biệt có chuyên môn sâu trong lĩnh vực dịch kênh - vòng mạc, với hơn {experienceYears} năm kinh nghiệm trong khám, điều trị và phẫu thuật các bệnh lý phức tạp của mắt.
              </p>

              {achievements.length > 0 && (
                <div className="my-6">
                  <ul className="space-y-3">
                    {achievements.map((achievement, idx) => (
                      <li key={idx} className="flex items-start">
                        <CheckCircle2 className="text-[#0c5a8a] mr-3 flex-shrink-0 mt-1 w-5 h-5" />
                        <span className="leading-relaxed">{achievement}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>
          </div>

          {/* Certificates & Degrees Section */}
          {(certificates.length > 0 || degrees.length > 0) && (
            <div className="bg-white rounded-lg shadow-lg p-8 mb-8">
              <h3 className="text-2xl font-bold text-[#0c5a8a] mb-6">
                Chứng chỉ & Bằng cấp
              </h3>
              
              {/* Degrees */}
              {degrees.length > 0 && (
                <div className="mb-8">
                  <h4 className="text-xl font-semibold text-gray-800 mb-4 flex items-center gap-2">
                    <GraduationCap className="w-6 h-6 text-[#0c5a8a]" />
                    Bằng cấp
                  </h4>
                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {degrees.map((degree) => (
                      <div
                        key={degree.id}
                        className="border border-gray-200 rounded-lg p-5 hover:shadow-md transition-all"
                      >
                        {degree.certificateImage && (
                          <div 
                            className="mb-4 rounded-lg overflow-hidden cursor-pointer group relative bg-gray-50"
                            onClick={() => setSelectedImage(degree.certificateImage)}
                          >
                            <img
                              src={degree.certificateImage}
                              alt={degree.degreeName || "Bằng cấp"}
                              className="w-full h-48 object-contain"
                              onError={(e) => {
                                e.target.style.display = "none";
                              }}
                            />
                            <div className="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-20 transition-all flex items-center justify-center">
                              <span className="text-white opacity-0 group-hover:opacity-100 text-sm font-semibold">Xem chi tiết</span>
                            </div>
                          </div>
                        )}
                        <h5 className="font-semibold text-gray-900 mb-2">
                          {degree.degreeName || "Bằng cấp"}
                        </h5>
                        {degree.issuedBy && (
                          <p className="text-sm text-gray-600 mb-1">
                            <span className="font-medium">Cấp bởi:</span> {degree.issuedBy}
                          </p>
                        )}
                        {degree.issuedDate && (
                          <p className="text-sm text-gray-600">
                            <span className="font-medium">Ngày cấp:</span>{" "}
                            {new Date(degree.issuedDate).toLocaleDateString("vi-VN")}
                          </p>
                        )}
                      </div>
                    ))}
                  </div>
                </div>
              )}

              {/* Certificates */}
              {certificates.length > 0 && (
                <div>
                  <h4 className="text-xl font-semibold text-gray-800 mb-4 flex items-center gap-2">
                    <FileText className="w-6 h-6 text-[#0c5a8a]" />
                    Chứng chỉ
                  </h4>
                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {certificates.map((cert) => (
                      <div
                        key={cert.id}
                        className="border border-gray-200 rounded-lg p-5 hover:shadow-md transition-all"
                      >
                        {cert.certificateImage && (
                          <div 
                            className="mb-4 rounded-lg overflow-hidden cursor-pointer group relative bg-gray-50"
                            onClick={() => setSelectedImage(cert.certificateImage)}
                          >
                            <img
                              src={cert.certificateImage}
                              alt={cert.certificateName || "Chứng chỉ"}
                              className="w-full h-48 object-contain"
                              onError={(e) => {
                                e.target.style.display = "none";
                              }}
                            />
                            <div className="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-20 transition-all flex items-center justify-center">
                              <span className="text-white opacity-0 group-hover:opacity-100 text-sm font-semibold">Xem chi tiết</span>
                            </div>
                          </div>
                        )}
                        <h5 className="font-semibold text-gray-900 mb-2">
                          {cert.certificateName || "Chứng chỉ"}
                        </h5>
                        {cert.issuedBy && (
                          <p className="text-sm text-gray-600 mb-1">
                            <span className="font-medium">Cấp bởi:</span> {cert.issuedBy}
                          </p>
                        )}
                        {cert.issuedDate && (
                          <p className="text-sm text-gray-600 mb-1">
                            <span className="font-medium">Ngày cấp:</span>{" "}
                            {new Date(cert.issuedDate).toLocaleDateString("vi-VN")}
                          </p>
                        )}
                        {cert.expiryDate && (
                          <p className="text-sm text-gray-600">
                            <span className="font-medium">Hết hạn:</span>{" "}
                            {new Date(cert.expiryDate).toLocaleDateString("vi-VN")}
                          </p>
                        )}
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>
          )}

          {/* Action Button */}
          <div className="bg-white rounded-lg shadow-lg p-8 text-center">
            <button
              onClick={() => navigate(`/booking?doctorId=${doctor.id || doctor.accountId}`)}
              className="px-8 py-4 bg-[#0c5a8a] hover:bg-[#094a75] text-white font-semibold rounded-lg transition-all duration-300 shadow-md hover:shadow-lg text-lg"
            >
              Đặt lịch khám với bác sĩ
            </button>
          </div>
        </div>
      </div>

      {/* Image Modal */}
      {selectedImage && (
        <div
          className="fixed inset-0 bg-black bg-opacity-75 z-50 flex items-center justify-center p-4"
          onClick={() => setSelectedImage(null)}
        >
          <div className="relative max-w-4xl max-h-[90vh] w-full">
            <button
              onClick={() => setSelectedImage(null)}
              className="absolute -top-10 right-0 text-white hover:text-gray-300 transition-colors"
            >
              <X className="w-8 h-8" />
            </button>
            <img
              src={selectedImage}
              alt="Xem chi tiết"
              className="w-full h-full object-contain rounded-lg"
              onClick={(e) => e.stopPropagation()}
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default DoctorDetail;