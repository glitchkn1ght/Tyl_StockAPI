using CommonModels;

namespace Stock_API.Models.Response
{
    public class TradeResponse : BaseResponse
    {
        public Trade Trade { get; set; }
    }
}
