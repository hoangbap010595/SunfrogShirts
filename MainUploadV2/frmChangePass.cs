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
    public partial class frmChangePass : XtraForm
    {
        private AccountServices accService = new AccountServices();
        public frmChangePass()
        {
            InitializeComponent();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            string username = frmMain.CURRENT_USERNAME;
            string oldPass = txtOldPass.Text;
            string newPass = txtNewPass.Text;
            string confirm = txtConfirmPass.Text;

            string curr_UserID = accService.CheckLogin(username, oldPass);
            if (curr_UserID != "-1")
            {
                if (newPass != confirm)
                {
                    XtraMessageBox.Show("Mật khẩu không trùng khớp.");
                }
                else {
                  int rs =  accService.ChangePassword(username, newPass);
                  if (rs > 0)
                  {
                      XtraMessageBox.Show("Cập nhật mật khẩu thành công.");
                      this.Close();
                  }
                  else
                  {
                      XtraMessageBox.Show("Cập nhật mật khẩu thất bại.");
                  }
                }
            }
            else
            {
                XtraMessageBox.Show("Mật khẩu cũ không đúng.");
            }
        }
    }
}
