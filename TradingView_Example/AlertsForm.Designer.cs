using TradingView_Example.Components.Controls;

namespace TradingView_Example
{
    partial class AlertsForm
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
            this.listViewAletsList = new System.Windows.Forms.ListView();
            this.columnHeaderAlertNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlertName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlertPair = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlertPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlertStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlertExpirationTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAlertMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxAddAlert = new System.Windows.Forms.GroupBox();
            this.textBoxAlertName = new System.Windows.Forms.TextBox();
            this.textBoxAlertMessage = new System.Windows.Forms.TextBox();
            this.checkBoxAlertEnableExT = new System.Windows.Forms.CheckBox();
            this.dateTimePickerAlertExT = new System.Windows.Forms.DateTimePicker();
            this.buttonRemoveAlert = new System.Windows.Forms.Button();
            this.buttonAddOrEditAlert = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxAlertSymbol = new System.Windows.Forms.ComboBox();
            this.numericUpDownAlertValue = new TradingView_Example.Components.Controls.CustomNumericUpDown();
            this.groupBoxAddAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAlertValue)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewAletsList
            // 
            this.listViewAletsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAlertNumber,
            this.columnHeaderAlertName,
            this.columnHeaderAlertPair,
            this.columnHeaderAlertPrice,
            this.columnHeaderAlertStatus,
            this.columnHeaderAlertExpirationTime,
            this.columnHeaderAlertMessage});
            this.listViewAletsList.FullRowSelect = true;
            this.listViewAletsList.GridLines = true;
            this.listViewAletsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewAletsList.HideSelection = false;
            this.listViewAletsList.Location = new System.Drawing.Point(12, 236);
            this.listViewAletsList.Name = "listViewAletsList";
            this.listViewAletsList.ShowGroups = false;
            this.listViewAletsList.ShowItemToolTips = true;
            this.listViewAletsList.Size = new System.Drawing.Size(410, 213);
            this.listViewAletsList.TabIndex = 8;
            this.listViewAletsList.UseCompatibleStateImageBehavior = false;
            this.listViewAletsList.View = System.Windows.Forms.View.Details;
            this.listViewAletsList.SelectedIndexChanged += new System.EventHandler(this.ListViewAletsList_SelectedIndexChanged);
            // 
            // columnHeaderAlertNumber
            // 
            this.columnHeaderAlertNumber.Text = "#";
            this.columnHeaderAlertNumber.Width = 30;
            // 
            // columnHeaderAlertName
            // 
            this.columnHeaderAlertName.Text = "Name";
            this.columnHeaderAlertName.Width = 70;
            // 
            // columnHeaderAlertPair
            // 
            this.columnHeaderAlertPair.Text = "Pair";
            this.columnHeaderAlertPair.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderAlertPrice
            // 
            this.columnHeaderAlertPrice.Text = "Price";
            this.columnHeaderAlertPrice.Width = 70;
            // 
            // columnHeaderAlertStatus
            // 
            this.columnHeaderAlertStatus.Text = "Status";
            this.columnHeaderAlertStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderAlertExpirationTime
            // 
            this.columnHeaderAlertExpirationTime.Text = "Ex.Time (m)";
            this.columnHeaderAlertExpirationTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderAlertExpirationTime.Width = 70;
            // 
            // columnHeaderAlertMessage
            // 
            this.columnHeaderAlertMessage.Text = "Message";
            this.columnHeaderAlertMessage.Width = 90;
            // 
            // groupBoxAddAlert
            // 
            this.groupBoxAddAlert.Controls.Add(this.numericUpDownAlertValue);
            this.groupBoxAddAlert.Controls.Add(this.textBoxAlertName);
            this.groupBoxAddAlert.Controls.Add(this.textBoxAlertMessage);
            this.groupBoxAddAlert.Controls.Add(this.checkBoxAlertEnableExT);
            this.groupBoxAddAlert.Controls.Add(this.dateTimePickerAlertExT);
            this.groupBoxAddAlert.Controls.Add(this.buttonRemoveAlert);
            this.groupBoxAddAlert.Controls.Add(this.buttonAddOrEditAlert);
            this.groupBoxAddAlert.Controls.Add(this.label4);
            this.groupBoxAddAlert.Controls.Add(this.label2);
            this.groupBoxAddAlert.Controls.Add(this.label3);
            this.groupBoxAddAlert.Controls.Add(this.label5);
            this.groupBoxAddAlert.Controls.Add(this.label1);
            this.groupBoxAddAlert.Controls.Add(this.comboBoxAlertSymbol);
            this.groupBoxAddAlert.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAddAlert.Name = "groupBoxAddAlert";
            this.groupBoxAddAlert.Size = new System.Drawing.Size(410, 218);
            this.groupBoxAddAlert.TabIndex = 1;
            this.groupBoxAddAlert.TabStop = false;
            this.groupBoxAddAlert.Text = "Add Alert";
            // 
            // textBoxAlertName
            // 
            this.textBoxAlertName.Location = new System.Drawing.Point(113, 20);
            this.textBoxAlertName.MaxLength = 32;
            this.textBoxAlertName.Name = "textBoxAlertName";
            this.textBoxAlertName.Size = new System.Drawing.Size(200, 24);
            this.textBoxAlertName.TabIndex = 0;
            // 
            // textBoxAlertMessage
            // 
            this.textBoxAlertMessage.Location = new System.Drawing.Point(113, 128);
            this.textBoxAlertMessage.MaxLength = 256;
            this.textBoxAlertMessage.Multiline = true;
            this.textBoxAlertMessage.Name = "textBoxAlertMessage";
            this.textBoxAlertMessage.Size = new System.Drawing.Size(200, 53);
            this.textBoxAlertMessage.TabIndex = 5;
            // 
            // checkBoxAlertEnableExT
            // 
            this.checkBoxAlertEnableExT.AutoSize = true;
            this.checkBoxAlertEnableExT.Location = new System.Drawing.Point(96, 106);
            this.checkBoxAlertEnableExT.Name = "checkBoxAlertEnableExT";
            this.checkBoxAlertEnableExT.Size = new System.Drawing.Size(18, 17);
            this.checkBoxAlertEnableExT.TabIndex = 3;
            this.checkBoxAlertEnableExT.UseVisualStyleBackColor = true;
            this.checkBoxAlertEnableExT.CheckedChanged += new System.EventHandler(this.CheckBoxAlertEnableExT_CheckedChanged);
            // 
            // dateTimePickerAlertExT
            // 
            this.dateTimePickerAlertExT.CustomFormat = "ddd, dd MMM yyyy HH:mm:ss";
            this.dateTimePickerAlertExT.Enabled = false;
            this.dateTimePickerAlertExT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerAlertExT.Location = new System.Drawing.Point(113, 101);
            this.dateTimePickerAlertExT.Name = "dateTimePickerAlertExT";
            this.dateTimePickerAlertExT.Size = new System.Drawing.Size(200, 24);
            this.dateTimePickerAlertExT.TabIndex = 4;
            // 
            // buttonRemoveAlert
            // 
            this.buttonRemoveAlert.Enabled = false;
            this.buttonRemoveAlert.Location = new System.Drawing.Point(250, 187);
            this.buttonRemoveAlert.Name = "buttonRemoveAlert";
            this.buttonRemoveAlert.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveAlert.TabIndex = 6;
            this.buttonRemoveAlert.Text = "Remove";
            this.buttonRemoveAlert.UseVisualStyleBackColor = true;
            this.buttonRemoveAlert.Click += new System.EventHandler(this.ButtonRemoveAlert_Click);
            // 
            // buttonAddOrEditAlert
            // 
            this.buttonAddOrEditAlert.Location = new System.Drawing.Point(331, 187);
            this.buttonAddOrEditAlert.Name = "buttonAddOrEditAlert";
            this.buttonAddOrEditAlert.Size = new System.Drawing.Size(75, 23);
            this.buttonAddOrEditAlert.TabIndex = 7;
            this.buttonAddOrEditAlert.Text = "Add";
            this.buttonAddOrEditAlert.UseVisualStyleBackColor = true;
            this.buttonAddOrEditAlert.Click += new System.EventHandler(this.ButtonAddOrEditAlert_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Alert name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Expireation time:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Message:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Value:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Condition:";
            // 
            // comboBoxAlertSymbol
            // 
            this.comboBoxAlertSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlertSymbol.FormattingEnabled = true;
            this.comboBoxAlertSymbol.Location = new System.Drawing.Point(113, 47);
            this.comboBoxAlertSymbol.Name = "comboBoxAlertSymbol";
            this.comboBoxAlertSymbol.Size = new System.Drawing.Size(200, 25);
            this.comboBoxAlertSymbol.TabIndex = 1;
            this.comboBoxAlertSymbol.SelectedIndexChanged += new System.EventHandler(this.ComboBoxAlertSymbol_SelectedIndexChanged);
            // 
            // numericUpDownAlertValue
            // 
            this.numericUpDownAlertValue.DecimalPlaces = 8;
            this.numericUpDownAlertValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownAlertValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownAlertValue.Location = new System.Drawing.Point(113, 74);
            this.numericUpDownAlertValue.Maximum = new decimal(new int[] {
            -469762049,
            -590869294,
            5421010,
            0});
            this.numericUpDownAlertValue.Name = "numericUpDownAlertValue";
            this.numericUpDownAlertValue.Size = new System.Drawing.Size(200, 24);
            this.numericUpDownAlertValue.TabIndex = 2;
            this.numericUpDownAlertValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownAlertValue.ThousandsSeparator = true;
            this.numericUpDownAlertValue.ValueChanged += new System.EventHandler(this.NumericUpDownAlertValue_ValueChanged);
            // 
            // AlertsForm
            // 
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.groupBoxAddAlert);
            this.Controls.Add(this.listViewAletsList);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlertsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alerts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AlertsForm_FormClosed);
            this.Load += new System.EventHandler(this.AlertsForm_Load);
            this.Shown += new System.EventHandler(this.AlertsForm_Shown);
            this.groupBoxAddAlert.ResumeLayout(false);
            this.groupBoxAddAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAlertValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewAletsList;
        private System.Windows.Forms.GroupBox groupBoxAddAlert;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxAlertSymbol;
        private System.Windows.Forms.CheckBox checkBoxAlertEnableExT;
        private System.Windows.Forms.DateTimePicker dateTimePickerAlertExT;
        private System.Windows.Forms.Button buttonRemoveAlert;
        private System.Windows.Forms.Button buttonAddOrEditAlert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAlertMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAlertName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertNumber;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertName;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertPair;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertExpirationTime;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertMessage;
        private CustomNumericUpDown numericUpDownAlertValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeaderAlertPrice;
    }
}