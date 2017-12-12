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

namespace SpreadShirts
{
    public partial class frmLogin : XtraForm
    {
        private static CookieContainer cookieApplication = new CookieContainer();
        private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:49.0) Gecko/20100101 Firefox/49.0";
        private ApplicationUser User;
        private string currToken = "";

        public delegate void SendUser(ApplicationUser user);
        public SendUser senduser;
        public frmLogin()
        {
            InitializeComponent();
            User = new ApplicationUser();
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

                    JObject jObjLogin = JObject.Parse(afterDecode);
                    username = jObjLogin["username"].ToString();
                    password = jObjLogin["password"].ToString();

                }
                if(username != "" && password != "")
                {
                    executeLogin(username, password);
                }
            }catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Message");
            }
        }
        private void executeLogin(string username, string password)
        {
            try
            {
                //Login Page
                //https://partner.spreadshirt.com/api/v1/sessions?apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b&locale=us_US&mediaType=json&sig=3a0fa71c6252416ba5479f2fd749eb586f34d1b7&time=1513042887199

                var urlLogin = ApplicationLibary.encodeURL("https://partner.spreadshirt.com/api/v1/sessions", "", "POST", "us_US", "json", "");
                //string urlLogin = "https://www.spreadshirt.com/api/v1/sessions?mediaType=json";
                string data2Send = "{\"rememberMe\":false,\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";

                HttpWebRequest wRequestLogin = (HttpWebRequest)WebRequest.Create(urlLogin);
                // wRequestLogin.CookieContainer = cookieApplication;
                wRequestLogin.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wRequestLogin.Accept = "application/json, text/plain, */*";
                wRequestLogin.Host = "partner.spreadshirt.com";
                wRequestLogin.ContentType = "application/json;charset=utf-8";
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
                cookieApplication = (CookieContainer)step2Login["cookies"];

                //var locationHeader = rspLogin.Headers["location"];
                //var cookieHeader = rspLogin.Headers["Set-Cookie"];
                //fixCookies(wRequestLogin, (HttpWebResponse)rspLogin);

                var obj = JObject.Parse(rs);
                User = new ApplicationUser();
                User.SESSION_ID = obj["id"].ToString();
                User.SESSION_HREF = obj["href"].ToString();
                User.IDENTITY_ID = obj["identity"]["id"].ToString();
                User.IDENTITY_HREF = obj["identity"]["href"].ToString();
                User.USER_ID = obj["user"]["id"].ToString();
                User.USER_HREF = obj["user"]["href"].ToString();

                /*
                 *--hoangbap1595 
                 * 73338c5b-2cbe-4333-8164-b23e30a6e0da
                 * https://www.spreadshirt.com/api/v1/sessions/73338c5b-2cbe-4333-8164-b23e30a6e0da
                 * NA-2832173b-7196-4e59-bf8d-3740fdb3c71a
                 * https://www.spreadshirt.com/api/v1/identities/NA-2832173b-7196-4e59-bf8d-3740fdb3c71a
                 * 302721328
                 * https://www.spreadshirt.com/api/v1/users/302721328
                 * 
                 * --lcoang1995
                 * 4f6e5652-e7a7-47b5-bf0a-d96c8cbaf458
                 * https://www.spreadshirt.com/api/v1/sessions/4f6e5652-e7a7-47b5-bf0a-d96c8cbaf458
                 * NA-776b20df-bfb4-4d47-a225-9d1252f9fd4c
                 * https://www.spreadshirt.com/api/v1/identities/NA-776b20df-bfb4-4d47-a225-9d1252f9fd4c
                 * 302719724
                 * https://www.spreadshirt.com/api/v1/users/302719724
                 * 
                /*-------------------LOGIN FINISH-------------------*/

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
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
            //Application.ExitThread();
            //Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

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
                wRequest.UserAgent = BROWSER_FIREFOX;
                wRequest.ContentLength = postDataBytes.Length;
                wRequest.Headers.Add("Origin", "chrome-extension://aejoelaoggembcahagimdiliamlcdmfm");

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

    }
}
