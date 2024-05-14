using ApplicationSecurity;
using ApplicationSecurity.PerformanceUtils;
using ApplicationSecurity.UI.Forms;
using SmartTalentManagement.GUI.Maintenance;
using SmartTalentManagement.GUI.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SmartTalentManagement
{
    public partial class MainWindow : Telerik.WinControls.UI.RadForm
    {
        CurrentUserCredentials _credentials = new CurrentUserCredentials();
        SMARTPayEntities dbContext = new SMARTPayEntities();
        private SystemSecurityConfiguration _securityConfig;

        public MainWindow()
        {
            InitializeComponent();
            ThemeResolutionService.ApplicationThemeName = "Breeze";
        }

        private void mnuTask_Click(object sender, EventArgs e)
        {
            FrmTask task = new FrmTask();
            task.MdiParent = this;
            task.Show();
        }

        private void mnuProductionLine_Click(object sender, EventArgs e)
        {
            FrmProductionLine productionLine = new FrmProductionLine();
            productionLine.MdiParent = this;
            productionLine.Show();
        }

        private void mnuEmployee_Click(object sender, EventArgs e)
        {
            FrmEmployee employee = new FrmEmployee();
            employee.MdiParent = this;
            employee.Show();
        }

        private void mnuTransactionEntry_Click(object sender, EventArgs e)
        {
            FrmTransactionEntry transactionEntry = new FrmTransactionEntry();
            transactionEntry.MdiParent = this;
            transactionEntry.Show();
        }

        private void mnuTransactionApproval_Click(object sender, EventArgs e)
        {
            FrmTransactionApproval transactionApproval = new FrmTransactionApproval();
            transactionApproval.MdiParent = this;
            transactionApproval.Show();
        }

        private void mnuPayrollExport_Click(object sender, EventArgs e)
        {
            FrmPayrollTransaction payrollTransaction = new FrmPayrollTransaction();
            payrollTransaction.MdiParent = this;
            payrollTransaction.Show();
        }

        private void mnuUserGroups_Click(object sender, EventArgs e)
        {
            SystemUserGroupForm systemUserGroup = new SystemUserGroupForm();
            systemUserGroup.MdiParent = this;
            systemUserGroup.Show();            
        }

        private void mnuSystemUsers_Click(object sender, EventArgs e)
        {
            SystemUserForm systemUser = new SystemUserForm();
            systemUser.MdiParent = this;
            systemUser.Show();
        }

        private void mnuChangePassword_Click(object sender, EventArgs e)
        {
            SystemUserDisplayForm systemUserDisplay = new SystemUserDisplayForm();
            systemUserDisplay.MdiParent = this;
            systemUserDisplay.Show();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //ApplicationSecurityConstants._activeUser = "Admin";


            lblUser.Text = string.Empty;
            //lblLocation.Text = string.Empty;
            lblDate.Text = string.Empty;
            lblTime.Text = string.Empty;

            _securityConfig = dbContext.SystemSecurityConfigurations.AsNoTracking().FirstOrDefault();

            UserLoginForm _login = new UserLoginForm(_credentials);
            _login.ShowDialog();

            int _userValueCount = _credentials._userCredentials().Count;

            if (_userValueCount == 0)
            {
                //mnuSettings.Enabled = false;
                //mnuInventory.Enabled = false;
                //mnuCRM.Enabled = false;
                //mnuCustomer.Enabled = false;
                //mnuProcessing.Enabled = false;
                //mnuDashBoard.Enabled = false;
                //mnuReports.Enabled = false;
                //mnuSecurity.Enabled = false;
                return;
            }

            lblUser.Text = "CURRENTLY LOGGED IN : " + _credentials.getFirstName().Trim().ToUpper() + " " + _credentials.getLastName().Trim().ToUpper();
            //lblLocation.Text = "LOCATION : " + _credentials._loggedInUserStoreName().Trim().ToUpper();
            lblDate.Text = "DATE LOGGED IN : " + DateTime.Today.ToLongDateString().ToUpper();
            lblTime.Text = "TIME LOGGED IN : " + DateTime.Now.ToShortTimeString();

            ApplicationSecurityConstants._activeUser = _credentials._loggedInUserName();
            ApplicationSecurityConstants._storeLocationId = _credentials._loggedInUserStoreId();
            ApplicationSecurityConstants._isLocationSpecific = _credentials._loggedInUserAccessSpan();

            string user = ApplicationSecurityConstants._activeUser.Trim().ToUpper();

            var _getUserImage = dbContext.SystemUserSettings.Where(x => x.UserName.Trim().ToUpper().Equals(user)).FirstOrDefault();

            if (_getUserImage != null)
            {
                Bitmap bmp2;
                var bytes2 = (byte[])_getUserImage.MdiParentImage;
                MemoryStream ms2 = new MemoryStream(bytes2);
                bmp2 = new Bitmap(ms2);
                this.BackgroundImage = bmp2;

                ms2.Flush();
                ms2.Dispose();
            }

            //EnableUserControls();
            //configureCommandBar();

            //timerAutoLogOut.Enabled = true;

            MemoryManagement.FlushMemory();
        }

        private void mnuSystemAudit_Click(object sender, EventArgs e)
        {
            SystemAuditTrailForm auditTrail = new SystemAuditTrailForm();
            auditTrail.MdiParent = this;
            auditTrail.Show();
        }

        private void mnuUserAudit_Click(object sender, EventArgs e)
        {
            SystemUserLoginAudit userLoginAudit = new SystemUserLoginAudit();
            userLoginAudit.MdiParent = this;
            userLoginAudit.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                DialogResult prompt = RadMessageBox.Show("This action will close the Application, is this the desired result?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

                if (prompt == DialogResult.No)
                    e.Cancel = true;
            }
        }
    }
}