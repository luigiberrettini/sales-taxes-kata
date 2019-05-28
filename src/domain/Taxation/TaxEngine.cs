using System.Collections.Generic;
using SalesTaxes.Domain.Catalog;

namespace SalesTaxes.Domain.Taxation
{
    public class TaxEngine
    {
        private static Tax NoTax { get; } = new Tax(0);

        private static Tax BasicTax { get; } = new Tax(10);

        private readonly HashSet<Category> _exemptCategories;

        public TaxEngine()
        {
            _exemptCategories = new HashSet<Category>
            {
                Category.Books,
                Category.Food,
                Category.Medical
            };
        }

        public Tax TaxFor(Article article)
        {
            return _exemptCategories.Contains(article.Category) ? NoTax : BasicTax;
        }
    }
}