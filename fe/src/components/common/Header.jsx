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
} from "lucide-react";
import { useAuth } from "../../contexts/AuthContext";

const Header = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isScrolled, setIsScrolled] = useState(false);
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  const { isAuthenticated, user, logout, hasRole } = useAuth();

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 20);
    };
    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  // Close user menu when clicking outside
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
    { name: "Trang Chủ", href: "/" },
    { name: "Dịch Vụ", href: "/services" },
    { name: "Trang Thiết Bị", href: "/equipment" },
    { name: "Đặt Lịch", href: "/booking" },
    { name: "Liên Hệ", href: "/contact" },
  ];

  const handleLogout = async () => {
    await logout();
    setIsUserMenuOpen(false);
    setIsMenuOpen(false);
  };

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
              <span className="text-sm font-medium text-green-600 -mt-1">
                Care
              </span>
            </div>
          </div>

          {/* Desktop Navigation */}
          <nav className="hidden lg:flex items-center space-x-8">
            {navigation.map((item) => (
              <Link
                key={item.name}
                to={item.href}
                className="text-gray-700 hover:text-blue-600 font-medium transition-colors duration-200"
              >
                {item.name}
              </Link>
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

            {isAuthenticated ? (
              // User menu
              <div className="relative user-menu">
                <button
                  onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
                  className="flex items-center space-x-2 text-gray-700 hover:text-blue-600 transition-colors"
                >
                  <div className="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center">
                    <User className="w-4 h-4 text-blue-600" />
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
                        <Shield className="w-4 h-4 mr-2 text-blue-600" />
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
                      to="/profile"
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
                  className="text-gray-700 hover:text-blue-600 font-medium transition-colors duration-200"
                >
                  Đăng nhập
                </Link>
                <Link
                  to="/register"
                  className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-full font-medium transition-colors duration-200"
                >
                  Đăng ký
                </Link>
              </div>
            )}

            <Link
              to="/booking"
              className="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-full font-medium transition-colors duration-200 flex items-center space-x-2"
            >
              <Calendar className="w-4 h-4" />
              <span>Đặt Lịch Ngay</span>
            </Link>
          </div>

          {/* Mobile menu button */}
          <button
            className="lg:hidden p-2 rounded-md text-gray-600 hover:text-blue-600 hover:bg-gray-100"
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
              {navigation.map((item) => (
                <Link
                  key={item.name}
                  to={item.href}
                  className="text-gray-700 hover:text-blue-600 font-medium py-2 transition-colors duration-200"
                  onClick={() => setIsMenuOpen(false)}
                >
                  {item.name}
                </Link>
              ))}
              <div className="pt-4 border-t border-gray-200">
                <a
                  href="tel:1900xxxx"
                  className="flex items-center space-x-2 text-gray-600 hover:text-blue-600 py-2"
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
                        className="flex items-center space-x-2 text-gray-700 hover:text-blue-600 py-2 border-b border-gray-200"
                        onClick={() => setIsMenuOpen(false)}
                      >
                        <Shield className="w-4 h-4 text-blue-600" />
                        <span className="font-medium">Quản lý Admin</span>
                      </Link>
                    )}

                    {/* Doctor Management Link */}
                    {hasRole && hasRole("doctor") && (
                      <Link
                        to="/doctor"
                        className="flex items-center space-x-2 text-gray-700 hover:text-blue-600 py-2 border-b border-gray-200"
                        onClick={() => setIsMenuOpen(false)}
                      >
                        <Shield className="w-4 h-4 text-green-600" />
                        <span className="font-medium">Quản lý Bác sĩ</span>
                      </Link>
                    )}

                    <Link
                      to="/profile"
                      className="flex items-center space-x-2 text-gray-700 hover:text-blue-600 py-2"
                      onClick={() => setIsMenuOpen(false)}
                    >
                      <Settings className="w-4 h-4" />
                      <span>Cài đặt tài khoản</span>
                    </Link>
                    <button
                      onClick={handleLogout}
                      className="flex items-center space-x-2 text-gray-700 hover:text-blue-600 py-2 w-full text-left"
                    >
                      <LogOut className="w-4 h-4" />
                      <span>Đăng xuất</span>
                    </button>
                  </div>
                ) : (
                  <div className="space-y-2">
                    <Link
                      to="/login"
                      className="block text-gray-700 hover:text-blue-600 font-medium py-2 text-center"
                      onClick={() => setIsMenuOpen(false)}
                    >
                      Đăng nhập
                    </Link>
                    <Link
                      to="/register"
                      className="block bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-full font-medium text-center transition-colors duration-200"
                      onClick={() => setIsMenuOpen(false)}
                    >
                      Đăng ký
                    </Link>
                  </div>
                )}

                <Link
                  to="/booking"
                  className="block bg-orange-500 hover:bg-orange-600 text-white px-4 py-2 rounded-full font-medium text-center mt-2 transition-colors duration-200"
                  onClick={() => setIsMenuOpen(false)}
                >
                  Đặt Lịch Ngay
                </Link>
              </div>
            </nav>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;
