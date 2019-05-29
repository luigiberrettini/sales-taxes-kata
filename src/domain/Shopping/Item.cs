using System.Collections.Generic;
using SalesTaxesKata.Domain.Catalog;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Item
    {
        public int Id { get; }

        public string Name { get; }

        public decimal UnitPriceBeforeTaxes { get; }

        public decimal UnitPriceAfterTaxes { get; }

        public int Quantity { get; private set; }

        public Item(Article article, decimal unitPriceAfterTaxes)
        {
            Id = article.Id;
            Name = article.Name;
            UnitPriceBeforeTaxes = article.Price;
            UnitPriceAfterTaxes = unitPriceAfterTaxes;
            Quantity = 1;
        }

        public void IncreaseQuantity() => Quantity++;
    }
}