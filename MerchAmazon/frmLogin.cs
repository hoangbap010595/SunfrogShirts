using DevExpress.XtraEditors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MerchAmazon
{
    public partial class frmLogin : XtraForm
    {
        private static CookieContainer cookieApplication = new CookieContainer();
        private ApplicationUser User;
        private string currToken = "";

        public delegate void SendUser(ApplicationUser user, CookieContainer cookies);
        public SendUser senduser;
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUserName.Text.Trim();
                var password = txtPassword.Text.Trim(); // ApplicationLibary.Base64Decode("");
                OpenFileDialog open = new OpenFileDialog();
                if (DialogResult.OK == open.ShowDialog())
                {
                    string dataFile = File.ReadAllText(open.FileName);
                    var encodeData = ApplicationLibary.Base64Decode(dataFile);
                    JObject jObj = JObject.Parse(encodeData);

                    var data = jObj["data"].ToString();
                    var offset = jObj["offset"].ToString();

                    var dataLogin = data.Remove(0, int.Parse(offset));
                    var afterDecode = ApplicationLibary.Base64Decode(dataLogin);
                    var a = @afterDecode;
                    JObject jObjLogin = JObject.Parse(a);
                    username = jObjLogin["username"].ToString();
                    password = jObjLogin["password"].ToString();

                }
                if (username != "" && password != "")
                {
                    executeLogin(username, password);
                }
                else
                {
                    XtraMessageBox.Show("Không tìm thấy dữ liệu!", "Message");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Message");
            }
        }
        private void executeLogin(string username, string password)
        {
            frmWait frm = new frmWait();
            frm.SetCaption("Login");
            frm.SetDescription("Connecting...");
            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    string urlLogin = "https://www.amazon.com/ap/signin?_encoding=UTF8&ignoreAuthState=1&openid.assoc_handle=usflex&openid.claimed_id=http://specs.openid.net/auth/2.0/identifier_select&openid.identity=http://specs.openid.net/auth/2.0/identifier_select&openid.mode=checkid_setup&openid.ns=http://specs.openid.net/auth/2.0&openid.ns.pape=http://specs.openid.net/extensions/pape/1.0&openid.pape.max_auth_age=0&openid.return_to=https://www.amazon.com/?ref_=nav_custrec_signin&switch_account=";
                    string data2Send = "{\"rememberMe\":false,\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

                    HttpWebRequest wRequestLogin = (HttpWebRequest)WebRequest.Create(urlLogin);
                    wRequestLogin.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                    wRequestLogin.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    wRequestLogin.Host = "www.amazon.com";
                    wRequestLogin.ContentType = "application/x-www-form-urlencoded";
                    wRequestLogin.Referer = "https://partner.spreadshirt.com/login";
                    wRequestLogin.CookieContainer = new CookieContainer();

                    Dictionary<string, object> step2Login = PostDataAPI(wRequestLogin, data2Send);
                    cookieApplication = (CookieContainer)step2Login["cookies"];
                    var rs = step2Login["data"].ToString();
                    if (int.Parse(step2Login["status"].ToString()) == -1)
                    {
                        XtraMessageBox.Show("Sai thông tin tài khoản hoặc mật khẩu\n" + rs, "Thông báo");
                        return;
                    }

                    var obj = JObject.Parse(rs);
                    User = new ApplicationUser();
                   

                    frm.Invoke((MethodInvoker)delegate { frm.Close(); });
                    if (senduser != null)
                    {
                        senduser(User, cookieApplication);
                        this.Invoke((MethodInvoker)delegate { this.Close(); });
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error");
                    frm.Invoke((MethodInvoker)delegate { frm.Close(); });
                }
            }));
            t.Start();
            frm.ShowDialog();
        }
        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txtPassword.Focus();
        }
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnLogin.PerformClick();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (User == null)
            {
                Application.ExitThread();
                Application.Exit();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            User = new ApplicationUser();
            //Int64 currTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //Int64 dueTime = 1524060800000;
            //currTime = currTime * 1000;
            //if (currTime > dueTime)
            //{
            //    btnLogin.Enabled = false;
            //    btnLogin.Visible = false;
            //    txtUserName.Enabled = false;
            //    txtPassword.Enabled = false;
            //    XtraMessageBox.Show("Thời gian dùng thử đã kết thúc", "Thông báo");
            //}
            //else
            //{
            //    XtraMessageBox.Show("Thời gian dùng thử kết thúc 4/18/2018, 9:13:20 PM", "Thông báo");
            //}
        }
        private Dictionary<string, object> PostDataAPI(HttpWebRequest wRequest, string data2Send)
        {
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            CookieContainer cookies = new CookieContainer();
            String htmlString;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);
                wRequest.Method = "POST";
                wRequest.UserAgent = ApplicationLibary.BROWSER_FIREFOX;
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.Headers.Add("Origin", ApplicationLibary.Origin);

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }

                using (HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse())
                {
                    foreach (Cookie cookie in wResponse.Cookies)
                    {
                        if (cookie.Name.Contains("sprd_auth_token"))
                            currToken = cookie.Value;
                        cookies.Add(cookie);
                    }
                    using (var reader = new StreamReader(wResponse.GetResponseStream()))
                    {
                        htmlString = reader.ReadToEnd();
                    }
                }

                dataReturn.Add("cookies", cookies);
                dataReturn.Add("data", htmlString);
                dataReturn.Add("status", 1);
                return dataReturn;
            }
            catch (Exception ex)
            {
                dataReturn.Add("cookies", cookies);
                dataReturn.Add("data", ex.Message);
                dataReturn.Add("status", -1);
                return dataReturn;
            }

        }
        private Dictionary<string, object> GetDataAPI(HttpWebRequest wRequest, string data2Send = "")
        {
            wRequest.Method = "GET";
            wRequest.UserAgent = ApplicationLibary.BROWSER_FIREFOX;
            wRequest.Headers.Add("Origin", ApplicationLibary.Origin);
            if (data2Send != "")
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);
                wRequest.ContentLength = postDataBytes.Length;

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }
            }

            HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
            CookieContainer cookies = new CookieContainer();
            foreach (Cookie cookie in wResponse.Cookies)
            {
                if (cookie.Name.Contains("sprd_auth_token"))
                    currToken = cookie.Value;
                cookies.Add(cookie);
            }

            String htmlString;
            using (var reader = new StreamReader(wResponse.GetResponseStream()))
            {
                htmlString = reader.ReadToEnd();
            }

            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            dataReturn.Add("cookies", cookies);
            dataReturn.Add("data", htmlString);

            return dataReturn;
        }
    }
}
