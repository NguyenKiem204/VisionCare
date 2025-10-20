import React from "react";
import Button from "../components/common/Button";
import useBooking from "../hooks/useBooking";
import StepIndicator from "../components/booking/StepIndicator";
import PersonalInfo from "../components/booking/PersonalInfo";
import ServiceSelection from "../components/booking/ServiceSelection";
import DoctorTimeSelection from "../components/booking/DoctorTimeSelection";
import Confirmation from "../components/booking/Confirmation";

const Payment = () => (
  <div className="space-y-4">
    <div className="p-4 rounded-xl border bg-white">
      <p className="font-semibold mb-2">Phương thức thanh toán</p>
      <div className="space-y-2">
        <label className="flex items-center gap-2">
          <input type="radio" name="pm" defaultChecked /> Thanh toán tại phòng
          khám
        </label>
        <label className="flex items-center gap-2">
          <input type="radio" name="pm" /> Chuyển khoản ngân hàng
        </label>
        <label className="flex items-center gap-2">
          <input type="radio" name="pm" /> Ví điện tử MoMo (QR)
        </label>
        <label className="flex items-center gap-2">
          <input type="radio" name="pm" /> Thẻ tín dụng
        </label>
      </div>
    </div>
    <div className="text-right">
      <Button variant="accent">Thanh toán</Button>
    </div>
  </div>
);

const Success = () => (
  <div className="text-center">
    <div className="mx-auto w-16 h-16 rounded-full bg-emerald-100 text-emerald-600 flex items-center justify-center mb-4">
      <svg
        className="w-8 h-8"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth="2"
          d="M5 13l4 4L19 7"
        />
      </svg>
    </div>
    <h3 className="text-2xl font-semibold mb-2">Đặt lịch thành công!</h3>
    <p className="text-gray-700 mb-4">Chúng tôi đã gửi SMS/Email xác nhận.</p>
    <a href="/" className="underline text-blue-600">
      Về trang chủ
    </a>
  </div>
);

const Booking = () => {
  const {
    currentStep,
    data,
    update,
    next,
    back,
    submit,
    isSubmitting,
    submitError,
  } = useBooking();

  return (
    <div className="min-h-screen">
      <section className="relative py-12">
        <div className="container mx-auto px-4">
          <h1 className="text-3xl md:text-4xl font-bold text-gray-900 mb-2">
            Đặt Lịch Khám Online
          </h1>
          <p className="text-gray-600 mb-6">
            Làm theo các bước để hoàn tất đặt lịch
          </p>
          <StepIndicator step={currentStep} />

          <div className="bg-white/85 backdrop-blur-sm rounded-2xl shadow p-6 md:p-8">
            {currentStep === 1 && (
              <ServiceSelection
                data={data}
                update={update}
                next={next}
                back={back}
              />
            )}
            {currentStep === 2 && (
              <DoctorTimeSelection
                data={data}
                update={update}
                next={next}
                back={back}
              />
            )}
            {currentStep === 3 && (
              <DoctorTimeSelection
                data={data}
                update={update}
                next={next}
                back={back}
              />
            )}
            {currentStep === 4 && (
              <PersonalInfo data={data} update={update} next={next} />
            )}
            {currentStep === 5 && (
              <Confirmation
                data={data}
                back={back}
                onSubmit={submit}
                isSubmitting={isSubmitting}
                submitError={submitError}
              />
            )}
            {currentStep === 6 && <Payment />}
            {currentStep === 7 && <Success />}
          </div>
        </div>
      </section>
    </div>
  );
};

export default Booking;
