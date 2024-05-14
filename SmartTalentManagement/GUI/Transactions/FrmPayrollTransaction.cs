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
using System.Transactions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace SmartTalentManagement.GUI.Transactions
{
    public partial class FrmPayrollTransaction : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();
        List<TalentManager_EmployeeJobView> employeeJobs;
        List<TalentManager_CompanyView> companyViews;

        public FrmPayrollTransaction()
        {
            InitializeComponent();
        }

        private void FrmPayrollTransaction_Load(object sender, EventArgs e)
        {
            companyViews = new List<TalentManager_CompanyView>();
            companyViews = dbContext.TalentManager_CompanyView.AsNoTracking().ToList();
            bindingSourceCompany.DataSource = companyViews;

            GroupDescriptor groupDescriptor = new GroupDescriptor();
            GroupDescriptor groupDescriptor1 = new GroupDescriptor();
            groupDescriptor.GroupNames.Add("DepartmentName", ListSortDirection.Ascending);
            groupDescriptor1.GroupNames.Add("EmployeeDescription", ListSortDirection.Ascending);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor1);
            grdDataDisplay.MasterTemplate.ExpandAll();

            this.grdDataDisplay.MasterTemplate.ShowTotals = true;

            GridViewSummaryItem summaryItem = new GridViewSummaryItem("CalculatedPaymentAmount", "{0:C}", GridAggregateFunction.Sum);
            GridViewSummaryItem summaryItem1 = new GridViewSummaryItem("UserPayment", "{0:C}", GridAggregateFunction.Sum);
            GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();
            summaryRowItem.Add(summaryItem);
            summaryRowItem.Add(summaryItem1);
            this.grdDataDisplay.SummaryRowsBottom.Add(summaryRowItem);

            this.ddlCompany.DropDownListElement.AutoCompleteAppend.LimitToList = true;
            //getFormData();
        }

        private void getFormData()
        {
            if (ddlCompany.SelectedValue != null && Convert.ToInt16(ddlCompany.SelectedValue) > 0)
            {
                Int16 companyId = Convert.ToInt16(ddlCompany.SelectedValue);
                employeeJobs = new List<TalentManager_EmployeeJobView>();

                employeeJobs = dbContext.TalentManager_EmployeeJobView.Where(x => x.IsApproved && !x.IsVoided && !x.IsExported && x.CompanyId == companyId).AsNoTracking().ToList();

                grdDataDisplay.DataSource = employeeJobs;
            }
        }

        private void FrmPayrollTransaction_FormClosing(object sender, FormClosingEventArgs e)
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
            string saveAs = "Payroll Transactions";
            string fullpath = destinationPath + "\\" + saveAs + ".xlsx";

            bool fileExist = File.Exists(fullpath);

            if (fileExist)
            {
                RadMessageBox.Show("A file containing the same name already exist in this location", Application.ProductName);
                return;
            }

            try
            {
                grdDataDisplay.Columns["Void"].IsVisible = false;

                PerformExcelExport.ExportGridViewData(grdDataDisplay, fullpath, saveAs);

                grdDataDisplay.Columns["Void"].IsVisible = true;
            }
            catch (Exception _exp)
            {
                RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
            }
        }

        private void grdDataDisplay_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.ForeColor = Color.Red;
                e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                e.CellElement.Font = new System.Drawing.Font("Segoe UI", 8.5f, FontStyle.Bold);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            var selectedPaymentRows = grdDataDisplay.Rows.Where(x => x.Cells["Select"].Value != null && Convert.ToBoolean(x.Cells["Select"].Value) == true).ToList();
                        
            if (selectedPaymentRows.Count > 0)
            {
                var invalidPaymentRowsExist = selectedPaymentRows.Where(x => x.Cells["UserPayment"].Value == null || Convert.ToDecimal(x.Cells["UserPayment"].Value) <= 0).Any();

                if (invalidPaymentRowsExist)
                {
                    RadMessageBox.Show("All selected payment rows must contain a payment amount greater than 0!", Application.ProductName);
                    return;
                }
            }

            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (_verifyAction == DialogResult.Yes)
            {
                foreach(var row in selectedPaymentRows)
                {
                    int detailId = Convert.ToInt32(row.Cells["EmployeeJobDetailId"].Value);
                    decimal userPaymentAmount = Convert.ToDecimal(row.Cells["UserPayment"].Value);

                    var employeeTransaction = dbContext.TalentManager_EmployeeJobDetail.Where(x => x.EmployeeJobDetailId == detailId && x.IsApproved && !x.IsExported && !x.IsVoided).FirstOrDefault();

                    if (employeeTransaction != null)
                    {
                        employeeTransaction.UserDecidedPaymentAmount = userPaymentAmount;
                        employeeTransaction.PaymentAmountDecidedByUser = ApplicationSecurityConstants._activeUser;
                    }
                }

                dbContext.SaveChanges();

                RadMessageBox.Show("Record(s) updated successfully!", Application.ProductName);

                getFormData();
            }
        }

        private void txtPayrollPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))// && (e.KeyChar != '.'))
                e.Handled = true;

            // only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void txtPayrollYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            if (string.IsNullOrEmpty(txtPayrollPeriod.Text.Trim()) || string.IsNullOrEmpty(txtPayrollYear.Text.Trim()))
            {
                RadMessageBox.Show("Payroll period and year information are required to continue!", Application.ProductName);
                return;
            }

            if (!Int32.TryParse(txtPayrollPeriod.Text.Trim(), out int value))
            {
                RadMessageBox.Show("Payroll period must be a numeric value!", Application.ProductName);
                return;
            }

            if (!Int32.TryParse(txtPayrollYear.Text.Trim(), out int value1))
            {
                RadMessageBox.Show("Payroll year must be a numeric value!", Application.ProductName);
                return;
            }

            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to commit these changes?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (_verifyAction == DialogResult.Yes)
            {
                using (var transcope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
                {
                    var payrollTransactionRows = grdDataDisplay.Rows.Where(x => x.Cells["EmployeeJobDetailId"].Value != null && Convert.ToInt32(x.Cells["EmployeeJobDetailId"].Value) > 0).ToList();

                    if (payrollTransactionRows.Count > 0)
                    {
                        List<int> approvedJobIdList = new List<int>();

                        approvedJobIdList = payrollTransactionRows.Select(x => Convert.ToInt32(x.Cells["EmployeeJobDetailId"].Value)).Distinct().ToList();
                        var getExclusivePaymentList = dbContext.TalentManager_EmployeeJobView.Where(c => approvedJobIdList.Contains(c.EmployeeJobDetailId)).AsNoTracking().ToList();

                        TalentManager_ExportedEmployeeJob addExport = new TalentManager_ExportedEmployeeJob();
                        addExport.CreatedBy = ApplicationSecurityConstants._activeUser;
                        addExport.DateCreated = DateTime.Now;
                        addExport.EmployeeCount = getExclusivePaymentList.Select(x => x.EmployeeId).Distinct().Count();
                        addExport.PayrollAmount = getExclusivePaymentList.Sum(x => x.TaskPaymentAmount);
                        addExport.PayrollPeriod = Convert.ToInt32(txtPayrollPeriod.Text);
                        addExport.PayrollYear = Convert.ToInt32(txtPayrollYear.Text);
                        addExport.CompanyId = Convert.ToInt16(ddlCompany.SelectedValue);
                        dbContext.TalentManager_ExportedEmployeeJob.Add(addExport);

                        foreach (var rows in getExclusivePaymentList)
                        {
                            decimal userPaymentAmount = rows.TaskPaymentAmount;
                            int employeeJobDetailId = rows.EmployeeJobDetailId;

                            TalentManager_ExportedEmployeeJobDetails addExportDetails = new TalentManager_ExportedEmployeeJobDetails();
                            addExportDetails.AmountPaid = userPaymentAmount;
                            addExportDetails.EmployeeJobDetailId = employeeJobDetailId;
                            addExportDetails.ExportedEmployeeJobId = addExport.ExportedEmployeeJobId;
                            addExport.TalentManager_ExportedEmployeeJobDetails.Add(addExportDetails);
                        }

                        var updateEmployeeJob = dbContext.TalentManager_EmployeeJobDetail.Where(x => approvedJobIdList.Contains(x.EmployeeJobDetailId)).ToList();

                        foreach (var job in updateEmployeeJob)
                            job.IsExported = true;

                        dbContext.SaveChanges();

                        //create payroll file here
                    }

                    transcope.Complete();
                }

                RadMessageBox.Show("Record created successfully!", Application.ProductName);
                getFormData();
            }
        }

        private void grdDataDisplay_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.Cells["EmployeeJobDetailId"].Value != null && Convert.ToInt32(e.Row.Cells["EmployeeJobDetailId"].Value) > 0)
            {
                int transactionDetailId = Convert.ToInt32(e.Row.Cells["EmployeeJobDetailId"].Value);
                Dictionary<int, string> transactionVoidDictionary = new Dictionary<int, string>();

                FrmVoidReasonComment voidReasonComment = new FrmVoidReasonComment(transactionDetailId, transactionVoidDictionary);
                voidReasonComment.ShowDialog();

                if (transactionVoidDictionary.Count <= 0)
                    return;

                string voidReason = transactionVoidDictionary.First().Value.ToString().Trim();

                var jobDetail = dbContext.TalentManager_EmployeeJobDetail.Where(x => x.EmployeeJobDetailId == transactionDetailId).FirstOrDefault();

                if(jobDetail != null)
                {
                    jobDetail.IsVoided = true;
                    jobDetail.VoidedBy = voidReason;
                    jobDetail.DateVoided = DateTime.Now;
                    jobDetail.VoidedBy = ApplicationSecurityConstants._activeUser;

                    dbContext.SaveChanges();

                    RadMessageBox.Show("Record voided successfully!", Application.ProductName);

                    getFormData();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            getFormData();
        }
    }
}