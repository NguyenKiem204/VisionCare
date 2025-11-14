import { useState, useEffect, useCallback } from "react";
import { BOOKING_STEPS } from "../utils/constants";
import { bookingAPI } from "../services/bookingAPI";
import { signalRService } from "../services/signalRService";
import toast from "react-hot-toast";
import { useAuth } from "../contexts/AuthContext";

export const useBooking = () => {
  const { user } = useAuth();
  const [currentStep, setCurrentStep] = useState(0);
  const [data, setData] = useState({ bookingMode: null });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState(null);
  const [holdToken, setHoldToken] = useState(null);
  const [holdExpiresAt, setHoldExpiresAt] = useState(null);

  const next = () => {
    setCurrentStep((s) => {
      if (s === 0) return 1;
      return s + 1;
    });
  };
  const back = () => {
    setCurrentStep((s) => {
      if (s === 1) return 0;
      return Math.max(0, s - 1);
    });
  };
  const update = (patch) => setData((d) => ({ ...d, ...patch }));

  useEffect(() => {
    signalRService.start().catch((error) => {
      console.error("Failed to start SignalR:", error);
    });

    return () => {
      signalRService.stop().catch(console.error);
    };
  }, []);

  useEffect(() => {
    if (!data.doctorId || !data.date) return;

    const handleSlotHeld = (payload) => {
      if (
        payload.doctorId === data.doctorId &&
        payload.date === data.date?.replace(/-/g, "")
      ) {
        update({ refreshSlots: Date.now() });
      }
    };

    const handleSlotBooked = (payload) => {
      if (
        payload.doctorId === data.doctorId &&
        payload.date === data.date?.replace(/-/g, "")
      ) {
        update({ refreshSlots: Date.now() });
      }
    };

    const handleSlotReleased = (payload) => {
      if (
        payload.doctorId === data.doctorId &&
        payload.date === data.date?.replace(/-/g, "")
      ) {
        update({ refreshSlots: Date.now() });
      }
    };

    const unsubscribeHeld = signalRService.on("SlotHeld", handleSlotHeld);
    const unsubscribeReleased = signalRService.on("SlotReleased", handleSlotReleased);
    const unsubscribeBooked = signalRService.on("SlotBooked", handleSlotBooked);

    if (data.doctorId && data.date) {
      signalRService.joinSlotsGroup(data.doctorId, data.date).catch(console.error);
    }

    return () => {
      unsubscribeHeld();
      unsubscribeBooked();
      unsubscribeReleased();
      if (data.doctorId && data.date) {
        signalRService.leaveSlotsGroup(data.doctorId, data.date).catch(console.error);
      }
    };
  }, [data.doctorId, data.date, data.slotId, holdToken]);

  /**
   */
  const holdSlot = useCallback(async (doctorId, slotId, scheduleDate) => {
    try {
      if (
        holdToken &&
        (data.doctorId !== doctorId || data.slotId !== slotId || data.date !== scheduleDate)
      ) {
        await bookingAPI.releaseHold({
          holdToken,
          doctorId: data.doctorId,
          slotId: data.slotId,
          scheduleDate: data.date,
        }).catch(() => { });
        setHoldToken(null);
        setHoldExpiresAt(null);
      }

      const response = await bookingAPI.holdSlot({
        doctorId,
        slotId,
        scheduleDate,
        customerId: user?.id || null,
      });

      setHoldToken(response.holdToken);
      setHoldExpiresAt(response.expiresAt);

      const holdKey = `hold:${doctorId}:${slotId}:${scheduleDate}`;
      localStorage.setItem(holdKey, JSON.stringify({
        holdToken: response.holdToken,
        expiresAt: response.expiresAt,
        doctorId,
        slotId,
        scheduleDate,
      }));

      const expiresIn = new Date(response.expiresAt) - new Date();
      if (expiresIn > 0) {
        setTimeout(() => {
          setHoldToken(null);
          setHoldExpiresAt(null);
        }, Math.max(0, expiresIn));
      }

      return response;
    } catch (error) {
      toast.error(error.response?.data?.message || "Không thể giữ slot này");
      throw error;
    }
  }, [user?.id]);

  useEffect(() => {
    return () => {
      if (holdToken && data.doctorId && data.slotId && data.date) {
        bookingAPI
          .releaseHold({
            holdToken,
            doctorId: data.doctorId,
            slotId: data.slotId,
            scheduleDate: data.date,
          })
          .catch(() => { })
          .finally(() => {
            setHoldToken(null);
            setHoldExpiresAt(null);
          });
      }
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data.doctorId, data.date]);

  /**
   * Release hold
   */
  const releaseHold = useCallback(async () => {
    if (holdToken && data.doctorId && data.slotId && data.date) {
      try {
        await bookingAPI.releaseHold({
          holdToken,
          doctorId: data.doctorId,
          slotId: data.slotId,
          scheduleDate: data.date,
        });

        const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
        localStorage.removeItem(holdKey);
      } catch (error) {
        console.warn("Release hold error (ignored):", error);
        const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
        localStorage.removeItem(holdKey);
      } finally {
        setHoldToken(null);
        setHoldExpiresAt(null);
      }
    }
  }, [holdToken, data.doctorId, data.slotId, data.date]);

  useEffect(() => {
    const handleBeforeUnload = () => {
      if (holdToken && data.doctorId && data.slotId && data.date) {
        releaseHold();
      }
    };

    const handleVisibilityChange = () => {
      if (document.hidden && holdToken && data.doctorId && data.slotId && data.date) {
        releaseHold();
      }
    };

    window.addEventListener("beforeunload", handleBeforeUnload);
    document.addEventListener("visibilitychange", handleVisibilityChange);

    return () => {
      window.removeEventListener("beforeunload", handleBeforeUnload);
      document.removeEventListener("visibilitychange", handleVisibilityChange);
    };
  }, [releaseHold, holdToken, data.doctorId, data.slotId, data.date]);

  /**
   */
  const submit = async () => {
    setIsSubmitting(true);
    setSubmitError(null);

    try {
      if (!data.doctorId || !data.serviceDetailId || !data.slotId || !data.date || !data.time) {
        const missingFields = [];
        if (!data.doctorId) missingFields.push("Bác sĩ");
        if (!data.serviceDetailId) missingFields.push("Dịch vụ");
        if (!data.slotId) missingFields.push("Khung giờ");
        if (!data.date) missingFields.push("Ngày khám");
        if (!data.time) missingFields.push("Giờ khám");

        const errorMsg = `Vui lòng điền đầy đủ thông tin: ${missingFields.join(", ")}`;
        setSubmitError(errorMsg);
        toast.error(errorMsg);
        throw new Error(errorMsg);
      }

      const request = {
        holdToken: holdToken || null,
        doctorId: data.doctorId,
        serviceDetailId: data.serviceDetailId,
        slotId: data.slotId,
        scheduleDate: data.date,
        startTime: data.time,
        customerId: user?.id || null,
        customerName: data.fullName || null,
        phone: data.phone || null,
        email: data.email || null,
        notes: data.notes || null,
        discountId: data.discountId || null,
      };

      console.log("[Booking] Creating booking request:", request);
      const response = await bookingAPI.createBooking(request);

      const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
      localStorage.removeItem(holdKey);
      setHoldToken(null);
      setHoldExpiresAt(null);

      if (response.paymentUrl) {
        window.location.href = response.paymentUrl;
        return;
      }

      update({
        bookingResponse: response,
        appointmentCode: response.appointmentCode,
        appointmentId: response.appointmentId,
      });

      setCurrentStep(BOOKING_STEPS.length);
      toast.success("Đặt lịch thành công!");

      return response;
    } catch (error) {
      let errorMessage = "Đã xảy ra lỗi khi đặt lịch. Vui lòng thử lại.";
      let errorDetails = [];

      if (error.response?.data) {
        const errorData = error.response.data;

        if (errorData.errors && typeof errorData.errors === 'object') {
          const fieldNames = {
            'DoctorId': 'Bác sĩ',
            'ServiceDetailId': 'Dịch vụ',
            'SlotId': 'Khung giờ',
            'ScheduleDate': 'Ngày khám',
            'StartTime': 'Giờ khám',
            'CustomerId': 'Khách hàng',
            'Phone': 'Số điện thoại',
            'Email': 'Email',
            'CustomerName': 'Họ tên',
            'Notes': 'Ghi chú',
          };

          for (const [key, messages] of Object.entries(errorData.errors)) {
            const fieldName = fieldNames[key] || key;
            if (Array.isArray(messages)) {
              messages.forEach(msg => {
                errorDetails.push(`${fieldName}: ${msg}`);
              });
            } else {
              errorDetails.push(`${fieldName}: ${messages}`);
            }
          }

          if (errorDetails.length > 0) {
            errorMessage = errorDetails.join('\n');
          } else if (errorData.message) {
            errorMessage = errorData.message;
          }
        } else if (errorData.message) {
          errorMessage = errorData.message;
        }
      } else if (error.message) {
        errorMessage = error.message;
      }

      if (error.response?.status === 500 && error.response?.data) {
        const errorData = error.response.data;
        if (errorData.exception) {
          errorMessage = `${errorMessage}\nChi tiết: ${errorData.exception}`;
        } else if (errorData.message) {
          errorMessage = errorData.message;
        }
      }

      console.error("[Booking] Error details:", {
        error,
        status: error.response?.status,
        response: error.response?.data,
        errorMessage,
        errorDetails
      });

      setSubmitError(errorMessage);

      const toastMessage = errorDetails.length > 0
        ? errorDetails[0]
        : errorMessage;
      toast.error(toastMessage, {
        duration: 5000,
      });

      throw error;
    } finally {
      setIsSubmitting(false);
    }
  };

  useEffect(() => {
    if (user && !data.fullName) {
      update({
        fullName: user.fullName || user.name || "",
        email: user.email || "",
        phone: user.phone || "",
        customerId: user.id || null,
      });
    }
  }, [user]);

  useEffect(() => {
    if (data.doctorId && data.slotId && data.date) {
      const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
      const saved = localStorage.getItem(holdKey);
      if (saved) {
        try {
          const holdData = JSON.parse(saved);
          if (new Date(holdData.expiresAt) > new Date()) {
            setHoldToken(holdData.holdToken);
            setHoldExpiresAt(holdData.expiresAt);
            bookingAPI.getAvailableSlots(data.doctorId, data.date, data.serviceTypeId)
              .then(slots => {
                const slot = slots.find(s => s.slotId === data.slotId);
                if (slot && slot.isOnHold && slot.holdByCustomerId === user?.id) {
                } else {
                  setHoldToken(null);
                  setHoldExpiresAt(null);
                  localStorage.removeItem(holdKey);
                }
              })
              .catch(() => {
              });
          } else {
            localStorage.removeItem(holdKey);
          }
        } catch (e) {
          console.error("Error restoring hold:", e);
          localStorage.removeItem(holdKey);
        }
      }
    }
  }, [data.doctorId, data.slotId, data.date, data.serviceTypeId, user?.id]);

  return {
    currentStep,
    data,
    isSubmitting,
    submitError,
    holdToken,
    holdExpiresAt,
    bookingMode: data.bookingMode,
    next,
    back,
    update,
    submit,
    holdSlot,
    releaseHold,
  };
};

export default useBooking;
