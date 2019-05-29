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
            var checkout = new Checkout(checkoutCountry, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.Price));

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
            var checkout = new Checkout(checkoutCountry, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.Price));
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
            var checkout = new Checkout(checkoutCountry, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.Price));

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
            var checkout = new Checkout(checkoutCountry, new TaxEngine());
            var supplier = new Supplier("VAT number", "Name", supplierCountry);
            var nonTaxedPrice = NonTaxedPriceOnNArticles(n, checkout, supplier, categories);
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Entries.Sum(x => x.Price));
        }

        private static decimal NonTaxedPriceOnNArticles(int n, Checkout checkout, Supplier supplier, IReadOnlyList<Category> categories)
        {
            decimal nonTaxedPrice = 0;
            Enumerable.Range(1, n)
                .ToList()
                .ForEach(x =>
                {
                    var article = new Article(x, supplier, categories[x % categories.Count], Guid.NewGuid().ToString(), x);
                    for (var i = 0; i < x; i++)
                        checkout.Scan(article);
                    nonTaxedPrice += x * x;
                });
            return nonTaxedPrice;
        }
    }
}