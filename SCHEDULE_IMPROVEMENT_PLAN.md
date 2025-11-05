# Kế Hoạch Cải Thiện Hệ Thống Quản Lý Lịch Làm Việc Bác Sĩ

## 📊 So Sánh Hiện Trạng vs Luồng Chuẩn

### ✅ ĐÃ CÓ (Đạt yêu cầu)

1. **WeeklySchedule (Template lịch tuần)**
   - ✅ Bảng `WeeklySchedule` với `day_of_week`, `slot_id`, `is_active`
   - ✅ API tạo/sửa/xóa WeeklySchedule
   - ✅ Logic generate Schedule từ WeeklySchedule

2. **Schedule (Lịch cụ thể)**
   - ✅ Bảng `Schedules` với `doctor_id`, `slot_id`, `schedule_date`, `status`
   - ✅ API quản lý Schedule

3. **Slots (Khung giờ)**
   - ✅ Bảng `Slots` với `start_time`, `end_time`, `service_type_id`
   - ✅ API quản lý Slots

4. **Doctor Absence (Nghỉ phép)**
   - ✅ Bảng `DoctorAbsence` với `start_date`, `end_date`, `absence_type`, `status`
   - ✅ API quản lý nghỉ phép
   - ✅ Logic block schedules khi có nghỉ phép approved

5. **Appointment**
   - ✅ Update schedule status = "Booked" khi tạo appointment

---

### ❌ THIẾU (Chưa đạt - Cần sửa)

#### **Giai đoạn 1: Thiết Lập Lịch Ban Đầu**

| # | Yêu cầu | Hiện trạng | Mức độ ưu tiên |
|---|---------|-----------|----------------|
| 1.1 | **Định nghĩa Nguồn lực (Rooms/Equipment)** | ❌ Chỉ có bảng `Equipment` nhưng chưa liên kết với Schedule | 🔴 CAO |
| 1.2 | **Định nghĩa Ca làm việc (WorkShift)** | ❌ Chưa có bảng `WorkShift` | 🔴 CAO |
| 1.3 | **Thời lượng dịch vụ** | ⚠️ Có `ServicesType.duration_minutes` nhưng chưa dùng trong logic tạo slots | 🟡 TRUNG |

#### **Giai đoạn 2: Xây Dựng Lịch Trình Cơ Sở**

| # | Yêu cầu | Hiện trạng | Mức độ ưu tiên |
|---|---------|-----------|----------------|
| 2.1 | **DoctorSchedule với Recurrence** | ⚠️ Chỉ có `WeeklySchedule` (recurrence hàng tuần), thiếu recurrence linh hoạt | 🟡 TRUNG |
| 2.2 | **Phân bổ Nguồn lực vào Schedule** | ❌ Schedule chưa có `room_id` hoặc `equipment_id` | 🔴 CAO |
| 2.3 | **Tự động tạo Slots từ Ca làm việc** | ❌ Chưa có logic tự động chia ca thành slots dựa trên thời lượng dịch vụ | 🔴 CAO |
| 2.4 | **Background Job tự động generate** | ❌ Chưa có background job tự động generate schedules hàng ngày | 🟡 TRUNG |

#### **Giai đoạn 3: Quản Lý Ngoại Lệ**

| # | Yêu cầu | Hiện trạng | Mức độ ưu tiên |
|---|---------|-----------|----------------|
| 3.1 | **Nghỉ phép theo slot cụ thể** | ⚠️ Chỉ có nghỉ phép theo ngày, chưa có nghỉ theo slot | 🟢 THẤP |
| 3.2 | **Chặn slot liền kề (thời lượng khám đặc biệt)** | ❌ Chưa có chức năng | 🟢 THẤP |

#### **Giai đoạn 4: Điều Phối theo Tình trạng**

| # | Yêu cầu | Hiện trạng | Mức độ ưu tiên |
|---|---------|-----------|----------------|
| 4.1 | **Xóa slot khi đặt appointment** | ⚠️ Chỉ update status = "Booked", không xóa | 🟡 TRUNG |
| 4.2 | **Tái tạo slot khi hủy (nếu đủ sớm)** | ❌ Chưa có logic tái tạo slot | 🟡 TRUNG |
| 4.3 | **Cutoff Time** | ❌ Chưa có cấu hình cutoff time | 🟢 THẤP |

---

## 🎯 Kế Hoạch Triển Khai

### **Phase 1: Cấu trúc Database (Ưu tiên CAO)**

