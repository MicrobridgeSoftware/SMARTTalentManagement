using ApplicationSecurity;
using ApplicationSecurity.SecurityUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace SmartTalentManagement.GUI.Maintenance
{
    public partial class FrmProductionLine : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();
        List<TalentManager_ProductionView> productionList;
        List<TalentManager_DepartmentView> departmentList;
        List<TalentManager_CompanyView> companyList;

        public FrmProductionLine()
        {
            InitializeComponent();
        }

        private void FrmProductionLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        private void getFormData()
        {
            productionList = new List<TalentManager_ProductionView>();

            productionList = dbContext.TalentManager_ProductionView.AsNoTracking().ToList();            
                       
            bindingSourceProductionLine.DataSource = productionList;
        }

        private void FrmProductionLine_Load(object sender, EventArgs e)
        {
            dbContext.Configuration.AutoDetectChangesEnabled = true;

            departmentList = new List<TalentManager_DepartmentView>();
            companyList = new List<TalentManager_CompanyView>();

            departmentList = dbContext.TalentManager_DepartmentView.AsNoTracking().ToList();
            companyList = dbContext.TalentManager_CompanyView.AsNoTracking().ToList();
            
            bindingSourceCompany.DataSource = companyList;
            bindingSourceDepartment.DataSource = departmentList;

            this.ddlCompany.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlDepartment.DropDownListElement.AutoCompleteAppend.LimitToList = true;

            getFormData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled)
                return;

            bindingSourceProductionLine.AddNew();

            chkActive.Enabled = true;
            txtCode.Enabled = true;
            txtDescription.Enabled = true;
            chkOvertime.Enabled = true;
            spnDuration.Enabled = true;
            ddlCompany.Enabled = true;
            ddlDepartment.Enabled = true;
            txtCostCentre.Enabled = true;

            txtCode.Clear();
            txtDescription.Clear();
            txtCostCentre.Clear();
            spnDuration.Value = 1;
            chkActive.Checked = false;
            chkOvertime.Checked = false;

            grdDataDisplay.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled || productionList.Count <= 0)
                return;

            grdDataDisplay.Enabled = false;

            chkActive.Enabled = true;
            txtCode.Enabled = true;
            txtDescription.Enabled = true;
            chkOvertime.Enabled = true;
            spnDuration.Enabled = true;
            ddlCompany.Enabled = true;
            ddlDepartment.Enabled = true;
            txtCostCentre.Enabled = true;

            if (chkOvertime.Checked)
                spnOvertimeStart.Enabled = true;
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
                txtCode.Enabled = false;
                txtDescription.Enabled = false;
                chkOvertime.Enabled = false;
                spnDuration.Enabled = false;
                ddlCompany.Enabled = false;
                ddlDepartment.Enabled = false;
                txtCostCentre.Enabled = false;

                chkOvertime.Checked = false;

                grdDataDisplay.Enabled = true;

                int recordId = ((TalentManager_ProductionView)bindingSourceProductionLine.Current).ProductionLineId;

                if (recordId == 0)
                    bindingSourceProductionLine.RemoveCurrent();
                else
                    bindingSourceProductionLine.MovePrevious();
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

            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                RadMessageBox.Show("Production code is required to continue!", Application.ProductName);
                return;
            }

            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                RadMessageBox.Show("Production description is required to continue!", Application.ProductName);
                return;
            }

            if (ddlCompany.SelectedValue == null || Convert.ToInt16(ddlCompany.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Company information is required to continue!", Application.ProductName);
                return;
            }

            if (ddlDepartment.SelectedValue == null || Convert.ToInt16(ddlDepartment.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Department information is required to continue!", Application.ProductName);
                return;
            }

            if (spnDuration.Value <= 0)
            {
                RadMessageBox.Show("Session duration information is required to continue!", Application.ProductName);
                return;
            }

            if (chkOvertime.Checked)
            {
                if (spnOvertimeStart.Value <= 0)
                {
                    RadMessageBox.Show("Overtime calculation information is required to continue!", Application.ProductName);
                    return;
                }

                if (spnOvertimeStart.Value < spnDuration.Value)
                {
                    RadMessageBox.Show("Overtime calculation start time must be greater than session duration!", Application.ProductName);
                    return;
                }
            }

            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (_verifyAction == DialogResult.Yes)
            {
                int recordId = ((TalentManager_ProductionView)bindingSourceProductionLine.Current).ProductionLineId;
                string code = txtCode.Text.Trim().ToUpper();
                
                bool productLineExist = dbContext.TalentManager_ProductionLine.Where(x => x.Code.Trim().ToUpper().Equals(code) && x.ProductionLineId != recordId).AsNoTracking().Any();

                if (productLineExist)
                {
                    RadMessageBox.Show("This production code already exist!", Application.ProductName);
                    return;
                }

                try
                {
                    if (recordId == 0)
                    {
                        TalentManager_ProductionLine addProductionLine = new TalentManager_ProductionLine();
                        addProductionLine.Code = txtCode.Text.Trim();
                        addProductionLine.CompanyId = Convert.ToInt16(ddlCompany.SelectedValue);
                        addProductionLine.DepartmentId = Convert.ToInt16(ddlDepartment.SelectedValue);
                        addProductionLine.CostCentre = txtCostCentre.Text.Trim();
                        addProductionLine.Description = txtDescription.Text.Trim();
                        addProductionLine.SessionDurationInHours = spnDuration.Value;
                        addProductionLine.OvertimeApplies = chkOvertime.Checked ? true : false;
                        addProductionLine.OvertimeCalculationStartHour = spnOvertimeStart.Value;
                        addProductionLine.Active = true;
                        addProductionLine.CreatedBy = ApplicationSecurityConstants._activeUser;
                        addProductionLine.DateCreated = DateTime.Now;
                        dbContext.TalentManager_ProductionLine.Add(addProductionLine);
                    }
                    else
                    {
                        var findRecord = dbContext.TalentManager_ProductionLine.Where(x => x.ProductionLineId == recordId).FirstOrDefault();
                        findRecord.Code = txtCode.Text.Trim();
                        findRecord.CompanyId = Convert.ToInt16(ddlCompany.SelectedValue);
                        findRecord.DepartmentId = Convert.ToInt16(ddlDepartment.SelectedValue);
                        findRecord.CostCentre = txtCostCentre.Text.Trim();
                        findRecord.Description = txtDescription.Text.Trim();
                        findRecord.SessionDurationInHours = spnDuration.Value;
                        findRecord.OvertimeApplies = chkOvertime.Checked ? true : false;
                        findRecord.OvertimeCalculationStartHour = spnOvertimeStart.Value;
                        findRecord.Active = chkActive.Checked ? true : false;
                        findRecord.LastModifiedBy = ApplicationSecurityConstants._activeUser;
                        findRecord.LastModifiedDate = DateTime.Now;
                    }

                    //IEnumerable<DbEntityEntry> entries = dbContext.ChangeTracker.Entries();
                    //PerformSystemAudit.CreateAuditRecord(entries, dbContext, ApplicationSecurityConstants._activeUser, "TalentManager_ProductionLine");

                    dbContext.SaveChanges();

                    grdDataDisplay.Enabled = true;
                    chkActive.Enabled = false;
                    txtCode.Enabled = false;
                    txtDescription.Enabled = false;
                    chkOvertime.Enabled = false;
                    spnDuration.Enabled = false;
                    ddlDepartment.Enabled = false;
                    ddlCompany.Enabled = false;
                    txtCostCentre.Enabled = false;

                    chkOvertime.Checked = false;

                    bindingSourceProductionLine.Clear();

                    getFormData();

                    RadMessageBox.Show("Record created successfully!", Application.ProductName);
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void chkOvertime_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkOvertime.Enabled)
            {
                if (chkOvertime.Checked)
                {
                    spnOvertimeStart.Enabled = true;
                }
                else
                {
                    spnOvertimeStart.Enabled = false;
                    spnOvertimeStart.Value = 0;
                }
            }
        }
    }
}