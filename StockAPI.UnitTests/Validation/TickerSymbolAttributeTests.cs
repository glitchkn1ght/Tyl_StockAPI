using NUnit.Framework;
using Stock_API.Validation;

namespace StockAPI.UnitTests.Validation
{
    [TestFixture]
    public class TickerSymbolAttributeTests
    {
        private TickerSymbolAttribute _attribute;

        [SetUp]
        public void Setup() 
        { 
            _attribute = new TickerSymbolAttribute();
        }

        [TestCase("AAA,BCC")]
        [TestCase("AAAA")]
        [TestCase("AAAA,AMZN")]
        public void WhenTickerSymbolsContainInvalidSymbols_ThenReturnFalse(string symbols)
        {
            bool result = _attribute.IsValid(symbols);

            Assert.IsFalse(result);
        }

        [TestCase("AMZN,MSFT")]
        [TestCase("AMZN")]
        public void WhenTickerSymbolsDoesNotContainInvalidSymbols_ThenReturnTrue(string symbols)
        {
            bool result = _attribute.IsValid(symbols);

            Assert.True(result);
        }
    }
}
