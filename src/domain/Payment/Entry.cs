namespace SalesTaxesKata.Domain.Payment
{
    public class Entry
    {
        public decimal Price { get; set; }

        public Entry(decimal price)
        {
            Price = price;
        }
    }
}