import React, { useState, useEffect } from "react";
import { ChevronLeft, ChevronRight, Star, Award, GraduationCap, Users } from "lucide-react";

const DoctorsCarousel = () => {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsVisible(true);
        }
      },
      { threshold: 0.1 }
    );

    const element = document.getElementById("doctors-section");
    if (element) {
      observer.observe(element);
    }

    return () => {
      if (element) {
        observer.unobserve(element);
      }
    };
  }, []);

  const doctors = [
    {
      id: 1,
      name: "BS. CKII Nguyễn Văn An",
      position: "Trưởng Khoa",
      specialty: "Chuyên gia về phẫu thuật khúc xạ và Lasik",
      experience: "25+ năm kinh nghiệm",
      education: "Tiến sĩ Y khoa - Đại học Y Hà Nội",
      achievement: "5000+ ca phẫu thuật thành công",
      image: "https://images.unsplash.com/photo-1612349317150-e413f6a5b16d?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      rating: 4.9,
      reviews: 1247
    },
    {
      id: 2,
      name: "BS. CKI Trần Thị Bình",
      position: "Phó Khoa",
      specialty: "Chuyên gia điều trị võng mạc và glaucoma",
      experience: "18+ năm kinh nghiệm",
      education: "Nghiên cứu sinh tại Đức 3 năm",
      achievement: "Chuyên gia hàng đầu về võng mạc",
      image: "https://images.unsplash.com/photo-1559839734-2b71ea197ec2?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      rating: 4.8,
      reviews: 892
    },
    {
      id: 3,
      name: "BS. CKI Lê Minh Châu",
      position: "Bác sĩ chính",
      specialty: "Chuyên gia nhãn khoa trẻ em",
      experience: "15+ năm kinh nghiệm",
      education: "Đào tạo tại Singapore Children Hospital",
      achievement: "Chuyên gia trẻ em hàng đầu",
      image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      rating: 4.9,
      reviews: 1563
    },
    {
      id: 4,
      name: "BS. CKI Phạm Thu Dung",
      position: "Bác sĩ chính",
      specialty: "Chuyên gia phẫu thuật thẩm mỹ mắt",
      experience: "12+ năm kinh nghiệm",
      education: "Chứng chỉ thẩm mỹ quốc tế",
      achievement: "Chuyên gia thẩm mỹ mắt",
      image: "https://images.unsplash.com/photo-1594824375455-6b5a9ba6a48a?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      rating: 4.7,
      reviews: 634
    }
  ];

  const nextSlide = () => {
    setCurrentSlide((prev) => (prev + 1) % Math.ceil(doctors.length / 2));
  };

  const prevSlide = () => {
    setCurrentSlide((prev) => (prev - 1 + Math.ceil(doctors.length / 2)) % Math.ceil(doctors.length / 2));
  };

  const getVisibleDoctors = () => {
    const startIndex = currentSlide * 2;
    return doctors.slice(startIndex, startIndex + 2);
  };

  return (
    <section id="doctors-section" className="py-16 bg-white">
      <div className="container mx-auto px-4">
        <div className="text-center mb-12">
          <h2 className="text-3xl md:text-4xl font-bold text-gray-900 mb-4">
            Đội Ngũ Bác Sĩ Hàng Đầu
          </h2>
          <p className="text-lg text-gray-600 max-w-3xl mx-auto">
            Đội ngũ bác sĩ chuyên môn cao với nhiều năm kinh nghiệm và được đào tạo tại các trung tâm y tế hàng đầu thế giới
          </p>
        </div>

        <div className="relative">
          {/* Doctors Grid */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8 mb-8">
            {getVisibleDoctors().map((doctor, index) => (
              <div
                key={doctor.id}
                className={`bg-white rounded-2xl shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-105 overflow-hidden ${
                  isVisible ? "animate-fade-in-up" : "opacity-0 translate-y-8"
                }`}
                style={{ transitionDelay: `${index * 200}ms` }}
              >
                <div className="relative">
                  <img
                    src={doctor.image}
                    alt={doctor.name}
                    className="w-full h-64 object-cover"
                  />
                  <div className="absolute top-4 right-4 bg-white/90 backdrop-blur-sm rounded-full px-3 py-1 flex items-center space-x-1">
                    <Star className="w-4 h-4 text-yellow-500 fill-current" />
                    <span className="text-sm font-semibold text-gray-900">{doctor.rating}</span>
                  </div>
                </div>
                
                <div className="p-6">
                  <div className="mb-4">
                    <h3 className="text-xl font-bold text-gray-900 mb-1">{doctor.name}</h3>
                    <p className="text-primary-600 font-semibold mb-2">{doctor.position}</p>
                    <p className="text-gray-600 text-sm leading-relaxed">{doctor.specialty}</p>
                  </div>

                  <div className="space-y-2 mb-4">
                    <div className="flex items-center space-x-2 text-sm text-gray-600">
                      <Users className="w-4 h-4 text-primary-500" />
                      <span>{doctor.experience}</span>
                    </div>
                    <div className="flex items-center space-x-2 text-sm text-gray-600">
                      <GraduationCap className="w-4 h-4 text-secondary-500" />
                      <span>{doctor.education}</span>
                    </div>
                    <div className="flex items-center space-x-2 text-sm text-gray-600">
                      <Award className="w-4 h-4 text-accent-500" />
                      <span>{doctor.achievement}</span>
                    </div>
                  </div>

                  <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-1">
                      {[...Array(5)].map((_, i) => (
                        <Star
                          key={i}
                          className={`w-4 h-4 ${
                            i < Math.floor(doctor.rating)
                              ? "text-yellow-500 fill-current"
                              : "text-gray-300"
                          }`}
                        />
                      ))}
                      <span className="text-sm text-gray-500 ml-2">({doctor.reviews} đánh giá)</span>
                    </div>
                    <a
                      href="/doctors"
                      className="text-primary-600 hover:text-primary-700 text-sm font-medium"
                    >
                      Xem thêm →
                    </a>
                  </div>
                </div>
              </div>
            ))}
          </div>

          {/* Navigation */}
          <div className="flex justify-center space-x-4">
            <button
              onClick={prevSlide}
              className="p-3 bg-gray-100 hover:bg-gray-200 text-gray-600 rounded-full transition-colors duration-200"
            >
              <ChevronLeft className="w-5 h-5" />
            </button>
            <button
              onClick={nextSlide}
              className="p-3 bg-gray-100 hover:bg-gray-200 text-gray-600 rounded-full transition-colors duration-200"
            >
              <ChevronRight className="w-5 h-5" />
            </button>
          </div>

          {/* Dots */}
          <div className="flex justify-center mt-6 space-x-2">
            {[...Array(Math.ceil(doctors.length / 2))].map((_, index) => (
              <button
                key={index}
                onClick={() => setCurrentSlide(index)}
                className={`w-3 h-3 rounded-full transition-all duration-300 ${
                  index === currentSlide
                    ? "bg-primary-500 scale-125"
                    : "bg-gray-300 hover:bg-gray-400"
                }`}
              />
            ))}
          </div>
        </div>

        <div className="text-center mt-12">
          <a
            href="/doctors"
            className="inline-flex items-center px-8 py-4 bg-primary-500 hover:bg-primary-600 text-white font-semibold rounded-full transition-all duration-300 transform hover:scale-105 shadow-lg"
          >
            Xem Đầy Đủ Đội Ngũ
            <ChevronRight className="w-5 h-5 ml-2" />
          </a>
        </div>
      </div>
    </section>
  );
};

export default DoctorsCarousel;
