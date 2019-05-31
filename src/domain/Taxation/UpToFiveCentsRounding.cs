using System;

namespace SalesTaxesKata.Domain.Taxation
{
    public class UpToFiveCentsRounding : Rounding
    {
        public override decimal Round(decimal tax)
        {
            return Math.Ceiling(tax * 20) / 20;
        }
    }
}