﻿using SalesTaxesKata.Domain.Geo;

namespace SalesTaxesKata.Domain.Sales
{
    public class Supplier
    {
        public string VatCode { get; }

        public string Name { get; }

        public Country Country { get; }

        public Supplier(string vatCode, string name, Country country)
        {
            VatCode = vatCode;
            Name = name;
            Country = country;
        }
    }
}