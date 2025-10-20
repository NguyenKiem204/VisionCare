import React from "react";
import {
  Users,
  Home,
  Calendar,
  BookOpen,
  BarChart3,
  TrendingUp,
  DollarSign,
} from "lucide-react";

const statCards = [
  { label: "Users", value: "96", icon: Users, color: "blue" },
  { label: "Residents", value: "3,650", icon: Home, color: "orange" },
  { label: "Events", value: "696", icon: Calendar, color: "red" },
  { label: "Prayer Books", value: "356", icon: BookOpen, color: "cyan" },
];

const colorVariants = {
  blue: "bg-blue-100 text-blue-600",
  orange: "bg-orange-100 text-orange-600",
  red: "bg-red-100 text-red-600",
  cyan: "bg-cyan-100 text-cyan-600",
};

export default function DashboardPage() {
  return (
    <div>
      {/* Stat cards row */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
        {statCards.map((card) => (
          <div
            key={card.label}
            className="bg-white dark:bg-gray-800 rounded-lg shadow-sm p-5 flex items-center gap-5"
          >
            <div className={`p-3 rounded-md ${colorVariants[card.color]}`}>
              <card.icon size={28} />
            </div>
            <div>
              <p className="text-gray-500 dark:text-gray-400 text-sm font-medium">
                {card.label}
              </p>
              <p className="text-2xl font-bold text-gray-800 dark:text-white">
                {card.value}
              </p>
            </div>
          </div>
        ))}
      </div>

      {/* Main charts area */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Revenue Updates Chart */}
        <div className="lg:col-span-2 bg-white dark:bg-gray-800 rounded-lg shadow-sm p-5">
          <h3 className="text-lg font-semibold text-gray-800 dark:text-white mb-1">
            Revenue Updates
          </h3>
          <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">
            Overview of Profit
          </p>
          <div className="h-64 bg-gray-100 dark:bg-gray-900 rounded-md flex items-center justify-center">
            <BarChart3 size={48} className="text-gray-300" />
            <p className="text-gray-400 ml-4">Chart Placeholder</p>
          </div>
        </div>

        {/* Yearly Breakup & Monthly Earnings */}
        <div className="space-y-6">
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm p-5">
            <h3 className="text-lg font-semibold text-gray-800 dark:text-white mb-1">
              Yearly Breakup
            </h3>
            <div className="h-32 bg-gray-100 dark:bg-gray-900 rounded-md flex items-center justify-center">
              <TrendingUp size={32} className="text-gray-300" />
              <p className="text-gray-400 ml-4">Breakup Placeholder</p>
            </div>
          </div>
          <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm p-5">
            <h3 className="text-lg font-semibold text-gray-800 dark:text-white mb-1">
              Monthly Earnings
            </h3>
            <div className="h-32 bg-gray-100 dark:bg-gray-900 rounded-md flex items-center justify-center">
              <DollarSign size={32} className="text-gray-300" />
              <p className="text-gray-400 ml-4">Earnings Placeholder</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
