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
      title: "Cách Bảo Vệ Mắt Khi Làm Việc Với Máy Tính",
      excerpt: "Những mẹo đơn giản giúp giảm mỏi mắt và bảo vệ thị lực khi sử dụng máy tính trong thời gian dài.",
      image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      category: "Chăm sóc mắt",
      author: "BS. Nguyễn Văn An",
      date: "15/12/2024",
      readTime: "5 phút đọc"
    },
    {
      id: 2,
      title: "Phẫu Thuật Lasik: Những Điều Cần Biết",
      excerpt: "Tìm hiểu về quy trình, chỉ định và những lưu ý quan trọng trước khi quyết định phẫu thuật Lasik.",
      image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      category: "Phẫu thuật",
      author: "BS. Trần Thị Bình",
      date: "12/12/2024",
      readTime: "8 phút đọc"
    },
    {
      id: 3,
      title: "Tầm Soát Mắt Định Kỳ: Tại Sao Quan Trọng?",
      excerpt: "Khám mắt định kỳ giúp phát hiện sớm các bệnh lý về mắt và bảo vệ thị lực tốt nhất.",
      image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
      category: "Sức khỏe",
      author: "BS. Lê Minh Châu",
      date: "10/12/2024",
      readTime: "6 phút đọc"
    }
  ];

  return (
    <section id="news-section" className="py-20 bg-white">
      <div className="container mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Cập Nhật Kiến Thức Sức Khỏe Mắt
          </h2>
          <p className="text-xl text-gray-600 max-w-3xl mx-auto">
            Những bài viết chuyên môn và mẹo chăm sóc mắt từ đội ngũ bác sĩ VisionCare
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 mb-12">
          {news.map((article, index) => (
            <article
              key={article.id}
              className={`bg-white rounded-2xl shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-105 overflow-hidden ${
                isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8"
              }`}
              style={{ transitionDelay: `${index * 200}ms` }}
            >
              <div className="relative">
                <img
                  src={article.image}
                  alt={article.title}
                  className="w-full h-48 object-cover"
                />
                <div className="absolute top-4 left-4">
                  <span className="bg-blue-500 text-white px-3 py-1 rounded-full text-sm font-medium">
                    {article.category}
                  </span>
                </div>
              </div>

              <div className="p-6">
                <h3 className="text-xl font-bold text-gray-900 mb-3 line-clamp-2">
                  {article.title}
                </h3>
                <p className="text-gray-600 mb-4 line-clamp-3">
                  {article.excerpt}
                </p>

                <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
                  <div className="flex items-center space-x-4">
                    <div className="flex items-center space-x-1">
                      <User className="w-4 h-4" />
                      <span>{article.author}</span>
                    </div>
                    <div className="flex items-center space-x-1">
                      <Calendar className="w-4 h-4" />
                      <span>{article.date}</span>
                    </div>
                  </div>
                  <span className="text-orange-500 font-medium">
                    {article.readTime}
                  </span>
                </div>

                <a
                  href={`/news/${article.id}`}
                  className="inline-flex items-center text-blue-600 hover:text-blue-700 font-medium group"
                >
                  Đọc tiếp
                  <ArrowRight className="w-4 h-4 ml-1 group-hover:translate-x-1 transition-transform" />
                </a>
              </div>
            </article>
          ))}
        </div>

        <div className="text-center">
          <a
            href="/news"
            className="inline-flex items-center px-8 py-4 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-full transition-all duration-300 transform hover:scale-105 shadow-lg"
          >
            Xem Tất Cả Bài Viết
            <ArrowRight className="w-5 h-5 ml-2" />
          </a>
        </div>
      </div>
    </section>
  );
};

export default NewsSection;
