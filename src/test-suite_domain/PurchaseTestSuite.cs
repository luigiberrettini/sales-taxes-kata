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
            var purchase = new Purchase(supplier.Country);
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
            var purchase = new Purchase(supplier.Country);
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
            var purchase = new Purchase(supplier.Country);
            var tax = new BasicTax();
            purchase.Add(article, tax);
            var receipt = purchase.BuildReceipt();
            var entry = receipt.Entries.Single();

            const int quantity = 1;
            var priceWithTaxes = tax.ApplyTo(article.Price);
            Assert.Equal(articleName, entry.Name);
            Assert.Equal(quantity, entry.Quantity);
            Assert.Equal(priceWithTaxes, entry.TotalPriceWithTaxes);
            Assert.Equal(priceWithTaxes - article.Price, receipt.SalesTaxes);
            Assert.Equal(priceWithTaxes, receipt.Total);
        }

        [Fact]
        public void EntryToStringFormatIsQuantityImportedIfApplicableNameColonPrice()
        {
            var supplier = new Supplier("VAT number", "Name", Country.Usa);
            const string articleName = "Article ABC";
            const decimal articlePrice = 32.54M;
            var article = new Article(1, supplier, Category.ArtsAndCrafts, articleName, articlePrice);
            var tax = new NoTax();

            var localPurchase = new Purchase(Country.Usa);
            localPurchase.Add(article, tax);
            localPurchase.Add(article, tax);
            var localReceipt = localPurchase.BuildReceipt();

            var importedPurchase = new Purchase(Country.Ita);
            importedPurchase.Add(article, tax);
            importedPurchase.Add(article, tax);
            var importedReceipt = importedPurchase.BuildReceipt();

            const int quantity = 2;
            const decimal totalPriceWithTaxes = quantity * articlePrice;
            Assert.Equal($"{quantity} {articleName}: {totalPriceWithTaxes}", localReceipt.Entries.Single().ToString());
            Assert.Equal($"{quantity} imported {articleName}: {totalPriceWithTaxes}", importedReceipt.Entries.Single().ToString());
        }
    }
}