using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Payment;
using System.Collections.Generic;

namespace SalesTaxes.Domain.Shopping
{
    public class Purchase
    {
        private readonly IDictionary<Article, Item> _items = new Dictionary<Article, Item>();

        public IEnumerable<Item> Items => _items.Values;

        public void Add(Article article)
        {
            var item = new Item(article);
            if (_items.ContainsKey(article))
                _items[article].Quantity++;
            else
                _items[article] = new Item(article);
        }

        public Receipt BuildReceipt()
        {
            return new Receipt(this);
        }
    }
}