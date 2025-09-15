import React, { useEffect, useRef, useState } from "react";
import { Shield, Award, Globe2, CheckCircle2 } from "lucide-react";

const clamp = (num, min, max) => Math.min(Math.max(num, min), max);

const EquipmentShowcaseItem = ({
  equipment,
  reversed = false,
  onViewDetails,
}) => {
  const mediaRef = useRef(null);
  const [parallaxY, setParallaxY] = useState(0);
  const [blurPx, setBlurPx] = useState(6);
  const [opacity, setOpacity] = useState(0.85);

  useEffect(() => {
    const handleScroll = () => {
      if (!mediaRef.current) return;
      const rect = mediaRef.current.getBoundingClientRect();
      const vh = window.innerHeight || 1;
      const elementCenter = rect.top + rect.height / 2;
      const viewportCenter = vh / 2;
      const delta = elementCenter - viewportCenter; // px distance from center

      // Normalize progress to [-1, 1]
      const progress = clamp(delta / (vh * 0.8), -1, 1);

      // Parallax: subtle shift opposite to scroll direction
      setParallaxY(clamp(progress * 24, -24, 24));

      // Blur reveal: sharpest when centered
      const blur = clamp(Math.abs(progress) * 10, 0, 8);
      setBlurPx(blur);

      // Opacity veil to create soft reveal
      const op = clamp(0.95 - (1 - Math.abs(progress)) * 0.25, 0.6, 0.95);
      setOpacity(op);
    };

    handleScroll();
    window.addEventListener("scroll", handleScroll, { passive: true });
    window.addEventListener("resize", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
      window.removeEventListener("resize", handleScroll);
    };
  }, []);

  return (
    <section
      className={`py-10 md:py-14 ${reversed ? "bg-white" : "bg-gray-50"}`}
    >
      <div className="container mx-auto px-4">
        <div
          className={`grid grid-cols-1 md:grid-cols-12 gap-8 md:gap-12 items-center`}
        >
          {/* Image */}
          <div
            className={`md:col-span-6 ${
              reversed ? "md:order-2" : "md:order-none"
            }`}
          >
            <div
              ref={mediaRef}
              className="relative rounded-3xl overflow-hidden will-change-transform"
              style={{ transform: `translateY(${parallaxY}px)` }}
            >
              <img
                src={equipment.image}
                alt={equipment.name}
                className="w-full h-64 md:h-80 object-cover"
                style={{ filter: `blur(${blurPx}px)` }}
              />
              {/* Soft gradient + veil to enhance reveal */}
              <div
                className="absolute inset-0"
                style={{
                  background:
                    "linear-gradient(to top, rgba(0,0,0,0.35), rgba(0,0,0,0) 40%), linear-gradient(to bottom, rgba(0,0,0,0.25), rgba(0,0,0,0) 35%)",
                  opacity,
                }}
              />
              <div className="absolute top-4 left-4 flex items-center gap-2">
                <span className="px-2.5 py-1 rounded-full text-[11px] font-semibold bg-blue-600 text-white">
                  FDA
                </span>
                <span className="px-2.5 py-1 rounded-full text-[11px] font-semibold bg-emerald-600 text-white">
                  CE
                </span>
              </div>
            </div>
          </div>

          {/* Content */}
          <div
            className={`md:col-span-6 ${
              reversed ? "md:order-1" : "md:order-none"
            }`}
          >
            <div className="max-w-xl">
              <h3 className="text-3xl font-semibold tracking-tight text-gray-900 mb-3">
                {equipment.name}
              </h3>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 mb-5">
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

              <p className="text-gray-700 leading-relaxed mb-6">
                {equipment.description ||
                  equipment.functions?.[0] ||
                  "Thiết bị y tế hiện đại, độ chính xác cao và thân thiện với bệnh nhân."}
              </p>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 mb-8">
                {equipment.applications && (
                  <div>
                    <p className="text-xs uppercase tracking-wider text-gray-500 mb-2">
                      Ứng dụng lâm sàng
                    </p>
                    <ul className="space-y-2">
                      {equipment.applications.slice(0, 3).map((item, idx) => (
                        <li
                          key={idx}
                          className="flex items-start gap-2 text-gray-700 text-sm"
                        >
                          <CheckCircle2 className="w-4 h-4 mt-0.5 text-emerald-600" />
                          <span>{item}</span>
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
                      {equipment.advantages.slice(0, 3).map((item, idx) => (
                        <li
                          key={idx}
                          className="flex items-start gap-2 text-gray-700 text-sm"
                        >
                          <CheckCircle2 className="w-4 h-4 mt-0.5 text-blue-600" />
                          <span>{item}</span>
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
              </div>

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
        </div>
      </div>
    </section>
  );
};

export default EquipmentShowcaseItem;
