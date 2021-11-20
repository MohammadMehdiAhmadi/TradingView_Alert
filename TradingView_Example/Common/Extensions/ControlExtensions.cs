using System;
using System.Windows.Forms;

namespace TradingView_Example.Common.Extensions
{
    public static class ControlExtensions
    {
        #region AddListViewSymbolListItem

        public static void InvokeUi(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                Action safeWrite = delegate { action(); };

                control.Invoke(safeWrite);
            }
            else
                action();
        }

        #endregion
    }
}
