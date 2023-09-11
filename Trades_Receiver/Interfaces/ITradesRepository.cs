using Stock_API.Models;

namespace Trades_Receiver.DAL.Repositories
{
    public interface ITradesRepository
    {
        public Task InsertTrade(Trade trade);
    }
}