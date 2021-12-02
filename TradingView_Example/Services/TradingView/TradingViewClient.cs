using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TradingView_Example.Common.Extensions;
using TradingView_Example.Common.Json.Serialization;
using TradingView_Example.Services.TradingView.Model;

namespace TradingView_Example.Services.TradingView
{
    public class TradingViewClient : ITradingViewClient
    {
        #region Fields

        private static ClientWebSocket _clientWebSocket;
        private static CancellationTokenSource _ctsSource;
        private static readonly Encoding _encoding = Encoding.UTF8;

        private RestClient _restClient;

        private string _baseApiAddress;
        private string _sessionId;

        public string AuthToken { get; set; }

        public string BaseApiAddress
        {
            get { return _baseApiAddress; }
            set
            {
                SetBaseApiAddress(value);
                _baseApiAddress = value;
            }
        }

        public IList<string> SubscribedSymbols { get; set; }

        #endregion

        #region Ctor

        public TradingViewClient()
        {
            SubscribedSymbols = new List<string>();
        }

        #endregion

        #region Set Base Api Address

        public void SetBaseApiAddress(string baseApiAddress)
        {
            _restClient = new RestClient(baseApiAddress);

            _restClient.AddHandler("application/json", () => NewtonsoftJsonSerializer.Default);
            _restClient.AddHandler("text/json", () => NewtonsoftJsonSerializer.Default);
            _restClient.AddHandler("text/x-json", () => NewtonsoftJsonSerializer.Default);
            _restClient.AddHandler("text/javascript", () => NewtonsoftJsonSerializer.Default);
            _restClient.AddHandler("*+json", () => NewtonsoftJsonSerializer.Default);

            _baseApiAddress = baseApiAddress;
        }

        #endregion

        #region Get Rest Request

        private RestRequest CreateRestRequest(string resource, Method method)
        {
            var request = new RestRequest
            {
                Resource = resource,
                Method = method,
                RequestFormat = DataFormat.Json,
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };

            request.JsonSerializer.ContentType = "application/json; charset=utf-8";
            request.AddHeader("origin", "https://www.tradingview.com");

            return request;
        }

        #endregion

        #region Login Async

        public async Task<(string, TradingViewUser)> LoginAsync(string username, string password)
        {
            BaseApiAddress = "https://www.tradingview.com";

            var request = CreateRestRequest("/accounts/signin/", Method.POST);

            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.AddParameter("remember", "on");


            var response = await _restClient.ExecuteAsync<LoginResultModel>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data.Error.HasValue())
                    return (response.Data.Error, null);

                if (response.Data.User == null)
                    return ("Server error!", null);

                await SendString("set_auth_token", new[] { response.Data.User.AuthToken });

                return (null, response.Data.User);
            }

