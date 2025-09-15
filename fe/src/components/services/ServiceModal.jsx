import React from "react";
import { X, Clock, DollarSign, User, CheckCircle, Calendar } from "lucide-react";

const ServiceModal = ({ service, isOpen, onClose }) => {
  if (!isOpen || !service) return null;

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-3xl max-w-6xl w-full max-h-[90vh] overflow-y-auto">
        <div className="p-8">
          {/* Modal Header */}
          <div className="flex items-start justify-between mb-8">
            <div>
              <h2 className="text-3xl font-bold text-gray-900 mb-2">
                {service.name}
              </h2>
              {service.badge && (
                <span className="inline-block px-4 py-2 bg-orange-100 text-orange-600 text-sm font-bold rounded-full">
                  {service.badge}
                </span>
              )}
            </div>
            <button
              onClick={onClose}
              className="p-3 hover:bg-gray-100 rounded-full transition-colors"
            >
              <X className="w-6 h-6" />
            </button>
          </div>

          {/* Modal Content */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
            {/* Left Column - Service Info */}
            <div className="space-y-8">
              <div>
                <h3 className="text-2xl font-semibold text-gray-900 mb-4">Mô tả dịch vụ</h3>
                <p className="text-gray-600 leading-relaxed text-lg">
                  {service.description}
                </p>
              </div>

              <div className="grid grid-cols-2 gap-6">
                {service.duration && (
                  <div className="bg-blue-50 p-6 rounded-2xl">
                    <Clock className="w-8 h-8 text-blue-600 mb-3" />
                    <h4 className="font-semibold text-gray-900 mb-1">Thời gian</h4>
                    <p className="text-gray-600">{service.duration}</p>
                  </div>
                )}
                <div className="bg-green-50 p-6 rounded-2xl">
                  <DollarSign className="w-8 h-8 text-green-600 mb-3" />
                  <h4 className="font-semibold text-gray-900 mb-1">Giá dịch vụ</h4>
                  <p className="text-green-600 font-bold text-lg">{service.price}</p>
                </div>
                <div className="bg-purple-50 p-6 rounded-2xl">
                  <User className="w-8 h-8 text-purple-600 mb-3" />
                  <h4 className="font-semibold text-gray-900 mb-1">Bác sĩ phụ trách</h4>
                  <p className="text-gray-600">{service.doctor}</p>
                </div>
              </div>

              {service.features && (
                <div>
                  <h3 className="text-xl font-semibold text-gray-900 mb-4">Tính năng nổi bật</h3>
                  <ul className="space-y-3">
                    {service.features.map((feature, index) => (
                      <li key={index} className="flex items-center space-x-3">
                        <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
                        <span className="text-gray-600">{feature}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>

            {/* Right Column - Additional Info */}
            <div className="space-y-8">
              <div className="bg-gray-50 p-8 rounded-2xl">
                <h3 className="text-xl font-semibold text-gray-900 mb-6">Lưu ý quan trọng</h3>
                <ul className="space-y-4 text-gray-600">
                  <li className="flex items-start space-x-3">
                    <div className="w-2 h-2 bg-orange-500 rounded-full mt-2"></div>
                    <span>Đặt lịch trước ít nhất 24 giờ</span>
                  </li>
                  <li className="flex items-start space-x-3">
                    <div className="w-2 h-2 bg-orange-500 rounded-full mt-2"></div>
                    <span>Mang theo CMND/CCCD khi khám</span>
                  </li>
                  <li className="flex items-start space-x-3">
                    <div className="w-2 h-2 bg-orange-500 rounded-full mt-2"></div>
                    <span>Không đeo kính áp tròng 24h trước khám</span>
                  </li>
                  <li className="flex items-start space-x-3">
                    <div className="w-2 h-2 bg-orange-500 rounded-full mt-2"></div>
                    <span>Thông báo tiền sử bệnh lý nếu có</span>
                  </li>
                </ul>
              </div>
            </div>
          </div>

          {/* Modal Actions */}
          <div className="flex flex-col sm:flex-row gap-4 mt-12 pt-8 border-t">
            <button className="flex-1 bg-blue-600 hover:bg-blue-700 text-white py-4 px-8 rounded-2xl font-bold text-lg transition-all duration-300 transform hover:scale-105 shadow-lg hover:shadow-xl flex items-center justify-center space-x-3">
              <Calendar className="w-6 h-6" />
              <span>📅 Đặt Lịch Ngay</span>
            </button>
            <button className="flex-1 bg-gray-100 hover:bg-gray-200 text-gray-700 py-4 px-8 rounded-2xl font-medium transition-all duration-300">
              Tư Vấn Miễn Phí
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ServiceModal;
