using SalesTaxesKata.Domain.Shopping;

namespace SalesTaxesKata.Domain.Payment
{
    public struct Entry
    {
        public int Quantity { get; }

        public string Description { get; }

        public decimal TotalPriceWithTaxes { get; }

        public Entry(Item item)
        {
            Description = item.IsImported ? $"imported {item.Name}" : item.Name;
            Quantity = item.Quantity;
            TotalPriceWithTaxes = item.TotalPriceAfterTaxes;
        }

        public override string ToString()
        {
            return $"{Quantity} {Description}: {TotalPriceWithTaxes:F2}";
        }
    }
}