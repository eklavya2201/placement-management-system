-- Demo data for presentations. Run AFTER schema.sql.
-- WARNING: this first DELETES everything in the 4 tables, then inserts fresh demo rows.

USE placement_db;

DELETE FROM applications;
DELETE FROM job_drives;
DELETE FROM companies;
DELETE FROM students;
ALTER TABLE applications AUTO_INCREMENT = 1;
ALTER TABLE job_drives AUTO_INCREMENT = 1;
ALTER TABLE companies AUTO_INCREMENT = 1;
ALTER TABLE students AUTO_INCREMENT = 1;

INSERT INTO students (name, branch, graduation_year, cgpa, email) VALUES
('Aarav Mehta',     'Computer Science',       2026, 9.12, 'aarav.mehta@college.edu'),
('Ananya Iyer',     'Computer Science',       2026, 8.74, 'ananya.iyer@college.edu'),
('Rohan Verma',     'Information Technology', 2026, 7.95, 'rohan.verma@college.edu'),
('Priya Nair',      'Electronics & Comm.',    2026, 8.31, 'priya.nair@college.edu'),
('Karan Singh',     'Mechanical',             2026, 6.88, 'karan.singh@college.edu'),
('Sneha Kulkarni',  'Computer Science',       2026, 9.40, 'sneha.kulkarni@college.edu'),
('Vikram Rao',      'Electrical',             2026, 7.20, 'vikram.rao@college.edu'),
('Isha Patel',      'Information Technology', 2026, 8.05, 'isha.patel@college.edu'),
('Aditya Joshi',    'Civil',                  2026, 6.45, 'aditya.joshi@college.edu'),
('Meera Pillai',    'Electronics & Comm.',    2026, 7.68, 'meera.pillai@college.edu'),
('Arjun Reddy',     'Computer Science',       2027, 8.92, 'arjun.reddy@college.edu'),
('Diya Sharma',     'Information Technology', 2027, 7.40, 'diya.sharma@college.edu');

INSERT INTO companies (name, sector, contact_email) VALUES
('Infosys',            'IT Services',      'campus@infosys.com'),
('Tata Consultancy',   'IT Services',      'hiring@tcs.com'),
('Amazon',             'E-commerce',       'university@amazon.com'),
('Larsen & Toubro',    'Engineering',      'careers@larsentoubro.com'),
('Deloitte',           'Consulting',       'campus@deloitte.com'),
('Texas Instruments',  'Semiconductors',   'india.campus@ti.com');

INSERT INTO job_drives (company_id, role, package_lpa, min_cgpa, drive_date) VALUES
(1, 'Systems Engineer',        6.5,  6.50, '2026-08-05'),
(2, 'Assistant System Eng.',   7.0,  6.00, '2026-08-12'),
(3, 'SDE I',                  32.0,  8.50, '2026-08-20'),
(4, 'Graduate Engineer',       8.0,  6.50, '2026-09-02'),
(5, 'Business Analyst',       10.5,  7.50, '2026-09-10'),
(6, 'Analog Design Engineer', 22.0,  8.00, '2026-09-18');

INSERT INTO applications (student_id, job_drive_id, status) VALUES
(1, 3, 'Selected'),
(6, 3, 'Shortlisted'),
(2, 3, 'Rejected'),
(2, 5, 'Selected'),
(4, 6, 'Selected'),
(3, 1, 'Selected'),
(5, 4, 'Shortlisted'),
(7, 4, 'Applied'),
(8, 5, 'Shortlisted'),
(10, 2, 'Selected'),
(9, 2, 'Applied'),
(12, 1, 'Applied'),
(11, 6, 'Applied');
