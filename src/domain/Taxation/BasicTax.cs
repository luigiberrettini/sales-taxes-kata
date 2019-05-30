using System.Collections.Generic;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public class BasicTax : Tax
    {
        private static readonly HashSet<Category> ExemptCategories = new HashSet<Category>
        {
            Category.Books,
            Category.Food,
            Category.Medical
        };

        public override decimal Rate => 10M;

        public BasicTax() : base(new UpToFiveCentsRounding())
        {
        }

        public override bool IsApplicable(Article article, Country saleCountry)
        {
            return !ExemptCategories.Contains(article.Category);
        }
    }
}