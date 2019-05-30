using System.Collections.Generic;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public class NoTax : Tax
    {
        private static readonly HashSet<Category> ExemptCategories = new HashSet<Category>
        {
            Category.Books,
            Category.Food,
            Category.Medical
        };

        public override decimal Rate => 0M;

        public override bool IsApplicable(Article article, Country saleCountry)
        {
            return ExemptCategories.Contains(article.Category) && article.SupplierCountry == saleCountry; ;
        }

        public override decimal ApplyTo(decimal price)
        {
            return price;
        }
    }
}