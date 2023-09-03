using Azure.Messaging.ServiceBus;
using CommonModels;
using CommonModels.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Trades_Receiver.DAL.Repositories;

namespace TradesProcessor.Service
{
    public class TradeProcessorService 
    {
        //private readonly ServiceBusClient _client;
        private readonly ServiceBusConfig _serviceBusConfig;
        private readonly ITradesRepository _tradeRepository;
        private readonly ILogger<TradeProcessorService> _logger;

        public TradeProcessorService(ILogger<TradeProcessorService> logger, /*ServiceBusClient client,*/ IOptions<ServiceBusConfig> serviceBusConfig, ITradesRepository tradesRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
           // _client = client ?? throw new ArgumentNullException(nameof(client));
            _serviceBusConfig = serviceBusConfig.Value;
            _tradeRepository = tradesRepository ?? throw new ArgumentNullException(nameof(tradesRepository));

        }

        public async Task ProcessTrades()
        {
            try
            {
                ServiceBusClient _client = new ServiceBusClient(_serviceBusConfig.ConnectionString);

                await using ServiceBusProcessor processor = _client.CreateProcessor(_serviceBusConfig.TopicName, "TradeProcessor", new ServiceBusProcessorOptions());

                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();
            }
            
            catch(Exception ex)
            {
                this._logger.LogError(($"[Operation=ProcessTrades], Status=Failure, Message=Exception Thrown, details {ex.Message}"));
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine(body);

            Trade receivedTrade = JsonConvert.DeserializeObject<Trade>(args.Message.Body.ToString())!;

            await _tradeRepository.InsertTrade(receivedTrade);

            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // the error source tells me at what point in the processing an error occurred
            Console.WriteLine(args.ErrorSource);
            // the fully qualified namespace is available
            Console.WriteLine(args.FullyQualifiedNamespace);
            // as well as the entity path
            Console.WriteLine(args.EntityPath);
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
