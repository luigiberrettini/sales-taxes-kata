using SalesTaxes.Domain.Shopping;
using System.Collections.Generic;
using System.Linq;

namespace SalesTaxes.Domain.Payment
{
    public class Receipt
    {
        public IEnumerable<Entry> Entries { get; }

        public Receipt(Purchase purchase)
        {
            Entries = purchase.Items.Select(x => new Entry { Price = x.Price * x.Quantity });
        }
    }
}