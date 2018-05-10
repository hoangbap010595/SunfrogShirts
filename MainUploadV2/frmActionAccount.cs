using DevExpress.XtraEditors;
using MainUploadV2.DataHelper;
using MainUploadV2.Services;
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
    public partial class frmActionAccount : XtraForm
    {
        private AccountServices accService = new AccountServices();
        public frmActionAccount()
        {
            InitializeComponent();
        }

        private void btnTheem_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string fullname = txtFullName.Text.Trim();
            string acctype = cbbAccountType.SelectedValue.ToString();
            string dateexpires = dtpExpires.DateTime.ToString("MM-dd-yyyy");
            int rs = accService.InsertNewAccount(username, password, fullname, dateexpires, acctype);
            if(rs == -1)
            {
                XtraMessageBox.Show("Account đã tồn tại trong hệ thống.");
                return;
            }else
            {
                XtraMessageBox.Show("Thêm thành công!");
            }
        }

        private void frmActionAccount_Load(object sender, EventArgs e)
        {
            cbbAccountType.DataSource = DataObjectModel.getListAccType();
            cbbAccountType.ValueMember = "Id";
            cbbAccountType.DisplayMember = "Name";
        }
    }
}
