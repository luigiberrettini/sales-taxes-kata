using System;
using System.Text.RegularExpressions;

namespace SalesTaxesKata.Domain.Shopping
{
    public struct Good
    {
        public string Name { get; }

        public int Quantity { get; }

        public decimal ShelfPrice { get; }

        public static Good FromString(string good)
        {
            // $"{quantity} {name} at {shelfPrice:F2}"
            const string regExPattern = @"^(\d+) (.+) at (\d+\.\d\d)$";
            var fields = Regex.Split(good, regExPattern);
            if (fields.Length != 3)
                throw new FormatException();
            var name = fields[1];
            var quantity = int.Parse(fields[0]);
            var shelfPrice = decimal.Parse(fields[2]);
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