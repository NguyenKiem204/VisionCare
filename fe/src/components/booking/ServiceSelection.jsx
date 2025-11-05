import React, { useState, useEffect } from "react";
import Button from "../common/Button";
import Loading from "../common/Loading";
import { searchServices } from "../../services/adminServiceAPI";
import { searchServiceTypes } from "../../services/adminServiceTypeAPI";
import toast from "react-hot-toast";

const ServiceSelection = ({ data, update, next, back }) => {
  const [loading, setLoading] = useState(true);
  const [serviceTypes, setServiceTypes] = useState([]);
  const [services, setServices] = useState([]);
  const [activeTypeId, setActiveTypeId] = useState(null);

  // Fetch service types (categories)
  useEffect(() => {
    const loadServiceTypes = async () => {
      try {
        const response = await searchServiceTypes({ page: 1, pageSize: 100 });
        const types = response.data?.data || response.data?.items || [];
        setServiceTypes(types);

        // Set first type as active
        if (types.length > 0 && !activeTypeId) {
          setActiveTypeId(types[0].serviceTypeId || types[0].id);
        }
      } catch (error) {
        console.error("Error loading service types:", error);
        toast.error("Không thể tải danh mục dịch vụ");
      } finally {
        setLoading(false);
      }
    };

    loadServiceTypes();
  }, []);

  // Fetch services when type changes
  useEffect(() => {
    if (!activeTypeId) return;

    const loadServices = async () => {
      try {
        setLoading(true);
        const response = await searchServices({
          serviceTypeId: activeTypeId,
          isActive: true,
          page: 1,
          pageSize: 100,
        });
        const servicesList = response.data?.data || response.data?.items || [];
        setServices(servicesList);
      } catch (error) {
        console.error("Error loading services:", error);
        toast.error("Không thể tải dịch vụ");
      } finally {
        setLoading(false);
      }
    };

    loadServices();
  }, [activeTypeId]);

  const handleServiceSelect = (serviceDetailId) => {
    // Only allow one service selection
    update({ serviceDetailId });
  };

  const handleNext = () => {
    if (!data.serviceDetailId) {
      toast.error("Vui lòng chọn dịch vụ");
      return;
    }
    next();
  };

  return (
    <div>
      <h3 className="text-lg font-semibold mb-4">Chọn loại dịch vụ</h3>

      {loading && serviceTypes.length === 0 ? (
        <Loading />
      ) : (
        <>
          <div className="flex flex-wrap gap-2 mb-6">
            {serviceTypes.map((type) => (
              <button
                key={type.serviceTypeId || type.id}
                onClick={() => setActiveTypeId(type.serviceTypeId || type.id)}
                className={`px-4 py-2 rounded-full border transition-colors ${
                  activeTypeId === (type.serviceTypeId || type.id)
                    ? "bg-blue-600 text-white border-blue-600"
                    : "bg-white text-gray-800 border-gray-200 hover:border-blue-300"
                }`}
              >
                {type.serviceTypeName || type.name || "Unknown"}
              </button>
            ))}
          </div>

          <h3 className="text-lg font-semibold mb-4">Chọn dịch vụ</h3>

          {loading ? (
            <Loading />
          ) : services.length === 0 ? (
            <p className="text-gray-500 text-center py-8">
              Không có dịch vụ nào trong danh mục này
            </p>
          ) : (
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
              {services.map((service) => {
                const serviceDetailId = service.serviceDetailId || service.id;
                const isSelected = data.serviceDetailId === serviceDetailId;
                return (
                  <label
                    key={serviceDetailId}
                    className={`flex items-center justify-between p-4 rounded-xl border cursor-pointer transition-all ${
                      isSelected
                        ? "border-blue-500 bg-blue-50 shadow-md"
                        : "border-gray-200 bg-white hover:border-blue-300"
                    }`}
                    onClick={() => handleServiceSelect(serviceDetailId)}
                  >
                    <div className="flex-1">
                      <p className="font-semibold text-gray-900">
                        {service.serviceDetailName || service.name || "Unknown Service"}
                      </p>
                      <p className="text-sm text-gray-600">
                        {service.description || "Không có mô tả"}
                      </p>
                      <p className="text-sm font-medium text-blue-600 mt-1">
                        {service.cost
                          ? new Intl.NumberFormat("vi-VN", {
                              style: "currency",
                              currency: "VND",
                            }).format(service.cost)
                          : "Liên hệ"}
                      </p>
                    </div>
                    <input
                      type="radio"
                      name="service"
                      checked={isSelected}
                      onChange={() => handleServiceSelect(serviceDetailId)}
                      className="w-5 h-5 text-blue-600"
                    />
                  </label>
                );
              })}
            </div>
          )}
        </>
      )}

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button variant="accent" onClick={handleNext} disabled={loading}>
          Tiếp tục
        </Button>
      </div>
    </div>
  );
};

export default ServiceSelection;
