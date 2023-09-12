using Azure;
using Microsoft.AspNetCore.Mvc;
using Stock_API.Controllers;
using Stock_API.Interfaces;
using Stock_API.Models.Response;
using Stock_API.Validation;
using System.ComponentModel.DataAnnotations;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StockResponse))]
        public async Task<IActionResult> GetStockPrices([TickerSymbol] string symbols)
        {
            StockResponse stockResponse = new StockResponse()
            {
                RequestedSymbols = symbols
            };

            try
            {
                if (!ModelState.IsValid)
                {
                    stockResponse.ResponseStatus = _modelStateValidator.MapModelStateErrors(ModelState);

                    _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Validation of Stock Symbols failed, details {stockResponse.ResponseStatus.Message}");

                    return BadRequest(stockResponse);
                }

                stockResponse.Stocks =  await _stockService.GetStocks(symbols);

                return new OkObjectResult(stockResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Exception Thrown, details {ex.Message}");

                stockResponse.ResponseStatus.Code = 500;
                stockResponse.ResponseStatus.Message = "Internal Server Error";
                return new ObjectResult(stockResponse) { StatusCode = 500 };
            }
        }

        [HttpGet("GetAllStockPrices")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StockResponse))]
        public async Task<IActionResult> GetAllStockPrices()
        {
            StockResponse stockResponse = new StockResponse()
            {
                RequestedSymbols = "All"
            };

            try
            {
                stockResponse.Stocks = await _stockService.GetStocks(null);

                stockResponse.ResponseStatus.Code = 0;
                stockResponse.ResponseStatus.Message = "OK";

                return new OkObjectResult(stockResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Exception Thrown, details {ex.Message}");

                stockResponse.ResponseStatus.Code = 500;
                stockResponse.ResponseStatus.Message = "Internal Server Error";
                return new ObjectResult(stockResponse) { StatusCode = 500 };
            }
        }
    } 
}