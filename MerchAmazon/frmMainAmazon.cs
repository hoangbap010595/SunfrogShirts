using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;


namespace MerchAmazon
{
    public partial class frmMainAmazon : XtraForm
    {
        private DataTable dataTableFileExcel;
        public frmMainAmazon()
        {
            InitializeComponent();
        }

        #region ====Check T-Shirt Upload Type
        private void ckUTypeDraff_CheckedChanged(object sender, EventArgs e)
        {
            if(ckUTypeDraff.Checked)
            {
                ckUTypeSample.Checked = ckUTypeSell.Checked = false;
            }
        }

        private void ckUTypeSample_CheckedChanged(object sender, EventArgs e)
        {
            if (ckUTypeSample.Checked)
            {
                ckUTypeDraff.Checked = ckUTypeSell.Checked = false;
            }
        }

        private void ckUTypeSell_CheckedChanged(object sender, EventArgs e)
        {
            if (ckUTypeSell.Checked)
            {
                ckUTypeSample.Checked = ckUTypeDraff.Checked = false;
            }
        }
        #endregion

        private void btnOpenFileCSV_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Excel .csv|*.csv|Excel .xlsx|*.xlsx|Excel .xls|*.xls";
                if (DialogResult.OK == op.ShowDialog())
                {
                    dataTableFileExcel = new DataTable();
                    var x = Path.GetExtension(op.FileName);
                    if (x == ".csv")
                        dataTableFileExcel = ApplicationLibary.getDataExcelFromFileCSVToDataTable(op.FileName);
                    else
                        dataTableFileExcel = ApplicationLibary.getDataExcelFromFileToDataTable(op.FileName);
                    lblTotalRecordFile.Text = dataTableFileExcel.Rows.Count + "record(s)";
                    ApplicationLibary.writeLog(lsBoxLog, "Success " + dataTableFileExcel.Rows.Count + " record(s) is opened", 1);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnStartUpload_Click(object sender, EventArgs e)
        {

        }
    }
}
