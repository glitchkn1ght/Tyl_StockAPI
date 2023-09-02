using Microsoft.AspNetCore.Mvc;
using Stock_API.Interfaces;
using Stock_API.Models.Response;

namespace Tyl_StockAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class StockPriceController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly IStockValidationService _stockValidationService;
        private readonly IStockService _stockService;

        public StockPriceController(ILogger<TradesController> logger, IStockValidationService stockValidationService , IStockService stockService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stockValidationService = stockValidationService ?? throw new ArgumentNullException(nameof(stockValidationService));
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
        }

        [HttpGet("GetStockPricesForSymbols/{symbols}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StockResponse))]
        public async Task<IActionResult> GetStockPrices(string symbols)
        {
            List<string> requestedSymbols = symbols.Split(',').ToList();

            StockResponse stockResponse = new StockResponse();

            try
            {
                BaseResponse validationResult = await this._stockValidationService.ValidateStock(requestedSymbols);

                if (validationResult.Code != 0)
                {
                    stockResponse.BaseResponse = validationResult;

                    _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Validation of Stock Symbols failed, details {validationResult.Message}");

                    return new BadRequestObjectResult(stockResponse);
                }
                
                stockResponse.Stocks =  await _stockService.GetStocks(requestedSymbols);

                return new OkObjectResult(stockResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Exception Thrown, details {ex.Message}");

                stockResponse.BaseResponse.Code = 500;
                stockResponse.BaseResponse.Message = "Internal Server Error";
                return new ObjectResult(stockResponse) { StatusCode = 500 };
            }
        }

        [HttpGet("GetAllStockPrices")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StockResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StockResponse))]
        public async Task<IActionResult> GetAllStockPrices()
        {
            StockResponse stockResponse = new StockResponse();

            try
            {
                stockResponse.Stocks = await _stockService.GetStocks(null);

                return new OkObjectResult(stockResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError($"[Operation=GetStockPrices], Status=Failure, Message=Exception Thrown, details {ex.Message}");

                stockResponse.BaseResponse.Code = 500;
                stockResponse.BaseResponse.Message = "Internal Server Error";
                return new ObjectResult(stockResponse) { StatusCode = 500 };
            }
        }
    } 
}