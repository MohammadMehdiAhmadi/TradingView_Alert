using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            Action safeWrite = delegate
            {
                loginToolStripMenuItem.Enabled = true;
            };
            listViewSymbolList.Invoke(safeWrite);

            if (Settings.Default.Symbols.Count > 0)
            {
                SelectedSymbols = Settings.Default.Symbols
                    .Cast<string>()
                    .Select(p => new TradingViewSymbol { Exchange = p.Split(':')[0], Symbol = p.Split(':')[1] })
                    .ToList();

                await SubscribeSymbolsAsync(SelectedSymbols).ConfigureAwait(false);
            }

            await Task.Factory.StartNew(async () =>
                await _tradingViewClient.LestenToAsync(data =>
                    listViewSymbolList.InvokeUi(() =>
                        UpdateListViewSymbolList(data)))).ConfigureAwait(false);

            menuStripMain.InvokeUi(() => alertsToolStripMenuItem.Enabled = true);
        }

        #endregion

        #region TextBoxSymbol TextChanged

        private async void TextBoxSymbol_TextChanged(object sender, EventArgs e)
        {
            var @this = sender as TextBox;
            if (@this.Text.IsNullOrEmpty())
            {
                await SearchSymbolAsync();
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
            await AddSymbol();
            textBoxSymbol.Clear();
        }

        #endregion

        #region ButtonAddSymbol Click

        private async void ButtonAddSymbol_Click(object sender, EventArgs e)
        {
            await AddSymbol();
            textBoxSymbol.Clear();
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

            await SubscribeSymbolsAsync(selectedSymbolItems);
        }

        private async Task SubscribeSymbolsAsync(IList<TradingViewSymbol> selectedItems)
        {
            foreach (var symbol in selectedItems)
            {
                var key = $"{symbol.Exchange}:{symbol.Symbol}";

                var existSymbol = false;
                if (listViewSymbolList.InvokeRequired)
                {
                    Action safeWrite = delegate { existSymbol = listViewSymbolList.Items.ContainsKey(key); };

                    listViewSymbolList.Invoke(safeWrite);
                }
                else
                    existSymbol = listViewSymbolList.Items.ContainsKey(key);

                if (existSymbol)
                    continue;


                var row = new[] { symbol.Symbol, symbol.Exchange, "", "", "" };
                var listViewItem = new ListViewItem(row)
                {
                    Tag = symbol,
                    Name = key
                };

                listViewSymbolList.InvokeUi(() => listViewSymbolList.Items.Add(listViewItem));

                await _tradingViewClient.SubscribeSymbolsAsync(key);
            }

            listViewSymbolList.InvokeUi(() => listViewSearchResult.SelectedItems.Clear());
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

            await _tradingViewClient.UnsubscribeSymbolsAsync(selectedItem.Name);

            ListViewSearchResultMakrckAsUnselected(selectedItem.Name);

            Settings.Default.Symbols.Remove(selectedItem.Name);
            listViewSymbolList.Items.Remove(selectedItem);

            SelectedSymbols.Remove(SelectedSymbols.FirstOrDefault(p => selectedItem.Name == p.ToString()));

            Settings.Default.Save();
        }

        private async Task UnsubscribeSymbolsAsync()
        {
            foreach (var item in SelectedSymbols)
                await _tradingViewClient.UnsubscribeSymbolsAsync(item.ToString());
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
            var dialogResult = MessageBox.Show($"Are you sure you want to log out?{Environment.NewLine}All your information will be deleted if you log out.", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
                return;


            await UnsubscribeSymbolsAsync();

            Settings.Default.Reset();
            Settings.Default.Symbols = new StringCollection();
            SelectedSymbols = new List<TradingViewSymbol>();

            loginToolStripMenuItem.Visible = true;
            logoutToolStripMenuItem.Visible = false;
            optionsToolStripMenuItem.Visible = false;


            toolStripTextBoxUsername.Clear();

            listViewSearchResult.Items.Clear();
            listViewSymbolList.Items.Clear();

            textBoxSymbol.Enabled = false;
            textBoxSymbol.Clear();

            comboBoxExchanges.Enabled = false;
            comboBoxExchanges.SelectedIndex = -1;
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

            await SearchSymbolAsync();
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
            e.Cancel = listViewSymbolList.SelectedItems.Count != 1;
            if (e.Cancel)
                return;

            var selectedItem = listViewSymbolList.SelectedItems[0];
            var tradingViewSymbol = selectedItem.Tag as TradingViewSymbol;

            toolStripMenuItemAddAlert.Text = string.Format(toolStripMenuItemAddAlert.Tag.ToString(), tradingViewSymbol.Symbol);
            toolStripMenuItemRemoveAlert.Text = string.Format(toolStripMenuItemRemoveAlert.Tag.ToString(), tradingViewSymbol.Symbol);
        }

        #endregion

        #region Utilites

        #region UpdateListViewSymbolList

        private void UpdateListViewSymbolList(TradingViewSymbolPrice data)
        {
            var item = listViewSymbolList.Items[data.Symbol];
            if (item == null)
                return;

            if (data.Price.HasValue)
            {
                item.SubItems[2].Text = data.Price.Value.ToString();

                _alertsForm.InvokeAlert(data);

                var symbol = SelectedSymbols.FirstOrDefault(p => p.ToString() == data.Symbol);
                symbol.CurrentPrice = data.Price.Value;
            }

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
            optionsToolStripMenuItem.Visible = true;

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
            var exchange = comboBoxExchanges.SelectedIndex >= 0 ? comboBoxExchanges.SelectedItem.ToString() : null;

            var searchedSymbol = textBoxSymbol.Text;
            var searchResult = await _tradingViewClient.SearchSymbolAsync(searchedSymbol, exchange);
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
            listViewSearchResult.Items.Clear();


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

                listViewSearchResult.Items.Add(listViewItem);
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
            var item = listViewSearchResult.Items[symbolKey];
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

        #endregion
    }
}