using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingView_Example.Services.TradingView.Model;

namespace TradingView_Example.Services.TradingView
{
    public interface ITradingViewClient
    {
        string BaseApiAddress { get; set; }

        string AuthToken { get; set; }

        IList<string> SubscribedSymbols { get; set; }

        Task<(string, TradingViewUser)> LoginAsync(string username, string password);

        Task<(string, IList<TradingViewSymbol>)> SearchSymbolAsync(string searchedSymbol, string exchange = null);

        Task<bool> SocketConnectAsync(string authToken);

        Task SubscribeSymbolsAsync(string symbol);

        Task UnsubscribeSymbolsAsync(string symbol);

        Task ListenToAsync(Action<TradingViewSymbolPrice> onMessage);
    }
}