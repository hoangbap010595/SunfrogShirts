using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MainUploadV2.Services;

namespace MainUploadV2
{
    public partial class frmNewPayMent : XtraForm
    {
        private PaymentServices payService = new PaymentServices();
        private AccountServices accService = new AccountServices();
        private ToolServices toolService = new ToolServices();
        public frmNewPayMent()
        {
            InitializeComponent();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            int ToolID = int.Parse(cbbTool.SelectedValue.ToString());
            string UserID = cbbUser.SelectedValue.ToString();
            string DateExpire = dtpExpires.DateTime.ToString("yyyy-MM-dd");
            string Note = txtNote.Text;

            string data = payService.PaymenTool(UserID, ToolID, DateExpire, Note);

            if (int.Parse(data) == 1)
            {
                XtraMessageBox.Show("Thêm mới thanh toán thành công.");
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("Thêm mới thanh toán thất bại.");
            }
        }

        private void frmNewPayMent_Load(object sender, EventArgs e)
        {
            cbbUser.DataSource = accService.GetListDataMember();
            cbbUser.ValueMember = "UserID";
            cbbUser.DisplayMember = "LoginID";

            cbbTool.DataSource = toolService.GetListDataTool();
            cbbTool.ValueMember = "ToolID";
            cbbTool.DisplayMember = "ToolName";

            dtpExpires.DateTime = DateTime.Now;
        }


    }
}
