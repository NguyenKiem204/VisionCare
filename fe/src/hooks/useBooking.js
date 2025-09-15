import { useState } from "react";
import { BOOKING_STEPS } from "../utils/constants";

export const useBooking = () => {
  const [currentStep, setCurrentStep] = useState(1);
  const [data, setData] = useState({});

  const next = () =>
    setCurrentStep((s) => Math.min(s + 1, BOOKING_STEPS.length));
  const back = () => setCurrentStep((s) => Math.max(s - 1, 1));
  const update = (patch) => setData((d) => ({ ...d, ...patch }));

  return { currentStep, data, next, back, update };
};

export default useBooking;
