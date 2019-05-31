using System;
using System.Text.RegularExpressions;

namespace SalesTaxesKata.Domain.Shopping
{
    public struct Good
    {
        // $"{quantity} {name} at {shelfPrice:F2}"
        private static readonly Regex RegExGood = new Regex(@"^(\d+) (.+) at (\d+\.\d\d)$", RegexOptions.Compiled);
        private static readonly Regex RegExImported = new Regex("imported | imported", RegexOptions.Compiled);

        public string Name { get; }

        public int Quantity { get; }

        public decimal ShelfPrice { get; }

        public bool IsImported { get; }

        public static Good FromString(string good)
        {
            var match = RegExGood.Match(good);
            if (match.Groups.Count != 4)
                throw new FormatException();
            var printedName = match.Groups[2].Value;
            var name = RegExImported.Replace(printedName, string.Empty);
            var quantity = int.Parse(match.Groups[1].Value);
            var shelfPrice = decimal.Parse(match.Groups[3].Value);
            var isImported = printedName != name;
            return new Good(name, quantity, shelfPrice, isImported);
        }

        public Good(string name, int quantity, decimal shelfPrice, bool isImported)
        {
            Name = name;
            Quantity = quantity;
            ShelfPrice = shelfPrice;
            IsImported = isImported;
        }
    }
}