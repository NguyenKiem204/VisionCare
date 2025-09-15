import React from "react";
import Button from "../common/Button";
import Loading from "../common/Loading";

const PersonalInfo = ({ data, update, next }) => {
  const [loading, setLoading] = React.useState(false);

  const onSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setTimeout(() => {
      setLoading(false);
      next();
    }, 400);
  };

  return (
    <form onSubmit={onSubmit} className="grid grid-cols-1 md:grid-cols-2 gap-5">
      <div>
        <label className="block text-sm font-medium mb-1">Họ và tên*</label>
        <input required className="w-full border rounded-lg px-3 py-2" placeholder="Nguyễn Văn A" value={data.fullName || ""} onChange={(e) => update({ fullName: e.target.value })} />
      </div>
      <div>
        <label className="block text-sm font-medium mb-1">Số điện thoại*</label>
        <input required type="tel" className="w-full border rounded-lg px-3 py-2" placeholder="09xx xxx xxx" value={data.phone || ""} onChange={(e) => update({ phone: e.target.value })} />
      </div>
      <div>
        <label className="block text-sm font-medium mb-1">Email*</label>
        <input required type="email" className="w-full border rounded-lg px-3 py-2" placeholder="you@email.com" value={data.email || ""} onChange={(e) => update({ email: e.target.value })} />
      </div>
      <div>
        <label className="block text-sm font-medium mb-1">Ngày sinh*</label>
        <input required type="date" className="w-full border rounded-lg px-3 py-2" value={data.dob || ""} onChange={(e) => update({ dob: e.target.value })} />
      </div>
      <div className="md:col-span-2 flex justify-end gap-3 pt-2">
        <Button type="submit" variant="accent">Tiếp tục</Button>
      </div>
      {loading && <Loading />}
    </form>
  );
};

export default PersonalInfo; 