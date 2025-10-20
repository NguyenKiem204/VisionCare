import React from "react";

const Contact = () => {
  return (
    <div className="pt-20 min-h-screen">
      <div className="container mx-auto px-4 py-12">
        <h1 className="text-3xl font-bold text-gray-900 mb-8">Liên Hệ</h1>
        <div className="bg-white rounded-lg shadow-lg p-8">
          <h2 className="text-2xl font-semibold mb-6">Thông tin liên hệ</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            <div>
              <h3 className="text-lg font-semibold mb-4">Địa chỉ</h3>
              <p className="text-gray-600">
                123 Nguyễn Huệ, Quận 1, TP.HCM
                <br />
                Việt Nam
              </p>
            </div>
            <div>
              <h3 className="text-lg font-semibold mb-4">Liên hệ</h3>
              <p className="text-gray-600">
                📞 028 1234 5678
                <br />
                📧 info@visioncare.com
                <br />
                🌐 www.visioncare.com
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Contact;
