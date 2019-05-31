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
            catalog.Add(new Article(1, Country.Usa, Category.Food, "box of chocolate", 10M));
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


        private static Receipt Receipt(string input, Country saleCountry, Catalog catalog)
        {
            var basket = Basket.FromString(input);
            var checkout = new Checkout(saleCountry, catalog, new TaxEngine());
            basket.Goods.ForEach(checkout.Scan);
            return checkout.EmitReceipt();
        }
    }
}