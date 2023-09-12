using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Stock_API.Controllers;
using Stock_API.Interfaces;
using Stock_API.Models;
using Stock_API.Models.Response;

namespace StockAPI.UnitTests.Controllers
{
    public class TradeControllerTests
    {
        private Mock<ILogger<TradesController>> _loggerMock;
        private Mock<IModelStateErrorMapper> _modelStateErrorMapper;
        private Mock<IServiceBusPublisher> _serviceBusPublisherMock;
        private TradesController _tradeController;
        private Trade _trade;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TradesController>>();
            _modelStateErrorMapper = new Mock<IModelStateErrorMapper>();
            _serviceBusPublisherMock = new Mock<IServiceBusPublisher>();

            _tradeController = new TradesController(_loggerMock.Object, _modelStateErrorMapper.Object, _serviceBusPublisherMock.Object);

            _trade = new Trade()
            {
                BrokerId = new Guid("136bb453-c002-49fe-9ab4-b8d3f528ca4c"),
                TradeId = new Guid("5b1589c9-d052-465b-8b62-bd241a9c6847"),
                NumberOfShares = 10,
                PriceTotalPounds = 100,
                TickerSymbol = "AMZN"
            };
        }

        [Test]
        public void When_TradeStoredSuccessfully_ThenStoreTradeReturnsOk()
        {
            ResponseStatus validationResponse = new ResponseStatus() { Code = 0, Message = "OK" };

            ObjectResult actual = (ObjectResult)this._tradeController.StoreTrade(_trade).Result;

            _modelStateErrorMapper.Verify(x => x.MapModelStateErrors(It.IsAny<ModelStateDictionary>()),Times.Never);
            _serviceBusPublisherMock.Verify(x => x.PublishTradeToTopic(_trade), Times.Once);

            Assert.IsInstanceOf<TradeResponse>(actual.Value);
            Assert.AreEqual(200, actual.StatusCode);
        }

        [Test]
        public void When_ModelStateHasErrors_ThenStoreTradeReturnsBadRequest()
        {
            ResponseStatus validationResponse = new ResponseStatus() { Code = -101, Message = "ValidationError"};

            _tradeController.ModelState.AddModelError("SomeKey", "SomeError");
            _modelStateErrorMapper.Setup(x => x.MapModelStateErrors(It.IsAny<ModelStateDictionary>())).Returns(validationResponse);

            ObjectResult actual = (ObjectResult)this._tradeController.StoreTrade(_trade).Result;

            _serviceBusPublisherMock.Verify(x => x.PublishTradeToTopic(It.IsAny<Trade>()), Times.Never);

            Assert.IsInstanceOf<TradeResponse>(actual.Value);
            Assert.AreEqual(-101, ((TradeResponse)actual.Value!).ResponseStatus.Code);
            Assert.AreEqual(400, actual.StatusCode);
        }

        [Test]
        public void When_ExceptionThrown_ThenStoreTradeReturnsInternalServerError()
        {
            _serviceBusPublisherMock.Setup(x => x.PublishTradeToTopic(_trade)).Throws(new Exception());

            ObjectResult actual = (ObjectResult)this._tradeController.StoreTrade(_trade).Result;

            _modelStateErrorMapper.Verify(x => x.MapModelStateErrors(It.IsAny<ModelStateDictionary>()), Times.Never);
        
            Assert.IsInstanceOf<TradeResponse>(actual.Value);
            Assert.AreEqual(500, actual.StatusCode);
            Assert.AreEqual("Internal Server Error", ((TradeResponse)actual.Value).ResponseStatus.Message);
        }
    }
}