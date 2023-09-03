using Azure.Messaging.ServiceBus;
using CommonModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Trades_Receiver.DAL.Repositories;

namespace TradesProcessor.Service
{
    public class TradeProcessorService 
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusConfig _serviceBusConfig;
        private readonly ITradesRepository _tradeRepository;
        private readonly ILogger<TradeProcessorService> _logger;
        private ServiceBusProcessor _processor;

        public TradeProcessorService(ILogger<TradeProcessorService> logger, ServiceBusClient client, IOptions<ServiceBusConfig> serviceBusConfig, ITradesRepository tradesRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _serviceBusConfig = serviceBusConfig.Value;
            _tradeRepository = tradesRepository ?? throw new ArgumentNullException(nameof(tradesRepository));
            _processor = _client.CreateProcessor(_serviceBusConfig.TopicName, _serviceBusConfig.SubscriptionName, new ServiceBusProcessorOptions());
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        public async Task ProcessTrades()
        {
            try
            {
                this._logger.LogInformation(($"someinfo"));


                await _processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();
            }
            
            catch(Exception ex)
            {
                this._logger.LogError(($"[Operation=ProcessTrades], Status=Failure, Message=Exception Thrown, details {ex.Message}"));
            }

            finally
            {
                await _processor.DisposeAsync();
                await _client.DisposeAsync();
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {   
            Trade receivedTrade = JsonConvert.DeserializeObject<Trade>(args.Message.Body.ToString())!;

            this._logger.LogInformation(($"[Operation=ProcessTrades], Status=Failure, Message=Processing Trade {receivedTrade.TradeId}"));

            await _tradeRepository.InsertTrade(receivedTrade);

            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            this._logger.LogError(($"[Operation=ProcessTrades], Status=Failure, Message=Error Source {args.ErrorSource}, Exception={args.Exception.Message}"));
            return Task.CompletedTask;
        }
    }
}
