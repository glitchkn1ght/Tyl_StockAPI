using CommonModels;

namespace Stock_API.Interfaces
{
    public interface IServiceBusPublisher
    {
        Task PublishTradeToTopic(Trade trade);
    }
}
