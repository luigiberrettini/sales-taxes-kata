using SalesTaxes.Domain;
using System.Linq;
using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Shopping;
using Xunit;

namespace SalesTaxes.TestSuite.Domain
{
    public class TestSuite
    {
        [Fact]
        public void OneBookIsExempt()
        {
            var counter = new Checkout();
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            counter.Scan(article);
            var receipt = counter.EmitReceipt();
            Assert.Equal(article.Price, receipt.Entries.SingleOrDefault()?.Price);
        }
    }
}