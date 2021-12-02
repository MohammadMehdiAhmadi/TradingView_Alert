using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using TradingView_Example.Common.Extensions;
using TradingView_Example.Components.Sorter;
using TradingView_Example.Properties;
using TradingView_Example.Services.TradingView;
using TradingView_Example.Services.TradingView.Model;

namespace TradingView_Example
{
    public partial class MainForm : Form
    {
        #region Fields

        private readonly AlertsForm _alertsForm;
        private readonly LoginForm _loginForm;
        private readonly SettingsForm _settingsForm;
        private Timer _typingTimer;

        private readonly ITradingViewClient _tradingViewClient;

        private ListViewColumnSorter _listViewColumnSorter;

        public static IList<TradingViewSymbol> SelectedSymbols { get; set; }

        #endregion

        #region Ctor

        public MainForm()
        {
            InitializeComponent();
            CustomInitializeComponent();

            _tradingViewClient = new TradingViewClient();

            _alertsForm = new AlertsForm();
            _loginForm = new LoginForm(_tradingViewClient);

            _settingsForm = new SettingsForm();
        }

        #endregion

        #region MainForm Load

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadUser();
        }

        #endregion

        #region MainForm Shown

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            if (Settings.Default.Exchange.HasValue())
                comboBoxExchanges.SelectedItem = Settings.Default.Exchange;

            var connectResult = await _tradingViewClient.SocketConnectAsync(Settings.Default.AuthToken).ConfigureAwait(false);
            if (!connectResult)
                MessageBox.Show("Problem connecting to the socket!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            listViewSymbolList.InvokeUi(() => loginToolStripMenuItem.Enabled = true);

            if (Settings.Default.Symbols.Count > 0)
            {
                LoadSelectedSymbols(Settings.Default.Symbols);

                LoadSymbolsOnListView(SelectedSymbols, clearItems: true);

                await SubscribeSymbolsAsync(SelectedSymbols).ConfigureAwait(false);
            }

            await Task.Factory.StartNew(async () => await _tradingViewClient.ListenToAsync(data =>
                listViewSymbolList.InvokeUi(() =>
                {
                    UpdateSelectedSymbols(data);
                    UpdateListViewSymbolList(data);
                }))).ConfigureAwait(false);

            menuStripMain.InvokeUi(() => alertsToolStripMenuItem.Enabled = true);
        }

        private void LoadSymbolsOnListView(IList<TradingViewSymbol> selectedSymbols, bool clearItems = false)
        {
            if (clearItems)
                listViewSymbolList.InvokeUi(() => listViewSymbolList.Items.Clear());

            foreach (var symbol in selectedSymbols)
            {
                var row = new[] { symbol.Symbol, symbol.Exchange, "", "", "" };
                var listViewItem = new ListViewItem(row)
                {
                    Tag = symbol,
                    Name = symbol.ToString()
                };

                listViewSymbolList.InvokeUi(() => listViewSymbolList.Items.Add(listViewItem));
            }

            listViewSearchResult.InvokeUi(() => listViewSearchResult.SelectedItems.Clear());
        }

        private void LoadSelectedSymbols(StringCollection symbols)
        {
            SelectedSymbols = symbols
                .Cast<string>()
                .Select(p => new TradingViewSymbol { Exchange = p.Split(':')[0], Symbol = p.Split(':')[1] })
                .ToList();
        }

        #endregion

        #region TextBoxSymbol TextChanged

        private async void TextBoxSymbol_TextChanged(object sender, EventArgs e)
        {
            var @this = sender as TextBox;
            if (@this.Text.IsNullOrEmpty())
            {
                await SearchSymbolAsync().ConfigureAwait(false);
                return;
            }

            if (_typingTimer == null)
            {
                _typingTimer = new Timer
                {
                    Interval = 300
                };

                _typingTimer.Tick += new EventHandler(SearchSymbolEventAsync);
            }

            _typingTimer.Stop();
            _typingTimer.Tag = (sender as TextBox).Text;
            _typingTimer.Start();
        }

