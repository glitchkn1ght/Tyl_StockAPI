namespace CommonModels
{
    public class Trade
    {
        public Guid TradeId { get; set; }   
        
        public Guid BrokerId { get; set; }

        public string TickerSymbol { get; set; }

        public decimal PriceTotal { get; set; } 

        public string PriceCurrency { get; set; }

        public decimal NumberOfShares { get; set; }
    }
}