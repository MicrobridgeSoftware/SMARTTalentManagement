namespace SmartTalentManagement.GUI.Maintenance
{
    partial class FrmEmployeeDayRate
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
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.spnRate = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnContinue = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnContinue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.spnRate);
            this.radGroupBox1.Controls.Add(this.radLabel1);
            this.radGroupBox1.HeaderMargin = new System.Windows.Forms.Padding(1);
            this.radGroupBox1.HeaderText = "Rate per day";
            this.radGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(311, 100);
            this.radGroupBox1.TabIndex = 0;
            this.radGroupBox1.Text = "Rate per day";
            // 
            // spnRate
            // 
            this.spnRate.DecimalPlaces = 2;
            this.spnRate.Location = new System.Drawing.Point(119, 44);
            this.spnRate.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.spnRate.Name = "spnRate";
            this.spnRate.ShowUpDownButtons = false;
            this.spnRate.Size = new System.Drawing.Size(165, 24);
            this.spnRate.TabIndex = 1;
            this.spnRate.TextAlignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.spnRate.ThousandsSeparator = true;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(20, 45);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(93, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Payment Amount";
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnContinue.Image = global::SmartTalentManagement.Properties.Resources.Thumbup_24;
            this.btnContinue.Location = new System.Drawing.Point(110, 121);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(115, 31);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Update Rate";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // FrmEmployeeDayRate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 164);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.radGroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEmployeeDayRate";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Employee Day Rate";
            this.Load += new System.EventHandler(this.FrmEmployeeDayRate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spnRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnContinue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadSpinEditor spnRate;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnContinue;
    }
}
