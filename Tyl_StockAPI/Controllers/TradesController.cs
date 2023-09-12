using CommonModels;
using Microsoft.AspNetCore.Mvc;
using Stock_API.Interfaces;
using Stock_API.Models;
using Stock_API.Models.Response;

namespace Stock_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly IModelStateErrorMapper _modelStateValidator;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public TradesController(ILogger<TradesController> logger, IModelStateErrorMapper modelStateValidator, IServiceBusPublisher serviceBusPublisher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelStateValidator = modelStateValidator ?? throw new ArgumentNullException(nameof(modelStateValidator));
            _serviceBusPublisher = serviceBusPublisher ?? throw new ArgumentNullException(nameof(serviceBusPublisher));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TradeResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(TradeResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(TradeResponse))]
        public async Task<IActionResult> StoreTrade(Trade trade)
        {
            TradeResponse tradeResponse = new TradeResponse()
            {
                Trade = trade
            };

            try
            {
                _logger.LogInformation($"[Operation=ProcessTrade], Status=Success, Message=Processing Trade {trade.TradeId} from Broker {trade.BrokerId}");

                if (!ModelState.IsValid)
                {
                    tradeResponse.ResponseStatus = _modelStateValidator.MapModelStateErrors(ModelState);

                    _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Model Validation failed, details {tradeResponse.ResponseStatus.Message}");

                    return BadRequest(tradeResponse);
                }

                _logger.LogInformation($"[Operation=ProcessTrade], Status=Success, Message=Model Validation Successful, Posting trade {trade.TradeId} to message bus.");

                await _serviceBusPublisher.PublishTradeToTopic(trade);

                return new OkObjectResult(tradeResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError($"[Operation=ProcessTrade], Status=Failure, Message=Exception Thrown, details {ex.Message}");

                tradeResponse.ResponseStatus.Code = 500;
                tradeResponse.ResponseStatus.Message = "Internal Server Error";
                return new ObjectResult(tradeResponse) { StatusCode = 500 };
            }
        }
    }
}