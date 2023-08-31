using Azure.Messaging.ServiceBus;
using CommonModels;

namespace TradesProcessor.Interfaces
{
    public interface IServiceBusTradeReceiver
    {
        Task<Trade> ReceiveTrade();
    }
}