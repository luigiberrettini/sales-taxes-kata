using System.Collections.Generic;
using SalesTaxesKata.Domain.Catalog;

namespace SalesTaxesKata.Domain.Taxation
{
    public class TaxEngine
    {
        private static Tax NoTax { get; } = new Tax(0);

        private static Tax BasicTax { get; } = new Tax(10);

        public Tax ImportDuty { get; } = new Tax(5);

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

        public Tax TaxFor(Article article, Country saleCountry)
        {
            var tax1 = _exemptCategories.Contains(article.Category) ? NoTax : BasicTax;
            var tax2 = saleCountry == article.Supplier.Country ? NoTax : ImportDuty;
            return new Tax(tax1.Rate + tax2.Rate);
        }
    }
}