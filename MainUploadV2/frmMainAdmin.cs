using DevExpress.XtraEditors;
using MainUploadV2.UControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainUploadV2
{
    public partial class frmMainAdmin : XtraForm
    {
        public frmMainAdmin()
        {
            InitializeComponent();
        }

        private void btnManageAccount_Click(object sender, EventArgs e)
        {
            groupControlMain.Controls.Clear();
            UCAccount frm = new UCAccount();
            frm.Dock = DockStyle.Fill;
            groupControlMain.Controls.Add(frm);
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            groupControlMain.Controls.Clear();
            UCAccount frm = new UCAccount();
            frm.Dock = DockStyle.Fill;
            groupControlMain.Controls.Add(frm);
        }
    }
}
