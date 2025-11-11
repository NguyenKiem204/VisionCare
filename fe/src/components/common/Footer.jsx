import React from "react";
import { MapPin, Phone, Mail, Clock, Facebook, Youtube, Instagram, MessageCircle } from "lucide-react";

const Footer = () => {
  return (
    <footer className="bg-gray-900 text-white relative z-20">
      {/* Main Footer */}
      <div className="container mx-auto px-4 py-16">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {/* VisionCare Info */}
          <div className="space-y-6">
            <div className="flex items-center space-x-2">
              <div className="w-12 h-12 bg-gradient-to-br from-blue-500 to-green-500 rounded-full flex items-center justify-center">
                <div className="w-7 h-7 relative">
                  <div className="absolute inset-0 rounded-full border-2 border-white"></div>
                  <div className="absolute top-1 left-1 w-2 h-2 bg-white rounded-full"></div>
                  <div className="absolute top-1 right-1 w-1 h-1 bg-white rounded-full"></div>
                </div>
              </div>
              <div className="flex flex-col">
                <span className="text-2xl font-bold">Vision</span>
                <span className="text-sm font-medium text-green-400 -mt-1">Care</span>
              </div>
            </div>
            <p className="text-gray-300 text-sm leading-relaxed">
              Chăm sóc mắt chuyên nghiệp - Công nghệ tiên tiến
            </p>
            <p className="text-gray-400 text-xs">
              20+ năm kinh nghiệm trong lĩnh vực nhãn khoa với đội ngũ bác sĩ chuyên môn cao và thiết bị hiện đại nhất.
            </p>
            <div className="flex space-x-4">
              <a href="#" className="text-gray-400 hover:text-blue-400 transition-colors">
                <Facebook className="w-5 h-5" />
              </a>
              <a href="#" className="text-gray-400 hover:text-blue-400 transition-colors">
                <Youtube className="w-5 h-5" />
              </a>
              <a href="#" className="text-gray-400 hover:text-blue-400 transition-colors">
                <MessageCircle className="w-5 h-5" />
              </a>
              <a href="#" className="text-gray-400 hover:text-blue-400 transition-colors">
                <Instagram className="w-5 h-5" />
              </a>
            </div>
          </div>

          {/* Contact Info */}
          <div className="space-y-6">
            <h3 className="text-lg font-semibold">Liên Hệ</h3>
            <div className="space-y-4">
              <div className="flex items-start space-x-3">
                <MapPin className="w-5 h-5 text-blue-400 mt-0.5 flex-shrink-0" />
                <div>
                  <p className="text-sm font-medium">Cơ sở 1:</p>
                  <p className="text-gray-300 text-sm">123 Đường ABC, Quận 1, TP.HCM</p>
                  <p className="text-sm font-medium mt-2">Cơ sở 2:</p>
                  <p className="text-gray-300 text-sm">456 Đường DEF, Quận Ba Đình, Hà Nội</p>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <Phone className="w-5 h-5 text-blue-400 flex-shrink-0" />
                <div>
                  <p className="text-sm font-medium">Tổng đài: 1900-xxxx (miễn phí)</p>
                  <p className="text-gray-300 text-sm">Khẩn cấp 24/7: 0909-xxx-xxx</p>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <Mail className="w-5 h-5 text-blue-400 flex-shrink-0" />
                <div>
                  <p className="text-sm font-medium">Tư vấn: tuvan@visioncare.vn</p>
                  <p className="text-gray-300 text-sm">Hỗ trợ: hotro@visioncare.vn</p>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <Clock className="w-5 h-5 text-blue-400 flex-shrink-0" />
                <div>
                  <p className="text-sm font-medium">T2-T6: 8:00 - 17:30</p>
                  <p className="text-gray-300 text-sm">T7: 8:00 - 12:00 | CN: Nghỉ (trừ cấp cứu)</p>
                </div>
              </div>
            </div>
          </div>

          {/* Quick Services */}
          <div className="space-y-6">
            <h3 className="text-lg font-semibold">Dịch Vụ Nhanh</h3>
            <div className="space-y-3">
              <a href="/services" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Khám mắt tổng quát
              </a>
              <a href="/services" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Phẫu thuật Lasik
              </a>
              <a href="/services" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Điều trị glaucoma
              </a>
              <a href="/services" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Nhãn khoa trẻ em
              </a>
              <a href="/services" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Cấp cứu mắt 24/7
              </a>
              <a href="/services" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Tư vấn online
              </a>
              <a href="/services" className="block text-blue-400 hover:text-blue-300 text-sm font-medium mt-3">
                Xem tất cả dịch vụ →
              </a>
            </div>
          </div>

          {/* Support */}
          <div className="space-y-6">
            <h3 className="text-lg font-semibold">Hỗ Trợ</h3>
            <div className="space-y-3">
              <a href="/booking" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Đặt lịch online
              </a>
              <a href="#" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Hướng dẫn chuẩn bị khám
              </a>
              <a href="#" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Chính sách bảo hành
              </a>
              <a href="#" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Bảo hiểm y tế
              </a>
              <a href="#" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Câu hỏi thường gặp
              </a>
              <a href="#" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Chính sách bảo mật
              </a>
              <a href="#" className="block text-gray-300 hover:text-blue-400 text-sm transition-colors">
                Điều khoản sử dụng
              </a>
            </div>
          </div>
        </div>
      </div>

      {/* Newsletter Section */}
      {/* <div className="bg-gray-800 py-12">
        <div className="container mx-auto px-4">
          <div className="max-w-2xl mx-auto text-center">
            <h3 className="text-2xl font-semibold mb-3">Đăng Ký Nhận Tin Sức Khỏe Mắt</h3>
            <p className="text-gray-300 text-sm mb-8">Nhận tips chăm sóc mắt và ưu đãi đặc biệt</p>
            <div className="flex flex-col sm:flex-row gap-4 max-w-md mx-auto">
              <input
                type="email"
                placeholder="Nhập email của bạn"
                className="flex-1 px-4 py-3 bg-gray-700 border border-gray-600 rounded-lg text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
              <button className="bg-blue-500 hover:bg-blue-600 text-white px-6 py-3 rounded-lg font-medium transition-colors duration-200">
                Đăng Ký
              </button>
            </div>
          </div>
        </div>
      </div> */}

      {/* Bottom Bar */}
      <div className="bg-gray-950 py-6">
        <div className="container mx-auto px-4">
          <div className="flex flex-col md:flex-row justify-between items-center space-y-2 md:space-y-0">
            <p className="text-gray-400 text-sm">© 2024 VisionCare. All rights reserved.</p>
            <p className="text-gray-400 text-sm">Thiết kế bởi VisionCare Team</p>
            <div className="flex space-x-4">
              <a href="#" className="text-gray-400 hover:text-blue-400 text-sm transition-colors">
                Chính sách bảo mật
              </a>
              <span className="text-gray-600">|</span>
              <a href="#" className="text-gray-400 hover:text-blue-400 text-sm transition-colors">
                Điều khoản sử dụng
              </a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
