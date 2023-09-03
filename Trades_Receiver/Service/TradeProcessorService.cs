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

        }

        public async Task ProcessTrades()
        {
            try
            {
                _processor = _client.CreateProcessor("trades", "TradeProcessor", new ServiceBusProcessorOptions());

                _processor.ProcessMessageAsync += MessageHandler;
                _processor.ProcessErrorAsync += ErrorHandler;

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
            string body = args.Message.Body.ToString();
            Console.WriteLine(body);

            Trade receivedTrade = JsonConvert.DeserializeObject<Trade>(args.Message.Body.ToString())!;

            await _tradeRepository.InsertTrade(receivedTrade);

            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // the error source tells me at what point in the processing an error occurred
            this._logger.LogError(($"[Operation=ProcessTrades], Status=Failure, Message=Error Source {args.ErrorSource}, Exception={args.Exception.Message}"));
            return Task.CompletedTask;
        }
    }
}
