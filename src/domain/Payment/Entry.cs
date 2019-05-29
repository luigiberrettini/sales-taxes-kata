namespace SalesTaxesKata.Domain.Payment
{
    public class Entry
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPriceWithTaxes { get; set; }

        public Entry(string name, int quantity, decimal totalPriceWithTaxes)
        {
            Name = name;
            Quantity = quantity;
            TotalPriceWithTaxes = totalPriceWithTaxes;
        }
    }
}