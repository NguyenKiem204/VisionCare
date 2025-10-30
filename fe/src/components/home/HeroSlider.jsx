import React, { useState, useEffect } from "react";
import { ChevronLeft, ChevronRight, Calendar, ArrowRight } from "lucide-react";
import { getBanners } from "../../services/bannerAPI";

const HeroSlider = () => {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [isTransitioning, setIsTransitioning] = useState(false);
  const [slides, setSlides] = useState([
    {
      id: 1,
      title: "VisionCare - Chăm Sóc Mắt Chuyên Nghiệp",
      subtitle: "20+ năm kinh nghiệm - Công nghệ hiện đại - Đội ngũ chuyên gia",
      background: "bg-gradient-to-r from-blue-600 to-green-600",
      image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80",
      cta: {
        primary: { text: "Đặt Lịch Ngay", href: "/booking", icon: Calendar },
        secondary: { text: "Tìm Hiểu Thêm", href: "/services", icon: ArrowRight }
      }
    },
    {
      id: 2,
      title: "Công Nghệ Tiên Tiến",
      subtitle: "Máy móc nhập khẩu từ Đức - Chẩn đoán chính xác 99.8%",
      background: "bg-gradient-to-r from-green-600 to-blue-600",
      image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80",
      cta: {
        primary: { text: "Xem Thiết Bị", href: "/equipment", icon: ArrowRight },
        secondary: { text: "Đặt Lịch Ngay", href: "/booking", icon: Calendar }
      }
    },
    {
      id: 3,
      title: "Dịch Vụ Toàn Diện",
      subtitle: "Từ khám tổng quát đến phẫu thuật chuyên sâu",
      background: "bg-gradient-to-r from-orange-600 to-blue-600",
      image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80",
      cta: {
        primary: { text: "Xem Dịch Vụ", href: "/services", icon: ArrowRight },
        secondary: { text: "Tư Vấn Miễn Phí", href: "/contact", icon: ArrowRight }
      }
    }
  ]);

  useEffect(() => {
    let mounted = true;
    (async () => {
      try {
        const data = await getBanners("home_hero");
        if (mounted && Array.isArray(data) && data.length > 0) {
          const mapped = data.map((b, idx) => ({
            id: b.bannerId || idx + 1,
            title: b.title,
            subtitle: b.description,
            background: "bg-gradient-to-r from-blue-600 to-green-600",
            image: b.imageUrl,
            cta: {
              primary: { text: "Đặt Lịch Ngay", href: "/booking", icon: Calendar },
              secondary: { text: "Tìm Hiểu Thêm", href: "/services", icon: ArrowRight }
            }
          }));
          setSlides(mapped);
          setCurrentSlide(0);
        }
      } catch (_) {
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
    }, 6000); // Tăng thời gian lên 6 giây
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

  const PrimaryIcon = slides[currentSlide].cta.primary.icon;
  const SecondaryIcon = slides[currentSlide].cta.secondary.icon;

  return (
    <section className="relative h-[70vh] min-h-[500px] max-h-[800px] overflow-hidden">
      {/* Background Images */}
      {slides.map((slide, index) => (
        <div
          key={slide.id}
          className={`absolute inset-0 transition-opacity duration-1000 ${
            index === currentSlide ? "opacity-100" : "opacity-0"
          }`}
        >
          <div
            className="absolute inset-0 bg-cover bg-center bg-no-repeat"
            style={{ backgroundImage: `url(${slide.image})` }}
          />
          <div className={`absolute inset-0 ${slide.background} opacity-70`} />
        </div>
      ))}

      {/* Content */}
      <div className="relative z-10 h-full flex items-center">
        <div className="container mx-auto px-4">
          <div className="max-w-4xl">
            <div className="transition-all duration-1000">
              <h1 className="text-3xl md:text-4xl lg:text-5xl font-bold text-white mb-4 leading-tight">
                {slides[currentSlide].title}
              </h1>
              <p className="text-lg md:text-xl text-white/90 mb-8 max-w-3xl">
                {slides[currentSlide].subtitle}
              </p>
              <div className="flex flex-col sm:flex-row gap-4">
                <a
                  href={slides[currentSlide].cta.primary.href}
                  className="inline-flex items-center justify-center px-6 py-3 bg-orange-500 hover:bg-orange-600 text-white font-semibold rounded-full transition-all duration-300 transform hover:scale-105 shadow-lg"
                >
                  <PrimaryIcon className="w-5 h-5 mr-2" />
                  {slides[currentSlide].cta.primary.text}
                </a>
                <a
                  href={slides[currentSlide].cta.secondary.href}
                  className="inline-flex items-center justify-center px-6 py-3 bg-white/20 hover:bg-white/30 text-white font-semibold rounded-full border-2 border-white/50 transition-all duration-300 backdrop-blur-sm"
                >
                  <SecondaryIcon className="w-5 h-5 mr-2" />
                  {slides[currentSlide].cta.secondary.text}
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Navigation Arrows */}
      <button
        onClick={prevSlide}
        disabled={isTransitioning}
        className="absolute left-4 top-1/2 transform -translate-y-1/2 z-20 bg-white/20 hover:bg-white/30 text-white p-3 rounded-full transition-all duration-300 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronLeft className="w-6 h-6" />
      </button>
      <button
        onClick={nextSlide}
        disabled={isTransitioning}
        className="absolute right-4 top-1/2 transform -translate-y-1/2 z-20 bg-white/20 hover:bg-white/30 text-white p-3 rounded-full transition-all duration-300 backdrop-blur-sm disabled:opacity-50 disabled:cursor-not-allowed"
      >
        <ChevronRight className="w-6 h-6" />
      </button>

      {/* Dots Navigation */}
      <div className="absolute bottom-6 left-1/2 transform -translate-x-1/2 z-20 flex space-x-2">
        {slides.map((_, index) => (
          <button
            key={index}
            onClick={() => goToSlide(index)}
            disabled={isTransitioning}
            className={`w-3 h-3 rounded-full transition-all duration-300 disabled:cursor-not-allowed ${
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
