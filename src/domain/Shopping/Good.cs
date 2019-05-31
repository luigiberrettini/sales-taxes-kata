using System;
using System.Text.RegularExpressions;

namespace SalesTaxesKata.Domain.Shopping
{
    public struct Good
    {
        // $"{quantity} {name} at {shelfPrice:F2}"
        private const string GoodRegExPattern = @"^(\d+) (.+) at (\d+\.\d\d)$";
        private const string ImportedRegExPattern = @"\({0,1}\[{0,1}imported\){0,1}\]{0,1} | \({0,1}\[{0,1}imported\){0,1}\]{0,1}";
        private static readonly Regex RegExGood = new Regex(GoodRegExPattern, RegexOptions.Compiled);
        private static readonly Regex RegExImported = new Regex(ImportedRegExPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Name { get; }

        public int Quantity { get; }

        public decimal ShelfPrice { get; }

        public Origin Origin { get; }

        public static Good FromString(string good)
        {
            var match = RegExGood.Match(good);
            if (match.Groups.Count != 4)
                throw new FormatException($"Format of {nameof(good)} is invalid: '{good}'");
            var printedName = match.Groups[2].Value;
            var name = RegExImported.Replace(printedName, string.Empty);
            var quantity = int.Parse(match.Groups[1].Value);
            var shelfPrice = decimal.Parse(match.Groups[3].Value);
            var origin = printedName == name ? Origin.Local : Origin.Imported;
            return new Good(name, quantity, shelfPrice, origin);
        }

        public Good(string name, int quantity, decimal shelfPrice, Origin origin)
        {
            Name = name;
            Quantity = quantity;
            ShelfPrice = shelfPrice;
            Origin = origin;
        }
    }
}