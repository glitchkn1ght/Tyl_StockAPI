using Stock_API.Interfaces;
using Stock_API.Models.Response;

namespace Stock_API.Service
{
    public class StockValidationService : IStockValidationService
    {
        //In order to be most performant this would be cached from an external service at an appropriate interval.
        List<string> validSymbols = new List<string> { "AAPL", "AMZN", "MSFT", "ATVI" };

        public async Task<BaseResponse> ValidateStock(List<string> requestedSymbols)
        {
            BaseResponse response = new BaseResponse();

            IEnumerable<string> invalidSymbols = requestedSymbols.Except(validSymbols);

            if(invalidSymbols.Any())
            {
                string validationErrors = String.Join(", ", invalidSymbols);

                response.Code = -101;
                response.Message = $"The following symbols are not valid {validationErrors}";
            }

            return response;
        }
    }
}
