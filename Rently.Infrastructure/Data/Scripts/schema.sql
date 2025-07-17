
-- Use Db
USE RentlyDb

-- Drop existing tables (in dependency order)
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Units;
DROP TABLE IF EXISTS Properties;
DROP TABLE IF EXISTS Landlords;

-- Create Landlords table
CREATE TABLE Landlords (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    StreetAddress NVARCHAR(200),
    City NVARCHAR(100),
    StateOrProvince NVARCHAR(100),
    PostalCode NVARCHAR(20),
    Country NVARCHAR(100),
    IdentityUserId NVARCHAR(450), -- matches IdentityUser.Id type (string)
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT UQ_Landlords_Email UNIQUE (Email),
    CONSTRAINT FK_Landlords_Users FOREIGN KEY (IdentityUserId)
        REFERENCES AspNetUsers(Id)
        ON DELETE SET NULL
);


-- Create Properties table
CREATE TABLE Properties (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LandlordId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200),
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (LandlordId) REFERENCES Landlords(Id)
);

-- Create Units table
CREATE TABLE Units (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL,
    UnitNumber NVARCHAR(50) NOT NULL,         -- renamed from UnitCode to UnitNumber
    RentAmount DECIMAL(10, 2) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (PropertyId) REFERENCES Properties(Id),
    CONSTRAINT UQ_Units_UnitNumber_PropertyId UNIQUE (UnitNumber, PropertyId) -- Ensure uniqueness of unit number per property
);

-- Create Payments table
CREATE TABLE Payments (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UnitId UNIQUEIDENTIFIER NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    StripePaymentId NVARCHAR(100) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    PaymentDate DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UnitId) REFERENCES Units(Id)
);
