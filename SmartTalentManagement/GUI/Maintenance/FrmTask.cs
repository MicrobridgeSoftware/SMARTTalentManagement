using ApplicationSecurity;
using ApplicationSecurity.SecurityUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace SmartTalentManagement.GUI.Maintenance
{
    public partial class FrmTask : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();
        List<TalentManager_DutyCodeView> dutyCodeList;
        List<TalentManager_CompanyView> companyList;
        List<TalentManager_WageType> wageTypeList;
        List<TalentManager_TaskView> taskList;

        public FrmTask()
        {
            InitializeComponent();
        }

        private void FrmTask_Load(object sender, EventArgs e)
        {
            dutyCodeList = new List<TalentManager_DutyCodeView>();
            companyList = new List<TalentManager_CompanyView>();
            wageTypeList = new List<TalentManager_WageType>();

            dbContext.Configuration.AutoDetectChangesEnabled = true;
                        
            companyList = dbContext.TalentManager_CompanyView.AsNoTracking().ToList();
            dutyCodeList = dbContext.TalentManager_DutyCodeView.AsNoTracking().ToList();
            wageTypeList = dbContext.TalentManager_WageType.AsNoTracking().ToList();

            bindingSourceDutyCode.DataSource = dutyCodeList;
            bindingSourceCompany.DataSource = companyList;
            bindingSourceWageType.DataSource = wageTypeList;

            this.ddlCompany.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlDutyCode.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlPaymentType.DropDownListElement.AutoCompleteAppend.LimitToList = true;

            ddlCompany.SelectedIndex = -1;
            ddlDutyCode.SelectedIndex = -1;
            ddlPaymentType.SelectedIndex = -1;
            ddlOvertime.SelectedIndex = -1;

            getFormData();
        }

        private void FrmTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        private void getFormData()
        {
            taskList = new List<TalentManager_TaskView>();

            taskList = dbContext.TalentManager_TaskView.AsNoTracking().ToList();
            
            bindingSourceTask.DataSource = taskList;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled)
                return;

            bindingSourceTask.AddNew();

            chkActive.Enabled = true;
            ddlPaymentType.Enabled = true;
            ddlDutyCode.Enabled = true;
            ddlCompany.Enabled = true;
            ddlOvertime.Enabled = true;

            chkActive.Checked = false;
            ddlCompany.SelectedIndex = -1;
            ddlDutyCode.SelectedIndex = -1;
            ddlOvertime.SelectedIndex = -1;
            ddlPaymentType.SelectedIndex = -1;

            grdDataDisplay.Enabled = false;            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled || taskList.Count <= 0)
                return;
            
            grdDataDisplay.Enabled = false;

            chkActive.Enabled = true;
            ddlPaymentType.Enabled = true;
            ddlDutyCode.Enabled = true;
            ddlCompany.Enabled = true;
            ddlOvertime.Enabled = true;
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            DialogResult _verifyAction = RadMessageBox.Show("All unsaved data will be lost.\n" +
                                                            "Are you sure you want to continue?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {
                chkActive.Enabled = false;
                ddlPaymentType.Enabled = false;
                ddlDutyCode.Enabled = false;
                ddlCompany.Enabled = false;
                ddlOvertime.Enabled = false;

                grdDataDisplay.Enabled = true;

                int recordId = ((TalentManager_TaskView)bindingSourceTask.Current).TaskId;

                if (recordId == 0)
                    bindingSourceTask.RemoveCurrent();
                else
                    bindingSourceTask.MovePrevious();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            if (ddlDutyCode.SelectedValue == null || Convert.ToInt32(ddlDutyCode.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Task description information is required to continue!", Application.ProductName);
                return;
            }

            if (ddlCompany.SelectedValue == null || Convert.ToInt32(ddlCompany.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Relating company information is required to continue!", Application.ProductName);
                return;
            }
                        
            if (ddlPaymentType.SelectedValue == null || Convert.ToInt32(ddlPaymentType.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Wage type information is required to continue!", Application.ProductName);
                return;
            }

            if (string.IsNullOrEmpty(ddlOvertime.Text.Trim()))
            {
                RadMessageBox.Show("Overtime information is required to continue!", Application.ProductName);
                return;
            }

            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (_verifyAction == DialogResult.Yes)
            {
                int recordId = ((TalentManager_TaskView)bindingSourceTask.Current).TaskId;
                int dutyCodeId = Convert.ToInt32(ddlDutyCode.SelectedValue);
                short companyId = Convert.ToInt16(ddlCompany.SelectedValue);

                bool TaskConfigExist = dbContext.TalentManager_Task.Where(x => x.DutyId == dutyCodeId && x.TaskId != recordId && x.CompanyId == companyId).AsNoTracking().Any();

                if (TaskConfigExist)
                {
                    RadMessageBox.Show("This task configuration already exist!", Application.ProductName);
                    return;
                }

                try
                {
                    if (recordId == 0)
                    {
                        TalentManager_Task addTask = new TalentManager_Task();
                        addTask.DutyId = dutyCodeId;
                        addTask.CompanyId = companyId;
                        addTask.WageTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);
                        addTask.Active = true;
                        addTask.ApplyOvertime = ddlOvertime.SelectedIndex == 0 ? false : true;
                        addTask.CreatedBy = ApplicationSecurityConstants._activeUser;
                        addTask.DateCreated = DateTime.Now;
                        dbContext.TalentManager_Task.Add(addTask);
                    }
                    else
                    {
                        var findRecord = dbContext.TalentManager_Task.Where(x => x.TaskId == recordId).FirstOrDefault();
                        findRecord.DutyId = dutyCodeId;
                        findRecord.CompanyId = companyId;
                        findRecord.WageTypeId = Convert.ToInt32(ddlPaymentType.SelectedValue);
                        findRecord.Active = chkActive.Checked ? true : false;
                        findRecord.ApplyOvertime = ddlOvertime.SelectedIndex == 0 ? false : true;
                        findRecord.LastModifiedBy = ApplicationSecurityConstants._activeUser;
                        findRecord.LastModifiedDate = DateTime.Now;
                    }

                    //IEnumerable<DbEntityEntry> entries = dbContext.ChangeTracker.Entries();
                    //PerformSystemAudit.CreateAuditRecord(entries, dbContext, ApplicationSecurityConstants._activeUser, "TalentManager_Task");

                    dbContext.SaveChanges();

                    grdDataDisplay.Enabled = true;
                    chkActive.Enabled = false;
                    ddlPaymentType.Enabled = false;
                    ddlDutyCode.Enabled = false;
                    ddlCompany.Enabled = false;
                    ddlOvertime.Enabled = false;

                    bindingSourceTask.Clear();

                    getFormData();

                    RadMessageBox.Show("Record created successfully!", Application.ProductName);
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }
    }
}