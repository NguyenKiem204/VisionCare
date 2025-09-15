import React, { useState, useEffect } from "react";
import { Users, Clock, Award, Phone } from "lucide-react";

const StatsBar = () => {
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

    const element = document.getElementById("stats-bar");
    if (element) {
      observer.observe(element);
    }

    return () => {
      if (element) {
        observer.unobserve(element);
      }
    };
  }, []);

  const stats = [
    {
      icon: Users,
      value: "15,000+",
      label: "Bệnh nhân tin tưởng",
      color: "text-blue-500",
    },
    {
      icon: Clock,
      value: "20+",
      label: "Năm kinh nghiệm",
      color: "text-green-500",
    },
    {
      icon: Award,
      value: "99.8%",
      label: "Tỷ lệ thành công",
      color: "text-orange-500",
    },
    {
      icon: Phone,
      value: "24/7",
      label: "Hỗ trợ khẩn cấp",
      color: "text-blue-600",
    },
  ];

  return (
    <section id="stats-bar" className="relative py-12">
      {/* darken background for readability */}
      <div aria-hidden className="absolute inset-0 bg-black/60" />
      <div className="relative z-10 container mx-auto px-4">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
          {stats.map((stat, index) => (
            <div
              key={index}
              className={`text-center transition-all duration-1000 ${
                isVisible
                  ? "opacity-100 translate-y-0"
                  : "opacity-0 translate-y-8"
              }`}
              style={{ transitionDelay: `${index * 200}ms` }}
            >
              <div className="flex justify-center mb-4">
                <div
                  className={`p-4 rounded-full bg-white/85 backdrop-blur-sm shadow-lg ${stat.color}`}
                >
                  <stat.icon className="w-8 h-8" />
                </div>
              </div>
              <div
                className={`text-3xl md:text-4xl font-bold ${stat.color} mb-2`}
              >
                {isVisible ? stat.value : "0"}
              </div>
              <div className="text-sm text-white font-medium">
                {stat.label}
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default StatsBar;
