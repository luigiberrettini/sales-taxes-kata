using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Item
    {
        private readonly Tax _tax;

        public int Id { get; }

        public string Name { get; }

        public decimal UnitPriceBeforeTaxes { get; }

        public decimal TotalPriceAfterTaxes { get; private set; }

        public int Quantity { get; private set; }

        public bool IsImported { get; }

        public Item(Country saleCountry, Article article, int quantity, Tax tax)
        {
            _tax = tax;
            Id = article.Id;
            Name = article.Name;
            UnitPriceBeforeTaxes = article.Price;
            TotalPriceAfterTaxes = _tax.ApplyTo(quantity * article.Price);
            Quantity = quantity;
            IsImported = saleCountry != article.SupplierCountry;
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
            TotalPriceAfterTaxes = _tax.ApplyTo(Quantity * UnitPriceBeforeTaxes);
        }
    }
}