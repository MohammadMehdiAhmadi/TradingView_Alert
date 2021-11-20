using System.Collections.Generic;

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
    }
}
