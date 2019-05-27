using SalesTaxes.Domain.Catalog;
using SalesTaxes.Domain.Payment;

namespace SalesTaxes.Domain.Shopping
{
    public class Checkout
    {
        private readonly Purchase _currentPurchase = new Purchase();

        public void Scan(Article article)
        {
            _currentPurchase.Add(article);
        }

        public Receipt EmitReceipt()
        {
            return _currentPurchase.BuildReceipt();
        }
    }
}