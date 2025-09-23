
-- Connect to the new database
\c visioncare;

-- Drop all tables if they exist (with CASCADE to handle foreign keys)
DROP TABLE IF EXISTS CommentBlog CASCADE;
DROP TABLE IF EXISTS Token_User CASCADE;
DROP TABLE IF EXISTS Token_Google CASCADE;
DROP TABLE IF EXISTS OTP_Services CASCADE;
DROP TABLE IF EXISTS Machine CASCADE;
DROP TABLE IF EXISTS Banner CASCADE;
DROP TABLE IF EXISTS Content_Stories CASCADE;
DROP TABLE IF EXISTS Images_Video CASCADE;
DROP TABLE IF EXISTS Images_Service CASCADE;
DROP TABLE IF EXISTS Images_Type CASCADE;
DROP TABLE IF EXISTS Blog CASCADE;
DROP TABLE IF EXISTS CheckOut CASCADE;
DROP TABLE IF EXISTS Feedback_Doctor CASCADE;
DROP TABLE IF EXISTS Feedback_Service CASCADE;
DROP TABLE IF EXISTS Follow_Up CASCADE;
DROP TABLE IF EXISTS MedicalHistory CASCADE;
DROP TABLE IF EXISTS Appointment CASCADE;
DROP TABLE IF EXISTS Staff CASCADE;
DROP TABLE IF EXISTS Schedules CASCADE;
DROP TABLE IF EXISTS Slots CASCADE;
DROP TABLE IF EXISTS Services_Detail CASCADE;
DROP TABLE IF EXISTS Services CASCADE;
DROP TABLE IF EXISTS Services_Type CASCADE;
DROP TABLE IF EXISTS Customers CASCADE;
DROP TABLE IF EXISTS Discount CASCADE;
DROP TABLE IF EXISTS CustomerRank CASCADE;
DROP TABLE IF EXISTS Certificate_Doctor CASCADE;
DROP TABLE IF EXISTS Certificate CASCADE;
DROP TABLE IF EXISTS Degree_Doctor CASCADE;
DROP TABLE IF EXISTS Degree CASCADE;
DROP TABLE IF EXISTS Doctors CASCADE;
DROP TABLE IF EXISTS Accounts CASCADE;
DROP TABLE IF EXISTS Permission_Role CASCADE;
DROP TABLE IF EXISTS Specialization CASCADE;
DROP TABLE IF EXISTS Permission CASCADE;
DROP TABLE IF EXISTS Role CASCADE;

-- Create tables

CREATE TABLE Role (
    role_id SERIAL PRIMARY KEY,
    role_name VARCHAR(255) NOT NULL
);

CREATE TABLE Permission (
    permission_id SERIAL PRIMARY KEY,
    permission_name VARCHAR(255) NOT NULL
);

CREATE TABLE Specialization(
    specialization_id SERIAL PRIMARY KEY,
    specialization_name VARCHAR(255),
    specialization_status VARCHAR(255)
);

CREATE TABLE Permission_Role (
    permission_id INTEGER NOT NULL,
    role_id INTEGER NOT NULL,
    PRIMARY KEY (permission_id, role_id),
    FOREIGN KEY (permission_id) REFERENCES Permission(permission_id),
    FOREIGN KEY (role_id) REFERENCES Role(role_id)
);

CREATE TABLE Accounts (
    account_id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255),
    email VARCHAR(255),
    phone_number VARCHAR(50),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    role_id INTEGER,
    google_id VARCHAR(255),
    facebook_id VARCHAR(255),
    first_confirm VARCHAR(30),
    status_account VARCHAR(10),
    FOREIGN KEY (role_id) REFERENCES Role(role_id)
);

CREATE TABLE Doctors (
    doctor_id SERIAL PRIMARY KEY,
    account_id INTEGER UNIQUE,
    doctor_name VARCHAR(255) NOT NULL,
    experience_years INTEGER,
    specialization_id INTEGER,
    profile_image VARCHAR(255),
    rating FLOAT,
    gender VARCHAR(50),
    dob DATE,
    address VARCHAR(500),
    doctor_status VARCHAR(255),
    FOREIGN KEY(account_id) REFERENCES Accounts(account_id),
    FOREIGN KEY(specialization_id) REFERENCES Specialization(specialization_id)
);

CREATE TABLE Degree(
    degree_id SERIAL PRIMARY KEY,
    degree_name VARCHAR(255)
);

CREATE TABLE Degree_Doctor(
    degree_doctor_id SERIAL PRIMARY KEY,
    doctor_id INTEGER,
    degree_id INTEGER,
    degree_image VARCHAR(255),
    date_degree TIMESTAMP,
    date_change TIMESTAMP,
    status VARCHAR(255),
    issued_by VARCHAR(255),
    version INTEGER DEFAULT 1,
    FOREIGN KEY (doctor_id) REFERENCES Doctors(doctor_id),
    FOREIGN KEY (degree_id) REFERENCES Degree(degree_id)
);

