import React from "react";
import Button from "../components/common/Button";
import useBooking from "../hooks/useBooking";
import StepIndicator from "../components/booking/StepIndicator";
import PersonalInfo from "../components/booking/PersonalInfo";
import ServiceSelection from "../components/booking/ServiceSelection";
import DoctorSelection from "../components/booking/DoctorSelection";
import DoctorTimeSelection from "../components/booking/DoctorTimeSelection";
import BookingModeSelector from "../components/booking/BookingModeSelector";
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

const Success = ({ data }) => {
  const appointmentCode = data?.appointmentCode || data?.bookingResponse?.appointmentCode;
  
  return (
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
      {appointmentCode && (
        <div className="mb-4 p-4 bg-blue-50 rounded-lg inline-block">
          <p className="text-sm text-gray-600 mb-1">Mã đặt lịch của bạn:</p>
          <p className="text-xl font-bold text-blue-600">{appointmentCode}</p>
        </div>
      )}
      <p className="text-gray-700 mb-4">
        Chúng tôi đã gửi SMS/Email xác nhận đến bạn.
      </p>
      <div className="flex flex-col sm:flex-row gap-4 justify-center">
        {appointmentCode && (
          <a
            href={`/booking/status?code=${appointmentCode}`}
            className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors inline-block"
          >
            Xem chi tiết đặt lịch
          </a>
        )}
        <a
          href="/"
          className="px-6 py-2 bg-gray-200 text-gray-800 rounded-lg hover:bg-gray-300 transition-colors inline-block"
        >
          Về trang chủ
        </a>
      </div>
    </div>
  );
};

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
    holdSlot,
    releaseHold,
    bookingMode,
  } = useBooking();

  // Determine which component to render based on step and booking mode
  const renderStep = () => {
    // Step 0: Select booking mode
    if (currentStep === 0) {
      return (
        <BookingModeSelector
          selectedMode={bookingMode}
          onModeChange={(mode) => update({ bookingMode: mode })}
          onNext={next}
        />
      );
    }

    // Flow: Book by Doctor
    if (bookingMode === "doctor") {
      if (currentStep === 1) {
        // Select doctor
        return (
          <DoctorSelection
            data={data}
            update={update}
            next={next}
            back={back}
            filterByService={false}
          />
        );
      }
      if (currentStep === 2) {
        // Select service
        return (
          <ServiceSelection
            data={data}
            update={update}
            next={next}
            back={back}
          />
        );
      }
      if (currentStep === 3) {
        // Select date/time
        return (
          <DoctorTimeSelection
            data={data}
            update={update}
            next={next}
            back={back}
            holdSlot={holdSlot}
            releaseHold={releaseHold}
          />
        );
      }
      if (currentStep === 4) {
        // Personal info
        return <PersonalInfo data={data} update={update} next={next} back={back} />;
      }
      if (currentStep === 5) {
        // Confirmation
        return (
          <Confirmation
            data={data}
            back={back}
            onSubmit={submit}
            isSubmitting={isSubmitting}
            submitError={submitError}
          />
        );
      }
    }

    // Flow: Book by Service
    if (bookingMode === "service") {
      if (currentStep === 1) {
        // Select service
        return (
          <ServiceSelection
            data={data}
            update={update}
            next={next}
            back={back}
          />
        );
      }
      if (currentStep === 2) {
        // Select doctor (filtered by service)
        return (
          <DoctorSelection
            data={data}
            update={update}
            next={next}
            back={back}
            filterByService={true}
          />
        );
      }
      if (currentStep === 3) {
        // Select date/time
        return (
          <DoctorTimeSelection
            data={data}
            update={update}
            next={next}
            back={back}
            holdSlot={holdSlot}
            releaseHold={releaseHold}
          />
        );
      }
      if (currentStep === 4) {
        // Personal info
        return <PersonalInfo data={data} update={update} next={next} back={back} />;
      }
      if (currentStep === 5) {
        // Confirmation
        return (
          <Confirmation
            data={data}
            back={back}
            onSubmit={submit}
            isSubmitting={isSubmitting}
            submitError={submitError}
          />
        );
      }
    }

    if (currentStep === 6) return <Payment />;
    if (currentStep === 7) return <Success data={data} />;

    return null;
  };

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
          {currentStep > 0 && <StepIndicator step={currentStep} />}

          <div className="bg-white/85 backdrop-blur-sm rounded-2xl shadow p-6 md:p-8">
            {renderStep()}
          </div>
        </div>
      </section>
    </div>
  );
};

export default Booking;
