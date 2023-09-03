namespace CommonModels
{
    public class ServiceBusConfig
    {
        public string TopicName { get; set; } = string.Empty;

        public string SubscriptionName { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;
    }
}
