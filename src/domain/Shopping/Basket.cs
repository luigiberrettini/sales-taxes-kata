using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesTaxesKata.Domain.Shopping
{
    public class Basket : IEquatable<Basket>
    {
        public IReadOnlyCollection<Good> Goods { get; }

        public static Basket FromString(string input)
        {
            var lines = input.Split((string[])new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var basket = new Basket();
            lines.ToList().ForEach(x => basket.Add(Good.FromString(x)));
            return basket;
        }

        public Basket()
        {
            Goods = new HashSet<Good>();
        }

        public void Add(Good good)
        {
            ((HashSet<Good>)Goods).Add(good);
        }

        public override int GetHashCode()
        {
            return Goods == null ? 0 : Goods.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) &&
                (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() && Equals((Basket)obj));
        }

        public bool Equals(Basket other)
        {
            return !(other is null) &&
                (ReferenceEquals(this, other) || Goods.SequenceEqual(other.Goods));
        }
    }
}