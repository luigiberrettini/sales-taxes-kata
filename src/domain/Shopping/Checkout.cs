using System.Linq;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Sales;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Checkout
    {
        private readonly Country _country;
        private readonly Catalog _catalog;
        private readonly TaxEngine _taxEngine;
        private readonly Purchase _currentPurchase;

        public Checkout(Country country, Catalog catalog, TaxEngine taxEngine)
        {
            _country = country;
            _taxEngine = taxEngine;
            _catalog = catalog;
            _currentPurchase = new Purchase(_country);
        }

        public void Scan(Good good)
        {
            var article = _catalog.Find(good.Name, good.IsImported, _country);
            Scan(article, good.Quantity);
        }

        public Receipt EmitReceipt()
        {
            return _currentPurchase.BuildReceipt();
        }

        private void Scan(Article article, int quantity)
        {
            _currentPurchase.Add(article, quantity, _taxEngine.TaxFor(article, _country));
        }
    }
}