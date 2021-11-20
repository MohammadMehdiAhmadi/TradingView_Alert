using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TradingView_Example.Models;
using TradingView_Example.Models.Enums;
using TradingView_Example.Services.TradingView.Model;

namespace TradingView_Example
{
    public partial class AlertsForm : BaseForm
    {
        #region Fields

        private TradingViewSymbol _tradingViewSymbol;
        private IList<TradingViewSymbol> _selectedSymbols;

        private TradingViewAlertModel TradingViewAlertModel
        {
            get 
            {
                var _tradingViewAlertModel = new TradingViewAlertModel
                {
                    Name = textBoxAlertName.Text,
                    Symbol = (comboBoxAlertSymbol.SelectedItem as TradingViewSymbol).Symbol,
                    Volume = numericUpDownAlertValue.Value,
                    Status = TradingViewAlertStatus.Active,
                    ExirationTime = dateTimePickerAlertExT.Enabled ? (DateTime?)dateTimePickerAlertExT.Value : null,
                    Message = textBoxAlertMessage.Text
                };

                return _tradingViewAlertModel;
            }
            set 
            {
                _tradingViewSymbol = new TradingViewSymbol 
                {
                    Exchange = value.Symbol.Split(':')[0],
                    Symbol = value.Symbol.Split(':')[1],
                    Type = "crypto"
                };

                textBoxAlertName.Text = value.Name;

                comboBoxAlertSymbol.Items.Clear();
                comboBoxAlertSymbol.Items.Add(_tradingViewSymbol);
                comboBoxAlertSymbol.SelectedIndex = 0;
                comboBoxAlertSymbol.Enabled = false;

                numericUpDownAlertValue.Value = value.Volume;

                dateTimePickerAlertExT.Enabled = value.ExirationTime.HasValue;
                if (dateTimePickerAlertExT.Enabled)
                    dateTimePickerAlertExT.Value = value.ExirationTime.Value;

                textBoxAlertMessage.Text = value.Message;
            }
        }


        #endregion

        #region Ctor

        public AlertsForm()
        {
            InitializeComponent();
            CustomInitializeComponent();
        }

        #endregion

        #region ShowDialog

        public DialogResult ShowDialog(TradingViewSymbol tradingViewSymbol = null, IList<TradingViewSymbol> selectedSymbols = null)
        {
            _tradingViewSymbol = tradingViewSymbol;
            _selectedSymbols = selectedSymbols;

            return base.ShowDialog();
        }

        #endregion

        #region Form Events

        #region AlertsForm Load

        private void AlertsForm_Load(object sender, EventArgs e)
        {
            LoadAlertSymbol();
        }

        #endregion

        #region AlertsForm Shown

        private void AlertsForm_Shown(object sender, EventArgs e)
        {

        }

        #endregion

        #region AlertsForm FormClosed

        private void AlertsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            buttonRemoveAlert.Enabled = false;
            listViewAletsList.Items.Clear();
        }

        #endregion

        #region CheckBoxAlertEnableExT CheckedChanged

