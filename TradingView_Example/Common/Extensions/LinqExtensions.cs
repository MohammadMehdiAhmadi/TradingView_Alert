using System;
using System.Collections.Generic;
using System.Linq;

namespace TradingView_Example.Common.Extensions
{
    public static class LinqExtensions
    {
        public static string Join<T>(this IEnumerable<T> items, string separator)
        {
            if (items == null)
                return null;

            return string.Join(separator, items);
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKey)
        {
            return from item in items
                   join otherItem in other on getKey(item)
                   equals getKey(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty()
                   where temp == null || temp.Equals(default(T))
                   select item;

        }
    }
}
