import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import {
  Menu,
  X,
  Phone,
  Calendar,
  User,
  LogOut,
  Settings,
  Shield,
  MapPin,
  Mail,
  Facebook,
  Youtube,
  Search,
  History,
} from "lucide-react";
import { useAuth } from "../../contexts/AuthContext";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  const { isAuthenticated, user, logout, hasRole } = useAuth();

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 40);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (isUserMenuOpen && !event.target.closest(".user-menu")) {
        setIsUserMenuOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [isUserMenuOpen]);

  const navigation = [
    { name: "TRANG CHỦ", href: "/" },
    { name: "VỀ MẮT VisionCare", href: "/about" },
    { name: "DỊCH VỤ", href: "/services" },
    { name: "ĐỘI NGŨ BÁC SĨ", href: "/#doctors-section" },
    { name: "TIN TỨC", href: "/blogs" },
  ];

  const handleLogout = async () => {
    await logout();
    setIsUserMenuOpen(false);
    setIsMenuOpen(false);
  };

  const getProfilePath = () => {
    const role = user?.roleName?.toLowerCase();
    if (role === "admin") return "/admin/profile";
    if (role === "doctor") return "/doctor/profile";
    if (role === "staff") return "/staff/profile";
    return "/customer/profile"; // Default to customer
  };

  return (
    <header className={`w-full z-50 ${isScrolled ? "fixed top-0" : "relative"}`}>
      {/* Top Bar - Xanh đậm với thông tin liên hệ - Chỉ hiển thị khi không sticky */}
      {!isScrolled && (
        <div className="bg-[#0c5a8a] text-white py-2.5 hidden md:block">
          <div className="container mx-auto px-4">
            <div className="flex items-center justify-between text-sm">
              <div className="flex items-center space-x-6">
                <div className="flex items-center space-x-2">
                  <MapPin className="w-4 h-4 flex-shrink-0" />
                  <span>33 Nguyễn Quốc Trị, Trung Hòa, Cầu Giấy, Hà Nội</span>
                </div>
                <div className="flex items-center space-x-2">
                  <Mail className="w-4 h-4 flex-shrink-0" />
                  <span>info@matanhduong.com</span>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <a href="#" className="w-7 h-7 bg-white rounded-full flex items-center justify-center hover:bg-gray-100 transition-colors">
                  <Facebook className="w-4 h-4 text-[#0c5a8a]" />
                </a>
                <a href="#" className="w-7 h-7 bg-white rounded-full flex items-center justify-center hover:bg-gray-100 transition-colors">
                  <Youtube className="w-4 h-4 text-[#0c5a8a]" />
                </a>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Main Navigation Bar - Trắng */}
      <div
        className={`w-full transition-all duration-300 ${
          isScrolled
            ? "bg-white/95 backdrop-blur-md shadow-lg"
            : "bg-white"
        }`}
      >
        <div className="container mx-auto px-4">
          <div className="flex items-center justify-between h-16 lg:h-20">
            {/* Logo */}
            <div className="flex items-center space-x-3">
              <div className="w-14 h-14 bg-gradient-to-br from-yellow-400 via-yellow-500 to-orange-500 rounded-full flex items-center justify-center shadow-lg relative">
                {/* Sun rays */}
                <div className="absolute inset-0">
                  {[...Array(12)].map((_, i) => (
                    <div
                      key={i}
                      className="absolute top-1/2 left-1/2 w-1 h-4 bg-yellow-400"
                      style={{
                        transform: `translate(-50%, -50%) rotate(${i * 30}deg) translateY(-20px)`,
                        transformOrigin: 'center'
                      }}
                    />
                  ))}
                </div>
                {/* Center circle */}
                <div className="w-6 h-6 bg-white rounded-full z-10"></div>
              </div>
              <div className="flex flex-col leading-tight">
                <span className="text-2xl font-bold text-[#0c5a8a] tracking-wide">VisionCare</span>
                <span className="text-xs font-semibold text-[#0c5a8a] tracking-widest">
                  EYE HOSPITAL
                </span>
              </div>
            </div>

            {/* Desktop Navigation */}
            <nav className="hidden lg:flex items-center space-x-8">
              {navigation.map((item) => {
                if (item.href.startsWith('/#')) {
                  return (
                    <a
                      key={item.name}
                      href={item.href}
                      onClick={(e) => {
                        e.preventDefault();
                        const hash = item.href.split('#')[1];
                        const element = document.getElementById(hash);
                        if (element) {
                          const headerHeight = isScrolled ? 80 : 120;
                          const elementPosition = element.getBoundingClientRect().top + window.pageYOffset;
                          const offsetPosition = elementPosition - headerHeight;
                          window.scrollTo({
                            top: offsetPosition,
                            behavior: 'smooth'
                          });
                        }
                      }}
                      className="text-gray-700 hover:text-[#0c5a8a] font-medium text-sm transition-colors duration-200 uppercase"
                    >
                      {item.name}
                    </a>
                  );
                }
                return (
                  <Link
                    key={item.name}
                    to={item.href}
                    className="text-gray-700 hover:text-[#0c5a8a] font-medium text-sm transition-colors duration-200 uppercase"
                  >
                    {item.name}
                  </Link>
                );
              })}
            </nav>

            {/* CTA Buttons */}
            <div className="hidden lg:flex items-center space-x-4">
              <button className="text-gray-700 hover:text-[#0c5a8a] transition-colors">
                <Search className="w-5 h-5" />
              </button>

              {isAuthenticated ? (
                // User menu
                <div className="relative user-menu">
                  <button
                    onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
                    className="flex items-center space-x-2 text-gray-700 hover:text-[#0c5a8a] transition-colors"
                  >
                    <div className="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
                      <User className="w-4 h-4 text-[#0c5a8a]" />
                    </div>
                    <span className="text-sm font-medium">
                      {user?.username || user?.email || "User"}
                    </span>
                  </button>

                  {isUserMenuOpen && (
                    <div className="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-50 border border-gray-200">
                      <div className="px-4 py-2 text-sm text-gray-500 border-b border-gray-200">
                        {user?.email || user?.username}
                      </div>

                      {/* Admin Management Link */}
                      {hasRole && hasRole("admin") && (
                        <Link
                          to="/admin"
                          className="flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 border-b border-gray-200"
                          onClick={() => setIsUserMenuOpen(false)}
                        >
                          <Shield className="w-4 h-4 mr-2 text-[#0c5a8a]" />
                          <span className="font-medium">Quản lý Admin</span>
                        </Link>
                      )}

                      {/* Doctor Management Link */}
                      {hasRole && hasRole("doctor") && (
                        <Link
                          to="/doctor"
                          className="flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 border-b border-gray-200"
                          onClick={() => setIsUserMenuOpen(false)}
                        >
                          <Shield className="w-4 h-4 mr-2 text-green-600" />
                          <span className="font-medium">Quản lý Bác sĩ</span>
                        </Link>
                      )}

                      <Link
                        to="/customer/history"
                        className="flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 border-b border-gray-200"
                        onClick={() => setIsUserMenuOpen(false)}
                      >
                        <History className="w-4 h-4 mr-2 text-yellow-500" />
                        Lịch sử khám
                      </Link>

                      <Link
                        to={getProfilePath()}
                        className="flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                        onClick={() => setIsUserMenuOpen(false)}
                      >
                        <Settings className="w-4 h-4 mr-2" />
                        Cài đặt tài khoản
                      </Link>
                      <button
                        onClick={handleLogout}
                        className="flex items-center w-full px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                      >
                        <LogOut className="w-4 h-4 mr-2" />
                        Đăng xuất
                      </button>
                    </div>
                  )}
                </div>
              ) : (
                // Login/Register buttons
                <div className="flex items-center space-x-2">
                  <Link
                    to="/login"
                    className="text-gray-700 hover:text-[#0c5a8a] font-medium transition-colors duration-200"
                  >
                    Đăng nhập
                  </Link>
                  <Link
                    to="/register"
                    className="bg-[#0c5a8a] hover:bg-[#094a73] text-white px-4 py-2 rounded-full font-medium transition-colors duration-200"
                  >
                    Đăng ký
                  </Link>
                </div>
              )}

              <Link
                to="/booking"
                className="bg-gradient-to-r from-yellow-400 to-orange-500 hover:from-yellow-500 hover:to-orange-600 text-white px-6 py-2.5 rounded-full font-semibold transition-all duration-200 flex items-center space-x-2 shadow-md uppercase text-sm"
              >
                <Calendar className="w-4 h-4" />
                <span>ĐẶT LỊCH</span>
              </Link>
            </div>

            {/* Mobile menu button */}
            <button
              className="lg:hidden p-2 rounded-md text-gray-600 hover:text-[#0c5a8a] hover:bg-gray-100"
              onClick={() => setIsMenuOpen(!isMenuOpen)}
            >
              {isMenuOpen ? (
                <X className="w-6 h-6" />
              ) : (
                <Menu className="w-6 h-6" />
              )}
            </button>
          </div>

          {/* Mobile Navigation */}
          {isMenuOpen && (
            <div className="lg:hidden py-4 border-t border-gray-200">
              <nav className="flex flex-col space-y-4">
                {navigation.map((item) => {
                  if (item.href.startsWith('/#')) {
                    return (
                      <a
                        key={item.name}
                        href={item.href}
                        onClick={(e) => {
                          e.preventDefault();
                          setIsMenuOpen(false);
                          const hash = item.href.split('#')[1];
                          const element = document.getElementById(hash);
                          if (element) {
                            const headerHeight = isScrolled ? 80 : 120;
                            const elementPosition = element.getBoundingClientRect().top + window.pageYOffset;
                            const offsetPosition = elementPosition - headerHeight;
                            window.scrollTo({
                              top: offsetPosition,
                              behavior: 'smooth'
                            });
                          }
                        }}
                        className="text-gray-700 hover:text-[#0c5a8a] font-medium py-2 transition-colors duration-200"
                      >
                        {item.name}
                      </a>
                    );
                  }
                  return (
                    <Link
                      key={item.name}
                      to={item.href}
                      className="text-gray-700 hover:text-[#0c5a8a] font-medium py-2 transition-colors duration-200"
                      onClick={() => setIsMenuOpen(false)}
                    >
                      {item.name}
                    </Link>
                  );
                })}
                <div className="pt-4 border-t border-gray-200">
                  <a
                    href="tel:1900xxxx"
                    className="flex items-center space-x-2 text-gray-600 hover:text-[#0c5a8a] py-2"
                  >
                    <Phone className="w-4 h-4" />
                    <span>1900-xxxx</span>
                  </a>

                  {isAuthenticated ? (
                    <div className="space-y-2">
                      <div className="px-2 py-1 text-sm text-gray-500">
                        Xin chào, {user?.username || user?.email || "User"}!
                      </div>

                      {/* Admin Management Link */}
                      {hasRole && hasRole("admin") && (
                        <Link
                          to="/admin"
                          className="flex items-center space-x-2 text-gray-700 hover:text-[#0c5a8a] py-2 border-b border-gray-200"
                          onClick={() => setIsMenuOpen(false)}
                        >
                          <Shield className="w-4 h-4 text-[#0c5a8a]" />
                          <span className="font-medium">Quản lý Admin</span>
                        </Link>
                      )}

                      {/* Doctor Management Link */}
                      {hasRole && hasRole("doctor") && (
                        <Link
                          to="/doctor"
                          className="flex items-center space-x-2 text-gray-700 hover:text-[#0c5a8a] py-2 border-b border-gray-200"
                          onClick={() => setIsMenuOpen(false)}
                        >
                          <Shield className="w-4 h-4 text-green-600" />
                          <span className="font-medium">Quản lý Bác sĩ</span>
                        </Link>
                      )}

                      <Link
                        to="/customer/history"
                        className="flex items-center space-x-2 text-gray-700 hover:text-[#0c5a8a] py-2 border-b border-gray-200"
                        onClick={() => setIsMenuOpen(false)}
                      >
                        <History className="w-4 h-4 text-yellow-500" />
                        <span>Lịch sử khám</span>
                      </Link>

                      <Link
                        to={getProfilePath()}
                        className="flex items-center space-x-2 text-gray-700 hover:text-[#0c5a8a] py-2"
                        onClick={() => setIsMenuOpen(false)}
                      >
                        <Settings className="w-4 h-4" />
                        <span>Cài đặt tài khoản</span>
                      </Link>
                      <button
                        onClick={handleLogout}
                        className="flex items-center space-x-2 text-gray-700 hover:text-[#0c5a8a] py-2 w-full text-left"
                      >
                        <LogOut className="w-4 h-4" />
                        <span>Đăng xuất</span>
                      </button>
                    </div>
                  ) : (
                    <div className="space-y-2">
                      <Link
                        to="/login"
                        className="block text-gray-700 hover:text-[#0c5a8a] font-medium py-2 text-center"
                        onClick={() => setIsMenuOpen(false)}
                      >
                        Đăng nhập
                      </Link>
                      <Link
                        to="/register"
                        className="block bg-[#0c5a8a] hover:bg-[#094a73] text-white px-4 py-2 rounded-full font-medium text-center transition-colors duration-200"
                        onClick={() => setIsMenuOpen(false)}
                      >
                        Đăng ký
                      </Link>
                    </div>
                  )}

                  <Link
                    to="/booking"
                    className="block bg-gradient-to-r from-yellow-400 to-orange-500 hover:from-yellow-500 hover:to-orange-600 text-white px-4 py-2 rounded-full font-semibold text-center mt-2 transition-all duration-200 shadow-md uppercase"
                    onClick={() => setIsMenuOpen(false)}
                  >
                    ĐẶT LỊCH
                  </Link>
                </div>
              </nav>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;