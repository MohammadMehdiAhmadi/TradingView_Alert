using Newtonsoft.Json;

namespace TradingView_Example.Services.TradingView.Model
{
    public class TradingViewUser
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }
    }
}