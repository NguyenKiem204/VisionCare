export const classNames = (...classes) => classes.filter(Boolean).join(" ");

export const formatCurrency = (value) =>
  new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(
    value
  );
