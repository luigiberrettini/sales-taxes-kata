﻿using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public class TaxEngine
    {
        private readonly IReadOnlyCollection<Tax> _taxes;

        public TaxEngine()
        {
            _taxes = new List<Tax>
            {
                new BasicTaxAndImportDuty(),
                new BasicTax(),
                new ImportDuty(),
                new NoTax()
            };
        }

        public Tax TaxFor(Article article, Country saleCountry)
        {
            return _taxes.First(x => x.IsApplicable(article, saleCountry));
        }
    }
}