using System;
using SalesTaxesKata.Domain.Catalog;

namespace SalesTaxesKata.Domain.Taxation
{
    public abstract class Tax
    {
        public abstract decimal Rate { get; }

        public abstract bool IsApplicable(Article article, Country saleCountry);

        public decimal ApplyTo(decimal price)
        {
            var taxedPriceCents = price * (100 + Rate);
            var roundedUpToNearestFive = Math.Ceiling(taxedPriceCents / 5) * 5;
            return roundedUpToNearestFive / 100;
        }
    }
}