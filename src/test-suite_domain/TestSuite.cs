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
            Assert.Equal(article.Price, tax.ApplyTo(article.Price));
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
            Assert.NotEqual(article.Price, tax.ApplyTo(article.Price));
        }

        [Fact]
        public void OnlyImportDutyIsDueForImportedBooks()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.Books, Guid.NewGuid().ToString(), 100);
            var saleCountry = Country.Usa;
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, saleCountry);
            Assert.Equal(105, tax.ApplyTo(article.Price));
        }

        [Fact]
        public void BasicTaxAndImportDutyAreDueForImportedComputers()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.Computers, Guid.NewGuid().ToString(), 100);
            var saleCountry = Country.Usa;
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, saleCountry);
            Assert.Equal(115, tax.ApplyTo(article.Price));
        }


        [Fact]
        public void PurchaseApplyTax()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var tax = new BasicTax();
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
            var basicTax = new BasicTax();
            Assert.Equal(2, basicTax.ApplyTo(1.8M));
            Assert.Equal(1.8M, basicTax.ApplyTo(1.6M));
            Assert.Equal(1.65M, basicTax.ApplyTo(1.5M));
            Assert.Equal(1.55M, basicTax.ApplyTo(1.4M));
            Assert.Equal(1.25M, basicTax.ApplyTo(1.1M));
            Assert.Equal(1.1M, basicTax.ApplyTo(1));

            var importDuty = new ImportDuty();
            Assert.Equal(1.9M, importDuty.ApplyTo(1.8M));
            Assert.Equal(1.3M, importDuty.ApplyTo(1.2M));
            Assert.Equal(1.05M, importDuty.ApplyTo(1));
            Assert.Equal(0.85M, importDuty.ApplyTo(0.8M));
            Assert.Equal(0.25M, importDuty.ApplyTo(0.2M));
            Assert.Equal(21, importDuty.ApplyTo(20));

            var basicTaxAndImportDuty = new BasicTaxAndImportDuty();
            Assert.Equal(1.9M, basicTaxAndImportDuty.ApplyTo(1.65M));
            Assert.Equal(1.8M, basicTaxAndImportDuty.ApplyTo(1.53M));
            Assert.Equal(3.45M, basicTaxAndImportDuty.ApplyTo(3M));
            Assert.Equal(1.65M, basicTaxAndImportDuty.ApplyTo(1.43M));
            Assert.Equal(1.55M, basicTaxAndImportDuty.ApplyTo(1.31M));
            Assert.Equal(2.3M, basicTaxAndImportDuty.ApplyTo(2));
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