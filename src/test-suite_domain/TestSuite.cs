using System;
using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Shopping;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
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
            var checkout = new Checkout(Country.Ita, new TaxEngine());
            var categories = new[] { Category.Books, Category.Food, Category.Medical };
            var expectedPrice = ScanArticles(n, checkout, categories, price => price);
            var receipt = checkout.EmitReceipt();
            Assert.Equal(expectedPrice, receipt.Entries.Sum(x => x.Price));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void TaxOnCheckoutForNonExemptCategories(int n)
        {
            var checkout = new Checkout(Country.Ita, new TaxEngine());
            var exemptCats = new[] { Category.Books, Category.Food, Category.Medical };
            var nonExemptCats = Enum.GetValues(typeof(Category)).Cast<Category>().Except(exemptCats).ToList();
            var expectedPrice = ScanArticles(n, checkout, nonExemptCats, price => price * 1.1M);
            var receipt = checkout.EmitReceipt();
            Assert.Equal(expectedPrice, receipt.Entries.Sum(x => x.Price));
        }

        [Theory]
        [InlineData(Category.Books)]
        [InlineData(Category.Food)]
        [InlineData(Category.Medical)]
        public void TaxesAreNotDueForExemptCategories(Category category)
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, supplier.Country);
            Assert.Equal(article.Price, tax.Apply(article.Price));
        }

        [Theory]
        [InlineData(Category.ArtsAndCrafts)]
        [InlineData(Category.Automotive)]
        [InlineData(Category.Baby)]
        [InlineData(Category.Beauty)]
        [InlineData(Category.Computers)]
        [InlineData(Category.Electronics)]
        [InlineData(Category.Fashion)]
        [InlineData(Category.Health)]
        [InlineData(Category.Home)]
        [InlineData(Category.Household)]
        [InlineData(Category.Luggage)]
        [InlineData(Category.Movies)]
        [InlineData(Category.Music)]
        [InlineData(Category.PersonalCare)]
        [InlineData(Category.Pets)]
        [InlineData(Category.Software)]
        [InlineData(Category.Sports)]
        [InlineData(Category.Toys)]
        [InlineData(Category.VideoGames)]
        public void TaxesAreDueForNonExemptCategories(Category category)
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, Country.Usa);
            Assert.NotEqual(article.Price, tax.Apply(article.Price));
        }

        [Fact]
        public void OnlyImportDutyIsDueForImportedBooks()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.Books, Guid.NewGuid().ToString(), 100);
            var saleCountry = Country.Usa;
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, saleCountry);
            Assert.Equal(105, tax.Apply(article.Price));
        }

        [Fact]
        public void BasicTaxAndImportDutyAreDueForImportedComputers()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var saleCountry = Country.Usa;
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, saleCountry);
            Assert.Equal(115, tax.Apply(article.Price));
        }


        [Fact]
        public void PurchaseApplyTax()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            const decimal taxRate = 10;
            var tax = new Tax(taxRate);
            var purchase = new Purchase();
            purchase.Add(article, tax);
            Assert.NotEqual(article.Price, purchase.Items.SingleOrDefault()?.Price);
        }

        [Fact]
        public void PurchaseGroupsByArticle()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var purchase = new Purchase();
            purchase.Add(article);
            purchase.Add(article);
            Assert.NotNull(purchase.Items.SingleOrDefault());
        }

        [Fact]
        public void TaxesAreRoundedUpToNearestFiveCents()
        {
            const decimal taxRate = 10;
            var tax = new Tax(taxRate);

            Assert.Equal(198, tax.Apply(180));
            Assert.Equal(2, tax.Apply(1.8M));

            Assert.Equal(176, tax.Apply(160));
            Assert.Equal(1.8M, tax.Apply(1.6M));

            Assert.Equal(165, tax.Apply(150));
            Assert.Equal(1.65M, tax.Apply(1.5M));

            Assert.Equal(154, tax.Apply(140));
            Assert.Equal(1.55M, tax.Apply(1.4M));

            Assert.Equal(121, tax.Apply(110));
            Assert.Equal(1.25M, tax.Apply(1.1M));

            Assert.Equal(110, tax.Apply(100));
            Assert.Equal(1.1M, tax.Apply(1));
        }

        private static decimal ScanArticles(int n, Checkout checkout, IReadOnlyList<Category> categories, Func<decimal, decimal> applyTax)
        {
            decimal expectedPrice = 0;
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            Enumerable.Range(1, n)
                .ToList()
                .ForEach(x =>
                {
                    expectedPrice += x * applyTax(x);
                    var article = new Article(x, supplier, categories[x % categories.Count], Guid.NewGuid().ToString(), x);
                    for (var i = 0; i < x; i++)
                        checkout.Scan(article);
                });
            return expectedPrice;
        }
    }
}