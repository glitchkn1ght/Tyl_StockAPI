namespace Stock_API.Models
{
    public class Stock
    {
        public Stock(string tickerSymbol, decimal pricePerUnit)
        {
            TickerSymbol = tickerSymbol;
            PricePerUnit = pricePerUnit;
        }

        public string TickerSymbol { get; set; }

        public decimal PricePerUnit { get; set; }   
    }
}
