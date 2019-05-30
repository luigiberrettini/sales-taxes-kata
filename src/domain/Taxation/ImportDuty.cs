using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public class ImportDuty : Tax
    {
        public override decimal Rate => 5M;

        public ImportDuty() : base(new UpToFiveCentsRounding())
        {
        }

        public override bool IsApplicable(Article article, Country saleCountry)
        {
            return article.SupplierCountry != saleCountry;
        }
    }
}