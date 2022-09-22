INSERT INTO Category([Name])
VALUES
('Category 1'),
('Category 2')

INSERT INTO Product([Name], Quantity, Price, CategoryId, CreatedDate, [Description], IsActive, [Type])
VALUES
('Product 1', 20, 15000, 1, GETDATE(), 'This is product 1', 1, 0),
('Product 2', 30, 16000, 1, GETDATE(), 'This is product 2', 1, 1),
('Product 3', 40, 17000, 2, GETDATE(), 'This is product 3', 1, 2)

