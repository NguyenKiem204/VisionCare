-- Connect to the database
\c visioncare;

-- Drop all tables if they exist (with CASCADE to handle foreign keys)
DROP TABLE IF EXISTS CommentBlog CASCADE;
DROP TABLE IF EXISTS Blog CASCADE;
DROP TABLE IF EXISTS ImagesService CASCADE;
DROP TABLE IF EXISTS Banner CASCADE;
DROP TABLE IF EXISTS Machine CASCADE;
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
DROP TABLE IF EXISTS Schedules CASCADE;
DROP TABLE IF EXISTS Slots CASCADE;
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
    duration_minutes SMALLINT NOT NULL
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
    status VARCHAR(20) DEFAULT 'Available', -- Available, Booked, Unavailable
    
    FOREIGN KEY (doctor_id) REFERENCES Doctors(account_id) ON DELETE CASCADE,
    FOREIGN KEY (slot_id) REFERENCES Slots(slot_id),
    UNIQUE (doctor_id, slot_id, schedule_date)
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

-- Payment System
CREATE TABLE CheckOut (
    checkout_id SERIAL PRIMARY KEY,
    appointment_id INTEGER NOT NULL,
    transaction_type VARCHAR(50), -- Cash, Card, Transfer, Insurance
    transaction_status VARCHAR(30), -- Pending, Completed, Failed, Refunded
    total_amount DECIMAL(10,2) NOT NULL,
    transaction_code VARCHAR(100),
    payment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes TEXT,
    
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) ON DELETE CASCADE
);

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
CREATE INDEX idx_schedules_doctor_date ON Schedules(doctor_id, schedule_date);
CREATE INDEX idx_medical_history_appointment ON MedicalHistory(appointment_id);

-- Content indexes
CREATE INDEX idx_blog_author ON Blog(author_id);
CREATE INDEX idx_blog_status_published ON Blog(status, published_at);
CREATE INDEX idx_comment_blog ON CommentBlog(blog_id);

-- Feedback indexes
CREATE INDEX idx_feedback_service_appointment ON FeedbackService(appointment_id);
CREATE INDEX idx_feedback_doctor_appointment ON FeedbackDoctor(appointment_id);

-- Payment indexes
CREATE INDEX idx_checkout_appointment ON CheckOut(appointment_id);
CREATE INDEX idx_checkout_status ON CheckOut(transaction_status);

-- Audit indexes
CREATE INDEX idx_audit_account ON AuditLogs(account_id);
CREATE INDEX idx_audit_timestamp ON AuditLogs(timestamp);
CREATE INDEX idx_audit_resource ON AuditLogs(resource, resource_id);