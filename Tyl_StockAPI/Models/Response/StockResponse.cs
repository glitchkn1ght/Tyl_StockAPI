namespace Stock_API.Models.Response
{
    public class StockResponse 
    {
        public GeneralResponse Response { get; set; } = new GeneralResponse();

        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
