using Newtonsoft.Json;

namespace TradingView_Example.Services.TradingView.Model
{
    public class LoginResultModel
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("user")]
        public TradingViewUser User { get; set; }
    }
}
