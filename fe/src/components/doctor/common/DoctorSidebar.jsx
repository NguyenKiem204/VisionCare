import { NavLink } from "react-router-dom";
import {
  LayoutDashboard,
  Users,
  Calendar,
  ClipboardList,
  Activity,
  ChevronsLeft,
  ChevronsRight,
} from "lucide-react";

const doctorMenu = [
  { to: "/doctor", label: "Dashboard", icon: LayoutDashboard },
  { to: "/doctor/patients", label: "Bệnh nhân", icon: Users },
  { to: "/doctor/schedule", label: "Lịch hôm nay", icon: Calendar },
  { to: "/doctor/doctor-schedules", label: "Lịch định kỳ", icon: Calendar },
  { to: "/doctor/absences", label: "Nghỉ phép", icon: ClipboardList },
  { to: "/doctor/ehr", label: "Hồ sơ khám", icon: Activity },
  { to: "/doctor/analytics", label: "Thống kê", icon: Activity },
];

export default function DoctorSidebar({ isOpen, setOpen }) {
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
              <span className="text-white font-bold text-lg">D</span>
            </div>
            <span className="text-xl font-bold ml-2 text-gray-900 dark:text-white">
              VisionCare
            </span>
          </>
        ) : (
          <div className="w-10 h-10 bg-blue-600 rounded-lg flex items-center justify-center">
            <span className="text-white font-bold text-lg">D</span>
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
        {doctorMenu.map((item) => {
          const Icon = item.icon;
          return (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.to === "/doctor"}
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
    </aside>
  );
}


