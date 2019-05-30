using SalesTaxesKata.Domain.Geo;
using SalesTaxesKata.Domain.Sales;

namespace SalesTaxesKata.Domain.Taxation
{
    public abstract class Tax
    {
        private readonly Rounding _rounding;

        public abstract decimal Rate { get; }

        public abstract bool IsApplicable(Article article, Country saleCountry);

        protected Tax(Rounding rounding)
        {
            _rounding = rounding;
        }

        public decimal ApplyTo(decimal price)
        {
            return _rounding.Round(TaxedPrice(price));
        }

        private decimal TaxedPrice(decimal price)
        {
            return price * (100 + Rate) / 100;
        }
    }
}