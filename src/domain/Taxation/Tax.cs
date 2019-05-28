using System;

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
            var taxedPriceCents = price * (100 + _rate);
            var roundedUpToNearestFive = Math.Ceiling(taxedPriceCents / 5) * 5;
            return roundedUpToNearestFive / 100;
        }
    }
}