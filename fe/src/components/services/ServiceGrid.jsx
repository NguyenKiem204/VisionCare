import React from "react";
import ServiceCard from "./ServiceCard";

const ServiceGrid = ({ services, onViewDetails }) => {
  return (
    <section className="py-12">
      <div className="container mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {services.map((service) => (
            <ServiceCard
              key={service.id}
              service={service}
              onViewDetails={onViewDetails}
            />
          ))}
        </div>

        {services.length === 0 && (
          <div className="text-center py-12">
            <p className="text-gray-500 text-lg">Không tìm thấy dịch vụ nào</p>
          </div>
        )}
      </div>
    </section>
  );
};

export default ServiceGrid;
