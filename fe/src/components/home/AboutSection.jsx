import React, { useState, useEffect } from "react";

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
    {
      icon: "👨‍⚕️",
      title: "CHUYÊN GIA HỘI TỤ",
      description: "Đội ngũ chuyên gia nhãn khoa giàu kinh nghiệm từng công tác tại các bệnh viện lớn tại Việt Nam."
    },
    {
      icon: "⚖️",
      title: "CÔNG NGHỆ HÀNG ĐẦU",
      description: "Bệnh viện đầu tiên sử dụng máy chụp ảnh đáy mắt trường siêu rộng và máy chụp cắt lớp võng mạc dạng chùm, ch..."
    },
    {
      icon: "📋",
      title: "TRÁCH NHIỆM VƯỢT TRỘI",
      description: "Với không gian hiện đại, thoải mái và quy trình phục vụ chuyên nghiệp, luôn chu đáo luôn sẵn sàng lắng nghe vấ..."
    },
    {
      icon: "🤝",
      title: "DỊCH VỤ TẬN TÂM",
      description: "Giải pháp điều trị tối ưu, an toàn và hiệu quả, đáp ứng mọi nhu cầu về chăm sóc sức khỏe thị giác cho cộng đồng."
    }
  ];

  return (
    <section id="about-section" className="py-16 bg-white">
      <div className="container mx-auto px-4 max-w-6xl">
        {/* Two Column Layout */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-stretch mb-16">
          {/* Left - Image */}
          <div className={`transition-all duration-1000 h-full ${
            isVisible ? "opacity-100 translate-x-0" : "opacity-0 -translate-x-8"
          }`}>
            <div className="relative h-full lg:h-[460px]">
              <img
                src="https://plus.unsplash.com/premium_photo-1677410176369-76ec12f34cf1?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1200&q=80"
                alt="Bác sĩ khám mắt"
                className="w-full h-full object-cover rounded-lg shadow-xl"
              />
              {/* Floating Card - Bottom Left */}
              <div className="absolute -bottom-6 -left-6 bg-gradient-to-br from-yellow-400 to-orange-500 rounded-2xl p-5 shadow-2xl">
                <div className="flex items-center space-x-3">
                  <div className="w-14 h-14 bg-white rounded-full flex items-center justify-center">
                    <span className="text-2xl">☎️</span>
                  </div>
                  <div className="text-white">
                    <p className="text-xs font-semibold">Bạn Cần Hỗ Trợ</p>
                    <p className="text-sm font-normal mb-1">Gọi Ngay: <span className="font-bold text-lg">1800 3369</span></p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Right - Content */}
          <div className={`transition-all duration-1000 h-full ${
            isVisible ? "opacity-100 translate-x-0" : "opacity-0 translate-x-8"
          }`}>
            <p className="text-yellow-500 text-xs font-semibold uppercase tracking-wide mb-3">
              BRIGHT YOUR SIGHT
            </p>
            <h2 className="text-3xl md:text-4xl font-bold text-[#0c5a8a] mb-5">
              BỪNG SÁNG TẦM NHÌN
            </h2>
            <p className="text-gray-600 text-sm leading-relaxed mb-8">
              Ứng dụng những thành tựu y khoa và phương pháp quản trị chuyên môn theo tiêu chuẩn nhãn khoa khắt khe bậc nhất, 
              Bệnh viện Mắt Ánh Dương tự hào là bệnh viện chuyên khoa mắt công nghệ cao hàng đầu tại Việt Nam. 
              Bệnh viện cam kết chất lượng chuẩn quốc tế từ khâu chẩn đoán, lựa chọn phác đồ điều trị phù hợp, 
              đến chăm sóc bệnh nhân toàn diện, tận tâm.
            </p>
            
            {/* Feature Grid - 2x2 */}
            <div className="grid grid-cols-2 gap-6">
              {features.map((feature, idx) => (
                <div
                  key={idx}
                  className={`flex items-start space-x-3 transition-all duration-1000 ${
                    isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8"
                  }`}
                  style={{ transitionDelay: `${idx * 150}ms` }}
                >
                  <div className="flex-shrink-0">
                    <div className="w-12 h-12 bg-yellow-100 rounded-lg flex items-center justify-center">
                      <span className="text-2xl">{feature.icon}</span>
                    </div>
                  </div>
                  <div>
                    <h3 className="text-xs font-bold text-[#0c5a8a] mb-1 uppercase">
                      {feature.title}
                    </h3>
                    <p className="text-xs text-gray-600 leading-relaxed">
                      {feature.description}
                    </p>
                  </div>
                </div>
              ))}
            </div>

            {/* CTA Button */}
            {/* <div className="mt-8">
              <a href="/#doctors-section" className="inline-flex items-center px-6 py-2.5 bg-gradient-to-r from-yellow-400 to-orange-500 hover:from-yellow-500 hover:to-orange-600 text-white text-sm font-bold rounded-full transition-all duration-300 transform hover:scale-105 shadow-lg uppercase">
                <span>→ XEM THÊM</span>
              </a>
            </div> */}
          </div>
        </div>
      </div>
    </section>
  );
};

export default AboutSection;