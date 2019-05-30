using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public class BasicTaxAndImportDuty : Tax
    {
        private readonly IReadOnlyCollection<Tax> _taxes;

        public override decimal Rate { get; }

        public BasicTaxAndImportDuty() : base(new UpToFiveCentsRounding())
        {
            _taxes = new List<Tax>
            {
                new BasicTax(),
                new ImportDuty()
            };
            Rate = _taxes.Sum(x => x.Rate);
        }

        public override bool IsApplicable(Article article, Country saleCountry)
        {
            return _taxes.All(x => x.IsApplicable(article, saleCountry));
        }
    }
}