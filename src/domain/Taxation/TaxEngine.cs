using System.Collections.Generic;
using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Taxation
{
    public class TaxEngine
    {
        private static Tax NoTax { get; } = new Tax(0);

        private static Tax BasicTax { get; } = new Tax(10);

        private readonly IDictionary<Category, Tax> _ratesByCategory;

        public TaxEngine()
        {
            _ratesByCategory = new Dictionary<Category, Tax>
            {
                [Category.Books] = NoTax,
                [Category.Food] = NoTax,
                [Category.Medical] = NoTax,
                [Category.Beauty] = BasicTax
            };
        }

        public Tax TaxFor(Article article)
        {
            return _ratesByCategory[article.Category];
        }
    }
}