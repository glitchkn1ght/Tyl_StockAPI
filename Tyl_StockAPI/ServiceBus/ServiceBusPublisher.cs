using Azure.Messaging.ServiceBus;
using CommonModels;
using CommonModels.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Stock_API.ServiceBus
{
    public interface IServiceBusPublisher
    {
        Task PublishTradeToTopic(Trade trade);
    }

    public class ServiceBusPublisher : IServiceBusPublisher
    {
        public readonly ServiceBusConfig _serviceBusConfig;

        public ServiceBusPublisher(IOptions<ServiceBusConfig> serviceBusConfig)
        {
            _serviceBusConfig = serviceBusConfig.Value;
        }

        public async Task PublishTradeToTopic(Trade trade)
        {
            await using ServiceBusClient client = new ServiceBusClient(_serviceBusConfig.ConnectionString);
            ServiceBusSender sender = client.CreateSender(_serviceBusConfig.TopicName);

            string message = JsonConvert.SerializeObject(trade);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(message);

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
