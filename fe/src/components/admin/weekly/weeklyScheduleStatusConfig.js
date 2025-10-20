import { FaUpload, FaArchive } from "react-icons/fa";

export const statusConfig = {
  DRAFT: {
    label: "Nháp",
    color: "bg-gray-400 text-white",
    button: {
      label: "Xuất bản",
      color: "bg-green-500 hover:bg-green-600 text-white",
      icon: FaUpload,
      nextStatus: "PUBLISHED",
    },
  },
  PUBLISHED: {
    label: "Đã xuất bản",
    color: "bg-green-500 text-white",
    button: {
      label: "Lưu trữ",
      color: "bg-red-500 hover:bg-red-600 text-white",
      icon: FaArchive,
      nextStatus: "ARCHIVED",
    },
  },
  ARCHIVED: {
    label: "Lưu trữ",
    color: "bg-red-500 text-white",
    button: {
      label: "Xuất bản",
      color: "bg-green-500 hover:bg-green-600 text-white",
      icon: FaUpload,
      nextStatus: "PUBLISHED",
    },
  },
};
