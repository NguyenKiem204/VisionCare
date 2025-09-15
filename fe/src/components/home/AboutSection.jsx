import React, { useState, useEffect } from "react";
import { CheckCircle, ArrowRight } from "lucide-react";

const AboutSection = () => {
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

    const element = document.getElementById("about-section");
    if (element) {
      observer.observe(element);
    }

    return () => {
      if (element) {
        observer.unobserve(element);
      }
    };
  }, []);

  const features = [
    "Đội ngũ bác sĩ giàu kinh nghiệm",
    "Công nghệ thiết bị hiện đại nhất",
    "Dịch vụ tận tâm, chu đáo",
    "Chi phí hợp lý, bảo hiểm y tế"
  ];

  return (
    <section id="about-section" className="py-20 bg-white">
      <div className="container mx-auto px-4">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-16 items-center">
          {/* Content */}
          <div className={`transition-all duration-1000 ${
            isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8"
          }`}>
            <h2 className="text-4xl font-bold text-gray-900 mb-6">
              Tại sao VisionCare là sự lựa chọn hàng đầu?
            </h2>
            <p className="text-lg text-gray-600 mb-8 leading-relaxed">
              Với hơn 20 năm kinh nghiệm trong lĩnh vực nhãn khoa, VisionCare tự hào là trung tâm chăm sóc mắt hàng đầu với đội ngũ bác sĩ chuyên môn cao, trang thiết bị hiện đại và dịch vụ tận tâm.
            </p>
            <div className="space-y-4 mb-8">
              {features.map((feature, index) => (
                <div
                  key={index}
                  className={`flex items-center space-x-3 transition-all duration-1000 ${
                    isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8"
                  }`}
                  style={{ transitionDelay: `${index * 200}ms` }}
                >
                  <CheckCircle className="w-6 h-6 text-green-500 flex-shrink-0" />
                  <span className="text-gray-700 font-medium">{feature}</span>
                </div>
              ))}
            </div>
            <a
              href="/about"
              className="inline-flex items-center px-8 py-4 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-full transition-all duration-300 transform hover:scale-105 shadow-lg"
            >
              Tìm hiểu lịch sử VisionCare
              <ArrowRight className="w-5 h-5 ml-2" />
            </a>
          </div>

          {/* Image Gallery */}
          <div className={`transition-all duration-1000 ${
            isVisible ? "opacity-100" : "opacity-0"
          }`}>
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-4">
                <div className="aspect-square bg-gradient-to-br from-blue-100 to-blue-200 rounded-xl overflow-hidden">
                  <img
                    src="https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80"
                    alt="Phòng khám hiện đại"
                    className="w-full h-full object-cover"
                  />
                </div>
                <div className="aspect-square bg-gradient-to-br from-green-100 to-green-200 rounded-xl overflow-hidden">
                  <img
                    src="https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80"
                    alt="Thiết bị y tế"
                    className="w-full h-full object-cover"
                  />
                </div>
              </div>
              <div className="space-y-4 mt-8">
                <div className="aspect-square bg-gradient-to-br from-orange-100 to-orange-200 rounded-xl overflow-hidden">
                  <img
                    src="https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80"
                    alt="Bác sĩ chuyên nghiệp"
                    className="w-full h-full object-cover"
                  />
                </div>
                <div className="aspect-square bg-gradient-to-br from-blue-100 to-green-100 rounded-xl overflow-hidden">
                  <img
                    src="https://images.unsplash.com/photo-1551601651-2a8555f1a136?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80"
                    alt="Chăm sóc bệnh nhân"
                    className="w-full h-full object-cover"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default AboutSection;
