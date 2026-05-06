-- FAST Societies Management System - Database Schema
-- Database: FASTSocietiesSystemDB
-- Purpose: Centralized platform for managing student societies at the university

-- ============================================================================
-- 1. CREATE OR DROP DATABASE
-- ============================================================================

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'FASTSocietiesSystemDB')
BEGIN
    ALTER DATABASE FASTSocietiesSystemDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FASTSocietiesSystemDB;
END

CREATE DATABASE FASTSocietiesSystemDB;
GO

USE FASTSocietiesSystemDB;
GO

-- ============================================================================
-- 2. DROP EXISTING TABLES (Careful with production data!)
-- ============================================================================

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ApprovalRequest') DROP TABLE [ApprovalRequest];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'EventRegistration') DROP TABLE [EventRegistration];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Task') DROP TABLE [Task];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Event') DROP TABLE [Event];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Announcement') DROP TABLE [Announcement];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Membership') DROP TABLE [Membership];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Society') DROP TABLE [Society];
IF EXISTS (SELECT * FROM sys.tables WHERE name = '[User]') DROP TABLE [User];
GO

-- ============================================================================
-- 3. CREATE CORE TABLES
-- ============================================================================
GO

-- USER TABLE (Base entity for all users: Students, Society Heads, Admins)
CREATE TABLE [User] (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Role NVARCHAR(20) NOT NULL 
        CHECK (Role IN ('Student', 'SocietyHead', 'Admin')),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active'
        CHECK (Status IN ('Active', 'Inactive', 'Suspended')),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE()
);

CREATE INDEX IX_User_Email ON [User](Email);
CREATE INDEX IX_User_Role ON [User](Role);
CREATE INDEX IX_User_Status ON [User](Status);
GO

-- ============================================================================
-- SOCIETY TABLE
-- ============================================================================
GO

CREATE TABLE [Society] (
    SocietyId INT PRIMARY KEY IDENTITY(1,1),
    SocietyName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    HeadId INT NOT NULL,
    Logo NVARCHAR(255),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending'
        CHECK (Status IN ('Pending', 'Approved', 'Active', 'Suspended', 'Inactive')),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Society_Head FOREIGN KEY (HeadId) REFERENCES [User](UserId)
);

CREATE INDEX IX_Society_HeadId ON [Society](HeadId);
CREATE INDEX IX_Society_Status ON [Society](Status);
GO

-- ============================================================================
-- MEMBERSHIP TABLE (Student - Society many-to-many relationship)
-- ============================================================================
GO

CREATE TABLE [Membership] (
    MembershipId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    SocietyId INT NOT NULL,
    JoinDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending'
        CHECK (Status IN ('Pending', 'Approved', 'Active', 'Rejected', 'Left')),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Membership_Student FOREIGN KEY (StudentId) REFERENCES [User](UserId),
    CONSTRAINT FK_Membership_Society FOREIGN KEY (SocietyId) REFERENCES [Society](SocietyId),
    CONSTRAINT UQ_Membership_StudentSociety UNIQUE (StudentId, SocietyId)
);

CREATE INDEX IX_Membership_StudentId ON [Membership](StudentId);
CREATE INDEX IX_Membership_SocietyId ON [Membership](SocietyId);
CREATE INDEX IX_Membership_Status ON [Membership](Status);
GO

-- ============================================================================
-- EVENT TABLE
-- ============================================================================
GO

CREATE TABLE [Event] (
    EventId INT PRIMARY KEY IDENTITY(1,1),
    SocietyId INT NOT NULL,
    EventTitle NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500),
    EventDate DATE NOT NULL,
    EventTime TIME,
    Location NVARCHAR(200),
    Capacity INT,
    RegistrationDeadline DATE,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending'
        CHECK (Status IN ('Pending', 'Approved', 'Scheduled', 'InProgress', 'Completed', 'Cancelled')),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Event_Society FOREIGN KEY (SocietyId) REFERENCES [Society](SocietyId)
);

CREATE INDEX IX_Event_SocietyId ON [Event](SocietyId);
CREATE INDEX IX_Event_EventDate ON [Event](EventDate);
CREATE INDEX IX_Event_Status ON [Event](Status);
GO