        #endregion

        #region ListViewSearchResult SelectedIndexChanged

        private void ListViewSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSearchResult.SelectedItems.Count == 0)
            {
                buttonAddSymbol.Enabled = false;
                return;
            }

            buttonAddSymbol.Enabled = true;
        }

        #endregion

        #region ListViewSearchResult ItemSelectionChanged

        private void ListViewSearchResult_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var tradingViewSymbol = e.Item.Tag as TradingViewSymbol;

            if (e.IsSelected && !tradingViewSymbol.CanSelect)
                e.Item.Selected = false;
        }

        #endregion

        #region ListViewSearchResult DoubleClick

        private async void ListViewSearchResult_DoubleClick(object sender, EventArgs e)
        {
            await AddSymbol().ConfigureAwait(false);
            textBoxSymbol.InvokeUi(() => textBoxSymbol.Clear());
        }

        #endregion

        #region ButtonAddSymbol Click

        private async void ButtonAddSymbol_Click(object sender, EventArgs e)
        {
            await AddSymbol().ConfigureAwait(false);
            textBoxSymbol.InvokeUi(() => textBoxSymbol.Clear());
        }

        private async Task AddSymbol()
        {
            var selectedItems = listViewSearchResult.SelectedItems;

            ListViewSearchResultMakrckAsSelected(selectedItems);

            var selectedSymbolItems = selectedItems
                .Cast<ListViewItem>()
                .Select(p => (TradingViewSymbol)p.Tag)
                .ToList();

            selectedSymbolItems.ForEach(p =>
            {
                SelectedSymbols.Add(p);
                Settings.Default.Symbols.Add(p.ToString());
            });

            Settings.Default.Save();

            LoadSymbolsOnListView(selectedSymbolItems);

            await SubscribeSymbolsAsync(selectedSymbolItems);
        }

        private async Task SubscribeSymbolsAsync(IList<TradingViewSymbol> selectedItems)
        {
            foreach (var symbol in selectedItems)
            {
                if (_tradingViewClient.SubscribedSymbols.Contains(symbol.ToString()))
                    continue;

                await _tradingViewClient.SubscribeSymbolsAsync(symbol.ToString());
            }
        }

        #endregion

        #region ListViewSymbolList SelectedIndexChanged

        private void ListViewSymbolList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSymbolList.SelectedItems.Count == 0)
            {
                buttonRemove.Enabled = false;
                return;
            }

            buttonRemove.Enabled = true;
        }

        #endregion

        #region ButtonRemove Click

        private async void ButtonRemove_Click(object sender, EventArgs e)
        {
            var selectedItem = listViewSymbolList.SelectedItems[0];
            if (selectedItem == null)
                return;

            await _tradingViewClient.UnsubscribeSymbolsAsync(selectedItem.Name).ConfigureAwait(false);

            ListViewSearchResultMakrckAsUnselected(selectedItem.Name);

            Settings.Default.Symbols.Remove(selectedItem.Name);
            listViewSymbolList.InvokeUi(() => listViewSymbolList.Items.Remove(selectedItem));

            SelectedSymbols.Remove(SelectedSymbols.FirstOrDefault(p => selectedItem.Name == p.ToString()));

            Settings.Default.Save();
        }

        private async Task UnsubscribeSymbolsAsync()
        {
            foreach (var item in SelectedSymbols)
                await _tradingViewClient.UnsubscribeSymbolsAsync(item.ToString()).ConfigureAwait(false);
        }

        #endregion

        #region LoginToolStripMenuItem Click

        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = _loginForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
                LoadUser();
        }

        #endregion

        #region AlertsToolStripMenuItem Click

        private void AlertsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _alertsForm.ShowDialog(null, SelectedSymbols);
        }

        #endregion

        #region ToolStripMenuItemAddAlert Click

        private void ToolStripMenuItemAddAlert_Click(object sender, EventArgs e)
        {
            var selectedItem = listViewSymbolList.SelectedItems[0];
            var tradingViewSymbol = selectedItem.Tag as TradingViewSymbol;
            tradingViewSymbol.CurrentPrice = Convert.ToDecimal(listViewSymbolList.SelectedItems[0].SubItems[2].Text);

            _alertsForm.ShowDialog(tradingViewSymbol);
        }

        #endregion

        #region LogoutToolStripMenuItem Click

        private async void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.Symbols.Count != 0)
            {
                var dialogResult = MessageBox.Show($"Do you want your data to be backed up?", "Backup Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    var backupResult = BackupUserData();
                    if (!backupResult)
                        return;
                }
            }

            await UnsubscribeSymbolsAsync().ConfigureAwait(false);

            Settings.Default.Reset();
            Settings.Default.Symbols = new StringCollection();
            SelectedSymbols = new List<TradingViewSymbol>();

            menuStripMain.InvokeUi(() =>
            {
                loginToolStripMenuItem.Visible = true;
                logoutToolStripMenuItem.Visible = false;
                toolsToolStripMenuItem.Visible = false;
                exportToolStripMenuItem.Visible = false;
                toolStripTextBoxUsername.Clear();
            });


            listViewSearchResult.InvokeUi(() => listViewSearchResult.Items.Clear());
            listViewSymbolList.InvokeUi(() => listViewSymbolList.Items.Clear());

            textBoxSymbol.InvokeUi(() =>
            {
                textBoxSymbol.Enabled = false;
                textBoxSymbol.Clear();
            });

            comboBoxExchanges.InvokeUi(() =>
            {
                comboBoxExchanges.Enabled = false;
                comboBoxExchanges.SelectedIndex = -1;
            });
        }

        #endregion

        #region ListViewSymbolList ColumnWidthChanging

        private void ListViewSymbolList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listViewSymbolList.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        #endregion

        #region ListViewSymbolList DrawColumnHeader

        private void ListViewSymbolList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        #endregion

        #region ListViewSymbolList DrawItem

        private void ListViewSymbolList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        #endregion

        #region ComboBoxExchanges SelectedIndexChanged

        private async void ComboBoxExchanges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxExchanges.SelectedIndex < 0)
            {
                listViewSearchResult.Items.Clear();

                return;
            }

            Settings.Default.Exchange = comboBoxExchanges.SelectedItem.ToString();
            Settings.Default.Save();

            await SearchSymbolAsync().ConfigureAwait(false);
        }

        #endregion

        #region ListViewSymbolList ColumnClick

        private void ListViewSymbolList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var listViewSymbolList = (ListView)sender;

            if (e.Column == _listViewColumnSorter.SortColumn)
            {
                if (_listViewColumnSorter.Order == SortOrder.Ascending)
                    _listViewColumnSorter.Order = SortOrder.Descending;
                else
                    _listViewColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                _listViewColumnSorter.SortColumn = e.Column;
                _listViewColumnSorter.Order = SortOrder.Ascending;
            }

            listViewSymbolList.Sort();

            StoreListViewSearchResultSortChanges();
        }

        #endregion

        #region ListViewSymbolList_ItemDrag

        private void ListViewSymbolList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listViewSymbolList.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        #endregion

        #region ListViewSymbolList_DragEnter

        private void ListViewSymbolList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        #endregion

        #region ListViewSymbolList_DragOver

        private void ListViewSymbolList_DragOver(object sender, DragEventArgs e)
        {
            var targetPoint = listViewSymbolList.PointToClient(new Point(e.X, e.Y));

            var targetIndex = listViewSymbolList.InsertionMark.NearestIndex(targetPoint);
            if (targetIndex > -1)
            {
                var itemBounds = listViewSymbolList.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                    listViewSymbolList.InsertionMark.AppearsAfterItem = true;
                else
                    listViewSymbolList.InsertionMark.AppearsAfterItem = false;
            }

            listViewSymbolList.InsertionMark.Index = targetIndex;
        }

        #endregion

        #region ListViewSymbolList_DragDrop

        private void ListViewSymbolList_DragDrop(object sender, DragEventArgs e)
        {
            var targetIndex = listViewSymbolList.InsertionMark.Index;
            if (targetIndex == -1)
                return;

            _listViewColumnSorter.OrderOfSort = SortOrder.None;

            if (listViewSymbolList.InsertionMark.AppearsAfterItem)
                targetIndex++;

            var draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            var newItem = (ListViewItem)draggedItem.Clone();
            newItem.Name = draggedItem.Name;

            listViewSymbolList.Items.Insert(targetIndex, newItem);
            listViewSymbolList.Items.Remove(draggedItem);

            StoreListViewSearchResultSortChanges();
        }

        #endregion

        #region ListViewSymbolList_DragLeave

        private void ListViewSymbolList_DragLeave(object sender, EventArgs e)
        {
            listViewSymbolList.InsertionMark.Index = -1;
        }

        #endregion

        #region ContextMenuStriplistViewSymbolList Opening

        private void ContextMenuStriplistViewSymbolList_Opening(object sender, CancelEventArgs e)
        {
            ClearAddListViewGroup();

            if (listViewSymbolList.SelectedItems.Count != 1)
            {
                toolStripSeparator1.Visible = false;
                toolStripSeparator2.Visible = false;
                toolStripMenuItemAddAlert.Visible = false;
                removeToolStripMenuItem.Visible = false;
                return;
            }

            var selectedItem = listViewSymbolList.SelectedItems[0];
            var tradingViewSymbol = selectedItem.Tag as TradingViewSymbol;

            toolStripSeparator1.Visible = true;
            toolStripSeparator2.Visible = true;
            toolStripMenuItemAddAlert.Visible = true;
            removeToolStripMenuItem.Visible = true;
            toolStripMenuItemAddAlert.Text = string.Format(toolStripMenuItemAddAlert.Tag.ToString(), tradingViewSymbol.Symbol);
        }

        #endregion

        #region ExistToolStripMenuItem Click

        private void ExistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.MinimizeOnExist = false;
            Application.Exit();
        }

        #endregion

        #region MainForm_FormClosing

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Settings.Default.MinimizeOnExist)
                return;

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            Hide();

            e.Cancel = true;

            return;
        }

        #endregion

        #region NotifyIconMain MouseClick

        private void NotifyIconMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (WindowState != FormWindowState.Minimized)
                return;

            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Visible = true;
            Show();
        }

        #endregion

        #region SettingsToolStripMenuItem Click

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _settingsForm.ShowDialog();
        }

        #endregion

        #region AddSectionToolStripMenuItem Click

        private void AddSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelAddListViewGroup.BringToFront();
            panelAddListViewGroup.Show();
        }

        #endregion

        #region RemoveSectionToolStripMenuItem Click

        private void RemoveSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region ButtonAddListViewGroup Click

        private void ButtonAddListViewGroup_Click(object sender, EventArgs e)
        {
            if (textBoxListViewGroupName.Text.IsNullOrEmpty())
            {
                textBoxListViewGroupName.BackColor = Color.MistyRose;
                return;
            }

            AddNewListViewGroup();
            ClearAddListViewGroup();
        }

        private void AddNewListViewGroup()
        {
            var listVireGroup = new ListViewGroup
            {
                Header = textBoxListViewGroupName.Text,
                Name = textBoxListViewGroupName.Text,
                HeaderAlignment = HorizontalAlignment.Left
            };

            var listViewSymbolListSelectedItem = listViewSymbolList.SelectedItems.Count > 0 ? listViewSymbolList.SelectedItems[0] : null;

            var allUnderSelectedItem = listViewSymbolListSelectedItem != null ? listViewSymbolList.Items.Cast<ListViewItem>().Where(p => p.Index >= listViewSymbolListSelectedItem.Index) : listViewSymbolList.Items.Cast<ListViewItem>();

            listViewSymbolList.Groups.Add(listVireGroup);

            foreach (ListViewItem item in allUnderSelectedItem)
            {
                item.Group = listVireGroup;
                //var newItem = (ListViewItem)item.Clone();
                //newItem.Name = item.Name;
                //newItem.Group = listVireGroup;

                //listViewSymbolList.Items.Remove(item);
                //listViewSymbolList.Items.Add(item);
            }
        }

        #endregion

        #region ButtonCancelAddListViewGroup Click

        private void ButtonCancelAddListViewGroup_Click(object sender, EventArgs e)
        {
            ClearAddListViewGroup();
        }

        private void ClearAddListViewGroup()
        {
            textBoxListViewGroupName.BackColor = Color.White;
            textBoxListViewGroupName.Clear();
            panelAddListViewGroup.SendToBack();
            panelAddListViewGroup.Hide();
        }

        #endregion

        #region RemoveToolStripMenuItem Click

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ButtonRemove_Click(buttonRemove, new EventArgs());
        }

        #endregion

        #region ListViewSymbolList DoubleClick

        private void ListViewSymbolList_DoubleClick(object sender, EventArgs e)
        {
            ToolStripMenuItemAddAlert_Click(toolStripMenuItemAddAlert, new EventArgs());
        }

        #endregion

        #region ListViewSymbolList MouseMove

        private void ListViewSymbolList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y > (listViewSymbolList.Items.Count * 20))
                listViewSymbolList.Cursor = Cursors.Default;
            else
                listViewSymbolList.Cursor = Cursors.Hand;
        }

        #endregion

        #region ListViewSearchResult MouseMove

        private void ListViewSearchResult_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y > (listViewSearchResult.Items.Count * 20))
                listViewSearchResult.Cursor = Cursors.Default;
            else
                listViewSearchResult.Cursor = Cursors.Hand;
        }

        #endregion

        #region ExistToolStripMenuItem1 Click

        private void ExistToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Settings.Default.MinimizeOnExist = false;
            Application.Exit();
        }

        #endregion

        #region ImportToolStripMenuItem Click

        private async void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = openFileDialogImport.ShowDialog();
            if (result != DialogResult.OK)
                return;

            await RestoreUserDataAsync(openFileDialogImport.FileName).ConfigureAwait(false);
        }

        #endregion

        #region ExportToolStripMenuItem Click

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var backupResult = BackupUserData();
            if (backupResult)
                MessageBox.Show($"Data backup was successful.", "Backup Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Utilities

        #region UpdateSelectedSymbols

        private void UpdateSelectedSymbols(TradingViewSymbolPrice data)
        {
            if (!data.Price.HasValue)
                return;

            var symbol = SelectedSymbols.FirstOrDefault(p => p.ToString() == data.Symbol);
            if (symbol == null)
                return;

            symbol.CurrentPrice = data.Price.Value;

            if (!_alertsForm.TradingViewAlerts.Any())
                return;

            _alertsForm.InvokeAlert(data);
        }

        #endregion

        #region UpdateListViewSymbolList

        private void UpdateListViewSymbolList(TradingViewSymbolPrice data)
        {
            if (WindowState != FormWindowState.Normal)
                return;

            var item = listViewSymbolList.Items[data.Symbol];
            if (item == null)
                return;

            if (data.Price.HasValue)
                item.SubItems[2].Text = data.Price.Value.ToString();

            if (data.Change.HasValue)
            {
                item.SubItems[3].Text = data.Change.ToString();

                if (data.Change.Value > 0)
                {
                    item.ForeColor = Color.DarkGreen;
                    item.BackColor = Color.Honeydew;
                    item.UseItemStyleForSubItems = true;
                }
                else
                {
                    item.ForeColor = Color.DarkRed;
                    item.BackColor = Color.MistyRose;
                    item.UseItemStyleForSubItems = true;
                }
            }

            if (data.ChangeP.HasValue)
                item.SubItems[4].Text = $"{data.ChangeP}%";
        }

        #endregion

        #region CustomInitializeComponent

        private void CustomInitializeComponent()
        {
            _listViewColumnSorter = new ListViewColumnSorter();
            SelectedSymbols = new List<TradingViewSymbol>();

            listViewSymbolList.ListViewItemSorter = _listViewColumnSorter;
            listViewSymbolList.InsertionMark.Color = Color.Green;

            if (Settings.Default.Symbols == null)
                Settings.Default.Symbols = new StringCollection();
        }

        #endregion

        #region LoadUser

        private void LoadUser()
        {
            if (!Settings.Default.AuthToken.HasValue())
                return;

            SetEnableStateForm(true);

            loginToolStripMenuItem.Visible = false;
            logoutToolStripMenuItem.Visible = true;
            toolsToolStripMenuItem.Visible = true;
            exportToolStripMenuItem.Visible = true;

            toolStripTextBoxUsername.Text = Settings.Default.Username;

            _tradingViewClient.AuthToken = Settings.Default.AuthToken;
        }

        #endregion

        #region SetEnableStateForm

        private void SetEnableStateForm(bool state)
        {
            listViewSymbolList.Enabled = state;
            textBoxSymbol.Enabled = state;
            comboBoxExchanges.Enabled = state;
            listViewSearchResult.Enabled = state;
        }

        #endregion

        #region SearchSymbolAsync

        private async void SearchSymbolEventAsync(object sender, EventArgs e)
        {
            if (!(sender is Timer timer))
                return;

            timer.Stop();

            await SearchSymbolAsync();
        }

        private async Task SearchSymbolAsync()
        {
            var exchange = string.Empty;
            comboBoxExchanges.InvokeUi(() => exchange = comboBoxExchanges.SelectedIndex >= 0 ? comboBoxExchanges.SelectedItem.ToString() : null);

            var searchedSymbol = textBoxSymbol.Text;
            var searchResult = await _tradingViewClient.SearchSymbolAsync(searchedSymbol, exchange).ConfigureAwait(false);
            if (searchResult.Item1.HasValue())
                MessageBox.Show(searchResult.Item1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            var result = searchResult.Item2
                .OrderByDescending(p => p.Symbol.Equals($"{searchedSymbol}USDT"))
                .ThenByDescending(p => p.Symbol.EndsWith("USDT"))
                .ToList();

            FillListBox(result);
        }

        #endregion

        #region FillListBox

        private void FillListBox(IList<TradingViewSymbol> symbols)
        {
            listViewSearchResult.InvokeUi(() => listViewSearchResult.Items.Clear());


            foreach (var symbol in symbols)
            {
                var key = $"{symbol.Exchange}:{symbol.Symbol}";
                symbol.CanSelect = !Settings.Default.Symbols.Contains(key);

                var row = new[] { symbol.Symbol, symbol.Exchange };
                var listViewItem = new ListViewItem(row)
                {
                    Tag = symbol,
                    Name = key
                };

                listViewItem.ForeColor = symbol.CanSelect ? SystemColors.WindowText : SystemColors.ScrollBar;
                listViewItem.UseItemStyleForSubItems = true;

                listViewSearchResult.InvokeUi(() => listViewSearchResult.Items.Add(listViewItem));
            }
        }

        #endregion

        #region ListViewSearchResultMakrckAsSelected

        private void ListViewSearchResultMakrckAsSelected(ListView.SelectedListViewItemCollection selectedItems)
        {
            foreach (ListViewItem item in selectedItems)
            {
                var tradingViewSymbol = item.Tag as TradingViewSymbol;
                tradingViewSymbol.CanSelect = false;

                item.Tag = tradingViewSymbol;
                item.ForeColor = SystemColors.ScrollBar;
                item.UseItemStyleForSubItems = true;
            }
        }

        private void ListViewSearchResultMakrckAsUnselected(string symbolKey)
        {
            ListViewItem item = null;
            listViewSearchResult.InvokeUi(() => item = listViewSearchResult.Items[symbolKey]);
            if (item == null)
                return;

            var tradingViewSymbol = item.Tag as TradingViewSymbol;
            tradingViewSymbol.CanSelect = true;

            item.Tag = tradingViewSymbol;
            item.ForeColor = SystemColors.WindowText;
            item.UseItemStyleForSubItems = true;
        }

        #endregion

        #region StoreListViewSearchResultSortChanges

        private void StoreListViewSearchResultSortChanges()
        {
            Settings.Default.Symbols.Clear();

            var selectedSymbolItems = listViewSymbolList.Items
                .Cast<ListViewItem>()
                .Select(p => (TradingViewSymbol)p.Tag)
                .Select(p => p.ToString())
                .ToArray();

            Settings.Default.Symbols.AddRange(selectedSymbolItems);
            Settings.Default.Save();
        }

        #endregion

        #region BackupUserData

        private bool BackupUserData()
        {
            Settings.Default.Save();

            saveFileDialogExport.FileName = $"TradingView-{Settings.Default.Username}-{DateTime.Now:MMddyyyyHHmmss}.xml";
            var result = saveFileDialogExport.ShowDialog();
            if (result != DialogResult.OK)
                return true;

            if (Path.GetExtension(saveFileDialogExport.FileName).ToLower() != ".xml")
            {
                MessageBox.Show($"The file extension required to store the information must be '.xml'.", "Backup Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                config.SaveAs(saveFileDialogExport.FileName, ConfigurationSaveMode.Full, forceSaveAll: true);

                return true;
            }
            catch (Exception EX)
            {
                MessageBox.Show($"Error on saving file!{Environment.NewLine}Message:{EX.Message}", "Backup Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region RestoreUserData

        private async Task<bool> RestoreUserDataAsync(string settingsFilePath)
        {
            var appSettings = Settings.Default;
            var userLogedIn = appSettings.AuthToken.HasValue();

            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                var appSettingsXmlName = appSettings.Context["GroupName"].ToString();
                var import = XDocument.Load(settingsFilePath);
                var settings = import.XPathSelectElements("//" + appSettingsXmlName).Single();

                if (userLogedIn)
                {
                    settings.Elements().Single(p => p.Attribute("name").Value == "UserId").SetElementValue("value", appSettings.UserId);
                    settings.Elements().Single(p => p.Attribute("name").Value == "Username").SetElementValue("value", appSettings.Username);
                    settings.Elements().Single(p => p.Attribute("name").Value == "FirstName").SetElementValue("value", appSettings.FirstName);
                    settings.Elements().Single(p => p.Attribute("name").Value == "LastName").SetElementValue("value", appSettings.LastName);
                    settings.Elements().Single(p => p.Attribute("name").Value == "AuthToken").SetElementValue("value", appSettings.AuthToken);
                }

                config.GetSectionGroup("userSettings")
                    .Sections[appSettingsXmlName]
                    .SectionInformation
                    .SetRawXml(settings.ToString());

                config.Save(ConfigurationSaveMode.Full, forceSaveAll: true);
                ConfigurationManager.RefreshSection("userSettings");

                appSettings.Reload();
                appSettings.Save();

                if (appSettings.Exchange.HasValue())
                    comboBoxExchanges.SelectedItem = appSettings.Exchange;

                if (!userLogedIn)
                    LoadUser();

                if (appSettings.Symbols.Count > 0)
                {
                    LoadSelectedSymbols(appSettings.Symbols);

                    await UnsubscribeSymbolsAsync().ConfigureAwait(false);

                    LoadSymbolsOnListView(SelectedSymbols, clearItems: true);

                    await SubscribeSymbolsAsync(SelectedSymbols).ConfigureAwait(false);

                    await SearchSymbolAsync().ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception EX)
            {
                MessageBox.Show($"Error on resoring file!{Environment.NewLine}Message:{EX.Message}", "Backup Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                appSettings.Reload();
                return false;
            }
        }

        #endregion

        #endregion
    }
}