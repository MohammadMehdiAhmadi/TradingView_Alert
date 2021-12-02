
namespace TradingView_Example
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listViewSymbolList = new System.Windows.Forms.ListView();
            this.columnHeaderSymbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderExchange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLast = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChangePer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStriplistViewSymbolList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemAddAlert = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonAddSymbol = new System.Windows.Forms.Button();
            this.textBoxSymbol = new System.Windows.Forms.TextBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.existToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alertsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxUsername = new System.Windows.Forms.ToolStripTextBox();
            this.listViewSearchResult = new System.Windows.Forms.ListView();
            this.searchColumnHeaderSymbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.searchColumnHeaderExchange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBoxExchanges = new System.Windows.Forms.ComboBox();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.existToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelAddListViewGroup = new System.Windows.Forms.Panel();
            this.buttonCancelAddListViewGroup = new System.Windows.Forms.Button();
            this.buttonAddListViewGroup = new System.Windows.Forms.Button();
            this.textBoxListViewGroupName = new System.Windows.Forms.TextBox();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogImport = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStriplistViewSymbolList.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.contextMenuStripNotifyIcon.SuspendLayout();
            this.panelAddListViewGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewSymbolList
            // 
            this.listViewSymbolList.AllowDrop = true;
            this.listViewSymbolList.AutoArrange = false;
            this.listViewSymbolList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewSymbolList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderSymbol,
            this.columnHeaderExchange,
            this.columnHeaderLast,
            this.columnHeaderChange,
            this.columnHeaderChangePer});
            this.listViewSymbolList.ContextMenuStrip = this.contextMenuStriplistViewSymbolList;
            this.listViewSymbolList.FullRowSelect = true;
            this.listViewSymbolList.GridLines = true;
            this.listViewSymbolList.HideSelection = false;
            this.listViewSymbolList.Location = new System.Drawing.Point(13, 27);
            this.listViewSymbolList.MultiSelect = false;
            this.listViewSymbolList.Name = "listViewSymbolList";
            this.listViewSymbolList.ShowItemToolTips = true;
            this.listViewSymbolList.Size = new System.Drawing.Size(403, 422);
            this.listViewSymbolList.TabIndex = 0;
            this.listViewSymbolList.UseCompatibleStateImageBehavior = false;
            this.listViewSymbolList.View = System.Windows.Forms.View.Details;
            this.listViewSymbolList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListViewSymbolList_ColumnClick);
            this.listViewSymbolList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.ListViewSymbolList_ColumnWidthChanging);
            this.listViewSymbolList.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.ListViewSymbolList_DrawColumnHeader);
            this.listViewSymbolList.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListViewSymbolList_DrawItem);
            this.listViewSymbolList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.ListViewSymbolList_ItemDrag);
            this.listViewSymbolList.SelectedIndexChanged += new System.EventHandler(this.ListViewSymbolList_SelectedIndexChanged);
            this.listViewSymbolList.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListViewSymbolList_DragDrop);
            this.listViewSymbolList.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListViewSymbolList_DragEnter);
            this.listViewSymbolList.DragOver += new System.Windows.Forms.DragEventHandler(this.ListViewSymbolList_DragOver);
            this.listViewSymbolList.DragLeave += new System.EventHandler(this.ListViewSymbolList_DragLeave);
            this.listViewSymbolList.DoubleClick += new System.EventHandler(this.ListViewSymbolList_DoubleClick);
            this.listViewSymbolList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListViewSymbolList_MouseMove);
            // 
            // columnHeaderSymbol
            // 
            this.columnHeaderSymbol.Text = "Symbol";
            this.columnHeaderSymbol.Width = 90;
            // 
            // columnHeaderExchange
            // 
            this.columnHeaderExchange.Text = "Exchange";
            this.columnHeaderExchange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderExchange.Width = 90;
            // 
            // columnHeaderLast
            // 
            this.columnHeaderLast.Text = "Last";
            this.columnHeaderLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderLast.Width = 80;
            // 
            // columnHeaderChange
            // 
            this.columnHeaderChange.Text = "Chg";
            this.columnHeaderChange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderChange.Width = 80;
            // 
            // columnHeaderChangePer
            // 
            this.columnHeaderChangePer.Text = "Chg %";
            this.columnHeaderChangePer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // contextMenuStriplistViewSymbolList
            // 
            this.contextMenuStriplistViewSymbolList.Font = new System.Drawing.Font("Tahoma", 9F);
            this.contextMenuStriplistViewSymbolList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStriplistViewSymbolList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.toolStripSeparator2,
            this.toolStripMenuItemAddAlert,
            this.toolStripSeparator1,
            this.addSectionToolStripMenuItem,
            this.removeSectionToolStripMenuItem});
            this.contextMenuStriplistViewSymbolList.Name = "contextMenuStriplistViewSymbolList";
            this.contextMenuStriplistViewSymbolList.Size = new System.Drawing.Size(171, 104);
            this.contextMenuStriplistViewSymbolList.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStriplistViewSymbolList_Opening);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Visible = false;
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            this.toolStripSeparator2.Visible = false;
            // 
            // toolStripMenuItemAddAlert
            // 
            this.toolStripMenuItemAddAlert.Name = "toolStripMenuItemAddAlert";
            this.toolStripMenuItemAddAlert.Size = new System.Drawing.Size(170, 22);
            this.toolStripMenuItemAddAlert.Tag = "Add Alert For {0}";
            this.toolStripMenuItemAddAlert.Text = "Add Alert For {0}";
            this.toolStripMenuItemAddAlert.Click += new System.EventHandler(this.ToolStripMenuItemAddAlert_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // addSectionToolStripMenuItem
            // 
            this.addSectionToolStripMenuItem.Name = "addSectionToolStripMenuItem";
            this.addSectionToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addSectionToolStripMenuItem.Text = "Add Section";
            this.addSectionToolStripMenuItem.Click += new System.EventHandler(this.AddSectionToolStripMenuItem_Click);
            // 
            // removeSectionToolStripMenuItem
            // 
            this.removeSectionToolStripMenuItem.Name = "removeSectionToolStripMenuItem";
            this.removeSectionToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.removeSectionToolStripMenuItem.Text = "Remove Section";
            this.removeSectionToolStripMenuItem.Visible = false;
            this.removeSectionToolStripMenuItem.Click += new System.EventHandler(this.RemoveSectionToolStripMenuItem_Click);
            // 
            // buttonAddSymbol
            // 
            this.buttonAddSymbol.Enabled = false;
            this.buttonAddSymbol.Location = new System.Drawing.Point(422, 397);
            this.buttonAddSymbol.Name = "buttonAddSymbol";
            this.buttonAddSymbol.Size = new System.Drawing.Size(200, 23);
            this.buttonAddSymbol.TabIndex = 2;
            this.buttonAddSymbol.Text = "Add Symbol";
            this.buttonAddSymbol.UseVisualStyleBackColor = true;
            this.buttonAddSymbol.Click += new System.EventHandler(this.ButtonAddSymbol_Click);
            // 
            // textBoxSymbol
            // 
            this.textBoxSymbol.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxSymbol.Enabled = false;
            this.textBoxSymbol.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSymbol.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxSymbol.Location = new System.Drawing.Point(422, 54);
            this.textBoxSymbol.Name = "textBoxSymbol";
            this.textBoxSymbol.Size = new System.Drawing.Size(200, 22);
            this.textBoxSymbol.TabIndex = 3;
            this.textBoxSymbol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxSymbol.TextChanged += new System.EventHandler(this.TextBoxSymbol_TextChanged);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(422, 426);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(200, 23);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.SystemColors.Control;
            this.menuStripMain.Font = new System.Drawing.Font("Tahoma", 9F);
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.loginToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.toolStripTextBoxUsername});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(634, 24);
            this.menuStripMain.TabIndex = 5;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.importToolStripMenuItem,
            this.toolStripSeparator3,
            this.existToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Visible = false;
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.ImportToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // existToolStripMenuItem1
            // 
            this.existToolStripMenuItem1.Name = "existToolStripMenuItem1";
            this.existToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.existToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.existToolStripMenuItem1.Text = "Exist";
            this.existToolStripMenuItem1.Click += new System.EventHandler(this.ExistToolStripMenuItem1_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alertsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.Visible = false;
            // 
            // alertsToolStripMenuItem
            // 
            this.alertsToolStripMenuItem.Enabled = false;
            this.alertsToolStripMenuItem.Name = "alertsToolStripMenuItem";
            this.alertsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.alertsToolStripMenuItem.Text = "Alerts";
            this.alertsToolStripMenuItem.Click += new System.EventHandler(this.AlertsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Enabled = false;
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.LoginToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Visible = false;
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.LogoutToolStripMenuItem_Click);
            // 
            // toolStripTextBoxUsername
            // 
            this.toolStripTextBoxUsername.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxUsername.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripTextBoxUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toolStripTextBoxUsername.Enabled = false;
            this.toolStripTextBoxUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripTextBoxUsername.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.toolStripTextBoxUsername.Margin = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.toolStripTextBoxUsername.Name = "toolStripTextBoxUsername";
            this.toolStripTextBoxUsername.ReadOnly = true;
            this.toolStripTextBoxUsername.Size = new System.Drawing.Size(200, 20);
            this.toolStripTextBoxUsername.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // listViewSearchResult
            // 
            this.listViewSearchResult.AutoArrange = false;
            this.listViewSearchResult.BackColor = System.Drawing.SystemColors.Control;
            this.listViewSearchResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewSearchResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.searchColumnHeaderSymbol,
            this.searchColumnHeaderExchange});
            this.listViewSearchResult.FullRowSelect = true;
            this.listViewSearchResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewSearchResult.HideSelection = false;
            this.listViewSearchResult.Location = new System.Drawing.Point(422, 82);
            this.listViewSearchResult.Name = "listViewSearchResult";
            this.listViewSearchResult.ShowItemToolTips = true;
            this.listViewSearchResult.Size = new System.Drawing.Size(200, 309);
            this.listViewSearchResult.TabIndex = 6;
            this.listViewSearchResult.UseCompatibleStateImageBehavior = false;
            this.listViewSearchResult.View = System.Windows.Forms.View.Details;
            this.listViewSearchResult.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListViewSearchResult_ItemSelectionChanged);
            this.listViewSearchResult.SelectedIndexChanged += new System.EventHandler(this.ListViewSearchResult_SelectedIndexChanged);
            this.listViewSearchResult.DoubleClick += new System.EventHandler(this.ListViewSearchResult_DoubleClick);
            this.listViewSearchResult.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListViewSearchResult_MouseMove);
            // 
            // searchColumnHeaderSymbol
            // 
            this.searchColumnHeaderSymbol.Width = 100;
            // 
            // searchColumnHeaderExchange
            // 
            this.searchColumnHeaderExchange.Width = 80;
            // 
            // comboBoxExchanges
            // 
            this.comboBoxExchanges.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExchanges.Enabled = false;
            this.comboBoxExchanges.FormattingEnabled = true;
            this.comboBoxExchanges.Items.AddRange(new object[] {
            "Binance",
            "Bitfinex",
            "BitMEX",
            "Bitstamp",
            "Bybit",
            "Coinbase",
            "FTX",
            "Huobi",
            "Kraken",
            "KuCoin",
            "PancakeSwap",
            "SushiSwap",
            "Uniswap"});
            this.comboBoxExchanges.Location = new System.Drawing.Point(422, 27);
            this.comboBoxExchanges.Name = "comboBoxExchanges";
            this.comboBoxExchanges.Size = new System.Drawing.Size(200, 21);
            this.comboBoxExchanges.TabIndex = 7;
            this.comboBoxExchanges.SelectedIndexChanged += new System.EventHandler(this.ComboBoxExchanges_SelectedIndexChanged);
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripNotifyIcon;
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "Trading View";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIconMain_MouseClick);
            // 
            // contextMenuStripNotifyIcon
            // 
            this.contextMenuStripNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.existToolStripMenuItem});
            this.contextMenuStripNotifyIcon.Name = "contextMenuStripNotifyIcon";
            this.contextMenuStripNotifyIcon.Size = new System.Drawing.Size(99, 26);
            // 
            // existToolStripMenuItem
            // 
            this.existToolStripMenuItem.Name = "existToolStripMenuItem";
            this.existToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.existToolStripMenuItem.Text = "Exist";
            this.existToolStripMenuItem.Click += new System.EventHandler(this.ExistToolStripMenuItem_Click);
            // 
            // panelAddListViewGroup
            // 
            this.panelAddListViewGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAddListViewGroup.Controls.Add(this.buttonCancelAddListViewGroup);
            this.panelAddListViewGroup.Controls.Add(this.buttonAddListViewGroup);
            this.panelAddListViewGroup.Controls.Add(this.textBoxListViewGroupName);
            this.panelAddListViewGroup.Location = new System.Drawing.Point(65, 206);
            this.panelAddListViewGroup.Name = "panelAddListViewGroup";
            this.panelAddListViewGroup.Size = new System.Drawing.Size(300, 31);
            this.panelAddListViewGroup.TabIndex = 8;
            this.panelAddListViewGroup.Visible = false;
            // 
            // buttonCancelAddListViewGroup
            // 
            this.buttonCancelAddListViewGroup.BackgroundImage = global::TradingView_Example.Properties.Resources.Cancel_16x;
            this.buttonCancelAddListViewGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCancelAddListViewGroup.Location = new System.Drawing.Point(223, 3);
            this.buttonCancelAddListViewGroup.Name = "buttonCancelAddListViewGroup";
            this.buttonCancelAddListViewGroup.Size = new System.Drawing.Size(35, 23);
            this.buttonCancelAddListViewGroup.TabIndex = 1;
            this.buttonCancelAddListViewGroup.UseVisualStyleBackColor = true;
            this.buttonCancelAddListViewGroup.Click += new System.EventHandler(this.ButtonCancelAddListViewGroup_Click);
            // 
            // buttonAddListViewGroup
            // 
            this.buttonAddListViewGroup.BackgroundImage = global::TradingView_Example.Properties.Resources.Add_16x;
            this.buttonAddListViewGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonAddListViewGroup.Location = new System.Drawing.Point(260, 3);
            this.buttonAddListViewGroup.Name = "buttonAddListViewGroup";
            this.buttonAddListViewGroup.Size = new System.Drawing.Size(35, 23);
            this.buttonAddListViewGroup.TabIndex = 1;
            this.buttonAddListViewGroup.UseVisualStyleBackColor = true;
            this.buttonAddListViewGroup.Click += new System.EventHandler(this.ButtonAddListViewGroup_Click);
            // 
            // textBoxListViewGroupName
            // 
            this.textBoxListViewGroupName.Location = new System.Drawing.Point(3, 4);
            this.textBoxListViewGroupName.MaxLength = 32;
            this.textBoxListViewGroupName.Name = "textBoxListViewGroupName";
            this.textBoxListViewGroupName.Size = new System.Drawing.Size(214, 21);
            this.textBoxListViewGroupName.TabIndex = 0;
            this.textBoxListViewGroupName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // saveFileDialogExport
            // 
            this.saveFileDialogExport.DefaultExt = "xml";
            this.saveFileDialogExport.Filter = "XML files (*.xml) | *.xml";
            this.saveFileDialogExport.RestoreDirectory = true;
            this.saveFileDialogExport.Title = "Backup Information";
            // 
            // openFileDialogImport
            // 
            this.openFileDialogImport.DefaultExt = "xml";
            this.openFileDialogImport.Filter = "XML files (*.xml) | *.xml";
            this.openFileDialogImport.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Controls.Add(this.comboBoxExchanges);
            this.Controls.Add(this.listViewSearchResult);
            this.Controls.Add(this.textBoxSymbol);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAddSymbol);
            this.Controls.Add(this.listViewSymbolList);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.panelAddListViewGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trading View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.contextMenuStriplistViewSymbolList.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.contextMenuStripNotifyIcon.ResumeLayout(false);
            this.panelAddListViewGroup.ResumeLayout(false);
            this.panelAddListViewGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewSymbolList;
        private System.Windows.Forms.ColumnHeader columnHeaderSymbol;
        private System.Windows.Forms.ColumnHeader columnHeaderLast;
        private System.Windows.Forms.ColumnHeader columnHeaderChange;
        private System.Windows.Forms.ColumnHeader columnHeaderChangePer;
        private System.Windows.Forms.Button buttonAddSymbol;
        private System.Windows.Forms.TextBox textBoxSymbol;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxUsername;
        private System.Windows.Forms.ListView listViewSearchResult;
        private System.Windows.Forms.ColumnHeader searchColumnHeaderSymbol;
        private System.Windows.Forms.ColumnHeader searchColumnHeaderExchange;
        private System.Windows.Forms.ComboBox comboBoxExchanges;
        private System.Windows.Forms.ColumnHeader columnHeaderExchange;
        private System.Windows.Forms.ContextMenuStrip contextMenuStriplistViewSymbolList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddAlert;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alertsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem existToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem removeSectionToolStripMenuItem;
        private System.Windows.Forms.Panel panelAddListViewGroup;
        private System.Windows.Forms.Button buttonCancelAddListViewGroup;
        private System.Windows.Forms.Button buttonAddListViewGroup;
        private System.Windows.Forms.TextBox textBoxListViewGroupName;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
        private System.Windows.Forms.OpenFileDialog openFileDialogImport;
    }
}

