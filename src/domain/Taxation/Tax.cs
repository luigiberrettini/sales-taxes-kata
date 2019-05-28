namespace SalesTaxes.Domain.Taxation
{
    public class Tax
    {
        private readonly decimal _rate;

        public Tax(decimal rate)
        {
            _rate = rate;
        }

        public decimal Apply(decimal price)
        {
            return price * (100 + _rate) / 100;
        }
    }
}