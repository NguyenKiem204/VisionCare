import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { ChevronLeft, ChevronRight, CheckCircle2 } from "lucide-react";
import { getTopRatedDoctors } from "../../services/adminDoctorAPI";

// Helper function để parse biography thành array
const parseBiography = (biography) => {
  if (!biography) return [];
  
  // Split by dấu chấm (.), xuống dòng (\n), hoặc dấu chấm phẩy (;)
  // Nhưng giữ lại dấu chấm nếu là số thập phân hoặc viết tắt
  let items = [];
  
  // Thử split bằng xuống dòng trước (nếu format đã có sẵn)
  if (biography.includes('\n')) {
    items = biography.split('\n');
  } else {
    // Split bằng dấu chấm, nhưng cẩn thận với số thập phân
    items = biography.split(/\.(?=\s+[A-ZĐ])/); // Split by dot followed by space and capital letter
  }
  
  return items
    .map(item => item.trim().replace(/^\.\s*/, '')) // Remove leading dot
    .filter(item => item.length > 10) // Filter out very short items (likely false splits)
    .map(item => {
      // Nếu item không kết thúc bằng dấu chấm, thêm lại
      if (!item.endsWith('.') && !item.endsWith('!') && !item.endsWith('?')) {
        return item + '.';
      }
      return item;
    });
};

