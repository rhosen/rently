USE RentlyDb

-- Drop existing tables (in dependency order)
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Units;
DROP TABLE IF EXISTS Properties;
DROP TABLE IF EXISTS Landlords;

-- Create Landlords table
CREATE TABLE Landlords (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20)
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
