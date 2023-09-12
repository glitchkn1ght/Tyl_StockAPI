using Stock_API.Models;

namespace Stock_API.Interfaces
{
    public interface IStockService
    {
       public Task<List<Stock>> GetStocks(string requestedSymbols);

       public Task<List<Stock>> GetAllStocks();
    }
}