CREATE TABLE Certificate(
    certificate_id SERIAL PRIMARY KEY,
    certificate_name VARCHAR(255)
);

CREATE TABLE Certificate_Doctor(
    certificate_id INTEGER,
    doctor_id INTEGER,
    date_certificate TIMESTAMP,
    date_change TIMESTAMP,
    status VARCHAR(255),
    issued_by VARCHAR(255),
    certificate_image VARCHAR(255),
    PRIMARY KEY (doctor_id, certificate_id),
    FOREIGN KEY (doctor_id) REFERENCES Doctors(doctor_id),
    FOREIGN KEY (certificate_id) REFERENCES Certificate(certificate_id)
);

CREATE TABLE CustomerRank(
    rankId SERIAL PRIMARY KEY,
    rankName VARCHAR(50),
    minAmount FLOAT
);

CREATE TABLE Discount(
    discountId SERIAL PRIMARY KEY,
    discountName VARCHAR(50),
    percent INTEGER,
    rankId INTEGER REFERENCES CustomerRank(rankId),
    endDate DATE,
    status BOOLEAN DEFAULT TRUE
);

CREATE TABLE Customers (
    account_id INTEGER PRIMARY KEY,
    full_name VARCHAR(255) NOT NULL,
    address VARCHAR(500),
    dob DATE,
    gender VARCHAR(50),
    rankId INTEGER,
    image_profile_user VARCHAR(255),
    FOREIGN KEY (rankId) REFERENCES CustomerRank(rankId),
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);

CREATE TABLE Services_Type(
    service_type_id SERIAL PRIMARY KEY,
    service_type_name VARCHAR(255),
    duration_service VARCHAR(50)
);

CREATE TABLE Services(
    service_id SERIAL PRIMARY KEY,
    service_name VARCHAR(255),
    service_description VARCHAR(255),
    service_introduce VARCHAR(1000),
    service_benefit VARCHAR(1000),
    service_status VARCHAR(20),
    specialization_id INTEGER,
    FOREIGN KEY(specialization_id) REFERENCES Specialization(specialization_id)
);

CREATE TABLE Services_Detail (
    service_detail_id SERIAL PRIMARY KEY,
    service_type_id INTEGER,
    service_id INTEGER,
    cost DECIMAL(18,2) NOT NULL,
    FOREIGN KEY(service_type_id) REFERENCES Services_Type(service_type_id),
    FOREIGN KEY(service_id) REFERENCES Services(service_id)
);

CREATE TABLE Slots (
    slot_id SERIAL PRIMARY KEY,
    start_time VARCHAR(20) NOT NULL,
    end_time VARCHAR(255) NOT NULL,
    service_type_id INTEGER NOT NULL,
    FOREIGN KEY (service_type_id) REFERENCES Services_Type(service_type_id),
    UNIQUE (start_time, end_time, service_type_id)
);

CREATE TABLE Schedules (
    doctor_id INTEGER,
    slot_id INTEGER,
    schedule_date DATE NOT NULL,
    schedule_status VARCHAR(255),
    PRIMARY KEY (doctor_id, slot_id, schedule_date),
    FOREIGN KEY (doctor_id) REFERENCES Doctors(doctor_id),
    FOREIGN KEY (slot_id) REFERENCES Slots(slot_id)
);

CREATE TABLE Staff (
    staff_id SERIAL PRIMARY KEY,
    account_id INTEGER UNIQUE,
    admin_fullname VARCHAR(255),
    admin_address VARCHAR(255),
    admin_dob DATE,
    admin_gender VARCHAR(50),
    image_profile_admin VARCHAR(255),
    admin_hired_date TIMESTAMP,
    admin_salary DECIMAL(18,2),
    FOREIGN KEY(account_id) REFERENCES Accounts(account_id)
);

CREATE TABLE Appointment(
    appointment_id SERIAL PRIMARY KEY,
    appointment_date TIMESTAMP,
    appointment_status VARCHAR(255),
    doctor_id INTEGER,
    slot_id INTEGER,
    service_detail_id INTEGER,
    discountId INTEGER,
    actualCost DECIMAL(18,2),
    patient_id INTEGER,
    staff_id INTEGER,
    FOREIGN KEY (doctor_id) REFERENCES Doctors(doctor_id),
    FOREIGN KEY (slot_id) REFERENCES Slots(slot_id),
    FOREIGN KEY (service_detail_id) REFERENCES Services_Detail(service_detail_id),
    FOREIGN KEY (discountId) REFERENCES Discount(discountId),
    FOREIGN KEY (staff_id) REFERENCES Staff(staff_id),
    FOREIGN KEY (patient_id) REFERENCES Customers(account_id),
    UNIQUE(doctor_id, slot_id, appointment_date)
);

CREATE TABLE MedicalHistory (
    appointment_id INTEGER PRIMARY KEY,
    diagnosis VARCHAR(255),
    symptoms VARCHAR(255),
    treatment VARCHAR(255),
    prescription VARCHAR(255),
    vision_left DECIMAL(3,2),
    vision_right DECIMAL(3,2),
    additional_tests VARCHAR(255),
    note VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id)
);

