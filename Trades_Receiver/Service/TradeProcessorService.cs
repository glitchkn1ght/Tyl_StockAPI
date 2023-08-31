using CommonModels;
using Microsoft.Extensions.Logging;
using Trades_Receiver.DAL.Repositories;
using TradesProcessor.Interfaces;

namespace TradesProcessor.Service
{
    public class TradeProcessorService
    {
        private readonly IServiceBusTradeReceiver _serviceBusTradeReceiver;
        private readonly ITradesRepository _tradeRepository;
        private readonly ILogger<TradeProcessorService> _logger;

        public TradeProcessorService(   ILogger<TradeProcessorService> logger, 
                                        IServiceBusTradeReceiver serviceBusTradeReceiver,
                                        ITradesRepository tradesRepository) 
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusTradeReceiver = serviceBusTradeReceiver ?? throw new ArgumentNullException(nameof(serviceBusTradeReceiver)); 
            _tradeRepository = tradesRepository ?? throw new ArgumentNullException(nameof(tradesRepository));
        }

        public async Task ProcessTradeTransactions()
        {
            try
            {
                Trade trade = await _serviceBusTradeReceiver.ReceiveTrade();

                if (trade == null)
                {
                    this._logger.LogWarning("[Operation = ProcessTradeTransactions],Status= Failed, Message=Could Not receive trade from Service bus");
                    return;
                }

                this._logger.LogInformation($"[Operation = ProcessTradeTransactions],Status= Sucess, Message=Received trade {trade.TradeId} from Service bus");

                int repoResult = await _tradeRepository.InsertTrade(trade);

                if(repoResult != 0)
                {
                    this._logger.LogWarning("[Operation = ProcessTradeTransactions],Status= Failed, Message=Could Not receive trade from Service bus");
                }

                else
                {
                    this._logger.LogInformation($"[Operation = ProcessTradeTransactions],Status= Sucess, Message=Trade {trade.TradeId} successfully stored in db");
                }
            }

            catch (Exception ex)
            {
                this._logger.LogError($"[Operation = ProcessTradeTransactions],Status= Sucess, Message=Exception Thrown when processing trade");
            }
        }
    }
}
