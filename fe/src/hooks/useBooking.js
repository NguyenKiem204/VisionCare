import { useState } from "react";
import { BOOKING_STEPS } from "../utils/constants";
import { mockApi } from "../utils/api";

export const useBooking = () => {
  const [currentStep, setCurrentStep] = useState(1);
  const [data, setData] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState(null);

  const next = () =>
    setCurrentStep((s) => Math.min(s + 1, BOOKING_STEPS.length));
  const back = () => setCurrentStep((s) => Math.max(s - 1, 1));
  const update = (patch) => setData((d) => ({ ...d, ...patch }));

  const submit = async () => {
    setIsSubmitting(true);
    setSubmitError(null);

    try {
      const response = await mockApi.createBooking(data);
      if (response.success) {
        setCurrentStep(BOOKING_STEPS.length); // Go to success step
        return response.data;
      } else {
        throw new Error(response.message || "Đặt lịch thất bại");
      }
    } catch (error) {
      setSubmitError(error.message || "Có lỗi xảy ra khi đặt lịch");
      throw error;
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    currentStep,
    data,
    isSubmitting,
    submitError,
    next,
    back,
    update,
    submit,
  };
};

export default useBooking;
