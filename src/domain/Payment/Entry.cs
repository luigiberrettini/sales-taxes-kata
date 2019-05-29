namespace SalesTaxesKata.Domain.Payment
{
    public class Entry
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
    }
}