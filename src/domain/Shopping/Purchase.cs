using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Purchase
    {
        private readonly Country _country;
        private readonly IDictionary<int, Item> _itemsByArticleId;

        public Purchase(Country country)
        {
            _country = country;
            _itemsByArticleId = new Dictionary<int, Item>();
        }

        public void Add(Article article, int quantity, Tax tax)
        {
            if (_itemsByArticleId.ContainsKey(article.Id))
            {
                _itemsByArticleId[article.Id].IncreaseQuantity(quantity);
                return;
            }
            _itemsByArticleId[article.Id] = new Item(_country, article, quantity, tax);
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