using Azure.Messaging.ServiceBus;
using CommonModels;
using CommonModels.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TradesProcessor.Interfaces;

namespace TradesProcessor.ServiceBus
{
    public class ServiceBusTradeReceiver : IServiceBusTradeReceiver
    {
        public readonly ServiceBusConfig _serviceBusConfig;

        public ServiceBusTradeReceiver(IOptions<ServiceBusConfig> serviceBusConfig)
        {
            _serviceBusConfig = serviceBusConfig.Value;
        }

        public async Task<Trade> ReceiveTrade()
        {
            await using var client = new ServiceBusClient(_serviceBusConfig.ConnectionString);
            ServiceBusReceiver receiver = client.CreateReceiver(_serviceBusConfig.TopicName, "TradeProcessor");

            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

            Trade receivedTrade = JsonConvert.DeserializeObject<Trade>(receivedMessage.Body.ToString())!;

            return receivedTrade;
        }
    }
}
