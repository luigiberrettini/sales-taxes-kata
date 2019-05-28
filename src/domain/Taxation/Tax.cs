using System;

namespace SalesTaxesKata.Domain.Taxation
{
    public class Tax
    {
        public decimal Rate { get; }

        public Tax(decimal rate)
        {
            Rate = rate;
        }

        public decimal Apply(decimal price)
        {
            var taxedPriceCents = price * (100 + Rate);
            var roundedUpToNearestFive = Math.Ceiling(taxedPriceCents / 5) * 5;
            return roundedUpToNearestFive / 100;
        }
    }
}