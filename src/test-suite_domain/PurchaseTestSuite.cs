using System;
using System.Linq;
using System.Text;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Sales;
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
            var article = new Article(1, Country.Ita, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var tax = new BasicTax();
            var purchase = new Purchase(article.SupplierCountry);
            purchase.Add(article, 1, tax);
            var receipt = purchase.BuildReceipt();
            Assert.NotEqual(article.Price, receipt.Total);
        }

        [Fact]
        public void ReceiptGroupsByArticle()
        {
            var article = new Article(1, Country.Ita, Category.ArtsAndCrafts, Guid.NewGuid().ToString(), 100);
            var tax = new NoTax();
            var purchase = new Purchase(article.SupplierCountry);
            purchase.Add(article, 2, tax);
            var receipt = purchase.BuildReceipt();
            Assert.Single(receipt.Entries);
        }

        [Fact]
        public void ReceiptForOneArticleMatchesItsData()
        {
            var articleName = Guid.NewGuid().ToString();
            var article = new Article(1, Country.Ita, Category.ArtsAndCrafts, articleName, 100);
            var purchase = new Purchase(article.SupplierCountry);
            var tax = new BasicTax();
            purchase.Add(article, 1, tax);
            var receipt = purchase.BuildReceipt();
            var entry = receipt.Entries.Single();

            const int quantity = 1;
            var priceWithTaxes = tax.ApplyTo(article.Price);
            Assert.Equal(articleName, entry.Name);
            Assert.Equal(quantity, entry.Quantity);
            Assert.Equal(priceWithTaxes, entry.TotalPriceWithTaxes);
            Assert.Equal(priceWithTaxes - article.Price, receipt.Taxes);
            Assert.Equal(priceWithTaxes, receipt.Total);
        }

        [Fact]
        public void EntryToStringFormatIsQuantityImportedIfApplicableNameColonPrice()
        {
            const string articleName = "Article ABC";
            const decimal articlePrice = 32.54M;
            const int quantity = 2;
            const decimal totalPriceWithTaxes = quantity * articlePrice;
            var article = new Article(1, Country.Usa, Category.ArtsAndCrafts, articleName, articlePrice);
            var tax = new NoTax();

            var localPurchase = new Purchase(Country.Usa);
            localPurchase.Add(article, quantity, tax);
            var localReceipt = localPurchase.BuildReceipt();

            var importedPurchase = new Purchase(Country.Ita);
            importedPurchase.Add(article, quantity, tax);
            var importedReceipt = importedPurchase.BuildReceipt();

            Assert.Equal($"{quantity} {articleName}: {totalPriceWithTaxes}", localReceipt.Entries.Single().ToString());
            Assert.Equal($"{quantity} imported {articleName}: {totalPriceWithTaxes}", importedReceipt.Entries.Single().ToString());
        }

        [Fact]
        public void ReceiptToStringHasEntriesSalesTaxesAndTotal()
        {
            const string article1Name = "Article ABC";
            const decimal article1Price = 32.54M;
            const string article2Name = "Article ABC";
            const decimal article2Price = 32.54M;
            var article1 = new Article(1, Country.Usa, Category.ArtsAndCrafts, article1Name, article1Price);
            var article2 = new Article(1, Country.Usa, Category.Baby, article2Name, article2Price);
            var receipt = new Receipt();
            var noTax = new NoTax();
            var item1 = new Item(Country.Ita, article1, 1, noTax);
            var item2 = new Item(Country.Ita, article2, 1, noTax);
            receipt.Add(item1);
            receipt.Add(item2);
            var entry1 = new Entry(item1);
            var entry2 = new Entry(item2);
            var sb = new StringBuilder();
            sb.AppendLine(entry1.ToString());
            sb.AppendLine(entry2.ToString());
            sb.AppendLine($"Sales Taxes: {receipt.Taxes}");
            sb.AppendLine($"Total: {receipt.Total}");
            Assert.Equal(sb.ToString(), receipt.ToString());

        }
    }
}