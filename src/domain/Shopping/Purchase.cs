using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Payment;
using System.Collections.Generic;
using System.Linq;

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
            var receipt = new Receipt();
            Items.ToList().ForEach(x => receipt.Add(new Entry(x.Price * x.Quantity)));
            return receipt;
        }
    }
}