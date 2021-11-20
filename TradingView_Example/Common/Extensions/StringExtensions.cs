using System.Diagnostics;

namespace TradingView_Example.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the string is null, empty or all whitespace.
        /// </summary>
        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static bool HasValue(this string value)
        {
            if (value == null)
                return false;

            return !string.IsNullOrWhiteSpace(value.Trim());
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string value)
        {
            return !string.IsNullOrEmpty(value) && RegularExpressions.IsEmail.IsMatch(value.Trim());
        }

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
