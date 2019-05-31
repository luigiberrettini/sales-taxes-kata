using System.Text;
using SalesTaxesKata.Domain.Shopping;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class BasketTestSuite
    {
        [Fact]
        public void LocalGoodFromStringFormatIsQuantityNameAtShelfPrice()
        {
            const string name = "Article ABC";
            const int quantity = 2;
            const decimal shelfPrice = 10M;
            var builtGood = new Good(name, quantity, shelfPrice, Origin.Local);
            var importedGood = Good.FromString($"{quantity} {name} at {shelfPrice:F2}");
            Assert.Equal(builtGood, importedGood);
        }

        [Fact]
        public void ImportedGoodFromStringFormatContainsTheWordImported()
        {
            const string name = "Article ABC DEF GHI";
            const int quantity = 2;
            const decimal shelfPrice = 10M;
            var builtGood = new Good(name, quantity, shelfPrice, Origin.Imported);
            const string printedName1 = "imported Article ABC DEF GHI";
            const string printedName2 = "Article ABC imported DEF GHI";
            const string printedName3 = "Article ABC DEF imported GHI";
            const string printedName4 = "Article ABC DEF GHI imported";
            var importedGood1 = Good.FromString($"{quantity} {printedName1} at {shelfPrice:F2}");
            var importedGood2 = Good.FromString($"{quantity} {printedName2} at {shelfPrice:F2}");
            var importedGood3 = Good.FromString($"{quantity} {printedName3} at {shelfPrice:F2}");
            var importedGood4 = Good.FromString($"{quantity} {printedName4} at {shelfPrice:F2}");
            Assert.Equal(builtGood, importedGood1);
            Assert.Equal(builtGood, importedGood2);
            Assert.Equal(builtGood, importedGood3);
            Assert.Equal(builtGood, importedGood4);
        }

        [Fact]
        public void BasketFromStringFormatIsOneLinePerGood()
        {
            var good1 = new Good("Article ABC 1", 11, 11.47M, Origin.Local);
            var good2 = new Good("Article ABC 2", 12, 12.47M, Origin.Local);
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