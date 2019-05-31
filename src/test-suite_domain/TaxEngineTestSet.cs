using System;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class TaxEngineTestSet
    {
        [Theory]
        [InlineData(Category.Books)]
        [InlineData(Category.Food)]
        [InlineData(Category.Medical)]
        public void NoTaxesForLocalBasicTaxExemptArticles(Category category)
        {
            var article = new Article(1, Country.Ita, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, article.SupplierCountry);
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
        public void BasicTaxForLocalNonBasicTaxExemptArticles(Category category)
        {
            var article = new Article(1, Country.Ita, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, article.SupplierCountry);
            Assert.Equal(new BasicTax().ApplyTo(article.Price), tax.ApplyTo(article.Price));
        }

        [Theory]
        [InlineData(Category.Books)]
        [InlineData(Category.Food)]
        [InlineData(Category.Medical)]
        public void ImportDutyForImportedBasicTaxExemptArticles(Category category)
        {
            var article = new Article(1, Country.Ita, category, Guid.NewGuid().ToString(), 100);
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
        public void BasicTaxAndImportDutyForImportedNonBasicTaxExemptArticles(Category category)
        {
            var article = new Article(1, Country.Ita, category, Guid.NewGuid().ToString(), 100);
            var taxEngine = new TaxEngine();
            var tax = taxEngine.TaxFor(article, Country.Usa);
            Assert.Equal(new BasicTaxAndImportDuty().ApplyTo(article.Price), tax.ApplyTo(article.Price));
        }
    }
}