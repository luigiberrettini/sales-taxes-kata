using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Checkout
    {
        private readonly Country _country;
        private readonly TaxEngine _taxEngine;
        private readonly Purchase _currentPurchase = new Purchase();

        public Checkout(Country country, TaxEngine taxEngine)
        {
            _country = country;
            _taxEngine = taxEngine;
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