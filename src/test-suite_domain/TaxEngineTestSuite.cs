using System;
using SalesTaxesKata.Domain;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class TaxEngineTestSuite
    {
        [Theory]
        [InlineData(Category.Books)]
        [InlineData(Category.Food)]
        [InlineData(Category.Medical)]
        public void NoTaxesForLocalExemptArticles(Category category)
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
        public void BasicTaxForLocalNonExemptArticles(Category category)
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, supplier.Country);
            Assert.Equal(new BasicTax().ApplyTo(article.Price), tax.ApplyTo(article.Price));
        }

        [Theory]
        [InlineData(Category.Books)]
        [InlineData(Category.Food)]
        [InlineData(Category.Medical)]
        public void ImportDutyForImportedExemptArticles(Category category)
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, Country.Usa);
            Assert.Equal(new ImportDuty().ApplyTo(article.Price), tax.ApplyTo(article.Price));
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
        public void BasicTaxAndImportDutyForImportedNonExemptArticles(Category category)
        {
            var supplier = new Supplier("VAT number", "Name", Country.Ita);
            var article = new Article(1, supplier, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, Country.Usa);
            Assert.Equal(new BasicTaxAndImportDuty().ApplyTo(article.Price), tax.ApplyTo(article.Price));
        }
    }
}