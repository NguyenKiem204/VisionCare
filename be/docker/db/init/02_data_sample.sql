-- VisionCare Database Sample Data
-- CHẠY THEO ĐÚNG THỨ TỰ NÀY

-- 1. Insert Roles
INSERT INTO Role (role_name) VALUES 
('Admin'),
('Doctor'),
('Staff'),
('Customer');

-- 2. Insert Permissions
INSERT INTO Permission (permission_name) VALUES 
('VIEW_APPOINTMENTS'),
('CREATE_APPOINTMENTS'),
('UPDATE_APPOINTMENTS'),
('DELETE_APPOINTMENTS'),
('VIEW_PATIENTS'),
('CREATE_PATIENTS'),
('UPDATE_PATIENTS'),
('VIEW_DOCTORS'),
('CREATE_DOCTORS'),
('UPDATE_DOCTORS'),
('VIEW_SERVICES'),
('CREATE_SERVICES'),
('UPDATE_SERVICES'),
('VIEW_REPORTS'),
('MANAGE_SYSTEM');

-- 3. Permission_Role relationships (map bằng tên)
-- Admin
INSERT INTO Permission_Role (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Admin';

-- Doctor
INSERT INTO Permission_Role (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Doctor'
  AND p.permission_name IN ('VIEW_APPOINTMENTS','CREATE_APPOINTMENTS','UPDATE_APPOINTMENTS','VIEW_PATIENTS','UPDATE_PATIENTS');

-- Staff
INSERT INTO Permission_Role (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Staff'
  AND p.permission_name IN ('VIEW_APPOINTMENTS','CREATE_APPOINTMENTS','UPDATE_APPOINTMENTS','VIEW_PATIENTS','CREATE_PATIENTS','UPDATE_PATIENTS','VIEW_SERVICES');

-- Customer
INSERT INTO Permission_Role (permission_id, role_id)
SELECT p.permission_id, r.role_id
FROM Permission p, Role r
WHERE r.role_name = 'Customer'
  AND p.permission_name IN ('VIEW_APPOINTMENTS','VIEW_PATIENTS');

-- 4. Insert Specializations
INSERT INTO Specialization (specialization_name, specialization_status) VALUES 
('Khúc xạ', 'Active'),
('Bệnh võng mạc', 'Active'),
('Bệnh glaucoma', 'Active'),
('Phẫu thuật mắt', 'Active'),
('Khám tổng quát', 'Active'),
('Bệnh giác mạc', 'Active');

-- 5. Insert Accounts
INSERT INTO Accounts (username, password, email, phone_number, role_id, status_account) VALUES 
('admin01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'admin@visioncare.com', '0901234567', 1, 'Active'),
('doctor01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'dr.nguyen@visioncare.com', '0902345678', 2, 'Active'),
('doctor02', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'dr.tran@visioncare.com', '0903456789', 2, 'Active'),
('doctor03', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'dr.le@visioncare.com', '0904567890', 2, 'Active'),
('staff01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'staff01@visioncare.com', '0905678901', 3, 'Active'),
('staff02', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'staff02@visioncare.com', '0906789012', 3, 'Active'),
('customer01', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'nguyen.van.a@gmail.com', '0907890123', 4, 'Active'),
('customer02', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'tran.thi.b@gmail.com', '0908901234', 4, 'Active'),
('customer03', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'le.van.c@gmail.com', '0909012345', 4, 'Active'),
('customer04', '$2a$10$N.zmdr9k7uOCQb0V8.5VV.E3HEGNzHYF5lEFpz5qOqvJ0gN7LnTf6', 'pham.thi.d@gmail.com', '0910123456', 4, 'Active');

-- 6. Insert Customer Ranks
INSERT INTO CustomerRank (rankName, minAmount) VALUES 
('Bronze', 0),
('Silver', 5000000),
('Gold', 10000000),
('Platinum', 20000000);

-- 7. Insert Discounts
INSERT INTO Discount (discountName, percent, rankId, endDate, status) VALUES 
('Bronze Discount', 5, 1, '2025-12-31', TRUE),
('Silver Discount', 10, 2, '2025-12-31', TRUE),
('Gold Discount', 15, 3, '2025-12-31', TRUE),
('Platinum Discount', 20, 4, '2025-12-31', TRUE),
('New Year Special', 25, NULL, '2025-01-31', TRUE);

-- 8. Insert Degrees
INSERT INTO Degree (degree_name) VALUES 
('Bác sĩ Y khoa'),
('Thạc sĩ Y học'),
('Tiến sĩ Y học'),
('Bác sĩ chuyên khoa I'),
('Bác sĩ chuyên khoa II');

-- 9. Insert Certificates
INSERT INTO Certificate (certificate_name) VALUES 
('Chứng chỉ Phẫu thuật Mắt'),
('Chứng chỉ Bệnh Võng mạc'),
('Chứng chỉ Glaucoma'),
('Chứng chỉ Khúc xạ'),
('Chứng chỉ Cấp cứu Y khoa');

-- 10. Insert Doctors
INSERT INTO Doctors (account_id, doctor_name, experience_years, specialization_id, profile_image, rating, gender, dob, address, doctor_status) VALUES 
(2, 'BS. Nguyễn Văn Nam', 15, 1, 'doctor01.jpg', 4.8, 'Nam', '1978-05-15', '123 Lê Lợi, Q.1, TP.HCM', 'Active'),
(3, 'BS. CKI Trần Thị Mai', 12, 2, 'doctor02.jpg', 4.7, 'Nữ', '1982-08-20', '456 Nguyễn Huệ, Q.1, TP.HCM', 'Active'),
(4, 'BS. CKII Lê Minh Tuấn', 20, 3, 'doctor03.jpg', 4.9, 'Nam', '1975-12-10', '789 Hai Bà Trưng, Q.3, TP.HCM', 'Active');

-- 11. Insert Degree_Doctor relationships
INSERT INTO Degree_Doctor (doctor_id, degree_id, degree_image, date_degree, status, issued_by) VALUES 
(1, 1, 'degree_doctor01_1.jpg', '2005-06-15 00:00:00', 'Valid', 'Đại học Y Dược TP.HCM'),
(1, 4, 'degree_doctor01_2.jpg', '2010-08-20 00:00:00', 'Valid', 'Bộ Y tế'),
(2, 1, 'degree_doctor02_1.jpg', '2008-07-10 00:00:00', 'Valid', 'Đại học Y Hà Nội'),
(2, 2, 'degree_doctor02_2.jpg', '2012-09-15 00:00:00', 'Valid', 'Đại học Y Hà Nội'),
(3, 1, 'degree_doctor03_1.jpg', '2002-06-20 00:00:00', 'Valid', 'Đại học Y Dược TP.HCM'),
(3, 5, 'degree_doctor03_2.jpg', '2008-12-15 00:00:00', 'Valid', 'Bộ Y tế');

-- 12. Insert Certificate_Doctor relationships
INSERT INTO Certificate_Doctor (doctor_id, certificate_id, date_certificate, status, issued_by, certificate_image) VALUES 
(1, 4, '2010-03-15 00:00:00', 'Valid', 'Hội Nhãn khoa Việt Nam', 'cert_doctor01_1.jpg'),
(2, 2, '2012-05-20 00:00:00', 'Valid', 'Hội Nhãn khoa Việt Nam', 'cert_doctor02_1.jpg'),
(3, 1, '2008-08-10 00:00:00', 'Valid', 'Hội Nhãn khoa Việt Nam', 'cert_doctor03_1.jpg'),
(3, 3, '2010-11-25 00:00:00', 'Valid', 'Hội Nhãn khoa Việt Nam', 'cert_doctor03_2.jpg');

-- 13. Insert Customers
INSERT INTO Customers (account_id, full_name, address, dob, gender, rankId, image_profile_user) VALUES 
(7, 'Nguyễn Văn A', '12 Trần Hưng Đạo, Q.5, TP.HCM', '1985-03-15', 'Nam', 2, 'customer01.jpg'),
(8, 'Trần Thị B', '34 Lý Tự Trọng, Q.1, TP.HCM', '1990-07-22', 'Nữ', 1, 'customer02.jpg'),
(9, 'Lê Văn C', '56 Pasteur, Q.3, TP.HCM', '1988-11-08', 'Nam', 3, 'customer03.jpg'),
(10, 'Phạm Thị D', '78 Võ Văn Tần, Q.3, TP.HCM', '1992-04-18', 'Nữ', 1, 'customer04.jpg');

-- 14. Insert Staff
INSERT INTO Staff (account_id, admin_fullname, admin_address, admin_dob, admin_gender, image_profile_admin, admin_hired_date, admin_salary) VALUES 
(5, 'Nguyễn Thị Lan', '100 Nguyễn Thị Minh Khai, Q.1, TP.HCM', '1987-09-12', 'Nữ', 'staff01.jpg', '2020-01-15 09:00:00', 15000000),
(6, 'Trần Văn Hưng', '200 Cách Mạng Tháng 8, Q.10, TP.HCM', '1985-02-28', 'Nam', 'staff02.jpg', '2019-03-20 09:00:00', 18000000);

-- 15. Insert Services_Type
INSERT INTO Services_Type (service_type_name, duration_service) VALUES 
('Khám cơ bản', '30 phút'),
('Khám chuyên sâu', '45 phút'),
('Phẫu thuật nhỏ', '60 phút'),
('Phẫu thuật lớn', '120 phút'),
('Tái khám', '15 phút');

-- 16. Insert Services
INSERT INTO Services (service_name, service_description, service_introduce, service_benefit, service_status, specialization_id) VALUES 
('Khám mắt tổng quát', 'Khám tổng quát tình trạng mắt', 'Dịch vụ khám mắt tổng quát bao gồm kiểm tra thị lực, đo nhãn áp, soi đáy mắt...', 'Phát hiện sớm các bệnh lý về mắt, tư vấn chăm sóc mắt hiệu quả', 'Active', 5),
('Đo khúc xạ', 'Đo độ cận, viễn, loạn thị', 'Sử dụng thiết bị hiện đại để đo chính xác độ khúc xạ của mắt', 'Xác định chính xác tình trạng khúc xạ, tư vấn kính phù hợp', 'Active', 1),
('Điều trị glaucoma', 'Điều trị bệnh tăng nhãn áp', 'Điều trị glaucoma bằng thuốc hoặc can thiệp ngoại khoa', 'Kiểm soát nhãn áp, bảo vệ thị thần kinh, duy trì thị lực', 'Active', 3),
('Phẫu thuật đục thủy tinh thể', 'Phẫu thuật thay thể thủy tinh', 'Phẫu thuật phaco hiện đại, an toàn, ít xâm lấn', 'Phục hồi thị lực, cải thiện chất lượng cuộc sống', 'Active', 4),
('Điều trị bệnh võng mạc', 'Điều trị các bệnh lý võng mạc', 'Điều trị bệnh võng mạc tiểu đường, thoái hóa điểm vàng...', 'Ngăn ngừa mù lòa, bảo vệ thị lực trung tâm', 'Active', 2),
('Khám mắt trẻ em', 'Khám và tư vấn cho trẻ em', 'Khám chuyên biệt dành cho trẻ em, phát hiện sớm các vấn đề về mắt', 'Phát hiện và điều trị sớm các bệnh lý mắt ở trẻ', 'Active', 5);

-- 17. Insert Services_Detail
INSERT INTO Services_Detail (service_type_id, service_id, cost) VALUES 
(1, 1, 300000),
(2, 1, 500000),
(1, 2, 200000),
(2, 2, 350000),
(2, 3, 800000),
(3, 3, 1200000),
(4, 4, 25000000),
(3, 4, 18000000),
(2, 5, 1500000),
(3, 5, 2500000),
(1, 6, 250000),
(2, 6, 400000);

-- 18. Insert Slots
INSERT INTO Slots (start_time, end_time, service_type_id) VALUES 
('08:00', '08:30', 1),
('08:30', '09:00', 1),
('09:00', '09:30', 1),
('09:30', '10:00', 1),
('10:00', '10:30', 1),
('10:30', '11:00', 1),
('13:30', '14:15', 2),
('14:15', '15:00', 2),
('15:00', '15:45', 2),
('15:45', '16:30', 2),
('08:00', '09:00', 3),
('09:00', '10:00', 3),
('13:30', '14:45', 5),
('14:45', '15:00', 5),
('15:00', '15:15', 5);

-- 19. Insert Schedules
INSERT INTO Schedules (doctor_id, slot_id, schedule_date, schedule_status) VALUES 
(1, 1, '2025-09-24', 'Available'),
(1, 2, '2025-09-24', 'Available'),
(1, 3, '2025-09-24', 'Booked'),
(1, 7, '2025-09-24', 'Available'),
(2, 4, '2025-09-24', 'Available'),
(2, 5, '2025-09-24', 'Booked'),
(2, 8, '2025-09-24', 'Available'),
(2, 9, '2025-09-24', 'Available'),
(3, 11, '2025-09-25', 'Available'),
(3, 12, '2025-09-25', 'Booked'),
(1, 1, '2025-09-25', 'Available'),
(1, 2, '2025-09-25', 'Available');

-- 20. Insert Appointments (using actual IDs)
INSERT INTO Appointment (appointment_date, appointment_status, doctor_id, slot_id, service_detail_id, discountId, actualCost, patient_id, staff_id) VALUES 
('2025-09-24 09:00:00', 'Completed', 1, 3, 1, 2, 270000, (SELECT account_id FROM Accounts WHERE username = 'customer01'), 1),
('2025-09-24 10:30:00', 'Completed', 2, 5, 3, NULL, 200000, (SELECT account_id FROM Accounts WHERE username = 'customer02'), 1),
('2025-09-25 09:00:00', 'Confirmed', 3, 12, 7, 3, 21250000, (SELECT account_id FROM Accounts WHERE username = 'customer03'), 2),
('2025-09-26 08:00:00', 'Pending', 1, 1, 2, NULL, 500000, (SELECT account_id FROM Accounts WHERE username = 'customer04'), 1);

-- 21. Insert Medical History
INSERT INTO MedicalHistory (appointment_id, diagnosis, symptoms, treatment, prescription, vision_left, vision_right, additional_tests, note) VALUES 
(1, 'Cận thị nhẹ', 'Mờ mắt khi nhìn xa', 'Đeo kính cận', 'Kính cận -1.5D', 0.8, 0.9, 'Đo nhãn áp: bình thường', 'Tái khám sau 6 tháng'),
(2, 'Khô mắt', 'Mắt khô, cộm cộm', 'Thuốc nhỏ mắt', 'Systane Ultra 4 lần/ngày', 1.0, 1.0, 'Test Schirmer: 8mm', 'Uống nhiều nước, nghỉ ngơi hợp lý');

-- 22. Insert Images_Type
INSERT INTO Images_Type (image_type) VALUES 
('Service'),
('Banner'),
('Doctor'),
('Equipment'),
('Before/After');

-- 23. Insert Images_Service
INSERT INTO Images_Service (service_id, image_main, image_before, image_after) VALUES 
(1, 'service01_main.jpg', 'service01_before.jpg', 'service01_after.jpg'),
(2, 'service02_main.jpg', 'service02_before.jpg', 'service02_after.jpg'),
(4, 'service04_main.jpg', 'service04_before.jpg', 'service04_after.jpg');

-- 24. Insert Images_Video
INSERT INTO Images_Video (image_url, image_description, image_type_id) VALUES 
('banner01.jpg', 'Banner khuyến mãi tháng 9', 2),
('banner02.jpg', 'Banner dịch vụ mới', 2),
('equipment01.jpg', 'Máy đo thị lực tự động', 4),
('equipment02.jpg', 'Máy phẫu thuật Phaco', 4);

-- 25. Insert Machines
INSERT INTO Machine (machine_name, machine_description, machine_img) VALUES 
('Máy đo thị lực tự động Topcon', 'Máy đo khúc xạ và thị lực tự động hiện đại', 'topcon_machine.jpg'),
('Máy phẫu thuật Phaco Alcon', 'Hệ thống phẫu thuật đục thủy tinh thể tiên tiến', 'alcon_phaco.jpg'),
('Máy chụp OCT Zeiss', 'Máy chụp cắt lớp quang học võng mạc', 'zeiss_oct.jpg'),
('Máy đo nhãn áp Goldmann', 'Máy đo nhãn áp tiêu chuẩn vàng', 'goldmann_tonometer.jpg');

-- 26. Insert Banners
INSERT INTO Banner (banner_name, banner_title, banner_description, banner_status, link_banner, href_banner) VALUES 
('Khuyến mãi tháng 9', 'Giảm giá 20% khám mắt tổng quát', 'Áp dụng từ 1-30/9/2025', 'Active', 'banner01.jpg', '/promotion/september'),
('Dịch vụ mới', 'Ra mắt dịch vụ điều trị võng mạc', 'Công nghệ tiên tiến từ Châu Âu', 'Active', 'banner02.jpg', '/services/retina');

-- 27. Insert Blog (using actual account_id values)
INSERT INTO Blog (blog_content, author_id, created_date_blog, title_meta, title_image_blog) VALUES 
('Bệnh cận thị đang ngày càng gia tăng, đặc biệt ở trẻ em và thanh thiếu niên. Việc phòng ngừa cận thị cần được quan tâm từ sớm...', (SELECT account_id FROM Accounts WHERE username = 'admin01'), '2025-09-20 14:30:00', 'Cách phòng ngừa cận thị hiệu quả', 'blog01.jpg'),
('Đục thủy tinh thể là bệnh lý thường gặp ở người cao tuổi. Tìm hiểu về triệu chứng và phương pháp điều trị hiện đại...', (SELECT account_id FROM Accounts WHERE username = 'doctor01'), '2025-09-18 10:15:00', 'Tất cả về bệnh đục thủy tinh thể', 'blog02.jpg');

-- 28. Insert Comment Blog (using actual account_id values)
INSERT INTO CommentBlog (comment, author_id, tuongtac, parent_comment_id, blog_id) VALUES 
('Bài viết rất hữu ích, cảm ơn bác sĩ!', (SELECT account_id FROM Accounts WHERE username = 'customer01'), 1, NULL, 1),
('Con tôi 8 tuổi đã cận thị, không biết có cách nào khắc phục không?', (SELECT account_id FROM Accounts WHERE username = 'customer02'), 1, NULL, 1),
('Bạn nên đưa con đến khám để được tư vấn cụ thể nhé', (SELECT account_id FROM Accounts WHERE username = 'customer01'), 1, 2, 1);

-- 29. Insert Content Stories
INSERT INTO Content_Stories (patient_name, image_patient, content_stories) VALUES 
('Cô Lan (58 tuổi)', 'patient_story01.jpg', 'Tôi bị đục thủy tinh thể nhiều năm, không dám phẫu thuật. Sau khi được bác sĩ tư vấn kỹ, tôi quyết định thực hiện. Kết quả vượt ngoài mong đợi, giờ tôi nhìn rõ hơn thời trẻ!'),
('Anh Minh (35 tuổi)', 'patient_story02.jpg', 'Làm việc với máy tính nhiều khiến mắt tôi mệt mỏi, khô mắt. Sau liệu trình điều trị tại đây, tình trạng đã cải thiện rõ rệt.');

-- 30. Insert Feedback_Service
INSERT INTO Feedback_Service (appointment_id, feedback_text, feedback_date, feedback_rating, response_text, response_date, staff_id) VALUES 
(1, 'Dịch vụ tốt, bác sĩ tận tâm', '2025-09-24 16:00:00', 5, 'Cảm ơn anh đã tin tưởng dịch vụ của chúng tôi', '2025-09-24 17:00:00', 1),
(2, 'Phòng khám sạch sẽ, nhân viên thân thiện', '2025-09-24 15:30:00', 4, 'Chúng tôi sẽ cố gắng cải thiện hơn nữa', '2025-09-24 16:30:00', 1);

-- 31. Insert Feedback_Doctor
INSERT INTO Feedback_Doctor (appointment_id, feedback_text, feedback_date, feedback_rating, response_text, response_date, staff_id) VALUES 
(1, 'Bác sĩ giải thích rất kỹ, dễ hiểu', '2025-09-24 16:00:00', 5, 'Cảm ơn phản hồi tích cực của bệnh nhân', '2025-09-24 17:00:00', 1),
(2, 'Bác sĩ chuyên môn cao, thăm khám cẩn thận', '2025-09-24 15:30:00', 5, 'Rất vui khi được bệnh nhân tin tưởng', '2025-09-24 16:30:00', 1);

-- 32. Insert CheckOut
INSERT INTO CheckOut (appointment_id, transaction_type, transaction_status, total_bill, checkout_code, checkout_time) VALUES 
(1, 'Cash', 'Completed', 270000, 'PAY001', '2025-09-24 15:45:00'),
(2, 'Card', 'Completed', 200000, 'PAY002', '2025-09-24 14:20:00');

-- 33. Insert Follow_Up
INSERT INTO Follow_Up (next_follow_up_date, follow_up_description, PatientName, phone, email) VALUES 
('2025-12-24', 'Tái khám kiểm tra thị lực', 'Nguyễn Văn A', '0907890123', 'nguyen.van.a@gmail.com'),
('2025-11-24', 'Kiểm tra tình trạng khô mắt', 'Trần Thị B', '0908901234', 'tran.thi.b@gmail.com');

-- 34. Update doctor ratings based on feedback
UPDATE Doctors SET rating = (
    SELECT AVG(feedback_rating::FLOAT) 
    FROM Feedback_Doctor fd 
    JOIN Appointment a ON fd.appointment_id = a.appointment_id 
    WHERE a.doctor_id = Doctors.doctor_id
) WHERE doctor_id IN (SELECT DISTINCT doctor_id FROM Appointment);