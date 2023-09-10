using Stock_API.Models.Response;

namespace Stock_API.Interfaces
{
    public interface ISymbolValidationService
    {
        public Task<ResponseStatus> ValidateTickerSymbol(string requestedSymbols);
    }
}