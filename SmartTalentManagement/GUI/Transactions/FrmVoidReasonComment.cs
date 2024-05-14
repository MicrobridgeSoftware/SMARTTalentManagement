using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace SmartTalentManagement.GUI.Transactions
{
    public partial class FrmVoidReasonComment : Telerik.WinControls.UI.RadForm
    {
        private int transactionDetailId;
        private Dictionary<int, string> transactionVoidDictionary;

        public FrmVoidReasonComment()
        {
            InitializeComponent();
        }

        public FrmVoidReasonComment(int transactionDetailId, Dictionary<int, string> transactionVoidDictionary) : this()
        {
            this.transactionDetailId = transactionDetailId;
            this.transactionVoidDictionary = transactionVoidDictionary;
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVoidReson.Text.Trim()))
            {
                RadMessageBox.Show("A reason must be provided for this transaction!", Application.ProductName);
                return;
            }

            transactionVoidDictionary.Add(transactionDetailId, txtVoidReson.Text.Trim());

            Close();
        }
    }
}