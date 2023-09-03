namespace CommonModels
{
    public class Trade
    {
        public Guid TradeId { get; set; }   
        
        public Guid BrokerId { get; set; }

        public string TickerSymbol { get; set; } = string.Empty;

        public decimal PriceTotal { get; set; } 

        public string TradeCurrency { get; set; } = string.Empty;

        public decimal NumberOfShares { get; set; }
    }
}