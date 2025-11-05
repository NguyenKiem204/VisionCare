import { useState, useEffect, useCallback } from "react";
import { BOOKING_STEPS } from "../utils/constants";
import { bookingAPI } from "../services/bookingAPI";
import { signalRService } from "../services/signalRService";
import toast from "react-hot-toast";
import { useAuth } from "../contexts/AuthContext";

export const useBooking = () => {
  const { user } = useAuth();
  const [currentStep, setCurrentStep] = useState(0); // Start at step 0 (mode selection)
  const [data, setData] = useState({ bookingMode: null });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState(null);
  const [holdToken, setHoldToken] = useState(null);
  const [holdExpiresAt, setHoldExpiresAt] = useState(null);

  const next = () => {
    setCurrentStep((s) => {
      // If at step 0, go to step 1
      if (s === 0) return 1;
      // Otherwise increment normally
      return s + 1;
    });
  };
  const back = () => {
    setCurrentStep((s) => {
      // If at step 1, go back to step 0 (mode selection)
      if (s === 1) return 0;
      // Otherwise decrement normally
      return Math.max(0, s - 1);
    });
  };
  const update = (patch) => setData((d) => ({ ...d, ...patch }));

  // Initialize SignalR connection
  useEffect(() => {
    signalRService.start().catch((error) => {
      console.error("Failed to start SignalR:", error);
    });

    return () => {
      signalRService.stop().catch(console.error);
    };
  }, []);

  // Set up SignalR listeners for slot updates
  useEffect(() => {
    if (!data.doctorId || !data.date) return;

    const handleSlotHeld = (payload) => {
      if (
        payload.doctorId === data.doctorId &&
        payload.date === data.date?.replace(/-/g, "")
      ) {
        // Bất kỳ slot nào bị giữ trong ngày này → refresh để cập nhật trạng thái "Đang giữ"
        // Refresh ngay cả khi là slot của mình (để update UI state)
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

    // Join slots group
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
   * Hold a slot before booking
   */
  const holdSlot = useCallback(async (doctorId, slotId, scheduleDate) => {
    try {
      // If already holding a different slot/day/doctor → release first
      if (
        holdToken &&
        (data.doctorId !== doctorId || data.slotId !== slotId || data.date !== scheduleDate)
      ) {
        await bookingAPI.releaseHold({
          holdToken,
          doctorId: data.doctorId,
          slotId: data.slotId,
          scheduleDate: data.date,
        }).catch(() => {});
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
      
      // Lưu holdToken vào localStorage để restore khi reload
      const holdKey = `hold:${doctorId}:${slotId}:${scheduleDate}`;
      localStorage.setItem(holdKey, JSON.stringify({
        holdToken: response.holdToken,
        expiresAt: response.expiresAt,
        doctorId,
        slotId,
        scheduleDate,
      }));

      // Set up auto-release after expiration
      const expiresIn = new Date(response.expiresAt) - new Date();
      if (expiresIn > 0) {
        setTimeout(() => {
          // Hold token will be cleared automatically by backend TTL
          // Just clear local state
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

  // Release hold when date/doctor changes or component unmounts
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
          .catch(() => {})
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
        
        // Clear from localStorage
        const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
        localStorage.removeItem(holdKey);
      } catch (error) {
        console.warn("Release hold error (ignored):", error);
        // Still clear from localStorage even if API fails
        const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
        localStorage.removeItem(holdKey);
      } finally {
        setHoldToken(null);
        setHoldExpiresAt(null);
      }
    }
  }, [holdToken, data.doctorId, data.slotId, data.date]);

  // Release immediately when tab closes, route changes, or page becomes hidden
  useEffect(() => {
    const handleBeforeUnload = () => {
      // fire-and-forget; navigator.sendBeacon not used due to auth header
      if (holdToken && data.doctorId && data.slotId && data.date) {
        releaseHold();
      }
    };

    const handleVisibilityChange = () => {
      // Khi tab trở thành hidden (chuyển tab, minimize, etc.) → release hold
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
   * Submit booking
   */
  const submit = async () => {
    setIsSubmitting(true);
    setSubmitError(null);

    try {
      // Prepare booking request
      const request = {
        holdToken: holdToken || null,
        doctorId: data.doctorId,
        serviceDetailId: data.serviceDetailId,
        slotId: data.slotId,
        scheduleDate: data.date, // YYYY-MM-DD
        startTime: data.time, // HH:mm
        customerId: user?.id || null,
        customerName: data.fullName || null,
        phone: data.phone || null,
        email: data.email || null,
        notes: data.notes || null,
        discountId: data.discountId || null,
      };

      const response = await bookingAPI.createBooking(request);

      // Release hold (will be done by backend, but good practice)
      const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
      localStorage.removeItem(holdKey);
      setHoldToken(null);
      setHoldExpiresAt(null);

      // If payment is needed, redirect to payment URL
      if (response.paymentUrl) {
        window.location.href = response.paymentUrl;
        return;
      }

      // Store booking info for confirmation page
      update({
        bookingResponse: response,
        appointmentCode: response.appointmentCode,
        appointmentId: response.appointmentId,
      });

      setCurrentStep(BOOKING_STEPS.length); // Go to success step
      toast.success("Đặt lịch thành công!");

      return response;
    } catch (error) {
      const errorMessage =
        error.response?.data?.message || error.message || "Có lỗi xảy ra khi đặt lịch";
      setSubmitError(errorMessage);
      toast.error(errorMessage);
      throw error;
    } finally {
      setIsSubmitting(false);
    }
  };

  // Pre-fill user data if logged in
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

  // Restore holdToken from localStorage khi vào lại trang
  useEffect(() => {
    if (data.doctorId && data.slotId && data.date) {
      const holdKey = `hold:${data.doctorId}:${data.slotId}:${data.date}`;
      const saved = localStorage.getItem(holdKey);
      if (saved) {
        try {
          const holdData = JSON.parse(saved);
          // Check xem hold còn hạn không
          if (new Date(holdData.expiresAt) > new Date()) {
            setHoldToken(holdData.holdToken);
            setHoldExpiresAt(holdData.expiresAt);
            // Validate hold với backend
            bookingAPI.getAvailableSlots(data.doctorId, data.date, data.serviceTypeId)
              .then(slots => {
                const slot = slots.find(s => s.slotId === data.slotId);
                if (slot && slot.isOnHold && slot.holdByCustomerId === user?.id) {
                  // Hold vẫn còn hiệu lực, giữ lại
                } else {
                  // Hold không còn, clear
                  setHoldToken(null);
                  setHoldExpiresAt(null);
                  localStorage.removeItem(holdKey);
                }
              })
              .catch(() => {
                // Ignore error
              });
          } else {
            // Hold đã hết hạn
            localStorage.removeItem(holdKey);
          }
        } catch (e) {
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
