import React, { useState, useEffect } from "react";
import Button from "../common/Button";
import Loading from "../common/Loading";
import { searchDoctors, getDoctorsByService } from "../../services/adminDoctorAPI";
import toast from "react-hot-toast";

const DoctorSelection = ({ data, update, next, back, filterByService = false }) => {
  const [loading, setLoading] = useState(true);
  const [doctors, setDoctors] = useState([]);
  const [selectedDoctor, setSelectedDoctor] = useState(data.doctorId || null);

  // Fetch doctors
  useEffect(() => {
    const loadDoctors = async () => {
      try {
        setLoading(true);
        let doctorsList = [];

        if (filterByService && data.serviceDetailId) {
          // Fetch doctors by service
          const response = await getDoctorsByService(data.serviceDetailId);
          doctorsList = response.data || [];
        } else {
          // Fetch all doctors or search
          const response = await searchDoctors({
            page: 1,
            pageSize: 100,
          });
          doctorsList = response.data?.data || response.data?.items || [];
        }

        setDoctors(doctorsList);

        // Set first doctor as selected if none selected and we have doctors
        if (!selectedDoctor && doctorsList.length > 0) {
          setSelectedDoctor(doctorsList[0].doctorId || doctorsList[0].id);
        }
      } catch (error) {
        console.error("Error loading doctors:", error);
        toast.error("Không thể tải danh sách bác sĩ");
      } finally {
        setLoading(false);
      }
    };

    loadDoctors();
  }, [filterByService, data.serviceDetailId]);

  const handleDoctorSelect = (doctorId) => {
    setSelectedDoctor(doctorId);
    update({ doctorId });
  };

  const handleNext = () => {
    if (!selectedDoctor) {
      toast.error("Vui lòng chọn bác sĩ");
      return;
    }
    update({ doctorId: selectedDoctor });
    next();
  };

  return (
    <div>
      <h3 className="text-lg font-semibold mb-4">
        {filterByService ? "Chọn bác sĩ cho dịch vụ này" : "Chọn bác sĩ"}
      </h3>

      {loading ? (
        <Loading />
      ) : doctors.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-gray-500 mb-4">
            {filterByService
              ? "Không có bác sĩ nào có thể thực hiện dịch vụ này"
              : "Không có bác sĩ nào khả dụng"}
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
          {doctors.map((doctor) => {
            const doctorId = doctor.doctorId || doctor.id;
            const isSelected = selectedDoctor === doctorId;
            return (
              <label
                key={doctorId}
                className={`flex items-center gap-4 p-4 rounded-xl border cursor-pointer transition-all ${
                  isSelected
                    ? "border-blue-500 bg-blue-50 shadow-md"
                    : "border-gray-200 bg-white hover:border-blue-300"
                }`}
                onClick={() => handleDoctorSelect(doctorId)}
              >
                <div className="flex-shrink-0">
                  {doctor.profileImage ? (
                    <img
                      src={doctor.profileImage}
                      alt={doctor.doctorName || doctor.fullName}
                      className="w-16 h-16 rounded-full object-cover"
                    />
                  ) : (
                    <div className="w-16 h-16 rounded-full bg-gray-200 flex items-center justify-center">
                      <svg
                        className="w-8 h-8 text-gray-400"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
                        />
                      </svg>
                    </div>
                  )}
                </div>
                <div className="flex-1 min-w-0">
                  <p className="font-semibold text-gray-900 truncate">
                    {doctor.doctorName || doctor.fullName || "Unknown Doctor"}
                  </p>
                  {doctor.specializationName && (
                    <p className="text-sm text-gray-600 truncate">
                      {doctor.specializationName}
                    </p>
                  )}
                  {doctor.experienceYears && (
                    <p className="text-sm text-gray-500">
                      Kinh nghiệm: {doctor.experienceYears} năm
                    </p>
                  )}
                  {doctor.rating !== null && doctor.rating !== undefined && (
                    <div className="flex items-center gap-1 mt-1">
                      <svg
                        className="w-4 h-4 text-yellow-400"
                        fill="currentColor"
                        viewBox="0 0 20 20"
                      >
                        <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                      </svg>
                      <span className="text-sm text-gray-600">
                        {doctor.rating.toFixed(1)}
                      </span>
                    </div>
                  )}
                </div>
                <input
                  type="radio"
                  name="doctor"
                  checked={isSelected}
                  onChange={() => handleDoctorSelect(doctorId)}
                  className="w-5 h-5 text-blue-600"
                />
              </label>
            );
          })}
        </div>
      )}

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button
          variant="accent"
          onClick={handleNext}
          disabled={loading || !selectedDoctor}
        >
          Tiếp tục
        </Button>
      </div>
    </div>
  );
};

export default DoctorSelection;

