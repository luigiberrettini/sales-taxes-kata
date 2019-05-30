using System;
using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Shopping;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class CheckoutTestSuite
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void NoTaxesOnCheckoutForLocalExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = checkoutCountry;
            var categories = new[] { Category.Books, Category.Food, Category.Medical };
            var expectedTax = new NoTax();
            var catalog = new Catalog();
            var checkout = new Checkout(checkoutCountry, catalog, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, catalog, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.TotalPriceWithTaxes));

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void BasicTaxOnCheckoutForLocalNonExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = checkoutCountry;
            var exemptCats = new[] { Category.Books, Category.Food, Category.Medical };
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().Except(exemptCats).ToList();
            var expectedTax = new BasicTax();
            var catalog = new Catalog();
            var checkout = new Checkout(checkoutCountry, catalog, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, catalog, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.TotalPriceWithTaxes));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void ImportDutyOnCheckoutForImportedExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = Country.Usa;
            var categories = new[] { Category.Books, Category.Food, Category.Medical };
            var expectedTax = new ImportDuty();
            var catalog = new Catalog();
            var checkout = new Checkout(checkoutCountry, catalog, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, catalog, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.TotalPriceWithTaxes));

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void BasicTaxAndImportDutyOnCheckoutForImportedNonExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = Country.Usa;
            var exemptCats = new[] { Category.Books, Category.Food, Category.Medical };
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().Except(exemptCats).ToList();
            var expectedTax = new BasicTaxAndImportDuty();
            var catalog = new Catalog();
            var checkout = new Checkout(checkoutCountry, catalog, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, catalog, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.TotalPriceWithTaxes));
        }

        [Fact]
        public void CheckoutRetrievesArticleFromGood()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.Books, Guid.NewGuid().ToString(), 10.37M);
            var catalog = new Catalog();
            catalog.Add(article);
            var good = new Good(article.Name, 11, article.Price);
            var checkout = new Checkout(supplier.Country, catalog, new TaxEngine());
            checkout.Scan(good);
            var receipt = checkout.EmitReceipt();
            var entry = receipt.Entries.Single();
            Assert.Equal(good.Name, entry.Name);
            Assert.Equal(good.Quantity, entry.Quantity);
            Assert.Equal(good.ShelfPrice * good.Quantity, entry.TotalPriceWithTaxes);
        }

        private static decimal NonTaxedPriceOnNArticles(int n, Catalog catalog, Checkout checkout, Supplier supplier,
            IReadOnlyList<Category> categories)
        {
            decimal nonTaxedPrice = 0;
            Enumerable.Range(1, n)
                .ToList()
                .ForEach(x =>
                {
                    var price = x;
                    var article = new Article(x, supplier, categories[x % categories.Count], Guid.NewGuid().ToString(), price);
                    catalog.Add(article);
                    var quantity = x;
                    var good = new Good(article.Name, quantity, price);
                    checkout.Scan(good);
                    nonTaxedPrice += x * x;
                });
            return nonTaxedPrice;
        }
    }
}