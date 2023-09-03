namespace CommonModels.Config
{
    public class ServiceBusConfig
    {
        public string TopicName { get; set; }

        public bool AutoCompleteMessages { get; set; }

        public int MaxConcurrentCalls { get; set; }
        
        public string ConnectionString { get; set; }
    }
}