-- ============================================================================
-- EVENT REGISTRATION TABLE (Student Event Registrations with Tickets)
-- ============================================================================
GO

CREATE TABLE [EventRegistration] (
    RegistrationId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    EventId INT NOT NULL,
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE(),
    TicketId NVARCHAR(50) UNIQUE,
    AttendanceStatus NVARCHAR(20) DEFAULT 'Registered'
        CHECK (AttendanceStatus IN ('Registered', 'CheckedIn', 'Absent', 'Cancelled')),
    CheckInDate DATETIME,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_EventRegistration_Student FOREIGN KEY (StudentId) REFERENCES [User](UserId),
    CONSTRAINT FK_EventRegistration_Event FOREIGN KEY (EventId) REFERENCES [Event](EventId),
    CONSTRAINT UQ_EventRegistration_StudentEvent UNIQUE (StudentId, EventId)
);

CREATE INDEX IX_EventRegistration_StudentId ON [EventRegistration](StudentId);
CREATE INDEX IX_EventRegistration_EventId ON [EventRegistration](EventId);
CREATE INDEX IX_EventRegistration_TicketId ON [EventRegistration](TicketId);
GO

-- ============================================================================
-- TASK TABLE (Tasks assigned by societies to members)
-- ============================================================================
GO

CREATE TABLE [Task] (
    TaskId INT PRIMARY KEY IDENTITY(1,1),
    SocietyId INT NOT NULL,
    CompletedBy INT,
    TaskTitle NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500),
    DueDate DATE NOT NULL,
    AssignedDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending'
        CHECK (Status IN ('Pending', 'InProgress', 'Completed', 'Overdue', 'Cancelled')),
    Priority NVARCHAR(20) DEFAULT 'Medium'
        CHECK (Priority IN ('Low', 'Medium', 'High')),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Task_Society FOREIGN KEY (SocietyId) REFERENCES [Society](SocietyId),
    CONSTRAINT FK_Task_CompletedBy FOREIGN KEY (CompletedBy) REFERENCES [User](UserId)
);

CREATE INDEX IX_Task_SocietyId ON [Task](SocietyId);
CREATE INDEX IX_Task_CompletedBy ON [Task](CompletedBy);
CREATE INDEX IX_Task_Status ON [Task](Status);
CREATE INDEX IX_Task_DueDate ON [Task](DueDate);
GO

-- ============================================================================
-- ANNOUNCEMENT TABLE (Announcements and posts by societies)
-- ============================================================================
GO

CREATE TABLE [Announcement] (
    AnnouncementId INT PRIMARY KEY IDENTITY(1,1),
    SocietyId INT NOT NULL,
    Title NVARCHAR(150) NOT NULL,
    Content NVARCHAR(1000) NOT NULL,
    CreatedBy INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    CONSTRAINT FK_Announcement_Society FOREIGN KEY (SocietyId) REFERENCES [Society](SocietyId),
    CONSTRAINT FK_Announcement_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [User](UserId)
);

CREATE INDEX IX_Announcement_SocietyId ON [Announcement](SocietyId);
CREATE INDEX IX_Announcement_CreatedDate ON [Announcement](CreatedDate);
GO

-- ============================================================================
-- APPROVAL REQUEST TABLE (For event and society approvals)
-- ============================================================================
GO

CREATE TABLE [ApprovalRequest] (
    ApprovalId INT PRIMARY KEY IDENTITY(1,1),
    RequestType NVARCHAR(20) NOT NULL
        CHECK (RequestType IN ('Event', 'Society', 'Membership')),
    RequesterId INT NOT NULL,
    TargetId INT NOT NULL,
    Description NVARCHAR(500),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending'
        CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    ApprovedBy INT,
    ApprovalDate DATETIME,
    RejectionReason NVARCHAR(500),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_ApprovalRequest_Requester FOREIGN KEY (RequesterId) REFERENCES [User](UserId),
    CONSTRAINT FK_ApprovalRequest_ApprovedBy FOREIGN KEY (ApprovedBy) REFERENCES [User](UserId)
);

