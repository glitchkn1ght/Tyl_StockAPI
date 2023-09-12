using NUnit.Framework;
using Stock_API.Validation;

namespace StockAPI.UnitTests.Validation
{
    [TestFixture]
    public class NotNullOrEmptyAttributeTests
    {
        private NotNullOrEmptyAttribute _attribute;

        [SetUp]
        public void Setup()
        {
            _attribute = new NotNullOrEmptyAttribute();
        }

        [Test]
        public void WhenGuidIsValidThenReturnTrue()
        {
            Guid guid = new Guid("3a966d9a-e6b5-4436-b903-f6bf49f88703");

            bool result = _attribute.IsValid(guid);

            Assert.IsTrue(result);
        }

        [Test]
        public void WhenGuidIsDefaultValueThenReturnFalse()
        {
            Guid guid = Guid.Empty;

            bool result = _attribute.IsValid(guid);

            Assert.IsFalse(result);
        }

        [Test]
        public void WhenGuidIsNullThenReturnFalse()
        {
             Guid? guid = null;

            bool result = _attribute.IsValid(guid);

            Assert.IsFalse(result);
        }
    }
}
