using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class BasketTestSuite
    {
        [Fact]
        public void GoodFromStringFormatIsQuantityNameAtShelfPrice()
        {
            const string name = "Article ABC";
            const int quantity = 2;
            const decimal shelfPrice = 10M;
            var builtGood = new Good(name, quantity, shelfPrice);
            var importedGood = Good.FromString($"{quantity} {name} at {shelfPrice}");
            Assert.Equal(builtGood, importedGood);
        }
    }
}