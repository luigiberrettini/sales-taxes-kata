using System;
using System.Text.RegularExpressions;

namespace SalesTaxesKata.Domain.Shopping
{
    public struct Good
    {
        // $"{quantity} {name} at {shelfPrice:F2}"
        private static readonly Regex RegEx = new Regex(@"^(\d+) (.+) at (\d+\.\d\d)$", RegexOptions.Compiled);

        public string Name { get; }

        public int Quantity { get; }

        public decimal ShelfPrice { get; }

        public static Good FromString(string good)
        {
            var match = RegEx.Match(good);
            if (match.Groups.Count != 4)
                throw new FormatException();
            var name = match.Groups[2].Value;
            var quantity = int.Parse(match.Groups[1].Value);
            var shelfPrice = decimal.Parse(match.Groups[3].Value);
            return new Good(name, quantity, shelfPrice);
        }

        public Good(string name, int quantity, decimal shelfPrice)
        {
            Name = name;
            Quantity = quantity;
            ShelfPrice = shelfPrice;
        }
    }
}