import React, { useState, useEffect } from "react";
import { Eye, User, Star } from "lucide-react";
import { services, serviceCategories } from "../data/services";
import ServiceFilter from "../components/services/ServiceFilter";
import ServiceGrid from "../components/services/ServiceGrid";
import ServiceModal from "../components/services/ServiceModal";

const Services = () => {
  const [selectedCategory, setSelectedCategory] = useState("Tất cả dịch vụ");
  const [searchTerm, setSearchTerm] = useState("");
  const [sortBy, setSortBy] = useState("Phổ biến nhất");
  const [filteredServices, setFilteredServices] = useState(services);
  const [selectedService, setSelectedService] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [stats, setStats] = useState({ services: 0, doctors: 0, years: 0 });

  // Counter animation for stats
  useEffect(() => {
    const animateCount = (end, setter, duration = 2000) => {
      let start = 0;
      const increment = end / (duration / 16);
      const timer = setInterval(() => {
        start += increment;
        if (start >= end) {
          setter(end);
          clearInterval(timer);
        } else {
          setter(Math.floor(start));
        }
      }, 16);
    };

    animateCount(15, (val) => setStats((prev) => ({ ...prev, services: val })));
    animateCount(4, (val) => setStats((prev) => ({ ...prev, doctors: val })));
    animateCount(20, (val) => setStats((prev) => ({ ...prev, years: val })));
  }, []);

  useEffect(() => {
    let filtered = services;

    if (selectedCategory !== "Tất cả dịch vụ") {
      filtered = filtered.filter(
        (service) => service.category === selectedCategory
      );
    }

    if (searchTerm) {
      filtered = filtered.filter(
        (service) =>
          service.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          service.description.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    switch (sortBy) {
      case "A-Z":
        filtered.sort((a, b) => a.name.localeCompare(b.name));
        break;
      case "Giá tăng dần":
        filtered.sort((a, b) => {
          const priceA = parseInt(a.price.replace(/[^\d]/g, ""));
          const priceB = parseInt(b.price.replace(/[^\d]/g, ""));
          return priceA - priceB;
        });
        break;
      default:
        break;
    }

    setFilteredServices(filtered);
  }, [selectedCategory, searchTerm, sortBy]);

  const handleViewDetails = (service) => {
    setSelectedService(service);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedService(null);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Hero Section */}
      <section className="relative h-[70vh] min-h-[600px] bg-gradient-to-br from-blue-50 via-white to-green-50 overflow-hidden">
        {/* Background Image */}
        <div className="absolute inset-0">
          <img
            src="https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80"
            alt="Phòng khám hiện đại"
            className="w-full h-full object-cover"
          />
          <div className="absolute inset-0 bg-gradient-to-r from-blue-600/80 via-blue-500/70 to-transparent"></div>
        </div>

        <div className="relative container mx-auto px-4 h-full flex items-center">
          <div className="text-white max-w-4xl">
            {/* Breadcrumb */}
            <nav className="text-sm mb-6 opacity-90">
              <a href="/" className="hover:text-blue-200 transition-colors">
                Home
              </a>
              <span className="mx-2">&gt;</span>
              <span className="text-gray-200">Dịch Vụ</span>
            </nav>

            {/* Headline */}
            <h1 className="text-5xl md:text-6xl lg:text-7xl font-bold mb-6 leading-tight">
              Dịch Vụ Chuyên Khoa
              <span className="block text-green-300">Toàn Diện</span>
            </h1>

            <p className="text-xl md:text-2xl mb-8 text-blue-100 max-w-2xl">
              Từ khám tổng quát đến phẫu thuật chuyên sâu với công nghệ tiên
              tiến nhất
            </p>

            {/* Stats Bar */}
            <div className="flex flex-wrap gap-8 text-center">
              <div className="flex items-center space-x-3">
                <div className="w-12 h-12 bg-white/20 rounded-full flex items-center justify-center backdrop-blur-sm">
                  <Eye className="w-6 h-6" />
                </div>
                <div>
                  <div className="text-3xl font-bold">{stats.services}+</div>
                  <div className="text-sm opacity-90">Dịch vụ</div>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-12 h-12 bg-white/20 rounded-full flex items-center justify-center backdrop-blur-sm">
                  <User className="w-6 h-6" />
                </div>
                <div>
                  <div className="text-3xl font-bold">{stats.doctors}</div>
                  <div className="text-sm opacity-90">Bác sĩ chuyên môn</div>
                </div>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-12 h-12 bg-white/20 rounded-full flex items-center justify-center backdrop-blur-sm">
                  <Star className="w-6 h-6" />
                </div>
                <div>
                  <div className="text-3xl font-bold">{stats.years}+</div>
                  <div className="text-sm opacity-90">Năm kinh nghiệm</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Filter & Search */}
      <ServiceFilter
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        selectedCategory={selectedCategory}
        setSelectedCategory={setSelectedCategory}
        sortBy={sortBy}
        setSortBy={setSortBy}
        serviceCategories={serviceCategories}
      />

      {/* Services Grid */}
      <ServiceGrid
        services={filteredServices}
        onViewDetails={handleViewDetails}
      />

      {/* Service Modal */}
      <ServiceModal
        service={selectedService}
        isOpen={isModalOpen}
        onClose={handleCloseModal}
      />
    </div>
  );
};

export default Services;
