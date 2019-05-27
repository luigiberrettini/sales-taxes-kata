using System.Collections.Generic;
using System.Linq;

namespace SalesTaxes.TestSuite.Domain
{
    public class Receipt
    {
        public IEnumerable<Entry> Entries { get; }

        public Receipt(IEnumerable<Article> purchase)
        {
            Entries = purchase.Select(x => new Entry { Price = x.Price });
        }
    }
}