using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Stock_API.Controllers;
using Stock_API.Interfaces;
using Stock_API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using CommonModels;
using Tyl_StockAPI.Controllers;
using Stock_API.Models;

namespace StockAPI.UnitTests.Controllers
{
    public class StockPriceTests
    {
        private Mock<ILogger<TradesController>> _loggerMock;
        private Mock<ISymbolValidationService> _symbolValidationServiceMock;
        private Mock<IStockService> _stockServiceMock;
        private StockPriceController _stockController;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TradesController>>();
            _symbolValidationServiceMock = new Mock<ISymbolValidationService>();
            _stockServiceMock = new Mock<IStockService>();

            _stockController = new StockPriceController(_loggerMock.Object, _symbolValidationServiceMock.Object, _stockServiceMock.Object);
        }

        [Test]
        public void When_StockRetrievedSuccessfully_ThenGetStockPricesReturnsOk()
        {
            string symbols = "AMZN";
            
            GeneralResponse validationResponse = new GeneralResponse() { Code = 0, Message = "OK" };

            List<Stock> stocks = new List<Stock>();
            stocks.Add(new Stock("AAPL", 100.1M));

            _symbolValidationServiceMock.Setup(x => x.ValidateTickerSymbol(symbols)).Returns(Task.FromResult(validationResponse));

            _stockServiceMock.Setup(x => x.GetStocks(symbols)).Returns(Task.FromResult(stocks));

            ObjectResult actual = (ObjectResult)this._stockController.GetStockPrices(symbols).Result;

            Assert.IsInstanceOf<StockResponse>(actual.Value);
            Assert.AreEqual(200, actual.StatusCode);
        }

        [Test]
        public void When_ValidationServiceReturnsNonSuccess_ThenGetStockPricesReturnsBadRequest()
        {
            string symbol = "AMZN";
            
            GeneralResponse validationResponse = new GeneralResponse() { Code = -101, Message = "ValidationError"};

            _symbolValidationServiceMock.Setup(x => x.ValidateTickerSymbol(It.IsAny<string>())).Returns(Task.FromResult(validationResponse));

            ObjectResult actual = (ObjectResult)this._stockController.GetStockPrices(symbol).Result;

            _stockServiceMock.Verify(x => x.GetStocks(It.IsAny<string>()), Times.Never);

            Assert.IsInstanceOf<StockResponse>(actual.Value);
            Assert.AreEqual(-101, ((StockResponse)actual.Value!).Response.Code);
            Assert.AreEqual(400, actual.StatusCode);
        }

        [Test]
        public void When_ExceptionThrown_ThenStoreTradeReturnsInternalServerError()
        {
            string symbol = "AMZN";

            _symbolValidationServiceMock.Setup(x => x.ValidateTickerSymbol(It.IsAny<string>())).Throws(new Exception());

            ObjectResult actual = (ObjectResult)this._stockController.GetStockPrices(symbol).Result;

            _stockServiceMock.Verify(x => x.GetStocks(It.IsAny<string>()), Times.Never);

            Assert.IsInstanceOf<StockResponse>(actual.Value);
            Assert.AreEqual(500, actual.StatusCode);
            Assert.AreEqual("Internal Server Error", ((StockResponse)actual.Value).Response.Message);
        }
    }
}