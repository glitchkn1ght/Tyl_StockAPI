using Stock_API.Interfaces;
using Stock_API.Models.Response;

namespace Stock_API.Service
{
    public class SymbolValidationService : ISymbolValidationService
    {
        //In order to be most performant this would be cached from an external service at an appropriate interval.
        List<string> validSymbols = new List<string> { "AAPL", "AMZN", "MSFT", "ATVI" };

        public async Task<ResponseStatus> ValidateTickerSymbol(string requestedSymbols)
        {
            ResponseStatus response = new ResponseStatus();
            List<string> requestedSymbolList = requestedSymbols.Split(',').ToList();

            IEnumerable<string> invalidSymbols = requestedSymbolList.Except(validSymbols);

            if(invalidSymbols.Any())
            {
                string validationErrors = String.Join(", ", invalidSymbols);

                response.Code = -101;
                response.Message = $"The following symbol(s) are not valid {validationErrors}";
            }

            else
            {
                response.Code = 0;
                response.Message = "OK";
            }

            return response;
        }
    }
}
