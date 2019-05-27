using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Shopping;
using System.Linq;
using Xunit;

namespace SalesTaxes.TestSuite.Domain
{
    public class TestSuite
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(100)]
        public void NBooksAreExempt(int n)
        {
            var checkout = new Checkout();
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            var priceForN = article.Price * n;
            Enumerable.Range(1, n).ToList().ForEach(x => checkout.Scan(article));
            var receipt = checkout.EmitReceipt();
            Assert.Equal(priceForN, receipt.Entries.SingleOrDefault()?.Price);
        }

        [Fact]
        public void PurchaseGroupsByArticle()
        {
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            var purchase = new Purchase();
            purchase.Add(article);
            purchase.Add(article);
            Assert.NotNull(purchase.Items.SingleOrDefault());
        }
    }
}