using System.Collections.Generic;
using System.Linq;
using SalesTaxesKata.Domain.Shopping;

namespace SalesTaxesKata.Domain.Payment
{
    public class Receipt
    {
        public IReadOnlyCollection<Entry> Entries { get; }

        public decimal SalesTaxes { get; private set; }

        public decimal Total => Entries.Sum(x => x.TotalPriceWithTaxes);

        public Receipt()
        {
            Entries = new List<Entry>();
            SalesTaxes = 0;
        }

        public void Add(Item item)
        {
            ((IList<Entry>)Entries).Add(new Entry(item));
            SalesTaxes += (item.UnitPriceAfterTaxes - item.UnitPriceBeforeTaxes) * item.Quantity;
        }
    }
}