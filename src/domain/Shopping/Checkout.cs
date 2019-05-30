using System.Linq;
using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Checkout
    {
        private readonly Country _country;
        private readonly Catalog.Catalog _catalog;
        private readonly TaxEngine _taxEngine;
        private readonly Purchase _currentPurchase;

        public Checkout(Country country, Catalog.Catalog catalog, TaxEngine taxEngine)
        {
            _country = country;
            _taxEngine = taxEngine;
            _catalog = catalog;
            _currentPurchase = new Purchase(_country);
        }

        public void Scan(Good good)
        {
            var article = _catalog.Find(good.Name);
            Enumerable
                .Range(1, good.Quantity)
                .ToList()
                .ForEach(x => Scan(article));
        }

        public void Scan(Article article)
        {
            _currentPurchase.Add(article, _taxEngine.TaxFor(article, _country));
        }

        public Receipt EmitReceipt()
        {
            return _currentPurchase.BuildReceipt();
        }
    }
}