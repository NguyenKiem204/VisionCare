import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { ChevronLeft, ChevronRight, Phone, Calendar, Users } from "lucide-react";
import { getBanners } from "../../services/bannerAPI";

const HeroSlider = () => {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [isTransitioning, setIsTransitioning] = useState(false);
  const [slides, setSlides] = useState([
    {
      id: 1,
      title: "Thu Thậy là Trảy Tâm",
      subtitle: "Lấp kỳ tích sau từ ca nhân bản",
      image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80",
    },
    {
      id: 2,
      title: "CÔNG NGHỆ TIÊN TIẾN",
      subtitle: "Máy móc nhập khẩu từ Đức",
      image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80",
    },
    {
      id: 3,
      title: "DỊCH VỤ TOÀN DIỆN",
      subtitle: "Chăm sóc chuyên nghiệp",
      image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80",
    }
  ]);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const data = await getBanners("home_hero");
        if (mounted && Array.isArray(data) && data.length > 0) {
          const mapped = data
            .filter(b => b && (b.imageUrl || b.ImageUrl)) // Chỉ lấy banner có image
            .map((b, idx) => ({
              id: b.bannerId || b.BannerId || idx + 1,
              title: b.title || b.Title || "",
              subtitle: b.description || b.Description || "",
              image: b.imageUrl || b.ImageUrl || "",
            }));
          if (mapped.length > 0) {
            setSlides(mapped);
            setCurrentSlide(0);
          }
        }
      } catch (error) {
        console.error("Error loading banners:", error);
        // fallback to default slides
      }
    })();
    return () => {
      mounted = false;
    };
  }, []);

  useEffect(() => {
    const timer = setInterval(() => {
      if (!isTransitioning) {
        setCurrentSlide((prev) => (prev + 1) % slides.length);
      }
    }, 6000);
    return () => clearInterval(timer);
  }, [slides.length, isTransitioning]);

  const nextSlide = () => {
    if (isTransitioning) return;
    setIsTransitioning(true);
    setCurrentSlide((prev) => (prev + 1) % slides.length);
    setTimeout(() => setIsTransitioning(false), 1000);
  };

  const prevSlide = () => {
    if (isTransitioning) return;
    setIsTransitioning(true);
    setCurrentSlide((prev) => (prev - 1 + slides.length) % slides.length);
    setTimeout(() => setIsTransitioning(false), 1000);
  };

  const goToSlide = (index) => {
    if (isTransitioning || index === currentSlide) return;
    setIsTransitioning(true);
    setCurrentSlide(index);
    setTimeout(() => setIsTransitioning(false), 1000);
  };

  return (
    <section className="relative h-[55vh] min-h-[450px] max-h-[600px] overflow-visible bg-gradient-to-br from-blue-600 via-blue-500 to-cyan-500">
      {/* Background Images */}
      {slides.map((slide, index) => (
        <div
          key={slide.id}
          className={`absolute inset-0 transition-opacity duration-1000 z-0 ${
            index === currentSlide ? "opacity-100" : "opacity-0"
          }`}
        >
          <div
            className="absolute inset-0 bg-cover bg-center bg-no-repeat"
            style={{ backgroundImage: `url(${slide.image})` }}
          />
          <div className="absolute inset-0 bg-gradient-to-r from-blue-600/80 via-blue-500/70 to-transparent" />
        </div>
      ))}

      {/* Wave Shape at Bottom - positioned to allow cards to show */}
      <div className="absolute bottom-0 left-0 right-0 h-20 z-10">
        <svg className="absolute bottom-0 w-full h-full" viewBox="0 0 1440 120" preserveAspectRatio="none">
          <path d="M0,60 Q360,0 720,60 T1440,60 L1440,120 L0,120 Z" fill="white" />
        </svg>
      </div>

      {/* Content */}
      <div className="relative z-10 h-full flex items-center">
        <div className="container mx-auto px-4">
          <div className="max-w-xl">
            {/* Logo */}
            <div className="flex items-center space-x-2 mb-6">
              <div className="w-12 h-12 bg-gradient-to-br from-yellow-400 via-yellow-500 to-orange-500 rounded-full flex items-center justify-center shadow-lg relative">
                <div className="absolute inset-0">
                  {[...Array(12)].map((_, i) => (
                    <div
                      key={i}
                      className="absolute top-1/2 left-1/2 w-0.5 h-3 bg-yellow-400"
                      style={{
                        transform: `translate(-50%, -50%) rotate(${i * 30}deg) translateY(-18px)`,
                        transformOrigin: 'center'
                      }}
                    />
                  ))}
                </div>
                <div className="w-5 h-5 bg-white rounded-full z-10"></div>
              </div>
              <div className="flex flex-col leading-tight">
                <span className="text-xl font-bold text-yellow-400 tracking-wide">VisionCare</span>
                <span className="text-[10px] font-semibold text-white tracking-widest">
                  EYE HOSPITAL
                </span>
              </div>
            </div>

            {/* Title - Handwritten Style */}
            <div className="mb-4">
              <h1 className="text-4xl md:text-5xl font-bold text-white mb-2 leading-tight" style={{ fontFamily: 'cursive' }}>
                {slides[currentSlide].title}
              </h1>
            </div>
          </div>
        </div>
      </div>

      {/* Floating Feature Cards - Bottom Right - overlapping white section */}
      <div className="absolute bottom-0 right-4 lg:right-8 -bottom-16 z-30 w-full max-w-4xl px-4">
        <div className="flex justify-end">
          <div className="bg-white rounded-2xl shadow-2xl flex divide-x divide-gray-200">
            <a href="tel:1900xxxx" className="flex-1 p-6 hover:bg-blue-50 transition-colors cursor-pointer">
              <div className="flex items-start space-x-4">
                <div className="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0">
                  <Phone className="w-6 h-6 text-blue-600" />
                </div>
                <div>
                  <p className="text-base font-bold text-gray-900 mb-1">Gọi tổng đài</p>
                  <p className="text-sm text-gray-500">Hỗ trợ 24/7</p>
                </div>
              </div>
            </a>

            <Link to="/booking" className="flex-1 p-6 hover:bg-blue-50 transition-colors cursor-pointer">
              <div className="flex items-start space-x-4">
                <div className="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0">
                  <Calendar className="w-6 h-6 text-blue-600" />
                </div>
                <div>
                  <p className="text-base font-bold text-gray-900 mb-1">Đặt Lịch Khám</p>
                  <p className="text-sm text-gray-500">Khám với chuyên gia</p>
                </div>
              </div>
            </Link>

            <Link to="/#doctors-section" className="flex-1 p-6 hover:bg-blue-50 transition-colors cursor-pointer">
              <div className="flex items-start space-x-4">
                <div className="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center flex-shrink-0">
                  <Users className="w-6 h-6 text-blue-600" />
                </div>
                <div>
                  <p className="text-base font-bold text-gray-900 mb-1">Bác sĩ tư vấn</p>
                  <p className="text-sm text-gray-500">Tổng hợp thông tin bác sĩ</p>
                </div>
              </div>
            </Link>
          </div>
        </div>
      </div>

      {/* Navigation Arrows */}
      <button
        onClick={prevSlide}
        disabled={isTransitioning}
        className="absolute left-4 top-1/2 transform -translate-y-1/2 z-20 bg-white/20 hover:bg-white/30 text-white p-2 rounded-full transition-all duration-300 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronLeft className="w-5 h-5" />
      </button>
      <button
        onClick={nextSlide}
        disabled={isTransitioning}
        className="absolute right-4 top-1/2 transform -translate-y-1/2 z-20 bg-white/20 hover:bg-white/30 text-white p-2 rounded-full transition-all duration-300 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronRight className="w-5 h-5" />
      </button>

      {/* Dots Navigation */}
      <div className="absolute bottom-28 left-1/2 transform -translate-x-1/2 z-20 flex space-x-2">
        {slides.map((_, index) => (
          <button
            key={index}
            onClick={() => goToSlide(index)}
            disabled={isTransitioning}
            className={`w-2 h-2 rounded-full transition-all duration-300 disabled:cursor-not-allowed ${
              index === currentSlide
                ? "bg-white scale-125"
                : "bg-white/50 hover:bg-white/70"
            }`}
          />
        ))}
      </div>
    </section>
  );
};

export default HeroSlider;