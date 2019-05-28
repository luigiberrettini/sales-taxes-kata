using System;
using System.Linq;
using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Shopping;
using SalesTaxes.Domain.Taxation;
using Xunit;

namespace SalesTaxes.TestSuite.Domain
{
    public class TestSuite
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void NoTaxOnCheckoutForExemptCategories(int n)
        {
            var checkout = new Checkout();
            var categories = new[] { Category.Books, Category.Food, Category.Medical };
            decimal expectedPrice = 0;
            Enumerable.Range(1, n)
                .ToList()
                .ForEach(x =>
                {
                    expectedPrice += x * x;
                    var article = new Article(x, categories[x % 3], Guid.NewGuid().ToString(), x);
                    for (var i = 0; i < x; i++)
                        checkout.Scan(article);
                });
            var receipt = checkout.EmitReceipt();
            Assert.Equal(expectedPrice, receipt.Entries.Sum(x => x.Price));
        }

        [Fact]
        public void TaxOnCheckoutOfOnePerfume()
        {
            var checkout = new Checkout(new TaxEngine());
            var article = new Article(1, Category.Beauty, "Boss bottled", 112M);
            checkout.Scan(article);
            var receipt = checkout.EmitReceipt();
            Assert.NotEqual(article.Price, receipt.Entries.SingleOrDefault()?.Price);
        }

        [Theory]
        [InlineData(Category.Books)]
        [InlineData(Category.Food)]
        [InlineData(Category.Medical)]
        public void TaxesAreNotDueForExemptCategories(Category category)
        {
            var article = new Article(1, category, Guid.NewGuid().ToString(), 112M);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article);
            Assert.Equal(article.Price, tax.Apply(article.Price));
        }

        [Fact]
        public void TaxesAreDueForPerfumes()
        {
            var article = new Article(1, Category.Beauty, "Boss bottled", 112M);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article);
            Assert.NotEqual(article.Price, tax.Apply(article.Price));
        }

        [Fact]
        public void PurchaseApplyTax()
        {
            var article = new Article(1, Category.Beauty, "Boss bottled", 112M);
            const decimal taxRate = 10;
            var tax = new Tax(taxRate);
            var purchase = new Purchase();
            purchase.Add(article, tax);
            Assert.NotEqual(article.Price, purchase.Items.SingleOrDefault()?.Price);
        }

        [Fact]
        public void PurchaseGroupsByArticle()
        {
            var article = new Article(1, Category.Books, "Gone with the wind", 25.0M);
            var purchase = new Purchase();
            purchase.Add(article);
            purchase.Add(article);
            Assert.NotNull(purchase.Items.SingleOrDefault());
        }
    }
}