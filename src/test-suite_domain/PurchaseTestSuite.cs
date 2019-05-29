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
            var taxedPrice = tax.ApplyTo(article.Price);
            var expectedEntry = new Entry(articleName, quantity, taxedPrice);
            Assert.Equal(expectedEntry.Quantity, entry.Quantity);
            Assert.Equal(expectedEntry.Name, entry.Name);
            Assert.Equal(expectedEntry.Price, entry.Price);
            Assert.Equal(expectedEntry.Price, receipt.SalesTaxes);
            Assert.Equal(expectedEntry.Price, receipt.Total);
        }
    }
}