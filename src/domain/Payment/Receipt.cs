using System.Collections.Generic;

namespace SalesTaxes.Domain.Payment
{
    public class Receipt
    {
        public IList<Entry> Entries { get; }

        public Receipt()
        {
            Entries = new List<Entry>();
        }

        public void Add(Entry entry)
        {
            Entries.Add(entry);
        }
    }
}