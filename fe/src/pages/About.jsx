import React from "react";
import DoctorsCarousel from "../components/home/DoctorsCarousel";

const About = () => {
  return (
    <div className="bg-white text-gray-800">
      {/* Hero Section */}
      <section className="relative overflow-hidden">
        {/* soft shapes */}
        <div className="pointer-events-none absolute -top-24 -right-24 h-80 w-80 rounded-full bg-blue-100 blur-2xl opacity-70" />
        <div className="pointer-events-none absolute bottom-0 -left-20 h-72 w-72 rounded-full bg-blue-50 blur-2xl" />
        <div className="mx-auto grid max-w-7xl gap-10 px-6 py-16 md:grid-cols-[1.15fr_1fr] md:items-center lg:py-24">
          <div className="relative z-10">
            <span className="inline-flex items-center rounded-full bg-blue-50 px-3 py-1 text-xs font-medium text-blue-700 ring-1 ring-inset ring-blue-100">
              VisionCare Eye Clinic
            </span>
            <h1 className="mt-4 text-3xl font-semibold tracking-tight text-gray-900 md:text-5xl">
              Caring for every vision, every day.
            </h1>
            <p className="mt-6 max-w-xl text-base leading-relaxed text-gray-600 md:text-lg">
              Tại VisionCare, chúng tôi tin rằng đôi mắt khỏe là nền tảng cho cuộc sống trọn vẹn. Mỗi cuộc hẹn là một cơ hội để mang lại sự rõ ràng và tự tin cho bệnh nhân.
            </p>
            <div className="mt-8 flex flex-wrap gap-4">
              <a
                href="/booking"
                className="inline-flex items-center justify-center rounded-md bg-blue-600 px-5 py-3 text-sm font-medium text-white shadow-sm transition hover:bg-blue-700"
              >
                Book an Appointment
              </a>
              <a
                href="#our-team"
                className="inline-flex items-center justify-center rounded-md border border-gray-200 px-5 py-3 text-sm font-medium text-gray-700 transition hover:bg-gray-50"
              >
                Meet Our Team
              </a>
            </div>
          </div>
          <div className="relative z-10">
            <div className="relative overflow-hidden rounded-2xl shadow-lg">
              <img
                src="https://images.unsplash.com/photo-1584515933487-779824d29309?q=80&w=1600&auto=format&fit=crop"
                alt="Bác sĩ nhãn khoa đang khám mắt cho bệnh nhân"
                className="h-80 w-full object-cover md:h-[460px]"
              />
              <div className="pointer-events-none absolute inset-0 bg-gradient-to-t from-black/10 via-transparent to-transparent" />
            </div>
          </div>
        </div>
        {/* stats band */}
        <div className="mx-auto -mt-6 max-w-7xl px-6 pb-6">
          <div className="grid gap-3 sm:grid-cols-3">
            {[{k:"Năm kinh nghiệm",v:"15+"},{k:"Bệnh nhân hài lòng",v:"50,000+"},{k:"Thiết bị hiện đại",v:"30+"}].map((s)=> (
              <div key={s.k} className="rounded-xl bg-white/90 px-6 py-5 shadow-sm backdrop-blur">
                <div className="text-3xl font-semibold text-[#0c5a8a]">{s.v}</div>
                <div className="mt-1 text-sm text-gray-600">{s.k}</div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* About VisionCare */}
      <section className="mx-auto max-w-7xl px-6 py-16 lg:py-20">
        <div className="grid gap-10 md:grid-cols-2 md:items-center">
          <div>
            <h2 className="text-2xl font-semibold text-gray-900 md:text-3xl">About VisionCare</h2>
            <div className="mt-6 space-y-5 text-gray-700">
              <p>VisionCare được thành lập với mục tiêu mang đến dịch vụ nhãn khoa tiêu chuẩn quốc tế cho mọi người.</p>
              <p>Đội ngũ bác sĩ của chúng tôi kết hợp kinh nghiệm lâm sàng và công nghệ hiện đại để mang lại kết quả điều trị tối ưu.</p>
              <p>Chúng tôi không chỉ khám mắt – chúng tôi đồng hành trong hành trình chăm sóc thị lực suốt đời.</p>
            </div>
            <div className="mt-8 grid gap-6 sm:grid-cols-2">
              {["Khám & tầm soát toàn diện","Điều trị chuẩn hoá","Theo dõi dài hạn","Chăm sóc lấy bệnh nhân làm trung tâm"].map((t)=> (
                <div key={t} className="group">
                  <div className="text-sm font-medium text-gray-900">
                    {t}
                  </div>
                  <div className="mt-2 h-px w-full bg-gradient-to-r from-gray-300/60 via-gray-200 to-transparent group-hover:from-blue-300/70 group-hover:via-blue-200 group-hover:to-transparent transition-colors" />
                </div>
              ))}
            </div>
          </div>
          <div className="relative">
            <div className="relative overflow-hidden rounded-2xl shadow-lg">
              <img
                src="https://images.unsplash.com/photo-1587370560942-ad2a04eabb6d?q=80&w=1600&auto=format&fit=crop"
                alt="Không gian nội thất phòng khám VisionCare"
                className="h-80 w-full object-cover md:h-[420px]"
              />
            </div>
            {/* Optional caption removed for cleaner look */}
          </div>
        </div>
      </section>

      {/* Doctors Carousel (true full-bleed) */}
      <section id="our-team" className="relative left-1/2 right-1/2 -mx-[50vw] w-screen py-12 lg:py-16">
        <DoctorsCarousel minimal />
      </section>

      {/* Technology & Care */}
      <section className="mx-auto max-w-7xl px-6 py-16 lg:py-20">
        <div className="flex items-end justify-between gap-6">
          <h3 className="text-2xl font-semibold text-gray-900 md:text-3xl">Technology & Care</h3>
          <span className="hidden rounded-full bg-blue-50 px-3 py-1 text-xs font-medium text-blue-700 ring-1 ring-inset ring-blue-100 md:inline-flex">ISO-clean • Patient-first</span>
        </div>
        <div className="mt-8 grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {[
            { src: "https://images.unsplash.com/photo-1579684385127-1ef15d508118?q=80&w=1600&auto=format&fit=crop", cap: "Advanced diagnostic imaging" },
            { src: "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?q=80&w=1600&auto=format&fit=crop", cap: "Comfort-first surgery suite" },
            { src: "https://images.unsplash.com/photo-1584982751601-97dcc096659c?q=80&w=1600&auto=format&fit=crop", cap: "Safe, hygienic, patient-centered care" },
            { src: "https://images.unsplash.com/photo-1550831107-1553da8c8464?q=80&w=1600&auto=format&fit=crop", cap: "Comprehensive refraction systems" },
          ].map((item) => (
            <figure key={item.src} className="group relative overflow-hidden rounded-2xl bg-white shadow-sm">
              <img src={item.src} alt={item.cap} className="h-48 w-full object-cover transition duration-300 group-hover:scale-[1.02] md:h-64" />
              <figcaption className="absolute inset-x-0 bottom-0 bg-gradient-to-t from-black/50 to-transparent px-4 py-3 text-sm font-medium text-white">
                {item.cap}
              </figcaption>
            </figure>
          ))}
        </div>
      </section>

      {/* Call to Action */}
      <section className="bg-gradient-to-r from-blue-50 to-blue-100">
        <div className="mx-auto max-w-7xl px-6 py-16 text-center lg:py-20">
          <h3 className="text-2xl font-semibold text-gray-900 md:text-3xl">
            See the world more clearly with VisionCare.
          </h3>
          <p className="mx-auto mt-4 max-w-2xl text-sm leading-relaxed text-gray-700 md:text-base">
            Đặt lịch hẹn hôm nay để được tư vấn và chăm sóc toàn diện cho đôi mắt của bạn.
          </p>
          <div className="mt-8">
            <a
              href="/booking"
              className="inline-flex items-center justify-center rounded-md bg-blue-600 px-6 py-3 text-sm font-medium text-white shadow transition hover:bg-blue-700"
            >
              Book Appointment Now
            </a>
          </div>
        </div>
      </section>
    </div>
  );
};

export default About;


