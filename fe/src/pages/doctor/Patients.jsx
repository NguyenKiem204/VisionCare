import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { Phone, Mail, MapPin, Calendar, User, Clock, FileText, X, Eye, Search, ChevronLeft, ChevronRight } from "lucide-react";
import api from "../../utils/api";
import { getPatientMedicalHistory } from "../../services/doctorMeAPI";
import toast from "react-hot-toast";

const DoctorPatients = () => {
  const navigate = useNavigate();
  const [patients, setPatients] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedPatient, setSelectedPatient] = useState(null);
  const [medicalHistory, setMedicalHistory] = useState([]);
  const [loadingHistory, setLoadingHistory] = useState(false);
  const [showHistoryModal, setShowHistoryModal] = useState(false);
  
  // Pagination and search state
  const [searchKeyword, setSearchKeyword] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(12); // 12 items per page (3 columns x 4 rows)
  const [totalCount, setTotalCount] = useState(0);
  const [searchInput, setSearchInput] = useState("");

  const load = useCallback(async (page = 1, keyword = "") => {
    setLoading(true);
    try {
      const res = await api.get("/doctor/me/patients", {
        params: {
          page,
          pageSize,
          keyword: keyword || undefined,
        },
      });
      
      // API returns PagedResponse<CustomerDto>
      const responseData = res?.data?.data ?? res?.data;
      if (responseData) {
        setPatients(Array.isArray(responseData.items || responseData) ? (responseData.items || responseData) : []);
        setTotalCount(responseData.totalCount || (Array.isArray(responseData) ? responseData.length : 0));
      } else {
        setPatients([]);
        setTotalCount(0);
      }
    } catch (e) {
      console.error("Load patients error", e);
      setPatients([]);
      setTotalCount(0);
      toast.error("Không thể tải danh sách bệnh nhân");
    } finally {
      setLoading(false);
    }
  }, [pageSize]);

  useEffect(() => {
    load(currentPage, searchKeyword);
  }, [currentPage, searchKeyword, load]);

  // Debounce search
  useEffect(() => {
    const timer = setTimeout(() => {
      setSearchKeyword(searchInput);
      setCurrentPage(1); // Reset to first page when searching
    }, 500);

    return () => clearTimeout(timer);
  }, [searchInput]);

  const loadMedicalHistory = async (patientId) => {
    setLoadingHistory(true);
    try {
      const history = await getPatientMedicalHistory(patientId);
      setMedicalHistory(Array.isArray(history) ? history : []);
      setShowHistoryModal(true);
    } catch (error) {
      console.error("Load medical history error", error);
      toast.error("Không thể tải hồ sơ bệnh án");
      setMedicalHistory([]);
    } finally {
      setLoadingHistory(false);
    }
  };

  const handleViewHistory = (patient) => {
    setSelectedPatient(patient);
    loadMedicalHistory(patient.id || patient.Id);
  };

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Bệnh nhân</h1>
        <p className="text-gray-600 dark:text-gray-300">Quản lý bệnh nhân của bác sĩ</p>
      </div>

      <div className="bg-white dark:bg-gray-800 shadow rounded-lg p-6">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">
          <h3 className="text-lg font-medium text-gray-900 dark:text-white">
            Danh sách bệnh nhân
            {totalCount > 0 && (
              <span className="ml-2 text-sm font-normal text-gray-500 dark:text-gray-400">
                ({totalCount} bệnh nhân)
              </span>
            )}
          </h3>
          
          {/* Search Bar */}
          <div className="relative flex-1 sm:max-w-md">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
            <input
              type="text"
              placeholder="Tìm kiếm theo tên, email, số điện thoại..."
              value={searchInput}
              onChange={(e) => setSearchInput(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
            {searchInput && (
              <button
                onClick={() => {
                  setSearchInput("");
                  setSearchKeyword("");
                  setCurrentPage(1);
                }}
                className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
              >
                <X className="h-4 w-4" />
              </button>
            )}
          </div>
        </div>
        {loading && (
          <div className="flex items-center justify-center py-8">
            <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>
          </div>
        )}
        {!loading && (
          <>
            {patients.length === 0 ? (
              <div className="text-center py-12">
                {searchKeyword ? (
                  <>
                    <Search className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                    <p className="text-gray-500 dark:text-gray-300">Không tìm thấy bệnh nhân</p>
                    <p className="text-sm text-gray-400 dark:text-gray-500 mt-2">
                      Thử tìm kiếm với từ khóa khác
                    </p>
                  </>
                ) : (
                  <>
                    <User className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                    <p className="text-gray-500 dark:text-gray-300">Chưa có bệnh nhân nào</p>
                    <p className="text-sm text-gray-400 dark:text-gray-500 mt-2">
                      Bệnh nhân sẽ xuất hiện ở đây sau khi có lịch hẹn với bạn
                    </p>
                  </>
                )}
              </div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {patients.map((p) => {
                  const patientId = p.id || p.Id;
                  const name = p.customerName || p.fullName || p.CustomerName || `Bệnh nhân #${patientId}`;
                  const phone = p.phone || p.Phone;
                  const email = p.email || p.Email;
                  const address = p.address || p.Address;
                  const gender = p.gender || p.Gender;
                  const age = p.age || p.Age;
                  const dob = p.dob || p.Dob;
                  
                  return (
                    <div
                      key={patientId}
                      className="border border-gray-200 dark:border-gray-700 rounded-lg p-5 hover:shadow-lg transition-all cursor-pointer bg-white dark:bg-gray-800"
                      onClick={() => {
                        // Navigate to patient detail or appointments
                        navigate(`/doctor/schedule?patientId=${patientId}`);
                      }}
                    >
                      <div className="flex items-start space-x-4 mb-4">
                        <div className="w-12 h-12 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-full flex items-center justify-center flex-shrink-0">
                          <span className="text-white font-bold text-lg">
                            {name[0].toUpperCase()}
                          </span>
                        </div>
                        <div className="flex-1 min-w-0">
                          <h3 className="text-base font-semibold text-gray-900 dark:text-white truncate mb-1">
                            {name}
                          </h3>
                          <div className="flex flex-wrap gap-2 text-xs text-gray-500 dark:text-gray-400">
                            {gender && (
                              <span className="px-2 py-0.5 bg-gray-100 dark:bg-gray-700 rounded">
                                {gender === "Male" ? "Nam" : gender === "Female" ? "Nữ" : gender}
                              </span>
                            )}
                            {age && (
                              <span className="px-2 py-0.5 bg-gray-100 dark:bg-gray-700 rounded">
                                {age} tuổi
                              </span>
                            )}
                          </div>
                        </div>
                      </div>
                      
                      <div className="space-y-2 text-sm">
                        {phone && (
                          <div className="flex items-center gap-2 text-gray-600 dark:text-gray-300">
                            <Phone className="h-4 w-4 text-gray-400" />
                            <span className="truncate">{phone}</span>
                          </div>
                        )}
                        {email && (
                          <div className="flex items-center gap-2 text-gray-600 dark:text-gray-300">
                            <Mail className="h-4 w-4 text-gray-400" />
                            <span className="truncate">{email}</span>
                          </div>
                        )}
                        {address && (
                          <div className="flex items-start gap-2 text-gray-600 dark:text-gray-300">
                            <MapPin className="h-4 w-4 text-gray-400 mt-0.5 flex-shrink-0" />
                            <span className="line-clamp-2">{address}</span>
                          </div>
                        )}
                        {dob && (
                          <div className="flex items-center gap-2 text-gray-600 dark:text-gray-300">
                            <Calendar className="h-4 w-4 text-gray-400" />
                            <span>
                              {new Date(dob).toLocaleDateString("vi-VN", {
                                year: "numeric",
                                month: "long",
                                day: "numeric",
                              })}
                            </span>
                          </div>
                        )}
                      </div>
                      
                      {(!phone && !email && !address && !dob) && (
                        <div className="mt-3 pt-3 border-t border-gray-200 dark:border-gray-700">
                          <p className="text-xs text-gray-400 dark:text-gray-500 italic">
                            Chưa có thông tin chi tiết
                          </p>
                        </div>
                      )}
                      
                      <div className="mt-4 pt-3 border-t border-gray-200 dark:border-gray-700">
                        <button
                          onClick={(e) => {
                            e.stopPropagation();
                            handleViewHistory(p);
                          }}
                          className="w-full px-3 py-2 bg-blue-600 text-white text-sm rounded-md hover:bg-blue-700 flex items-center justify-center gap-2"
                        >
                          <FileText className="h-4 w-4" />
                          Xem hồ sơ bệnh án
                        </button>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </>
        )}

        {/* Pagination */}
        {totalPages > 1 && (
          <div className="mt-6 flex items-center justify-between border-t border-gray-200 dark:border-gray-700 pt-4">
            <div className="text-sm text-gray-700 dark:text-gray-300">
              Hiển thị{" "}
              <span className="font-medium">
                {patients.length > 0 ? (currentPage - 1) * pageSize + 1 : 0}
              </span>{" "}
              đến{" "}
              <span className="font-medium">
                {Math.min(currentPage * pageSize, totalCount)}
              </span>{" "}
              trong tổng số <span className="font-medium">{totalCount}</span> bệnh nhân
            </div>
            <div className="flex items-center gap-2">
              <button
                onClick={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
                disabled={currentPage === 1 || loading}
                className="px-3 py-2 text-sm font-medium text-gray-700 dark:text-gray-300 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md hover:bg-gray-50 dark:hover:bg-gray-600 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
              >
                <ChevronLeft className="h-4 w-4" />
                Trước
              </button>
              
              <div className="flex items-center gap-1">
                {Array.from({ length: Math.min(5, totalPages) }, (_, i) => {
                  let pageNum;
                  if (totalPages <= 5) {
                    pageNum = i + 1;
                  } else if (currentPage <= 3) {
                    pageNum = i + 1;
                  } else if (currentPage >= totalPages - 2) {
                    pageNum = totalPages - 4 + i;
                  } else {
                    pageNum = currentPage - 2 + i;
                  }
                  
                  return (
                    <button
                      key={pageNum}
                      onClick={() => setCurrentPage(pageNum)}
                      disabled={loading}
                      className={`px-3 py-2 text-sm font-medium rounded-md ${
                        currentPage === pageNum
                          ? "bg-blue-600 text-white"
                          : "text-gray-700 dark:text-gray-300 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-600"
                      } disabled:opacity-50 disabled:cursor-not-allowed`}
                    >
                      {pageNum}
                    </button>
                  );
                })}
              </div>
              
              <button
                onClick={() => setCurrentPage((prev) => Math.min(totalPages, prev + 1))}
                disabled={currentPage === totalPages || loading}
                className="px-3 py-2 text-sm font-medium text-gray-700 dark:text-gray-300 bg-white dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-md hover:bg-gray-50 dark:hover:bg-gray-600 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
              >
                Sau
                <ChevronRight className="h-4 w-4" />
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Medical History Modal */}
      {showHistoryModal && selectedPatient && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow-xl max-w-5xl w-full max-h-[90vh] overflow-hidden flex flex-col">
            <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-gray-700">
              <div className="flex-1">
                <h2 className="text-xl font-bold text-gray-900 dark:text-white">
                  Hồ sơ bệnh án (Medical History)
                </h2>
                <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                  {selectedPatient.customerName || selectedPatient.fullName || selectedPatient.CustomerName || `Bệnh nhân #${selectedPatient.id || selectedPatient.Id}`}
                </p>
                <p className="text-xs text-gray-400 dark:text-gray-500 mt-1">
                  Lịch sử khám bệnh tổng quan - Để xem chi tiết SOAP notes, vui lòng vào trang "Hồ sơ khám"
                </p>
              </div>
              <button
                onClick={() => {
                  setShowHistoryModal(false);
                  setSelectedPatient(null);
                  setMedicalHistory([]);
                }}
                className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
              >
                <X className="h-6 w-6" />
              </button>
            </div>

            <div className="flex-1 overflow-y-auto p-6">
              {loadingHistory ? (
                <div className="text-center py-8">
                  <p className="text-gray-500 dark:text-gray-300">Đang tải...</p>
                </div>
              ) : medicalHistory.length === 0 ? (
                <div className="text-center py-8">
                  <FileText className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                  <p className="text-gray-500 dark:text-gray-300">Chưa có hồ sơ bệnh án</p>
                  <p className="text-sm text-gray-400 dark:text-gray-500 mt-2">
                    Hồ sơ bệnh án sẽ xuất hiện sau khi khám bệnh
                  </p>
                </div>
              ) : (
                <div className="space-y-4">
                  {medicalHistory.map((record) => (
                    <div
                      key={record.id || record.Id}
                      className="border border-gray-200 dark:border-gray-700 rounded-lg p-5 hover:shadow-md transition-shadow"
                    >
                      <div className="flex items-start justify-between mb-4">
                        <div>
                          <h3 className="font-semibold text-gray-900 dark:text-white">
                            Lần khám #{record.appointmentId || record.AppointmentId}
                          </h3>
                          {record.appointmentDate || record.AppointmentDate ? (
                            <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                              {new Date(record.appointmentDate || record.AppointmentDate).toLocaleDateString("vi-VN", {
                                year: "numeric",
                                month: "long",
                                day: "numeric",
                                hour: "2-digit",
                                minute: "2-digit",
                              })}
                            </p>
                          ) : null}
                        </div>
                        {record.created || record.Created ? (
                          <span className="text-xs text-gray-400 dark:text-gray-500">
                            {new Date(record.created || record.Created).toLocaleDateString("vi-VN")}
                          </span>
                        ) : null}
                      </div>

                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        {record.symptoms || record.Symptoms ? (
                          <div>
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Triệu chứng
                            </label>
                            <p className="text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                              {record.symptoms || record.Symptoms}
                            </p>
                          </div>
                        ) : null}

                        {record.diagnosis || record.Diagnosis ? (
                          <div>
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Chẩn đoán
                            </label>
                            <p className="text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                              {record.diagnosis || record.Diagnosis}
                            </p>
                          </div>
                        ) : null}

                        {record.treatment || record.Treatment ? (
                          <div>
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Điều trị
                            </label>
                            <p className="text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                              {record.treatment || record.Treatment}
                            </p>
                          </div>
                        ) : null}

                        {record.prescription || record.Prescription ? (
                          <div>
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Đơn thuốc
                            </label>
                            <p className="text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                              {record.prescription || record.Prescription}
                            </p>
                          </div>
                        ) : null}

                        {(record.visionLeft || record.VisionLeft) && (record.visionRight || record.VisionRight) ? (
                          <div className="md:col-span-2">
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Thị lực
                            </label>
                            <div className="flex gap-4">
                              <div className="flex-1 bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                                <span className="text-xs text-gray-500 dark:text-gray-400">Mắt trái:</span>
                                <span className="ml-2 text-sm font-medium text-gray-900 dark:text-white">
                                  {record.visionLeft || record.VisionLeft}
                                </span>
                              </div>
                              <div className="flex-1 bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                                <span className="text-xs text-gray-500 dark:text-gray-400">Mắt phải:</span>
                                <span className="ml-2 text-sm font-medium text-gray-900 dark:text-white">
                                  {record.visionRight || record.VisionRight}
                                </span>
                              </div>
                            </div>
                          </div>
                        ) : null}

                        {record.additionalTests || record.AdditionalTests ? (
                          <div className="md:col-span-2">
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Xét nghiệm bổ sung
                            </label>
                            <p className="text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                              {record.additionalTests || record.AdditionalTests}
                            </p>
                          </div>
                        ) : null}

                        {record.notes || record.Notes ? (
                          <div className="md:col-span-2">
                            <label className="block text-xs font-medium text-gray-500 dark:text-gray-400 mb-1">
                              Ghi chú
                            </label>
                            <p className="text-sm text-gray-900 dark:text-white bg-gray-50 dark:bg-gray-700/50 p-3 rounded">
                              {record.notes || record.Notes}
                            </p>
                          </div>
                        ) : null}
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default DoctorPatients;
