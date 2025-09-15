import React from "react";
import Button from "../common/Button";

const doctors = [
  { id: 1, name: "BS. Nguyễn Văn An", specialty: "Khúc xạ" },
  { id: 2, name: "BS. Trần Thị Bình", specialty: "Võng mạc" },
  { id: 3, name: "BS. Lê Minh Châu", specialty: "Nhi" },
];

const days = Array.from({ length: 14 }).map((_, i) => {
  const d = new Date();
  d.setDate(d.getDate() + i);
  return d;
});

const slots = [
  "08:00",
  "08:30",
  "09:00",
  "09:30",
  "10:00",
  "14:00",
  "14:30",
  "15:00",
  "15:30",
  "16:00",
];

const DoctorTimeSelection = ({ data, update, next, back }) => {
  const [activeDoctor, setActiveDoctor] = React.useState(
    data.doctorId || doctors[0].id
  );
  const [activeDay, setActiveDay] = React.useState(
    data.date || days[0].toISOString().slice(0, 10)
  );
  const [activeTime, setActiveTime] = React.useState(data.time || "08:00");

  const commit = () => {
    update({ doctorId: activeDoctor, date: activeDay, time: activeTime });
    next();
  };

  return (
    <div>
      <h3 className="text-lg font-semibold mb-4">Chọn bác sĩ</h3>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        {doctors.map((d) => (
          <button
            key={d.id}
            onClick={() => setActiveDoctor(d.id)}
            className={`text-left p-4 rounded-xl border ${
              activeDoctor === d.id
                ? "border-blue-500 bg-blue-50"
                : "border-gray-200 bg-white"
            }`}
          >
            <p className="font-semibold text-gray-900">{d.name}</p>
            <p className="text-sm text-gray-600">{d.specialty}</p>
          </button>
        ))}
      </div>

      <h3 className="text-lg font-semibold mb-3">Chọn ngày</h3>
      <div className="flex gap-2 overflow-x-auto pb-2 mb-6">
        {days.map((d) => {
          const key = d.toISOString().slice(0, 10);
          const lbl = d.toLocaleDateString("vi-VN", {
            weekday: "short",
            day: "2-digit",
            month: "2-digit",
          });
          const active = activeDay === key;
          return (
            <button
              key={key}
              onClick={() => setActiveDay(key)}
              className={`px-3 py-2 rounded-lg border whitespace-nowrap ${
                active ? "bg-blue-600 text-white" : "bg-white text-gray-800"
              }`}
            >
              {lbl}
            </button>
          );
        })}
      </div>

      <h3 className="text-lg font-semibold mb-3">Chọn giờ</h3>
      <div className="grid grid-cols-3 md:grid-cols-5 gap-2 mb-6">
        {slots.map((t) => (
          <button
            key={t}
            onClick={() => setActiveTime(t)}
            className={`px-3 py-2 rounded-lg border ${
              activeTime === t
                ? "bg-blue-600 text-white"
                : "bg-white text-gray-800"
            }`}
          >
            {t}
          </button>
        ))}
      </div>

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button variant="accent" onClick={commit}>
          Tiếp tục
        </Button>
      </div>
    </div>
  );
};

export default DoctorTimeSelection;
