import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { Eye, EyeOff, Mail, Lock, Shield, Stethoscope, Users, User } from "lucide-react";

const ModernLogin = ({ role = null, redirectPath = "/" }) => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const { login } = useAuth();
  const navigate = useNavigate();

  // Role configuration
  const roleConfig = {
    admin: {
      title: "Đăng nhập Admin",
      subtitle: "Hệ thống quản lý VisionCare",
      description: "Quản lý toàn bộ hệ thống VisionCare",
      icon: Shield,
      gradient: "from-purple-500 via-purple-600 to-indigo-600",
      image: "https://images.unsplash.com/photo-1486312338219-ce68d2c6f44d?w=800&auto=format&fit=crop",
    },
    doctor: {
      title: "Đăng nhập Doctor",
      subtitle: "Hệ thống quản lý bệnh nhân & lịch khám",
      description: "Quản lý lịch khám và bệnh nhân",
      icon: Stethoscope,
      gradient: "from-emerald-500 via-teal-500 to-cyan-500",
      image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=800&auto=format&fit=crop",
    },
    staff: {
      title: "Đăng nhập Staff",
      subtitle: "Hệ thống hỗ trợ & quản lý đặt lịch",
      description: "Hỗ trợ đặt lịch và chăm sóc khách hàng",
      icon: Users,
      gradient: "from-orange-500 via-amber-500 to-yellow-500",
      image: "https://images.unsplash.com/photo-1497366216548-37526070297c?w=800&auto=format&fit=crop",
    },
    customer: {
      title: "Đăng nhập tài khoản",
      subtitle: "Chào mừng đến với VisionCare",
      description: "Đặt lịch khám và theo dõi sức khỏe",
      icon: User,
      gradient: "from-blue-500 via-blue-600 to-cyan-600",
      image: "https://images.unsplash.com/photo-1556761175-b413da4baf72?w=800&auto=format&fit=crop",
    },
  };

  const config = roleConfig[role] || roleConfig.customer;
  const IconComponent = config.icon;

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
    setError("");
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError("");

    try {
      const result = await login(formData);

      if (result.success) {
        // Small delay to ensure state is updated
        setTimeout(() => {
          navigate(redirectPath);
        }, 100);
      } else {
        setError(result.message || "Đăng nhập thất bại");
      }
    } catch (error) {
      console.error("Login error:", error);
      setError("Có lỗi xảy ra, vui lòng thử lại");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-blue-900 to-slate-900 flex relative overflow-y-auto">
      {/* Background Image */}
      <div className="fixed inset-0 z-0">
        <img 
          src="https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=1920&auto=format&fit=crop"
          alt="Background"
          className="w-full h-full object-cover"
        />
        <div className={`absolute inset-0 bg-gradient-to-br ${config.gradient} mix-blend-multiply opacity-50`}></div>
        <div className="absolute inset-0 bg-gradient-to-br from-slate-900/80 via-blue-900/80 to-slate-900/80"></div>
      </div>

      {/* Animated Background Elements */}
      <div className="fixed inset-0 overflow-hidden z-0">
        <div className="absolute -top-40 -right-40 w-80 h-80 bg-blue-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob"></div>
        <div className="absolute -bottom-40 -left-40 w-80 h-80 bg-purple-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-2000"></div>
        <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-80 h-80 bg-cyan-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-4000"></div>
      </div>

      {/* Center - Login Form */}
      <div className="w-full flex flex-col items-center justify-center p-6 lg:p-12 relative z-10 min-h-screen">
        <div className="w-full max-w-md">
          {/* Glass Card Container */}
          <div className="bg-white/10 backdrop-blur-2xl rounded-2xl shadow-2xl border border-white/20 p-6 lg:p-8">
            
            {/* Logo & Icon */}
            <div className="flex items-center justify-center mb-6">
              <div className={`w-16 h-16 rounded-xl bg-gradient-to-br ${config.gradient} flex items-center justify-center shadow-lg shadow-blue-500/30`}>
                <IconComponent className="w-8 h-8 text-white" />
              </div>
            </div>

            {/* Header */}
            <div className="text-center mb-6">
              <div className="inline-block px-3 py-1 rounded-full bg-white/10 border border-white/20 backdrop-blur-xl mb-3">
                <span className="text-xs font-semibold text-white/90">VisionCare</span>
              </div>
              
              <h1 className="text-2xl lg:text-3xl font-bold text-white mb-1.5">
                {config.title}
              </h1>
              <p className="text-blue-200/80 text-xs">
                {config.subtitle}
              </p>
            </div>

            <form className="space-y-4" onSubmit={handleSubmit}>
              {/* Email Field */}
              <div>
                <label className="block text-xs font-medium text-white/90 mb-1.5 ml-1">
                  Email
                </label>
                <div className="relative group">
                  <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <Mail className="h-4 w-4 text-blue-300/60 group-focus-within:text-blue-400 transition-colors" />
                  </div>
                  <input
                    name="email"
                    type="email"
                    required
                    className="w-full pl-10 pr-4 py-2.5 text-sm bg-white/10 backdrop-blur-xl border border-white/20 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400/50 focus:border-blue-400/50 transition-all text-white placeholder-white/40"
                    placeholder="example@visioncare.com"
                    value={formData.email}
                    onChange={handleChange}
                  />
                </div>
              </div>

              {/* Password Field */}
              <div>
                <label className="block text-xs font-medium text-white/90 mb-1.5 ml-1">
                  Mật khẩu
                </label>
                <div className="relative group">
                  <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <Lock className="h-4 w-4 text-blue-300/60 group-focus-within:text-blue-400 transition-colors" />
                  </div>
                  <input
                    name="password"
                    type={showPassword ? "text" : "password"}
                    required
                    className="w-full pl-10 pr-10 py-2.5 text-sm bg-white/10 backdrop-blur-xl border border-white/20 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400/50 focus:border-blue-400/50 transition-all text-white placeholder-white/40"
                    placeholder="••••••••••••"
                    value={formData.password}
                    onChange={handleChange}
                  />
                  <button
                    type="button"
                    className="absolute inset-y-0 right-0 pr-3 flex items-center text-blue-300/60 hover:text-white transition-colors"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? (
                      <EyeOff className="h-4 w-4" />
                    ) : (
                      <Eye className="h-4 w-4" />
                    )}
                  </button>
                </div>
              </div>

              {/* Remember & Forgot */}
              <div className="flex items-center justify-between text-xs">
                <label className="flex items-center space-x-1.5 cursor-pointer group">
                  <input type="checkbox" className="w-3.5 h-3.5 rounded border-white/20 bg-white/10 text-blue-500 focus:ring-blue-500/50" />
                  <span className="text-white/70 group-hover:text-white transition-colors">Ghi nhớ đăng nhập</span>
                </label>
                <Link to="/forgot-password" className="text-blue-300 hover:text-blue-200 transition-colors font-medium text-xs">
                  Quên mật khẩu?
                </Link>
              </div>

              {/* Error Message */}
              {error && (
                <div className="bg-red-500/20 backdrop-blur-xl border border-red-400/30 text-red-200 px-3 py-2 rounded-lg text-xs animate-shake">
                  {error}
                </div>
              )}

              {/* Submit Button */}
              <button
                type="submit"
                disabled={isLoading}
                className={`w-full py-3 px-4 rounded-lg text-sm font-semibold text-white bg-gradient-to-r ${config.gradient} shadow-lg shadow-blue-500/30 hover:shadow-xl hover:shadow-blue-500/40 transform hover:scale-[1.02] transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none relative overflow-hidden group`}
              >
                <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent translate-x-[-200%] group-hover:translate-x-[200%] transition-transform duration-1000"></div>
                <span className="relative">
                  {isLoading ? (
                    <div className="flex items-center justify-center space-x-2">
                      <svg className="animate-spin h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                        <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                        <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                      </svg>
                      <span className="text-xs">Đang đăng nhập...</span>
                    </div>
                  ) : "Đăng nhập"}
                </span>
              </button>

              {/* Divider */}
              <div className="relative my-4">
                <div className="absolute inset-0 flex items-center">
                  <div className="w-full border-t border-white/10"></div>
                </div>
                <div className="relative flex justify-center text-xs">
                  <span className="px-3 bg-transparent text-white/50">Hoặc đăng nhập với</span>
                </div>
              </div>

              {/* Social Login */}
              <div className="grid grid-cols-2 gap-3">
                <button
                  type="button"
                  className="flex items-center justify-center gap-1.5 py-2.5 px-3 bg-white/10 backdrop-blur-xl border border-white/20 rounded-lg text-xs font-medium text-white hover:bg-white/20 transition-all group"
                >
                  <svg className="w-4 h-4" viewBox="0 0 24 24" fill="currentColor">
                    <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"/>
                  </svg>
                  <span className="group-hover:text-blue-200 transition-colors">Facebook</span>
                </button>
                <button
                  type="button"
                  className="flex items-center justify-center gap-1.5 py-2.5 px-3 bg-white/10 backdrop-blur-xl border border-white/20 rounded-lg text-xs font-medium text-white hover:bg-white/20 transition-all group"
                >
                  <svg className="w-4 h-4" viewBox="0 0 24 24">
                    <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
                    <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
                    <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
                    <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
                  </svg>
                  <span className="group-hover:text-blue-200 transition-colors">Google</span>
                </button>
              </div>
            </form>

            {/* Footer */}
            <div className="mt-6 text-center">
              <p className="text-white/50 text-xs">
                {role ? (
                  <>
                    <Link to="/" className="text-blue-300 hover:text-blue-200 transition-colors">
                      ← Về trang chủ
                    </Link>
                  </>
                ) : (
                  <>
                    Chưa có tài khoản?{" "}
                    <Link to="/register" className="text-blue-300 hover:text-blue-200 transition-colors font-medium">
                      Đăng ký ngay
                    </Link>
                  </>
                )}
              </p>
            </div>
          </div>

          {/* Copyright */}
          <div className="mt-4 text-center">
            <p className="text-white/30 text-xs">
              © 2024 VisionCare. All rights reserved.
            </p>
          </div>
        </div>
      </div>

      <style>{`
        @keyframes blob {
          0%, 100% { transform: translate(0, 0) scale(1); }
          25% { transform: translate(20px, -50px) scale(1.1); }
          50% { transform: translate(-20px, 20px) scale(0.9); }
          75% { transform: translate(50px, 50px) scale(1.05); }
        }
        .animate-blob {
          animation: blob 7s infinite;
        }
        .animation-delay-2000 {
          animation-delay: 2s;
        }
        .animation-delay-4000 {
          animation-delay: 4s;
        }
      `}</style>
    </div>
  );
};

export default ModernLogin;