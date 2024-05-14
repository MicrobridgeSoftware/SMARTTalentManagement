using ApplicationSecurity;
using SmartTalentManagement.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace SmartTalentManagement.GUI.Transactions
{
    public partial class FrmTransactionApproval : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();
        List<TalentManager_EmployeeJobView> employeeJobs;
        private SystemUserGroup loggedInUserGroupProfile;

        public FrmTransactionApproval()
        {
            InitializeComponent();
        }

        private void FrmTransactionApproval_Load(object sender, EventArgs e)
        {
            string user = ApplicationSecurityConstants._activeUser.Trim().ToUpper();
            var userDetails = dbContext.SystemUsers.Where(x => x.UserName.Trim().ToUpper().Equals(user)).FirstOrDefault();

            if (userDetails != null)
                loggedInUserGroupProfile = dbContext.SystemUserGroups.Where(x => x.SystemUserGroupId == userDetails.SystemUserGroupId).AsNoTracking().FirstOrDefault();

            GroupDescriptor groupDescriptor = new GroupDescriptor();
            GroupDescriptor groupDescriptor1 = new GroupDescriptor();
            groupDescriptor.GroupNames.Add("DepartmentName", ListSortDirection.Ascending);
            groupDescriptor1.GroupNames.Add("EmployeeDescription", ListSortDirection.Ascending);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor1);
            grdDataDisplay.MasterTemplate.ExpandAll();

            getFormData();
        }

        private void getFormData()
        {
            employeeJobs = new List<TalentManager_EmployeeJobView>();
            string user = ApplicationSecurityConstants._activeUser.Trim().ToUpper();

            if (ApplicationSecurityConstants._isLocationSpecific)
            {
                int departmentId = ApplicationSecurityConstants._storeLocationId;
                employeeJobs = dbContext.TalentManager_EmployeeJobView.Where(x => !x.IsApproved && !x.IsExported && !x.IsVoided && x.DepartmentId == departmentId && !x.CreatedBy.Trim().ToUpper().Equals(user)).AsNoTracking().ToList();
            }
            else
                employeeJobs = dbContext.TalentManager_EmployeeJobView.Where(x => !x.IsApproved && !x.IsExported && !x.IsVoided && !x.CreatedBy.Trim().ToUpper().Equals(user)).AsNoTracking().ToList();

            //employeeJobs = dbContext.TalentManager_EmployeeJobView.Where(x => !x.IsApproved && !x.IsExported).AsNoTracking().ToList();

            grdDataDisplay.DataSource = employeeJobs;
        }

        private void FrmTransactionApproval_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Select Path to save this export";
            folder.ShowDialog();

            if (string.IsNullOrEmpty(folder.SelectedPath))
            {
                RadMessageBox.Show("A path must be selected to facilitate this export!", Application.ProductName);
                return;
            }

            string destinationPath = folder.SelectedPath;
            string saveAs = "Transactions for approval";
            string fullpath = destinationPath + "\\" + saveAs + ".xlsx";

            bool fileExist = File.Exists(fullpath);

            if (fileExist)
            {
                RadMessageBox.Show("A file containing the same name already exist in this location", Application.ProductName);
                return;
            }

            try
            {
                grdDataDisplay.Columns["Approve"].IsVisible = false;

                PerformExcelExport.ExportGridViewData(grdDataDisplay, fullpath, saveAs);

                grdDataDisplay.Columns["Approve"].IsVisible = true;
            }
            catch(Exception _exp)
            {
                RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
            }
        }

        private void btnApproveAll_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            if (loggedInUserGroupProfile != null && !loggedInUserGroupProfile.ApproveSessionTransactions)
            {
                RadMessageBox.Show("You do not have sufficient user privilege to approve these records!", Application.ProductName);
                return;
            }

            for (int i = 0; i < grdDataDisplay.Rows.Count; i++)
                grdDataDisplay.Rows[i].Cells["Approve"].Value = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            var approvedGridRows = grdDataDisplay.Rows.Where(x => x.Cells["Approve"].Value != null && Convert.ToBoolean(x.Cells["Approve"].Value) == true).ToList();

            if (approvedGridRows.Count > 0)
            {
                if (loggedInUserGroupProfile != null && !loggedInUserGroupProfile.ApproveSessionTransactions)
                {
                    RadMessageBox.Show("You do not have sufficient user privilege to approve these records!", Application.ProductName);
                    return;
                }

                DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

                if (_verifyAction == DialogResult.Yes)
                {
                    try
                    {
                        DateTime jobApprovalDate_Time = DateTime.Now;

                        List<int> employeeJobDetailIdList = new List<int>();

                        foreach (var approval in approvedGridRows)
                        {
                            int jobDetailId = Convert.ToInt32(approval.Cells["EmployeeJobDetailId"].Value);
                            employeeJobDetailIdList.Add(jobDetailId);
                        }

                        List<TalentManager_EmployeeJobDetail> unapprovedJobs = dbContext.TalentManager_EmployeeJobDetail.Where(x => employeeJobDetailIdList.Contains(x.EmployeeJobDetailId)
                                                                               && !x.IsApproved && !x.IsExported && !x.IsVoided).ToList();

                        foreach (TalentManager_EmployeeJobDetail job in unapprovedJobs)
                        {
                            job.IsApproved = true;
                            job.ApprovedBy = ApplicationSecurityConstants._activeUser;
                            job.DateApproved = jobApprovalDate_Time;
                        }

                        dbContext.SaveChanges();

                        RadMessageBox.Show("Record(s) created successfully!", Application.ProductName);

                        getFormData();
                    }
                    catch (Exception _exp)
                    {
                        RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                    }
                }
            }
            else
                RadMessageBox.Show("Select the gridview row(s) that you would like to approve!", Application.ProductName);
        }
    }
}