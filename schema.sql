-- Run this whole script once in MySQL Workbench (or `mysql -u root -p < schema.sql`)
-- before starting the app. It creates the database and all tables.

CREATE DATABASE IF NOT EXISTS placement_db;
USE placement_db;

CREATE TABLE IF NOT EXISTS students (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    branch VARCHAR(100) NOT NULL,
    graduation_year INT NOT NULL,
    cgpa DECIMAL(4,2) NOT NULL,
    email VARCHAR(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS companies (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    sector VARCHAR(100) NOT NULL,
    contact_email VARCHAR(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS job_drives (
    id INT AUTO_INCREMENT PRIMARY KEY,
    company_id INT NOT NULL,
    role VARCHAR(150) NOT NULL,
    package_lpa DECIMAL(6,2) NOT NULL,
    min_cgpa DECIMAL(4,2) NOT NULL,
    drive_date DATE NOT NULL,
    FOREIGN KEY (company_id) REFERENCES companies(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS applications (
    id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT NOT NULL,
    job_drive_id INT NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'Applied',
    FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE CASCADE,
    FOREIGN KEY (job_drive_id) REFERENCES job_drives(id) ON DELETE CASCADE,
    UNIQUE KEY unique_application (student_id, job_drive_id)
);
