import React from "react";
import Button from "../common/Button";

const categories = [
  "Khám tổng quát",
  "Phẫu thuật",
  "Điều trị chuyên sâu",
  "Dịch vụ trẻ em",
  "Cấp cứu",
];

const ServiceSelection = ({ data, update, next, back }) => {
  const [active, setActive] = React.useState(categories[0]);
  const toggle = (name) => {
    const chosen = new Set(data.selectedServices || []);
    if (chosen.has(name)) chosen.delete(name);
    else chosen.add(name);
    update({ selectedServices: Array.from(chosen) });
  };

  return (
    <div>
      <div className="flex flex-wrap gap-2 mb-6">
        {categories.map((c) => (
          <button
            key={c}
            onClick={() => setActive(c)}
            className={`px-4 py-2 rounded-full border ${
              active === c ? "bg-blue-600 text-white" : "bg-white text-gray-800"
            }`}
          >
            {c}
          </button>
        ))}
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
        {[1, 2, 3, 4, 5, 6].map((i) => {
          const name = `${active} #${i}`;
          const checked = (data.selectedServices || []).includes(name);
          return (
            <label
              key={i}
              className={`flex items-center justify-between p-4 rounded-xl border ${
                checked
                  ? "border-blue-500 bg-blue-50"
                  : "border-gray-200 bg-white"
              }`}
            >
              <div>
                <p className="font-semibold text-gray-900">{name}</p>
                <p className="text-sm text-gray-600">Thời gian: 30-60 phút</p>
              </div>
              <input
                type="checkbox"
                checked={checked}
                onChange={() => toggle(name)}
                className="w-5 h-5"
              />
            </label>
          );
        })}
      </div>

      <div className="flex justify-between">
        <Button variant="secondary" onClick={back}>
          Quay lại
        </Button>
        <Button variant="accent" onClick={next}>
          Tiếp tục
        </Button>
      </div>
    </div>
  );
};

export default ServiceSelection;
