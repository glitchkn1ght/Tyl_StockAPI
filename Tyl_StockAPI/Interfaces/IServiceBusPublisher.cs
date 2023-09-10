using Stock_API.Models;

namespace Stock_API.Interfaces
{
    public interface IServiceBusPublisher
    {
        Task PublishTradeToTopic(Trade trade);
    }
}
