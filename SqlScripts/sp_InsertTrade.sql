USE [TylTrades]
GO

/****** Object:  StoredProcedure [dbo].[InsertTrade]    Script Date: 03/09/2023 20:14:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<James Smith>
-- Create date: <03/09/2023>
-- Description:	<Insert Trade Procedure>
-- =============================================
CREATE PROCEDURE [dbo].[InsertTrade]
	@TradeId uniqueidentifier,
	@BrokerId uniqueidentifier,
	@TickerSymbol varchar(10),
	@PriceTotal money,
	@TradeCurrency varchar(5),
	@NumberOfShares INT
AS
BEGIN

    Insert Into [dbo].[Trades]
	(	TradeId,
		BrokerId,
		TickerSymbol,
		PriceTotal,
		TradeCurrency,
		NumberOfShares
	)
	VALUES 
	(
	  @TradeId,
	  @BrokerId,
	  @TickerSymbol,
	  @PriceTotal,
	  @TradeCurrency,
	  @NumberOfShares
	)
END
GO


