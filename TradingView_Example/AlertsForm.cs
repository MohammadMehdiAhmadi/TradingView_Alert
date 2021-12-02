using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using TradingView_Example.Models;
using TradingView_Example.Models.Enums;
using TradingView_Example.Services.TradingView.Model;

namespace TradingView_Example
{
    public partial class AlertsForm : BaseForm
    {
        #region Fields

        public IList<TradingViewAlertModel> TradingViewAlerts { get; set; }

        private TradingViewSymbol TradingViewSymbol { get; set; }

        private IList<TradingViewSymbol> SelectedSymbols { get; set; }

        private TradingViewAlertModel SelectedTradingViewAlertModel
        {
            get
            {
                var tradingViewSymbol = comboBoxAlertSymbol.SelectedItem as TradingViewSymbol;

                var key = $"{tradingViewSymbol.Exchange}:{tradingViewSymbol.Symbol}";

                var selectedSymbol = MainForm.SelectedSymbols.FirstOrDefault(p => p.ToString() == key);

                var _tradingViewAlertModel = new TradingViewAlertModel
                {
                    Name = textBoxAlertName.Text,
                    Exchange = selectedSymbol.Exchange,
                    Symbol = selectedSymbol.Symbol,
                    Volume = numericUpDownAlertValue.Value,
                    PriceState = selectedSymbol.CurrentPrice > numericUpDownAlertValue.Value ? TradingViewPriceState.CrossDown : TradingViewPriceState.CrossUp,
                    Status = TradingViewAlertStatus.Active,
                    ExirationTime = dateTimePickerAlertExT.Enabled ? (DateTime?)dateTimePickerAlertExT.Value : null,
                    Message = textBoxAlertMessage.Text
                };

                return _tradingViewAlertModel;
            }
            set
            {
                var key = $"{value.Exchange}:{value.Symbol}";

                if (comboBoxAlertSymbol.Items.Count > 1)
                {
                    var target = comboBoxAlertSymbol.FindStringExact(key);

                    comboBoxAlertSymbol.SelectedIndex = target;
                    comboBoxAlertSymbol.Enabled = false;
                }

                textBoxAlertName.Text = value.Name;
                numericUpDownAlertValue.Value = value.Volume;
                checkBoxAlertEnableExT.Checked = value.ExirationTime.HasValue;
                dateTimePickerAlertExT.Enabled = value.ExirationTime.HasValue;
                if (checkBoxAlertEnableExT.Checked)
                    dateTimePickerAlertExT.Value = value.ExirationTime.Value;
                else
                    dateTimePickerAlertExT.Value = DateTime.Now.AddDays(1);

                textBoxAlertMessage.Text = value.Message;
            }
        }

        #endregion

        #region Ctor

        public AlertsForm()
        {
            InitializeComponent();
            CustomInitializeComponent();

            TradingViewAlerts = TradingViewAlertModel.LoadAlerts();
        }

        #endregion

        #region ShowDialog

        public DialogResult ShowDialog(TradingViewSymbol tradingViewSymbol = null, IList<TradingViewSymbol> selectedSymbols = null)
        {
            TradingViewSymbol = tradingViewSymbol;
            SelectedSymbols = selectedSymbols;

            return base.ShowDialog();
        }

        #endregion

        #region Invoke Alert

