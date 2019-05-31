using SalesTaxesKata.Domain.Shopping;

namespace SalesTaxesKata.Domain.Payment
{
    public struct Entry
    {
        public string Name { get; }

        public int Quantity { get; }

        public decimal TotalPriceWithTaxes { get; }

        public Entry(Item item)
        {
            Name = item.IsImported ? $"imported {item.Name}" : item.Name;
            Quantity = item.Quantity;
            TotalPriceWithTaxes = item.UnitPriceAfterTaxes * item.Quantity;
        }

        public override string ToString()
        {
            return $"{Quantity} {Name}: {TotalPriceWithTaxes:F2}";
        }
    }
}