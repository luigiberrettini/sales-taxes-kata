using System;
using System.Linq;
using SalesTaxesKata.Domain;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Shopping;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class PurchaseTestSuite
    {
        [Fact]
        public void PurchaseAppliesTax()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var tax = new BasicTax();
            var purchase = new Purchase();
            purchase.Add(article, tax);
            var receipt = purchase.BuildReceipt();
            Assert.NotEqual(article.Price, receipt.Total);
        }

        [Fact]
        public void ReceiptGroupsByArticle()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var tax = new NoTax();
            var purchase = new Purchase();
            purchase.Add(article, tax);
            purchase.Add(article, tax);
            var receipt = purchase.BuildReceipt();
            Assert.Single(receipt.Entries);
        }

        [Fact]
        public void ReceiptForOneArticleMatchesItsData()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var articleName = Guid.NewGuid().ToString();
            var article = new Article(1, supplier, Category.ArtsAndCrafts, articleName, 100);
            var purchase = new Purchase();
            var tax = new BasicTax();
            purchase.Add(article, tax);
            var receipt = purchase.BuildReceipt();
            var entry = receipt.Entries.Single();

            const int quantity = 1;
            var priceWithTaxes = tax.ApplyTo(article.Price);
            var expectedEntry = new Entry(articleName, quantity, priceWithTaxes);
            Assert.Equal(expectedEntry, entry);
            Assert.Equal(priceWithTaxes - article.Price, receipt.SalesTaxes);
            Assert.Equal(expectedEntry.TotalPriceWithTaxes, receipt.Total);
        }

        [Fact]
        public void EntryToStringFormatIsQuantityNameColonPrice()
        {
            const string name = "Article ABC";
            const int quantity = 5;
            const decimal totalPriceWithTaxes = 32.54M;
            var entry = new Entry(name, quantity, totalPriceWithTaxes);
            Assert.Equal($"{quantity} {name}: {totalPriceWithTaxes}", entry.ToString());
        }
    }
}