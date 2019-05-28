using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Taxation
{
    public class TaxEngine
    {
        public Tax TaxFor(Article article)
        {
            var rate = article.Category == Category.Beauty ? 10 : 0;
            return new Tax(rate);
        }
    }
}