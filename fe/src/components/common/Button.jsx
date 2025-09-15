import React from "react";

const base =
  "inline-flex items-center justify-center rounded-full font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-60 disabled:cursor-not-allowed";

const variants = {
  primary: "bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-300",
  secondary:
    "bg-white text-gray-900 border border-gray-300 hover:border-blue-400 hover:text-blue-700",
  accent: "bg-orange-500 text-white hover:bg-orange-600 focus:ring-orange-300",
  ghost: "bg-transparent text-gray-900 hover:bg-gray-100",
};

const sizes = {
  sm: "px-4 py-2 text-sm",
  md: "px-6 py-3 text-base",
  lg: "px-8 py-4 text-lg",
};

const Button = ({
  variant = "primary",
  size = "md",
  className = "",
  children,
  ...props
}) => {
  return (
    <button
      className={`${base} ${variants[variant]} ${sizes[size]} ${className}`}
      {...props}
    >
      {children}
    </button>
  );
};

export default Button;
