using ApplicationSecurity;
using ApplicationSecurity.PerformanceUtils;
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
using Telerik.WinControls.UI;

namespace SmartTalentManagement.GUI.Transactions
{
    public partial class FrmTransactionEntry : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();
        List<TalentManager_TaskView> taskList;
        List<TalentManager_ContractedEmployeeView> contractedEmployeeList;
        List<TalentManager_EmployeeJobView> employeeJobs;
        List<TalentManager_SessionSpan> sessionSpans;
        List<TalentManager_PaymentType> paymentTypeList;
        List<TalentManager_CompanyView> companyList;
        List<TalentManager_DepartmentView> departmentList;
        List<TalentManager_ProductionView> productionList;
        private SystemUserGroup loggedInUserGroupProfile;

        public FrmTransactionEntry()
        {
            InitializeComponent();
        }

        private void FrmTransactionEntry_Load(object sender, EventArgs e)
        {
            //dtpTransactionDate.Value = DateTime.Today.Date;
            //dtpTransactionDate.MaxDate = DateTime.Today.Date;

            //dtpEndDate.Value = DateTime.Today.Date;
            //dtpEndDate.MaxDate = DateTime.Today.Date;

            calTransDate.SelectedDate = DateTime.Today.Date;
            calTransDate.RangeMaxDate = DateTime.Today.Date;

            //maskStartTime.Value = DateTime.Now;
            //maskEndTime.Value = DateTime.Now;

            string user = ApplicationSecurityConstants._activeUser.Trim().ToUpper();
            var userDetails = dbContext.SystemUsers.Where(x => x.UserName.Trim().ToUpper().Equals(user)).FirstOrDefault();

            if (userDetails != null)
                loggedInUserGroupProfile = dbContext.SystemUserGroups.Where(x => x.SystemUserGroupId == userDetails.SystemUserGroupId).AsNoTracking().FirstOrDefault();

            taskList = new List<TalentManager_TaskView>();
            contractedEmployeeList = new List<TalentManager_ContractedEmployeeView>();
            sessionSpans = new List<TalentManager_SessionSpan>();
            paymentTypeList = new List<TalentManager_PaymentType>();
            companyList = new List<TalentManager_CompanyView>();
            departmentList = new List<TalentManager_DepartmentView>();
            productionList = new List<TalentManager_ProductionView>();

            if (ApplicationSecurityConstants._isLocationSpecific)
            {
                int departmentId = ApplicationSecurityConstants._storeLocationId;
                contractedEmployeeList = dbContext.TalentManager_ContractedEmployeeView.Where(x => x.IsActiveContract && x.DepartmentId == departmentId).AsNoTracking().ToList();
            }
            else
                contractedEmployeeList = dbContext.TalentManager_ContractedEmployeeView.Where(x => x.IsActiveContract).AsNoTracking().ToList();

            taskList = dbContext.TalentManager_TaskView.AsNoTracking().ToList();
            sessionSpans = dbContext.TalentManager_SessionSpan.OrderByDescending(x => x.SessionSpanId).AsNoTracking().ToList();
            paymentTypeList = dbContext.TalentManager_PaymentType.AsNoTracking().ToList();
            companyList = dbContext.TalentManager_CompanyView.AsNoTracking().ToList();
            departmentList = dbContext.TalentManager_DepartmentView.AsNoTracking().ToList();
            productionList = dbContext.TalentManager_ProductionView.Where(x => x.Active).AsNoTracking().ToList();

            List<TalentManager_ContractedEmployeeView> dropDownEmployeeList = new List<TalentManager_ContractedEmployeeView>();

            dropDownEmployeeList = contractedEmployeeList.GroupBy(x => new { x.EmployeeId, x.EmployeeDescription, x.Photo, x.DayRate }).Select(x => new TalentManager_ContractedEmployeeView
            {
                EmployeeId = x.Key.EmployeeId,
                EmployeeDescription = x.Key.EmployeeDescription,
                Photo = x.Key.Photo,
                DayRate = x.Key.DayRate
            }).ToList();

            bindingSourceEmployee.DataSource = dropDownEmployeeList;
            bindingSourceSessionSpan.DataSource = sessionSpans;
            bindingSourceTask.DataSource = taskList;
            bindingSourcePaymentType.DataSource = paymentTypeList;
            bindingSourceProduction.DataSource = productionList;
            bindingSourceCompany.DataSource = companyList;
            bindingSourceDepartment.DataSource = departmentList;

            this.ddlEmployee.DropDownListElement.AutoCompleteSuggest.SuggestMode = Telerik.WinControls.UI.SuggestMode.Contains;
            //this.ddlEmployee.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlPaymentType.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlSessionSpan.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlCompany.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            this.ddlDepartment.DropDownListElement.AutoCompleteAppend.LimitToList = true;

            ddlEmployee.SelectedIndex = -1;
            ddlSessionSpan.SelectedIndex = -1;
            ddlPaymentType.SelectedIndex = -1;
            ddlCompany.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            chkDdlProduction.CheckedItems.Clear();
            chkDdlTask.CheckedItems.Clear();

            GroupDescriptor groupDescriptor = new GroupDescriptor();
            groupDescriptor.GroupNames.Add("EmployeeDescription", ListSortDirection.Ascending);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor);
            grdDataDisplay.MasterTemplate.ExpandAll();

