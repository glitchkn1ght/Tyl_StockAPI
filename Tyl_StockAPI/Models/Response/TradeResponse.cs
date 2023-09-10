using CommonModels;

namespace Stock_API.Models.Response
{
    public class TradeResponse
    {
        public ResponseStatus ResponseStatus { get; set; } = new ResponseStatus();
        
        public Trade Trade { get; set; } = new Trade();
    }
}
