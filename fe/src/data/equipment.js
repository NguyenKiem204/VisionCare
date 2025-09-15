export const equipment = [
  // THIẾT BỊ CHẨN ĐOÁN & KHÁM
  {
    id: 1,
    name: "Máy đo thị lực tự động Topcon CV-5000",
    category: "Chẩn đoán & Khám",
    origin: "Nhật Bản",
    image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    functions: [
      "Đo khúc xạ tự động",
      "Đo thị lực từ xa và gần", 
      "Kiểm tra thị trường"
    ],
    advantages: [
      "Chính xác cao 99.8%",
      "Thời gian khám nhanh chóng",
      "Thoải mái cho bệnh nhân"
    ],
    applications: ["Khám tổng quát", "Đo độ cận/viễn thị"],
    value: "2.5 tỷ VND"
  },
  {
    id: 2,
    name: "Máy chụp đáy mắt OCT Zeiss Cirrus HD 5000",
    category: "Chẩn đoán & Khám",
    origin: "Đức",
    image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    technology: "Optical Coherence Tomography",
    functions: [
      "Chụp cắt lớp võng mạc",
      "Phân tích thần kinh thị giác",
      "Theo dõi tiến triển bệnh"
    ],
    resolution: "5 micromet",
    applications: ["Chẩn đoán glaucoma", "Bệnh võng mạc"],
    value: "8 tỷ VND"
  },
  {
    id: 3,
    name: "Máy đo nhãn áp Goldmann",
    category: "Chẩn đoán & Khám",
    origin: "Thụy Sĩ",
    image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    standard: "WHO Gold Standard",
    applications: ["Tầm soát và theo dõi glaucoma"],
    value: "500 triệu VND"
  },
  {
    id: 4,
    name: "Kính sinh hiển vi khám Zeiss S7",
    category: "Chẩn đoán & Khám",
    origin: "Đức",
    image: "https://images.unsplash.com/photo-1551601651-2a8555f1a136?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    features: ["Zoom 1:6", "LED illumination"],
    applications: ["Khám chi tiết các bộ phận mắt"],
    value: "1.2 tỷ VND"
  },

  // THIẾT BỊ PHẪU THUẬT & LASER
  {
    id: 5,
    name: "Hệ thống Laser Femtosecond Zeiss VisuMax",
    category: "Phẫu thuật & Laser",
    origin: "Đức",
    image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    technology: "ReLEx SMILE",
    applications: ["Phẫu thuật khúc xạ không dao"],
    advantages: [
      "Vết mổ nhỏ chỉ 2-4mm",
      "Hồi phục nhanh",
      "Độ chính xác cao"
    ],
    value: "15 tỷ VND"
  },
  {
    id: 6,
    name: "Máy Phẫu thuật đục thủy tinh thể Alcon Centurion",
    category: "Phẫu thuật & Laser",
    origin: "Mỹ",
    image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    technology: "Intelligent Phaco",
    features: [
      "Kiểm soát áp suất tự động",
      "Rung động siêu âm tối ưu",
      "Bảo vệ nội mô giác mạc"
    ],
    lensTypes: ["Đa tiêu cự", "Toric"],
    value: "12 tỷ VND"
  },
  {
    id: 7,
    name: "Laser Argon cho Võng mạc",
    category: "Phẫu thuật & Laser",
    origin: "Đức",
    image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    applications: [
      "Điều trị võng mạc đái tháo đường",
      "Đông máu võng mạc",
      "Điều trị glaucoma"
    ],
    value: "3 tỷ VND"
  },
  {
    id: 8,
    name: "Hệ thống Phẫu thuật Vitrectomy Alcon Constellation",
    category: "Phẫu thuật & Laser",
    origin: "Mỹ",
    image: "https://images.unsplash.com/photo-1551601651-2a8555f1a136?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    cuttingSpeed: "7500 cắt/phút",
    applications: ["Phẫu thuật dịch kính", "Võng mạc"],
    value: "18 tỷ VND"
  },

  // THIẾT BỊ HỖ TRỢ
  {
    id: 9,
    name: "Máy siêu âm mắt A/B Scan",
    category: "Thiết bị hỗ trợ",
    origin: "Mỹ",
    image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    functions: ["Đo chiều dài nhãn cầu", "IOL calculation"],
    applications: ["Chuẩn bị phẫu thuật đục thủy tinh thể"],
    value: "800 triệu VND"
  },
  {
    id: 10,
    name: "Máy đo độ cong giác mạc Topography",
    category: "Thiết bị hỗ trợ",
    origin: "Nhật Bản",
    image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    functions: ["Bản đồ 3D giác mạc"],
    applications: ["Chẩn đoán cận thị", "Loạn thị"],
    value: "1.5 tỷ VND"
  },
  {
    id: 11,
    name: "Thiết bị Ortho-K (OK Lens)",
    category: "Thiết bị hỗ trợ",
    origin: "Mỹ",
    image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    functions: ["Thiết kế kính cứng đeo qua đêm"],
    applications: ["Kiểm soát cận thị tiến triển ở trẻ em"],
    effectiveness: "Giảm 50-70% tốc độ tăng cận thị",
    value: "2 tỷ VND"
  },

  // PHÒNG MỔ & VÔ TRÙNG
  {
    id: 12,
    name: "Phòng mổ Laminar Air Flow",
    category: "Phòng mổ & Vô trùng",
    origin: "Đức",
    image: "https://images.unsplash.com/photo-1551601651-2a8555f1a136?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    standard: "ISO 14644 Class 5",
    features: [
      "Luồng khí vô trùng",
      "Áp suất dương",
      "Nhiệt độ và độ ẩm kiểm soát"
    ],
    safety: "99.99% vô trùng",
    value: "5 tỷ VND"
  },
  {
    id: 13,
    name: "Hệ thống Monitor 4K",
    category: "Phòng mổ & Vô trùng",
    origin: "Nhật Bản",
    image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    resolution: "Ultra HD 4K",
    applications: ["Hiển thị real-time trong phẫu thuật"],
    value: "1 tỷ VND"
  },
  {
    id: 14,
    name: "Bàn mổ điều chỉnh điện tử",
    category: "Phòng mổ & Vô trùng",
    origin: "Đức",
    image: "https://images.unsplash.com/photo-1559757148-5c350d0d3c56?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    features: [
      "Điều chỉnh đa chiều",
      "Memory positions",
      "Ergonomic design"
    ],
    value: "3 tỷ VND"
  },
  {
    id: 15,
    name: "Hệ thống khử trùng Plasma",
    category: "Phòng mổ & Vô trùng",
    origin: "Mỹ",
    image: "https://images.unsplash.com/photo-1582750433449-648ed127bb54?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80",
    technology: "Low-temperature plasma",
    applications: ["Khử trùng dụng cụ nhạy cảm nhiệt"],
    value: "1.8 tỷ VND"
  }
];

export const equipmentCategories = [
  "Chẩn đoán & Khám",
  "Phẫu thuật & Laser",
  "Thiết bị hỗ trợ", 
  "Phòng mổ & Vô trùng"
];
