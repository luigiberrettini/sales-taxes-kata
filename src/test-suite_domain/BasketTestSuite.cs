using System.Text;
using SalesTaxesKata.Domain.Shopping;
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
            var importedGood = Good.FromString($"{quantity} {name} at {shelfPrice:F2}");
            Assert.Equal(builtGood, importedGood);
        }

        [Fact]
        public void BasketFromStringFormatIsOneLinePerGood()
        {
            var good1 = new Good("Article ABC 1", 11, 11.47M);
            var good2 = new Good("Article ABC 2", 12, 12.47M);
            var builtBasket = new Basket();
            builtBasket.Add(good1);
            builtBasket.Add(good2);
            var sb = new StringBuilder();
            sb.AppendLine($"{good1.Quantity} {good1.Name} at {good1.ShelfPrice:F2}");
            sb.AppendLine($"{good2.Quantity} {good2.Name} at {good2.ShelfPrice:F2}");
            var importedBasket = Basket.FromString(sb.ToString());
            Assert.Equal(builtBasket, importedBasket);
        }
    }
}