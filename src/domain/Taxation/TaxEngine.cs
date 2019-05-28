using System.Collections.Generic;
using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Taxation
{
    public class TaxEngine
    {
        private readonly IDictionary<Category, Tax> _ratesByCategory;

        public TaxEngine()
        {
            _ratesByCategory = new Dictionary<Category, Tax>
            {
                [Category.Books] = new Tax(0),
                [Category.Food] = new Tax(0),
                [Category.Medical] = new Tax(0),
                [Category.Beauty] = new Tax(10)
            };
        }

        public Tax TaxFor(Article article)
        {
            return _ratesByCategory[article.Category];
        }
    }
}