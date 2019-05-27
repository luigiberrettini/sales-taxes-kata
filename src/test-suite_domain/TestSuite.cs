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

        [Fact]
        public void TwoBooksAreExempt()
        {
            var checkout = new Checkout();
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            var priceForTwo = article.Price * 2;
            checkout.Scan(article);
            checkout.Scan(article);
            var receipt = checkout.EmitReceipt();
            Assert.Equal(priceForTwo, receipt.Entries.SingleOrDefault()?.Price);
        }

        [Fact]
        public void PurchaseGroupsByArticle()
        {
            var article = new Article { Id = 1, Name = "Gone with the wind", Price = 25.0M };
            var purchase = new Purchase();
            purchase.Add(article);
            purchase.Add(article);
            Assert.NotNull(purchase.Articles.SingleOrDefault());
        }
    }
}