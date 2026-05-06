-- FAST Societies Management System - Database Schema (Clean - No Seed Data)
-- Database: FASTSocietiesSystemDB
-- Purpose: Centralized platform for managing student societies at the university

USE master;
GO

-- ============================================================================
-- 1. DROP AND CREATE DATABASE
-- ============================================================================

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'FASTSocietiesSystemDB')
BEGIN
    DECLARE @killSessionsSql NVARCHAR(MAX) = N'';

    -- Kill all sessions currently connected to the target database.
    SELECT @killSessionsSql = @killSessionsSql + N'KILL ' + CAST(spid AS NVARCHAR(10)) + N';'
    FROM master..sysprocesses
    WHERE dbid = DB_ID('FASTSocietiesSystemDB')
      AND spid <> @@SPID;

    IF LEN(@killSessionsSql) > 0
    BEGIN
        EXEC sp_executesql @killSessionsSql;
    END

    -- Recover from stuck SINGLE_USER state and force disconnect all sessions.
    IF EXISTS (
        SELECT 1
        FROM sys.databases
        WHERE name = 'FASTSocietiesSystemDB'
          AND user_access_desc = 'SINGLE_USER'
    )
    BEGIN
        ALTER DATABASE FASTSocietiesSystemDB SET MULTI_USER WITH ROLLBACK IMMEDIATE;
    END

    ALTER DATABASE FASTSocietiesSystemDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FASTSocietiesSystemDB;
END

-- Fail fast if DB could not be dropped; do not continue with partial state.
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'FASTSocietiesSystemDB')
BEGIN
    RAISERROR('Could not drop FASTSocietiesSystemDB. Close all connections (SSMS Object Explorer/app), then rerun.', 16, 1);
    SET NOEXEC ON;
END

CREATE DATABASE FASTSocietiesSystemDB;
GO

USE FASTSocietiesSystemDB;
GO

-- ============================================================================
-- 2. CREATE CORE TABLES
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
-- 3. CREATE VIEWS FOR COMMON QUERIES
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
-- 4. DATABASE VERIFICATION
-- ============================================================================
GO

PRINT 'Database FASTSocietiesSystemDB created successfully!';
PRINT 'Schema complete - all tables and views created.';
PRINT 'Ready for application use.';
GO

SET NOEXEC OFF;
GO
