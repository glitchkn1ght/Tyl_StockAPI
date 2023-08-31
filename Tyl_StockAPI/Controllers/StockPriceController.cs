using Azure;
using Microsoft.AspNetCore.Mvc;
using Stock_API.ServiceBus;
using Tyl_StockAPI.Models;
using Tyl_StockAPI.Models.Response;

namespace Tyl_StockAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class StockPriceController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public StockPriceController(ILogger<TradesController> logger, IServiceBusPublisher serviceBusPublisher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceBusPublisher = serviceBusPublisher ?? throw new ArgumentNullException(nameof(serviceBusPublisher));
        }

        [HttpGet("GetSingleClaimByUCR/{uniqueClaimsReference}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StockResponse))]
        public async Task<IActionResult> GetStockPriceForSymbol(string symbol)
        {
            StockResponse response = new StockResponse();

            try
            {
                bool validationResult = false;

                if (!validationResult)
                {
                    response.Code = -101;
                    response.Message = "Validation of Stock Symbol falied, please check input.";
    
                    return new BadRequestObjectResult(response);
                }

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