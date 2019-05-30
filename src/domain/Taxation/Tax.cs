using System;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public abstract class Tax
    {
        public abstract decimal Rate { get; }

        public abstract bool IsApplicable(Article article, Country saleCountry);

        public virtual decimal ApplyTo(decimal price)
        {
            var taxedPrice = price * (100 + Rate) / 100;
            return Math.Ceiling(taxedPrice * 20) / 20;
        }
    }
}