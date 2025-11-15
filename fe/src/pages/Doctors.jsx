import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { getTopRatedDoctors } from "../services/adminDoctorAPI";
import { ChevronRight } from "lucide-react";
import toast from "react-hot-toast";

const Doctors = () => {
  const navigate = useNavigate();
  const [doctors, setDoctors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filteredDoctors, setFilteredDoctors] = useState([]);

  useEffect(() => {
    const loadDoctors = async () => {
      try {
        setLoading(true);
        const response = await getTopRatedDoctors(50);
        let doctorsData = [];
        
        if (response?.data) {
          if (Array.isArray(response.data)) {
            doctorsData = response.data;
          } else if (Array.isArray(response.data.data)) {
            doctorsData = response.data.data;
          } else if (Array.isArray(response.data.items)) {
            doctorsData = response.data.items;
          }
        }
        
        const mappedDoctors = doctorsData
          .filter(d => d && d.doctorStatus === 'Active')
          .map((doctor) => ({
            id: doctor.id || doctor.accountId,
            name: doctor.doctorName || doctor.fullName || "",
            specialization: doctor.specializationName || "Bác sĩ",
            biography: doctor.biography || "",
            image: doctor.avatar || doctor.profileImage || `https://ui-avatars.com/api/?name=${encodeURIComponent(doctor.doctorName || "D")}&background=0c5a8a&color=fff&size=200`,
            rating: doctor.rating || 0,
            experienceYears: doctor.experienceYears || 0,
          }));
        
        setDoctors(mappedDoctors);
        setFilteredDoctors(mappedDoctors);
      } catch (error) {
        console.error("Error loading doctors:", error);
        toast.error("Không thể tải danh sách bác sĩ");
      } finally {
        setLoading(false);
      }
    };

    loadDoctors();
  }, []);

  const parseBiography = (biography) => {
    if (!biography) return [];
    
    let items = [];
    
    if (biography.includes('\n')) {
      items = biography.split('\n');
    } else {
      items = biography.split(/\.(?=\s+[A-ZĐ])/);
    }
    
    return items
      .map(item => item.trim().replace(/^\.\s*/, ''))
      .filter(item => item.length > 10)
      .map(item => {
        if (!item.endsWith('.') && !item.endsWith('!') && !item.endsWith('?')) {
          return item + '.';
        }
        return item;
      });
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-[#0c5a8a] mx-auto"></div>
          <p className="mt-4 text-gray-600">Đang tải danh sách bác sĩ...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Hero Header */}
      <div className="relative h-64 bg-gradient-to-r from-gray-700 to-gray-600 overflow-hidden">
        <div 
          className="absolute inset-0 bg-cover bg-center opacity-30"
          style={{
            backgroundImage: "url('https://images.unsplash.com/photo-1631217868264-e5b90bb7e133?ixlib=rb-4.0.3&auto=format&fit=crop&w=1920&q=80')"
          }}
        ></div>
        <div className="absolute inset-0 bg-gradient-to-b from-transparent to-gray-900/50"></div>
        
        <div className="relative h-full flex flex-col items-center justify-center text-white px-4">
          <h1 className="text-4xl md:text-5xl font-bold mb-4">
            ĐỘI NGŨ BÁC SĨ
          </h1>
          
          {/* Breadcrumb */}
          <div className="flex items-center gap-2 text-sm">
            <button onClick={() => navigate("/")} className="hover:text-yellow-400 transition-colors">
              Trang Chủ
            </button>
            <ChevronRight className="w-4 h-4" />
            <span className="text-yellow-400">Bác Sĩ</span>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="container mx-auto px-4 py-12">
        <div className="max-w-7xl mx-auto">
          {filteredDoctors.length === 0 ? (
            <div className="text-center py-12">
              <p className="text-gray-600 text-lg">Không tìm thấy bác sĩ nào</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
              {filteredDoctors.map((doctor) => {
                const achievements = parseBiography(doctor.biography);
                
                return (
                  <div
                    key={doctor.id}
                    className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-xl transition-all duration-300"
                  >
                    <div className="flex flex-col md:flex-row">
                      {/* Doctor Image - Left Side */}
                      <div className="md:w-2/5 relative">
                        <div className="h-full min-h-[400px] bg-gray-100">
                          <img
                            src={doctor.image}
                            alt={doctor.name}
                            className="w-full h-full object-cover"
                          />
                        </div>
                        
                        {/* Yellow accent bar */}
                        <div className="absolute bottom-0 left-0 w-1 h-32 bg-yellow-400"></div>
                        
                        {/* Name overlay at bottom */}
                        <div className="absolute bottom-0 left-0 right-0 bg-white p-4 border-l-4 border-yellow-400">
                          <h3 className="text-lg font-bold text-[#0c5a8a]">
                            {doctor.name}
                          </h3>
                          <p className="text-sm text-gray-600">
                            {doctor.specialization}
                          </p>
                        </div>
                      </div>

                      {/* Doctor Info - Right Side */}
                      <div className="md:w-3/5 p-6 flex flex-col">
                        {/* Achievements List */}
                        {achievements.length > 0 && (
                          <ul className="space-y-3 mb-6 flex-1">
                            {achievements.map((achievement, idx) => (
                              <li key={idx} className="flex items-start text-sm text-gray-700">
                                <span className="inline-block w-1.5 h-1.5 rounded-full bg-gray-400 mt-2 mr-3 flex-shrink-0"></span>
                                <span className="leading-relaxed">{achievement}</span>
                              </li>
                            ))}
                          </ul>
                        )}

                        {/* View Details Button */}
                        <button
                          onClick={() => navigate(`/doctors/${doctor.id}`)}
                          className="self-start px-6 py-2.5 bg-[#0c5a8a] hover:bg-[#094a75] text-white font-medium rounded-full transition-all duration-300 flex items-center gap-2"
                        >
                          <span>XEM CHI TIẾT</span>
                          <ChevronRight className="w-4 h-4" />
                        </button>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Doctors;