import React from "react";
import { Calendar, Phone } from "lucide-react";

const CallToAction = () => {
  return (
    <section className="relative py-20">
      {/* dark translucent layer over the page background */}
      <div aria-hidden className="absolute inset-0 bg-black/60" />
      <div className="relative z-10 container mx-auto px-4">
        <div className="text-center">
          <h2 className="text-4xl md:text-5xl font-bold text-white mb-6">
            Hãy để VisionCare chăm sóc đôi mắt của bạn
          </h2>
          <p className="text-xl text-white/90 mb-12 max-w-3xl mx-auto">
            Đặt lịch khám ngay hôm nay để được tư vấn miễn phí và chăm sóc
            chuyên nghiệp
          </p>

          <div className="flex flex-col sm:flex-row gap-6 justify-center items-center">
            <a
              href="/booking"
              className="inline-flex items-center justify-center px-8 py-4 bg-orange-500 hover:bg-orange-600 text-white font-bold text-lg rounded-full transition-all duration-300 transform hover:scale-105 shadow-xl"
            >
              <Calendar className="w-6 h-6 mr-3" />
              Đặt Lịch Khám Ngay
            </a>

            <a
              href="tel:1900xxxx"
              className="inline-flex items-center justify-center px-8 py-4 bg-white/15 hover:bg-white/25 text-white font-bold text-lg rounded-full border-2 border-white/40 transition-all duration-300 backdrop-blur-sm"
            >
              <Phone className="w-6 h-6 mr-3" />
              Hotline: 1900-xxxx
            </a>
          </div>

          <div className="mt-12 grid grid-cols-1 md:grid-cols-3 gap-8 text-center">
            <div className="text-white">
              <div className="text-3xl font-bold mb-2">24/7</div>
              <div className="text-white/85">Hỗ trợ khẩn cấp</div>
            </div>
            <div className="text-white">
              <div className="text-3xl font-bold mb-2">99.8%</div>
              <div className="text-white/85">Tỷ lệ thành công</div>
            </div>
            <div className="text-white">
              <div className="text-3xl font-bold mb-2">20+</div>
              <div className="text-white/85">Năm kinh nghiệm</div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default CallToAction;
