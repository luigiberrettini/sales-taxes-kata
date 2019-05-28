using System.Linq;
using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Shopping;
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
            var article = new Article(1, "Gone with the wind", 25.0M);
            var priceForN = article.Price * n;
            Enumerable.Range(1, n).ToList().ForEach(x => checkout.Scan(article));
            var receipt = checkout.EmitReceipt();
            Assert.Equal(priceForN, receipt.Entries.SingleOrDefault()?.Price);
        }

        [Fact]
        public void OnePerfumeIsTaxed()
        {
            var checkout = new Checkout();
            var article = new Article(1, "Boss bottled", 112M);
            checkout.Scan(article);
            var receipt = checkout.EmitReceipt();
            Assert.NotEqual(article.Price, receipt.Entries.SingleOrDefault()?.Price);
        }

        [Fact]
        public void PurchaseApplyTax()
        {
            var article = new Article(1, "Boss bottled", 112M);
            const decimal taxRate = 10;
            var tax = new Tax(taxRate);
            var purchase = new Purchase();
            purchase.Add(article, tax);
            Assert.NotEqual(article.Price, purchase.Items.SingleOrDefault()?.Price);
        }

        [Fact]
        public void PurchaseGroupsByArticle()
        {
            var article = new Article(1, "Gone with the wind", 25.0M);
            var purchase = new Purchase();
            purchase.Add(article);
            purchase.Add(article);
            Assert.NotNull(purchase.Items.SingleOrDefault());
        }
    }
}