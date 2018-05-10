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
using TCProShirts;

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
            if(rs != "-1")
            {
                Dictionary<string, object> data = accService.GetInfoMember(rs);
                var dateExpires = DateTime.Parse(data["DateExpires"].ToString());
                var currDate = DateTime.UtcNow;
                if (currDate > dateExpires)
                {
                    XtraMessageBox.Show("Tài khoản của bạn đã hết hạn sử dụng.\nVui lòng liên hệ với quản trị hệ thống.");
                    return;
                }
                btnLogin.Enabled = false;
                XtraMessageBox.Show("Đăng nhập thành công.");
                lblExpires.Text = data["DateExpires"].ToString();
                lblDateCreate.Text = data["DateCreate"].ToString();
                lblDayExpires.Text = data["DayExpires"].ToString() + " day(s)";
                List<Dictionary<string, object>> tools = accService.GetToolForUser(rs);
                if (tools.Count > 0)
                {
                    foreach (Dictionary<string,object> item in tools)
                    {
                        int toolID = int.Parse(item["ToolID"].ToString());
                        switch (toolID)
                        {
                            case 1:
                                btnActiveToolSpread.Enabled = true;
                                break;
                            case 2:
                                btnAcctiveTeeChipPro.Enabled = true;
                                break;
                            case 3:
                                btnAcctiveMerchAmazon.Enabled = true;
                                break;
                            case 4:
                                btnAcctiveTeePublic.Enabled = true;
                                break;
                        }
                    }
                }else
                {
                    XtraMessageBox.Show("Không tìm thấy tool được kích hoạt cho tài khoản của bạn.\nVui lòng liên hệ quản trị hệ thống.");
                }
            }
            else
            {
                XtraMessageBox.Show("Sai tài khoản hoặc mật khẩu.");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            timerOnline.Tick += TimerOnline_Tick;
            timerOnline.Start();
            lblToDay.Text = DateTime.Now.ToString("MMM, dd-yyyy");
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

        private void btnAcctiveTeeChipPro_Click(object sender, EventArgs e)
        {
            TCProShirts.frmMainTeechip frm = new frmMainTeechip();
            frm.Show();
        }
    }
}
