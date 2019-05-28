using SalesTaxesKata.Domain.Catalog;
using SalesTaxesKata.Domain.Payment;
using SalesTaxesKata.Domain.Taxation;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Checkout
    {
        private readonly TaxEngine _taxEngine;
        private readonly Purchase _currentPurchase = new Purchase();

        public Checkout(TaxEngine taxEngine)
        {
            _taxEngine = taxEngine;
        }

        public void Scan(Article article)
        {
            _currentPurchase.Add(article, _taxEngine.TaxFor(article));
        }

        public Receipt EmitReceipt()
        {
            return _currentPurchase.BuildReceipt();
        }
    }
}