        public async void InvokeAlert(TradingViewSymbolPrice data)
        {
            var alerts = TradingViewAlerts.Where(p => $"{p.Exchange}:{p.Symbol}" == data.Symbol).ToList();
            if (alerts == null || !alerts.Any())
                return;

            var needSave = false;

            foreach (var alert in alerts)
            {
                if (alert.Status != TradingViewAlertStatus.Active)
                    continue;

                if (alert.ExirationTime.HasValue && alert.ExirationTime.Value < DateTime.Now)
                {
                    alert.Status = TradingViewAlertStatus.StopedExpired;

                    needSave = true;

                    continue;
                }

                if (alert.PriceState == TradingViewPriceState.CrossDown && data.Price.Value <= alert.Volume ||
                    alert.PriceState == TradingViewPriceState.CrossUp && data.Price.Value >= alert.Volume)
                {
                    alert.Status = TradingViewAlertStatus.StopedTrigred;

                    AhowAlert(alert, data);

                    needSave = true;

                    SystemSounds.Hand.Play();

                    continue;
                }
            }

            if (needSave)
            {
                await TradingViewAlertModel.SaveAsync(TradingViewAlerts);

                LoadAlertsList();
            }
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

            SetEditMode(false);
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
            if (!(comboBoxAlertSymbol.SelectedItem is TradingViewSymbol selectedSymbol))
                return;

            TradingViewSymbol = selectedSymbol;

            Text = $"[{selectedSymbol}] Alerts";

            GenerateAlertName(selectedSymbol);

            comboBoxAlertType.Items[0] = $"Price ({selectedSymbol})";
            textBoxAlertMessage.Text = $"{selectedSymbol} Crossing ({selectedSymbol.CurrentPrice})";
            numericUpDownAlertValue.Value = selectedSymbol.CurrentPrice;
        }

        #endregion

        #region NumericUpDownAlertValue ValueChanged

        private void NumericUpDownAlertValue_ValueChanged(object sender, EventArgs e)
        {
            textBoxAlertMessage.Text = $"{TradingViewSymbol} Crossing ({numericUpDownAlertValue.Value})";

            SelectedTradingViewAlertModel.Volume = numericUpDownAlertValue.Value;
        }

        #endregion

        #region ButtonAddOrEditAlert Click

