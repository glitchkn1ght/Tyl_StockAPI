using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Stock_API.Controllers;
using Stock_API.Interfaces;
using Stock_API.Models;
using Stock_API.Models.Response;
using Tyl_StockAPI.Controllers;

namespace StockAPI.UnitTests.Controllers
{
    [TestFixture]
    public class StockPriceTests
    {
        private Mock<ILogger<TradesController>> _loggerMock;
        private Mock<IModelStateErrorMapper> _modelStateValidator;
        private Mock<IStockService> _stockServiceMock;
        private StockPriceController _stockController;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TradesController>>();
            _modelStateValidator = new Mock<IModelStateErrorMapper>();
            _stockServiceMock = new Mock<IStockService>();

            _stockController = new StockPriceController(_loggerMock.Object, _modelStateValidator.Object, _stockServiceMock.Object);
        }

        [Test]
        public void When_StockRetrievedSuccessfully_ThenGetStockPricesReturnsOk()
        {
            string symbols = "AMZN";
            
            ResponseStatus validationResponse = new ResponseStatus() { Code = 0, Message = "OK" };

            List<Stock> stocks = new List<Stock>();
            stocks.Add(new Stock("AAPL", 100.1M));

            _stockServiceMock.Setup(x => x.GetStocks(symbols)).Returns(Task.FromResult(stocks));

            ObjectResult actual = (ObjectResult)this._stockController.GetStockPrices(symbols).Result;

            Assert.IsInstanceOf<StockResponse>(actual.Value);
            Assert.AreEqual(200, actual.StatusCode);
        }

        [Test]
        public void When_ModelStateHasErrors_ThenGetStockPricesReturnsBadRequest()
        {
            string symbol = "AMZN";
            ResponseStatus validationResponse = new ResponseStatus() { Code = -101, Message = "ValidationError" };
            _stockController.ModelState.AddModelError("SomeKey", "SomeError");

            _modelStateValidator.Setup(x => x.MapModelStateErrors(It.IsAny<ModelStateDictionary>())).Returns(validationResponse);

            ObjectResult actual = (ObjectResult)this._stockController.GetStockPrices(symbol).Result;

            _stockServiceMock.Verify(x => x.GetStocks(It.IsAny<string>()), Times.Never);

            Assert.IsInstanceOf<StockResponse>(actual.Value);
            Assert.AreEqual(-101, ((StockResponse)actual.Value!).ResponseStatus.Code);
            Assert.AreEqual(400, actual.StatusCode);
        }

        [Test]
        public void When_ExceptionThrown_ThenStoreTradeReturnsInternalServerError()
        {
            string symbol = "AMZN";

            _stockServiceMock.Setup(x => x.GetStocks(It.IsAny<string>())).Throws(new Exception());

            ObjectResult actual = (ObjectResult)this._stockController.GetStockPrices(symbol).Result;

            Assert.IsInstanceOf<StockResponse>(actual.Value);
            Assert.AreEqual(500, actual.StatusCode);
            Assert.AreEqual("Internal Server Error", ((StockResponse)actual.Value).ResponseStatus.Message);
        }
    }
}