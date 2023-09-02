namespace Stock_API.Models.Response
{
    public class BaseResponse
    {
        public int Code { get; set; } = 0;

        public string Message { get; set; } = string.Empty;
    }
}
