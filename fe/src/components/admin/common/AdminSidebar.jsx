import { NavLink, useNavigate } from "react-router-dom";
import {
  LayoutDashboard,
  Users,
  Calendar,
  Stethoscope,
  Settings,
  Power,
  ChevronsLeft,
  ChevronsRight,
  UserCheck,
  Wrench,
  MessageSquare,
  CalendarCheck,
  FileText,
  History,
  ListChecks,
  Images,
} from "lucide-react";
import { useAuth } from "../../../contexts/AuthContext";

const adminMenu = [
  { group: "DASHBOARD" },
  { to: "/admin", label: "Dashboard", icon: LayoutDashboard },
  { group: "MANAGEMENT" },
  { to: "/admin/customers", label: "Khách hàng", icon: Users },
  { to: "/admin/doctors", label: "Bác sĩ", icon: UserCheck },
  { to: "/admin/staff", label: "Nhân viên", icon: Users },
  { to: "/admin/services", label: "Dịch vụ", icon: Stethoscope },
  { to: "/admin/service-types", label: "Loại dịch vụ", icon: ListChecks },
  { to: "/admin/appointments", label: "Lịch hẹn", icon: Calendar },
  { to: "/admin/weekly-schedule", label: "Lịch tuần", icon: CalendarCheck },
  { to: "/admin/users", label: "Người dùng", icon: UserCheck },
  { group: "MEDICAL RECORDS" },
  { to: "/admin/medical-history", label: "Lịch sử khám", icon: History },
  { to: "/admin/medical-records", label: "Hồ sơ y tế", icon: FileText },
  { to: "/admin/follow-up", label: "Theo dõi", icon: CalendarCheck },
  { group: "OTHER" },
  { to: "/admin/home-content", label: "Nội dung Home", icon: Images },
  { to: "/admin/equipment", label: "Thiết bị", icon: Wrench },
  { to: "/admin/feedback", label: "Phản hồi", icon: MessageSquare },
  { to: "/admin/settings", label: "Cài đặt", icon: Settings },
];

export default function AdminSidebar({ isOpen, setOpen }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <aside
      className={`fixed top-0 left-0 z-50 bg-white dark:bg-gray-900 shadow-2xl dark:shadow-black/40 transition-colors duration-300 rounded-tl-2xl ${
        isOpen ? "w-64" : "w-20"
      } flex flex-col h-screen`}
    >
      <div className="flex items-center justify-center p-4 border-b h-[73px] dark:border-gray-700">
        {isOpen ? (
          <>
            <div className="w-10 h-10 bg-blue-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-lg">V</span>
            </div>
            <span className="text-xl font-bold ml-2 text-gray-900 dark:text-white">
              VisionCare
            </span>
          </>
        ) : (
          <div className="w-10 h-10 bg-blue-600 rounded-lg flex items-center justify-center">
            <span className="text-white font-bold text-lg">V</span>
          </div>
        )}
      </div>

      <button
        onClick={() => setOpen(!isOpen)}
        className="absolute -right-3 top-7 bg-gray-200 dark:bg-gray-700 border border-gray-300 dark:border-gray-500 p-1.5 rounded-full shadow-md flex items-center justify-center transition-colors duration-200 hover:bg-gray-300 dark:hover:bg-gray-500"
      >
        {isOpen ? (
          <ChevronsLeft size={16} className="text-gray-700 dark:text-white" />
        ) : (
          <ChevronsRight size={16} className="text-gray-700 dark:text-white" />
        )}
      </button>

      <nav className="flex-1 px-4 py-4 space-y-2 overflow-y-auto custom-scrollbar">
        {adminMenu.map((item, index) => {
          if (item.group) {
            return isOpen ? (
              <h3
                key={index}
                className="px-3 pt-4 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wider"
              >
                {item.group}
              </h3>
            ) : (
              <div key={index} className="h-10"></div>
            );
          }
          const Icon = item.icon;
          return (
            <NavLink
              key={item.to}
              to={item.to}
              title={item.label}
              end={item.to === "/admin"}
              className={({ isActive }) =>
                `flex items-center p-2 rounded-lg transition-colors duration-300 ${
                  isActive
                    ? "bg-blue-500 text-white shadow"
                    : "text-gray-700 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-800"
                }`
              }
            >
              <Icon size={20} />
              {isOpen && <span className="ml-4 font-medium">{item.label}</span>}
            </NavLink>
          );
        })}
      </nav>
      <div className="p-4 border-t dark:border-gray-700 mt-auto">
        <div className="flex items-center p-2 rounded-lg">
          <div className="w-10 h-10 bg-blue-500 rounded-full flex items-center justify-center">
            <span className="text-white font-semibold text-sm">
              {user?.username?.charAt(0)?.toUpperCase() ||
                user?.email?.charAt(0)?.toUpperCase() ||
                "A"}
            </span>
          </div>
          {isOpen && (
            <div className="ml-3">
              <p className="font-semibold text-sm text-gray-900 dark:text-white">
                {user?.username || user?.email}
              </p>
              <p className="text-xs text-gray-500 dark:text-gray-400">
                {user?.roleName || "Admin"}
              </p>
            </div>
          )}
          {isOpen && (
            <button
              onClick={() => {
                logout();
                navigate("/login");
              }}
              className="ml-auto text-gray-500 dark:text-gray-400 hover:text-red-500"
              title="Logout"
            >
              <Power size={20} />
            </button>
          )}
        </div>
      </div>
    </aside>
  );
}
