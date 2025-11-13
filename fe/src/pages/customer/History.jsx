import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useSearchParams } from "react-router-dom";
import toast from "react-hot-toast";
import BookingHistoryTab from "../../components/customer/history/BookingHistoryTab";
import PrescriptionHistoryTab from "../../components/customer/history/PrescriptionHistoryTab";
import MedicalHistoryTimeline from "../../components/customer/history/MedicalHistoryTimeline";
import { customerHistoryAPI } from "../../services/customerHistoryAPI";

const BOOKING_PAGE_SIZE = 6;

const TABS = [
  { id: "bookings", label: "Cuộc hẹn của tôi" },
  { id: "prescriptions", label: "Đơn thuốc" },
  { id: "medical", label: "Hồ sơ điều trị" },
];

const normalizeStatus = (filter) => {
  switch (filter) {
    case "pending":
      return "Pending";
    case "confirmed":
      return "Confirmed";
    case "completed":
      return "Completed";
    case "cancelled":
      return "Cancelled";
    default:
      return undefined;
  }
};

const CustomerHistory = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const initialTab = searchParams.get("tab");
  const defaultTab = TABS.some((tab) => tab.id === initialTab)
    ? initialTab
    : "bookings";

  const [activeTab, setActiveTab] = useState(defaultTab);

  useEffect(() => {
    if (initialTab && initialTab !== activeTab) {
      setActiveTab(defaultTab);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [initialTab]);

  const [statusFilter, setStatusFilter] = useState("all");
  const [timeFilter, setTimeFilter] = useState("all");

  const [bookingState, setBookingState] = useState({
    items: [],
    totalCount: 0,
    page: 1,
  });
  const [bookingLoading, setBookingLoading] = useState(false);
  const [bookingError, setBookingError] = useState(null);

  const [prescriptions, setPrescriptions] = useState([]);
  const [prescriptionLoading, setPrescriptionLoading] = useState(false);
  const [prescriptionError, setPrescriptionError] = useState(null);

  const [medicalHistory, setMedicalHistory] = useState([]);
  const [medicalLoading, setMedicalLoading] = useState(false);
  const [medicalError, setMedicalError] = useState(null);

  const updateTabInUrl = useCallback(
    (tabId) => {
      setSearchParams((prev) => {
        const params = new URLSearchParams(prev);
        params.set("tab", tabId);
        return params;
      });
    },
    [setSearchParams]
  );

  const fetchBookings = useCallback(
    async (pageOverride) => {
      setBookingLoading(true);
      setBookingError(null);

      const requestPage =
        typeof pageOverride === "number" ? pageOverride : bookingState.page;

      const params = {
        page: requestPage,
        pageSize: BOOKING_PAGE_SIZE,
        sortBy: "date",
        sortDescending: true,
      };

      const normalizedStatus = normalizeStatus(statusFilter);
      if (normalizedStatus) {
        params.status = normalizedStatus;
      }

      if (timeFilter === "upcoming") {
        params.upcomingOnly = true;
      } else if (timeFilter === "past") {
        params.pastOnly = true;
      }

      try {
        const response = await customerHistoryAPI.getBookings(params);
        setBookingState({
          items: response.items || [],
          totalCount: response.totalCount || 0,
          page: response.page || requestPage,
        });
      } catch (error) {
        const message =
          error?.response?.data?.message ||
          "Không thể tải lịch sử cuộc hẹn. Vui lòng thử lại.";
        setBookingError(message);
        toast.error(message);
      } finally {
        setBookingLoading(false);
      }
    },
    [bookingState.page, statusFilter, timeFilter]
  );

  const fetchPrescriptions = useCallback(async () => {
    setPrescriptionLoading(true);
    setPrescriptionError(null);
    try {
      const response = await customerHistoryAPI.getPrescriptions();
      setPrescriptions(response || []);
    } catch (error) {
      const message =
        error?.response?.data?.message ||
        "Không thể tải danh sách đơn thuốc. Vui lòng thử lại.";
      setPrescriptionError(message);
      toast.error(message);
    } finally {
      setPrescriptionLoading(false);
    }
  }, []);

  const fetchMedicalHistory = useCallback(async () => {
    setMedicalLoading(true);
    setMedicalError(null);
    try {
      const response = await customerHistoryAPI.getMedicalHistory();
      setMedicalHistory(response || []);
    } catch (error) {
      const message =
        error?.response?.data?.message ||
        "Không thể tải hồ sơ điều trị. Vui lòng thử lại.";
      setMedicalError(message);
      toast.error(message);
    } finally {
      setMedicalLoading(false);
    }
  }, []);

  useEffect(() => {
    if (activeTab === "bookings") {
      fetchBookings(typeof bookingState.page === "number" ? bookingState.page : 1);
    }
  }, [activeTab, fetchBookings]);

  useEffect(() => {
    if (activeTab === "prescriptions" && prescriptions.length === 0) {
      fetchPrescriptions();
    }
  }, [activeTab, prescriptions.length, fetchPrescriptions]);

  useEffect(() => {
    if (activeTab === "medical" && medicalHistory.length === 0) {
      fetchMedicalHistory();
    }
  }, [activeTab, medicalHistory.length, fetchMedicalHistory]);

  useEffect(() => {
    if (activeTab === "bookings") {
      fetchBookings(1);
    }
  }, [statusFilter, timeFilter, activeTab, fetchBookings]);

  const handleTabChange = (tabId) => {
    setActiveTab(tabId);
    updateTabInUrl(tabId);
  };

  const tabContent = useMemo(() => {
    if (activeTab === "bookings") {
      return (
        <BookingHistoryTab
          bookings={bookingState.items}
          loading={bookingLoading}
          statusFilter={statusFilter}
          timeFilter={timeFilter}
          onStatusChange={setStatusFilter}
          onTimeChange={setTimeFilter}
          onRefresh={() => fetchBookings(bookingState.page)}
          page={bookingState.page}
          pageCount={Math.ceil(
            bookingState.totalCount / BOOKING_PAGE_SIZE || 1
          )}
          onPageChange={(nextPage) => {
            if (nextPage !== bookingState.page) {
              fetchBookings(nextPage);
            }
          }}
          error={bookingError}
        />
      );
    }

    if (activeTab === "prescriptions") {
      return (
        <PrescriptionHistoryTab
          prescriptions={prescriptions}
          loading={prescriptionLoading}
          error={prescriptionError}
          onRefresh={fetchPrescriptions}
        />
      );
    }

    return (
      <MedicalHistoryTimeline
        histories={medicalHistory}
        loading={medicalLoading}
        error={medicalError}
        onRefresh={fetchMedicalHistory}
      />
    );
  }, [
    activeTab,
    bookingError,
    bookingLoading,
    bookingState.items,
    bookingState.page,
    bookingState.totalCount,
    fetchBookings,
    fetchMedicalHistory,
    fetchPrescriptions,
    medicalError,
    medicalHistory,
    medicalLoading,
    prescriptionError,
    prescriptionLoading,
    prescriptions,
    statusFilter,
    timeFilter,
  ]);

  return (
    <div className="min-h-screen bg-slate-50 py-10 dark:bg-slate-950">
      <div className="container mx-auto px-4">
        <div className="mb-8 text-center">
          <span className="inline-flex items-center rounded-full bg-yellow-100 px-4 py-1 text-xs font-semibold uppercase tracking-wide text-yellow-700">
            VisionCare Care+
          </span>
          <h1 className="mt-4 text-3xl font-bold text-slate-900 dark:text-white">
            Lịch sử khám & điều trị của bạn
          </h1>
          <p className="mt-2 text-sm text-slate-500 dark:text-slate-400">
            Theo dõi hành trình chăm sóc đôi mắt với thông tin chi tiết về lịch
            hẹn, đơn thuốc và hồ sơ điều trị được cập nhật theo thời gian thực.
          </p>
        </div>

        <div className="mx-auto max-w-5xl">
          <div className="mb-6 grid gap-3 rounded-2xl border border-slate-100 bg-white p-2 shadow-md dark:border-slate-800 dark:bg-slate-900 sm:grid-cols-3">
            {TABS.map((tab) => {
              const isActive = tab.id === activeTab;
              return (
                <button
                  key={tab.id}
                  onClick={() => handleTabChange(tab.id)}
                  className={`rounded-xl px-4 py-3 text-sm font-semibold transition ${
                    isActive
                      ? "bg-gradient-to-r from-yellow-400 to-orange-500 text-white shadow-md"
                      : "bg-transparent text-slate-600 hover:bg-slate-50 hover:text-slate-900 dark:text-slate-300 dark:hover:bg-slate-800/70"
                  }`}
                >
                  {tab.label}
                </button>
              );
            })}
          </div>

          <div className="rounded-3xl border border-slate-100 bg-white p-6 shadow-lg dark:border-slate-800 dark:bg-slate-900/90">
            {tabContent}
          </div>
        </div>
      </div>
    </div>
  );
};

export default CustomerHistory;

