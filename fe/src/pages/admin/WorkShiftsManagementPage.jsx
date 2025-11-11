import React, { useEffect, useState, useCallback } from "react";
import { fetchWorkShifts, searchWorkShifts, createWorkShift, updateWorkShift, deleteWorkShift } from "../../services/adminWorkShiftAPI";
import WorkShiftTable from "../../components/admin/workShifts/WorkShiftTable";
import WorkShiftModal from "../../components/admin/workShifts/WorkShiftModal";

const WorkShiftsManagementPage = () => {
  const [workShifts, setWorkShifts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedWorkShift, setSelectedWorkShift] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [searchKeyword, setSearchKeyword] = useState("");

  const loadWorkShifts = useCallback(async () => {
    setLoading(true);
    try {
      const res = await searchWorkShifts({ keyword: searchKeyword || null, page: 1, pageSize: 100 });
      setWorkShifts(res.data?.data || res.data || []);
    } catch (error) {
      console.error("Failed to load work shifts:", error);
    }
    setLoading(false);
  }, [searchKeyword]);

  useEffect(() => {
    loadWorkShifts();
  }, [loadWorkShifts]);

  const handleSave = async (data) => {
    try {
      if (selectedWorkShift) {
        await updateWorkShift(selectedWorkShift.id, data);
      } else {
        await createWorkShift(data);
      }
      setModalOpen(false);
      loadWorkShifts();
    } catch (error) {
      console.error("Failed to save work shift:", error);
      alert("Có lỗi xảy ra. Vui lòng thử lại.");
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa ca làm việc này?")) {
      try {
        await deleteWorkShift(id);
        loadWorkShifts();
      } catch (error) {
        console.error("Failed to delete work shift:", error);
      }
    }
  };

  return (
    <div className="max-w-6xl w-full mx-auto px-2 py-4 bg-white dark:bg-gray-800 rounded-2xl shadow-lg border border-gray-200 dark:border-gray-700 mt-6">
      <h1 className="text-2xl font-bold mb-4 text-gray-900 dark:text-white text-center">Quản lý ca làm việc</h1>
      <div className="flex justify-between items-center mb-4">
        <input
          type="text"
          placeholder="Tìm kiếm..."
          value={searchKeyword}
          onChange={(e) => setSearchKeyword(e.target.value)}
          className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg dark:bg-gray-900 dark:text-white"
        />
        <button
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow transition"
          onClick={() => {
            setSelectedWorkShift(null);
            setModalOpen(true);
          }}
        >
          + Thêm ca làm việc
        </button>
      </div>
      <WorkShiftTable
        workShifts={workShifts}
        loading={loading}
        onEdit={(shift) => {
          setSelectedWorkShift(shift);
          setModalOpen(true);
        }}
        onDelete={handleDelete}
        onView={(shift) => alert(`Ca: ${shift.shiftName}\nThời gian: ${shift.startTime} - ${shift.endTime}`)}
      />
      <WorkShiftModal open={modalOpen} workShift={selectedWorkShift} onClose={() => setModalOpen(false)} onSave={handleSave} />
    </div>
  );
};

export default WorkShiftsManagementPage;

