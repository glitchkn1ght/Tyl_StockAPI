using CommonModels;

namespace Stock_API.Models.Response
{
    public class TradeResponse
    {
        public GeneralResponse Response { get; set; } = new GeneralResponse();
        
        public Trade Trade { get; set; } = new Trade();
    }
}
