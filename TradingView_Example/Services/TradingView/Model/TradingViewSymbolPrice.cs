namespace TradingView_Example.Services.TradingView.Model
{
    public class TradingViewSymbolPrice
    {
        public string Symbol { get; set; }

        public decimal? Price  { get; set; }

        public decimal? Volume { get; set; }

        public decimal? ChangeP { get; set; }

        public decimal? Change { get; set; }
    }
}
