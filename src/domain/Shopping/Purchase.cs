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
                _items[article.Id].IncreaseQuantity();
                return;
            }

            var unitPriceAfterTaxes = tax.ApplyTo(article.Price);
            var item = new Item(article, unitPriceAfterTaxes);
            _items[article.Id] = item;
        }

        public void Add(Article article)
        {
            if (_items.ContainsKey(article.Id))
                _items[article.Id].IncreaseQuantity();
            else
                _items[article.Id] = new Item(article, article.Price);
        }

        public Receipt BuildReceipt()
        {
            var receipt = new Receipt();
            Items.ToList().ForEach(receipt.Add);
            return receipt;
        }
    }
}