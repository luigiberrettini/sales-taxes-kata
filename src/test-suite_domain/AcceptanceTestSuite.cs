using System;
using System.Collections.Generic;
using System.Text;
using SalesTaxesKata.Domain.Geo;
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

            var basket = Basket.FromString(input);
            var checkout = new Checkout(Country.Ita, catalog, new TaxEngine());
            basket.Goods.ForEach(checkout.Scan);
            var receipt = checkout.EmitReceipt();
            Assert.Equal(output, receipt.ToString());
        }
    }

    public static class EnumerableExtensionKit
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}