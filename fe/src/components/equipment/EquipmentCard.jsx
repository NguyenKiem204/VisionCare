import React from "react";
import { Shield, Award, Globe2, CheckCircle2 } from "lucide-react";

const EquipmentCard = ({ equipment, onViewDetails }) => {
  return (
    <article className="bg-white rounded-3xl overflow-hidden shadow-sm hover:shadow-md transition-shadow duration-300">
      {/* Media */}
      <div className="relative">
        <img
          src={equipment.image}
          alt={equipment.name}
          className="w-full h-64 md:h-72 lg:h-80 object-cover"
        />
        {/* Soft overlay for readability */}
        <div className="absolute inset-0 bg-gradient-to-t from-black/30 via-black/5 to-transparent" />
        {/* Trust badges */}
        <div className="absolute top-4 left-4 flex items-center gap-2">
          <span className="px-2.5 py-1 rounded-full text-[11px] font-semibold bg-blue-600 text-white">
            FDA
          </span>
          <span className="px-2.5 py-1 rounded-full text-[11px] font-semibold bg-emerald-600 text-white">
            CE
          </span>
        </div>
      </div>

      {/* Content */}
      <div className="p-6 md:p-8">
        {/* Header */}
        <header className="mb-4">
          <h3 className="text-2xl font-semibold tracking-tight text-gray-900">
            {equipment.name}
          </h3>
        </header>

        {/* Meta grid */}
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 mb-6">
          <div className="flex items-center gap-2 text-sm text-gray-700">
            <Globe2 className="w-4 h-4 text-blue-600" />
            <span>Xuất xứ: {equipment.origin}</span>
          </div>
          {equipment.technology && (
            <div className="flex items-center gap-2 text-sm text-gray-700">
              <Award className="w-4 h-4 text-indigo-600" />
              <span>Công nghệ: {equipment.technology}</span>
            </div>
          )}
          {equipment.standard && (
            <div className="flex items-center gap-2 text-sm text-gray-700">
              <Shield className="w-4 h-4 text-emerald-600" />
              <span>Tiêu chuẩn: {equipment.standard}</span>
            </div>
          )}
        </div>

        {/* Story-like bullets */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
          {equipment.applications && (
            <div>
              <p className="text-xs uppercase tracking-wider text-gray-500 mb-2">
                Ứng dụng lâm sàng
              </p>
              <ul className="space-y-2">
                {equipment.applications.slice(0, 3).map((app, idx) => (
                  <li
                    key={idx}
                    className="flex items-start gap-2 text-gray-700 text-sm"
                  >
                    <CheckCircle2 className="w-4 h-4 mt-0.5 text-emerald-600" />
                    <span>{app}</span>
                  </li>
                ))}
              </ul>
            </div>
          )}
          {equipment.advantages && (
            <div>
              <p className="text-xs uppercase tracking-wider text-gray-500 mb-2">
                Lợi ích nổi bật
              </p>
              <ul className="space-y-2">
                {equipment.advantages.slice(0, 3).map((adv, idx) => (
                  <li
                    key={idx}
                    className="flex items-start gap-2 text-gray-700 text-sm"
                  >
                    <CheckCircle2 className="w-4 h-4 mt-0.5 text-blue-600" />
                    <span>{adv}</span>
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>

        {/* Subtle value (de-emphasized price) */}
        {equipment.value && (
          <div className="mb-6">
            <p className="text-xs uppercase tracking-wider text-gray-500 mb-1">
              Giá trị đầu tư
            </p>
            <p className="text-sm font-medium text-gray-700">
              {equipment.value}
            </p>
          </div>
        )}

        {/* CTA */}
        <div className="pt-2">
          <button
            onClick={() => onViewDetails?.(equipment)}
            className="inline-flex items-center gap-2 px-5 py-2.5 rounded-full border border-gray-300 text-gray-800 hover:border-blue-400 hover:text-blue-700 transition-colors"
          >
            Khám phá công nghệ
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M9 5l7 7-7 7"
              />
            </svg>
          </button>
        </div>
      </div>
    </article>
  );
};

export default EquipmentCard;
