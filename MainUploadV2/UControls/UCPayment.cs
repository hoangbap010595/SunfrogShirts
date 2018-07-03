using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MainUploadV2.Services;

namespace MainUploadV2.UControls
{
    public partial class UCPayment : XtraUserControl
    {
        private PaymentServices payService = new PaymentServices();
        public UCPayment()
        {
            InitializeComponent();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            frmNewPayMent frm = new frmNewPayMent();
            frm.ShowDialog();
        }

        private void UCAccount_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = payService.GetListPayMent();
        }
    }
}
