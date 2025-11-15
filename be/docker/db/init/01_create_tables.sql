-- Connect to the database
\c visioncare;

-- Drop all tables if they exist (with CASCADE to handle foreign keys)
DROP TABLE IF EXISTS CommentBlog CASCADE;
DROP TABLE IF EXISTS Blog CASCADE;
DROP TABLE IF EXISTS ImagesService CASCADE;
DROP TABLE IF EXISTS Banner CASCADE;
DROP TABLE IF EXISTS Machine CASCADE;
DROP TABLE IF EXISTS Equipment CASCADE;
DROP TABLE IF EXISTS ContentStories CASCADE;
DROP TABLE IF EXISTS CheckOut CASCADE;
DROP TABLE IF EXISTS Discount CASCADE;
DROP TABLE IF EXISTS FeedbackService CASCADE;
DROP TABLE IF EXISTS FeedbackDoctor CASCADE;
DROP TABLE IF EXISTS FollowUp CASCADE;
DROP TABLE IF EXISTS AuditLogs CASCADE;
DROP TABLE IF EXISTS Claims CASCADE;
DROP TABLE IF EXISTS RefreshTokens CASCADE;
DROP TABLE IF EXISTS OTPServices CASCADE;
DROP TABLE IF EXISTS MedicalHistory CASCADE;
DROP TABLE IF EXISTS Appointment CASCADE;
DROP TABLE IF EXISTS Staff CASCADE;
DROP TABLE IF EXISTS DoctorAbsence CASCADE;
DROP TABLE IF EXISTS DoctorSchedule CASCADE;
DROP TABLE IF EXISTS WeeklySchedule CASCADE;
DROP TABLE IF EXISTS Schedules CASCADE;
DROP TABLE IF EXISTS Slots CASCADE;
DROP TABLE IF EXISTS WorkShift CASCADE;
DROP TABLE IF EXISTS Rooms CASCADE;
DROP TABLE IF EXISTS ServicesDetail CASCADE;
DROP TABLE IF EXISTS Services CASCADE;
DROP TABLE IF EXISTS ServicesType CASCADE;
DROP TABLE IF EXISTS Customers CASCADE;
DROP TABLE IF EXISTS CustomerRank CASCADE;
DROP TABLE IF EXISTS CertificateDoctor CASCADE;
DROP TABLE IF EXISTS Certificate CASCADE;
DROP TABLE IF EXISTS DegreeDoctor CASCADE;
DROP TABLE IF EXISTS Degree CASCADE;
DROP TABLE IF EXISTS Doctors CASCADE;
DROP TABLE IF EXISTS Accounts CASCADE;
DROP TABLE IF EXISTS PermissionRole CASCADE;
DROP TABLE IF EXISTS Specialization CASCADE;
DROP TABLE IF EXISTS Permission CASCADE;
DROP TABLE IF EXISTS Role CASCADE;

-- ===== CORE AUTHENTICATION & AUTHORIZATION TABLES =====

