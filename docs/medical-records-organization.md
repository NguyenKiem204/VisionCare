# Tổ chức Hệ thống Hồ sơ Y tế

## Phân biệt 2 loại hồ sơ

### 1. **Hồ sơ khám (Encounter)** - SOAP Notes
- **Mục đích**: Ghi chép chi tiết quá trình khám bệnh theo chuẩn SOAP
- **Cấu trúc**:
  - **Subjective (Chủ quan)**: Triệu chứng bệnh nhân mô tả
  - **Objective (Khách quan)**: Kết quả khám, đo lường
  - **Assessment (Đánh giá)**: Chẩn đoán
  - **Plan (Kế hoạch)**: Phương án điều trị
- **Đặc điểm**:
  - Có **Prescriptions** (đơn thuốc) với nhiều dòng thuốc chi tiết
  - Có **Orders** (chỉ định) - xét nghiệm, thủ thuật
  - Status: Draft → Signed (khi hoàn tất)
  - Gắn với Appointment
- **API**: `/api/doctor/me/ehr/encounters`
- **Trang quản lý**: `/doctor/ehr` (Encounter.jsx)

### 2. **Hồ sơ bệnh án (Medical History)** - Medical Records
- **Mục đích**: Lưu trữ lịch sử khám bệnh, tóm tắt các lần khám
- **Cấu trúc**:
  - **Symptoms**: Triệu chứng (text)
  - **Diagnosis**: Chẩn đoán (text)
  - **Treatment**: Điều trị (text)
  - **Prescription**: Đơn thuốc (text, không có lines)
  - **VisionLeft/VisionRight**: Thị lực
  - **AdditionalTests**: Xét nghiệm bổ sung
  - **Notes**: Ghi chú
- **Đặc điểm**:
  - Đơn giản hơn, chỉ là text fields
  - Không có Prescriptions/Orders riêng
  - Dùng để xem lịch sử tổng quan
  - Gắn với Appointment
- **API**: `/api/medical-records` hoặc `/api/doctor/me/patients/{id}/medical-history`
- **Trang quản lý**: Xem trong Patients.jsx modal

## Workflow đề xuất

### Khi khám bệnh:
1. Tạo **Encounter** (Hồ sơ khám) với SOAP notes
2. Thêm **Prescriptions** (đơn thuốc chi tiết) vào Encounter
3. Thêm **Orders** (chỉ định) vào Encounter
4. Khi hoàn tất, có thể tạo **Medical History** (Hồ sơ bệnh án) để lưu tóm tắt

### Khi xem lịch sử:
- **Xem chi tiết**: Dùng Encounter (có đầy đủ SOAP, đơn thuốc, chỉ định)
- **Xem tổng quan**: Dùng Medical History (tóm tắt các lần khám)

## Tổ chức UI

### Trang quản lý tập trung (đề xuất):
- **Tab 1: Hồ sơ khám (Encounters)**
  - Danh sách Encounters với SOAP notes
  - Tạo/sửa Encounter
  - Quản lý Prescriptions và Orders
  
- **Tab 2: Hồ sơ bệnh án (Medical History)**
  - Danh sách Medical History theo bệnh nhân
  - Xem lịch sử khám tổng quan
  - Tạo/sửa Medical History

### Tích hợp vào Appointment Detail:
- Khi xem Appointment, có thể:
  - Xem/tao Encounter (nếu chưa có)
  - Xem Medical History (nếu có)

