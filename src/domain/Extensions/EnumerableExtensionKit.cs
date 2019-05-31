using System;
using System.Collections.Generic;

namespace SalesTaxesKata.Domain.Extensions
{
    public static class EnumerableExtensionKit
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}