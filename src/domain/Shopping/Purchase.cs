using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Payment;
using System.Collections.Generic;
using System.Linq;

namespace SalesTaxes.Domain.Shopping
{
    public class Purchase
    {
        private readonly IDictionary<int, Item> _items = new Dictionary<int, Item>();

        public IEnumerable<Item> Items => _items.Values;

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