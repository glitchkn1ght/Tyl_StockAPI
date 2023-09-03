using CommonModels;

namespace Stock_API.Models.Response
{
    public class TradeResponse : GeneralResponse
    {
        public GeneralResponse Response { get; set; } = new GeneralResponse();
        
        public Trade Trade { get; set; } = new Trade();
    }
}