CREATE TABLE Follow_Up (
    next_follow_up_date DATE,
    follow_up_description VARCHAR(255),
    PatientName VARCHAR(255),
    phone VARCHAR(20),
    email VARCHAR(255)
);

CREATE TABLE Feedback_Service(
    feedback_id SERIAL PRIMARY KEY,
    appointment_id INTEGER,
    feedback_text VARCHAR(255),
    feedback_date TIMESTAMP,
    feedback_rating INTEGER CHECK (feedback_rating BETWEEN 1 AND 5),
    response_text VARCHAR(255),
    response_date TIMESTAMP,
    staff_id INTEGER REFERENCES Staff(staff_id),
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id)
);

CREATE TABLE Feedback_Doctor(
    feedback_id SERIAL PRIMARY KEY,
    appointment_id INTEGER,
    feedback_text VARCHAR(255),
    feedback_date TIMESTAMP,
    feedback_rating INTEGER CHECK (feedback_rating BETWEEN 1 AND 5),
    response_text VARCHAR(255),
    response_date TIMESTAMP,
    staff_id INTEGER REFERENCES Staff(staff_id),
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id)
);

CREATE TABLE CheckOut(
    checkout_id SERIAL PRIMARY KEY,
    appointment_id INTEGER,
    transaction_type VARCHAR(255),
    transaction_status VARCHAR(255),
    total_bill DECIMAL(18,2),
    checkout_code VARCHAR(255),
    checkout_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY(appointment_id) REFERENCES Appointment(appointment_id)
);

CREATE TABLE Blog(
    blog_id SERIAL PRIMARY KEY,
    blog_content TEXT,
    author_id INTEGER,
    created_date_blog TIMESTAMP,
    title_meta TEXT,
    title_image_blog VARCHAR(255),
    FOREIGN KEY (author_id) REFERENCES Accounts(account_id)
);

CREATE TABLE Images_Type(
    image_type_id SERIAL PRIMARY KEY,
    image_type VARCHAR(255)
);

CREATE TABLE Images_Service(
    service_id INTEGER PRIMARY KEY,
    image_main VARCHAR(255),
    image_before VARCHAR(255),
    image_after VARCHAR(255),
    FOREIGN KEY(service_id) REFERENCES Services(service_id)
);

CREATE TABLE Images_Video(
    image_id SERIAL PRIMARY KEY,
    image_url VARCHAR(255),
    image_description VARCHAR(255),
    image_type_id INTEGER,
    FOREIGN KEY(image_type_id) REFERENCES Images_Type(image_type_id)
);

CREATE TABLE Content_Stories(
    patient_name VARCHAR(255) PRIMARY KEY,
    image_patient VARCHAR(255),
    content_stories TEXT
);

CREATE TABLE Banner(
    banner_id SERIAL PRIMARY KEY,
    banner_name VARCHAR(255),
    banner_title VARCHAR(255),
    banner_description VARCHAR(255),
    banner_status VARCHAR(10),
    link_banner VARCHAR(255),
    href_banner VARCHAR(255)
);

CREATE TABLE Machine(
    machine_id SERIAL PRIMARY KEY,
    machine_name VARCHAR(255),
    machine_description TEXT,
    machine_img VARCHAR(255)
);

CREATE TABLE OTP_Services(
    otp_id SERIAL PRIMARY KEY,
    account_id INTEGER,
    otp VARCHAR(20),
    created_otp_time VARCHAR(255),
    otp_expiry_date VARCHAR(255),
    FOREIGN KEY (account_id) REFERENCES Accounts(account_id)
);

CREATE TABLE Token_Google(
    token_id SERIAL PRIMARY KEY,
    token VARCHAR(255),
    created_date TIMESTAMP
);

CREATE TABLE Token_User(
    token_id SERIAL PRIMARY KEY,
    token_user VARCHAR(255),
    account_id INTEGER,
    created_date_token TIMESTAMP,
    FOREIGN KEY(account_id) REFERENCES Accounts(account_id)
);

CREATE TABLE CommentBlog(
    comment_blog_id SERIAL PRIMARY KEY,
    comment VARCHAR(255),
    author_id INTEGER,
    tuongtac INTEGER,
    parent_comment_id INTEGER,
    blog_id INTEGER,
    FOREIGN KEY(blog_id) REFERENCES Blog(blog_id),
    FOREIGN KEY(author_id) REFERENCES Accounts(account_id)
);

-- Create indexes for better performance (optional)
CREATE INDEX idx_accounts_username ON Accounts(username);
CREATE INDEX idx_accounts_email ON Accounts(email);
CREATE INDEX idx_doctors_specialization ON Doctors(specialization_id);
CREATE INDEX idx_appointments_date ON Appointment(appointment_date);
CREATE INDEX idx_appointments_doctor ON Appointment(doctor_id);
CREATE INDEX idx_appointments_patient ON Appointment(patient_id);