const DoctorsCarousel = () => {
  const navigate = useNavigate();
  const [currentSlide, setCurrentSlide] = useState(0);
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadDoctors = async () => {
      try {
        setLoading(true);
        const response = await getTopRatedDoctors(10);
        let doctorsData = [];
        
        // Handle different response structures
        if (response?.data) {
          if (Array.isArray(response.data)) {
            doctorsData = response.data;
          } else if (Array.isArray(response.data.data)) {
            doctorsData = response.data.data;
          } else if (Array.isArray(response.data.items)) {
            doctorsData = response.data.items;
          }
        }
        
        // Map to component format
        const mappedDoctors = doctorsData
          .filter(d => d && d.doctorStatus === 'Active')
          .slice(0, 10) // Limit to 10 doctors
          .map((doctor, index) => ({
            id: doctor.id || doctor.accountId || index + 1,
            name: doctor.doctorName || doctor.fullName || "",
            category: doctor.specializationName || "Bác sĩ",
            achievements: parseBiography(doctor.biography),
            image: doctor.avatar || doctor.profileImage || `https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80&sig=${index}`,
            bgColor: index % 3 === 0 ? "bg-yellow-100" : index % 3 === 1 ? "bg-yellow-200" : "bg-blue-100"
          }));
        
        // Fallback to default doctors if no data
        if (mappedDoctors.length === 0) {
          setDoctors([
            {
              id: 1,
              name: "TS.BS Đặng Trần Đạt",
              category: "Cố Vấn Chuyên Môn",
              achievements: [
                "Bằng khen của Bộ trưởng Bộ Y Tế về đóng góp cho ngành Nhãn khoa",
                "Bằng khen Sáng kiến, sáng tạo thủ đô do Ban Nhân Dân Thành Phố Hà Nội trao tặng",
                "Trưởng khoa Khám và Điều trị theo yêu cầu - Bệnh viện Mắt Trung Ương",
                "Hơn 30 năm kinh nghiệm, thực hiện thành công hơn 100,000 ca phẫu thuật nhãn khoa"
              ],
              image: "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
              bgColor: "bg-yellow-100"
            }
          ]);
        } else {
          setDoctors(mappedDoctors);
        }
      } catch (error) {
        console.error("Error loading doctors:", error);
        // Fallback to default
        setDoctors([
          {
            id: 1,
            name: "TS.BS Đặng Trần Đạt",
            category: "Cố Vấn Chuyên Môn",
            achievements: [
              "Bằng khen của Bộ trưởng Bộ Y Tế về đóng góp cho ngành Nhãn khoa",
              "Bằng khen Sáng kiến, sáng tạo thủ đô do Ban Nhân Dân Thành Phố Hà Nội trao tặng",
              "Trưởng khoa Khám và Điều trị theo yêu cầu - Bệnh viện Mắt Trung Ương",
              "Hơn 30 năm kinh nghiệm, thực hiện thành công hơn 100,000 ca phẫu thuật nhãn khoa"
            ],
            image: "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
            bgColor: "bg-yellow-100"
          }
        ]);
      } finally {
        setLoading(false);
      }
    };

    loadDoctors();
  }, []);

  const getVisibleDoctors = () => {
    if (doctors.length === 0) return [];
    const total = doctors.length;
    const prev = (currentSlide - 1 + total) % total;
    const next = (currentSlide + 1) % total;
    
    return [
      { ...doctors[prev], position: 'prev' },
      { ...doctors[currentSlide], position: 'current' },
      { ...doctors[next], position: 'next' }
    ];
  };

  if (loading) {
    return (
      <section id="doctors-section" className="py-16 bg-gradient-to-b from-blue-50 to-white relative overflow-hidden w-full">
        <div className="container mx-auto px-4">
          <div className="text-center">
            <h2 className="text-4xl lg:text-5xl font-bold mb-2">
              <span className="text-[#0c5a8a]">ĐỘI NGŨ </span>
              <span className="text-yellow-500">BÁC SĨ</span>
            </h2>
            <p className="text-gray-600 mt-4">Đang tải...</p>
          </div>
        </div>
      </section>
    );
  }

  if (doctors.length === 0) {
    return null;
  }

  const nextSlide = () => {
    setCurrentSlide((prev) => (prev + 1) % doctors.length);
  };

  const prevSlide = () => {
    setCurrentSlide((prev) => (prev - 1 + doctors.length) % doctors.length);
  };

  return (
    <section id="doctors-section" className="py-16 bg-gradient-to-b from-blue-50 to-white relative overflow-hidden w-full">
      {/* Decorative circles */}
      <div className="absolute top-20 left-10 w-32 h-32 bg-blue-200 rounded-full opacity-30"></div>
      <div className="absolute top-40 left-5 w-16 h-16 bg-blue-300 rounded-full opacity-40"></div>
      <div className="absolute top-20 right-10 w-32 h-32 bg-blue-400 rounded-full opacity-30"></div>
      <div className="absolute top-40 right-5 w-20 h-20 bg-blue-300 rounded-full opacity-40"></div>

      {/* Header - Centered */}
      <div className="container mx-auto px-4 relative z-10 mb-12">
        <div className="text-center">
          <h2 className="text-4xl lg:text-5xl font-bold mb-2">
            <span className="text-[#0c5a8a]">ĐỘI NGŨ </span>
            <span className="text-yellow-500">BÁC SĨ</span>
          </h2>
        </div>
      </div>

      {/* Carousel - Full width */}
      <div className="relative w-full overflow-hidden">
        <div className="relative flex items-center justify-center min-h-[600px] lg:min-h-[700px] py-8">
          {/* Previous Button - Căn giữa với cards */}
          <button
            onClick={prevSlide}
            className="absolute left-4 lg:left-8 z-30 p-3 bg-white hover:bg-gray-100 text-gray-600 rounded-full shadow-lg transition-all duration-300 transform hover:scale-110"
          >
            <ChevronLeft className="w-6 h-6" />
          </button>

          {/* Cards Container - 3 cards visible: prev (25%), current (50%), next (25%) */}
          <div className="flex items-stretch justify-center gap-4 px-4 w-full">
            {getVisibleDoctors().map((doctor, idx) => (
              <div
                key={`${doctor.id}-${idx}`}
                className={`flex-shrink-0 transition-all duration-500 h-full ${
                  doctor.position === 'current'
                    ? 'w-1/2 opacity-100 z-20 scale-100'
                    : doctor.position === 'prev'
                    ? 'w-1/4 opacity-60 z-10 scale-95'
                    : 'w-1/4 opacity-60 z-10 scale-95'
                }`}
              >
                <div
                  onClick={() => {
                    const doctorId = doctor.id;
                    if (doctorId) {
                      navigate(`/doctors/${doctorId}`);
                    }
                  }}
                  className="bg-white rounded-3xl shadow-xl p-8 lg:p-10 h-full flex flex-col cursor-pointer hover:shadow-2xl transition-all duration-300"
                >
                  <div className={`flex flex-col md:flex-row items-center gap-4 lg:gap-6 flex-1 ${
                    doctor.position === 'current' ? '' : 'flex-col'
                  }`}>
                    {/* Image with decorative background */}
                    <div className="relative flex-shrink-0">
                      <div className={`absolute inset-0 ${doctor.bgColor} rounded-full transform -translate-x-4 -translate-y-4 ${
                        doctor.position === 'current' ? 'w-64 h-80 lg:w-80 lg:h-96' : 'w-40 h-52 lg:w-48 lg:h-64'
                      }`}></div>
                      <div className={`relative overflow-hidden rounded-3xl ${
                        doctor.position === 'current' ? 'w-56 h-72 lg:w-72 lg:h-[22rem]' : 'w-32 h-40 lg:w-40 lg:h-52'
                      }`}>
                        <img
                          src={doctor.image}
                          alt={doctor.name}
                          className="w-full h-full object-cover"
                        />
                      </div>
                    </div>

                    {/* Content - Flex column để phân bổ đều */}
                    <div className="flex-1 w-full flex flex-col justify-between min-h-0">
                      <div>
                        <p className={`text-gray-500 mb-2 ${
                          doctor.position === 'current' ? 'text-base' : 'text-xs lg:text-sm'
                        }`}>{doctor.category}</p>
                        <h3 className={`font-bold text-[#0c5a8a] mb-4 ${
                          doctor.position === 'current' ? 'text-2xl lg:text-3xl' : 'text-lg lg:text-xl'
                        }`}>
                          {doctor.name}
                        </h3>

                        {/* Achievements List - với max-height và overflow */}
                        {doctor.achievements && doctor.achievements.length > 0 ? (
                          <ul className={`mb-4 ${
                            doctor.position === 'current' ? 'space-y-2 lg:space-y-3' : 'space-y-1'
                          } ${doctor.position === 'current' ? 'max-h-[200px] lg:max-h-[240px]' : 'max-h-[120px]'} overflow-y-auto`}>
                            {doctor.achievements.slice(0, doctor.position === 'current' ? 4 : 2).map((achievement, idx) => (
                              <li key={idx} className={`flex items-start text-gray-700 ${
                                doctor.position === 'current' ? 'text-sm lg:text-base' : 'text-xs lg:text-sm'
                              }`}>
                                <CheckCircle2 className={`text-[#0c5a8a] mr-2 flex-shrink-0 mt-0.5 ${
                                  doctor.position === 'current' ? 'w-4 h-4 lg:w-5 lg:h-5' : 'w-3 h-3 lg:w-4 lg:h-4'
                                }`} />
                                <span className="flex-1 leading-relaxed">{achievement}</span>
                              </li>
                            ))}
                          </ul>
                        ) : (
                          <p className={`text-gray-500 italic mb-4 ${
                            doctor.position === 'current' ? 'text-sm lg:text-base' : 'text-xs lg:text-sm'
                          }`}>
                            Chưa có thông tin giới thiệu
                          </p>
                        )}
                      </div>

                      {/* Button - luôn ở dưới cùng */}
                      <div className="mt-auto pt-4">
                        <button
                          onClick={(e) => {
                            e.stopPropagation();
                            const doctorId = doctor.id;
                            if (doctorId) {
                              navigate(`/doctors/${doctorId}`);
                            }
                          }}
                          className={`inline-flex items-center bg-[#0c5a8a] hover:bg-[#094a75] text-white font-semibold rounded-full transition-all duration-300 shadow-md ${
                            doctor.position === 'current' 
                              ? 'px-6 py-2.5 lg:px-8 lg:py-3 text-sm lg:text-base' 
                              : 'px-4 py-2 text-xs lg:text-sm'
                          }`}
                        >
                          → XEM CHI TIẾT
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>

          {/* Next Button - Căn giữa với cards */}
          <button
            onClick={nextSlide}
            className="absolute right-4 lg:right-8 z-30 p-3 bg-white hover:bg-gray-100 text-gray-600 rounded-full shadow-lg transition-all duration-300 transform hover:scale-110"
          >
            <ChevronRight className="w-6 h-6" />
          </button>
        </div>

        {/* Dots Navigation - Gần cards hơn */}
        <div className="flex justify-center mt-4 space-x-2">
          {doctors.map((_, index) => (
            <button
              key={index}
              onClick={() => setCurrentSlide(index)}
              className={`w-3 h-3 rounded-full transition-all duration-300 ${
                index === currentSlide
                  ? "bg-[#0c5a8a] w-8"
                  : "bg-gray-300 hover:bg-gray-400"
              }`}
            />
          ))}
        </div>
      </div>
    </section>
  );
};

export default DoctorsCarousel;