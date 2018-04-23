using DevExpress.XtraEditors;
using MainUploadV2.SpreadShirts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainUploadV2.Services;

namespace MainUploadV2
{
    public partial class frmMain : XtraForm
    {
        private AccountServices accService = new AccountServices();
        private int _CurrentTime = 0;
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnActiveTool_Click(object sender, EventArgs e)
        {
            Form frm = Application.OpenForms["frmSpreadShirts"];
            if (frm == null)
            {
                frm = new frmSpreadShirts();
                frm.Show();
            }
            else
            {
                frm.Activate();
                frm.Focus();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUserName.Text.Trim();
            string pass = txtPassword.Text.Trim();
            var rs = accService.CheckLogin(user, pass);
            if(rs == 1)
            {

            }else
            {
                XtraMessageBox.Show("Sai tài khoản hoặc mật khẩu.");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            timerOnline.Tick += TimerOnline_Tick;
            timerOnline.Start();
            lblToDay.Text = DateTime.Now.ToString("MMM, dd-yyyy");

            Int64 currTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Int64 dueTime = 1524207791000;
            currTime = currTime * 1000;
            if (currTime > dueTime)
            {
                btnLogin.Enabled = false;
                btnLogin.Visible = false;
                txtUserName.Enabled = txtPassword.Enabled = false;
                btnActiveToolSpread.Enabled = false;
                btnActiveToolSpread.Visible = false;
                XtraMessageBox.Show("Thời gian dùng thử đã kết thúc", "Thông báo");
            }

        }

        private void TimerOnline_Tick(object sender, EventArgs e)
        {
            _CurrentTime++;
            lblTimeLive.Text = convertTime(_CurrentTime);
        }

        private string convertTime(int secs)
        {
            int hours = secs / 3600;
            int mins = (secs % 3600) / 60;
            secs = secs % 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, mins, secs);
        }
    }
}
