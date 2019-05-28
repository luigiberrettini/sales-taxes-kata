using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Taxation
{
    public class TaxEngine
    {
        public Tax TaxFor(Article article)
        {
            return new Tax(0);
        }
    }
}