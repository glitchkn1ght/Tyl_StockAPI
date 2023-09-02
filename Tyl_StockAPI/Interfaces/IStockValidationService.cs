using Stock_API.Models.Response;

namespace Stock_API.Interfaces
{
    public interface IStockValidationService
    {
        public Task<BaseResponse> ValidateStock(List<string> requestedSymbols);
    }
}