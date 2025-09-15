export const services = [
  // NHÓM 1: KHÁM & CHẨN ĐOÁN CƠ BẢN
  {
    id: 1,
    name: "Khám Mắt Tổng Quát",
    description: "Kiểm tra toàn diện thị lực, đo độ cận/viễn/loạn thị",
    category: "Khám tổng quát",
    duration: "45-60 phút",
    price: "200.000 - 300.000 VND",
    doctor: "Tất cả bác sĩ",
    badge: "Phổ biến",
    icon: "Stethoscope",
    features: [
      "Đo thị lực chi tiết",
      "Kiểm tra áp suất mắt",
      "Khám đáy mắt",
      "Tư vấn chuyên sâu"
    ]
  },
  {
    id: 2,
    name: "Đo Thị Lực & Khúc Xạ",
    description: "Đo chính xác độ cận, viễn, loạn thị bằng máy tự động",
    category: "Khám tổng quát",
    duration: "30 phút",
    price: "150.000 VND",
    doctor: "Tất cả bác sĩ",
    icon: "Eye"
  },
  {
    id: 3,
    name: "Chụp Đáy Mắt (OCT)",
    description: "Chụp cắt lớp quang học, phát hiện sớm bệnh võng mạc",
    category: "Khám tổng quát",
    duration: "20 phút",
    price: "400.000 VND",
    doctor: "BS. Trần Thị Bình",
    icon: "Camera"
  },
  {
    id: 4,
    name: "Đo Nhãn Áp",
    description: "Kiểm tra áp suất trong mắt, tầm soát glaucoma",
    category: "Khám tổng quát",
    duration: "15 phút",
    price: "100.000 VND",
    doctor: "Tất cả bác sĩ",
    icon: "Activity"
  },

  // NHÓM 2: ĐIỀU TRỊ CHUYÊN SÂU
  {
    id: 5,
    name: "Điều Trị Cận Thị Tiến Triển",
    description: "Kiểm soát cận thị ở trẻ em bằng kính OK, atropine",
    category: "Điều trị chuyên sâu",
    duration: "Trọn gói 1 năm",
    price: "15.000.000 - 25.000.000 VND",
    doctor: "BS. Lê Minh Châu",
    ageRange: "6-18 tuổi",
    methods: ["Ortho-K", "Atropine 0.01%"],
    icon: "Baby"
  },
  {
    id: 6,
    name: "Điều Trị Glaucoma",
    description: "Điều trị tăng nhãn áp bằng thuốc và laser",
    category: "Điều trị chuyên sâu",
    duration: "Theo phác đồ",
    price: "500.000 - 50.000.000 VND",
    doctor: "BS. Trần Thị Bình",
    methods: ["Thuốc nhỏ mắt", "Laser SLT", "Phẫu thuật"],
    icon: "AlertTriangle"
  },
  {
    id: 7,
    name: "Điều Trị Bệnh Võng Mạc",
    description: "Điều trị đục thủy tinh thể, thoái hóa điểm vàng",
    category: "Điều trị chuyên sâu",
    duration: "Theo phác đồ",
    price: "20.000.000 - 80.000.000 VND",
    doctor: "BS. Trần Thị Bình",
    methods: ["Tiêm thuốc trong dịch kính", "Laser võng mạc"],
    icon: "Heart"
  },

  // NHÓM 3: PHẪU THUẬT
  {
    id: 8,
    name: "Phẫu Thuật Lasik",
    description: "Phẫu thuật laser điều chỉnh khúc xạ",
    category: "Phẫu thuật",
    duration: "15-20 phút/mắt",
    price: "25.000.000 - 45.000.000 VND (cả 2 mắt)",
    doctor: "BS. Nguyễn Văn An",
    badge: "Phổ biến nhất",
    warranty: "2 năm",
    range: "Từ -1.0D đến -10.0D",
    icon: "Zap"
  },
  {
    id: 9,
    name: "Phẫu Thuật Đục Thủy Tinh Thể",
    description: "Thay thế thủy tinh thể bằng lens nhân tạo",
    category: "Phẫu thuật",
    duration: "30-45 phút/mắt",
    price: "15.000.000 - 80.000.000 VND/mắt",
    doctor: "BS. Nguyễn Văn An",
    technology: ["Siêu âm Phaco", "Femto Laser"],
    icon: "Eye"
  },
  {
    id: 10,
    name: "Phẫu Thuật Glaucoma",
    description: "Tạo đường dẫn lưu cho dịch mắt",
    category: "Phẫu thuật",
    duration: "45-60 phút",
    price: "30.000.000 - 60.000.000 VND",
    doctor: "BS. Trần Thị Bình",
    methods: ["Trabeculectomy", "Tube shunt"],
    icon: "Activity"
  },

  // NHÓM 4: THẨM MỸ
  {
    id: 11,
    name: "Cắt Mí Mắt (Blepharoplasty)",
    description: "Phẫu thuật thẩm mỹ mí trên, mí dưới",
    category: "Thẩm mỹ",
    duration: "1-2 giờ",
    price: "8.000.000 - 15.000.000 VND",
    doctor: "BS. Phạm Thu Dung",
    recovery: "7-10 ngày",
    icon: "Sparkles"
  },
  {
    id: 12,
    name: "Nâng Mí, Tạo Mí Kép",
    description: "Tạo đường mí kép tự nhiên, khắc phục sụp mí",
    category: "Thẩm mỹ",
    duration: "1-2 giờ",
    price: "12.000.000 - 20.000.000 VND",
    doctor: "BS. Phạm Thu Dung",
    icon: "Sparkles"
  },

  // NHÓM 5: TRẺ EM
  {
    id: 13,
    name: "Khám Mắt Trẻ Em",
    description: "Khám chuyên biệt cho trẻ dưới 16 tuổi",
    category: "Dịch vụ trẻ em",
    duration: "30-45 phút",
    price: "300.000 - 500.000 VND",
    doctor: "BS. Lê Minh Châu",
    ageRange: "Dưới 16 tuổi",
    content: ["Đo thị lực", "Tầm soát cận thị", "Lác mắt"],
    icon: "Baby"
  },
  {
    id: 14,
    name: "Điều Trị Lác Mắt",
    description: "Điều trị lác mắt bằng kính, che mắt, phẫu thuật",
    category: "Dịch vụ trẻ em",
    duration: "Theo phác đồ",
    price: "500.000 - 25.000.000 VND",
    doctor: "BS. Lê Minh Châu",
    ageRange: "6 tháng - 12 tuổi",
    methods: ["Kính", "Che mắt", "Phẫu thuật"],
    icon: "Eye"
  },

  // NHÓM 6: CẤP CỨU
  {
    id: 15,
    name: "Cấp Cứu Mắt 24/7",
    description: "Xử lý các trường hợp cấp cứu: dị vật, chấn thương",
    category: "Cấp cứu",
    duration: "24/7",
    price: "500.000 VND (ngoài giờ)",
    doctor: "Bác sĩ trực",
    badge: "24/7",
    icon: "Phone"
  }
];

export const serviceCategories = [
  "Tất cả dịch vụ",
  "Khám tổng quát",
  "Phẫu thuật",
  "Điều trị chuyên sâu",
  "Dịch vụ trẻ em",
  "Thẩm mỹ",
  "Cấp cứu"
];
