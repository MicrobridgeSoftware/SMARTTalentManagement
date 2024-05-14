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
    public partial class FrmEmployeeDayRate : Telerik.WinControls.UI.RadForm
    {
        SMARTPayEntities dbContext = new SMARTPayEntities();
        private int employeeId;
        private string employeeName;

        public FrmEmployeeDayRate()
        {
            InitializeComponent();
        }

        public FrmEmployeeDayRate(int employeeId, string employeeName) : this()
        {
            this.employeeId = employeeId;
            this.employeeName = employeeName;
        }

        private void FrmEmployeeDayRate_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " - " + employeeName;

            var getDayRateInfo = dbContext.TalentManager_EmployeeDayRate.Where(x => x.EmployeeId == employeeId).AsNoTracking().FirstOrDefault();

            if (getDayRateInfo != null)
                spnRate.Value = getDayRateInfo.DayRate;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            var getEmployeeDayRate = dbContext.TalentManager_EmployeeDayRate.Where(x => x.EmployeeId == employeeId).FirstOrDefault();

            if (spnRate.Value <= 0 && getEmployeeDayRate == null)
                return;

            DialogResult prompt = RadMessageBox.Show("Are you sure you want to commit this change?", Application.ProductName, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

            if (prompt == DialogResult.Yes)
            {
                try
                {
                    if (spnRate.Value <= 0 && getEmployeeDayRate != null)
                    {
                        DialogResult deletePrompt = RadMessageBox.Show("This action will remove day related rate from this employee" + Environment.NewLine +
                                                                           "Are you sure you want to continue?", Application.ProductName, MessageBoxButtons.YesNo,
                                                                           RadMessageIcon.Question, MessageBoxDefaultButton.Button2);

                        if (deletePrompt == DialogResult.Yes)
                            dbContext.TalentManager_EmployeeDayRate.Remove(getEmployeeDayRate);
                    }

                    if (spnRate.Value > 0 && getEmployeeDayRate == null)
                    {
                        TalentManager_EmployeeDayRate addDayRate = new TalentManager_EmployeeDayRate();
                        addDayRate.DayRate = spnRate.Value;
                        addDayRate.EmployeeId = employeeId;
                        dbContext.TalentManager_EmployeeDayRate.Add(addDayRate);
                    }
                    else
                        getEmployeeDayRate.DayRate = spnRate.Value;

                    dbContext.SaveChanges();

                    RadMessageBox.Show("Record created successfully!", Application.ProductName);

                    Close();
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }
    }
}