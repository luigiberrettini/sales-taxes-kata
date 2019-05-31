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
        private Purchase _currentPurchase;

        public Checkout(Country country, Catalog catalog, TaxEngine taxEngine)
        {
            _country = country;
            _taxEngine = taxEngine;
            _catalog = catalog;
            _currentPurchase = new Purchase(_country);
        }

        public void Scan(Good good)
        {
            var article = _catalog.Find(good.Name, good.Origin, _country);
            Scan(article, good.Quantity);
        }

        public Receipt EmitReceipt()
        {
            var receipt = _currentPurchase.BuildReceipt();
            _currentPurchase = new Purchase(_country);
            return receipt;
        }

        private void Scan(Article article, int quantity)
        {
            _currentPurchase.Add(article, quantity, _taxEngine.TaxFor(article, _country));
        }
    }
}