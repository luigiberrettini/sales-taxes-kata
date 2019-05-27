using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Shopping;
using System.Linq;
using Xunit;

namespace SalesTaxes.TestSuite.Domain
{
    public class TestSuite
    {
        [Fact]
        public void OneBookIsExempt()
        {
            var checkout = new Checkout();
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            checkout.Scan(article);
            var receipt = checkout.EmitReceipt();
            Assert.Equal(article.Price, receipt.Entries.SingleOrDefault()?.Price);
        }
    }
}