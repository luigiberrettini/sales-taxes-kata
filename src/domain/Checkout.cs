namespace SalesTaxes.Domain
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
            return new Receipt(_currentPurchase);
        }
    }
}