namespace Stock_API.Models.Response
{
    public class StockResponse 
    {
        public ResponseStatus ResponseStatus { get; set; } = new ResponseStatus();

        public string RequestedSymbols { get; set; } = string.Empty;

        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
