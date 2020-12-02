-- Generate Database for Project 0

DROP TABLE IF EXISTS Location
CREATE TABLE Location (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(99) NOT NULL,
	Address NVARCHAR(99) NOT NULL,
	City NVARCHAR(99) NOT NULL,
	State NCHAR(2),
	Country NVARCHAR(99) NOT NULL,
	PostalCode NVARCHAR(99),
	Phone NVARCHAR(99),
)

DROP TABLE IF EXISTS Customer
CREATE TABLE Customer (
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(99) NOT NULL,
	LastName NVARCHAR(99) NOT NULL,
	Email NVARCHAR(99) UNIQUE NOT NULL,
	Class INT NOT NULL DEFAULT 1
)

DROP TABLE IF EXISTS Product
CREATE TABLE Product (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(99) NOT NULL
)

DROP TABLE IF EXISTS LocationInventory
CREATE TABLE LocationInventory (
	LocationId INT NOT NULL FOREIGN KEY REFERENCES Location (Id),
	ProductId INT NOT NULL FOREIGN KEY REFERENCES Product (Id),
	Price MONEY NOT NULL CHECK (Price > 0),
	Stock INT NOT NULL CHECK (Stock >= 0),
	PRIMARY KEY (LocationId, ProductId)
)

DROP TABLE IF EXISTS [Order]
CREATE TABLE [Order] (
	Id INT PRIMARY KEY IDENTITY,
	CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customer (Id),
	LocationId INT NOT NULL FOREIGN KEY REFERENCES Location (Id),
	[Date] DATETIME NOT NULL
)

DROP TABLE IF EXISTS OrderContents
CREATE TABLE OrderContents (
	OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order] (Id),
	ProductId INT NOT NULL FOREIGN KEY REFERENCES Product (Id),
	Quantity INT NOT NULL CHECK (Quantity > 0),
	Price MONEY NOT NULL,
	PRIMARY KEY (OrderId, ProductId)
)

-- Generate Dummy Data

INSERT INTO Location (Name, Address, City, State, Country, PostalCode, Phone) VALUES
	('Walmart Neighborhood Market', '5175 Brookberry Park Ave', 'Winston-Salem', 'NC', 'United States', '27104', '(336)245-3007'),
	('Walmart Supercenter', '4550 Kester Mill Rd', 'Winston-Salem', 'NC', 'United States', '27103', '(336) 760-9868'),
	('Walmart Neighborhood Market', '180 Harvey St', 'Winston-Salem', 'NC', 'United States', '27103', '(336)293-9331'),
	('Walmart Supercenter', '3475 Pkwy Village Ct', 'Winston-Salem', 'NC', 'United States', '27127', '(336)771-1011');
INSERT INTO Customer (FirstName, LastName, Email, Class) VALUES
	('Matt', 'Goodman', 'matthew.goodman@revature.net', 2),
	('Nick', 'Escalona', 'nick.escalona@revature.net', 1)
INSERT INTO Product (Name) VALUES
	('Watermelon'),
	('Strawberries'),
	('Milk'),
	('Frozen Pizza')
INSERT INTO LocationInventory (LocationId, ProductId, Price, Stock) VALUES
	((SELECT Id FROM Location WHERE Address='180 Harvey St'), (SELECT Id FROM Product WHERE Name='Watermelon'), 5, 200),
	((SELECT Id FROM Location WHERE Address='180 Harvey St'), (SELECT Id FROM Product WHERE Name='Strawberries'), 3, 400),
	((SELECT Id FROM Location WHERE Address='3475 Pkwy Village Ct'), (SELECT Id FROM Product WHERE Name='Watermelon'), 5, 200),
	((SELECT Id FROM Location WHERE Address='3475 Pkwy Village Ct'), (SELECT Id FROM Product WHERE Name='Frozen Pizza'), 6, 30),
	((SELECT Id FROM Location WHERE Address='5175 Brookberry Park Ave'), (SELECT Id FROM Product WHERE Name='Milk'), 3, 40),
	((SELECT Id FROM Location WHERE Address='5175 Brookberry Park Ave'), (SELECT Id FROM Product WHERE Name='Strawberries'), 2, 300),
	((SELECT Id FROM Location WHERE Address='4550 Kester Mill Rd'), (SELECT Id FROM Product WHERE Name='Milk'), 4, 60),
	((SELECT Id FROM Location WHERE Address='4550 Kester Mill Rd'), (SELECT Id FROM Product WHERE Name='Frozen Pizza'), 5, 20)
INSERT INTO [Order] (CustomerId, LocationId, [Date]) VALUES
	((SELECT Id FROM Customer WHERE Email='matthew.goodman@revature.net'), (SELECT Id FROM Location WHERE Address='4550 Kester Mill Rd'), SYSDATETIME()),
	((SELECT Id FROM Customer WHERE Email='nick.escalona@revature.net'), (SELECT Id FROM Location WHERE Address='5175 Brookberry Park Ave'), SYSDATETIME())


DECLARE @maxid INT
SELECT @maxid = MAX(Id) FROM [Order]
DECLARE @prodid INT
SELECT @prodid = Id FROM Product WHERE Name='Frozen Pizza'
INSERT INTO OrderContents (OrderId, ProductId, Quantity, Price) VALUES
	(1,	4, 1, (SELECT Price FROM LocationInventory li WHERE li.ProductId=4 AND li.LocationId=(SELECT LocationId FROM [Order] WHERE Id=1))),
	(2,	2, 2, (SELECT Price FROM LocationInventory li WHERE li.ProductId=2 AND li.LocationId=(SELECT LocationId FROM [Order] WHERE Id=2)))
