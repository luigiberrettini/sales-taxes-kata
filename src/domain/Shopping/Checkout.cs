using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Payment;
using SalesTaxes.Domain.Taxation;

namespace SalesTaxes.Domain.Shopping
{
    public class Checkout
    {
        private readonly TaxEngine _taxEngine;
        private readonly Purchase _currentPurchase = new Purchase();

        public Checkout()
        {
        }

        public Checkout(TaxEngine taxEngine)
        {
            _taxEngine = taxEngine;
        }

        public void Scan(Article article)
        {
            if (_taxEngine == null)
            {
                _currentPurchase.Add(article);
                return;
            }
            _currentPurchase.Add(article, _taxEngine.TaxFor(article));
        }

        public Receipt EmitReceipt()
        {
            return _currentPurchase.BuildReceipt();
        }
    }
}