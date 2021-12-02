using Microsoft.Win32;
using System.Windows.Forms;
using TradingView_Example.Properties;

namespace TradingView_Example
{
    public partial class SettingsForm : Form
    {
        #region Ctor

        public SettingsForm()
        {
            InitializeComponent();
        }

        #endregion

        #region SettingsForm Load

        private void SettingsForm_Load(object sender, System.EventArgs e)
        {
            checkBoxMinimizeOnExist.Checked = Settings.Default.MinimizeOnExist;
            checkBoxAutoStart.Checked = Settings.Default.AutoStart;
            checkBoxMinimizedStart.Enabled = Settings.Default.AutoStart;
            checkBoxMinimizedStart.Checked = Settings.Default.StartMinimized;
        }

        #endregion

        #region CheckBoxMinimizeOnExist CheckedChanged

        private void CheckBoxMinimizeOnExist_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxMinimizeOnExist.Checked)
                Program.RegistryKey.SetValue("TradingView_Example", Application.ExecutablePath + " -windowsStartup");
            else
                Program.RegistryKey.DeleteValue("TradingView_Example", false);

            Settings.Default.MinimizeOnExist = checkBoxMinimizeOnExist.Checked;
            Settings.Default.Save();
        }

        #endregion

        #region CheckBoxAutoStart CheckedChanged

        private void CheckBoxAutoStart_CheckedChanged(object sender, System.EventArgs e)
        {
            checkBoxMinimizedStart.Enabled = checkBoxAutoStart.Checked;

            Settings.Default.AutoStart = checkBoxAutoStart.Checked;
            Settings.Default.Save();
        }

        #endregion

        #region CheckBoxMinimizedStart CheckedChanged

        private void CheckBoxMinimizedStart_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.StartMinimized = checkBoxMinimizedStart.Checked;
            Settings.Default.Save();
        }

        #endregion
    }
}
