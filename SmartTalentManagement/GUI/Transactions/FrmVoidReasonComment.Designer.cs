namespace SmartTalentManagement.GUI.Transactions
{
    partial class FrmVoidReasonComment
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
            this.txtVoidReson = new Telerik.WinControls.UI.RadTextBox();
            this.btnVoid = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVoidReson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnVoid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.txtVoidReson);
            this.radGroupBox1.HeaderMargin = new System.Windows.Forms.Padding(1);
            this.radGroupBox1.HeaderText = "Void Reason";
            this.radGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(475, 156);
            this.radGroupBox1.TabIndex = 0;
            this.radGroupBox1.Text = "Void Reason";
            // 
            // txtVoidReson
            // 
            this.txtVoidReson.AcceptsReturn = true;
            this.txtVoidReson.Location = new System.Drawing.Point(5, 21);
            this.txtVoidReson.Multiline = true;
            this.txtVoidReson.Name = "txtVoidReson";
            // 
            // 
            // 
            this.txtVoidReson.RootElement.StretchVertically = true;
            this.txtVoidReson.Size = new System.Drawing.Size(465, 130);
            this.txtVoidReson.TabIndex = 0;
            // 
            // btnVoid
            // 
            this.btnVoid.Location = new System.Drawing.Point(193, 182);
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.Size = new System.Drawing.Size(110, 34);
            this.btnVoid.TabIndex = 1;
            this.btnVoid.Text = "Continue";
            this.btnVoid.Click += new System.EventHandler(this.btnVoid_Click);
            // 
            // FrmVoidReasonComment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 222);
            this.Controls.Add(this.btnVoid);
            this.Controls.Add(this.radGroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVoidReasonComment";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Void Comment";
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVoidReson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnVoid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadTextBox txtVoidReson;
        private Telerik.WinControls.UI.RadButton btnVoid;
    }
}
