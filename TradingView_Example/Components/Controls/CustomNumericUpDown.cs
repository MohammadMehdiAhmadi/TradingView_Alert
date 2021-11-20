using System.Windows.Forms;

namespace TradingView_Example.Components.Controls
{
    public class CustomNumericUpDown : NumericUpDown
    {
        protected override void UpdateEditText()
        {
            Text = Value.ToString("0." + new string('#', DecimalPlaces));
        }
    }
}
