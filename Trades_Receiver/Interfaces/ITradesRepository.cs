using CommonModels;

namespace Trades_Receiver.DAL.Repositories
{
    public interface ITradesRepository
    {
        Task<int> InsertTrade(Trade trade);
    }
}