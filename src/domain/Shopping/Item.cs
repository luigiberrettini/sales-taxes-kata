using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;
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

        public bool IsImported { get; }

        public Item(Country saleCountry, Article article, Tax tax)
        {
            Id = article.Id;
            Name = article.Name;
            UnitPriceBeforeTaxes = article.Price;
            UnitPriceAfterTaxes = tax.ApplyTo(article.Price);
            Quantity = 1;
            IsImported = saleCountry != article.SupplierCountry;
        }

        public void IncreaseQuantity() => Quantity++;
    }
}