        private void CheckBoxAlertEnableExT_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerAlertExT.Enabled = checkBoxAlertEnableExT.Checked;
            dateTimePickerAlertExT.Value = DateTime.Now.AddDays(1);
        }

        #endregion

        #region ComboBoxAlertSymbol SelectedIndexChanged

        private void ComboBoxAlertSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSymbol = comboBoxAlertSymbol.SelectedItem as TradingViewSymbol;
            _tradingViewSymbol = selectedSymbol;

            Text = $"[{selectedSymbol}] Alerts";
            textBoxAlertName.Text = $"{selectedSymbol}-alert";
            textBoxAlertMessage.Text = $"{selectedSymbol} Crossing ({selectedSymbol.CurrentPrice})";
            numericUpDownAlertValue.Value = selectedSymbol.CurrentPrice;
        }

        #endregion

        #region NumericUpDownAlertValue ValueChanged

        private void NumericUpDownAlertValue_ValueChanged(object sender, EventArgs e)
        {
            textBoxAlertMessage.Text = $"{_tradingViewSymbol} Crossing ({numericUpDownAlertValue.Value})";

            TradingViewAlertModel.Volume = numericUpDownAlertValue.Value;
        }

        #endregion

        #region ButtonAddOrEditAlert Click

        private async void ButtonAddOrEditAlert_Click(object sender, EventArgs e)
        {
            var alerts = TradingViewAlertModel.LoadAlerts();

            var _tradingViewAlertModel = TradingViewAlertModel;

            if (alerts.Any(p => p.Name == _tradingViewAlertModel.Name))
            {
                MessageBox.Show("Alert is exist!", "Add Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InsertAlertOnList(_tradingViewAlertModel);

            alerts.Add(_tradingViewAlertModel);

            await TradingViewAlertModel.SaveAsync(alerts);
        }

        #endregion

        #region ButtonRemoveAlert Click

        private void ButtonRemoveAlert_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region ListViewAletsList SelectedIndexChanged

        private void ListViewAletsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAletsList.SelectedItems.Count == 0)
            {
                buttonRemoveAlert.Enabled = false;
                return;
            }

            buttonRemoveAlert.Enabled = true;
        }

        #endregion

        #endregion

        #region Utilities

        #region CustomInitializeComponent

        private void CustomInitializeComponent()
        {
            dateTimePickerAlertExT.Value = DateTime.Now.AddDays(1);
        }

        #endregion

        #region LoadAlertSymbol

        private void LoadAlertSymbol()
        {
            if (_tradingViewSymbol == null)
            {
                LoadDefaultAlertSymbol(_selectedSymbols);
                return;
            }

            comboBoxAlertSymbol.Items.Clear();
            comboBoxAlertSymbol.Items.Add(_tradingViewSymbol);
            comboBoxAlertSymbol.Enabled = false;
            comboBoxAlertSymbol.SelectedIndex = 0;

            LoadAlertsList(_tradingViewSymbol);
        }

        private void LoadDefaultAlertSymbol(IList<TradingViewSymbol> selectedSymbols)
        {
            comboBoxAlertSymbol.Items.Clear();
            comboBoxAlertSymbol.Enabled = true;

            checkBoxAlertEnableExT.Checked = false;
            dateTimePickerAlertExT.Value = DateTime.Now;

            if (!selectedSymbols.Any())
                return;

            comboBoxAlertSymbol.Items.AddRange(selectedSymbols.ToArray());

            comboBoxAlertSymbol.SelectedIndex = 0;

            LoadAlertsList();
        }

        #endregion

        #region InsertAlertOnList

        private void InsertAlertOnList(TradingViewAlertModel _tradingViewAlertModel)
        {
            _tradingViewAlertModel.Number = listViewAletsList.Items.Count + 1;

            var exirationTime = _tradingViewAlertModel.ExirationTime.HasValue ? (_tradingViewAlertModel.ExirationTime.Value - DateTime.Now).TotalMinutes.ToString() : "-";
            var row = new[]
            {
                _tradingViewAlertModel.Number.ToString(),
                _tradingViewAlertModel.Name,
                _tradingViewAlertModel.Symbol,
                _tradingViewAlertModel.Volume.ToString(),
                _tradingViewAlertModel.Status.ToString(),
                exirationTime,
                _tradingViewAlertModel.Message
            };

            var listViewItem = new ListViewItem(row)
            {
                Tag = _tradingViewSymbol,
                Name = _tradingViewSymbol.ToString()
            };

            listViewAletsList.Items.Add(listViewItem);
        }

        #endregion

        #region LoadAlertsList

        private void LoadAlertsList(TradingViewSymbol selectedSymbol = null)
        {
            listViewAletsList.Items.Clear();

            var alerts = TradingViewAlertModel
                .LoadAlerts()
                .Where(p => selectedSymbol == null || p.Symbol == selectedSymbol.Symbol)
                .ToList();

            alerts.ForEach(p => InsertAlertOnList(p));
        }

        #endregion

        #endregion
    }
}
