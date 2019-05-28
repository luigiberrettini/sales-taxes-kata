using System.Collections.Generic;
using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Taxation
{
    public class TaxEngine
    {
        private IDictionary<Category, decimal> ratesByCategory;

        public TaxEngine()
        {
            ratesByCategory = new Dictionary<Category, decimal>
            {
                [Category.Books] = 0,
                [Category.Beauty] = 10
            };
        }

        public Tax TaxFor(Article article)
        {
            return new Tax(ratesByCategory[article.Category]);
        }
    }
}