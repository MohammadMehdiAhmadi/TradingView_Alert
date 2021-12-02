namespace TradingView_Example
{
    partial class SettingsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxMinimizedStart = new System.Windows.Forms.CheckBox();
            this.checkBoxMinimizeOnExist = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxMinimizedStart);
            this.groupBox1.Controls.Add(this.checkBoxMinimizeOnExist);
            this.groupBox1.Controls.Add(this.checkBoxAutoStart);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 137);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System Integration";
            // 
            // checkBoxMinimizedStart
            // 
            this.checkBoxMinimizedStart.AutoSize = true;
            this.checkBoxMinimizedStart.Location = new System.Drawing.Point(21, 68);
            this.checkBoxMinimizedStart.Name = "checkBoxMinimizedStart";
            this.checkBoxMinimizedStart.Size = new System.Drawing.Size(108, 17);
            this.checkBoxMinimizedStart.TabIndex = 0;
            this.checkBoxMinimizedStart.Text = "Launch minimized";
            this.checkBoxMinimizedStart.UseVisualStyleBackColor = true;
            this.checkBoxMinimizedStart.CheckedChanged += new System.EventHandler(this.CheckBoxMinimizedStart_CheckedChanged);
            // 
            // checkBoxMinimizeOnExist
            // 
            this.checkBoxMinimizeOnExist.AutoSize = true;
            this.checkBoxMinimizeOnExist.Location = new System.Drawing.Point(21, 22);
            this.checkBoxMinimizeOnExist.Name = "checkBoxMinimizeOnExist";
            this.checkBoxMinimizeOnExist.Size = new System.Drawing.Size(106, 17);
            this.checkBoxMinimizeOnExist.TabIndex = 0;
            this.checkBoxMinimizeOnExist.Text = "Minimize on exist";
            this.checkBoxMinimizeOnExist.UseVisualStyleBackColor = true;
            this.checkBoxMinimizeOnExist.CheckedChanged += new System.EventHandler(this.CheckBoxMinimizeOnExist_CheckedChanged);
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(21, 45);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(178, 17);
            this.checkBoxAutoStart.TabIndex = 0;
            this.checkBoxAutoStart.Text = "Launch app when system starts";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            this.checkBoxAutoStart.CheckedChanged += new System.EventHandler(this.CheckBoxAutoStart_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxMinimizedStart;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
        private System.Windows.Forms.CheckBox checkBoxMinimizeOnExist;
    }
}