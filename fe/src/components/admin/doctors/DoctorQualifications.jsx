import React, { useState, useEffect, useCallback } from "react";
import { Plus, Edit, Trash2, Award, FileText } from "lucide-react";
import CertificateModal from "./CertificateModal";
import DegreeModal from "./DegreeModal";
import {
  fetchCertificates,
  fetchDegrees,
  getCertificatesByDoctor,
  getDegreesByDoctor,
  createDoctorCertificate,
  createDoctorDegree,
  deleteDoctorCertificate,
  deleteDoctorDegree,
} from "../../../services/adminDoctorCertificateAPI";

const DoctorQualifications = ({ doctorId, doctorName }) => {
  const [certificates, setCertificates] = useState([]);
  const [degrees, setDegrees] = useState([]);
  const [doctorCertificates, setDoctorCertificates] = useState([]);
  const [doctorDegrees, setDoctorDegrees] = useState([]);
  const [loading, setLoading] = useState(false);
  const [showCertificateModal, setShowCertificateModal] = useState(false);
  const [showDegreeModal, setShowDegreeModal] = useState(false);

  const loadData = useCallback(async () => {
    setLoading(true);
    try {
      const [certsRes, degreesRes, doctorCertsRes, doctorDegreesRes] = await Promise.all([
        fetchCertificates(),
        fetchDegrees(),
        getCertificatesByDoctor(doctorId),
        getDegreesByDoctor(doctorId),
      ]);

      setCertificates(certsRes.data || []);
      setDegrees(degreesRes.data || []);
      setDoctorCertificates(doctorCertsRes.data || []);
      setDoctorDegrees(doctorDegreesRes.data || []);
    } catch (error) {
      console.error("Failed to load qualifications:", error);
    }
    setLoading(false);
  }, [doctorId]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  const handleAddCertificate = async (data) => {
    try {
      await createDoctorCertificate({
        doctorId,
        certificateId: data.certificateId,
        issuedDate: data.issuedDate,
        issuedBy: data.issuedBy,
        certificateImage: data.certificateImage,
        expiryDate: data.expiryDate,
        status: data.status,
      });
      setShowCertificateModal(false);
      loadData();
    } catch (error) {
      console.error("Failed to add certificate:", error);
    }
  };

  const handleAddDegree = async (data) => {
    try {
      await createDoctorDegree({
        doctorId,
        degreeId: data.degreeId,
        issuedDate: data.issuedDate,
        issuedBy: data.issuedBy,
        certificateImage: data.certificateImage,
        status: data.status,
      });
      setShowDegreeModal(false);
      loadData();
    } catch (error) {
      console.error("Failed to add degree:", error);
    }
  };

  const handleDeleteCertificate = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa chứng chỉ này?")) {
      try {
        await deleteDoctorCertificate(id);
        loadData();
      } catch (error) {
        console.error("Failed to delete certificate:", error);
      }
    }
  };

  const handleDeleteDegree = async (id) => {
    if (window.confirm("Bạn có chắc muốn xóa bằng cấp này?")) {
      try {
        await deleteDoctorDegree(id);
        loadData();
      } catch (error) {
        console.error("Failed to delete degree:", error);
      }
    }
  };

  if (loading) {
    return <div className="text-center py-4">Đang tải...</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
          Bằng cấp và chứng chỉ của {doctorName}
        </h3>
        <div className="flex gap-2">
          <button
            onClick={() => setShowCertificateModal(true)}
            className="flex items-center gap-2 px-3 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
          >
            <FileText className="w-4 h-4" />
            Thêm chứng chỉ
          </button>
          <button
            onClick={() => setShowDegreeModal(true)}
            className="flex items-center gap-2 px-3 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition"
          >
            <Award className="w-4 h-4" />
            Thêm bằng cấp
          </button>
        </div>
      </div>

      {/* Certificates */}
      <div>
        <h4 className="text-md font-medium text-gray-700 dark:text-gray-300 mb-3">
          Chứng chỉ ({doctorCertificates.length})
        </h4>
        <div className="grid gap-3">
          {doctorCertificates.map((cert) => (
            <div
              key={cert.id}
              className="flex items-center justify-between p-4 bg-white dark:bg-gray-800 rounded-lg border border-gray-200 dark:border-gray-700"
            >
              <div className="flex items-center gap-3">
                <FileText className="w-5 h-5 text-blue-500" />
                <div>
                  <p className="font-medium text-gray-900 dark:text-white">
                    {cert.certificateName}
                  </p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">
                    Cấp bởi: {cert.issuedBy} | Ngày cấp: {cert.issuedDate}
                    {cert.expiryDate && ` | Hết hạn: ${cert.expiryDate}`}
                  </p>
                </div>
              </div>
              <div className="flex gap-2">
                <button
                  onClick={() => handleDeleteCertificate(cert.id)}
                  className="p-2 text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition"
                >
                  <Trash2 className="w-4 h-4" />
                </button>
              </div>
            </div>
          ))}
          {doctorCertificates.length === 0 && (
            <p className="text-gray-500 dark:text-gray-400 text-center py-4">
              Chưa có chứng chỉ nào
            </p>
          )}
        </div>
      </div>

      {/* Degrees */}
      <div>
        <h4 className="text-md font-medium text-gray-700 dark:text-gray-300 mb-3">
          Bằng cấp ({doctorDegrees.length})
        </h4>
        <div className="grid gap-3">
          {doctorDegrees.map((degree) => (
            <div
              key={degree.id}
              className="flex items-center justify-between p-4 bg-white dark:bg-gray-800 rounded-lg border border-gray-200 dark:border-gray-700"
            >
              <div className="flex items-center gap-3">
                <Award className="w-5 h-5 text-green-500" />
                <div>
                  <p className="font-medium text-gray-900 dark:text-white">
                    {degree.degreeName}
                  </p>
                  <p className="text-sm text-gray-500 dark:text-gray-400">
                    Cấp bởi: {degree.issuedBy} | Ngày cấp: {degree.issuedDate}
                  </p>
                </div>
              </div>
              <div className="flex gap-2">
                <button
                  onClick={() => handleDeleteDegree(degree.id)}
                  className="p-2 text-red-600 hover:bg-red-50 dark:hover:bg-red-900/20 rounded-lg transition"
                >
                  <Trash2 className="w-4 h-4" />
                </button>
              </div>
            </div>
          ))}
          {doctorDegrees.length === 0 && (
            <p className="text-gray-500 dark:text-gray-400 text-center py-4">
              Chưa có bằng cấp nào
            </p>
          )}
        </div>
      </div>

      {/* Certificate Modal */}
      {showCertificateModal && (
        <CertificateModal
          certificates={certificates}
          onClose={() => setShowCertificateModal(false)}
          onSave={handleAddCertificate}
        />
      )}

      {/* Degree Modal */}
      {showDegreeModal && (
        <DegreeModal
          degrees={degrees}
          onClose={() => setShowDegreeModal(false)}
          onSave={handleAddDegree}
        />
      )}
    </div>
  );
};

export default DoctorQualifications;
