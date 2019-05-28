﻿using System.Collections.Generic;
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

        public Tax TaxFor(Article article)
        {
            if (article.Supplier.Country == Country.Ita)
                return ImportDuty;
            return _exemptCategories.Contains(article.Category) ? NoTax : BasicTax;
        }
    }
}