CREATE TABLE Role (
    role_id SERIAL PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL UNIQUE,
    role_description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Permission (
    permission_id SERIAL PRIMARY KEY,
    permission_name VARCHAR(100) NOT NULL UNIQUE,
    resource VARCHAR(50) NOT NULL, -- users, appointments, medical_records, services, blog, etc.
    action VARCHAR(20) NOT NULL,   -- create, read, update, delete, manage
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE PermissionRole (
    permission_id INTEGER NOT NULL,
    role_id INTEGER NOT NULL,
    granted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (permission_id, role_id),
    FOREIGN KEY (permission_id) REFERENCES Permission(permission_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES Role(role_id) ON DELETE CASCADE
);

CREATE TABLE Accounts (
    account_id SERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    username VARCHAR(100) UNIQUE,
    password_hash VARCHAR(255), -- NULL for OAuth-only accounts
    
    -- Security fields
    email_confirmed BOOLEAN DEFAULT FALSE,
    email_confirmation_token VARCHAR(255),
    lockout_end TIMESTAMP,
    access_failed_count SMALLINT DEFAULT 0,
    
    -- Password management
    password_reset_token VARCHAR(255),
    password_reset_expires TIMESTAMP,
    last_password_change TIMESTAMP,
    
    -- OAuth integration
    google_id VARCHAR(255) UNIQUE,
    facebook_id VARCHAR(255) UNIQUE,
    
    -- Account management
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMP,
    status VARCHAR(20) DEFAULT 'Active', -- Active, Suspended, Deleted
    
    role_id INTEGER NOT NULL,
    
    FOREIGN KEY (role_id) REFERENCES Role(role_id)
);

CREATE TABLE Claims (
    claim_id SERIAL PRIMARY KEY,
    account_id INTEGER NOT NULL,
    claim_type VARCHAR(50) NOT NULL,
    claim_value VARCHAR(255) NOT NULL,
    expires_at TIMESTAMP, -- NULL = no expiration
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id) ON DELETE CASCADE,
    UNIQUE(account_id, claim_type, claim_value)
);

CREATE TABLE RefreshTokens (
    token_id SERIAL PRIMARY KEY,
    token_hash VARCHAR(255) NOT NULL UNIQUE,
    account_id INTEGER NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    revoked_at TIMESTAMP,
    created_by_ip INET,
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id) ON DELETE CASCADE
);

CREATE TABLE OTPServices (
    otp_id SERIAL PRIMARY KEY,
    account_id INTEGER NOT NULL,
    otp_hash VARCHAR(255) NOT NULL,
    otp_type VARCHAR(30) NOT NULL, -- EMAIL_CONFIRMATION, PASSWORD_RESET, LOGIN
    expires_at TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    used_at TIMESTAMP,
    attempts SMALLINT DEFAULT 0,
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id) ON DELETE CASCADE
);

CREATE TABLE AuditLogs (
    audit_id SERIAL PRIMARY KEY,
    account_id INTEGER,
    action VARCHAR(100) NOT NULL,
    resource VARCHAR(50),
    resource_id INTEGER,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    ip_address INET,
    success BOOLEAN NOT NULL,
    details JSONB,
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);

-- ===== BUSINESS DOMAIN TABLES =====

CREATE TABLE Specialization (
    specialization_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    status VARCHAR(20) DEFAULT 'Active'
);

CREATE TABLE CustomerRank (
    rank_id SERIAL PRIMARY KEY,
    rank_name VARCHAR(50) NOT NULL,
    min_amount DECIMAL(12,2) NOT NULL
);

CREATE TABLE Discount (
    discount_id SERIAL PRIMARY KEY,
    discount_name VARCHAR(100) NOT NULL,
    discount_percent DECIMAL(5,2) NOT NULL,
    rank_id INTEGER,
    start_date DATE DEFAULT CURRENT_DATE,
    end_date DATE,
    status VARCHAR(20) DEFAULT 'Active',
    
    FOREIGN KEY (rank_id) REFERENCES CustomerRank(rank_id)
);

CREATE TABLE Customers (
    account_id INTEGER PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    address TEXT,
    dob DATE,
    gender VARCHAR(10),
    rank_id INTEGER,
    avatar VARCHAR(255),
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id) ON DELETE CASCADE,
    FOREIGN KEY (rank_id) REFERENCES CustomerRank(rank_id)
);

CREATE TABLE Doctors (
    account_id INTEGER PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    experience_years SMALLINT,
    specialization_id INTEGER NOT NULL,
    avatar VARCHAR(255),
    rating DECIMAL(2,1) DEFAULT 0.0,
    gender VARCHAR(10),
    dob DATE,
    address TEXT,
    status VARCHAR(20) DEFAULT 'Active',
    biography TEXT,
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id) ON DELETE CASCADE,
    FOREIGN KEY (specialization_id) REFERENCES Specialization(specialization_id)
);

CREATE TABLE Staff (
    account_id INTEGER PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    address TEXT,
    dob DATE,
    gender VARCHAR(10),
    avatar VARCHAR(255),
    hired_date DATE DEFAULT CURRENT_DATE,
    salary DECIMAL(12,2),
    
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id) ON DELETE CASCADE
);

