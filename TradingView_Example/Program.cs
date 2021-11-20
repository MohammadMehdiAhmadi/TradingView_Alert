using System;
using System.Windows.Forms;
using TradingView_Example.Ioc;

namespace TradingView_Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = SimpleInjectorConfig.Setup();

            Application.Run(container.GetInstance<MainForm>());
        }
    }
}
