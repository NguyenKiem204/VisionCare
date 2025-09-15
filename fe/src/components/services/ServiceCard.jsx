import React from "react";
import { Clock, DollarSign, User, Eye } from "lucide-react";

const ServiceCard = ({ service, onViewDetails }) => {
  return (
    <div className="bg-white rounded-2xl shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:scale-105 overflow-hidden group">
      {/* Service Image */}
      <div className="aspect-video overflow-hidden relative">
        <img
          src="https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80"
          alt={service.name}
          className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
        />
        <div className="absolute top-4 left-4">
          <div className="w-12 h-12 bg-white/90 rounded-full flex items-center justify-center backdrop-blur-sm">
            <Eye className="w-6 h-6 text-blue-600" />
          </div>
        </div>
        {service.badge && (
          <div className="absolute top-4 right-4">
            <span className="px-3 py-1 bg-orange-500 text-white text-xs font-bold rounded-full">
              {service.badge}
            </span>
          </div>
        )}
      </div>

      {/* Service Content */}
      <div className="p-6">
        <h3 className="text-xl font-bold text-gray-900 mb-3 group-hover:text-blue-600 transition-colors">
          {service.name}
        </h3>
        
        <p className="text-gray-600 mb-4 line-clamp-2 leading-relaxed">
          {service.description}
        </p>

        {/* Service Details */}
        <div className="space-y-3 mb-6">
          {service.duration && (
            <div className="flex items-center text-sm text-gray-500">
              <Clock className="w-4 h-4 mr-2 text-blue-500" />
              <span>{service.duration}</span>
            </div>
          )}
          <div className="flex items-center text-sm">
            <DollarSign className="w-4 h-4 mr-2 text-green-500" />
            <span className="font-bold text-green-600 text-lg">{service.price}</span>
          </div>
          <div className="flex items-center text-sm text-gray-500">
            <User className="w-4 h-4 mr-2 text-purple-500" />
            <span>{service.doctor}</span>
          </div>
        </div>

        <button
          onClick={() => onViewDetails(service)}
          className="w-full bg-blue-600 hover:bg-blue-700 text-white py-3 px-6 rounded-xl font-medium transition-all duration-300 transform hover:scale-105 shadow-lg hover:shadow-xl"
        >
          Xem Chi Tiết
        </button>
      </div>
    </div>
  );
};

export default ServiceCard;
