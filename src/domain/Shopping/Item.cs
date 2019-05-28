using SalesTaxesKata.Domain.Catalog;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Item
    {
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public Item(Article article)
        {
            Price = article.Price;
            Quantity = 1;
        }
    }
}