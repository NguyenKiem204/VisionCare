import React from "react";
import { Stethoscope, Lightbulb, Microscope, Building } from "lucide-react";

const EquipmentTabs = ({ activeTab, setActiveTab, categories }) => {
  const getTabIcon = (category) => {
    switch (category) {
      case "Chẩn đoán & Khám": return Stethoscope;
      case "Phẫu thuật & Laser": return Lightbulb;
      case "Thiết bị hỗ trợ": return Microscope;
      case "Phòng mổ & Vô trùng": return Building;
      default: return Stethoscope;
    }
  };

  return (
    <div className="sticky top-0 z-40 bg-white/95 backdrop-blur-sm border-b shadow-sm">
      <div className="container mx-auto px-4 py-6">
        <div className="text-center mb-8">
          <h2 className="text-3xl font-bold text-gray-900 mb-4">
            Danh Mục Thiết Bị
          </h2>
          <p className="text-gray-600 max-w-2xl mx-auto">
            Khám phá các thiết bị y tế hiện đại được sử dụng tại VisionCare
          </p>
        </div>
        
        <div className="flex flex-wrap justify-center gap-4">
          {categories.map((category) => {
            const IconComponent = getTabIcon(category);
            return (
              <button
                key={category}
                onClick={() => setActiveTab(category)}
                className={`flex items-center space-x-3 px-8 py-4 rounded-2xl font-medium transition-all duration-300 ${
                  activeTab === category
                    ? "bg-blue-600 text-white shadow-lg transform scale-105"
                    : "bg-white text-gray-700 hover:bg-gray-100 border-2 border-gray-200 hover:border-blue-300 hover:scale-105"
                }`}
              >
                <IconComponent className="w-6 h-6" />
                <span>{category}</span>
              </button>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default EquipmentTabs;