-- Doctor Credentials
CREATE TABLE Degree (
    degree_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE DegreeDoctor (
    doctor_id INTEGER,
    degree_id INTEGER,
    issued_date DATE,
    issued_by VARCHAR(255),
    certificate_image VARCHAR(255),
    status VARCHAR(20) DEFAULT 'Active',
    
    PRIMARY KEY (doctor_id, degree_id),
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    FOREIGN KEY (degree_id) REFERENCES Degree(degree_id)
);

CREATE TABLE Certificate (
    certificate_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE CertificateDoctor (
    doctor_id INTEGER,
    certificate_id INTEGER,
    issued_date DATE,
    issued_by VARCHAR(255),
    certificate_image VARCHAR(255),
    expiry_date DATE,
    status VARCHAR(20) DEFAULT 'Active',
    
    PRIMARY KEY (doctor_id, certificate_id),
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    FOREIGN KEY (certificate_id) REFERENCES Certificate(certificate_id)
);

-- Services Management
CREATE TABLE ServicesType (
    service_type_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    duration_minutes SMALLINT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

CREATE TABLE Services (
    service_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    benefits TEXT,
    status VARCHAR(20) DEFAULT 'Active',
    specialization_id INTEGER,
    
    FOREIGN KEY (specialization_id) REFERENCES Specialization(specialization_id)
);

CREATE TABLE ServicesDetail (
    service_detail_id SERIAL PRIMARY KEY,
    service_id INTEGER NOT NULL,
    service_type_id INTEGER NOT NULL,
    cost DECIMAL(10,2) NOT NULL,
    
    FOREIGN KEY (service_id) REFERENCES Services(service_id) ON DELETE CASCADE,
    FOREIGN KEY (service_type_id) REFERENCES ServicesType(service_type_id)
);

CREATE TABLE ImagesService (
    service_id INTEGER PRIMARY KEY,
    image_main VARCHAR(255),
    image_before VARCHAR(255),
    image_after VARCHAR(255),
    
    FOREIGN KEY (service_id) REFERENCES Services(service_id) ON DELETE CASCADE
);

-- Resources Management
CREATE TABLE Rooms (
    room_id SERIAL PRIMARY KEY,
    room_name VARCHAR(100) NOT NULL UNIQUE,
    room_code VARCHAR(20) UNIQUE,
    capacity INTEGER DEFAULT 1,
    status VARCHAR(20) DEFAULT 'Active', -- Active, Maintenance, Inactive
    location VARCHAR(255),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

-- Work Shift Management
CREATE TABLE WorkShift (
    shift_id SERIAL PRIMARY KEY,
    shift_name VARCHAR(100) NOT NULL,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    is_active BOOLEAN DEFAULT true,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    CHECK (end_time > start_time)
);

-- Equipment Management
CREATE TABLE Equipment (
    equipment_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    model VARCHAR(255),
    serial_number VARCHAR(255),
    manufacturer VARCHAR(255),
    purchase_date DATE,
    last_maintenance_date DATE,
    status VARCHAR(50) DEFAULT 'Active', -- Active, Maintenance, Broken
    location VARCHAR(255),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

-- Scheduling System
CREATE TABLE Slots (
    slot_id SERIAL PRIMARY KEY,
    start_time TIME NOT NULL,
    end_time TIME NOT NULL,
    service_type_id INTEGER NOT NULL,
    
    FOREIGN KEY (service_type_id) REFERENCES ServicesType(service_type_id),
    UNIQUE (start_time, service_type_id)
);

CREATE TABLE Schedules (
    schedule_id SERIAL PRIMARY KEY,
    doctor_id INTEGER NOT NULL,
    slot_id INTEGER NOT NULL,
    schedule_date DATE NOT NULL,
    status VARCHAR(20) DEFAULT 'Available', -- Available, Booked, Unavailable, Blocked
    room_id INTEGER,
    equipment_id INTEGER,
    
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    FOREIGN KEY (slot_id) REFERENCES Slots(slot_id),
    FOREIGN KEY (room_id) REFERENCES Rooms(room_id),
    FOREIGN KEY (equipment_id) REFERENCES Equipment(equipment_id),
    UNIQUE (doctor_id, slot_id, schedule_date)
);

-- Weekly Schedule Template (for recurring schedules)
CREATE TABLE WeeklySchedule (
    weekly_schedule_id SERIAL PRIMARY KEY,
    doctor_id INTEGER NOT NULL,
    day_of_week INTEGER NOT NULL, -- 0=Sunday, 1=Monday, 2=Tuesday, etc.
    slot_id INTEGER NOT NULL,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    FOREIGN KEY (slot_id) REFERENCES Slots(slot_id),
    UNIQUE (doctor_id, day_of_week, slot_id)
);

-- Doctor Schedule with Recurrence (New flexible scheduling)
CREATE TABLE DoctorSchedule (
    doctor_schedule_id SERIAL PRIMARY KEY,
    doctor_id INTEGER NOT NULL,
    shift_id INTEGER NOT NULL,
    room_id INTEGER,
    equipment_id INTEGER,
    start_date DATE NOT NULL,
    end_date DATE, -- NULL = no end date
    day_of_week INTEGER, -- 1=Monday, 2=Tuesday, ..., 7=Sunday, NULL = all days
    recurrence_rule VARCHAR(50) DEFAULT 'WEEKLY', -- 'DAILY', 'WEEKLY', 'MONTHLY', 'CUSTOM'
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    FOREIGN KEY (shift_id) REFERENCES WorkShift(shift_id),
    FOREIGN KEY (room_id) REFERENCES Rooms(room_id),
    FOREIGN KEY (equipment_id) REFERENCES Equipment(equipment_id),
    CHECK (end_date IS NULL OR end_date >= start_date),
    CHECK (day_of_week IS NULL OR (day_of_week >= 1 AND day_of_week <= 7))
);

-- Doctor Absence/Leave Management
CREATE TABLE DoctorAbsence (
    absence_id SERIAL PRIMARY KEY,
    doctor_id INTEGER NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    absence_type VARCHAR(50) DEFAULT 'Leave', -- Leave, Emergency, Sick, Other
    reason TEXT,
    status VARCHAR(20) DEFAULT 'Pending', -- Pending, Approved, Rejected
    is_resolved BOOLEAN DEFAULT false,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    CHECK (end_date >= start_date)
);

-- Appointment & Medical Records
CREATE TABLE Appointment (
    appointment_id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    doctor_id INTEGER NOT NULL,
    service_detail_id INTEGER NOT NULL,
    discount_id INTEGER,
    appointment_datetime TIMESTAMP NOT NULL,
    status VARCHAR(30) DEFAULT 'Scheduled', -- Scheduled, Completed, Cancelled, No_Show
    appointment_code VARCHAR(20) UNIQUE, -- Format: VC-YYYYMMDD-XXXXXX
    payment_status VARCHAR(30) DEFAULT 'Unpaid', -- Unpaid, Pending, Paid, Failed, Refunding, Refunded
    actual_cost DECIMAL(10,2),
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    created_by INTEGER, -- Staff who created the appointment
    
    FOREIGN KEY (patient_id) REFERENCES Customers(account_id),
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id),
    FOREIGN KEY (service_detail_id) REFERENCES ServicesDetail(service_detail_id),
    FOREIGN KEY (discount_id) REFERENCES Discount(discount_id),
    FOREIGN KEY (created_by) REFERENCES Accounts(account_id)
);

CREATE INDEX idx_appointment_code ON Appointment(appointment_code);

CREATE TABLE MedicalHistory (
    medical_history_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL,
    diagnosis TEXT,
    symptoms TEXT,
    treatment TEXT,
    prescription TEXT,
    vision_left DECIMAL(3,2),
    vision_right DECIMAL(3,2),
    additional_tests TEXT,
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE
);

CREATE TABLE FollowUp (
    follow_up_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL,
    next_appointment_date DATE,
    description TEXT,
    status VARCHAR(20) DEFAULT 'Pending', -- Pending, Scheduled, Completed, Cancelled
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE
);

-- EHR: Encounters, Prescriptions, Orders
CREATE TABLE IF NOT EXISTS Encounters (
    encounter_id SERIAL PRIMARY KEY,
    appointment_id INT NOT NULL,
    doctor_id INT NOT NULL,
    customer_id INT NOT NULL,
    subjective TEXT,
    objective TEXT,
    assessment TEXT,
    plan TEXT,
    status VARCHAR(20) NOT NULL DEFAULT 'Draft', -- Draft|Signed
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    CONSTRAINT fk_encounter_appointment FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE,
    CONSTRAINT fk_encounter_doctor FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE RESTRICT,
    CONSTRAINT fk_encounter_customer FOREIGN KEY (customer_id) REFERENCES Customers(account_id) ON DELETE RESTRICT
);
CREATE INDEX IF NOT EXISTS idx_encounters_doctor ON Encounters(doctor_id);
CREATE INDEX IF NOT EXISTS idx_encounters_appointment ON Encounters(appointment_id);

CREATE TABLE IF NOT EXISTS Prescriptions (
    prescription_id SERIAL PRIMARY KEY,
    encounter_id INT NOT NULL,
    notes TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    CONSTRAINT fk_prescription_encounter FOREIGN KEY (encounter_id) REFERENCES Encounters(encounter_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_prescriptions_encounter ON Prescriptions(encounter_id);

CREATE TABLE IF NOT EXISTS PrescriptionLines (
    line_id SERIAL PRIMARY KEY,
    prescription_id INT NOT NULL,
    drug_code VARCHAR(100) NULL,
    drug_name VARCHAR(255) NOT NULL,
    dosage VARCHAR(100) NULL,
    frequency VARCHAR(100) NULL,
    duration VARCHAR(100) NULL,
    instructions TEXT NULL,
    CONSTRAINT fk_line_prescription FOREIGN KEY (prescription_id) REFERENCES Prescriptions(prescription_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_lines_prescription ON PrescriptionLines(prescription_id);

CREATE TABLE IF NOT EXISTS Orders (
    order_id SERIAL PRIMARY KEY,
    encounter_id INT NOT NULL,
    order_type VARCHAR(50) NOT NULL, -- Test|Procedure
    name VARCHAR(255) NOT NULL,
    status VARCHAR(30) NOT NULL DEFAULT 'Requested', -- Requested|InProgress|Completed|Canceled
    result_url TEXT NULL,
    notes TEXT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    CONSTRAINT fk_order_encounter FOREIGN KEY (encounter_id) REFERENCES Encounters(encounter_id) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_orders_encounter ON Orders(encounter_id);

-- Payment System
CREATE TABLE CheckOut (
    checkout_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL,
    transaction_type VARCHAR(50) DEFAULT 'VNPay', -- VNPay (chỉ thanh toán online)
    transaction_status VARCHAR(30), -- Pending, Completed, Failed, Refunded
    total_amount DECIMAL(10,2) NOT NULL,
    transaction_code VARCHAR(100),
    payment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes TEXT,
    payment_gateway VARCHAR(50), -- VNPay, MoMo, Stripe
    gateway_transaction_id VARCHAR(255),
    gateway_response JSONB, -- Full response từ gateway
    refund_requested_at TIMESTAMP,
    refund_completed_at TIMESTAMP,
    refund_amount DECIMAL(10,2),
    refund_reason TEXT,
    
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE
);

CREATE INDEX idx_checkout_gateway_txn ON CheckOut(gateway_transaction_id);
CREATE INDEX idx_checkout_status ON CheckOut(transaction_status);

-- Feedback System
CREATE TABLE FeedbackService (
    feedback_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL,
    rating SMALLINT CHECK (rating BETWEEN 1 AND 5),
    feedback_text TEXT,
    feedback_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    response_text TEXT,
    response_date TIMESTAMP,
    responded_by INTEGER, -- Staff who responded
    
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE,
    FOREIGN KEY (responded_by) REFERENCES Staff(account_id)
);

CREATE TABLE FeedbackDoctor (
    feedback_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL,
    rating SMALLINT CHECK (rating BETWEEN 1 AND 5),
    feedback_text TEXT,
    feedback_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    response_text TEXT,
    response_date TIMESTAMP,
    responded_by INTEGER, -- Staff who responded
    
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE,
    FOREIGN KEY (responded_by) REFERENCES Staff(account_id)
);

-- Content Management
CREATE TABLE Blog (
    blog_id SERIAL PRIMARY KEY,
    title VARCHAR(500) NOT NULL,
    slug VARCHAR(500) NOT NULL UNIQUE,
    content TEXT NOT NULL,
    excerpt TEXT,
    featured_image VARCHAR(255),
    author_id INTEGER NOT NULL,
    status VARCHAR(20) DEFAULT 'Draft', -- Draft, Published, Archived
    published_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    view_count INTEGER DEFAULT 0,
    
    FOREIGN KEY (author_id) REFERENCES Accounts(account_id)
);

CREATE TABLE CommentBlog (
    comment_id SERIAL PRIMARY KEY,
    blog_id INTEGER NOT NULL,
    author_id INTEGER,
    parent_comment_id INTEGER,
    comment_text TEXT NOT NULL,
    status VARCHAR(20) DEFAULT 'Active', -- Active, Hidden, Deleted
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (blog_id) REFERENCES Blog(blog_id) ON DELETE CASCADE,
    FOREIGN KEY (author_id) REFERENCES Accounts(account_id),
    FOREIGN KEY (parent_comment_id) REFERENCES CommentBlog(comment_id)
);

-- Marketing & Content
CREATE TABLE Banner (
    banner_id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    image_url VARCHAR(255),
    link_url VARCHAR(255),
    display_order INTEGER DEFAULT 0,
    status VARCHAR(20) DEFAULT 'Active', -- Active, Inactive
    start_date DATE DEFAULT CURRENT_DATE,
    end_date DATE,
    
    UNIQUE(display_order, status)
);

CREATE TABLE ContentStories (
    story_id SERIAL PRIMARY KEY,
    patient_name VARCHAR(255),
    patient_image VARCHAR(255),
    story_content TEXT NOT NULL,
    display_order INTEGER DEFAULT 0,
    status VARCHAR(20) DEFAULT 'Active',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Machine (
    machine_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    image_url VARCHAR(255),
    specifications TEXT,
    status VARCHAR(20) DEFAULT 'Active'
);

CREATE TABLE SectionContent (
    section_key VARCHAR(100) PRIMARY KEY, -- (hero_slider, why_us, about, background_image...)
    content TEXT,           -- HTML hoặc JSON hoặc text, tùy section
    image_url VARCHAR(255), -- Dành cho section cần 1 ảnh riêng lẻ
    more_data JSONB,        -- Dành cho dữ liệu động mở rộng (list slide, v.v.)
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ===== PERFORMANCE INDEXES =====

-- Authentication indexes
CREATE INDEX idx_accounts_email ON Accounts(email);
CREATE INDEX idx_accounts_username ON Accounts(username);
CREATE INDEX idx_accounts_role ON Accounts(role_id);
CREATE INDEX idx_accounts_status ON Accounts(status);
CREATE INDEX idx_accounts_google_id ON Accounts(google_id) WHERE google_id IS NOT NULL;
CREATE INDEX idx_accounts_facebook_id ON Accounts(facebook_id) WHERE facebook_id IS NOT NULL;

-- Security indexes
CREATE INDEX idx_refresh_tokens_account ON RefreshTokens(account_id);
CREATE INDEX idx_refresh_tokens_expires ON RefreshTokens(expires_at) WHERE revoked_at IS NULL;
CREATE INDEX idx_claims_account_type ON Claims(account_id, claim_type);
CREATE INDEX idx_otp_account_type ON OTPServices(account_id, otp_type) WHERE used_at IS NULL;

-- Business logic indexes
CREATE INDEX idx_doctors_specialization ON Doctors(specialization_id);
CREATE INDEX idx_appointments_patient ON Appointment(patient_id);
CREATE INDEX idx_appointments_doctor ON Appointment(doctor_id);
CREATE INDEX idx_appointments_datetime ON Appointment(appointment_datetime);
CREATE INDEX idx_appointments_status ON Appointment(status);

-- Scheduling indexes
CREATE INDEX idx_schedules_doctor ON Schedules(doctor_id);
CREATE INDEX idx_schedules_date ON Schedules(schedule_date);
CREATE INDEX idx_schedules_status ON Schedules(status);
CREATE INDEX idx_schedules_room ON Schedules(room_id) WHERE room_id IS NOT NULL;
CREATE INDEX idx_schedules_equipment ON Schedules(equipment_id) WHERE equipment_id IS NOT NULL;
CREATE INDEX idx_weekly_schedules_doctor ON WeeklySchedule(doctor_id);
CREATE INDEX idx_doctor_schedules_doctor ON DoctorSchedule(doctor_id);
CREATE INDEX idx_doctor_schedules_shift ON DoctorSchedule(shift_id);
CREATE INDEX idx_doctor_schedules_active ON DoctorSchedule(is_active) WHERE is_active = true;
CREATE INDEX idx_doctor_absence_doctor ON DoctorAbsence(doctor_id);
CREATE INDEX idx_doctor_absence_status ON DoctorAbsence(status);
CREATE INDEX idx_doctor_absence_dates ON DoctorAbsence(start_date, end_date);
CREATE INDEX idx_rooms_status ON Rooms(status);
CREATE INDEX idx_work_shift_active ON WorkShift(is_active) WHERE is_active = true;
CREATE INDEX idx_schedules_doctor_date ON Schedules(doctor_id, schedule_date);
CREATE INDEX idx_medical_history_appointment ON MedicalHistory(appointment_id);

-- Performance indexes for refresh tokens (SHA256 + salt optimization)
-- These indexes are optimized for the new SHA256 hash system instead of BCrypt
CREATE INDEX idx_refreshtokens_valid 
ON refreshtokens (revoked_at, expires_at) 
WHERE revoked_at IS NULL;

CREATE INDEX idx_refreshtokens_account_lookup 
ON refreshtokens (account_id) 
WHERE revoked_at IS NULL;

CREATE INDEX idx_refreshtokens_hash 
ON refreshtokens (token_hash) 
WHERE revoked_at IS NULL;

CREATE INDEX idx_refreshtokens_cleanup 
ON refreshtokens (expires_at) 
WHERE revoked_at IS NULL;

-- Additional performance indexes
CREATE INDEX idx_accounts_email_lookup 
ON accounts (email) 
WHERE status = 'Active';

CREATE INDEX idx_accounts_role_lookup 
ON accounts (role_id) 
WHERE status = 'Active';

-- Content indexes
CREATE INDEX idx_blog_author ON Blog(author_id);
CREATE INDEX idx_blog_status_published ON Blog(status, published_at);
CREATE UNIQUE INDEX idx_blog_slug ON Blog(slug);
CREATE INDEX idx_comment_blog ON CommentBlog(blog_id);

-- Feedback indexes
CREATE INDEX idx_feedback_service_appointment ON FeedbackService(appointment_id);
CREATE INDEX idx_feedback_doctor_appointment ON FeedbackDoctor(appointment_id);

-- Payment indexes
CREATE INDEX idx_checkout_appointment ON CheckOut(appointment_id);

-- Audit indexes
CREATE INDEX idx_audit_account ON AuditLogs(account_id);
CREATE INDEX idx_audit_timestamp ON AuditLogs(timestamp);
CREATE INDEX idx_audit_resource ON AuditLogs(resource, resource_id);

-- Equipment indexes
CREATE INDEX idx_equipment_name ON Equipment(name);
CREATE INDEX idx_equipment_status ON Equipment(status);
CREATE INDEX idx_equipment_location ON Equipment(location);
CREATE INDEX idx_equipment_manufacturer ON Equipment(manufacturer);