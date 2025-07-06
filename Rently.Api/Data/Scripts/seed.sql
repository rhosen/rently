-- Seed data with hard-coded GUIDs for relationships
INSERT INTO Landlords (Id, Name, Email, Phone)
VALUES 
('A1111111-1111-1111-1111-111111111111', 'Alice Johnson', 'rubel@gmail.com', '123-456-7890');

INSERT INTO Properties (Id, LandlordId, Name, Address, CreatedAt, IsDeleted, IsActive)
VALUES 
('B2222222-2222-2222-2222-222222222222', 'A1111111-1111-1111-1111-111111111111', 'Greenwood Apartments', '123 Main St, Springfield', GETUTCDATE(), 0, 1);

INSERT INTO Units (Id, PropertyId, UnitNumber, RentAmount, CreatedAt, IsDeleted, IsActive)
VALUES 
('C3333333-3333-3333-3333-333333333333', 'B2222222-2222-2222-2222-222222222222', 'GW-101', 1200.00, GETUTCDATE(), 0, 1),
('C4444444-4444-4444-4444-444444444444', 'B2222222-2222-2222-2222-222222222222', 'GW-102', 1500.00, GETUTCDATE(), 0, 1);

INSERT INTO Payments (Id, UnitId, Amount, StripePaymentId, Status, PaymentDate, CreatedAt)
VALUES 
(NEWID(), 'C3333333-3333-3333-3333-333333333333', 1200.00, 'TXN-001', 'Completed', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'C4444444-4444-4444-4444-444444444444', 1500.00, 'TXN-002', 'Completed', GETUTCDATE(), GETUTCDATE());
