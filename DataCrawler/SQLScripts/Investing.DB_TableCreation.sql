USE [Investing]
GO

CREATE TABLE [dbo].[Exchange]
(
[Id] [uniqueidentifier] DEFAULT NEWID() NOT NULL,
[Name] [nchar](20) NOT NULL,
PRIMARY KEY (Id)
)

CREATE TABLE [dbo].[Sector]
(
[Id] [uniqueidentifier] DEFAULT NEWID() NOT NULL,
[Name] [nchar](255) NOT NULL,
PRIMARY KEY (Id)
)

CREATE TABLE [dbo].[Industry]
(
[Id] [uniqueidentifier] DEFAULT NEWID() NOT NULL,
[Name] [nchar](255) NOT NULL,
PRIMARY KEY (Id)
)

CREATE TABLE [dbo].[Stock]
(
[Id] [uniqueidentifier] DEFAULT NEWID() NOT NULL,
[ExchangeId] [uniqueidentifier] NOT NULL,
[CompanyName] [nvarchar](255) NOT NULL,
[Symbol] [nvarchar](25) NOT NULL,
[IPOYear] [nchar](4) NULL,
[SectorId] [uniqueidentifier] NULL,
[IndustryId] [uniqueidentifier] NULL,
[isActive] [bit] NOT NULL,
PRIMARY KEY (Id),
FOREIGN KEY (ExchangeId) REFERENCES [dbo].[Exchange](Id),
FOREIGN KEY (SectorId) REFERENCES [dbo].[Sector](Id),
FOREIGN KEY (IndustryId) REFERENCES [dbo].[Industry](Id)
)

CREATE TABLE [dbo].[CurrentStockSummary]
(
[Id] [uniqueidentifier] DEFAULT NEWID() NOT NULL,
[StockId] [uniqueidentifier] NOT NULL,
[SharePrice] [float] NOT NULL,
[SharesOutstanding] [float] NOT NULL,
[MarketCapitalization] [float] NOT NULL,
[EarningsPerShare] [float] NOT NULL,
[PriceToEarningsRatio] [float] NOT NULL,
PRIMARY KEY (Id),
FOREIGN KEY (StockId) REFERENCES [dbo].[Stock](Id)
)

CREATE TABLE [dbo].[Financial]
(
[Id] [uniqueidentifier] DEFAULT NEWID() NOT NULL,
[StockId] [uniqueidentifier] NOT NULL,
[DateReported] [datetime] NOT NULL,
[AssetAmount] [float] NOT NULL,
[LiabilityAmount] [float] NOT NULL,
[Equity] [float] NOT NULL,
[Revenue] [float] NOT NULL,
[Expenses] [float] NOT NULL,
[NetIncome] [float] NOT NULL,
[CashIn] [float] NOT NULL,
[CashOut] [float] NOT NULL,
[NetCash] [float] NOT NULL,
[EarningsPerShare] [float] NOT NULL,
[isAnnualReport] [bit] NOT NULL,
PRIMARY KEY (Id),
FOREIGN KEY (StockId) REFERENCES [dbo].[Stock](Id)
)