using Stock_API.Models;

namespace Stock_API.Interfaces
{
    public interface IStockService
    {
       public Task<List<Stock>> GetStocks(List<string>? requestedSymbols);
    }
}