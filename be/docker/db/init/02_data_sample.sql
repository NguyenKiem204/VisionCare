-- VisionCare Database Sample Data
-- Execute in this exact order

-- 1. Insert Roles
INSERT INTO Role (role_name, role_description) VALUES 
('Admin', 'System administrator with full access'),
('Doctor', 'Medical professional providing services'),
('Staff', 'Administrative staff member'),
('Customer', 'Registered patient/customer');

-- 2. Insert Permissions
INSERT INTO Permission (permission_name, resource, action) VALUES 
('manage_users', 'users', 'manage'),
('view_appointments', 'appointments', 'read'),
('create_appointments', 'appointments', 'create'),
('update_appointments', 'appointments', 'update'),
('delete_appointments', 'appointments', 'delete'),
('view_patients', 'patients', 'read'),
('create_patients', 'patients', 'create'),
('update_patients', 'patients', 'update'),
('view_doctors', 'doctors', 'read'),
('create_doctors', 'doctors', 'create'),
('update_doctors', 'doctors', 'update'),
('view_services', 'services', 'read'),
('create_services', 'services', 'create'),
('update_services', 'services', 'update'),
('view_reports', 'reports', 'read'),
('manage_system', 'system', 'manage'),
('view_medical_records', 'medical_records', 'read'),
('create_medical_records', 'medical_records', 'create'),
('update_medical_records', 'medical_records', 'update'),
('manage_blog', 'blog', 'manage'),
('view_blog', 'blog', 'read');

