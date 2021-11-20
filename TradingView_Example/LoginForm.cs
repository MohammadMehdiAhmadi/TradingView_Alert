using System;
using System.Windows.Forms;
using TradingView_Example.Common.Extensions;
using TradingView_Example.Properties;
using TradingView_Example.Services.TradingView;

namespace TradingView_Example
{
    public partial class LoginForm : Form
    {
        #region Fields

        private readonly ITradingViewClient _tradingViewClient;

        #endregion

        #region Ctor

        public LoginForm(ITradingViewClient tradingViewClient)
        {
            _tradingViewClient = tradingViewClient;

            InitializeComponent();
        }

        #endregion

        #region LoginForm Load

        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBoxUsername.Focus();
        }

        #endregion

        #region ButtonLogin Click

        private async void ButtonLogin_Click(object sender, EventArgs e)
        {
            var username = textBoxUsername.Text;
            var password = textBoxPassword.Text;

            if (username.IsEmpty())
            {
                MessageBox.Show("Username id required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.IsEmpty())
            {
                MessageBox.Show("Username id required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            SetInputEnableState(false);

            var loginResult = await _tradingViewClient.LoginAsync(username, password);
            if (loginResult.Item1.HasValue())
            {
                MessageBox.Show(loginResult.Item1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonLogin.Enabled = true;
                textBoxUsername.Enabled = true;
                textBoxPassword.Enabled = true;
                return;
            }

            var user = loginResult.Item2;

            Settings.Default.UserId = user.Id;
            Settings.Default.Username = user.Username;
            Settings.Default.FirstName = user.FirstName;
            Settings.Default.LastName = user.LastName;
            Settings.Default.AuthToken = user.AuthToken;

            Settings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region LoginForm FormClosed

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            buttonLogin.Enabled = true;
            SetInputEnableState(true);
        }

        #endregion

        #region SetInputEnableState

        private void SetInputEnableState(bool state)
        {
            buttonLogin.Enabled = state;
            textBoxUsername.Enabled = state;
            textBoxPassword.Enabled = state;

            Application.DoEvents();
        }

        #endregion
    }
}
