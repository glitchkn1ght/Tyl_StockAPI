namespace Stock_API.Models.Response
{
    public class StockResponse 
    {
        public GeneralResponse Response { get; set; } = new GeneralResponse();

        public string RequestedSymbols { get; set; } = string.Empty;

        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
