import React, { useState, useEffect } from "react";
import {
  Eye,
  Heart,
  AlertTriangle,
  Baby,
  Sparkles,
  Phone,
  Video,
  Calendar,
  Shield,
  Stethoscope,
  Microscope,
  Activity,
} from "lucide-react";

const ServicesGrid = () => {
  const [currentPage, setCurrentPage] = useState(0);
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

    const element = document.getElementById("services-section");
    if (element) {
      observer.observe(element);
    }

    return () => {
      if (element) {
        observer.unobserve(element);
      }
    };
  }, []);

  const services = [
    {
      icon: Eye,
      title: "Khúc Xạ & Cận Thị",
      color: "text-blue-500",
    },
    {
      icon: Heart,
      title: "Võng Mạc",
      color: "text-green-500",
    },
    {
      icon: AlertTriangle,
      title: "Glaucoma",
      color: "text-orange-500",
    },
    {
      icon: Baby,
      title: "Nhãn Khoa Trẻ Em",
      color: "text-pink-500",
    },
    {
      icon: Sparkles,
      title: "Phẫu Thuật Thẩm Mỹ",
      color: "text-purple-500",
    },
    {
      icon: Phone,
      title: "Cấp Cứu Mắt 24/7",
      color: "text-red-500",
    },
    {
      icon: Video,
      title: "Tư Vấn Online",
      color: "text-blue-600",
    },
    {
      icon: Calendar,
      title: "Khám Định Kỳ",
      color: "text-green-600",
    },
    {
      icon: Shield,
      title: "Bảo Hiểm Y Tế",
      color: "text-indigo-500",
    },
    {
      icon: Stethoscope,
      title: "Khám Tổng Quát",
      color: "text-blue-700",
    },
    {
      icon: Microscope,
      title: "Xét Nghiệm Mắt",
      color: "text-green-700",
    },
    {
      icon: Activity,
      title: "Theo Dõi Bệnh Lý",
      color: "text-orange-600",
    },
  ];

  const itemsPerPage = 6;
  const totalPages = Math.ceil(services.length / itemsPerPage);
  const currentServices = services.slice(
    currentPage * itemsPerPage,
    (currentPage + 1) * itemsPerPage
  );

  const goToPage = (page) => {
    setCurrentPage(page);
  };

  return (
    <section id="services-section" className="relative py-20">
      <div aria-hidden className="absolute inset-0 bg-black/60" />
      <div className="relative z-10 container mx-auto px-4">
        <div className="text-center mb-16">
          <div className="inline-flex items-center px-4 py-2 rounded-full bg-white/75 backdrop-blur-sm shadow-sm">
            <h2 className="text-2xl md:text-3xl font-bold text-blue-600">
              CHUYÊN KHOA
            </h2>
          </div>
        </div>

        {/* Services Grid - 2 rows of 6 items */}
        <div className="grid grid-cols-6 gap-8 mb-12">
          {currentServices.map((service, index) => {
            const IconComponent = service.icon;
            return (
              <div
                key={index}
                className={`flex flex-col items-center text-center transition-all duration-500 ${
                  isVisible
                    ? "opacity-100 translate-y-0"
                    : "opacity-0 translate-y-8"
                }`}
                style={{ transitionDelay: `${index * 100}ms` }}
              >
                <div className="w-20 h-20 bg-white/92 backdrop-blur-sm border-2 border-orange-500 rounded-full flex items-center justify-center mb-4 shadow-lg hover:shadow-xl transition-all duration-300 hover:scale-110">
                  <IconComponent className={`w-10 h-10 ${service.color}`} />
                </div>
                <h3 className="text-sm font-semibold text-white drop-shadow-md leading-tight">
                  {service.title}
                </h3>
              </div>
            );
          })}
        </div>

        {/* Pagination Dots */}
        <div className="flex justify-center space-x-2">
          {Array.from({ length: totalPages }).map((_, index) => (
            <button
              key={index}
              onClick={() => goToPage(index)}
              className={`w-3 h-3 rounded-full transition-all duration-300 ${
                index === currentPage
                  ? "bg-orange-500 scale-125"
                  : "bg-gray-300/80 hover:bg-orange-300"
              }`}
            />
          ))}
        </div>
      </div>
    </section>
  );
};

export default ServicesGrid;
