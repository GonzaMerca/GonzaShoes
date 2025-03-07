CREATE DATABASE GonzaShoes;

USE GonzaShoes;

-- Tabla User
CREATE TABLE [dbo].[User] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);

INSERT INTO [User] (Name, Email, Password, CreatedUserId) VALUES ('Admin', 'admin@gonzashoes.com.ar', CONVERT(VARCHAR(40), HASHBYTES('SHA1', '12345'), 2), 0);

-- Tabla Brand
CREATE TABLE [dbo].[Brand] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);

-- Tabla Color
CREATE TABLE [dbo].[Color] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [HexCode] NVARCHAR(7) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);

-- Tabla Size
CREATE TABLE [dbo].[Size] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Number] DECIMAL(18,5) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);

-- Tabla Model
CREATE TABLE [dbo].[ModelProduct] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [BrandId] INT NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ModelProduct_Brand FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand]([Id])
);

-- Tabla Product
CREATE TABLE [dbo].[Product] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ModelProductId] INT NOT NULL,
    [BrandId] INT NOT NULL,
    [ColorId] INT NOT NULL,
    [SizeId] INT NOT NULL,
    [Price] DECIMAL(18,5) NOT NULL,
    [Stock] INT NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Product_ModelProduct FOREIGN KEY ([ModelProductId]) REFERENCES [dbo].[ModelProduct]([Id]),
    CONSTRAINT FK_Product_Brand FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand]([Id]),
    CONSTRAINT FK_Product_Color FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Color]([Id]),
    CONSTRAINT FK_Product_Size FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Size]([Id])
);

CREATE TABLE [dbo].[ProductStockFlow] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    ModelProductId INT NOT NULL,
    BrandId INT NOT NULL,
    ColorId INT NOT NULL,
    SizeId INT NOT NULL,
    OrderId INT NULL,
    Income DECIMAL(18,2) NOT NULL,
    Outcome DECIMAL(18,2) NOT NULL,
    RemainingStock DECIMAL(18,2) NOT NULL,
    OrderProductItemId INT NULL,
    Description NVARCHAR(255),
    Date DATETIME NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ProductStockFlow_Product FOREIGN KEY (ProductId) REFERENCES Product(Id),
    CONSTRAINT FK_ProductStockFlow_ModelProduct FOREIGN KEY (ModelProductId) REFERENCES ModelProduct(Id),
    CONSTRAINT FK_ProductStockFlow_Brand FOREIGN KEY (BrandId) REFERENCES Brand(Id),
    CONSTRAINT FK_ProductStockFlow_Color FOREIGN KEY (ColorId) REFERENCES Color(Id),
    CONSTRAINT FK_ProductStockFlow_Size FOREIGN KEY (SizeId) REFERENCES Size(Id),
    CONSTRAINT FK_ProductStockFlow_Order FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    CONSTRAINT FK_ProductStockFlow_OrderProductItem FOREIGN KEY (OrderProductItemId) REFERENCES OrderItem(Id)
);

-- Tabla Order
CREATE TABLE [dbo].[Order] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [DateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [Status] INT NOT NULL,
    [TotalWithNoDiscount] DECIMAL(18,5) NOT NULL,
    [Discount] DECIMAL(18,5) NOT NULL,
    [DiscountPercentage] DECIMAL(18,5) NOT NULL,
    [Total] DECIMAL(18,5) NOT NULL,
    [Observation] NVARCHAR(255) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);

-- Tabla OrderItem
CREATE TABLE [dbo].[OrderItem] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [OrderId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    [ProductName] NVARCHAR(255) NOT NULL,
    [BrandId] INT NOT NULL,
    [BrandName] NVARCHAR(255) NOT NULL,
    [ModelProductId] INT NOT NULL,
    [ModelProductName] NVARCHAR(255) NOT NULL,
    [ColorId] INT NOT NULL,
    [ColorName] NVARCHAR(255) NOT NULL,
    [SizeId] INT NOT NULL,
    [SizeNumber] DECIMAL(18,5) NOT NULL,
    [Quantity] DECIMAL(18,5) NOT NULL,
    [UnitPrice] DECIMAL(18,5) NOT NULL,
    [TotalWithNoDiscount] DECIMAL(18,5) NOT NULL,
    [Discount] DECIMAL(18,5) NOT NULL,
    [DiscountPercentage] DECIMAL(18,5) NOT NULL,
    [Total] DECIMAL(18,5) NOT NULL,
    [Observation] NVARCHAR(255) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order]([Id]),
    CONSTRAINT FK_OrderItem_Product FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([Id]),
    CONSTRAINT FK_OrderItem_Brand FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand]([Id]),
    CONSTRAINT FK_OrderItem_ModelProduct FOREIGN KEY ([ModelProductId]) REFERENCES [dbo].[ModelProduct]([Id]),
    CONSTRAINT FK_OrderItem_Color FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Color]([Id]),
    CONSTRAINT FK_OrderItem_Size FOREIGN KEY ([SizeId]) REFERENCES [dbo].[Size]([Id])
);

-- Tabla OrderPayment
CREATE TABLE [dbo].[OrderPayment] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [OrderId] INT NOT NULL,
    [Amount] DECIMAL(18,5) NOT NULL,
    [PayWith] DECIMAL(18,5) NOT NULL,
    [Cash] DECIMAL(18,5) NOT NULL,
    [DebitCard] DECIMAL(18,5) NOT NULL,
    [CreditCard] DECIMAL(18,5) NOT NULL,
    [Transfer] DECIMAL(18,5) NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT GETDATE(),
    [DateUpdated] DATETIME NULL,
    [CreatedUserId] INT NOT NULL,
    [UpdatedUserId] INT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_OrderPayment_Order FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order]([Id])
);
