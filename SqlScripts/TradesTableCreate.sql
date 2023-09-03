USE [TylTrades]
GO

/****** Object:  Table [dbo].[Trades]    Script Date: 03/09/2023 20:13:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Trades](
	[TradeId] [uniqueidentifier] NULL,
	[BrokerId] [uniqueidentifier] NULL,
	[TickerSymbol] [varchar](10) NULL,
	[PriceTotal] [money] NULL,
	[TradeCurrency] [varchar](5) NULL,
	[NumberOfShares] [int] NULL
) ON [PRIMARY]
GO