CREATE INDEX IX_ApprovalRequest_RequestType ON [ApprovalRequest](RequestType);
CREATE INDEX IX_ApprovalRequest_Status ON [ApprovalRequest](Status);
CREATE INDEX IX_ApprovalRequest_RequesterId ON [ApprovalRequest](RequesterId);
GO

-- ============================================================================
-- 4. CREATE VIEWS FOR COMMON QUERIES
-- ============================================================================
GO

-- View: Active Societies
CREATE VIEW vw_ActiveSocieties AS
SELECT 
    s.SocietyId,
    s.SocietyName,
    s.Description,
    s.HeadId,
    u.FirstName + ' ' + u.LastName AS HeadName,
    s.Status,
    s.CreatedDate,
    (SELECT COUNT(*) FROM Membership WHERE SocietyId = s.SocietyId AND Status = 'Active') AS MemberCount
FROM Society s
INNER JOIN [User] u ON s.HeadId = u.UserId
WHERE s.Status IN ('Active', 'Approved');
GO

-- View: Upcoming Events
CREATE VIEW vw_UpcomingEvents AS
SELECT 
    e.EventId,
    e.EventTitle,
    e.Description,
    e.EventDate,
    e.EventTime,
    e.Location,
    e.Capacity,
    s.SocietyName,
    s.SocietyId,
    (SELECT COUNT(*) FROM EventRegistration WHERE EventId = e.EventId) AS RegisteredCount,
    e.Status
FROM Event e
INNER JOIN Society s ON e.SocietyId = s.SocietyId
WHERE e.EventDate >= CAST(GETDATE() AS DATE) 
  AND e.Status IN ('Approved', 'Scheduled');
GO

-- View: Student Memberships
CREATE VIEW vw_StudentMemberships AS
SELECT 
    m.MembershipId,
    m.StudentId,
    s.SocietyId,
    s.SocietyName,
    m.JoinDate,
    m.Status,
    (SELECT COUNT(*) FROM Event WHERE SocietyId = s.SocietyId AND EventDate >= CAST(GETDATE() AS DATE)) AS UpcomingEventsCount
FROM Membership m
INNER JOIN Society s ON m.SocietyId = s.SocietyId
WHERE m.Status IN ('Active', 'Approved');
GO

-- View: Society Member Details
CREATE VIEW vw_SocietyMembers AS
SELECT 
    m.MembershipId,
    m.SocietyId,
    s.SocietyName,
    m.StudentId,
    u.Email,
    u.FirstName + ' ' + u.LastName AS StudentName,
    u.PhoneNumber,
    m.JoinDate,
    m.Status
FROM Membership m
INNER JOIN Society s ON m.SocietyId = s.SocietyId
INNER JOIN [User] u ON m.StudentId = u.UserId
WHERE m.Status IN ('Active', 'Approved');
GO

-- ============================================================================
-- 5. SEED DATA FOR TESTING
-- ============================================================================
GO

-- Insert test users
INSERT INTO [User] (Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, Status)
VALUES 
    ('student1@university.edu', 'hashed_password_1', 'Ali', 'Khan', '92-3001234567', 'Student', 'Active'),
    ('student2@university.edu', 'hashed_password_2', 'Fatima', 'Ahmed', '92-3101234567', 'Student', 'Active'),
    ('student3@university.edu', 'hashed_password_3', 'Hassan', 'Ali', '92-3201234567', 'Student', 'Active'),
    ('head1@university.edu', 'hashed_password_4', 'Zara', 'Khan', '92-3301234567', 'SocietyHead', 'Active'),
    ('head2@university.edu', 'hashed_password_5', 'Omar', 'Hassan', '92-3401234567', 'SocietyHead', 'Active'),
    ('admin1@university.edu', 'hashed_password_6', 'Admin', 'User', '92-3501234567', 'Admin', 'Active');

-- Insert test societies
INSERT INTO [Society] (SocietyName, Description, HeadId, Status)
VALUES 
    ('Gaming Society', 'A society for gaming enthusiasts and esports', 4, 'Active'),
    ('Developers Club', 'For software developers and programmers', 5, 'Active'),
    ('Sports Society', 'All sports activities and competitions', 4, 'Pending');

