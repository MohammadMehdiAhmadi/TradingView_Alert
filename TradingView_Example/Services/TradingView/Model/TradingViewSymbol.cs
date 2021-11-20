using Newtonsoft.Json;

namespace TradingView_Example.Services.TradingView.Model
{
    public class TradingViewSymbol
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }

        public decimal CurrentPrice { get; set; }

        public bool CanSelect { get; set; }

        public override string ToString()
        {
            return $"{Exchange}:{Symbol}".ToUpper();
        }
    }
}
