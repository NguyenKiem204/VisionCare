import React, { useState, useEffect } from "react";
import { Menu, X, Phone, Calendar } from "lucide-react";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const navigation = [
    { name: "Trang Chủ", href: "/" },
    { name: "Dịch Vụ", href: "/services" },
    { name: "Trang Thiết Bị", href: "/equipment" },
    { name: "Đặt Lịch", href: "/booking" },
    { name: "Liên Hệ", href: "/contact" },
  ];

  return (
    <header
      className={`w-full z-50 transition-all duration-300 ${
        isScrolled
          ? "fixed top-0 bg-white/95 backdrop-blur-md shadow-lg"
          : "relative bg-white"
      }`}
    >
      <div className="container mx-auto px-4">
        <div className="flex items-center justify-between h-16 lg:h-20">
          {/* Logo */}
          <div className="flex items-center space-x-2">
            <div className="w-10 h-10 bg-gradient-to-br from-blue-500 to-green-500 rounded-full flex items-center justify-center">
              <div className="w-6 h-6 relative">
                <div className="absolute inset-0 rounded-full border-2 border-white"></div>
                <div className="absolute top-1 left-1 w-2 h-2 bg-white rounded-full"></div>
                <div className="absolute top-1 right-1 w-1 h-1 bg-white rounded-full"></div>
              </div>
            </div>
            <div className="flex flex-col">
              <span className="text-xl font-bold text-blue-800">Vision</span>
              <span className="text-sm font-medium text-green-600 -mt-1">Care</span>
            </div>
          </div>

          {/* Desktop Navigation */}
          <nav className="hidden lg:flex items-center space-x-8">
            {navigation.map((item) => (
              <a
                key={item.name}
                href={item.href}
                className="text-gray-700 hover:text-blue-600 font-medium transition-colors duration-200"
              >
                {item.name}
              </a>
            ))}
          </nav>

          {/* CTA Buttons */}
          <div className="hidden lg:flex items-center space-x-4">
            <a
              href="tel:1900xxxx"
              className="flex items-center space-x-2 text-gray-600 hover:text-blue-600 transition-colors"
            >
              <Phone className="w-4 h-4" />
              <span className="text-sm font-medium">1900-xxxx</span>
            </a>
            <a
              href="/booking"
              className="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-full font-medium transition-colors duration-200 flex items-center space-x-2"
            >
              <Calendar className="w-4 h-4" />
              <span>Đặt Lịch Ngay</span>
            </a>
          </div>

          {/* Mobile menu button */}
          <button
            className="lg:hidden p-2 rounded-md text-gray-600 hover:text-blue-600 hover:bg-gray-100"
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          >
            {isMenuOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
          </button>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <div className="lg:hidden py-4 border-t border-gray-200">
            <nav className="flex flex-col space-y-4">
              {navigation.map((item) => (
                <a
                  key={item.name}
                  href={item.href}
                  className="text-gray-700 hover:text-blue-600 font-medium py-2 transition-colors duration-200"
                  onClick={() => setIsMenuOpen(false)}
                >
                  {item.name}
                </a>
              ))}
              <div className="pt-4 border-t border-gray-200">
                <a
                  href="tel:1900xxxx"
                  className="flex items-center space-x-2 text-gray-600 hover:text-blue-600 py-2"
                >
                  <Phone className="w-4 h-4" />
                  <span>1900-xxxx</span>
                </a>
                <a
                  href="/booking"
                  className="block bg-orange-500 hover:bg-orange-600 text-white px-4 py-2 rounded-full font-medium text-center mt-2 transition-colors duration-200"
                >
                  Đặt Lịch Ngay
                </a>
              </div>
            </nav>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;
