using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Stock_API.Controllers;
using Stock_API.Interfaces;
using Stock_API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using CommonModels;

namespace StockAPI.UnitTests.Controllers
{
    public class TradeControllerTests
    {
        private Mock<ILogger<TradesController>> _loggerMock;
        private Mock<ISymbolValidationService> _symbolValidationServiceMock;
        private Mock<IServiceBusPublisher> _serviceBusPublisherMock;
        private TradesController _tradeController;
        private Trade _trade;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TradesController>>();
            _symbolValidationServiceMock = new Mock<ISymbolValidationService>();
            _serviceBusPublisherMock = new Mock<IServiceBusPublisher>();

            _tradeController = new TradesController(_loggerMock.Object, _symbolValidationServiceMock.Object, _serviceBusPublisherMock.Object);

            _trade = new Trade()
            {
                BrokerId = new Guid("136bb453-c002-49fe-9ab4-b8d3f528ca4c"),
                TradeId = new Guid("5b1589c9-d052-465b-8b62-bd241a9c6847"),
                TradeCurrency = "GBP",
                NumberOfShares = 10,
                PriceTotal = 100,
                TickerSymbol = "AMZN"
            };
        }

        [Test]
        public void When_TradeStoredSuccessfully_ThenStoreTradeReturnsOk()
        {
            GeneralResponse validationResponse = new GeneralResponse() { Code = 0, Message = "OK" };

            _symbolValidationServiceMock.Setup(x => x.ValidateTickerSymbol(_trade.TickerSymbol)).Returns(Task.FromResult(validationResponse));

            ObjectResult actual = (ObjectResult)this._tradeController.StoreTrade(_trade).Result;

            this._serviceBusPublisherMock.Verify(x => x.PublishTradeToTopic(_trade), Times.Once);

            Assert.IsInstanceOf<TradeResponse>(actual.Value);
            Assert.AreEqual(200, actual.StatusCode);
        }

        [Test]
        public void When_ValidationServiceReturnsNonSuccess_ThenStoreTradeReturnsBadRequest()
        {
            GeneralResponse validationResponse = new GeneralResponse() { Code = -101, Message = "ValidationError"};

            _symbolValidationServiceMock.Setup(x => x.ValidateTickerSymbol(It.IsAny<string>())).Returns(Task.FromResult(validationResponse));

            ObjectResult actual = (ObjectResult)this._tradeController.StoreTrade(_trade).Result;

            this._serviceBusPublisherMock.Verify(x => x.PublishTradeToTopic(It.IsAny<Trade>()), Times.Never);

            Assert.IsInstanceOf<TradeResponse>(actual.Value);
            Assert.AreEqual(-101, ((TradeResponse)actual.Value!).Response.Code);
            Assert.AreEqual(400, actual.StatusCode);
        }

        [Test]
        public void When_ExceptionThrown_ThenStoreTradeReturnsInternalServerError()
        {
            _symbolValidationServiceMock.Setup(x => x.ValidateTickerSymbol(It.IsAny<string>())).Throws(new Exception());

            ObjectResult actual = (ObjectResult)this._tradeController.StoreTrade(_trade).Result;

            this._serviceBusPublisherMock.Verify(x => x.PublishTradeToTopic(It.IsAny<Trade>()), Times.Never);

            Assert.IsInstanceOf<TradeResponse>(actual.Value);
            Assert.AreEqual(500, actual.StatusCode);
            Assert.AreEqual("Internal Server Error", ((TradeResponse)actual.Value).Response.Message);
        }
    }
}