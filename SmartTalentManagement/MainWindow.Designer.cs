namespace SmartTalentManagement
{
    partial class MainWindow
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
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuProductionLine = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuTask = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem2 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuEmployee = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem5 = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuTransactionEntry = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuTransactionApproval = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem1 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuPayrollExport = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem2 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem3 = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuUserGroups = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuSystemUsers = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuChangePassword = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuSeparatorItem3 = new Telerik.WinControls.UI.RadMenuSeparatorItem();
            this.mnuSystemAudit = new Telerik.WinControls.UI.RadMenuItem();
            this.mnuUserAudit = new Telerik.WinControls.UI.RadMenuItem();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.lblUser = new Telerik.WinControls.UI.RadLabelElement();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.lblDate = new Telerik.WinControls.UI.RadLabelElement();
            this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.lblTime = new Telerik.WinControls.UI.RadLabelElement();
            this.commandBarSeparator3 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.windows7Theme1 = new Telerik.WinControls.Themes.Windows7Theme();
            this.breezeTheme1 = new Telerik.WinControls.Themes.BreezeTheme();
            this.aquaTheme1 = new Telerik.WinControls.Themes.AquaTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMenu1
            // 
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1,
            this.radMenuItem5,
            this.radMenuItem2,
            this.radMenuItem3});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Size = new System.Drawing.Size(823, 25);
            this.radMenu1.TabIndex = 1;
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuProductionLine,
            this.mnuTask,
            this.radMenuSeparatorItem2,
            this.mnuEmployee});
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "System Maintenance";
            // 
            // mnuProductionLine
            // 
            this.mnuProductionLine.Name = "mnuProductionLine";
            this.mnuProductionLine.Text = "Production";
            this.mnuProductionLine.Click += new System.EventHandler(this.mnuProductionLine_Click);
            // 
            // mnuTask
            // 
            this.mnuTask.Name = "mnuTask";
            this.mnuTask.Text = "Task";
            this.mnuTask.Click += new System.EventHandler(this.mnuTask_Click);
            // 
            // radMenuSeparatorItem2
            // 
            this.radMenuSeparatorItem2.Name = "radMenuSeparatorItem2";
            this.radMenuSeparatorItem2.Text = "radMenuSeparatorItem2";
            this.radMenuSeparatorItem2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mnuEmployee
            // 
            this.mnuEmployee.Name = "mnuEmployee";
            this.mnuEmployee.Text = "Employee Contract";
            this.mnuEmployee.Click += new System.EventHandler(this.mnuEmployee_Click);
            // 
            // radMenuItem5
            // 
            this.radMenuItem5.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuTransactionEntry,
            this.mnuTransactionApproval,
            this.radMenuSeparatorItem1,
            this.mnuPayrollExport});
            this.radMenuItem5.Name = "radMenuItem5";
            this.radMenuItem5.Text = "Transactions";
            // 
            // mnuTransactionEntry
            // 
            this.mnuTransactionEntry.Name = "mnuTransactionEntry";
            this.mnuTransactionEntry.Text = "Transaction Entry";
            this.mnuTransactionEntry.Click += new System.EventHandler(this.mnuTransactionEntry_Click);
            // 
            // mnuTransactionApproval
            // 
            this.mnuTransactionApproval.Name = "mnuTransactionApproval";
            this.mnuTransactionApproval.Text = "Transaction Approval";
            this.mnuTransactionApproval.Click += new System.EventHandler(this.mnuTransactionApproval_Click);
            // 
            // radMenuSeparatorItem1
            // 
            this.radMenuSeparatorItem1.Name = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.Text = "radMenuSeparatorItem1";
            this.radMenuSeparatorItem1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mnuPayrollExport
            // 
            this.mnuPayrollExport.Name = "mnuPayrollExport";
            this.mnuPayrollExport.Text = "Export to Payroll";
            this.mnuPayrollExport.Click += new System.EventHandler(this.mnuPayrollExport_Click);
            // 
            // radMenuItem2
            // 
            this.radMenuItem2.Name = "radMenuItem2";
            this.radMenuItem2.Text = "Reports";
            // 
            // radMenuItem3
            // 
            this.radMenuItem3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.mnuUserGroups,
            this.mnuSystemUsers,
            this.mnuChangePassword,
            this.radMenuSeparatorItem3,
            this.mnuSystemAudit,
            this.mnuUserAudit});
            this.radMenuItem3.Name = "radMenuItem3";
            this.radMenuItem3.Text = "Security";
            // 
            // mnuUserGroups
            // 
            this.mnuUserGroups.Name = "mnuUserGroups";
            this.mnuUserGroups.Text = "System User Groups";
            this.mnuUserGroups.Click += new System.EventHandler(this.mnuUserGroups_Click);
            // 
            // mnuSystemUsers
            // 
            this.mnuSystemUsers.Name = "mnuSystemUsers";
            this.mnuSystemUsers.Text = "System Users";
            this.mnuSystemUsers.Click += new System.EventHandler(this.mnuSystemUsers_Click);
            // 
            // mnuChangePassword
            // 
            this.mnuChangePassword.Name = "mnuChangePassword";
            this.mnuChangePassword.Text = "Change User Password";
            this.mnuChangePassword.Click += new System.EventHandler(this.mnuChangePassword_Click);
            // 
            // radMenuSeparatorItem3
            // 
            this.radMenuSeparatorItem3.Name = "radMenuSeparatorItem3";
            this.radMenuSeparatorItem3.Text = "radMenuSeparatorItem3";
            this.radMenuSeparatorItem3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mnuSystemAudit
            // 
            this.mnuSystemAudit.Name = "mnuSystemAudit";
            this.mnuSystemAudit.Text = "System Audit";
            this.mnuSystemAudit.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.mnuSystemAudit.Click += new System.EventHandler(this.mnuSystemAudit_Click);
            // 
            // mnuUserAudit
            // 
            this.mnuUserAudit.Name = "mnuUserAudit";
            this.mnuUserAudit.Text = "User Audit";
            this.mnuUserAudit.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.mnuUserAudit.Click += new System.EventHandler(this.mnuUserAudit_Click);
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.lblUser,
            this.commandBarSeparator1,
            this.lblDate,
            this.commandBarSeparator2,
            this.lblTime,
            this.commandBarSeparator3,
            this.radLabelElement1});
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 380);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(823, 24);
            this.radStatusStrip1.TabIndex = 2;
            // 
            // lblUser
            // 
            this.lblUser.Name = "lblUser";
            this.radStatusStrip1.SetSpring(this.lblUser, false);
            this.lblUser.Text = "radLabelElement1";
            this.lblUser.TextWrap = true;
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.radStatusStrip1.SetSpring(this.commandBarSeparator1, false);
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // lblDate
            // 
            this.lblDate.Name = "lblDate";
            this.radStatusStrip1.SetSpring(this.lblDate, false);
            this.lblDate.Text = "radLabelElement1";
            this.lblDate.TextWrap = true;
            // 
            // commandBarSeparator2
            // 
            this.commandBarSeparator2.Name = "commandBarSeparator2";
            this.radStatusStrip1.SetSpring(this.commandBarSeparator2, false);
            this.commandBarSeparator2.VisibleInOverflowMenu = false;
            // 
            // lblTime
            // 
            this.lblTime.Name = "lblTime";
            this.radStatusStrip1.SetSpring(this.lblTime, false);
            this.lblTime.Text = "radLabelElement2";
            this.lblTime.TextWrap = true;
            // 
            // commandBarSeparator3
            // 
            this.commandBarSeparator3.Name = "commandBarSeparator3";
            this.radStatusStrip1.SetSpring(this.commandBarSeparator3, false);
            this.commandBarSeparator3.VisibleInOverflowMenu = false;
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusStrip1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "POWERED BY MICROBRIDGE SOFTWARE ASSOCIATES LTD.";
            this.radLabelElement1.TextWrap = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 404);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radMenu1);
            this.IsMdiContainer = true;
            this.Name = "MainWindow";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "SMART Talent Management";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadMenuItem mnuTask;
        private Telerik.WinControls.UI.RadMenuItem mnuProductionLine;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem2;
        private Telerik.WinControls.UI.RadMenuItem mnuEmployee;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem5;
        private Telerik.WinControls.UI.RadMenuItem mnuTransactionEntry;
        private Telerik.WinControls.UI.RadMenuItem mnuTransactionApproval;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem1;
        private Telerik.WinControls.UI.RadMenuItem mnuPayrollExport;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem2;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem3;
        private Telerik.WinControls.UI.RadMenuItem mnuUserGroups;
        private Telerik.WinControls.UI.RadMenuItem mnuSystemUsers;
        private Telerik.WinControls.UI.RadMenuItem mnuChangePassword;
        private Telerik.WinControls.UI.RadMenuSeparatorItem radMenuSeparatorItem3;
        private Telerik.WinControls.UI.RadMenuItem mnuSystemAudit;
        private Telerik.WinControls.UI.RadMenuItem mnuUserAudit;
        private Telerik.WinControls.UI.RadLabelElement lblUser;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.RadLabelElement lblDate;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
        private Telerik.WinControls.UI.RadLabelElement lblTime;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator3;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.Themes.Windows7Theme windows7Theme1;
        private Telerik.WinControls.Themes.BreezeTheme breezeTheme1;
        private Telerik.WinControls.Themes.AquaTheme aquaTheme1;
    }
}