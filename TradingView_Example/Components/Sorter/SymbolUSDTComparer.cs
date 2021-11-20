using System.Collections.Generic;

namespace TradingView_Example.Components.Sorter
{
    public class SymbolUSDTComparer : IComparer<string>
    {
        private string _symbol;

        public SymbolUSDTComparer(string symbol)
        {
            _symbol = symbol;
        }

        public int Compare(string first, string second)
        {
            return first.EndsWith(_symbol) ? 1 : -1;
        }
    }
}
