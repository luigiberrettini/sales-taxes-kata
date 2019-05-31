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
            return price + _rounding.Round(TaxAmount(price));
        }

        private decimal TaxAmount(decimal price)
        {
            return price * Rate / 100;
        }
    }
}