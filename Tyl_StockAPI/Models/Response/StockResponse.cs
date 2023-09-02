namespace Stock_API.Models.Response
{
    public class StockResponse 
    {
        public BaseResponse BaseResponse { get; set; } = new BaseResponse();

        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}
