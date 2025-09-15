import React from "react";
import { X, Shield, Award, CheckCircle2 } from "lucide-react";

const EquipmentModal = ({ isOpen, equipment, onClose }) => {
  if (!isOpen || !equipment) return null;

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-3 md:p-4">
      <div className="bg-white rounded-2xl md:rounded-3xl max-w-3xl md:max-w-4xl w-full max-h-[85vh] overflow-y-auto">
        <div className="p-4 md:p-6">
          {/* Header */}
          <div className="flex items-start justify-between mb-4 md:mb-6">
            <div>
              <h2 className="text-xl md:text-2xl font-bold text-gray-900 mb-2">{equipment.name}</h2>
              <div className="flex items-center gap-2">
                <span className="px-2 py-0.5 rounded-full text-[10px] md:text-[11px] font-semibold bg-blue-600 text-white">FDA</span>
                <span className="px-2 py-0.5 rounded-full text-[10px] md:text-[11px] font-semibold bg-emerald-600 text-white">CE</span>
                {equipment.standard && (
                  <span className="inline-flex items-center gap-1 px-2 py-0.5 rounded-full text-[10px] md:text-[11px] font-semibold bg-emerald-50 text-emerald-700">
                    <Shield className="w-3.5 h-3.5" /> {equipment.standard}
                  </span>
                )}
              </div>
            </div>
            <button onClick={onClose} className="p-2 hover:bg-gray-100 rounded-full">
              <X className="w-5 h-5 md:w-6 md:h-6" />
            </button>
          </div>

          {/* Media */}
          <div className="rounded-xl md:rounded-2xl overflow-hidden mb-4 md:mb-6">
            <img src={equipment.image} alt={equipment.name} className="w-full h-56 md:h-72 object-cover" />
          </div>

          {/* Content grid */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 md:gap-8">
            <div>
              <h3 className="text-base md:text-lg font-semibold text-gray-900 mb-2 md:mb-3">Mô tả & Công nghệ</h3>
              <p className="text-gray-700 leading-relaxed mb-3 md:mb-4 text-sm md:text-base">
                {equipment.description || "Thiết bị y tế hiện đại với độ chính xác cao, độ bền tốt và trải nghiệm thoải mái cho bệnh nhân."}
              </p>
              {equipment.technology && (
                <div className="inline-flex items-center gap-2 px-2.5 py-1.5 rounded-full bg-blue-50 text-blue-700 text-xs md:text-sm font-medium">
                  <Award className="w-4 h-4" /> {equipment.technology}
                </div>
              )}
            </div>

            <div className="space-y-5 md:space-y-6">
              {equipment.applications && (
                <div>
                  <h4 className="text-xs md:text-sm font-semibold text-gray-900 mb-2 uppercase tracking-wider">Ứng dụng lâm sàng</h4>
                  <ul className="space-y-2">
                    {equipment.applications.map((item, idx) => (
                      <li key={idx} className="flex items-start gap-2 text-gray-700 text-sm">
                        <CheckCircle2 className="w-4 h-4 mt-0.5 text-emerald-600" />
                        <span>{item}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}

              {equipment.advantages && (
                <div>
                  <h4 className="text-xs md:text-sm font-semibold text-gray-900 mb-2 uppercase tracking-wider">Ưu điểm</h4>
                  <ul className="space-y-2">
                    {equipment.advantages.map((item, idx) => (
                      <li key={idx} className="flex items-start gap-2 text-gray-700 text-sm">
                        <CheckCircle2 className="w-4 h-4 mt-0.5 text-blue-600" />
                        <span>{item}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>
          </div>

          {/* Actions */}
          <div className="mt-6 md:mt-8 pt-5 md:pt-6 border-t flex flex-col sm:flex-row gap-3 md:gap-4">
            <button className="flex-1 px-5 md:px-6 py-2.5 md:py-3 rounded-full border border-gray-300 text-gray-800 hover:border-blue-400 hover:text-blue-700 transition-colors text-sm md:text-base">
              Tư vấn sử dụng thiết bị
            </button>
            <button className="flex-1 px-5 md:px-6 py-2.5 md:py-3 rounded-full bg-blue-600 text-white hover:bg-blue-700 transition-colors text-sm md:text-base">
              Khám phá thêm hình ảnh
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EquipmentModal;
