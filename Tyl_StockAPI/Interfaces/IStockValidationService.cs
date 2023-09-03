using Stock_API.Models.Response;

namespace Stock_API.Interfaces
{
    public interface ISymbolValidationService
    {
        public Task<GeneralResponse> ValidateTickerSymbol(string requestedSymbols);
    }
}