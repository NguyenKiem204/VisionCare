# PROMPT HOÀN CHỈNH: WEBSITE VISIONCARE - TRUNG TÂM CHĂM SÓC MẮT

## YÊU CẦU TỔNG QUAN
Tạo website VisionCare - trung tâm chăm sóc mắt hoàn chỉnh với giao diện hiện đại, thân thiện và chuyên nghiệp sử dụng ReactJS và Tailwind CSS.

## THIẾT KẾ LOGO VISIONCARE

### Logo Concept
- **Icon**: Mắt stylized kết hợp với hình tròn/lens
  - Thiết kế tối giản, hiện đại
  - Gradient từ xanh dương (#3B82F6) sang xanh lá (#10B981)
  - Đường nét mềm mại, tạo cảm giác tin cậy
  
- **Typography**: 
  - Font: Sans-serif, bold cho "Vision", regular cho "Care"  
  - "Vision" màu xanh dương đậm (#1E40AF)
  - "Care" màu xanh lá (#059669)
  - Kích thước cân đối, dễ đọc ở mọi kích thước

- **Layout Options**:
  - Horizontal: Icon + Text ngang hàng
  - Vertical: Icon trên, text dưới  
  - Icon-only version cho favicon và mobile

## MÀU SẮC CHỦ ĐẠO
- **Màu chính**: Xanh dương nhạt (#3B82F6) - tượng trưng cho sự tin cậy và y tế
- **Màu phụ**: Xanh lá nhạt (#10B981) - tượng trưng cho sự tươi mới và sức khỏe
- **Màu nền**: Trắng (#FFFFFF) và xám nhạt (#F8FAFC)
- **Màu nhấn**: Cam nhẹ (#F59E0B) cho các CTA buttons
- **Màu text**: Xám đậm (#1F2937) và xám vừa (#6B7280)

## CẤU TRÚC WEBSITE

### 1. HEADER NAVIGATION
- Logo VisionCare (bên trái) - Logo kết hợp icon mắt stylized + text "VisionCare"
- Menu điều hướng: Trang Chủ | Dịch Vụ | Trang Thiết Bị | Đặt Lịch | Liên Hệ
- Nút "Đặt Lịch Ngay" nổi bật (màu cam)
- Responsive hamburger menu cho mobile
- Sticky header khi scroll

### 2. TRANG CHỦ (HOME) - CHI TIẾT TỪNG SECTION

#### 2.1 Hero Section với Slideshow (Full screen height)
- **Slide 1**: "VisionCare - Chăm Sóc Mắt Chuyên Nghiệp" 
  - Background: Hình ảnh bác sĩ thân thiện đang khám mắt cho bệnh nhân
  - Overlay gradient: rgba(59, 130, 246, 0.7)
  - Title: H1 lớn, màu trắng, font-bold
  - Subtitle: "20+ năm kinh nghiệm - Công nghệ hiện đại - Đội ngũ chuyên gia"
  - CTA buttons: "Đặt Lịch Ngay" (cam) + "Tìm Hiểu Thêm" (outline trắng)

- **Slide 2**: "Công Nghệ Tiên Tiến"
  - Background: Thiết bị khám mắt hiện đại, phòng khám sang trọng
  - Text: "Máy móc nhập khẩu từ Đức - Chẩn đoán chính xác 99.8%"
  - Icon tech: Các icon thiết bị hiện đại

- **Slide 3**: "Dịch Vụ Toàn Diện"
  - Background: Phòng khám sạch sẽ, hiện đại
  - Text: "Từ khám tổng quát đến phẫu thuật chuyên sâu"
  - Stats overlay: Số liệu thành công

- Auto-play 5 giây/slide, có dots navigation và arrow controls
- Parallax effect khi scroll

#### 2.2 Quick Stats Bar (Sticky dưới hero)
- Container ngang, background gradient nhẹ
- 4 thống kê nổi bật với counter animation:
  - "15,000+ Bệnh nhân tin tưởng"
  - "20+ Năm kinh nghiệm"
  - "99.8% Tỷ lệ thành công"
  - "24/7 Hỗ trợ khẩn cấp"

#### 2.3 Về VisionCare (Section với background trắng)
- Layout 2 cột: Text bên trái, hình ảnh bên phải
- **Nội dung**:
  - Tiêu đề: "Tại sao VisionCare là sự lựa chọn hàng đầu?"
  - Đoạn mở đầu: Giới thiệu tổng quan về trung tâm
  - 4 điểm nổi bật với checkmark icons:
    - Đội ngũ bác sĩ giàu kinh nghiệm
    - Công nghệ thiết bị hiện đại nhất
    - Dịch vụ tận tâm, chu đáo
    - Chi phí hợp lý, bảo hiểm y tế
- **Hình ảnh**: Gallery nhỏ 2x2 về phòng khám, bác sĩ, thiết bị
- Button: "Tìm hiểu lịch sử VisionCare"

#### 2.4 Chuyên Khoa & Dịch Vụ Nổi Bật (Section background xám nhạt)
- Tiêu đề section: "Chuyên Khoa & Dịch Vụ Chuyên Sâu"
- **Grid 3x3 responsive** (3 cột desktop, 2 cột tablet, 1 cột mobile):

**Hàng 1 - Chuyên khoa chính:**
1. **Khúc Xạ & Cận Thị**
   - Icon: Glasses/Eye với laser
   - Mô tả: "Điều trị cận thị, viễn thị, loạn thị bằng công nghệ Lasik"
   - Badge: "Phổ biến nhất"

2. **Võng Mạc**  
   - Icon: Eye với blood vessels
   - Mô tả: "Chẩn đoán và điều trị các bệnh lý võng mạc, đục thủy tinh thể"
   - Badge: "Chuyên sâu"

3. **Glaucoma (Tăng nhãn áp)**
   - Icon: Eye với pressure waves  
   - Mô tả: "Tầm soát sớm và điều trị bệnh tăng nhãn áp"
   - Badge: "Cấp thiết"

**Hàng 2 - Dịch vụ đặc biệt:**
4. **Nhãn Khoa Trẻ Em**
   - Icon: Child với glasses
   - Mô tả: "Khám và điều trị chuyên biệt cho trẻ em dưới 16 tuổi"

5. **Phẫu Thuật Thẩm Mỹ Mắt**
   - Icon: Eye với sparkle
   - Mô tả: "Cắt mí, nâng mí, điều trị quầng thâm chuyên nghiệp"

6. **Cấp Cứu Mắt 24/7**
   - Icon: Emergency cross
   - Mô tả: "Sẵn sàng hỗ trợ các trường hợp cấp cứu về mắt"
   - Badge: "24/7"

**Hàng 3 - Dịch vụ hỗ trợ:**
7. **Tư Vấn Online**
   - Icon: Video call
   - Mô tả: "Tư vấn từ xa qua video call với bác sĩ"

8. **Khám Định Kỳ**
   - Icon: Calendar với heart
   - Mô tả: "Chương trình theo dõi sức khỏe mắt dài hạn"

9. **Bảo Hiểm Y Tế**
   - Icon: Shield
   - Mô tả: "Hỗ trợ thanh toán qua các loại bảo hiểm y tế"

- Mỗi card có hover effect, shadow và scale
- Button cuối section: "Xem Tất Cả Dịch Vụ"

#### 2.5 Đội Ngũ Bác Sĩ Tiêu Biểu (Section background trắng)
- Tiêu đề: "Đội Ngũ Bác Sĩ Hàng Đầu"
- **Layout carousel/slider** hiển thị 4 bác sĩ cùng lúc (responsive)

**Thông tin mỗi bác sĩ:**
1. **BS. CKII Nguyễn Văn An** - Trưởng Khoa
   - Ảnh: Professional headshot
   - Chuyên môn: "Chuyên gia về phẫu thuật khúc xạ và Lasik"  
   - Kinh nghiệm: "25+ năm kinh nghiệm"
   - Học vấn: "Tiến sĩ Y khoa - Đại học Y Hà Nội"
   - Thành tích: "5000+ ca phẫu thuật thành công"

2. **BS. CKI Trần Thị Bình** - Phó Khoa
   - Chuyên môn: "Chuyên gia điều trị võng mạc và glaucoma"
   - Kinh nghiệm: "18+ năm kinh nghiệm"
   - Thành tích: "Nghiên cứu sinh tại Đức 3 năm"

3. **BS. CKI Lê Minh Châu** - Bác sĩ chính
   - Chuyên môn: "Chuyên gia nhãn khoa trẻ em"
   - Thành tích: "Đào tạo tại Singapore Children Hospital"

4. **BS. CKI Phạm Thu Dung** - Bác sĩ chính  
   - Chuyên môn: "Chuyên gia phẫu thuật thẩm mỹ mắt"
   - Thành tích: "Chứng chỉ thẩm mỹ quốc tế"

- Navigation dots và arrows
- Button: "Xem Đầy Đủ Đội Ngũ"

#### 2.6 Thống Kê & Thành Tích (Section background gradient)
- **Counter Animation Section** với 6 chỉ số chính:
- Layout grid 3x2 với icon lớn cho mỗi stat

1. **Bệnh nhân đã điều trị**: 15,247+ (counter từ 0)
2. **Năm hoạt động**: 20+ năm  
3. **Ca phẫu thuật**: 8,500+ ca
4. **Tỷ lệ thành công**: 99.8%
5. **Bác sĩ chuyên môn cao**: 12 người
6. **Giải thưởng y tế**: 15 giải thưởng

- Animation trigger khi scroll vào view
- Background subtle pattern

#### 2.7 Tại Sao Chọn VisionCare (Section background trắng)
**4 lý do chính với icon và giải thích chi tiết:**

1. **Công Nghệ Hàng Đầu**
   - Icon: Thiết bị hiện đại
   - Chi tiết: "Máy móc nhập khẩu từ Đức, Nhật Bản. Luôn cập nhật công nghệ mới nhất"

2. **Đội Ngũ Chuyên Gia**  
   - Icon: Nhóm bác sĩ
   - Chi tiết: "100% bác sĩ có chứng chỉ chuyên khoa cấp I, II. Kinh nghiệm quốc tế"

3. **Dịch Vụ Toàn Diện**
   - Icon: Medical services
   - Chi tiết: "Từ tầm soát, chẩn đoán đến điều trị và phẫu thuật tất cả các bệnh về mắt"

4. **Cam Kết Chất Lượng**
   - Icon: Certificate/Award
   - Chi tiết: "Bảo hành dịch vụ, cam kết kết quả. Hỗ trợ 24/7"

#### 2.8 Testimonials/Đánh Giá Khách Hàng (Section background nhẹ)
- **Carousel layout** với 3 testimonial hiển thị cùng lúc
- **Mỗi testimonial card có:**
  - Avatar khách hàng
  - Rating 5 sao
  - Quote text  
  - Tên và tuổi khách hàng
  - Loại dịch vụ đã sử dụng

**Nội dung mẫu:**
1. "Cô Lan, 45 tuổi - Phẫu thuật cận thị Lasik"
2. "Anh Minh, 38 tuổi - Điều trị glaucoma"  
3. "Bé An, 8 tuổi - Điều trị cận thị trẻ em"

- Auto-play với pause on hover
- Navigation controls

#### 2.9 Tin Tức & Blog (Section background trắng)
- Tiêu đề: "Cập Nhật Kiến Thức Sức Khỏe Mắt"
- **Grid 3 cột** hiển thị 3 bài viết mới nhất:
  - Thumbnail image
  - Category tag
  - Tiêu đề bài viết
  - Excerpt ngắn
  - Ngày đăng và tác giả
  - "Đọc tiếp" link

#### 2.10 Call-to-Action Cuối Trang (Section background gradient)
- **Centered content với 2 CTA chính:**
- "Đặt Lịch Khám Ngay" - Button lớn màu cam
- "Hotline: 1900-xxxx" - Button outline
- Text: "Hãy để VisionCare chăm sóc đôi mắt của bạn"

### 3. TRANG DỊCH VỤ - CHI TIẾT ĐẦY ĐỦ

#### 3.1 Hero Section Trang Dịch Vụ
- Background image: Thiết bị y tế hiện đại  
- Overlay: "Dịch Vụ Chuyên Khoa Toàn Diện"
- Breadcrumb navigation: Home > Dịch Vụ
- Quick stats: Số dịch vụ, bác sĩ, năm kinh nghiệm

#### 3.2 Filter & Search Bar
- **Filter categories**:
  - Tất cả dịch vụ
  - Khám tổng quát  
  - Phẫu thuật
  - Điều trị chuyên sâu
  - Dịch vụ trẻ em
  - Thẩm mỹ
  - Cấp cứu
- Search input với placeholder "Tìm kiếm dịch vụ..."
- Sort options: Phổ biến nhất, A-Z, Giá tăng dần

#### 3.3 Grid Dịch Vụ (3 cột responsive)
**Danh sách chi tiết 15 dịch vụ:**

**NHÓM 1: KHÁM & CHẨN ĐOÁN CƠ BẢN**
1. **Khám Mắt Tổng Quát**
   - Icon: Stethoscope + Eye
   - Mô tả: "Kiểm tra toàn diện thị lực, đo độ cận/viễn/loạn thị"
   - Thời gian: 45-60 phút
   - Giá: 200.000 - 300.000 VND
   - Bác sĩ: Tất cả bác sĩ
   - Badge: "Phổ biến"

2. **Đo Thị Lực & Khúc Xạ**
   - Mô tả: "Đo chính xác độ cận, viễn, loạn thị bằng máy tự động"
   - Thời gian: 30 phút
   - Giá: 150.000 VND

3. **Chụp Đáy Mắt (OCT)**
   - Mô tả: "Chụp cắt lớp quang học, phát hiện sớm bệnh võng mạc"
   - Thời gian: 20 phút  
   - Giá: 400.000 VND

4. **Đo Nhãn Áp**
   - Mô tả: "Kiểm tra áp suất trong mắt, tầm soát glaucoma"
   - Thời gian: 15 phút
   - Giá: 100.000 VND

**NHÓM 2: ĐIỀU TRỊ CHUYÊN SÂU**
5. **Điều Trị Cận Thị Tiến Triển**
   - Mô tả: "Kiểm soát cận thị ở trẻ em bằng kính OK, atropine"
   - Phương pháp: Ortho-K, Atropine 0.01%
   - Độ tuổi: 6-18 tuổi
   - Giá: 15.000.000 - 25.000.000 VND (trọn gói 1 năm)

6. **Điều Trị Glaucoma**  
   - Mô tả: "Điều trị tăng nhãn áp bằng thuốc và laser"
   - Phương pháp: Thuốc nhỏ mắt, laser SLT, phẫu thuật
   - Bác sĩ: BS. Trần Thị Bình
   - Giá: 500.000 - 50.000.000 VND

7. **Điều Trị Bệnh Võng Mạc**
   - Mô tả: "Điều trị đục thủy tinh thể, thoái hóa điểm vàng"
   - Phương pháp: Tiêm thuốc trong dịch kính, laser võng mạc
   - Giá: 20.000.000 - 80.000.000 VND

**NHÓM 3: PHẪU THUẬT**
8. **Phẫu Thuật Lasik** ⭐
   - Mô tả: "Phẫu thuật laser điều chỉnh khúc xạ"
   - Độ cận: Từ -1.0D đến -10.0D
   - Thời gian: 15-20 phút/mắt
   - Bảo hành: 2 năm
   - Giá: 25.000.000 - 45.000.000 VND (cả 2 mắt)
   - Badge: "Phổ biến nhất"

9. **Phẫu Thuật Đục Thủy Tinh Thể**
   - Mô tả: "Thay thế thủy tinh thể bằng lens nhân tạo"
   - Công nghệ: Siêu âm Phaco, Femto Laser
   - Giá: 15.000.000 - 80.000.000 VND/mắt

10. **Phẫu Thuật Glaucoma**
    - Mô tả: "Tạo đường dẫn lưu cho dịch mắt"
    - Phương pháp: Trabeculectomy, tube shunt
    - Giá: 30.000.000 - 60.000.000 VND

**NHÓM 4: THẨM MỸ**  
11. **Cắt Mí Mắt (Blepharoplasty)**
    - Mô tả: "Phẫu thuật thẩm mỹ mí trên, mí dưới"
    - Thời gian: 1-2 giờ
    - Nghỉ dưỡng: 7-10 ngày
    - Giá: 8.000.000 - 15.000.000 VND

12. **Nâng Mí, Tạo Mí Kép**
    - Mô tả: "Tạo đường mí kép tự nhiên, khắc phục sụp mí"
    - Giá: 12.000.000 - 20.000.000 VND

**NHÓM 5: TRẺ EM**
13. **Khám Mắt Trẻ Em** 
    - Mô tả: "Khám chuyên biệt cho trẻ dưới 16 tuổi"
    - Nội dung: Đo thị lực, tầm soát cận thị, lác mắt
    - Bác sĩ: BS. Lê Minh Châu  
    - Giá: 300.000 - 500.000 VND

14. **Điều Trị Lác Mắt**
    - Mô tả: "Điều trị lác mắt bằng kính, che mắt, phẫu thuật"
    - Độ tuổi: 6 tháng - 12 tuổi
    - Giá: 500.000 - 25.000.000 VND

**NHÓM 6: CẤP CỨU**
15. **Cấp Cứu Mắt 24/7**
    - Mô tả: "Xử lý các trường hợp cấp cứu: dị vật, chấn thương"
    - Thời gian: 24/7
    - Phí cấp cứu: 500.000 VND (ngoài giờ)
    - Badge: "24/7"

#### 3.4 Service Detail Modal
- Click vào mỗi dịch vụ mở popup chi tiết:
  - Gallery hình ảnh thiết bị/quy trình
  - Mô tả chi tiết quy trình
  - Chỉ định và chống chỉ định  
  - Chuẩn bị trước khám/phẫu thuật
  - Chăm sóc sau điều trị
  - FAQ thường gặp
  - Đặt lịch trực tiếp
  - Bác sĩ phụ trách

### 4. TRANG THIẾT BỊ - SHOWCASE CÔNG NGHỆ

#### 4.1 Hero Section
- Background: Phòng phẫu thuật hiện đại
- Title: "Công Nghệ Thiết Bị Hàng Đầu Thế Giới"
- Subtitle: "Chúng tôi đầu tư vào những công nghệ tiên tiến nhất"
- Stats bar: "15+ Thiết bị hiện đại | Nhập khẩu 100% | Bảo trì định kỳ"

#### 4.2 Equipment Categories Tabs
- **Tab Navigation:**
  - Chẩn đoán & Khám
  - Phẫu thuật & Laser  
  - Thiết bị hỗ trợ
  - Phòng mổ & Vô trùng

#### 4.3 Thiết Bị Chẩn Đoán & Khám (Tab 1)

**1. Máy đo thị lực tự động Topcon CV-5000**
- **Hình ảnh**: Professional photo thiết bị
- **Xuất xứ**: Nhật Bản
- **Chức năng**: 
  - Đo khúc xạ tự động
  - Đo thị lực từ xa và gần
  - Kiểm tra thị trường
- **Ưu điểm**:
  - Chính xác cao 99.8%
  - Thời gian khám nhanh chóng
  - Thoải mái cho bệnh nhân
- **Ứng dụng**: Khám tổng quát, đo độ cận/viễn thị

**2. Máy chụp đáy mắt OCT Zeiss Cirrus HD 5000**
- **Xuất xứ**: Đức
- **Công nghệ**: Optical Coherence Tomography
- **Chức năng**:
  - Chụp cắt lớp võng mạc
  - Phân tích thần kinh thị giác
  - Theo dõi tiến triển bệnh
- **Độ phân giải**: 5 micromet
- **Ứng dụng**: Chẩn đoán glaucoma, bệnh võng mạc

**3. Máy đo nhãn áp Goldmann**
- **Xuất xứ**: Thụy Sĩ
- **Tiêu chuẩn**: WHO Gold Standard
- **Ứng dụng**: Tầm soát và theo dõi glaucoma

**4. Kính sinh hiển vi khám Zeiss S7**
- **Tính năng**: Zoom 1:6, LED illumination
- **Ứng dụng**: Khám chi tiết các bộ phận mắt

#### 4.4 Thiết Bị Phẫu Thuật & Laser (Tab 2)

**5. Hệ thống Laser Femtosecond Zeiss VisuMax**
- **Công nghệ**: ReLEx SMILE
- **Ứng dụng**: Phẫu thuật khúc xạ không dao
- **Ưu điểm**: 
  - Vết mổ nhỏ chỉ 2-4mm
  - Hồi phục nhanh
  - Độ chính xác cao
- **Giá trị**: 15 tỷ VND

**6. Máy Phẫu thuật đục thủy tinh thể Alcon Centurion**
- **Công nghệ**: Intelligent Phaco
- **Tính năng**:
  - Kiểm soát áp suất tự động
  - Rung động siêu âm tối ưu
  - Bảo vệ nội mô giác mạc
- **Lens nhân tạo**: Đa tiêu cự, Toric

**7. Laser Argon cho Võng mạc**
- **Ứng dụng**: 
  - Điều trị võng mạc đái tháo đường
  - Đông máu võng mạc
  - Điều trị glaucoma

**8. Hệ thống Phẫu thuật Vitrectomy Alcon Constellation**
- **Tốc độ cắt**: 7500 cắt/phút
- **Ứng dụng**: Phẫu thuật dịch kính, võng mạc

#### 4.5 Thiết Bị Hỗ Trợ (Tab 3)

**9. Máy siêu âm mắt A/B Scan**
- **Chức năng**: Đo chiều dài nhãn cầu, IOL calculation
- **Ứng dụng**: Chuẩn bị phẫu thuật đục thủy tinh thể

**10. Máy đo độ cong giác mạc Topography**
- **Chức năng**: Bản đồ 3D giác mạc
- **Ứng dụng**: Chẩn đoán cận thị, loạn thị

**11. Thiết bị Ortho-K (OK Lens)**
- **Chức năng**: Thiết kế kính cứng đeo qua đêm
- **Ứng dụng**: Kiểm soát cận thì tiến triển ở trẻ em
- **Hiệu quả**: Giảm 50-70% tốc độ tăng cận thị

#### 4.6 Phòng Mổ & Vô Trùng (Tab 4)

**12. Phòng mổ Laminar Air Flow**
- **Tiêu chuẩn**: ISO 14644 Class 5
- **Tính năng**: 
  - Luồng khí vô trùng
  - Áp suất dương
  - Nhiệt độ và độ ẩm kiểm soát
- **An toàn**: 99.99% vô trùng

**13. Hệ thống Monitor 4K**
- **Độ phân giải**: Ultra HD 4K
- **Ứng dụng**: Hiển thị real-time trong phẫu thuật

**14. Bàn mổ điều chỉnh điện tử**
- **Tính năng**: 
  - Điều chỉnh đa chiều
  - Memory positions
  - Ergonomic design

**15. Hệ thống khử trùng Plasma**
- **Công nghệ**: Low-temperature plasma
- **Ứng dụng**: Khử trùng dụng cụ nhạy cảm nhiệt

#### 4.7 Chứng Nhận & Bảo Trì
- **Chứng chỉ FDA**: Tất cả thiết bị có chứng nhận FDA
- **CE Mark**: Đạt tiêu chuẩn châu Âu
- **Bảo trì**: Định kỳ 3 tháng/lần
- **Calibration**: Hiệu chuẩn theo tiêu chuẩn quốc tế
- **Warranty**: 2-5 năm bảo hành chính hãng

#### 4.8 Đầu Tư & Phát Triển
- **Tổng đầu tư**: Hơn 50 tỷ VND cho thiết bị
- **Cập nhật**: Thay mới thiết bị 3-5 năm/lần
- **Đào tạo**: Bác sĩ được đào tạo tại nước ngoài
- **Nghiên cứu**: Tham gia thử nghiệm lâm sàng quốc tế

### 5. TRANG ĐẶT LỊCH - HỆ THỐNG BOOKING THÔNG MINH

#### 5.1 Hero Section
- Title: "Đặt Lịch Khám Online - Nhanh Chóng & Tiện Lợi"
- Subtitle: "Chỉ 3 bước đơn giản để đặt lịch khám tại VisionCare"
- Process indicators: "Chọn dịch vụ → Chọn thời gian → Xác nhận"

#### 5.2 Booking Steps Progress Bar
- **Step 1**: Thông tin cá nhân ✓
- **Step 2**: Chọn dịch vụ ✓  
- **Step 3**: Chọn bác sĩ & thời gian ✓
- **Step 4**: Xác nhận & thanh toán

#### 5.3 Step 1 - Thông tin Cá Nhân
**Form Layout 2 cột:**

**Cột trái - Thông tin bắt buộc:**
- Họ và tên* (input text)
- Số điện thoại* (input tel với validation)
- Email* (input email với validation)
- Ngày sinh* (date picker)
- Giới tính* (radio buttons: Nam/Nữ/Khác)

**Cột phải - Thông tin bổ sung:**
- Địa chỉ (textarea)
- Nghề nghiệp (input text)
- Người giới thiệu (dropdown: Bạn bè, Facebook, Google, Khác)
- Ghi chú đặc biệt (textarea)

**Bảo hiểm y tế:**
- Có bảo hiểm y tế (checkbox)
- Loại bảo hiểm (dropdown: BHYT, BHTN, Tư nhân, Khác)
- Mã số BHYT (input nếu có)

#### 5.4 Step 2 - Chọn Dịch Vụ

**Service Categories Tabs:**
- **Khám tổng quát** (Most popular)
- **Phẫu thuật**
- **Điều trị chuyên sâu**
- **Dịch vụ trẻ em**
- **Cấp cứu**

**Service Selection Grid:**
- Checkbox multiple selection
- Mỗi dịch vụ hiển thị:
  - Tên dịch vụ
  - Mô tả ngắn
  - Thời gian dự kiến
  - Giá dịch vụ
  - Icon đại diện

**Popular Services Quick Select:**
- Khám mắt tổng quát
- Đo thị lực
- Tư vấn phẫu thuật Lasik
- Khám mắt trẻ em

#### 5.5 Step 3 - Chọn Bác Sĩ & Thời Gian

**Doctor Selection:**
- **Option 1**: "Bác sĩ bất kỳ" (ưu tiên lịch trống)
- **Option 2**: Chọn bác sĩ cụ thể

**Available Doctors Grid:**
- Avatar + tên bác sĩ
- Chuyên môn chính
- Rating và số đánh giá
- "Xem thêm" link tới profile
- Badge: "Recommended" cho dịch vụ được chọn

**Calendar & Time Selection:**
**Calendar View:**
- Tháng hiện tại với navigation
- Ngày có lịch trống: màu xanh
- Ngày hết lịch: màu xám
- Ngày được chọn: màu cam highlight

**Time Slots Grid (khi chọn ngày):**
**Buổi Sáng (8:00-12:00):**
- 08:00-08:30 ✓ Available
- 08:30-09:00 ✗ Booked  
- 09:00-09:30 ✓ Available
- 09:30-10:00 ✓ Available
- ... và tiếp tục

**Buổi Chiều (13:30-17:30):**
- 13:30-14:00 ✓ Available
- 14:00-14:30 ✓ Available
- ... tiếp tục

**Special Notes:**
- Emergency slots: Màu đỏ, phí phụ thu
- Weekend slots: Màu vàng, có thể phụ thu
- Tooltip hiển thị thông tin bác sĩ trực

#### 5.6 Step 4 - Xác Nhận & Thanh Toán

**Booking Summary Card:**
- **Thông tin cá nhân**: Tên, SĐT, Email
- **Dịch vụ đã chọn**: Danh sách với giá
- **Bác sĩ**: Tên + chuyên môn
- **Thời gian**: Ngày, giờ, thời lượng dự kiến
- **Tổng chi phí**: Breakdown chi tiết

**Payment Options:**
- **Thanh toán tại phòng khám** (default)
- **Chuyển khoản ngân hàng**
  - QR code thanh toán
  - Thông tin tài khoản
- **Ví điện tử**: MoMo, ZaloPay
- **Thẻ tín dụng**: Visa, MasterCard

**Deposit Policy:**
- Phẫu thuật: Đặt cọc 30%
- Khám thường: Miễn đặt cọc
- Hủy lịch: 24h trước miễn phí

**Terms & Conditions:**
- Checkbox đồng ý điều khoản
- Link tới chính sách bảo mật
- Chính sách hủy/đổi lịch

#### 5.7 Confirmation Page
**Thành công:**
- Checkmark animation
- "Đặt lịch thành công!"
- Mã đặt lịch: #VC-2024-001234
- Thông tin chi tiết booking
- **Actions**:
  - Tải PDF xác nhận
  - Thêm vào Calendar
  - Chia sẻ với gia đình
  - Về trang chủ

**Email/SMS Notification:**
- Gửi ngay lập tức
- Reminder 24h trước khám
- Reminder 2h trước khám

#### 5.8 Manage Booking Section
**Existing Customers:**
- "Quản lý lịch hẹn hiện tại"
- Input: SĐT + Mã đặt lịch
- **Actions available**:
  - Xem chi tiết
  - Đổi lịch (nếu > 24h)
  - Hủy lịch
  - In phiếu khám

### 6. CHATBOT - AI ASSISTANT THÔNG MINH

#### 6.1 Floating Chat Button
- **Position**: Fixed bottom-right
- **Design**: Circular button với VisionCare logo
- **Animation**: Subtle pulse effect
- **Badge**: "Online" status indicator
- **Colors**: Gradient blue-green matching brand

#### 6.2 Chat Window Interface
**Header:**
- Avatar: VisionCare assistant
- Name: "VisionBot - Trợ lý ảo"
- Status: "Đang online • Phản hồi trong 1 phút"
- Actions: Minimize, Close

**Welcome Message:**
"Xin chào! Tôi là VisionBot, trợ lý ảo của VisionCare. Tôi có thể giúp bạn:
✓ Đặt lịch khám nhanh chóng  
✓ Tư vấn dịch vụ phù hợp
✓ Giải đáp thắc mắc về sức khỏe mắt
✓ Hướng dẫn chuẩn bị khám

Bạn cần hỗ trợ gì hôm nay?"

#### 6.3 Quick Actions Buttons
**Suggestion Chips:**
- "🗓️ Đặt lịch khám"
- "💰 Bảng giá dịch vụ" 
- "📍 Địa chỉ phòng khám"
- "⏰ Giờ mở cửa"
- "📞 Hotline khẩn cấp"
- "🔍 Tìm hiểu về Lasik"

#### 6.4 Conversation Flows

**Flow 1: Đặt lịch khám**
Bot: "Tuyệt vời! Để đặt lịch, tôi cần một vài thông tin:
1️⃣ Bạn cần khám dịch vụ gì? (Khám tổng quát/Lasik/Glaucoma/Khác)
2️⃣ Thời gian mong muốn? (Sáng/Chiều/Cuối tuần)
3️⃣ Số điện thoại để xác nhận?"

**Flow 2: Tư vấn dịch vụ**
Bot: "Tôi sẽ giúp bạn chọn dịch vụ phù hợp. Bạn có thể mô tả triệu chứng hoặc vấn đề về mắt không?"

**Flow 3: Bảng giá**
Bot: "Đây là bảng giá một số dịch vụ phổ biến:
👁️ Khám mắt tổng quát: 200.000 - 300.000đ
🔍 Chụp OCT: 400.000đ  
⚡ Phẫu thuật Lasik: 25-45 triệu/cả 2 mắt
👶 Khám trẻ em: 300.000đ

💡 *Giá có thể thay đổi theo tình trạng cụ thể*"

#### 6.5 AI Capabilities
**Natural Language Processing:**
- Hiểu tiếng Việt tự nhiên
- Xử lý lỗi chính tả, từ viết tắt
- Context awareness trong cuộc hội thoại

**Medical Knowledge Base:**
- Database các bệnh mắt thường gặp
- Triệu chứng và gợi ý dịch vụ
- Không chẩn đoán, chỉ tư vấn sơ bộ

**Integration với hệ thống:**
- Kiểm tra lịch trống real-time
- Tạo appointment trực tiếp
- Sync với CRM system

#### 6.6 Escalation tới Human Agent
**Trigger conditions:**
- Bot không hiểu câu hỏi
- User yêu cầu nói chuyện với người
- Vấn đề phức tạp cần chuyên gia

**Handoff process:**
"Tôi sẽ kết nối bạn với chuyên viên tư vấn của chúng tôi. Vui lòng chờ trong giây lát..."

**Agent Interface:**
- Chat history với bot
- User profile summary  
- Suggested responses
- Case priority level

#### 6.7 Analytics & Improvement
**Tracking Metrics:**
- Conversation completion rate
- User satisfaction (thumbs up/down)
- Common questions không trả lời được
- Conversion rate từ chat sang booking

**Continuous Learning:**
- Weekly review chat logs
- Update knowledge base
- Improve response accuracy
- A/B testing different flows

### 7. KIẾN TRÚC KỸ THUẬT & COMPONENT STRUCTURE

#### 7.1 Project Architecture
```
src/
├── components/
│   ├── common/
│   │   ├── Header.jsx
│   │   ├── Footer.jsx
│   │   ├── Loading.jsx
│   │   ├── Modal.jsx
│   │   └── Button.jsx
│   ├── home/
│   │   ├── HeroSlider.jsx
│   │   ├── StatsBar.jsx
│   │   ├── AboutSection.jsx
│   │   ├── ServicesGrid.jsx
│   │   ├── DoctorsCarousel.jsx
│   │   ├── StatisticsSection.jsx
│   │   ├── WhyChooseUs.jsx
│   │   ├── Testimonials.jsx
│   │   └── NewsSection.jsx
│   ├── services/
│   │   ├── ServiceGrid.jsx
│   │   ├── ServiceFilter.jsx
│   │   ├── ServiceCard.jsx
│   │   └── ServiceModal.jsx
│   ├── equipment/
│   │   ├── EquipmentTabs.jsx
│   │   ├── EquipmentCard.jsx
│   │   └── CertificationSection.jsx
│   ├── booking/
│   │   ├── BookingForm.jsx
│   │   ├── StepIndicator.jsx
│   │   ├── PersonalInfo.jsx
│   │   ├── ServiceSelection.jsx
│   │   ├── DoctorTimeSelection.jsx
│   │   ├── Confirmation.jsx
│   │   └── Calendar.jsx
│   └── chatbot/
│       ├── ChatWidget.jsx
│       ├── ChatWindow.jsx
│       ├── MessageBubble.jsx
│       └── QuickActions.jsx
├── pages/
│   ├── Home.jsx
│   ├── Services.jsx
│   ├── Equipment.jsx
│   ├── Booking.jsx
│   └── Contact.jsx
├── hooks/
│   ├── useBooking.js
│   ├── useChat.js
│   └── useCalendar.js
├── utils/
│   ├── api.js
│   ├── constants.js
│   └── helpers.js
└── data/
    ├── services.js
    ├── doctors.js
    └── equipment.js
```

#### 7.2 State Management Strategy
**Context API cho Global State:**
- BookingContext: Quản lý booking flow
- ChatContext: Chatbot state
- UIContext: Theme, modals, loading states

**Local State với useState:**
- Component-specific states
- Form inputs validation
- Animation states

#### 7.3 Key Hooks Usage
```javascript
// Custom hooks for complex logic
const useBooking = () => {
  const [bookingData, setBookingData] = useState({});
  const [currentStep, setCurrentStep] = useState(1);
  // Logic for booking flow
};

const useChat = () => {
  const [messages, setMessages] = useState([]);
  const [isTyping, setIsTyping] = useState(false);
  // Chat functionality
};
```

#### 7.4 Responsive Breakpoints Strategy
```css
/* Tailwind Custom Config */
screens: {
  'xs': '475px',
  'sm': '640px',   // Mobile landscape
  'md': '768px',   // Tablet
  'lg': '1024px',  // Desktop small
  'xl': '1280px',  // Desktop
  '2xl': '1536px', // Large desktop
}
```

**Component Responsive Patterns:**
- Grid: `grid-cols-1 md:grid-cols-2 lg:grid-cols-3`
- Text: `text-sm md:text-base lg:text-lg`
- Spacing: `p-4 md:p-6 lg:p-8`
- Images: `w-full md:w-1/2 lg:w-1/3`

#### 7.5 Animation Strategy
**Scroll Animations với Intersection Observer:**
```javascript
const useScrollAnimation = () => {
  const [isVisible, setIsVisible] = useState(false);
  // Intersection Observer logic
};
```

**CSS Classes cho Animations:**
```css
.fade-in-up {
  @apply opacity-0 translate-y-8 transition-all duration-700;
}
.fade-in-up.animate {
  @apply opacity-100 translate-y-0;
}

.scale-on-hover {
  @apply transition-transform duration-300 hover:scale-105;
}
```

#### 7.6 Performance Optimizations
- **Lazy Loading**: Components và images
- **Code Splitting**: Route-based splitting
- **Image Optimization**: WebP format, responsive sizes
- **Memoization**: React.memo cho expensive components
- **Debouncing**: Search inputs, API calls

#### 7.7 SEO & Accessibility
```javascript
// SEO Component
const SEOHead = ({ title, description, image }) => (
  <Helmet>
    <title>{title} | VisionCare</title>
    <meta name="description" content={description} />
    <meta property="og:image" content={image} />
  </Helmet>
);
```

**Accessibility Features:**
- ARIA labels và roles
- Keyboard navigation
- Focus management
- Screen reader support
- High contrast mode support

### 8. FOOTER - THÔNG TIN TOÀN DIỆN

#### 8.1 Footer Layout (4 cột responsive)

**Cột 1 - VisionCare Info:**
- Logo VisionCare (lớn hơn header)
- Tagline: "Chăm sóc mắt chuyên nghiệp - Công nghệ tiên tiến"
- Mô tả ngắn: "20+ năm kinh nghiệm trong lĩnh vực nhãn khoa với đội ngũ bác sĩ chuyên môn cao và thiết bị hiện đại nhất."
- Social Media:
  - Facebook icon + link
  - YouTube icon + link  
  - Zalo icon + link
  - Instagram icon + link

**Cột 2 - Liên Hệ:**
- **📍 Địa chỉ**: 
  - Cơ sở 1: 123 Đường ABC, Quận 1, TP.HCM
  - Cơ sở 2: 456 Đường DEF, Quận Ba Đình, Hà Nội
- **📞 Hotline**: 
  - Tổng đài: 1900-xxxx (miễn phí)
  - Khẩn cấp 24/7: 0909-xxx-xxx
- **✉️ Email**: 
  - Tư vấn: tuvan@visioncare.vn
  - Hỗ trợ: hotro@visioncare.vn
- **🕒 Giờ làm việc**:
  - T2-T6: 8:00 - 17:30
  - T7: 8:00 - 12:00
  - CN: Nghỉ (trừ cấp cứu)

**Cột 3 - Dịch Vụ Nhanh:**
- Khám mắt tổng quát
- Phẫu thuật Lasik
- Điều trị glaucoma  
- Nhãn khoa trẻ em
- Cấp cứu mắt 24/7
- Tư vấn online
- Link: "Xem tất cả dịch vụ →"

**Cột 4 - Hỗ Trợ:**
- Đặt lịch online
- Hướng dẫn chuẩn bị khám
- Chính sách bảo hành
- Bảo hiểm y tế
- Câu hỏi thường gặp
- Chính sách bảo mật
- Điều khoản sử dụng

#### 8.2 Google Maps Integration
- **Embedded map**: Hiển thị 2 cơ sở
- **Custom markers**: VisionCare branded pins
- **Info windows**: Địa chỉ, SĐT, giờ mở cửa
- **Directions**: Link tới Google Maps navigation

#### 8.3 Newsletter Signup
- **Section riêng** trên footer chính
- Background: Light gradient
- Title: "Đăng Ký Nhận Tin Sức Khỏe Mắt"
- Form: Email input + Subscribe button
- Benefits: "Nhận tips chăm sóc mắt và ưu đãi đặc biệt"

#### 8.4 Certifications & Awards
- **Logo carousel** các chứng nhận:
  - Bộ Y Tế cấp phép
  - ISO 9001:2015
  - JCI Accreditation (nếu có)
  - Top 10 Phòng Khám Uy Tín
- Auto-scroll với pause on hover

#### 8.5 Bottom Bar
- Background: Darker shade
- **Trái**: © 2024 VisionCare. All rights reserved.
- **Giữa**: "Thiết kế bởi [Agency Name]"
- **Phải**: 
  - "Chính sách bảo mật" 
  - "|" separator
  - "Điều khoản sử dụng"

### 9. MOBILE OPTIMIZATION CHI TIẾT

#### 9.1 Mobile Header
- **Collapsed menu**: Hamburger icon
- **Logo**: Smaller version, centered hoặc left-aligned
- **Key actions**: Phone call button, đặt lịch button
- **Sticky behavior**: Header thu gọn khi scroll

#### 9.2 Mobile Hero Section
- **Single slide view**: Swipe để chuyển slide
- **Vertical layout**: Text over image
- **Touch-friendly buttons**: Larger tap areas
- **Auto-advance**: 4 giây/slide

#### 9.3 Mobile Service Grid
- **Single column**: Full-width cards
- **Infinite scroll**: Load more on scroll
- **Quick filters**: Horizontal scroll chips
- **Tap to expand**: Accordion style details

#### 9.4 Mobile Booking Form
- **Step-by-step**: One section per screen
- **Progress bar**: Visual indication
- **Large inputs**: Easy typing on mobile
- **Calendar**: Month view với swipe navigation

#### 9.5 Mobile Chatbot
- **Full-screen mode**: Better typing experience
- **Voice input**: Speech-to-text integration
- **Quick replies**: Larger touch targets
- **Persistent**: Stays accessible across pages

## YÊU CẦU KỸ THUẬT

### ReactJS Components
- Sử dụng functional components với hooks
- State management với useState/useContext
- Responsive design với Tailwind breakpoints
- Smooth animations với Tailwind transitions
- Image optimization và lazy loading
- SEO-friendly với proper meta tags

### Tailwind CSS Classes Chính
```css
/* Colors */
bg-blue-500, bg-emerald-500, bg-orange-500
text-gray-900, text-gray-600
hover:bg-blue-600, focus:ring-blue-300

/* Layout */
container mx-auto px-4
grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3
flex flex-col md:flex-row items-center
space-y-4 md:space-y-0 md:space-x-8

/* Components */
rounded-lg shadow-lg
border border-gray-200
bg-white/90 backdrop-blur-sm
transition-all duration-300
hover:scale-105 hover:shadow-xl
```

### Animations và Effects
- Fade in khi scroll vào view
- Hover effects cho buttons và cards
- Loading states cho forms
- Smooth transitions giữa slides
- Parallax effects cho hero section (optional)

### Responsive Design
- Mobile-first approach
- Breakpoints: sm (640px), md (768px), lg (1024px), xl (1280px)
- Touch-friendly buttons cho mobile
- Hamburger menu cho navigation
- Swipe gestures cho slideshow

## FEATURES ĐẶC BIỆT
- Dark mode toggle (optional)
- Multi-language support (Việt/English)
- Accessibility compliance (WCAG 2.1)
- Performance optimization
- Progressive Web App features
- Online appointment booking system
- Patient portal integration ready

## CONTENT TONE
- Chuyên nghiệp nhưng thân thiện
- Tạo niềm tin và sự an tâm
- Ngôn ngữ dễ hiểu, tránh thuật ngữ y khoa phức tạp
- Nhấn mạnh tính an toàn và chất lượng dịch vụ

## CALL-TO-ACTIONS
- "Đặt Lịch Ngay" (màu cam, nổi bật)
- "Tư Vấn Miễn Phí"
- "Gọi Ngay: [SĐT]"
- "Tìm Hiểu Thêm"
- "Xem Chi Tiết"

---

**TỔNG KẾT**: Hãy tạo ra một website vừa đẹp mắt, vừa chuyên nghiệp và user-friendly, thể hiện sự tin cậy và chất lượng cao của VisionCare - trung tâm chăm sóc mắt hàng đầu!