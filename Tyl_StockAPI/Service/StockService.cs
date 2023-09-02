using Stock_API.Models;

namespace Stock_API.Interfaces
{
    public class StockService : IStockService
    {
        //In reality this would be calling to some external system/API such as finhub or bloomberg.
    
        public StockService() { }

        public async Task<List<Stock>> GetStocks(List<string>? requestedSymbols)
        {
            //create httpclient, call to external api
            List<Stock> stocks = new List<Stock>();
            Random rnd = new Random();

            if (requestedSymbols != null)
            {
                foreach (string s in requestedSymbols)
                {
                    stocks.Add(new Stock(s, rnd.Next(1, 500)));
                }
            }

            else
            {
                stocks.Add(new Stock("AAPL", rnd.Next(1, 500)));
                stocks.Add(new Stock("AMZN", rnd.Next(1, 500)));
                stocks.Add(new Stock("MSFT", rnd.Next(1, 500)));
                stocks.Add(new Stock("ATVI", rnd.Next(1, 500)));

            }
            return await Task.FromResult(stocks);
        }
    }
}
