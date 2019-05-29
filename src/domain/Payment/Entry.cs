namespace SalesTaxesKata.Domain.Payment
{
    public struct Entry
    {
        public string Name { get; }

        public int Quantity { get; }

        public decimal TotalPriceWithTaxes { get; }

        public Entry(string name, int quantity, decimal totalPriceWithTaxes)
        {
            Name = name;
            Quantity = quantity;
            TotalPriceWithTaxes = totalPriceWithTaxes;
        }

        public override string ToString()
        {
            return $"{Quantity} {Name}: {TotalPriceWithTaxes}";
        }
    }
}