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
using System.Net;
using System.IO;

namespace MainUploadV2
{
    public partial class frmMain : XtraForm
    {
        public static string CURRENT_USERNAME = "";
        private AccountServices accService = new AccountServices();
        private Timer timer_Expires = null;
        private int _CurrentTime = 0;
        private string curr_UserID = "";
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
            curr_UserID = accService.CheckLogin(user, pass);
            if (curr_UserID != "-1")
            {
                CURRENT_USERNAME = user;
                if (ckRemember.Checked)
                {
                    PartialViewService.Username = user;
                    PartialViewService.Password = pass;
                    PartialViewService.Remember = ckRemember.Checked;
                }
                Dictionary<string, object> data = accService.GetInfoMember(curr_UserID);
                var dateExpires = DateTime.Parse(data["DateExpires"].ToString());
                var currDate = DateTime.UtcNow;
                if (currDate > dateExpires)
                {
                    XtraMessageBox.Show("Tài khoản của bạn đã hết hạn sử dụng.\nVui lòng liên hệ với quản trị hệ thống.");
                    return;
                }
                btnLogin.Enabled = false;
                linkDoiMatKhau.Visible = true;
                XtraMessageBox.Show("Đăng nhập thành công.");
                lblExpires.Text = data["DateExpires"].ToString();
                lblDateCreate.Text = data["DateCreate"].ToString();
                lblDayExpires.Text = data["DayExpires"].ToString() + " day(s)";
                List<Dictionary<string, object>> tools = accService.GetToolForUser(curr_UserID);
                if (tools.Count > 0)
                {
                    foreach (Dictionary<string, object> item in tools)
                    {
                        int toolID = int.Parse(item["ToolID"].ToString());
                        DateTime dateExpire = DateTime.Parse(item["DateExpires"].ToString());
                        string Expires = item["Expires"].ToString();
                        switch (toolID)
                        {
                            case 1:
                                if (DateTime.UtcNow > dateExpire)
                                {
                                    btnActiveToolSpread.Enabled = false;
                                    btnActiveToolSpread.Text = "SpreadShirt (Hết hạn)";
                                }
                                else
                                {
                                    btnActiveToolSpread.Enabled = true;
                                    btnActiveToolSpread.ToolTip = "Ngày hết hạn:" + dateExpire.ToString("dd-MM-yyyy") + " (" + Expires + " ngày)";
                                }
                                break;
                            case 2:
                                if (DateTime.UtcNow > dateExpire)
                                {
                                    btnAcctiveTeeChipPro.Enabled = false;
                                    btnAcctiveTeeChipPro.Text = "TeeChip Pro (Hết hạn)";
                                }
                                else
                                {
                                    btnAcctiveTeeChipPro.Enabled = true;
                                    btnAcctiveTeeChipPro.ToolTip = "Ngày hết hạn:" + dateExpire.ToString("dd-MM-yyyy") + " (" + Expires + " ngày)";
                                }
                                break;
                            case 3:
                                if (DateTime.UtcNow > dateExpire)
                                {
                                    btnAcctiveMerchAmazon.Enabled = false;
                                    btnAcctiveMerchAmazon.Text = "MerchAmazon (Hết hạn)";
                                }
                                else
                                {
                                    btnAcctiveMerchAmazon.Enabled = true;
                                    btnAcctiveMerchAmazon.ToolTip = "Ngày hết hạn:" + dateExpire.ToString("dd-MM-yyyy") + " (" + Expires + " ngày)";
                                }
                                break;
                            case 4:
                                if (DateTime.UtcNow > dateExpire)
                                {
                                    btnAcctiveTeePublic.Enabled = false;
                                    btnAcctiveTeePublic.Text = "Tee Public (Hết hạn)";
                                }
                                else
                                {
                                    btnAcctiveTeePublic.Enabled = true;
                                    btnAcctiveTeePublic.ToolTip = "Ngày hết hạn:" + dateExpire.ToString("dd-MM-yyyy") + " (" + Expires + " ngày)";
                                }
                                break;
                        }
                    }
                }
                else
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
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            timerOnline.Tick += TimerOnline_Tick;
            timerOnline.Start();
            lblToDay.Text = DateTime.Now.ToString("MMM, dd-yyyy");

            timer_Expires = new Timer();
            timer_Expires.Interval = 1800000;
            timer_Expires.Tick += timer_Expires_Tick;
            timer_Expires.Enabled = true;
            timer_Expires.Start();

            if (PartialViewService.Remember)
            {
                txtUserName.Text = PartialViewService.Username;
                txtPassword.Text = PartialViewService.Password;
                ckRemember.Checked = PartialViewService.Remember;
            }
        }

        void timer_Expires_Tick(object sender, EventArgs e)
        {
            Dictionary<string, object> data = accService.GetInfoMember(curr_UserID);
            var dateExpires = DateTime.Parse(data["DateExpires"].ToString());
            var currDate = DateTime.UtcNow;
            if (currDate > dateExpires)
            {
                XtraMessageBox.Show("Tài khoản của bạn đã hết hạn sử dụng.\nVui lòng liên hệ với quản trị hệ thống.");
                btnAcctiveMerchAmazon.Enabled = btnAcctiveTeeChipPro.Enabled = btnAcctiveTeePublic.Enabled = btnActiveToolSpread.Enabled = false;
                Form frm = Application.OpenForms["frmSpreadShirts"];
                if (frm != null)
                {
                    frm.Close();
                }
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

        private void btnAcctiveTeeChipPro_Click(object sender, EventArgs e)
        {
            TCProShirts.frmMainTeechip frm = new frmMainTeechip();
            frm.Show();
        }

        private void linkDoiMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmChangePass frm = new frmChangePass();
            frm.ShowDialog();
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmHuongDan frm = new frmHuongDan();
            frm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == XtraMessageBox.Show("Kết thúc phiên làm việc ?", "Xác thực", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                Application.Exit();
        }

        private void btnCheckForUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string fileNameUpdate = "\\updater.exe";
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                var filePath = String.Concat(appPath, fileNameUpdate);
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception exx)
            {
                XtraMessageBox.Show(exx.Message);
            }
        }
    }
}
