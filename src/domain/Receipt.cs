using System.Collections.Generic;
using System.Linq;

namespace SalesTaxes.Domain
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