using Azure;
using CommonModels;
using Microsoft.AspNetCore.Mvc;
using Stock_API.Models.Response;
using Stock_API.ServiceBus;
using Tyl_StockAPI.Models;
using Tyl_StockAPI.Models.Response;

namespace Tyl_StockAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public TradesController(ILogger<TradesController> logger, IServiceBusPublisher serviceBusPublisher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusPublisher = serviceBusPublisher ?? throw new ArgumentNullException(nameof(serviceBusPublisher));
        }

        [HttpPost]
        public async Task<IActionResult> ProcessTrade(Trade trade)
        {
            TradeResponse response = new TradeResponse();
            
            try
            {
                bool validationResult = false;

                _logger.LogInformation($"[Operation=ProcessTrade], Status=Success, Message=Processing Trade {trade.TradeId} from Broker {trade.BrokerId}");

                if (!validationResult)
                {
                    response.Code = -101;
                    response.Message = "Validation of Trade Symbol falied, please check input.";

                    return new BadRequestObjectResult(response);
                }

                await _serviceBusPublisher.PublishTradeToTopic(trade);

                return new OkObjectResult(response);

            }

            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = "Internal Server Error";
                return new ObjectResult(response) { StatusCode = 500 };
            }
        }
    }
}