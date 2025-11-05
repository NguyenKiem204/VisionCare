import React, { useState, useEffect } from "react";
import Button from "../common/Button";
import Loading from "../common/Loading";
import { searchDoctors } from "../../services/adminDoctorAPI";
import CalendarTimeSelection from "./CalendarTimeSelection";
import toast from "react-hot-toast";

const DoctorTimeSelection = ({ data, update, next, back, holdSlot, releaseHold }) => {
  const [loading, setLoading] = useState(true);
  const [doctors, setDoctors] = useState([]);
  const [activeDoctor, setActiveDoctor] = useState(data.doctorId || null);

  // Fetch doctors if not already selected
  useEffect(() => {
    // If doctor is already selected from previous step, skip fetching
    if (data.doctorId) {
      setActiveDoctor(data.doctorId);
      setLoading(false);
      return;
    }

    const loadDoctors = async () => {
      try {
        setLoading(true);
        const response = await searchDoctors({
          page: 1,
          pageSize: 100,
        });
        const doctorsList = response.data?.data || response.data?.items || [];
        setDoctors(doctorsList);

        // Set first doctor as active if none selected
        if (!activeDoctor && doctorsList.length > 0) {
          setActiveDoctor(doctorsList[0].doctorId || doctorsList[0].id);
        }
      } catch (error) {
        console.error("Error loading doctors:", error);
        toast.error("Không thể tải danh sách bác sĩ");
      } finally {
        setLoading(false);
      }
    };

    loadDoctors();
  }, [data.doctorId, activeDoctor]);

  // If doctor is already selected, show calendar directly
  if (activeDoctor && data.doctorId) {
    return (
      <CalendarTimeSelection
        data={data}
        update={update}
        next={next}
        back={back}
        holdSlot={holdSlot}
        releaseHold={releaseHold}
      />
    );
  }

  // Show doctor selection first
  return (
    <div>
      <h3 className="text-lg font-semibold mb-4">Chọn bác sĩ</h3>

      {loading && doctors.length === 0 ? (
        <Loading />
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
          {doctors.map((doctor) => {
            const doctorId = doctor.doctorId || doctor.id;
            const isActive = activeDoctor === doctorId;
            return (
              <button
                key={doctorId}
                onClick={() => {
                  setActiveDoctor(doctorId);
                  update({ doctorId: doctorId });
                }}
                className={`text-left p-4 rounded-xl border transition-all ${
                  isActive
                    ? "border-blue-500 bg-blue-50 shadow-md"
                    : "border-gray-200 bg-white hover:border-blue-300"
                }`}
              >
                <p className="font-semibold text-gray-900">
                  {doctor.doctorName || doctor.fullName || doctor.name || "Unknown Doctor"}
                </p>
                <p className="text-sm text-gray-600">
                  {doctor.specializationName || doctor.specialty || "Không có chuyên khoa"}
                </p>
              </button>
            );
          })}
        </div>
      )}

      {activeDoctor && (
        <div className="mt-6">
          <CalendarTimeSelection
            data={{ ...data, doctorId: activeDoctor }}
            update={update}
            next={next}
            back={back}
            holdSlot={holdSlot}
          />
        </div>
      )}
    </div>
  );
};

export default DoctorTimeSelection;