            getFormData();
        }

        private void getFormData()
        {
            employeeJobs = new List<TalentManager_EmployeeJobView>();

            string user = ApplicationSecurityConstants._activeUser.Trim().ToUpper();

            employeeJobs = dbContext.TalentManager_EmployeeJobView.Where(x => !x.IsApproved && !x.IsVoided && !x.IsExported && x.CreatedBy.Trim().ToUpper().Equals(user)).AsNoTracking().ToList();

            bindingSourceJob.DataSource = employeeJobs;
        }

        private void ddlEmployee_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (!grdDataDisplay.Enabled)
            {
                bindingSourceCompany.DataSource = null;
                bindingSourceDepartment.DataSource = null;
                bindingSourcePaymentType.DataSource = null;
                radPictureBox1.Image = null;

                maskEndTime.Value = DateTime.Now.Date;
                maskStartTime.Value = DateTime.Now.Date;

                if (ddlEmployee.SelectedIndex >= 0)
                {
                    int employeeId = ((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).EmployeeId;

                    List<short> companyIdList = new List<short>();
                    companyIdList = contractedEmployeeList.Where(x => x.EmployeeId == employeeId).Select(x => x.CompanyId).Distinct().ToList();
                    bindingSourceCompany.DataSource = companyList.Where(x => companyIdList.Contains(x.CompanyId)).ToList();

                    List<short> departmentIdList = new List<short>();
                    departmentIdList = contractedEmployeeList.Where(x => x.EmployeeId == employeeId).Select(x => x.DepartmentId).Distinct().ToList();
                    bindingSourceDepartment.DataSource = departmentList.Where(x => departmentIdList.Contains(x.DepartmentId)).ToList();

                    //if (((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).Photo != null)
                    //{
                    //    Bitmap bmp;
                    //    var bytes = (byte[])((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).Photo.ToArray();

                    //    using (MemoryStream ms = new MemoryStream(bytes))
                    //    {
                    //        bmp = new Bitmap(ms);
                    //        radPictureBox1.Image = bmp;
                    //    }

                    //    MemoryManagement.FlushMemory();
                    //}

                    if (((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).DayRate > 0)
                        bindingSourcePaymentType.DataSource = paymentTypeList;
                    else
                        bindingSourcePaymentType.DataSource = paymentTypeList.Where(x => !x.PayAtFlatRate).ToList();
                }
            }

            if (ddlEmployee.SelectedIndex >= 0)
            {
                if (((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).Photo != null)
                {
                    Bitmap bmp;
                    var bytes = (byte[])((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).Photo.ToArray();

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        bmp = new Bitmap(ms);
                        radPictureBox1.Image = bmp;
                    }

                    MemoryManagement.FlushMemory();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled)
                return;

            foreach (Control control in tableLayoutPanel1.Controls)
                control.Enabled = true;

            chkDdlTask.CheckedItems.Clear();
            chkDdlProduction.CheckedItems.Clear();

            bindingSourceJob.AddNew();

            txtCostCentre.Enabled = false;
            ddlEmployee.Enabled = true;
            ddlPaymentType.Enabled = true;

            ddlCompany.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlEmployee.SelectedIndex = -1;
            ddlPaymentType.SelectedIndex = -1;
            ddlSessionSpan.SelectedIndex = -1;

            calTransDate.SelectedDate = DateTime.Today.Date;

            maskStartTime.Value = DateTime.Now.Date;
            maskEndTime.Value = DateTime.Now.Date;

            grdDataDisplay.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        //private void dtpTransactionDate_ValueChanged(object sender, EventArgs e)
        //{
        //    if (!grdDataDisplay.Enabled)//here
        //    {
        //        maskStartTime.Value = dtpTransactionDate.Value.Date;
        //        maskEndTime.Value = dtpTransactionDate.Value.Date;

        //        dtpEndDate.MinDate = dtpTransactionDate.Value;
        //    }
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            if (ddlEmployee.SelectedValue == null || Convert.ToInt32(ddlEmployee.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Employee information is required to continue!", Application.ProductName);
                return;
            }            

            if (ddlPaymentType.SelectedValue == null || ddlPaymentType.SelectedValue.ToString().Trim().Equals(string.Empty))
            {
                RadMessageBox.Show("Payment type information is required to continue!", Application.ProductName);
                return;
            }

            if (ddlCompany.SelectedValue == null || Convert.ToInt32(ddlCompany.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Company information is required to continue!", Application.ProductName);
                return;
            }

            if (ddlDepartment.SelectedValue == null || Convert.ToInt32(ddlDepartment.SelectedValue) <= 0)
            {
                RadMessageBox.Show("Department information is required to continue!", Application.ProductName);
                return;
            }

            if (chkDdlProduction.CheckedItems.Count <= 0)
            {
                RadMessageBox.Show("Production information is required to continue!", Application.ProductName);
                return;
            }

            if (chkDdlTask.CheckedItems.Count <= 0)
            {
                RadMessageBox.Show("Task information is required to continue!", Application.ProductName);
                return;
            }

            if (ddlSessionSpan.SelectedValue == null || ddlSessionSpan.SelectedValue.ToString().Trim().Equals(string.Empty) || ddlSessionSpan.SelectedIndex < 0)
            {
                RadMessageBox.Show("Session span information is required to continue!", Application.ProductName);
                return;
            }

            if (calTransDate.SelectedDates.Count <= 0)
            {
                RadMessageBox.Show("Transaction date information is required to continue!", Application.ProductName);
                return;
            }

            bool multiDaySession = ((TalentManager_SessionSpan)bindingSourceSessionSpan.Current).AddDayToEndTime;

            if ((DateTime)maskStartTime.Value > (DateTime)maskEndTime.Value && !multiDaySession)
            {
                RadMessageBox.Show("Session end time must be greater than start time!", Application.ProductName);
                return;
            }

            //if (chkDdlProduction.CheckedItems.Count > chkDdlTask.CheckedItems.Count)
            //{
            //    RadMessageBox.Show("Production selection list cannot be greater than selected task list!", Application.ProductName);
            //    return;
            //}

            short companyId = Convert.ToInt16(ddlCompany.SelectedValue);
            short departmentId = Convert.ToInt16(ddlDepartment.SelectedValue);
            int employeeId = Convert.ToInt32(ddlEmployee.SelectedValue);
            int nonExistentContractCounter = 0;
            decimal taskTotalHrs = 0;

            foreach (var task in chkDdlTask.CheckedItems)
            {
                int taskId = (int)task.Value;

                foreach (var production in chkDdlProduction.CheckedItems)
                {
                    int productionId = (int)production.Value;

                    bool contractExists = contractedEmployeeList.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId && x.DepartmentId == departmentId &&
                                          x.TaskId == taskId && x.ProductionLineId == productionId).Any();

                    if (!contractExists)
                        nonExistentContractCounter++;
                }
            }

            if (nonExistentContractCounter > 0)
            {
                RadMessageBox.Show("No contract information can be found for " + nonExistentContractCounter.ToString() + " of your selection(s)", Application.ProductName);
                return;
            }

            if (!((TalentManager_PaymentType)bindingSourcePaymentType.Current).PayAtFlatRate)
            {
                if (chkDdlProduction.CheckedItems.Count > 1 || chkDdlTask.CheckedItems.Count > 1)
                {
                    RadMessageBox.Show("Multi select option is not applicable to task related payment type!", Application.ProductName);
                    return;
                }
            }
            else
            {
                if (((TalentManager_SessionSpan)bindingSourceSessionSpan.Current).AddDayToEndTime)
                {
                    RadMessageBox.Show("This Session Span information is not used for this payment type!!", Application.ProductName);
                    return;
                }

                List<int> productionIdList = new List<int>();
                List<int> dutyIdList = new List<int>();

                foreach (var production in chkDdlProduction.CheckedItems)
                    productionIdList.Add((int)production.Value);

                foreach (var task in chkDdlTask.CheckedItems)
                    dutyIdList.Add(taskList.Where(x => x.TaskId == (int)task.Value).Select(x => x.DutyId).First());

                //foreach (var task in chkDdlTask.CheckedItems)
                //{
                //    int taskId = (int)task.Value;

                //    taskTotalHrs += contractedEmployeeList.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId
                //                    && x.DepartmentId == departmentId && x.TaskId == taskId).Select(x => x.SessionDurationInHours).FirstOrDefault();

                //    bool hourlyRatedTaskExists = contractedEmployeeList.Where(x => x.TaskId == taskId && x.EmployeeId == employeeId && x.CompanyId == companyId
                //                                 && x.DepartmentId == departmentId && x.WageTypeDescription.Trim().Equals("Hourly")).Any();

                //    if (hourlyRatedTaskExists)
                //    {
                //        RadMessageBox.Show("This payment type does not apply to hourly rated task!", Application.ProductName);
                //        return;
                //    }
                //}

                bool hourlyRatedTaskExists = contractedEmployeeList.Where(x => dutyIdList.Contains(x.DutyId) && x.EmployeeId == employeeId && x.CompanyId == companyId
                                             && x.DepartmentId == departmentId && x.WageTypeDescription.Trim().Equals("Hourly")
                                             && productionIdList.Contains(x.ProductionLineId)).Any();

                if (hourlyRatedTaskExists)
                {
                    RadMessageBox.Show("This payment type does not apply to hourly rated task!", Application.ProductName);
                    return;
                }

                taskTotalHrs = contractedEmployeeList.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId
                               && x.DepartmentId == departmentId && dutyIdList.Contains(x.DutyId)
                               && productionIdList.Contains(x.ProductionLineId)).Sum(x => x.SessionDurationInHours);


                if (taskTotalHrs > 8)
                {
                    RadMessageBox.Show("Selected tasks must be less than or equal to 8 hours!", Application.ProductName);
                    return;
                }
            }

            int jobId = ((TalentManager_EmployeeJobView)bindingSourceJob.Current).EmployeeJobId;
            //DateTime validationStartDate = dtpTransactionDate.Value;
            //DateTime validationEndDate = jobId > 0 ? dtpTransactionDate.Value : dtpEndDate.Value;
            int repeatJobCounter = 0;

            if (jobId > 0 && (chkDdlProduction.CheckedItems.Count > 1 || chkDdlTask.CheckedItems.Count > 1))
            {
                RadMessageBox.Show("Multiple production/task selection is not permitted while in edit mode!", Application.ProductName);
                return;
            }

            if (jobId > 0 && calTransDate.SelectedDates.Count > 1)
            {
                RadMessageBox.Show("The selection of multiple dates is not permitted in edit mode!", Application.ProductName);
                return;
            }

            //List<DateTime> dateList = new List<DateTime>();
            //dateList = calTransDate.SelectedDates.ToList();

            //while (validationStartDate <= validationEndDate)
            //{
            //    foreach (var task in chkDdlTask.CheckedItems)
            //    {
            //        int taskId = (int)task.Value;

            //        foreach (var production in chkDdlProduction.CheckedItems)
            //        {
            //            int productionId = (int)production.Value;

            //            bool employeeJobExists = employeeJobs.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId && x.DepartmentId == departmentId &&
            //                                     x.TaskId == taskId && x.ProductionLineId == productionId && x.JobDate == validationStartDate && x.EmployeeJobId != jobId).Any();

            //            if (employeeJobExists)
            //                repeatJobCounter++;
            //        }
            //    }

            //    validationStartDate = validationStartDate.AddDays(1);
            //}

            foreach (DateTime date in calTransDate.SelectedDates)
            {
                foreach (var task in chkDdlTask.CheckedItems)
                {
                    int taskId = (int)task.Value;

                    foreach (var production in chkDdlProduction.CheckedItems)
                    {
                        int productionId = (int)production.Value;

                        bool employeeJobExists = employeeJobs.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId && x.DepartmentId == departmentId &&
                                                 x.TaskId == taskId && x.ProductionLineId == productionId && x.JobDate == date && x.EmployeeJobId != jobId).Any();

                        if (employeeJobExists)
                            repeatJobCounter++;
                    }
                }
            }

            if (repeatJobCounter > 0)
            {
                RadMessageBox.Show("One or more of the selected tasks information already exists for the selected date range!", Application.ProductName);
                return;
            }

            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (_verifyAction == DialogResult.Yes)
            {
                try
                {
                    DateTime createdDate_Time = DateTime.Now;
                    //DateTime date = dtpTransactionDate.Value;
                    //DateTime endDate = dtpEndDate.Value;

                    if (jobId == 0)
                    {
                        foreach (DateTime date in calTransDate.SelectedDates)
                        {
                            TalentManager_EmployeeJob employeeJob = new TalentManager_EmployeeJob();
                            employeeJob.CompanyId = companyId;
                            employeeJob.CompanyName = ddlCompany.Text.Trim();
                            employeeJob.CreatedBy = ApplicationSecurityConstants._activeUser;
                            employeeJob.DailyWageRate = ((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).DayRate;
                            employeeJob.DateCreated = createdDate_Time;
                            employeeJob.DepartmentId = departmentId;
                            employeeJob.DepartmentName = ddlDepartment.Text.Trim();
                            employeeJob.EmployeeId = employeeId;
                            employeeJob.JobDate = date;
                            employeeJob.Notes = txtComment.Text.Trim();
                            employeeJob.PaymentType = ddlPaymentType.Text.Trim();
                            dbContext.TalentManager_EmployeeJob.Add(employeeJob);

                            //DateTime startTime = (DateTime)maskStartTime.Value;
                            TimeSpan start = ((DateTime)maskStartTime.Value).TimeOfDay;
                            TimeSpan end = ((DateTime)maskEndTime.Value).TimeOfDay;
                            DateTime startTime = date.Add(start);

                            //DateTime endTime = multiDaySession ? Convert.ToDateTime(maskEndTime.Value).AddDays(1) : (DateTime)maskEndTime.Value;
                            DateTime endTime = multiDaySession ? date.AddDays(1).Add(end) : date.Add(end);

                            foreach (var task in chkDdlTask.CheckedItems)
                            {
                                int taskId = (int)task.Value;

                                foreach (var production in chkDdlProduction.CheckedItems)
                                {
                                    int productionId = (int)production.Value;

                                    var contractDetails = contractedEmployeeList.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId && x.DepartmentId == departmentId &&
                                                          x.TaskId == taskId && x.ProductionLineId == productionId).FirstOrDefault();

                                    if (contractDetails != null)
                                    {
                                        decimal overTimeWorkOut = 0;
                                        decimal actualHrsWorked = (decimal)endTime.Subtract(startTime).TotalMinutes / 60;
                                        decimal taskPaymentTotal = 0;

                                        if (!((TalentManager_PaymentType)bindingSourcePaymentType.Current).PayAtFlatRate)
                                        {
                                            if (actualHrsWorked > contractDetails.OvertimeCalculationStartHour && contractDetails.OvertimeAppliesToProduction && contractDetails.OvertimeApplicableToDuty)
                                            {
                                                if (contractDetails.WageTypeDescription.Trim().Equals("Hourly"))
                                                    overTimeWorkOut = Math.Round((actualHrsWorked - contractDetails.OvertimeCalculationStartHour) * contractDetails.RateOfPay, 2);
                                                else
                                                    overTimeWorkOut = Math.Round((actualHrsWorked - contractDetails.OvertimeCalculationStartHour) * (contractDetails.RateOfPay / contractDetails.SessionDurationInHours), 2);
                                            }
                                        }

                                        if (contractDetails.WageTypeDescription.Trim().Equals("Hourly"))
                                            taskPaymentTotal = (Math.Round(actualHrsWorked, 0) * contractDetails.RateOfPay);
                                        else
                                            taskPaymentTotal = ((TalentManager_PaymentType)bindingSourcePaymentType.Current).PayAtFlatRate ?
                                                               Math.Round(((((TalentManager_ContractedEmployeeView)bindingSourceEmployee.Current).DayRate / taskTotalHrs) * contractDetails.SessionDurationInHours), 2)
                                                               : contractDetails.RateOfPay;

                                        TalentManager_EmployeeJobDetail jobDetails = new TalentManager_EmployeeJobDetail();
                                        jobDetails.ActualHoursWorked = actualHrsWorked;
                                        jobDetails.CostCentre = contractDetails.CostCentre.Trim();
                                        jobDetails.DutyDescription = contractDetails.DutyDescription;
                                        jobDetails.DutyId = contractDetails.DutyId;
                                        jobDetails.EmployeeContractId = contractDetails.EmployeeContractId;
                                        jobDetails.EmployeeJobId = employeeJob.EmployeeJobId;
                                        jobDetails.EndTime = endTime;
                                        jobDetails.IsApproved = false;// loggedInUserGroupProfile.ApproveSessionTransactions;
                                        jobDetails.OvertimePaymentAmount = overTimeWorkOut;
                                        jobDetails.TaskPaymentAmount = taskPaymentTotal;
                                        jobDetails.ProductionLineDescription = contractDetails.ProductionLineDescription;
                                        jobDetails.ProductionLineId = contractDetails.ProductionLineId;
                                        jobDetails.SessionDurationInHours = contractDetails.SessionDurationInHours;
                                        jobDetails.StartTime = startTime;
                                        jobDetails.WageRate = contractDetails.RateOfPay;
                                        jobDetails.IsExported = false;
                                        jobDetails.IsVoided = false;
                                        jobDetails.WageTypeDescription = contractDetails.WageTypeDescription;
                                        employeeJob.TalentManager_EmployeeJobDetail.Add(jobDetails);
                                    }
                                }
                            }

                            dbContext.SaveChanges();
                            //date = date.AddDays(1);
                        }
                    }
                    else
                    {
                        var jobHeaderData = dbContext.TalentManager_EmployeeJob.Where(x => x.EmployeeJobId == jobId).FirstOrDefault();

                        if (jobHeaderData != null)
                        {
                            jobHeaderData.PaymentType = ddlPaymentType.Text.Trim();
                            jobHeaderData.Notes = txtComment.Text.Trim();

                            int jobDetailId = ((TalentManager_EmployeeJobView)bindingSourceJob.Current).EmployeeJobDetailId;

                            var jobDetailData = dbContext.TalentManager_EmployeeJobDetail.Where(x => x.EmployeeJobDetailId == jobDetailId && !x.IsApproved).FirstOrDefault();

                            if (jobDetailData != null)
                            {
                                int productionId = (int)chkDdlProduction.CheckedItems[0].Value;
                                int taskId = (int)chkDdlTask.CheckedItems[0].Value;

                                var contractDetails = contractedEmployeeList.Where(x => x.EmployeeId == employeeId && x.CompanyId == companyId && x.DepartmentId == departmentId &&
                                                      x.TaskId == taskId && x.ProductionLineId == productionId).FirstOrDefault();


                                DateTime startTime = (DateTime)maskStartTime.Value;
                                DateTime endTime = multiDaySession ? Convert.ToDateTime(maskEndTime.Value).AddDays(1) : (DateTime)maskEndTime.Value;
                                decimal overTimeWorkOut = 0;
                                decimal actualHrsWorked = (decimal)endTime.Subtract(startTime).TotalMinutes / 60;
                                decimal taskPaymentTotal = 0;

                                if (!((TalentManager_PaymentType)bindingSourcePaymentType.Current).PayAtFlatRate)
                                {
                                    if (actualHrsWorked > contractDetails.OvertimeCalculationStartHour && contractDetails.OvertimeAppliesToProduction && contractDetails.OvertimeApplicableToDuty)
                                    {
                                        if (contractDetails.WageTypeDescription.Trim().Equals("Hourly"))
                                            overTimeWorkOut = Math.Round((actualHrsWorked - contractDetails.OvertimeCalculationStartHour) * contractDetails.RateOfPay, 2);
                                        else
                                            overTimeWorkOut = Math.Round((actualHrsWorked - contractDetails.OvertimeCalculationStartHour) * (contractDetails.RateOfPay / contractDetails.SessionDurationInHours), 2);
                                    }
                                }

                                if (contractDetails.WageTypeDescription.Trim().Equals("Hourly"))
                                    taskPaymentTotal = (Math.Round(actualHrsWorked, 0) * contractDetails.RateOfPay);
                                else
                                    taskPaymentTotal = contractDetails.RateOfPay;

                                jobDetailData.ProductionLineId = productionId;
                                jobDetailData.ProductionLineDescription = contractDetails.ProductionLineDescription;
                                jobDetailData.DutyId = contractDetails.DutyId;
                                jobDetailData.DutyDescription = contractDetails.DutyDescription;
                                jobDetailData.StartTime = startTime;
                                jobDetailData.EndTime = endTime;
                                jobDetailData.ActualHoursWorked = actualHrsWorked;
                                jobDetailData.TaskPaymentAmount = taskPaymentTotal;
                                jobDetailData.WageRate = contractDetails.RateOfPay;
                                jobDetailData.WageTypeDescription = contractDetails.WageTypeDescription.Trim();
                                jobDetailData.SessionDurationInHours = contractDetails.SessionDurationInHours;
                                jobDetailData.LastModifiedBy = ApplicationSecurityConstants._activeUser;
                                jobDetailData.LastModifiedDate = DateTime.Now;
                                jobDetailData.CostCentre = contractDetails.CostCentre.Trim();

                                dbContext.SaveChanges();
                            }
                        }
                    }

                    foreach (Control control in tableLayoutPanel1.Controls)
                    {
                        if (control is RadLabel || control is RadGroupBox)
                            control.Enabled = true;
                        else
                            control.Enabled = false;
                    }

                    ddlEmployee.Enabled = false;
                    ddlPaymentType.Enabled = false;

                    ddlEmployee.SelectedIndex = -1;
                    ddlPaymentType.SelectedIndex = -1;
                    ddlCompany.SelectedIndex = -1;
                    ddlDepartment.SelectedIndex = -1;
                    ddlSessionSpan.SelectedIndex = -1;
                    chkDdlProduction.CheckedItems.Clear();
                    chkDdlTask.CheckedItems.Clear();

                    grdDataDisplay.Enabled = true;

                    RadMessageBox.Show("Record(s) created successfully!", Application.ProductName);

                    getFormData();
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            DialogResult _verifyAction = RadMessageBox.Show("All unsaved data will be lost.\n" +
                                                            "Are you sure you want to continue?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {

                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    if (control is RadLabel || control is RadGroupBox)
                        control.Enabled = true;
                    else
                        control.Enabled = false;
                }

                ddlEmployee.SelectedIndex = -1;
                ddlEmployee.Enabled = false;
                ddlPaymentType.Enabled = false;
                grdDataDisplay.Enabled = true;
            }
        }

        private void FrmTransactionEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        private void ddlDepartment_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (!grdDataDisplay.Enabled)
            {
                bindingSourceProduction.DataSource = null;

                if (ddlCompany.SelectedValue != null && ddlDepartment.SelectedValue != null && ddlCompany.SelectedIndex >= 0 && ddlDepartment.SelectedIndex >= 0)
                {
                    short companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                    short departmentId = Convert.ToInt16(ddlDepartment.SelectedValue);

                    List<int> productionIdList = new List<int>();
                    productionIdList = contractedEmployeeList.Where(x => x.CompanyId == companyId && x.DepartmentId == departmentId).Select(x => x.ProductionLineId).Distinct().ToList();
                    bindingSourceProduction.DataSource = productionList.Where(x => productionIdList.Contains(x.ProductionLineId)).ToList();
                }
            }
        }

        private void chkDdlProduction_ItemCheckedChanged(object sender, RadCheckedListDataItemEventArgs e)
        {
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled || employeeJobs.Count <= 0)
                return;

            if (((TalentManager_EmployeeJobView)bindingSourceJob.Current).PaymentType.Trim().Equals("Day"))
            {
                RadMessageBox.Show("This transaction type may not be editted. The delete option may be used instead!", Application.ProductName);
                return;
            }

            ddlPaymentType.Enabled = true;
            chkDdlProduction.Enabled = true;
            chkDdlTask.Enabled = true;
            maskStartTime.Enabled = true;
            maskEndTime.Enabled = true;
            txtComment.Enabled = true;
            ddlSessionSpan.Enabled = true;

            //ddlPaymentType.SelectedValue = ((TalentManager_EmployeeJobView)bindingSourceJob.Current).PaymentType;
            //chkDdlProduction.SelectedItems = ((TalentManager_EmployeeJobView)bindingSourceJob.Current).ProductionLineId

            grdDataDisplay.Enabled = false;
            ddlEmployee.SelectedValue = ((TalentManager_EmployeeJobView)bindingSourceJob.Current).EmployeeId;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled || employeeJobs.Count <= 0)
                return;

            if (loggedInUserGroupProfile != null && !loggedInUserGroupProfile.DeleteUnapproveTransactions)
            {
                RadMessageBox.Show("You do not have sufficient user privilege to delete this record!", Application.ProductName);
                return;
            }

            DialogResult promptUser = RadMessageBox.Show("Deleting a record is an irreversible process" + Environment.NewLine +
                                                         "If you continue, you will not be able to undo this action" + Environment.NewLine +
                                                         "Are you sure you want to continue?", Application.ProductName, MessageBoxButtons.YesNo,
                                                         RadMessageIcon.Question, MessageBoxDefaultButton.Button2);
            if (promptUser == DialogResult.Yes)
            {
                int jobId = ((TalentManager_EmployeeJobView)bindingSourceJob.Current).EmployeeJobId;

                int jobCount = employeeJobs.Where(x => x.EmployeeJobId == jobId).Count();

                try
                {
                    var getJobDetails = dbContext.TalentManager_EmployeeJobDetail.Where(x => x.EmployeeJobId == jobId && !x.IsApproved).ToList();


                    if (getJobDetails.Count > 0)
                    {
                        foreach (var detail in getJobDetails)
                            dbContext.TalentManager_EmployeeJobDetail.Remove(detail);

                        var getJobData = dbContext.TalentManager_EmployeeJob.Where(x => x.EmployeeJobId == jobId).FirstOrDefault();

                        if (getJobData != null)
                            dbContext.TalentManager_EmployeeJob.Remove(getJobData);

                        dbContext.SaveChanges();

                        getFormData();
                    }
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void bindingSourceJob_PositionChanged(object sender, EventArgs e)
        {
            //if (grdDataDisplay.Enabled)
            //{
            //    chkDdlProduction.CheckedItems.Clear();
            //    chkDdlTask.CheckedItems.Clear();
            //}
        }

        private void chkDdlProduction_ItemCheckedChanging(object sender, RadCheckedListDataItemCancelEventArgs e)
        {
            //if (chkDdlProduction.CheckedItems.Count > 0)
            //    chkDdlProduction.Items[0].Checked = false;//.Clear();//[0].Checked = false;//.Clear();
        }

        private void calTransDate_SelectionChanged(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled)
            {
                maskStartTime.Value = DateTime.Now.Date;
                maskEndTime.Value = DateTime.Now.Date;
            }
        }

        private void chkDdlProduction_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!grdDataDisplay.Enabled)
            {
                bindingSourceTask.DataSource = null;
                //txtCostCentre.Text = string.Empty;

                if (ddlCompany.SelectedValue != null && ddlDepartment.SelectedValue != null && ddlCompany.SelectedIndex >= 0 && ddlDepartment.SelectedIndex >= 0)
                {
                    short companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                    short departmentId = Convert.ToInt16(ddlDepartment.SelectedValue);
                    List<int> productionIdList = new List<int>();
                    List<int> taskIdList = new List<int>();

                    foreach (var production in chkDdlProduction.CheckedItems)
                        productionIdList.Add((int)production.Value);

                    taskIdList = contractedEmployeeList.Where(x => productionIdList.Contains(x.ProductionLineId) && x.CompanyId == companyId && x.DepartmentId == departmentId)
                                 .Select(x => x.TaskId).Distinct().ToList();

                    bindingSourceTask.DataSource = taskList.Where(x => taskIdList.Contains(x.TaskId)).ToList();

                    //if (productionIdList.Count > 1)
                    //{
                    //    txtCostCentre.Text = "Multiple";
                    //}
                    //else if (productionIdList.Count == 1)
                    //{
                    //    int prodLineId = productionIdList[0];
                    //    txtCostCentre.Text = productionList.Where(x => x.ProductionLineId == prodLineId).Select(x => x.CostCentre.Trim()).FirstOrDefault();
                    //}
                }
            }
        }
    }
}