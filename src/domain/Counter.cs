namespace SalesTaxes.Domain
{
    public class Counter
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