import React, { useState, useEffect } from "react";
import {
  Stethoscope,
  Shield,
  Award,
  TrendingUp,
  Users,
  TestTube,
} from "lucide-react";
import { equipment, equipmentCategories } from "../data/equipment";
import EquipmentTabs from "../components/equipment/EquipmentTabs";
import EquipmentCard from "../components/equipment/EquipmentCard";
import EquipmentModal from "../components/equipment/EquipmentModal";
import EquipmentShowcaseItem from "../components/equipment/EquipmentShowcaseItem";

const Equipment = () => {
  const [activeTab, setActiveTab] = useState("Chẩn đoán & Khám");
  const [filteredEquipment, setFilteredEquipment] = useState([]);
  const [selectedEquipment, setSelectedEquipment] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const filtered = equipment.filter((item) => item.category === activeTab);
    setFilteredEquipment(filtered);
  }, [activeTab]);

  const handleViewDetails = (item) => {
    setSelectedEquipment(item);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedEquipment(null);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Hero Section */}
      <section className="relative h-[70vh] min-h-[600px] bg-gradient-to-br from-blue-50 via-white to-green-50 overflow-hidden">
        {/* Background Image */}
        <div className="absolute inset-0">
          <img
            src="https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80"
            alt="Phòng mổ hiện đại"
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
              <span className="mx-2" aria-hidden="true">
                &gt;
              </span>
              <span className="text-gray-200">Thiết Bị</span>
            </nav>

            {/* Headline */}
            <h1 className="text-5xl md:text-6xl lg:text-7xl font-bold mb-6 leading-tight">
              Công Nghệ Thiết Bị
              <span className="block text-green-300">Hàng Đầu Thế Giới</span>
            </h1>

            <p className="text-xl md:text-2xl mb-8 text-blue-100 max-w-2xl">
              Chúng tôi đầu tư vào công nghệ tiên tiến nhất để chăm sóc đôi mắt
              bạn
            </p>

            {/* Stats Bar */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
              <div className="bg-white/20 backdrop-blur-sm rounded-2xl p-6 text-center">
                <div className="w-16 h-16 bg-white/20 rounded-full flex items-center justify-center mx-auto mb-4">
                  <Stethoscope className="w-8 h-8" />
                </div>
                <div className="text-3xl font-bold mb-2">15+</div>
                <div className="text-sm opacity-90">Thiết bị hiện đại</div>
              </div>
              <div className="bg-white/20 backdrop-blur-sm rounded-2xl p-6 text-center">
                <div className="w-16 h-16 bg-white/20 rounded-full flex items-center justify-center mx-auto mb-4">
                  <Shield className="w-8 h-8" />
                </div>
                <div className="text-3xl font-bold mb-2">100%</div>
                <div className="text-sm opacity-90">Nhập khẩu</div>
              </div>
              <div className="bg-white/20 backdrop-blur-sm rounded-2xl p-6 text-center">
                <div className="w-16 h-16 bg-white/20 rounded-full flex items-center justify-center mx-auto mb-4">
                  <Award className="w-8 h-8" />
                </div>
                <div className="text-3xl font-bold mb-2">Định kỳ</div>
                <div className="text-sm opacity-90">Bảo trì</div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Equipment Tabs */}
      <EquipmentTabs
        activeTab={activeTab}
        setActiveTab={setActiveTab}
        categories={equipmentCategories}
      />

      {/* Equipment Grid */}
      <section className="py-8 md:py-12">
        {filteredEquipment.map((item, idx) => (
          <EquipmentShowcaseItem
            key={item.id}
            equipment={item}
            reversed={idx % 2 === 1}
            onViewDetails={handleViewDetails}
          />
        ))}
      </section>

      {/* Equipment Modal */}
      <EquipmentModal
        isOpen={isModalOpen}
        equipment={selectedEquipment}
        onClose={handleCloseModal}
      />

      {/* Certification & Warranty Section */}
      <section className="py-20 bg-white">
        <div className="container mx-auto px-4">
          <div className="text-center mb-16">
            <h2 className="text-4xl font-bold text-gray-900 mb-6">
              Chứng Nhận & Bảo Trì
            </h2>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto">
              Tất cả thiết bị đạt chứng nhận quốc tế, bảo trì 3 tháng/lần
            </p>
          </div>

          {/* Trust Badges */}
          <div className="flex flex-wrap justify-center items-center gap-12 mb-16">
            <div className="text-center">
              <div className="w-20 h-20 bg-blue-100 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-blue-600 font-bold text-2xl">FDA</span>
              </div>
              <p className="text-sm text-gray-600">Chứng nhận FDA</p>
            </div>
            <div className="text-center">
              <div className="w-20 h-20 bg-green-100 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-green-600 font-bold text-2xl">CE</span>
              </div>
              <p className="text-sm text-gray-600">Tiêu chuẩn châu Âu</p>
            </div>
            <div className="text-center">
              <div className="w-20 h-20 bg-orange-100 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-orange-600 font-bold text-2xl">ISO</span>
              </div>
              <p className="text-sm text-gray-600">ISO 9001:2015</p>
            </div>
            <div className="text-center">
              <div className="w-20 h-20 bg-purple-100 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-purple-600 font-bold text-2xl">JCI</span>
              </div>
              <p className="text-sm text-gray-600">JCI Accreditation</p>
            </div>
          </div>

          {/* Warranty Info */}
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div className="text-center">
              <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <Shield className="w-8 h-8 text-blue-600" />
              </div>
              <h3 className="font-semibold text-gray-900 mb-2">
                Chứng chỉ FDA
              </h3>
              <p className="text-sm text-gray-600">
                Tất cả thiết bị có chứng nhận FDA
              </p>
            </div>

            <div className="text-center">
              <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <Award className="w-8 h-8 text-green-600" />
              </div>
              <h3 className="font-semibold text-gray-900 mb-2">CE Mark</h3>
              <p className="text-sm text-gray-600">Đạt tiêu chuẩn châu Âu</p>
            </div>

            <div className="text-center">
              <div className="w-16 h-16 bg-orange-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <TrendingUp className="w-8 h-8 text-orange-600" />
              </div>
              <h3 className="font-semibold text-gray-900 mb-2">Bảo trì</h3>
              <p className="text-sm text-gray-600">Định kỳ 3 tháng/lần</p>
            </div>

            <div className="text-center">
              <div className="w-16 h-16 bg-purple-100 rounded-full flex items-center justify-center mx-auto mb-4">
                <span className="text-purple-600 font-bold text-xl">2-5</span>
              </div>
              <h3 className="font-semibold text-gray-900 mb-2">Warranty</h3>
              <p className="text-sm text-gray-600">
                2-5 năm bảo hành chính hãng
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Investment & Research Section */}
      <section className="py-20 bg-gradient-to-br from-blue-50 to-green-50">
        <div className="container mx-auto px-4">
          <div className="text-center mb-16">
            <h2 className="text-4xl font-bold text-gray-900 mb-6">
              Đầu Tư & Phát Triển
            </h2>
            <p className="text-xl text-gray-600 max-w-3xl mx-auto">
              VisionCare không ngừng đầu tư và phát triển để mang đến dịch vụ
              tốt nhất
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            <div className="text-center">
              <div className="w-20 h-20 bg-blue-100 rounded-2xl flex items-center justify-center mx-auto mb-6">
                <TrendingUp className="w-10 h-10 text-blue-600" />
              </div>
              <h3 className="text-3xl font-bold text-gray-900 mb-2">50+ Tỷ</h3>
              <p className="text-gray-600 font-medium">Tổng đầu tư thiết bị</p>
            </div>

            <div className="text-center">
              <div className="w-20 h-20 bg-green-100 rounded-2xl flex items-center justify-center mx-auto mb-6">
                <Award className="w-10 h-10 text-green-600" />
              </div>
              <h3 className="text-3xl font-bold text-gray-900 mb-2">3-5 Năm</h3>
              <p className="text-gray-600 font-medium">Cập nhật định kỳ</p>
            </div>

            <div className="text-center">
              <div className="w-20 h-20 bg-orange-100 rounded-2xl flex items-center justify-center mx-auto mb-6">
                <Users className="w-10 h-10 text-orange-600" />
              </div>
              <h3 className="text-3xl font-bold text-gray-900 mb-2">100%</h3>
              <p className="text-gray-600 font-medium">Đào tạo quốc tế</p>
            </div>

            <div className="text-center">
              <div className="w-20 h-20 bg-purple-100 rounded-2xl flex items-center justify-center mx-auto mb-6">
                <TestTube className="w-10 h-10 text-purple-600" />
              </div>
              <h3 className="text-3xl font-bold text-gray-900 mb-2">15+</h3>
              <p className="text-gray-600 font-medium">Nghiên cứu lâm sàng</p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Equipment;
