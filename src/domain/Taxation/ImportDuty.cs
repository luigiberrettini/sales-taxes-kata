using SalesTaxesKata.Domain.Catalog;

namespace SalesTaxesKata.Domain.Taxation
{
    public class ImportDuty : Tax
    {
        public override decimal Rate => 5M;

        public override bool IsApplicable(Article article, Country saleCountry)
        {
            return article.Supplier.Country != saleCountry;
        }
    }
}