-- Insert test memberships
INSERT INTO [Membership] (StudentId, SocietyId, Status)
VALUES 
    (1, 1, 'Active'),
    (1, 2, 'Active'),
    (2, 1, 'Active'),
    (3, 2, 'Pending'),
    (2, 3, 'Active');

-- Insert test events
INSERT INTO [Event] (SocietyId, EventTitle, Description, EventDate, EventTime, Location, Capacity, RegistrationDeadline, Status)
VALUES 
    (1, 'Gaming Tournament 2026', 'Annual gaming tournament - CS2 and Valorant', '2026-06-15', '14:00:00', 'Main Hall', 100, '2026-06-10', 'Approved'),
    (2, 'Coding Bootcamp', 'Intensive coding workshop for beginners', '2026-05-20', '10:00:00', 'Lab 101', 50, '2026-05-18', 'Approved'),
    (1, 'Gaming Movie Night', 'Watch gaming documentaries and trailers', '2026-05-25', '19:00:00', 'Auditorium', 150, '2026-05-23', 'Scheduled');

-- Insert test event registrations
INSERT INTO [EventRegistration] (StudentId, EventId, TicketId, AttendanceStatus)
VALUES 
    (1, 1, 'TICKET-001', 'Registered'),
    (2, 1, 'TICKET-002', 'Registered'),
    (1, 2, 'TICKET-003', 'Registered'),
    (3, 2, 'TICKET-004', 'Registered');

-- Insert test tasks
INSERT INTO [Task] (SocietyId, TaskTitle, Description, DueDate, Status, Priority)
VALUES 
    (1, 'Prepare Gaming Tournament Schedule', 'Create brackets and timeline for tournament', '2026-05-31', 'Pending', 'High'),
    (2, 'Update Website Content', 'Add new workshop details to club website', '2026-05-30', 'InProgress', 'Medium'),
    (1, 'Send Event Reminders', 'Email registered participants about Gaming Tournament', '2026-06-10', 'Pending', 'High');

-- Insert test announcements
INSERT INTO [Announcement] (SocietyId, Title, Content, CreatedBy)
VALUES 
    (1, 'Gaming Tournament Announcement', 'Annual gaming tournament happening June 15th. Register now!', 4),
    (2, 'Coding Bootcamp Starting', 'Join our intensive coding bootcamp starting May 20th', 5),
    (1, 'New Gaming Lounge Opened', 'Check out our new gaming setup in the Student Center', 4);

-- ============================================================================
-- 6. DATABASE VERIFICATION QUERIES
-- ============================================================================
GO

-- Verify all tables were created
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;
GO

-- Count records in each table
PRINT '=== Data Summary ===';
GO

DECLARE @UserCount INT = (SELECT COUNT(*) FROM [User]);
DECLARE @SocietyCount INT = (SELECT COUNT(*) FROM [Society]);
DECLARE @MembershipCount INT = (SELECT COUNT(*) FROM [Membership]);
DECLARE @EventCount INT = (SELECT COUNT(*) FROM [Event]);
DECLARE @EventRegCount INT = (SELECT COUNT(*) FROM [EventRegistration]);
DECLARE @TaskCount INT = (SELECT COUNT(*) FROM [Task]);
DECLARE @AnnouncementCount INT = (SELECT COUNT(*) FROM [Announcement]);

PRINT 'Users: ' + CAST(@UserCount AS VARCHAR(10));
PRINT 'Societies: ' + CAST(@SocietyCount AS VARCHAR(10));
PRINT 'Memberships: ' + CAST(@MembershipCount AS VARCHAR(10));
PRINT 'Events: ' + CAST(@EventCount AS VARCHAR(10));
PRINT 'Event Registrations: ' + CAST(@EventRegCount AS VARCHAR(10));
PRINT 'Tasks: ' + CAST(@TaskCount AS VARCHAR(10));
PRINT 'Announcements: ' + CAST(@AnnouncementCount AS VARCHAR(10));
GO

-- ============================================================================
-- END OF SCHEMA SETUP
-- ============================================================================
GO

PRINT 'Database FASTSocietiesSystemDB created successfully!';
PRINT 'All tables, views, and seed data loaded.';
GO
