using Microsoft.AspNetCore.Mvc;
using Stock_API.Controllers;
using Stock_API.Interfaces;
using Stock_API.Models.Response;
using Stock_API.Validation;

namespace Tyl_StockAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class StockPriceController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly IModelStateErrorMapper _modelStateValidator;
        private readonly IStockService _stockService;

        public StockPriceController(ILogger<TradesController> logger, IModelStateErrorMapper modelStateValidator, IStockService stockService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _modelStateValidator = modelStateValidator ?? throw new ArgumentNullException(nameof(modelStateValidator));
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
        }

        [HttpGet("GetStockPricesForSymbols/{symbols}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseStatus))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseStatus))]
        public async Task<IActionResult> GetStockPrices([TickerSymbol] string symbols)
        {
            StockResponse stockResponse = new StockResponse()
            {
                RequestedSymbols = symbols
            };

            if (!ModelState.IsValid)
            {
                stockResponse.ResponseStatus = _modelStateValidator.MapModelStateErrors(ModelState);

                _logger.LogError($"Model State Validation failed, details {stockResponse.ResponseStatus.Message}");

                return BadRequest(stockResponse.ResponseStatus);
            }

            _logger.LogError($"Model State Validation Succeeded, Returning Stock Prices for symbols provided");

            stockResponse.Stocks = await _stockService.GetStocks(symbols);

            return new OkObjectResult(stockResponse);
        }

        [HttpGet("GetAllStockPrices")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseStatus))]
        public async Task<IActionResult> GetAllStockPrices()
        {   
            StockResponse stockResponse = new StockResponse()
            {
                RequestedSymbols = "All"
            };

            stockResponse.Stocks = await _stockService.GetAllStocks();

            return new OkObjectResult(stockResponse);
        }
    } 
}