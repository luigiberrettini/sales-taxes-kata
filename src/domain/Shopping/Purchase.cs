using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Purchase
    {
        private readonly IDictionary<int, Item> _items = new Dictionary<int, Item>();

        public IEnumerable<Item> Items => _items.Values;

        public void Add(Article article, Tax tax)
        {
            if (_items.ContainsKey(article.Id))
            {
                _items[article.Id].Quantity++;
                return;
            }

            var item = new Item(article);
            item.Price = tax.Apply(item.Price);
            _items[article.Id] = item;
        }

        public void Add(Article article)
        {
            if (_items.ContainsKey(article.Id))
                _items[article.Id].Quantity++;
            else
                _items[article.Id] = new Item(article);
        }

        public Receipt BuildReceipt()
        {
            var receipt = new Receipt();
            Items.ToList().ForEach(x => receipt.Add(new Entry(x.Price * x.Quantity)));
            return receipt;
        }
    }
}