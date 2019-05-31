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

        [Fact]
        public void ReceiptWithManyProducts()
        {
            var catalog = new Catalog();
            catalog.Add(new Article(1, Country.Ita, Category.Books, "book", 12.49M));
            catalog.Add(new Article(2, Country.Ita, Category.Food, "chocolate bar", 0.85M));
            catalog.Add(new Article(3, Country.Ita, Category.Medical, "packet of headache pills", 9.75M));
            catalog.Add(new Article(4, Country.Abw, Category.Books, "book", 22.49M));
            catalog.Add(new Article(5, Country.Afg, Category.Food, "chocolate bar", 4.98M));
            catalog.Add(new Article(6, Country.Ago, Category.Medical, "packet of headache pills", 7.53M));
            catalog.Add(new Article(7, Country.Ita, Category.Computers, "monitor 16:9 1920x1080", 459.72M));
            catalog.Add(new Article(8, Country.Ita, Category.Fashion, "blazer season spring/summer 2020", 847.83M));
            catalog.Add(new Article(9, Country.Ita, Category.Movies, "Gone with the wind", 13.2M));
            catalog.Add(new Article(10, Country.Bdi, Category.Computers, "monitor 16:9 1920x1080", 322.01M));
            catalog.Add(new Article(11, Country.Caf, Category.Fashion, "blazer season spring/summer 2020", 1230.48M));
            catalog.Add(new Article(12, Country.Deu, Category.Movies, "Gone with the wind", 31.07M));

            var sb = new StringBuilder();
            sb.AppendLine("1 book at 12.49");
            sb.AppendLine("2 chocolate bar at 0.85");
            sb.AppendLine("3 packet of headache pills at 9.75");
            sb.AppendLine("4 imported book at 22.49");
            sb.AppendLine("5 chocolate bar imported at 4.98");
            sb.AppendLine("6 packet of imported headache pills at 7.53");
            sb.AppendLine("6 monitor 16:9 1920x1080 at 459.72");
            sb.AppendLine("5 blazer season spring/summer 2020 at 847.83");
            sb.AppendLine("4 Gone with the wind at 13.2");
            sb.AppendLine("3 Imported monitor 16:9 1920x1080 at 322.01");
            sb.AppendLine("2 blazer (IMPORTED) season spring/summer 2020 at 1230.48");
            sb.AppendLine("1 Gone with the wind [imported from US] at 31.07");
            var input = sb.ToString();

            sb.Clear();
            sb.AppendLine("1 book: 12.49");
            sb.AppendLine("2 chocolate bar: 1.70");
            sb.AppendLine("3 packet of headache pills: 29.25");
            sb.AppendLine("4 imported book: 94.46");
            sb.AppendLine("5 chocolate bar imported: 26.15");
            sb.AppendLine("6 packet of imported headache pills: 47.48");
            sb.AppendLine("6 monitor 16:9 1920x1080: 3034.17");
            sb.AppendLine("5 blazer season spring/summer 2020: 4663.10");
            sb.AppendLine("4 Gone with the wind: 58.10");
            sb.AppendLine("3 Imported monitor 16:9 1920x1080: 1110.98");
            sb.AppendLine("2 blazer (IMPORTED) season spring/summer 2020: 2830.11");
            sb.AppendLine("1 Gone with the wind [imported from US]: 35.77");
            sb.AppendLine("Sales Taxes: 1231.95");
            sb.AppendLine("Total: 11943.76");
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