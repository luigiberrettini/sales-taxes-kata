using System.Text;
using SalesTaxesKata.Domain.Extensions;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Shopping;
using SalesTaxesKata.Domain.Taxation;
using Xunit;

namespace SalesTaxesKata.TestSuite.Domain
{
    public class AcceptanceTestSuite
    {
        [Fact]
        public void ReceiptForBookCdChocolate()
        {
            var catalog = new Catalog();
            catalog.Add(new Article(1, Country.Ita, Category.Books, "book", 12.49M));
            catalog.Add(new Article(2, Country.Ita, Category.Music, "music CD", 14.99M));
            catalog.Add(new Article(3, Country.Ita, Category.Food, "chocolate bar", 0.85M));

            var sb = new StringBuilder();
            sb.AppendLine("1 book at 12.49");
            sb.AppendLine("1 music CD at 14.99");
            sb.AppendLine("1 chocolate bar at 0.85");
            var input = sb.ToString();

            sb.Clear();
            sb.AppendLine("1 book: 12.49");
            sb.AppendLine("1 music CD: 16.49");
            sb.AppendLine("1 chocolate bar: 0.85");
            sb.AppendLine("Sales Taxes: 1.50");
            sb.AppendLine("Total: 29.83");
            var output = sb.ToString();

            Assert.Equal(output, Receipt(input, Country.Ita, catalog).ToString());
        }

        [Fact]
        public void ReceiptForImportedChocolateImportedPerfume()
        {
            var catalog = new Catalog();
            catalog.Add(new Article(1, Country.Usa, Category.Food, "box of chocolates", 10M));
            catalog.Add(new Article(2, Country.Usa, Category.Beauty, "bottle of perfume", 47.5M));

            var sb = new StringBuilder();
            sb.AppendLine("1 imported box of chocolates at 10.00");
            sb.AppendLine("1 imported bottle of perfume at 47.50");
            var input = sb.ToString();

            sb.Clear();
            sb.AppendLine("1 imported box of chocolates: 10.50");
            sb.AppendLine("1 imported bottle of perfume: 54.65");
            sb.AppendLine("Sales Taxes: 7.65");
            sb.AppendLine("Total: 65.15");
            var output = sb.ToString();

            Assert.Equal(output, Receipt(input, Country.Ita, catalog).ToString());
        }

        [Fact]
        public void ReceiptForImportedPerfumePerfumePillsImportedChocolate()
        {
            var catalog = new Catalog();
            catalog.Add(new Article(1, Country.Usa, Category.Beauty, "bottle of perfume", 27.99M));
            catalog.Add(new Article(2, Country.Ita, Category.Beauty, "bottle of perfume", 18.99M));
            catalog.Add(new Article(3, Country.Ita, Category.Medical, "packet of headache pills", 9.75M));
            catalog.Add(new Article(4, Country.Abw, Category.Food, "box of chocolates", 11.25M));

            var sb = new StringBuilder();
            sb.AppendLine("1 imported bottle of perfume at 27.99");
            sb.AppendLine("1 bottle of perfume at 18.99");
            sb.AppendLine("1 packet of headache pills at 9.75");
            sb.AppendLine("1 box of imported chocolates at 11.25");
            var input = sb.ToString();

            sb.Clear();
            sb.AppendLine("1 imported bottle of perfume: 32.19");
            sb.AppendLine("1 bottle of perfume: 20.89");
            sb.AppendLine("1 packet of headache pills: 9.75");
            sb.AppendLine("1 imported box of chocolates: 11.85");
            sb.AppendLine("Sales Taxes: 6.70");
            sb.AppendLine("Total: 74.68");
            var output = sb.ToString();

            Assert.Equal(output, Receipt(input, Country.Ita, catalog).ToString());
        }

        private static Receipt Receipt(string input, Country saleCountry, Catalog catalog)
        {
            var basket = Basket.FromString(input);
            var checkout = new Checkout(saleCountry, catalog, new TaxEngine());
            basket.Goods.ForEach(checkout.Scan);
            return checkout.EmitReceipt();
        }
    }
}