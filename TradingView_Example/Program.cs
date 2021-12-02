using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using TradingView_Example.Properties;

namespace TradingView_Example
{
    public static class Program
    {
        #region Fields

        public static readonly RegistryKey RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (IsApplicationAlreadyRunning())
                {
                    MessageBox.Show("The application is already running!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    var windowsStartup = args.Any() && args.Contains("-windowsStartup");
                    var autoStart = Settings.Default.AutoStart;
                    var startMinimized = Settings.Default.StartMinimized;

                    var mainForm = new MainForm();
                    if (windowsStartup)
                    {
                        mainForm.WindowState = autoStart && startMinimized ? FormWindowState.Minimized : FormWindowState.Normal;
                        mainForm.ShowInTaskbar = !autoStart || !startMinimized;
                        mainForm.Visible = autoStart && !startMinimized;
                    }

                    Application.Run(mainForm);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsApplicationAlreadyRunning()
        {
            var proc = Process.GetCurrentProcess().ProcessName;
            var processes = Process.GetProcessesByName(proc);

            return processes.Length > 1;
        }
    }
}