        private async void ButtonAddOrEditAlert_Click(object sender, EventArgs e)
        {
            var _tradingViewAlertModel = SelectedTradingViewAlertModel;


            var isEditMode = (bool)(buttonAddOrEditAlert.Tag as dynamic).EditMode;
            if (isEditMode)
            {
                var alert = TradingViewAlerts.FirstOrDefault(p => p.Name == _tradingViewAlertModel.Name);
                if (alert == null)
                    MessageBox.Show("Alert not found!", "Edit Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);

                alert.Status = TradingViewAlertStatus.Active;
                alert.ExirationTime = _tradingViewAlertModel.ExirationTime;
                alert.Message = _tradingViewAlertModel.Message;
                alert.Volume = _tradingViewAlertModel.Volume;

                UpdateAlertOnList(_tradingViewAlertModel);

                SetEditMode(false);

                listViewAletsList.Focus();
            }
            else
            {
                if (TradingViewAlerts.Any(p => p.Name == _tradingViewAlertModel.Name))
                {
                    MessageBox.Show("Alert is exist!", "Add Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_tradingViewAlertModel.ExirationTime.HasValue &&
                    _tradingViewAlertModel.ExirationTime.Value <= DateTime.Now)
                {
                    MessageBox.Show("Alert exiration time is wrang!", "Add Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var existAlerts = listViewAletsList.Items
                    .Cast<ListViewItem>()
                    .Select(p => p.Tag as TradingViewAlertModel)
                    .Where(p =>
                        p.Symbol == _tradingViewAlertModel.Symbol &&
                        p.Exchange == _tradingViewAlertModel.Exchange
                    )
                    .ToList();

                if (existAlerts.Any(p => p.Volume == _tradingViewAlertModel.Volume))
                {
                    MessageBox.Show("Alert is exist with same volume!", "Add Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                InsertAlertOnList(_tradingViewAlertModel);

                TradingViewAlerts.Add(_tradingViewAlertModel);

                GenerateAlertName(TradingViewSymbol);
            }

            await TradingViewAlertModel.SaveAsync(TradingViewAlerts);
        }

        #endregion

        #region ButtonRemoveAlert Click

        private async void ButtonRemoveAlert_Click(object sender, EventArgs e)
        {
            var _tradingViewAlertModel = SelectedTradingViewAlertModel;

            var alert = TradingViewAlerts.FirstOrDefault(p => p.Name == _tradingViewAlertModel.Name);
            if (alert == null)
                MessageBox.Show("Alert not found!", "Edit Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);

            TradingViewAlerts.Remove(alert);

            DeleteAlertOnList(_tradingViewAlertModel);

            SetEditMode(false);

            listViewAletsList.Focus();

            await TradingViewAlertModel.SaveAsync(TradingViewAlerts);
        }

        #endregion

        #region ListViewAletsList DoubleClick

        private void ListViewAletsList_DoubleClick(object sender, EventArgs e)
        {
            var selectedItem = listViewAletsList.SelectedItems[0];
            if (selectedItem == null)
                return;

            SelectedTradingViewAlertModel = selectedItem.Tag as TradingViewAlertModel;

            SetEditMode(true);
        }

        #endregion

        #region ButtonAlertCancel Click

        private void ButtonAlertCancel_Click(object sender, EventArgs e)
        {
            SetEditMode(false);
        }

        #endregion

        #endregion

        #region Utilities

        #region CustomInitializeComponent

        private void CustomInitializeComponent()
        {
            dateTimePickerAlertExT.Value = DateTime.Now.AddDays(1);
            buttonAddOrEditAlert.Tag = new { EditMode = false };
            comboBoxAlertType.SelectedIndex = 0;
        }

        #endregion

        #region LoadAlertSymbol

        private void LoadAlertSymbol()
        {
            if (TradingViewSymbol == null)
            {
                LoadDefaultAlertSymbol(SelectedSymbols);
                return;
            }

            LoadAlertsList(TradingViewSymbol);

            comboBoxAlertSymbol.Items.Clear();
            comboBoxAlertSymbol.Items.Add(TradingViewSymbol);
            comboBoxAlertSymbol.Enabled = false;
            comboBoxAlertSymbol.SelectedIndex = 0;
        }

        private void LoadDefaultAlertSymbol(IList<TradingViewSymbol> selectedSymbols)
        {
            comboBoxAlertSymbol.Items.Clear();
            comboBoxAlertSymbol.Enabled = true;

            checkBoxAlertEnableExT.Checked = false;
            dateTimePickerAlertExT.Value = DateTime.Now;

            if (!selectedSymbols.Any())
                return;

            LoadAlertsList();

            comboBoxAlertSymbol.Items.AddRange(selectedSymbols.ToArray());

            comboBoxAlertSymbol.SelectedIndex = 0;
        }

        #endregion

        #region InsertAlertOnList

        private void InsertAlertOnList(TradingViewAlertModel _tradingViewAlertModel)
        {
            _tradingViewAlertModel.Number = listViewAletsList.Items.Count + 1;

            var exirationTime = _tradingViewAlertModel.ExirationTime.HasValue ? (int?)(_tradingViewAlertModel.ExirationTime.Value - DateTime.Now).TotalMinutes : null;
            var row = new[]
            {
                _tradingViewAlertModel.Number.ToString(),
                _tradingViewAlertModel.Name,
                _tradingViewAlertModel.Symbol,
                _tradingViewAlertModel.Volume.ToString(),
                _tradingViewAlertModel.Status.ToString(),
                exirationTime.HasValue && exirationTime.Value > 0 ? exirationTime.ToString() : "-",
                _tradingViewAlertModel.Message
            };

            var listViewItem = new ListViewItem(row)
            {
                Tag = _tradingViewAlertModel,
                Name = _tradingViewAlertModel.ToString()
            };

            UpdateAlertSttatusColor(listViewItem, _tradingViewAlertModel.Status);

            listViewAletsList.Items.Add(listViewItem);
        }

        #endregion

        #region UpdateAlertOnList

        private void UpdateAlertOnList(TradingViewAlertModel _tradingViewAlertModel)
        {
            var listViewItem = listViewAletsList.Items[_tradingViewAlertModel.Name];
            if (listViewItem == null)
                return;

            var exirationTime = _tradingViewAlertModel.ExirationTime.HasValue ? (int?)(_tradingViewAlertModel.ExirationTime.Value - DateTime.Now).TotalMinutes : null;

            listViewItem.SubItems[3].Text = _tradingViewAlertModel.Volume.ToString();
            listViewItem.SubItems[4].Text = _tradingViewAlertModel.Status.ToString();
            listViewItem.SubItems[5].Text = exirationTime.HasValue && exirationTime.Value > 0 ? exirationTime.ToString() : "-";
            listViewItem.SubItems[6].Text = _tradingViewAlertModel.Message;

            listViewItem.Tag = _tradingViewAlertModel;

            listViewItem.Selected = true;

            UpdateAlertSttatusColor(listViewItem, _tradingViewAlertModel.Status);
        }

        #endregion

        #region DeleteAlertOnList

        private void DeleteAlertOnList(TradingViewAlertModel _tradingViewAlertModel)
        {
            var listViewItem = listViewAletsList.Items[_tradingViewAlertModel.Name];
            if (listViewItem == null)
                return;

            listViewAletsList.Items.Remove(listViewItem);
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

        #region SetEditMode

        private void SetEditMode(bool state)
        {
            if (state)
            {
                buttonRemoveAlert.Enabled = true;

                buttonAddOrEditAlert.Text = "Save";
                buttonAddOrEditAlert.Tag = new { EditMode = true };

                listViewAletsList.Enabled = false;
                listViewAletsList.SelectedItems.Clear();

                textBoxAlertName.ReadOnly = true;

                buttonAlertCancel.Visible = true;

                return;
            }

            buttonAddOrEditAlert.Text = "Add";
            buttonAddOrEditAlert.Tag = new { EditMode = false };

            buttonRemoveAlert.Enabled = false;

            textBoxAlertName.ReadOnly = false;

            listViewAletsList.Enabled = true;

            buttonAlertCancel.Visible = false;

            if (comboBoxAlertSymbol.Items.Count > 0)
                comboBoxAlertSymbol.Enabled = true;
        }

        #endregion

        #region GenerateAlertName

        private void GenerateAlertName(TradingViewSymbol selectedSymbol)
        {
            var existAlertCount = listViewAletsList.Items
                .Cast<ListViewItem>()
                .Select(p => p.Tag as TradingViewAlertModel)
                .Count(p =>
                    p.Symbol == selectedSymbol.Symbol &&
                    p.Exchange == selectedSymbol.Exchange
                );

            textBoxAlertName.Text = existAlertCount == 0 ? $"{selectedSymbol}-alert" : $"{selectedSymbol}-alert-{existAlertCount + 1}";
        }

        #endregion

        #region AhowAlert

        private void AhowAlert(TradingViewAlertModel alert, TradingViewSymbolPrice data)
        {
            var toastContentBuilder = new ToastContentBuilder()
                .AddText($"Alert For {alert.Symbol}", hintMaxLines: 1)
                .AddText(alert.Message)
                .AddText($"Current Price: {data.Price}")
                .AddCustomTimeStamp(DateTime.Now);

            toastContentBuilder.Show();
        }

        #endregion

        #region UpdateAlertSttatusColor

        private void UpdateAlertSttatusColor(ListViewItem listViewItem, TradingViewAlertStatus status)
        {
            listViewItem.UseItemStyleForSubItems = true;

            switch (status)
            {
                case TradingViewAlertStatus.Active:
                    listViewItem.ForeColor = Color.DarkGreen;
                    listViewItem.BackColor = Color.Honeydew;
                    break;

                case TradingViewAlertStatus.StopedTrigred:
                    listViewItem.ForeColor = Color.DarkOrange;
                    listViewItem.BackColor = Color.PapayaWhip;
                    break;

                case TradingViewAlertStatus.StopedExpired:
                    listViewItem.ForeColor = Color.OrangeRed;
                    listViewItem.BackColor = Color.MistyRose;
                    break;
            }
        }

        #endregion

        #endregion
    }
}
