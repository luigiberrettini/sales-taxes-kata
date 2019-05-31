using System.Collections.Generic;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public class NoBasicTax : Tax
    {
        private static readonly HashSet<Category> Categories = new HashSet<Category>
        {
            Category.Books,
            Category.Food,
            Category.Medical
        };

        public override decimal Rate => 0M;

        public NoBasicTax() : base(new NoRounding())
        {
        }

        public override bool IsApplicable(Article article, Country saleCountry)
        {
            return Categories.Contains(article.Category) && article.SupplierCountry == saleCountry; ;
        }
    }
}