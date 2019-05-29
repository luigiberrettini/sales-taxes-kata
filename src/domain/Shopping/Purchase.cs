using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Purchase
    {
        private readonly IDictionary<int, Item> _itemsByArticleId = new Dictionary<int, Item>();

        public void Add(Article article, Tax tax)
        {
            if (_itemsByArticleId.ContainsKey(article.Id))
                _itemsByArticleId[article.Id].IncreaseQuantity();
            else
                _itemsByArticleId[article.Id] = new Item(article, tax);
        }

        public Receipt BuildReceipt()
        {
            var receipt = new Receipt();
            var items = _itemsByArticleId.Values.ToList();
            items.ForEach(receipt.Add);
            return receipt;
        }
    }
}