-- 3. Insert Permission-Role relationships
-- Admin gets all permissions
INSERT INTO PermissionRole (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Admin';

-- Doctor permissions
INSERT INTO PermissionRole (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Doctor'
  AND p.permission_name IN ('view_appointments', 'update_appointments', 'view_patients', 'update_patients', 
                           'view_medical_records', 'create_medical_records', 'update_medical_records');

-- Staff permissions
INSERT INTO PermissionRole (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Staff'
  AND p.permission_name IN ('view_appointments', 'create_appointments', 'update_appointments', 
                           'view_patients', 'create_patients', 'update_patients', 'view_services', 'view_doctors');

-- Customer permissions
INSERT INTO PermissionRole (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Customer'
  AND p.permission_name IN ('view_appointments', 'view_blog');

-- 4. Insert Specializations
INSERT INTO Specialization (name, status) VALUES 
('Khúc xạ', 'Active'),
('Bệnh võng mạc', 'Active'),
('Bệnh glaucoma', 'Active'),
('Phẫu thuật mắt', 'Active'),
('Khám tổng quát', 'Active'),
('Bệnh giác mạc', 'Active');

-- 5. Insert Customer Ranks
INSERT INTO CustomerRank (rank_name, min_amount) VALUES 
('Bronze', 0),
('Silver', 5000000),
('Gold', 10000000),
('Platinum', 20000000);

-- 6. Insert Discounts
INSERT INTO Discount (discount_name, discount_percent, rank_id, start_date, end_date, status) VALUES 
('Bronze Discount', 5.00, 1, '2025-01-01', '2025-12-31', 'Active'),
('Silver Discount', 10.00, 2, '2025-01-01', '2025-12-31', 'Active'),
('Gold Discount', 15.00, 3, '2025-01-01', '2025-12-31', 'Active'),
('Platinum Discount', 20.00, 4, '2025-01-01', '2025-12-31', 'Active'),
('New Year Special', 25.00, NULL, '2025-01-01', '2025-01-31', 'Active'),
('Spring Promotion', 15.00, NULL, '2025-03-01', '2025-05-31', 'Active');

-- 7. Insert Accounts and get their IDs
INSERT INTO Accounts (email, username, password_hash, email_confirmed, role_id, status) VALUES 
('dr.nguyen@visioncare.com', 'doctor01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 2, 'Active'),
('dr.tran@visioncare.com', 'doctor02', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 2, 'Active'),
('dr.le@visioncare.com', 'doctor03', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 2, 'Active'),
('dr.pham@visioncare.com', 'doctor04', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 2, 'Active'),
('staff01@visioncare.com', 'staff01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 3, 'Active'),
('staff02@visioncare.com', 'staff02', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 3, 'Active'),
('nguyen.van.a@gmail.com', 'customer01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 4, 'Active'),
('tran.thi.b@gmail.com', 'customer02', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 4, 'Active'),
('le.van.c@gmail.com', 'customer03', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 4, 'Active'),
('pham.thi.d@gmail.com', 'customer04', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 4, 'Active'),
('hoang.van.e@gmail.com', 'customer05', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', TRUE, 4, 'Active');

-- 8. Insert Degrees
INSERT INTO Degree (name) VALUES 
('Bác sĩ Y khoa'),
('Thạc sĩ Y học'),
('Tiến sĩ Y học'),
('Bác sĩ chuyên khoa I'),
('Bác sĩ chuyên khoa II');

-- 9. Insert Certificates
INSERT INTO Certificate (name) VALUES 
('Chứng chỉ Phẫu thuật Mắt'),
('Chứng chỉ Bệnh Võng mạc'),
('Chứng chỉ Glaucoma'),
('Chứng chỉ Khúc xạ'),
('Chứng chỉ Cấp cứu Y khoa'),
('Chứng chỉ Phẫu thuật Laser');

-- 10. Insert Doctors
INSERT INTO Doctors (account_id, full_name, phone, experience_years, specialization_id, avatar, rating, gender, dob, address, status) VALUES 
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 'BS. Nguyễn Văn Nam', '0902345678', 15, 1, 'doctor01.jpg', 4.8, 'Nam', '1978-05-15', '123 Lê Lợi, Q.1, TP.HCM', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 'BS. CKI Trần Thị Mai', '0903456789', 12, 2, 'doctor02.jpg', 4.7, 'Nữ', '1982-08-20', '456 Nguyễn Huệ, Q.1, TP.HCM', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 'BS. CKII Lê Minh Tuấn', '0904567890', 20, 3, 'doctor03.jpg', 4.9, 'Nam', '1975-12-10', '789 Hai Bà Trưng, Q.3, TP.HCM', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 'BS. CKI Phạm Thị Lan', '0905678901', 8, 4, 'doctor04.jpg', 4.6, 'Nữ', '1985-03-25', '321 Pasteur, Q.3, TP.HCM', 'Active');

-- 11. Insert Degree-Doctor relationships
INSERT INTO DegreeDoctor (doctor_id, degree_id, issued_date, issued_by, certificate_image, status) VALUES 
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 1, '2005-06-15', 'Đại học Y Dược TP.HCM', 'degree_doctor01_1.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 4, '2010-08-20', 'Bộ Y tế', 'degree_doctor01_2.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 1, '2008-07-10', 'Đại học Y Hà Nội', 'degree_doctor02_1.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 2, '2012-09-15', 'Đại học Y Hà Nội', 'degree_doctor02_2.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 1, '2002-06-20', 'Đại học Y Dược TP.HCM', 'degree_doctor03_1.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 5, '2008-12-15', 'Bộ Y tế', 'degree_doctor03_2.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 1, '2012-06-15', 'Đại học Y Dược TP.HCM', 'degree_doctor04_1.jpg', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 4, '2018-08-20', 'Bộ Y tế', 'degree_doctor04_2.jpg', 'Active');

-- 12. Insert Certificate-Doctor relationships
INSERT INTO CertificateDoctor (doctor_id, certificate_id, issued_date, issued_by, certificate_image, expiry_date, status) VALUES 
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 4, '2010-03-15', 'Hội Nhãn khoa Việt Nam', 'cert_doctor01_1.jpg', '2027-03-15', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 2, '2012-05-20', 'Hội Nhãn khoa Việt Nam', 'cert_doctor02_1.jpg', '2027-05-20', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 1, '2008-08-10', 'Hội Nhãn khoa Việt Nam', 'cert_doctor03_1.jpg', '2026-08-10', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 3, '2010-11-25', 'Hội Nhãn khoa Việt Nam', 'cert_doctor03_2.jpg', '2027-11-25', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 1, '2018-06-15', 'Hội Nhãn khoa Việt Nam', 'cert_doctor04_1.jpg', '2028-06-15', 'Active'),
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 6, '2020-09-10', 'Hội Nhãn khoa Việt Nam', 'cert_doctor04_2.jpg', '2027-09-10', 'Active');

-- 13. Insert Customers
INSERT INTO Customers (account_id, full_name, phone, address, dob, gender, rank_id, avatar) VALUES 
((SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), 'Nguyễn Văn A', '0907890123', '12 Trần Hưng Đạo, Q.5, TP.HCM', '1985-03-15', 'Nam', 2, 'customer01.jpg'),
((SELECT account_id FROM accounts WHERE email = 'tran.thi.b@gmail.com'), 'Trần Thị B', '0908901234', '34 Lý Tự Trọng, Q.1, TP.HCM', '1990-07-22', 'Nữ', 1, 'customer02.jpg'),
((SELECT account_id FROM accounts WHERE email = 'le.van.c@gmail.com'), 'Lê Văn C', '0909012345', '56 Pasteur, Q.3, TP.HCM', '1988-11-08', 'Nam', 3, 'customer03.jpg'),
((SELECT account_id FROM accounts WHERE email = 'pham.thi.d@gmail.com'), 'Phạm Thị D', '0910123456', '78 Võ Văn Tần, Q.3, TP.HCM', '1992-04-18', 'Nữ', 1, 'customer04.jpg'),
((SELECT account_id FROM accounts WHERE email = 'hoang.van.e@gmail.com'), 'Hoàng Văn E', '0911234567', '90 Nguyễn Trãi, Q.5, TP.HCM', '1987-09-30', 'Nam', 4, 'customer05.jpg');

-- 14. Insert Staff
INSERT INTO Staff (account_id, full_name, phone, address, dob, gender, avatar, hired_date, salary) VALUES 
((SELECT account_id FROM accounts WHERE email = 'staff01@visioncare.com'), 'Nguyễn Thị Lan', '0905678901', '100 Nguyễn Thị Minh Khai, Q.1, TP.HCM', '1987-09-12', 'Nữ', 'staff01.jpg', '2020-01-15', 15000000),
((SELECT account_id FROM accounts WHERE email = 'staff02@visioncare.com'), 'Trần Văn Hưng', '0906789012', '200 Cách Mạng Tháng 8, Q.10, TP.HCM', '1985-02-28', 'Nam', 'staff02.jpg', '2019-03-20', 18000000);

-- 15. Insert Services Types
INSERT INTO ServicesType (name, duration_minutes, created_at, updated_at) VALUES 
('Khám cơ bản', 30, '2024-01-01 08:00:00', '2024-01-01 08:00:00'),
('Khám chuyên sâu', 45, '2024-01-01 08:00:00', '2024-01-01 08:00:00'),
('Phẫu thuật nhỏ', 60, '2024-01-01 08:00:00', '2024-01-01 08:00:00'),
('Phẫu thuật lớn', 120, '2024-01-01 08:00:00', '2024-01-01 08:00:00'),
('Tái khám', 15, '2024-01-01 08:00:00', '2024-01-01 08:00:00'),
('Xét nghiệm', 20, '2024-01-01 08:00:00', '2024-01-01 08:00:00');

-- 16. Insert Services
INSERT INTO Services (name, description, benefits, status, specialization_id) VALUES 
('Khám mắt tổng quát', 'Khám tổng quát tình trạng mắt bao gồm kiểm tra thị lực, đo nhãn áp, soi đáy mắt', 'Phát hiện sớm các bệnh lý về mắt, tư vấn chăm sóc mắt hiệu quả', 'Active', 5),
('Đo khúc xạ', 'Đo độ cận, viễn, loạn thị bằng thiết bị hiện đại', 'Xác định chính xác tình trạng khúc xạ, tư vấn kính phù hợp', 'Active', 1),
('Điều trị glaucoma', 'Điều trị bệnh tăng nhãn áp bằng thuốc hoặc can thiệp ngoại khoa', 'Kiểm soát nhãn áp, bảo vệ thị thần kinh, duy trì thị lực', 'Active', 3),
('Phẫu thuật đục thủy tinh thể', 'Phẫu thuật phaco hiện đại, an toàn, ít xâm lấn', 'Phục hồi thị lực, cải thiện chất lượng cuộc sống', 'Active', 4),
('Điều trị bệnh võng mạc', 'Điều trị bệnh võng mạc tiểu đường, thoái hóa điểm vàng', 'Ngăn ngừa mù lòa, bảo vệ thị lực trung tâm', 'Active', 2),
('Khám mắt trẻ em', 'Khám và tư vấn chuyên biệt dành cho trẻ em', 'Phát hiện và điều trị sớm các bệnh lý mắt ở trẻ', 'Active', 5),
('Phẫu thuật Laser cận thị', 'Phẫu thuật điều chỉnh khúc xạ bằng laser', 'Giảm hoặc loại bỏ hoàn toàn việc đeo kính, cải thiện thị lực', 'Active', 1),
('Điều trị bệnh giác mạc', 'Điều trị các bệnh lý về giác mạc', 'Phục hồi độ trong suốt của giác mạc, cải thiện thị lực', 'Active', 6);

-- 17. Insert Services Detail
INSERT INTO ServicesDetail (service_id, service_type_id, cost) VALUES 
(1, 1, 300000),
(1, 2, 500000),
(2, 1, 200000),
(2, 2, 350000),
(3, 2, 800000),
(3, 3, 1200000),
(4, 4, 25000000),
(4, 3, 18000000),
(5, 2, 1500000),
(5, 3, 2500000),
(6, 1, 250000),
(6, 2, 400000),
(7, 4, 35000000),
(7, 3, 28000000),
(8, 2, 1000000),
(8, 3, 1800000);

-- 18. Insert Images Service
INSERT INTO ImagesService (service_id, image_main, image_before, image_after) VALUES 
(1, 'service01_main.jpg', 'service01_before.jpg', 'service01_after.jpg'),
(2, 'service02_main.jpg', 'service02_before.jpg', 'service02_after.jpg'),
(4, 'service04_main.jpg', 'service04_before.jpg', 'service04_after.jpg'),
(7, 'service07_main.jpg', 'service07_before.jpg', 'service07_after.jpg');

-- 19. Insert Slots
INSERT INTO Slots (start_time, end_time, service_type_id) VALUES 
('08:00:00', '08:30:00', 1),
('08:30:00', '09:00:00', 1),
('09:00:00', '09:30:00', 1),
('09:30:00', '10:00:00', 1),
('10:00:00', '10:30:00', 1),
('10:30:00', '11:00:00', 1),
('13:30:00', '14:15:00', 2),
('14:15:00', '15:00:00', 2),
('15:00:00', '15:45:00', 2),
('15:45:00', '16:30:00', 2),
('08:00:00', '09:00:00', 3),
('09:00:00', '10:00:00', 3),
('13:30:00', '14:45:00', 5),
('14:45:00', '15:00:00', 5),
('15:00:00', '15:15:00', 5),
('07:00:00', '09:00:00', 4),
('09:00:00', '11:00:00', 4),
('13:00:00', '15:00:00', 4);

-- 20. Insert Schedules
INSERT INTO Schedules (doctor_id, slot_id, schedule_date, status) VALUES 
-- Doctor 1 (Nguyễn Văn Nam) - Khúc xạ
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 1, '2025-09-26', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 2, '2025-09-26', 'Booked'),
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 3, '2025-09-26', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 7, '2025-09-26', 'Available'),
-- Doctor 2 (Trần Thị Mai) - Bệnh võng mạc  
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 4, '2025-09-26', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 5, '2025-09-26', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 8, '2025-09-26', 'Booked'),
((SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 9, '2025-09-26', 'Available'),
-- Doctor 3 (Lê Minh Tuấn) - Glaucoma
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 11, '2025-09-27', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 12, '2025-09-27', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 16, '2025-09-27', 'Booked'),
-- Doctor 4 (Phạm Thị Lan) - Phẫu thuật
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 17, '2025-09-28', 'Available'),
((SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 18, '2025-09-28', 'Available');

-- 21. Insert Appointments
INSERT INTO Appointment (patient_id, doctor_id, service_detail_id, discount_id, appointment_datetime, status, actual_cost, notes, created_by) VALUES 
((SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), (SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 1, 2, '2025-09-26 08:30:00', 'Completed', 270000, 'Khám định kỳ', (SELECT account_id FROM accounts WHERE email = 'staff01@visioncare.com')),
((SELECT account_id FROM accounts WHERE email = 'tran.thi.b@gmail.com'), (SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 9, NULL, '2025-09-26 15:00:00', 'Completed', 1500000, 'Điều trị võng mạc tiểu đường', (SELECT account_id FROM accounts WHERE email = 'staff01@visioncare.com')),
((SELECT account_id FROM accounts WHERE email = 'le.van.c@gmail.com'), (SELECT account_id FROM accounts WHERE email = 'dr.le@visioncare.com'), 5, 3, '2025-09-27 13:00:00', 'Scheduled', 680000, 'Điều trị glaucoma', (SELECT account_id FROM accounts WHERE email = 'staff02@visioncare.com')),
((SELECT account_id FROM accounts WHERE email = 'pham.thi.d@gmail.com'), (SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 3, NULL, '2025-09-27 09:00:00', 'Scheduled', 200000, 'Đo khúc xạ', (SELECT account_id FROM accounts WHERE email = 'staff01@visioncare.com')),
((SELECT account_id FROM accounts WHERE email = 'hoang.van.e@gmail.com'), (SELECT account_id FROM accounts WHERE email = 'dr.pham@visioncare.com'), 15, 4, '2025-09-28 09:00:00', 'Scheduled', 22400000, 'Phẫu thuật Laser cận thị', (SELECT account_id FROM accounts WHERE email = 'staff02@visioncare.com'));

-- 22. Insert Medical History
INSERT INTO MedicalHistory (appointment_id, diagnosis, symptoms, treatment, prescription, vision_left, vision_right, additional_tests, notes) VALUES 
(1, 'Cận thị nhẹ', 'Mờ mắt khi nhìn xa, đau đầu sau khi đọc sách lâu', 'Đeo kính cận, nghỉ ngơi hợp lý', 'Kính cận -1.5D mắt trái, -1.25D mắt phải', 0.8, 0.9, 'Đo nhãn áp: 15mmHg (bình thường), Soi đáy mắt: không có bất thường', 'Tái khám sau 6 tháng, hạn chế sử dụng thiết bị điện tử'),
(2, 'Bệnh võng mạc tiểu đường giai đoạn 2', 'Giảm thị lực, nhìn mờ, xuất hiện các đốm đen', 'Tiêm thuốc chống VEGF, kiểm soát đường huyết', 'Ranibizumab 0.5mg tiêm nội nhãn, Metformin 500mg x 2 lần/ngày', 0.6, 0.7, 'OCT võng mạc: phù hoàng điểm, xuất tiết cứng', 'Tái khám sau 4 tuần, cần theo dõi đường huyết chặt chẽ');

-- 23. Insert Follow Up
INSERT INTO FollowUp (appointment_id, next_appointment_date, description, status) VALUES 
(1, '2026-03-26', 'Tái khám kiểm tra thị lực, đo khúc xạ', 'Pending'),
(2, '2025-10-26', 'Kiểm tra tình trạng võng mạc sau điều trị', 'Pending');

-- 24. Insert Check Out
INSERT INTO CheckOut (appointment_id, transaction_type, transaction_status, total_amount, transaction_code, payment_date, notes) VALUES 
(1, 'Cash', 'Completed', 270000, 'PAY202509260001', '2025-09-26 09:15:00', 'Thanh toán tiền mặt'),
(2, 'Card', 'Completed', 1500000, 'PAY202509260002', '2025-09-26 16:00:00', 'Thanh toán qua thẻ Visa');

-- 25. Insert Feedback Service
INSERT INTO FeedbackService (appointment_id, rating, feedback_text, feedback_date, response_text, response_date, responded_by) VALUES 
(1, 5, 'Dịch vụ rất tốt, nhân viên thân thiện, phòng khám sạch sẽ. Bác sĩ khám rất kỹ và giải thích dễ hiểu.', '2025-09-26 10:00:00', 'Cảm ơn anh đã tin tưởng và lựa chọn dịch vụ của chúng tôi. Chúng tôi sẽ tiếp tục cải thiện chất lượng phục vụ.', '2025-09-26 14:00:00', 6),
(2, 4, 'Thiết bị hiện đại, bác sĩ chuyên môn cao. Chỉ có điều thời gian chờ hơi lâu.', '2025-09-26 16:30:00', 'Cảm ơn chị đã phản hồi. Chúng tôi sẽ cải thiện quy trình để giảm thời gian chờ đợi.', '2025-09-26 17:00:00', 6);

-- 26. Insert Feedback Doctor
INSERT INTO FeedbackDoctor (appointment_id, rating, feedback_text, feedback_date, response_text, response_date, responded_by) VALUES 
(1, 5, 'Bác sĩ Nguyễn rất tận tâm, kiên nhẫn giải thích tình trạng bệnh và cách chăm sóc mắt. Rất hài lòng!', '2025-09-26 10:00:00', 'Cảm ơn sự tin tưởng của bệnh nhân. Tôi luôn cố gắng mang lại sự chăm sóc tốt nhất.', '2025-09-26 11:00:00', 6),
(2, 5, 'Bác sĩ Trần có kinh nghiệm phong phú, điều trị hiệu quả. Tôi cảm thấy yên tâm khi được chị khám.', '2025-09-26 16:30:00', 'Rất cảm ơn lời nhận xét tích cực. Chúc chị sớm hồi phục hoàn toàn.', '2025-09-26 18:00:00', 6);

-- 27. Insert Banners (match FE HeroSlider default)
INSERT INTO Banner (title, description, image_url, link_url, display_order, status, start_date, end_date) VALUES 
('VisionCare - Chăm Sóc Mắt Chuyên Nghiệp', '20+ năm kinh nghiệm - Công nghệ hiện đại - Đội ngũ chuyên gia', 'https://images.unsplash.com/photo-1559757148-5c350d0d3c56?auto=format&fit=crop&w=1920&q=80', '/booking', 1, 'Active', '2025-01-01', '2026-12-31'),
('Công Nghệ Tiên Tiến', 'Máy móc nhập khẩu từ Đức - Chẩn đoán chính xác 99.8%', 'https://images.unsplash.com/photo-1576091160399-112ba8d25d1f?auto=format&fit=crop&w=1920&q=80', '/equipment', 2, 'Active', '2025-01-01', '2026-12-31'),
('Dịch Vụ Toàn Diện', 'Từ khám tổng quát đến phẫu thuật chuyên sâu', 'https://images.unsplash.com/photo-1582750433449-648ed127bb54?auto=format&fit=crop&w=1920&q=80', '/services', 3, 'Active', '2025-01-01', '2026-12-31');

-- 28. Insert Content Stories
INSERT INTO ContentStories (patient_name, patient_image, story_content, display_order, status) VALUES 
('Cô Lan (58 tuổi)', 'patient_story01.jpg', 'Tôi bị đục thủy tinh thể nhiều năm, thị lực ngày càng giảm sút. Sau khi được bác sĩ Lê tư vấn kỹ lưỡng và thực hiện phẫu thuật Phaco, thị lực của tôi đã được phục hồi hoàn toàn. Giờ đây tôi có thể đọc sách, xem TV mà không cần kính. Cảm ơn đội ngũ bác sĩ VisionCare!', 1, 'Active'),
('Anh Minh (35 tuổi)', 'patient_story02.jpg', 'Làm việc với máy tính 10-12 tiếng mỗi ngày khiến mắt tôi thường xuyên khô, mệt mỏi và đỏ. Sau khi được điều trị tại VisionCare với liệu pháp IPL và các loại thuốc nhỏ mắt chuyên dụng, tình trạng của tôi đã cải thiện đáng kể. Giờ đây tôi có thể làm việc thoải mái hơn nhiều.', 2, 'Active'),
('Bé An (8 tuổi)', 'patient_story03.jpg', 'Con tôi bị cận thị từ khi 6 tuổi. Nhờ được phát hiện sớm và áp dụng phương pháp điều trị kiểm soát cận thị tại VisionCare, độ cận của bé đã được kiểm soát tốt. Các bác sĩ rất kiên nhẫn với trẻ em và tạo không khí thân thiện giúp bé không sợ khi khám mắt.', 3, 'Active');

-- 29. Insert Machines
INSERT INTO Machine (name, description, image_url, specifications, status) VALUES 
('Máy đo thị lực tự động Topcon KR-800', 'Máy đo khúc xạ và độ cong giác mạc tự động với độ chính xác cao', 'topcon_kr800.jpg', 'Đo khúc xạ: ±25.00D, Độ cong giác mạc: 5.00-150.00D, Đường kính đồng tử: 2.0-9.0mm', 'Active'),
('Máy phẫu thuật Phaco Alcon Centurion', 'Hệ thống phẫu thuật đục thủy tinh thể thế hệ mới với công nghệ Intelligent Phaco', 'alcon_centurion.jpg', 'Tần số siêu âm: 28,500-57,000 Hz, Công suất tối đa: 100%, Hệ thống làm mát: Active Cooling', 'Active'),
('Máy chụp OCT Zeiss Cirrus HD-5000', 'Máy chụp cắt lớp quang học võng mạc và đĩa thị thần kinh độ phân giải cao', 'zeiss_cirrus.jpg', 'Độ phân giải: 5μm, Tốc độ quét: 68,000 A-scans/giây, Chiều sâu quét: 2mm', 'Active'),
('Máy đo nhãn áp Goldmann AT-900', 'Máy đo nhãn áp tiêu chuẩn vàng trong chẩn đoán glaucoma', 'goldmann_at900.jpg', 'Phạm vi đo: 0-80 mmHg, Độ chính xác: ±0.5 mmHg, Đường kính đầu đo: 3.06mm', 'Active'),
('Máy Laser Excimer Wavelight EX500', 'Hệ thống laser điều chỉnh khúc xạ thế hệ mới với công nghệ Eye Tracking', 'wavelight_ex500.jpg', 'Tần số laser: 500Hz, Đường kính chùm tia: 0.95mm, Hệ thống theo dõi mắt: 6D Real-time', 'Active');

-- 30. Insert Blog posts
INSERT INTO Blog (title, content, excerpt, featured_image, author_id, status, published_at, view_count) VALUES 
('10 cách bảo vệ mắt khi làm việc với máy tính', 
'Trong thời đại số hóa hiện nay, việc làm việc với máy tính đã trở thành không thể thiếu. Tuy nhiên, điều này cũng đồng nghĩa với việc mắt chúng ta phải chịu áp lực lớn...

**1. Áp dụng quy tắc 20-20-20**
Cứ sau 20 phút làm việc, hãy nhìn vào một vật cách xa 20 feet (6 mét) trong 20 giây. Điều này giúp mắt được nghỉ ngơi và giảm căng thẳng.

**2. Điều chỉnh độ sáng màn hình**
Độ sáng màn hình nên tương đương với môi trường xung quanh. Màn hình quá sáng hoặc quá tối đều có thể gây mỏi mắt.

**3. Duy trì khoảng cách hợp lý**
Màn hình nên đặt cách mắt từ 50-70cm, với phần trên của màn hình ngang tầm mắt hoặc thấp hơn một chút.

**4. Chớp mắt thường xuyên**
Khi tập trung vào màn hình, chúng ta thường chớp mắt ít hơn, dẫn đến khô mắt. Hãy ý thức chớp mắt đều đặn.

**5. Sử dụng kính chống ánh sáng xanh**
Ánh sáng xanh từ màn hình có thể gây mỏi mắt và ảnh hưởng đến giấc ngủ. Kính chống ánh sáng xanh có thể giúp giảm thiểu tác hại này.',

'Hướng dẫn chi tiết 10 cách đơn giản nhưng hiệu quả để bảo vệ đôi mắt khi làm việc với máy tính hàng ngày.',
'blog_computer_eye_care.jpg', 1, 'Published', '2025-09-20 09:00:00', 1250),

('Phẫu thuật Laser điều trị cận thị - Những điều bạn cần biết',
'Phẫu thuật Laser điều trị cận thị (LASIK) là một trong những phương pháp hiệu quả nhất để điều chỉnh khúc xạ mắt hiện nay...

**Phẫu thuật LASIK là gì?**
LASIK (Laser-Assisted in Situ Keratomileusis) là kỹ thuật sử dụng tia laser để tái tạo hình dạng giác mạc, giúp ánh sáng khúc xạ đúng cách và tập trung chính xác lên võng mạc.

**Ai có thể thực hiện phẫu thuật LASIK?**
- Tuổi từ 18-45
- Độ cận thị ổn định trong ít nhất 1 năm
- Độ dày giác mạc đủ điều kiện
- Không mắc các bệnh lý mắt khác

**Quy trình phẫu thuật**
1. Khám và tư vấn chi tiết
2. Thực hiện các xét nghiệm cần thiết
3. Phẫu thuật (khoảng 15-20 phút cho cả 2 mắt)
4. Theo dõi và chăm sóc sau phẫu thuật

**Lợi ích của phẫu thuật LASIK**
- Cải thiện thị lực nhanh chóng
- Không cần đeo kính hoặc lens
- Tỷ lệ thành công cao (>95%)
- Thời gian hồi phục nhanh',

'Tìm hiểu về phẫu thuật Laser LASIK - phương pháp hiện đại giúp loại bỏ kính cận thị an toàn và hiệu quả.',
'blog_lasik_surgery.jpg', 2, 'Published', '2025-09-18 14:30:00', 890),

('Bệnh võng mạc tiểu đường - Nguyên nhân, triệu chứng và cách điều trị',
'Bệnh võng mạc tiểu đường là một trong những biến chứng nghiêm trọng của bệnh tiểu đường, có thể dẫn đến mù lòa nếu không được phát hiện và điều trị kịp thời...

**Nguyên nhân**
Đường huyết cao kéo dài sẽ làm tổn thương các mạch máu nhỏ trong võng mạc, gây ra hiện tượng rò rỉ máu và dịch, hình thành các mạch máu mới bất thường.

**Triệu chứng**
- Thị lực mờ, giảm sút
- Xuất hiện các đốm đen trong tầm nhìn
- Khó nhìn vào ban đêm
- Nhìn thấy các vòng sáng quanh đèn
- Mất thị lực đột ngột (giai đoạn nặng)

**Phương pháp điều trị**
1. **Tiêm thuốc chống VEGF**: Giúp giảm phù võng mạc và ngăn ngừa sự phát triển của mạch máu mới
2. **Laser võng mạc**: Phá hủy các vùng thiếu máu trên võng mạc
3. **Phẫu thuật cắt dịch kính**: Trong các trường hợp nặng có xuất huyết dịch kính

**Phòng ngừa**
- Kiểm soát đường huyết tốt
- Khám mắt định kỳ (6 tháng/lần)
- Kiểm soát huyết áp
- Duy trì lối sống lành mạnh',

'Hướng dẫn toàn diện về bệnh võng mạc tiểu đường - nguyên nhân, triệu chứng và các phương pháp điều trị hiện đại.',
'blog_diabetic_retinopathy.jpg', 3, 'Published', '2025-09-15 11:00:00', 1520);

-- 31. Insert Blog Comments
INSERT INTO CommentBlog (blog_id, author_id, parent_comment_id, comment_text, status) VALUES 
(1, (SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), NULL, 'Bài viết rất hữu ích! Tôi làm việc với máy tính 8 tiếng mỗi ngày và thường xuyên bị mỏi mắt. Sẽ áp dụng quy tắc 20-20-20 ngay.', 'Active'),
(1, (SELECT account_id FROM accounts WHERE email = 'tran.thi.b@gmail.com'), NULL, 'Cảm ơn bác sĩ đã chia sẻ. Cho tôi hỏi kính chống ánh sáng xanh có thực sự hiệu quả không ạ?', 'Active'),
(1, (SELECT account_id FROM accounts WHERE email = 'admin@visioncare.com'), 2, 'Kính chống ánh sáng xanh có thể giúp giảm một phần ánh sáng xanh từ màn hình, tuy nhiên hiệu quả có thể khác nhau tùy từng người. Quan trọng nhất vẫn là nghỉ ngơi mắt đều đặn.', 'Active'),
(2, (SELECT account_id FROM accounts WHERE email = 'le.van.c@gmail.com'), NULL, 'Tôi đã phẫu thuật LASIK tại VisionCare năm ngoái và kết quả rất tốt. Giờ không cần đeo kính nữa, rất thuận tiện!', 'Active'),
(2, (SELECT account_id FROM accounts WHERE email = 'pham.thi.d@gmail.com'), NULL, 'Em 19 tuổi, cận thị -4.5D có thể làm LASIK được không ạ?', 'Active'),
(2, (SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 5, 'Với độ cận -4.5D và tuổi 19, bạn có thể cân nhắc phẫu thuật LASIK. Tuy nhiên cần khám chi tiết để đánh giá độ dày giác mạc và các yếu tố khác. Bạn có thể đặt lịch khám tư vấn để được tư vấn cụ thể nhé.', 'Active'),
(3, (SELECT account_id FROM accounts WHERE email = 'hoang.van.e@gmail.com'), NULL, 'Bố tôi bị tiểu đường 10 năm rồi, gần đây thấy mắt mờ. Có phải là bệnh võng mạc tiểu đường không ạ?', 'Active'),
(3, (SELECT account_id FROM accounts WHERE email = 'dr.tran@visioncare.com'), 7, 'Với tiền sử tiểu đường 10 năm và triệu chứng mờ mắt, rất có khả năng là bệnh võng mạc tiểu đường. Bạn nên đưa bố đến khám ngay để được chẩn đoán và điều trị kịp thời. Việc phát hiện sớm rất quan trọng để ngăn ngừa các biến chứng nghiêm trọng.', 'Active');

-- 32. Insert OTP Services (for testing)
INSERT INTO OTPServices (account_id, otp_hash, otp_type, expires_at, attempts) VALUES 
((SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), '$2a$10$abcdefghijklmnopqrstuvwxyz123456', 'LOGIN', '2025-09-26 09:00:00', 0),
((SELECT account_id FROM accounts WHERE email = 'tran.thi.b@gmail.com'), '$2a$10$zyxwvutsrqponmlkjihgfedcba654321', 'PASSWORD_RESET', '2025-09-26 10:00:00', 1);

-- 33. Insert Refresh Tokens (for testing)
INSERT INTO RefreshTokens (token_hash, account_id, expires_at, created_by_ip) VALUES 
('$2a$10$refreshtoken1234567890abcdef', (SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), '2025-10-26 08:00:00', '192.168.1.100'),
('$2a$10$refreshtoken0987654321fedcba', (SELECT account_id FROM accounts WHERE email = 'tran.thi.b@gmail.com'), '2025-10-26 09:00:00', '192.168.1.101');

-- 34. Insert Claims (skipped - admin account created by DbSeeder, not in sample data)

-- 35. Insert Audit Logs
INSERT INTO AuditLogs (account_id, action, resource, resource_id, ip_address, success, details) VALUES 
((SELECT account_id FROM accounts WHERE email = 'admin@visioncare.com'), 'LOGIN', 'accounts', (SELECT account_id FROM accounts WHERE email = 'admin@visioncare.com'), '192.168.1.10', TRUE, '{"browser": "Chrome", "os": "Windows"}'),
((SELECT account_id FROM accounts WHERE email = 'staff01@visioncare.com'), 'CREATE_APPOINTMENT', 'appointments', 1, '192.168.1.20', TRUE, '{"patient_id": 8, "doctor_id": 2}'),
((SELECT account_id FROM accounts WHERE email = 'dr.nguyen@visioncare.com'), 'UPDATE_MEDICAL_HISTORY', 'medical_history', 1, '192.168.1.30', TRUE, '{"appointment_id": 1}'),
((SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), 'LOGIN', 'accounts', (SELECT account_id FROM accounts WHERE email = 'nguyen.van.a@gmail.com'), '192.168.1.100', TRUE, '{"browser": "Safari", "os": "iOS"}'),
((SELECT account_id FROM accounts WHERE email = 'tran.thi.b@gmail.com'), 'VIEW_APPOINTMENT', 'appointments', 2, '192.168.1.101', TRUE, '{"appointment_id": 2}');

-- 36. Update last login times for accounts
UPDATE Accounts SET last_login = CURRENT_TIMESTAMP WHERE email IN ('admin@visioncare.com', 'dr.nguyen@visioncare.com', 'staff01@visioncare.com', 'nguyen.van.a@gmail.com', 'tran.thi.b@gmail.com');

-- 37. Update view counts for blogs
UPDATE Blog SET view_count = view_count + FLOOR(RANDOM() * 100) + 50;

-- 38. Insert Equipment Sample Data
INSERT INTO Equipment (name, model, serial_number, manufacturer, purchase_date, last_maintenance_date, status, location, notes, created_at, updated_at) VALUES 
('Máy đo thị lực tự động', 'Topcon RM-800', 'TP-2024-001', 'Topcon Corporation', '2024-01-15', '2024-09-15', 'Active', 'Phòng khám 1', 'Máy đo thị lực tự động cao cấp', '2024-01-15 08:00:00', '2024-09-15 10:30:00'),
('Máy đo nhãn áp', 'Icare IC200', 'IC-2024-002', 'Icare Finland Oy', '2024-02-20', '2024-08-20', 'Active', 'Phòng khám 2', 'Máy đo nhãn áp không tiếp xúc', '2024-02-20 09:15:00', '2024-08-20 14:20:00'),
('Máy chụp võng mạc', 'Canon CR-2', 'CR-2024-003', 'Canon Medical Systems', '2024-03-10', '2024-07-10', 'Maintenance', 'Phòng chụp X-quang', 'Đang bảo trì định kỳ', '2024-03-10 10:00:00', '2024-07-10 16:45:00'),
('Máy đo độ dày giác mạc', 'Tomey SP-3000P', 'TM-2024-004', 'Tomey Corporation', '2024-04-05', '2024-06-05', 'Active', 'Phòng khám 3', 'Máy đo độ dày giác mạc chính xác', '2024-04-05 11:30:00', '2024-06-05 09:15:00'),
('Máy đo trường thị giác', 'Humphrey Field Analyzer', 'HF-2024-005', 'Carl Zeiss Meditec', '2024-05-12', '2024-05-12', 'Active', 'Phòng khám 4', 'Máy đo trường thị giác tự động', '2024-05-12 13:20:00', '2024-05-12 13:20:00'),
('Máy siêu âm mắt', 'Aviso S', 'AS-2024-006', 'Quantel Medical', '2024-06-18', '2024-12-18', 'Active', 'Phòng siêu âm', 'Máy siêu âm mắt A/B scan', '2024-06-18 14:00:00', '2024-12-18 11:30:00'),
('Máy đo độ cong giác mạc', 'Topcon KR-1', 'KR-2024-007', 'Topcon Corporation', '2024-07-25', '2024-01-25', 'Broken', 'Kho thiết bị', 'Cần sửa chữa - lỗi phần mềm', '2024-07-25 15:45:00', '2024-01-25 08:30:00'),
('Máy đo áp lực nội nhãn', 'Reichert 7CR', 'RC-2024-008', 'Reichert Technologies', '2024-08-30', '2024-08-30', 'Active', 'Phòng khám 5', 'Máy đo áp lực nội nhãn không tiếp xúc', '2024-08-30 12:15:00', '2024-08-30 12:15:00'),
('Máy chụp OCT', 'Cirrus HD-OCT', 'CO-2024-009', 'Carl Zeiss Meditec', '2024-09-15', '2024-03-15', 'Active', 'Phòng chụp OCT', 'Máy chụp OCT võng mạc cao cấp', '2024-09-15 16:30:00', '2024-03-15 10:45:00'),
('Máy đo độ dày giác mạc', 'Pachymeter SP-3000', 'PC-2024-010', 'Tomey Corporation', '2024-10-20', '2024-10-20', 'Active', 'Phòng khám 6', 'Máy đo độ dày giác mạc di động', '2024-10-20 17:00:00', '2024-10-20 17:00:00');

-- 39. Final verification queries (optional - for testing)
-- SELECT 'Roles' as table_name, COUNT(*) as count FROM Role
-- UNION ALL SELECT 'Accounts', COUNT(*) FROM Accounts
-- UNION ALL SELECT 'Doctors', COUNT(*) FROM Doctors  
-- UNION ALL SELECT 'Customers', COUNT(*) FROM Customers
-- UNION ALL SELECT 'Services', COUNT(*) FROM Services
-- UNION ALL SELECT 'Appointments', COUNT(*) FROM Appointment
-- UNION ALL SELECT 'Blog Posts', COUNT(*) FROM Blog
-- UNION ALL SELECT 'Comments', COUNT(*) FROM CommentBlog
-- UNION ALL SELECT 'Equipment', COUNT(*) FROM Equipment;

-- Sample data for SectionContent
INSERT INTO SectionContent(section_key, content, image_url, more_data)
VALUES
('hero_slider', 'Slider trang chủ VisionCare', NULL, '[{"title": "Bảo vệ mắt của bạn","subtitle": "Chúng tôi luôn đồng hành cùng bạn", "image": "https://images.unsplash.com/photo-hero1.jpg"}, {"title": "Đội ngũ chuyên gia chuyên nghiệp","subtitle": "Uy tín - Chất lượng - Hiện đại", "image": "https://images.unsplash.com/photo-hero2.jpg"}]');

-- Chuẩn hóa why_us theo FE (title/subtitle/bullets/images)
DELETE FROM SectionContent WHERE section_key = 'why_us';
INSERT INTO SectionContent(section_key, content, image_url, more_data) VALUES
('why_us',
 'Với hơn 20 năm kinh nghiệm trong lĩnh vực nhãn khoa, VisionCare tự hào là trung tâm chăm sóc mắt hàng đầu với đội ngũ bác sĩ chuyên môn cao, trang thiết bị hiện đại và dịch vụ tận tâm.',
 NULL,
 '{
   "title": "Tại sao VisionCare là sự lựa chọn hàng đầu?",
   "subtitle": "Với hơn 20 năm kinh nghiệm trong lĩnh vực nhãn khoa, VisionCare tự hào là trung tâm chăm sóc mắt hàng đầu với đội ngũ bác sĩ chuyên môn cao, trang thiết bị hiện đại và dịch vụ tận tâm.",
   "bullets": [
     "Đội ngũ bác sĩ giàu kinh nghiệm",
     "Công nghệ thiết bị hiện đại nhất",
     "Dịch vụ tận tâm, chu đáo",
     "Chi phí hợp lý, bảo hiểm y tế"
   ],
   "images": [
     "https://images.unsplash.com/photo-1581091870622-7b5f98d3e4f4?auto=format&fit=crop&w=1200&q=80",
     "https://images.unsplash.com/photo-1550831107-1553da8c8464?auto=format&fit=crop&w=1200&q=80",
     "https://images.unsplash.com/photo-1527613426441-4da17471b66d?auto=format&fit=crop&w=1200&q=80",
     "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?auto=format&fit=crop&w=1200&q=80"
   ]
 }');

-- About & Background image giữ như FE hiện tại
DELETE FROM SectionContent WHERE section_key IN ('about','background_image');
INSERT INTO SectionContent(section_key, content, image_url, more_data) VALUES
('about', 'VisionCare cam kết mang đến trải nghiệm khám mắt chuyên nghiệp với công nghệ hiện đại và đội ngũ bác sĩ giàu kinh nghiệm.', 'https://images.unsplash.com/photo-about.jpg', NULL),
('background_image', NULL, 'https://plus.unsplash.com/premium_photo-1677333508737-6b6d642bc6d6?q=80&w=2071&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D', NULL);