#### 1.1. Thêm bảng Rooms
```sql
CREATE TABLE Rooms (
    room_id SERIAL PRIMARY KEY,
    room_name VARCHAR(100) NOT NULL UNIQUE,
    room_code VARCHAR(20) UNIQUE,
    capacity INTEGER DEFAULT 1,
    status VARCHAR(20) DEFAULT 'Active',
    location VARCHAR(255),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### 1.2. Thêm bảng WorkShift
```sql
CREATE TABLE WorkShift (
    shift_id SERIAL PRIMARY KEY,
    shift_name VARCHAR(100) NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    is_active BOOLEAN DEFAULT true,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### 1.3. Cập nhật bảng Schedules
```sql
ALTER TABLE Schedules 
ADD COLUMN room_id INTEGER REFERENCES Rooms(room_id),
ADD COLUMN equipment_id INTEGER REFERENCES Equipment(equipment_id);

CREATE INDEX idx_schedules_room ON Schedules(room_id);
CREATE INDEX idx_schedules_equipment ON Schedules(equipment_id);
```

#### 1.4. Thêm bảng DoctorSchedule (recurrence)
```sql
CREATE TABLE DoctorSchedule (
    doctor_schedule_id SERIAL PRIMARY KEY,
    doctor_id INTEGER NOT NULL REFERENCES Doctors(account_id),
    shift_id INTEGER NOT NULL REFERENCES WorkShift(shift_id),
    room_id INTEGER REFERENCES Rooms(room_id),
    start_date DATE NOT NULL,
    end_date DATE,
    day_of_week INTEGER, -- 1=Monday, 2=Tuesday, etc. NULL = all days
    recurrence_rule VARCHAR(50), -- 'DAILY', 'WEEKLY', 'MONTHLY', 'CUSTOM'
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE
);
```

---

### **Phase 2: Logic Tự Động (Ưu tiên CAO)**

#### 2.1. Service tự động tạo Slots từ WorkShift
- Tạo `SlotGenerationService`
- Logic: Lấy `WorkShift` → Chia theo `ServiceType.duration_minutes` → Tạo Slots
- Chạy khi tạo DoctorSchedule mới

#### 2.2. Cập nhật ScheduleGenerationService
- Thêm logic tạo schedules từ `DoctorSchedule` (thay vì chỉ từ `WeeklySchedule`)
- Tự động phân bổ room/equipment
- Xử lý recurrence rules

#### 2.3. Background Job
- Tạo `ScheduleGenerationBackgroundService`
- Chạy hàng ngày lúc 00:00 để generate schedules cho 14-30 ngày tới
- Tự động cleanup schedules quá hạn

---

### **Phase 3: Xử Lý Appointment (Ưu tiên TRUNG)**

#### 3.1. Cập nhật AppointmentService
- Khi tạo appointment: Xóa schedule (hoặc đánh dấu "Booked" + không hiển thị)
- Khi hủy appointment: Kiểm tra cutoff time → Tái tạo schedule nếu đủ sớm
- Cutoff time: Cấu hình trong `appsettings.json` (ví dụ: 24 giờ trước)

#### 3.2. Cập nhật BookingService
- Tương tự AppointmentService
- Đảm bảo schedule không bị double booking

---

### **Phase 4: Nâng Cao (Ưu tiên THẤP)**

#### 4.1. Nghỉ phép theo slot
- Thêm `DoctorAbsenceSlot` table
- Cho phép nghỉ phép chi tiết theo slot cụ thể

#### 4.2. Chặn slot liền kề
- Thêm `BlockedSchedule` table
- Logic: Khi bác sĩ cần thời gian dài, tự động block các slot liền kề

---

## 📝 Checklist Triển Khai

### **Backend (BE)**

- [ ] **Database Migrations**
  - [ ] Tạo bảng `Rooms`
  - [ ] Tạo bảng `WorkShift`
  - [ ] Tạo bảng `DoctorSchedule`
  - [ ] Thêm `room_id`, `equipment_id` vào `Schedules`
  - [ ] Thêm `cutoff_hours` vào config

- [ ] **Domain Entities**
  - [ ] `Room.cs`
  - [ ] `WorkShift.cs`
  - [ ] `DoctorSchedule.cs`
  - [ ] Update `Schedule.cs` (thêm Room, Equipment)

- [ ] **Repositories**
  - [ ] `IRoomRepository`, `RoomRepository`
  - [ ] `IWorkShiftRepository`, `WorkShiftRepository`
  - [ ] `IDoctorScheduleRepository`, `DoctorScheduleRepository`
  - [ ] Update `IScheduleRepository` (thêm filter theo room/equipment)

- [ ] **Services**
  - [ ] `SlotGenerationService` (tạo slots từ WorkShift)
  - [ ] Update `ScheduleGenerationService` (hỗ trợ DoctorSchedule)
  - [ ] Update `AppointmentService` (xóa/tái tạo schedule)
  - [ ] Update `BookingService` (xóa/tái tạo schedule)
  - [ ] `ScheduleGenerationBackgroundService` (background job)

- [ ] **Controllers**
  - [ ] `RoomsController` (CRUD)
  - [ ] `WorkShiftsController` (CRUD)
  - [ ] Update `SchedulingController` (thêm room/equipment filter)

- [ ] **Background Jobs**
  - [ ] Hangfire/Quartz job tự động generate schedules hàng ngày

### **Frontend (FE)**

- [ ] **Pages**
  - [ ] `/admin/rooms` - Quản lý phòng khám
  - [ ] `/admin/work-shifts` - Quản lý ca làm việc
  - [ ] Update `/admin/doctor-schedule` - Thêm room/equipment selection

- [ ] **Components**
  - [ ] `RoomManagement.jsx`
  - [ ] `WorkShiftManagement.jsx`
  - [ ] Update `CreateWeeklyScheduleModal` - Thêm room/equipment

- [ ] **Services**
  - [ ] `roomAPI.js`
  - [ ] `workShiftAPI.js`
  - [ ] Update `scheduleAPI.js` - Thêm room/equipment params

---

## 🚀 Thứ Tự Triển Khai Khuyến Nghị

1. **Week 1**: Phase 1 (Database) + Domain Entities
2. **Week 2**: Phase 2 (Logic tự động) + Background Job
3. **Week 3**: Phase 3 (Xử lý Appointment)
4. **Week 4**: Phase 4 (Nâng cao) + Testing

---

## 📌 Notes

- Giữ nguyên `WeeklySchedule` để backward compatibility
- `DoctorSchedule` là cách mới, linh hoạt hơn
- Có thể migrate dần từ WeeklySchedule → DoctorSchedule
- Cutoff time: Mặc định 24 giờ, có thể config trong appsettings