            return ($"Error on login user :: Status Code: {response.StatusCode} :: Message: {response.StatusDescription}", null);
        }

        #endregion

        #region SearchSymbolAsync

        public async Task<(string, IList<TradingViewSymbol>)> SearchSymbolAsync(string searchedSymbol, string exchange = null)
        {
            BaseApiAddress = "https://symbol-search.tradingview.com";

            var request = CreateRestRequest("/symbol_search/", Method.GET);

            request.AddParameter("type", "crypto");

            if (searchedSymbol.HasValue())
                request.AddParameter("text", searchedSymbol.ToUpper());

            if (exchange.HasValue())
                request.AddParameter("exchange", exchange.ToUpper());

            var response = await _restClient.ExecuteAsync<IList<TradingViewSymbol>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data == null)
                    return ("Server error!", null);

                return (null, response.Data);
            }

            return ($"Error on login user :: Status Code: {response.StatusCode} :: Message: {response.StatusDescription}", null);
        }

        #endregion

        #region SocketConnectAsync

        public async Task<bool> SocketConnectAsync(string authToken)
        {
            var headers = new Dictionary<string, string>
            {
                { "Origin", "https://s.tradingview.com" }
            };

            var cookieContainer = new CookieContainer();
            _clientWebSocket = new ClientWebSocket();

            _clientWebSocket.Options.Cookies = cookieContainer;

            foreach (var header in headers)
                _clientWebSocket.Options.SetRequestHeader(header.Key, header.Value);

            _clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            _clientWebSocket.Options.SetBuffer(65536, 65536);

            _ctsSource = new CancellationTokenSource();


            try
            {
                using (var tcs = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    await _clientWebSocket.ConnectAsync(new Uri("wss://widgetdata.tradingview.com/socket.io/websocket"), default).ConfigureAwait(false);

                    _sessionId = GenerateSession();

                    await SendString("set_auth_token", new[] { authToken.HasValue() ? authToken : "unauthorized_user_token" });
                    await SendString("quote_create_session", new[] { _sessionId });
                    await SendString("quote_set_fields", new[] { _sessionId, "volume", "lp", "chp", "ch" });
                }
            }
            catch (Exception EX)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region SubscribeSymbolsAsync

        public async Task SubscribeSymbolsAsync(string symbol)
        {
            await SendString("quote_add_symbols", new[] { _sessionId, symbol });

            SubscribedSymbols.Add(symbol);
        }

        #endregion

        #region UnsubscribeSymbolsAsync

        public async Task UnsubscribeSymbolsAsync(string symbol)
        {
            await SendString("quote_remove_symbols", new[] { _sessionId, symbol });

            SubscribedSymbols.Remove(symbol);
        }

        #endregion

        #region LestenToAsync

        public async Task ListenToAsync(Action<TradingViewSymbolPrice> onMessage)
        {
            var handler = new Action<TradingViewSymbolPrice>(data => onMessage(data));

            var buffer = new ArraySegment<byte>(new byte[65536]);

            try
            {
                while (true)
                {
                    WebSocketReceiveResult receiveResult = null;
                    while (true)
                    {
                        try
                        {
                            receiveResult = await _clientWebSocket.ReceiveAsync(buffer, _ctsSource.Token).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException EX)
                        {
                            break;
                        }
                        catch (WebSocketException EX)
                        {
                            break;
                        }
                        catch (IOException EX)
                        {
                            break;
                        }

                        if (receiveResult.MessageType == WebSocketMessageType.Close)
                        {
                            break;
                        }

                        await HandleMessageAsync(buffer.Array, buffer.Offset, receiveResult.Count, receiveResult.MessageType, handler).ConfigureAwait(false);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task HandleMessageAsync(byte[] data, int offset, int count, WebSocketMessageType messageType, Action<TradingViewSymbolPrice> handler)
        {
            var strData = _encoding.GetString(data, offset, count);

            var response = Regex.Split(Regex.Replace(strData, @"~h~", string.Empty), @"~m~[0-9]{1,}~m~");

            foreach (var item in response)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                if (item.Contains("release"))
                    continue;

                if (int.TryParse(item, out int pingNumber))
                {
                    await SendPingAsync(pingNumber);

                    return;
                }

                var packet = JsonConvert.DeserializeObject<dynamic>(item);

                if (packet.m == "protocol_error")
                    continue;

                if (packet.m == "quote_completed")
                    continue;

                if (packet.m == "qsd")
                {
                    var tradingViewSymbolPrice = new TradingViewSymbolPrice();

                    var symbolPrice = packet.p[1];

                    if (symbolPrice.s != "ok")
                        continue;

                    tradingViewSymbolPrice.Change = (decimal?)symbolPrice.v.ch;
                    tradingViewSymbolPrice.ChangeP = (decimal?)symbolPrice.v.chp;
                    tradingViewSymbolPrice.Price = (decimal?)symbolPrice.v.lp;
                    tradingViewSymbolPrice.Symbol = (string)symbolPrice.n;
                    tradingViewSymbolPrice.Volume = (decimal?)symbolPrice.v.volume;

                    handler?.Invoke(tradingViewSymbolPrice);

                    continue;
                }
            }
        }

        #endregion

        #region Utilities

        #region SendString

        public async Task SendString(string message, string[] baseData, CancellationToken cancellation = default)
        {
            try
            {
                var dataObject = JsonConvert.SerializeObject(new { m = message, p = baseData });

                var data = $"~m~{dataObject.Length}~m~{dataObject}";

                var encoded = Encoding.UTF8.GetBytes(data);

                var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

                await _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
            }
            catch
            {
                throw;
            }
        }

        public async Task SendPingAsync(int pingNumber, CancellationToken cancellation = default)
        {
            try
            {
                var pingString = $"~h~{pingNumber}";

                var data = $"~m~{pingString.Length}~m~{pingString}";

                var encoded = Encoding.UTF8.GetBytes(data);

                var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

                await _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GenerateSession

        private string GenerateSession()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var randomString = new string(Enumerable.Repeat(chars, 12)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"qs_{randomString}";
        }

        #endregion

        #endregion
    }
}
