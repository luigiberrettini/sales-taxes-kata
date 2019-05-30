using System;

namespace SalesTaxesKata.Domain.Taxation
{
    public class UpToFiveCentsRounding : Rounding
    {
        public override decimal Round(decimal taxedPrice)
        {
            return Math.Ceiling(taxedPrice * 20) / 20;
        }
    }
}