using Azure.Messaging.ServiceBus;
using CommonModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stock_API.Interfaces;
using Stock_API.Models;

namespace Stock_API.ServiceBus
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ServiceBusConfig _serviceBusConfig;
        private readonly ServiceBusClient _client;

        public ServiceBusPublisher(IOptions<ServiceBusConfig> serviceBusConfig, ServiceBusClient client)
        {
            _serviceBusConfig = serviceBusConfig.Value;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task PublishTradeToTopic(Trade trade)
        {
            ServiceBusSender sender = _client.CreateSender(_serviceBusConfig.TopicName);

            string message = JsonConvert.SerializeObject(trade);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(message);

            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
