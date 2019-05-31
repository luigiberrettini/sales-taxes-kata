using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalesTaxesKata.Domain.Extensions;
using SalesTaxesKata.Domain.Shopping;

namespace SalesTaxesKata.Domain.Payment
{
    public class Receipt
    {
        public IReadOnlyCollection<Entry> Entries { get; }

        public decimal Taxes { get; private set; }

        public decimal Total => Entries.Sum(x => x.TotalPriceWithTaxes);

        public Receipt()
        {
            Entries = new List<Entry>();
            Taxes = 0;
        }

        public void Add(Item item)
        {
            ((IList<Entry>)Entries).Add(new Entry(item));
            Taxes += item.TotalPriceAfterTaxes - item.UnitPriceBeforeTaxes * item.Quantity;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Entries.ForEach(x => sb.AppendLine(x.ToString()));
            sb.AppendLine($"Sales Taxes: {Taxes}");
            sb.AppendLine($"Total: {Total}");
            return sb.ToString();
        }
    }
}