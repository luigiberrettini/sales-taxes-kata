﻿using System;
using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Extensions;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Shopping;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class CheckoutTestSet
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void NoTaxesOnCheckoutForLocalBasicTaxExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = checkoutCountry;
            var categories = new[] { Category.Books, Category.Food, Category.Medical };
            var expectedTax = new NoBasicTax();
            CheckReceiptTaxedPrice(n, categories, supplierCountry, checkoutCountry, expectedTax);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void BasicTaxOnCheckoutForLocalNonBasicTaxExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = checkoutCountry;
            var exemptCats = new[] { Category.Books, Category.Food, Category.Medical };
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().Except(exemptCats).ToList();
            var expectedTax = new BasicTax();
            CheckReceiptTaxedPrice(n, categories, supplierCountry, checkoutCountry, expectedTax);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void ImportDutyOnCheckoutForImportedBasicTaxExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = Country.Usa;
            var categories = new[] { Category.Books, Category.Food, Category.Medical };
            var expectedTax = new ImportDuty();
            CheckReceiptTaxedPrice(n, categories, supplierCountry, checkoutCountry, expectedTax);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void BasicTaxAndImportDutyOnCheckoutForImportedNonBasicTaxExemptArticles(int n)
        {
            var checkoutCountry = Country.Ita;
            var supplierCountry = Country.Usa;
            var exemptCats = new[] { Category.Books, Category.Food, Category.Medical };
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().Except(exemptCats).ToList();
            var expectedTax = new BasicTaxAndImportDuty();
            CheckReceiptTaxedPrice(n, categories, supplierCountry, checkoutCountry, expectedTax);
        }

        [Fact]
        public void CheckoutRetrievesArticleFromGood()
        {
            var article = new Article(1, Country.Ita, Category.Books, Guid.NewGuid().ToString(), 10.37M);
            var catalog = new Catalog();
            catalog.TryAdd(article);
            var good = new Good(article.Name, 11, 1.23M, Origin.Local);
            var checkout = new Checkout(article.SupplierCountry, catalog, new TaxEngine());
            checkout.Scan(good);
            var receipt = checkout.EmitReceipt();
            var entry = receipt.Entries.Single();
            Assert.Equal(good.Name, entry.Description);
            Assert.Equal(good.Quantity, entry.Quantity);
            Assert.Equal(article.Price * good.Quantity, entry.TotalPriceWithTaxes);
        }

        private static void CheckReceiptTaxedPrice(int n, IReadOnlyList<Category> categories, Country supplierCountry, Country checkoutCountry, Tax expectedTax)
        {
            var catalog = new Catalog();
            var checkout = new Checkout(checkoutCountry, catalog, new TaxEngine());
            decimal nonTaxedPrice = 0;
            Enumerable.Range(1, n)
                .ForEach(x =>
                {
                    var category = categories[x % categories.Count];
                    var name = Guid.NewGuid().ToString();
                    var price = x;
                    var article = new Article(x, supplierCountry, category, name, price);
                    catalog.TryAdd(article);
                    var quantity = x;
                    var origin = checkoutCountry == supplierCountry ? Origin.Local : Origin.Imported;
                    var good = new Good(article.Name, quantity, price, origin);
                    checkout.Scan(good);
                    nonTaxedPrice += x * x;
                });
            Assert.Equal(expectedTax.ApplyTo(nonTaxedPrice), checkout.EmitReceipt().Total);
        }
    }
}