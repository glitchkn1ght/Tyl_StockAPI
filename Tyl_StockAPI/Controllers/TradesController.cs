using CommonModels;
using Microsoft.AspNetCore.Mvc;
using Stock_API.Interfaces;
using Stock_API.Models.Response;

namespace Stock_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly ISymbolValidationService _symbolValidationService;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public TradesController(ILogger<TradesController> logger, ISymbolValidationService symbolValidationService, IServiceBusPublisher serviceBusPublisher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusPublisher = serviceBusPublisher ?? throw new ArgumentNullException(nameof(serviceBusPublisher));
            _symbolValidationService = symbolValidationService ?? throw new ArgumentNullException(nameof(symbolValidationService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StockResponse))]
        public async Task<IActionResult> StoreTrade(Trade trade)
        {
            TradeResponse tradeResponse = new TradeResponse()
            {
                Trade = trade
            };

            try
            {
                _logger.LogInformation($"[Operation=ProcessTrade], Status=Success, Message=Processing Trade {trade.TradeId} from Broker {trade.BrokerId}");

                tradeResponse.Response = await _symbolValidationService.ValidateTickerSymbol(trade.TickerSymbol);

                if (tradeResponse.Response.Code != 0)
                {
                    return new BadRequestObjectResult(tradeResponse);
                }

                _logger.LogInformation($"[Operation=ProcessTrade], Status=Success, Message=Validation of Symbols Successful, Posting trade {trade.TradeId} to message bus.");

                await _serviceBusPublisher.PublishTradeToTopic(trade);

                return new OkObjectResult(tradeResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError($"[Operation=ProcessTrade], Status=Failure, Message=Exception Thrown, details {ex.Message}");

                tradeResponse.Response.Code = 500;
                tradeResponse.Response.Message = "Internal Server Error";
                return new ObjectResult(tradeResponse) { StatusCode = 500 };
            }
        }
    }
}