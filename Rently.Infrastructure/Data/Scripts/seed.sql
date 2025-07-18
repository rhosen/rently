USE RentlyDb

INSERT INTO Properties (Id, LandlordId, Name, Address, CreatedAt, IsDeleted, IsActive)
VALUES 
('3ED453D7-30BB-4B78-AE43-47DE1D60D34A', '3ED453D7-30BB-4B78-AE43-47DE1D60D34A', 'Greenwood Apartments', '123 Main St, Springfield', GETUTCDATE(), 0, 1);

INSERT INTO Units (Id, PropertyId, UnitNumber, RentAmount, CreatedAt, IsDeleted, IsActive)
VALUES 
('C3333333-3333-3333-3333-333333333333', '3ED453D7-30BB-4B78-AE43-47DE1D60D34A', 'GW-101', 1200.00, GETUTCDATE(), 0, 1),
('C4444444-4444-4444-4444-444444444444', '3ED453D7-30BB-4B78-AE43-47DE1D60D34A', 'GW-102', 1500.00, GETUTCDATE(), 0, 1);

INSERT INTO Payments (Id, UnitId, Amount, StripePaymentId, Status, PaymentDate, CreatedAt)
VALUES 
(NEWID(), 'C3333333-3333-3333-3333-333333333333', 1200.00, 'TXN-001', 'Completed', GETUTCDATE(), GETUTCDATE()),
(NEWID(), 'C4444444-4444-4444-4444-444444444444', 1500.00, 'TXN-002', 'Completed', GETUTCDATE(), GETUTCDATE());
