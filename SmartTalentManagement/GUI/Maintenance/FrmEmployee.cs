using ApplicationSecurity;
using ApplicationSecurity.PerformanceUtils;
using ApplicationSecurity.SecurityUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;

namespace SmartTalentManagement.GUI.Maintenance
{
    public partial class FrmEmployee : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();        
        List<TalentManager_TaskView> taskList;
        List<TalentManager_EmployeeView> EmployeeList;
        List<TalentManager_ContractedEmployeeView> contractedEmployeeList;
        List<TalentManager_ProductionView> productionList;
        List<TalentManager_DepartmentView> departmentList;
        List<TalentManager_CompanyView> companyList;
        private SystemUserGroup loggedInUserGroupProfile;

        public FrmEmployee()
        {
            InitializeComponent();
        }

        private void mcbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.MinDate = dtpStartDate.Value;
        }

        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today.Date;
            dtpEndDate.Value = DateTime.Today.Date;

            dbContext.Configuration.AutoDetectChangesEnabled = true;

            mcbEmployee.EditorControl.TableElement.RowHeight = 50;

            productionList = new List<TalentManager_ProductionView>();
            departmentList = new List<TalentManager_DepartmentView>();
            companyList = new List<TalentManager_CompanyView>();

            string user = ApplicationSecurityConstants._activeUser.Trim().ToUpper();
            var userDetails = dbContext.SystemUsers.Where(x => x.UserName.Trim().ToUpper().Equals(user)).FirstOrDefault();

            if (userDetails != null)            
                loggedInUserGroupProfile = dbContext.SystemUserGroups.Where(x => x.SystemUserGroupId == userDetails.SystemUserGroupId).AsNoTracking().FirstOrDefault();
            
            taskList = new List<TalentManager_TaskView>();
            EmployeeList = new List<TalentManager_EmployeeView>();

            companyList = dbContext.TalentManager_CompanyView.AsNoTracking().ToList();
            taskList = dbContext.TalentManager_TaskView.Where(x => x.Active).AsNoTracking().ToList();
            EmployeeList = dbContext.TalentManager_EmployeeView.AsNoTracking().ToList();

            //if (ApplicationSecurityConstants._isLocationSpecific)
            //{
            //    int departmentId = ApplicationSecurityConstants._storeLocationId;
            //    productionList = dbContext.TalentManager_ProductionView.Where(x => x.DepartmentId == departmentId && x.Active).AsNoTracking().ToList();
            //    departmentList = dbContext.TalentManager_DepartmentView.Where(x => x.DepartmentId == departmentId).AsNoTracking().ToList();
            //}
            //else
            //{
                productionList = dbContext.TalentManager_ProductionView.Where(x => x.Active).AsNoTracking().ToList();                
                departmentList = dbContext.TalentManager_DepartmentView.AsNoTracking().ToList();
            //}
                        
            bindingSourceEmployee.DataSource = EmployeeList;
            bindingSourceCompany.DataSource = companyList;

            mcbEmployee.SelectedIndex = -1;
            ddlCompany.SelectedIndex = -1;

            this.ddlTask.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlCompany.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlDepartment.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlProduction.DropDownListElement.AutoCompleteAppend.LimitToList = true;

            GroupDescriptor groupDescriptor = new GroupDescriptor();
            groupDescriptor.GroupNames.Add("EmployeeDescription", ListSortDirection.Ascending);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor);
            grdDataDisplay.MasterTemplate.ExpandAll();

            this.mcbEmployee.AutoFilter = true;
            this.mcbEmployee.DisplayMember = "EmployeeDescription";
            FilterDescriptor filter = new FilterDescriptor();
            filter.PropertyName = this.mcbEmployee.DisplayMember;
            filter.Operator = FilterOperator.Contains;
            this.mcbEmployee.EditorControl.MasterTemplate.FilterDescriptors.Add(filter);

            getFormData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled)
                return;

            bindingSourceEmployeeContract.AddNew();

            mcbEmployee.Enabled = true;
            txtContractCode.Enabled = true;
            ddlTask.Enabled = true;
            dtpStartDate.Enabled = true;
            dtpEndDate.Enabled = true;
            maskPay.Enabled = true;
            chkActive.Enabled = true;
            ddlCompany.Enabled = true;
            ddlDepartment.Enabled = true;
            ddlProduction.Enabled = true;

            mcbEmployee.SelectedIndex = -1;
            txtContractCode.Clear();
            ddlTask.SelectedIndex = -1;
            chkActive.Checked = true;
            dtpEndDate.Value = DateTime.Today.Date;
            dtpStartDate.Value = DateTime.Today.Date;
            maskPay.Value = 0;

            grdDataDisplay.Enabled = false;
        }

        private void FrmEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        private void getFormData()
        {
            contractedEmployeeList = new List<TalentManager_ContractedEmployeeView>();

            if (ApplicationSecurityConstants._isLocationSpecific)
            {
                int departmentId = ApplicationSecurityConstants._storeLocationId;
                contractedEmployeeList = dbContext.TalentManager_ContractedEmployeeView.Where(x => x.IsActiveContract && x.DepartmentId == departmentId).AsNoTracking().ToList();
            }
            else
                contractedEmployeeList = dbContext.TalentManager_ContractedEmployeeView.Where(x => x.IsActiveContract).AsNoTracking().ToList();

            bindingSourceEmployeeContract.DataSource = contractedEmployeeList;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled || contractedEmployeeList.Count <= 0)
                return;

            if (loggedInUserGroupProfile != null && !loggedInUserGroupProfile.ModifyEmployeeContract)
            {
                RadMessageBox.Show("You do not have sufficient user privilege to edit this record!", Application.ProductName);
                return;
            }
                
            grdDataDisplay.Enabled = false;

            txtContractCode.Enabled = true;
            ddlTask.Enabled = true;
            dtpStartDate.Enabled = true;
            dtpEndDate.Enabled = true;
            maskPay.Enabled = true;
            chkActive.Enabled = true;
            ddlCompany.Enabled = true;
            ddlDepartment.Enabled = true;
            ddlProduction.Enabled = true;
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            DialogResult _verifyAction = RadMessageBox.Show("All unsaved data will be lost.\n" +
                                                            "Are you sure you want to continue?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {
                mcbEmployee.Enabled = false;
                txtContractCode.Enabled = false;
                ddlTask.Enabled = false;
                dtpStartDate.Enabled = false;
                dtpEndDate.Enabled = false;
                maskPay.Enabled = false;
                chkActive.Enabled = false;
                ddlCompany.Enabled = false;
                ddlDepartment.Enabled = false;
                ddlProduction.Enabled = false;

                grdDataDisplay.Enabled = true;

                int recordId = ((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current).EmployeeContractId;

                if (recordId == 0)
                    bindingSourceEmployeeContract.RemoveCurrent();
                else
                    bindingSourceEmployeeContract.MovePrevious();
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

            if (mcbEmployee.SelectedValue == null || Convert.ToInt32(mcbEmployee.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Employee information is required to continue!", Application.ProductName);
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

            if (ddlProduction.SelectedValue == null || Convert.ToInt32(ddlProduction.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Production information is required to continue!", Application.ProductName);
                return;
            }
            
            //if (string.IsNullOrEmpty(txtContractCode.Text.Trim()))
            //{
            //    RadMessageBox.Show("Contract code is required to continue!", Application.ProductName);
            //    return;
            //}

            if (ddlTask.SelectedValue == null || Convert.ToInt32(ddlTask.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Contracted task information is required to continue!", Application.ProductName);
                return;
            }
                        
            if (Convert.ToDecimal(maskPay.Text.Trim().Replace("$", "").Replace("(","").Replace(")","")) <= 0)
            {
                RadMessageBox.Show("Rate of pay information is required to continue!", Application.ProductName);
                return;
            }

            if (maskPay.Text.Trim().Contains("(") || maskPay.Text.Trim().Contains(")"))
            {
                RadMessageBox.Show("Rate of pay information is required to continue!", Application.ProductName);
                return;
            }

            if (dtpStartDate.Value == dtpEndDate.Value)
            {
                DialogResult prompt = RadMessageBox.Show("Contract start and end dates are the same, do you want to continue?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

                if (prompt == DialogResult.No)
                    return;
            }

            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (_verifyAction == DialogResult.Yes)
            {
                int recordId = ((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current).EmployeeContractId;
                //string contractCode = txtContractCode.Text.Trim().ToUpper();

                //bool contractCodeExist = dbContext.TalentManager_EmployeeContract.Where(x => x.EmployeeContractId != recordId && x.ContractCode.Trim().ToUpper().Equals(contractCode)).AsNoTracking().Any();

                //if (contractCodeExist)
                //{
                //    RadMessageBox.Show("Contract codes should be unique. This one already exist!", Application.ProductName);
                //    return;
                //}

                int taskId = Convert.ToInt32(ddlTask.SelectedValue);
                var taskData = taskList.Where(x => x.TaskId == taskId).FirstOrDefault();

                int dutyId = taskData.DutyId;
                int empId = Convert.ToInt32(mcbEmployee.SelectedValue);
                int productionId = ((TalentManager_ProductionView)bindingSourceProduction.Current).ProductionLineId;

                var dutyAlreadyActive = dbContext.TalentManager_EmployeeContract.Where(x => x.DutyId == dutyId && x.EmployeeId == empId && x.IsActiveContract 
                                        && x.EmployeeContractId != recordId && x.ProductionLineId == productionId).AsNoTracking().Any();

                if (dutyAlreadyActive)
                {
                    RadMessageBox.Show("You cannot have two active contracts performing the same duties" + Environment.NewLine + 
                                       "You should make the previous contract inactive and try again", Application.ProductName);
                    return;
                }

                try
                {
                    if (recordId == 0)
                    {
                        var getNumberSequenceValues = dbContext.TalentManager_SystemNumberConfig.Where(x => x.ObjectType.Trim().Equals("Contract")).AsNoTracking().FirstOrDefault();

                        int padding = 0;

                        if (getNumberSequenceValues.UsePadding)
                            padding = getNumberSequenceValues.PaddingAmount;

                        int sysNo = getNumberSequenceValues.NextSystemNumber;
                        string contractNoPadding = sysNo.ToString().PadLeft(padding, '0');
                        string contractNo = contractNoPadding.ToString();
                                        
                        int sysNoId = getNumberSequenceValues.SystemNumberConfigId;
                        int nextNo = sysNo + 1;
                        var updateSysNumber = dbContext.TalentManager_SystemNumberConfig.Where(x => x.SystemNumberConfigId == sysNoId).FirstOrDefault();
                        updateSysNumber.NextSystemNumber = nextNo;

                        RadMessageBox.Show(contractNo);
                        return;

                        TalentManager_EmployeeContract addContract = new TalentManager_EmployeeContract();
                        addContract.EmployeeId = Convert.ToInt32(mcbEmployee.SelectedValue);
                        addContract.CompanyId = ((TalentManager_CompanyView)bindingSourceCompany.Current).CompanyId;
                        addContract.CompanyDescription = ((TalentManager_CompanyView)bindingSourceCompany.Current).CompanyName.Trim();
                        addContract.DepartmentName = ((TalentManager_DepartmentView)bindingSourceDepartment.Current).DepartmentDescription.Trim();
                        addContract.TaskId = Convert.ToInt32(ddlTask.SelectedValue);
                        addContract.DutyDescription = taskData.Description.Trim();// ddlTask.Text.Trim();
                        addContract.DepartmentId = ((TalentManager_DepartmentView)bindingSourceDepartment.Current).DepartmentId;
                        addContract.DutyId = taskData.DutyId;
                        addContract.OvertimeApplicableToDuty = taskData.ApplyOvertime;
                        addContract.OvertimeAppliesToProduction = ((TalentManager_ProductionView)bindingSourceProduction.Current).OvertimeApplies;
                        addContract.OvertimeCalculationStartHour = ((TalentManager_ProductionView)bindingSourceProduction.Current).OvertimeCalculationStartHour;
                        addContract.ProductionLineDescription = ((TalentManager_ProductionView)bindingSourceProduction.Current).Description.Trim();
                        addContract.ProductionLineId = ((TalentManager_ProductionView)bindingSourceProduction.Current).ProductionLineId;
                        addContract.SessionDurationInHours = ((TalentManager_ProductionView)bindingSourceProduction.Current).SessionDurationInHours;
                        addContract.WageTypeDescription = taskData.WageTypeDescription.Trim();
                        addContract.ContractCode = contractNo;// txtContractCode.Text.Trim();
                        addContract.ContractStartDate = dtpStartDate.Value;
                        addContract.ContractEndDate = dtpEndDate.Value;
                        addContract.CostCentre = ((TalentManager_ProductionView)bindingSourceProduction.Current).CostCentre.Trim();
                        addContract.RateOfPay = Convert.ToDecimal(maskPay.Text.Trim().Replace("$", ""));
                        addContract.IsActiveContract = true;
                        addContract.CreatedBy = ApplicationSecurityConstants._activeUser;
                        addContract.DateCreated = DateTime.Now;
                        dbContext.TalentManager_EmployeeContract.Add(addContract);
                    }
                    else
                    {
                        var findRecord = dbContext.TalentManager_EmployeeContract.Where(x => x.EmployeeContractId == recordId).FirstOrDefault();
                        findRecord.CompanyId = ((TalentManager_CompanyView)bindingSourceCompany.Current).CompanyId;
                        findRecord.CompanyDescription = ((TalentManager_CompanyView)bindingSourceCompany.Current).CompanyName.Trim();
                        findRecord.DepartmentName = ((TalentManager_DepartmentView)bindingSourceDepartment.Current).DepartmentDescription.Trim();
                        findRecord.TaskId = Convert.ToInt32(ddlTask.SelectedValue);
                        findRecord.DutyDescription = taskData.Description.Trim();// ddlTask.Text.Trim();
                        findRecord.DepartmentId = ((TalentManager_DepartmentView)bindingSourceDepartment.Current).DepartmentId;
                        findRecord.DutyId = taskData.DutyId;
                        findRecord.OvertimeApplicableToDuty = taskData.ApplyOvertime;
                        findRecord.OvertimeAppliesToProduction = ((TalentManager_ProductionView)bindingSourceProduction.Current).OvertimeApplies;
                        findRecord.OvertimeCalculationStartHour = ((TalentManager_ProductionView)bindingSourceProduction.Current).OvertimeCalculationStartHour;
                        findRecord.ProductionLineDescription = ((TalentManager_ProductionView)bindingSourceProduction.Current).Description.Trim();
                        findRecord.ProductionLineId = ((TalentManager_ProductionView)bindingSourceProduction.Current).ProductionLineId;
                        findRecord.SessionDurationInHours = ((TalentManager_ProductionView)bindingSourceProduction.Current).SessionDurationInHours;
                        findRecord.WageTypeDescription = taskData.WageTypeDescription.Trim();
                        //findRecord.ContractCode = txtContractCode.Text.Trim();
                        findRecord.ContractStartDate = dtpStartDate.Value;
                        findRecord.ContractEndDate = dtpEndDate.Value;
                        findRecord.RateOfPay = Convert.ToDecimal(maskPay.Text.Trim().Replace("$", ""));
                        findRecord.IsActiveContract = chkActive.Checked ? true : false;
                        findRecord.CostCentre = ((TalentManager_ProductionView)bindingSourceProduction.Current).CostCentre.Trim();
                        findRecord.LastModifiedBy = ApplicationSecurityConstants._activeUser;
                        findRecord.LastModifiedDate = DateTime.Now;

                        IEnumerable<DbEntityEntry> entries = dbContext.ChangeTracker.Entries();
                        PerformSystemAudit.CreateAuditRecord(entries, dbContext, ApplicationSecurityConstants._activeUser, "EmployeeContract");
                    }


                    dbContext.SaveChanges();

                    grdDataDisplay.Enabled = true;
                    mcbEmployee.Enabled = false;
                    txtContractCode.Enabled = false;
                    ddlTask.Enabled = false;
                    dtpStartDate.Enabled = false;
                    dtpEndDate.Enabled = false;
                    maskPay.Enabled = false;
                    chkActive.Enabled = false;
                    ddlCompany.Enabled = false;
                    ddlDepartment.Enabled = false;
                    ddlProduction.Enabled = false;

                    bindingSourceEmployeeContract.Clear();

                    getFormData();

                    RadMessageBox.Show("Record created successfully!", Application.ProductName);
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void ddlCompany_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            bindingSourceTask.DataSource = null;
            bindingSourceDepartment.DataSource = null;           

            if (ddlCompany.SelectedIndex >= 0)
            {
                short companyId = ((TalentManager_CompanyView)bindingSourceCompany.Current).CompanyId;
                List<short> departmentIdList = new List<short>();                

                departmentIdList = productionList.Where(x => x.CompanyId == companyId).Select(x => x.DepartmentId).Distinct().ToList();

                bindingSourceTask.DataSource = taskList.Where(x => x.CompanyId == companyId).ToList();
                bindingSourceDepartment.DataSource = departmentList.Where(x => departmentIdList.Contains(x.DepartmentId)).ToList();                
            }
        }

        private void ddlTask_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            lblPaymentType.Text = string.Empty;

            if (ddlTask.SelectedIndex >= 0)
            {
                lblPaymentType.Text = ((TalentManager_TaskView)bindingSourceTask.Current).WageTypeDescription.Trim();
            }
        }

        private void ddlDepartment_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            bindingSourceProduction.DataSource = null;

            if (ddlCompany.SelectedIndex >= 0)
            {
                short companyId = ((TalentManager_CompanyView)bindingSourceCompany.Current).CompanyId;
                short departmentId = ((TalentManager_DepartmentView)bindingSourceDepartment.Current).DepartmentId;

                bindingSourceProduction.DataSource = productionList.Where(x => x.DepartmentId == departmentId && x.CompanyId == companyId).ToList();
            }
        }

        private void bindingSourceEmployeeContract_PositionChanged(object sender, EventArgs e)
        {
            radPictureBox1.Image = null;

            if ((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current != null)
            {
                if (((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current).Photo != null)
                {
                    Bitmap bmp;
                    var bytes = (byte[])((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current).Photo.ToArray();

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        bmp = new Bitmap(ms);
                        radPictureBox1.Image = bmp;
                    }

                    MemoryManagement.FlushMemory();
                }
            }
        }

        private void btnDayRate_Click(object sender, EventArgs e)
        {
            if (mcbEmployee.SelectedValue == null || Convert.ToInt32(mcbEmployee.SelectedValue) <= 0 || !grdDataDisplay.Enabled)
                return;

            int employeeId = ((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current).EmployeeId;
            string employeeName = ((TalentManager_ContractedEmployeeView)bindingSourceEmployeeContract.Current).EmployeeDescription.Trim();

            FrmEmployeeDayRate employeeDayRate = new FrmEmployeeDayRate(employeeId, employeeName);
            employeeDayRate.ShowDialog();
        }
    }    
}