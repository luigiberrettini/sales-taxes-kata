using System.Collections.Generic;
using System.Linq;
using SalesTaxes.Domain.Shopping;

namespace SalesTaxes.Domain.Payment
{
    public class Receipt
    {
        public IEnumerable<Entry> Entries { get; }

        public Receipt(Purchase purchase)
        {
            Entries = purchase.Articles.Select(x => new Entry { Price = x.Price });
        }
    }
}