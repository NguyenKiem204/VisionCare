import React, { useState, useEffect } from "react";
import { Calendar, User, ArrowRight } from "lucide-react";

const NewsSection = () => {
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

    const element = document.getElementById("news-section");
    if (element) {
      observer.observe(element);
    }

    return () => {
      if (element) {
        observer.unobserve(element);
      }
    };
  }, []);

  const news = [
    {
      id: 1,
      title: "Bệnh Viện Mắt Ánh Dương Ký Kết Hợp Tác Cùng Trường Tiểu...",
      excerpt: "Ngày 21/10/2025, tại Trường Tiểu học & THCS Tây Hà Nội, Bệnh viện Mắt Ánh Dương đã chính thức ký kết biên bản...",
      image: "https://images.unsplash.com/photo-1540575467063-178a50c2df87?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      author: "hangvt-admin",
      date: "23/10/2025"
    },
    {
      id: 2,
      title: "LỄ KÝ KẾT HỢP TÁC CHIẾN LƯỢC TOÀN DIỆN GIỮA BỆN...",
      excerpt: "Ngày 21/10/2025, tại Hà Nội, Bệnh viện Mắt Ánh Dương và Trường Tiểu học & THCS Tây Hà Nội đã chính thức diễn ra...",
      image: "https://images.unsplash.com/photo-1531482615713-2afd69097998?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      author: "hangvt-admin",
      date: "22/10/2025"
    },
    {
      id: 3,
      title: "BỆNH VIỆN MẮT ÁNH DƯƠNG ĐỒNG HÀNH CÙNG TRƯỜNG...",
      excerpt: "Ngày 10/10/2025, Bệnh viện Mắt Ánh Dương phối hợp cùng Trường Liên cấp Tây Hà Nội (WIHS) tổ chức thành công...",
      image: "https://images.unsplash.com/photo-1576091160550-2173dba999ef?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      author: "hangvt-admin",
      date: "17/10/2025"
    },
    {
      id: 4,
      title: "Kính Cận Là Gì? Ai Cần Sử Dụng Kính Cận?",
      excerpt: "Kính cận là một dụng cụ quang học có gọng và tròng kính, được thiết kế đặc biệt để điều chỉnh tật khúc xạ cận thị...",
      image: "https://images.unsplash.com/photo-1574258495973-f010dfbb5371?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80",
      author: "hangvt-admin",
      date: "02/10/2025"
    }
  ];

  return (
    <section id="news-section" className="py-20 bg-gray-50">
      <div className="container mx-auto px-4 max-w-7xl">
        <div className="text-center mb-16">
          <p className="text-yellow-500 font-semibold text-sm uppercase tracking-wider mb-3">
            TIN TỨC
          </p>
          <h2 className="text-4xl md:text-5xl font-bold text-teal-700 mb-4">
            HOẠT ĐỘNG CỦA BỆNH VIỆN
          </h2>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {news.map((article, index) => (
            <article
              key={article.id}
              className={`bg-white rounded-xl shadow-md hover:shadow-xl transition-all duration-300 overflow-hidden ${
                isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8"
              }`}
              style={{ transitionDelay: `${index * 150}ms` }}
            >
              <div className="relative">
                <img
                  src={article.image}
                  alt={article.title}
                  className="w-full h-48 object-cover"
                />
              </div>

              <div className="p-5">
                <h3 className="text-base font-bold text-teal-700 mb-3 line-clamp-2 min-h-[3rem]">
                  {article.title}
                </h3>

                <div className="flex items-center gap-4 text-xs text-gray-500 mb-3">
                  <div className="flex items-center gap-1">
                    <User className="w-3 h-3 text-yellow-500" />
                    <span>{article.author}</span>
                  </div>
                  <div className="flex items-center gap-1">
                    <Calendar className="w-3 h-3 text-yellow-500" />
                    <span>{article.date}</span>
                  </div>
                </div>

                <p className="text-sm text-gray-600 mb-4 line-clamp-3">
                  {article.excerpt}
                </p>

                <button className="flex items-center gap-2 px-6 py-2 bg-yellow-500 hover:bg-yellow-600 text-white text-sm font-semibold rounded-full transition-all duration-300">
                  <ArrowRight className="w-4 h-4" />
                  ĐỌC THÊM
                </button>
              </div>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
};

export default NewsSection;