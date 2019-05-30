namespace SalesTaxesKata.Domain.Taxation
{
    public class NoRounding : Rounding
    {
        public override decimal Round(decimal taxedPrice)
        {
            return taxedPrice;
        }
    }
}