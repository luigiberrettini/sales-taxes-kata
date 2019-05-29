using System.Collections.Generic;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Item
    {
        public int Id { get; }

        public string Name { get; }

        public decimal UnitPriceBeforeTaxes { get; }

        public decimal UnitPriceAfterTaxes { get; }

        public int Quantity { get; private set; }

        public Item(Article article, Country country, Tax tax)
        {
            Id = article.Id;
            Name = article.Name;
            UnitPriceBeforeTaxes = article.Price;
            UnitPriceAfterTaxes = tax.ApplyTo(article.Price);
            Quantity = 1;
        }

        public void IncreaseQuantity() => Quantity++;
    }
}