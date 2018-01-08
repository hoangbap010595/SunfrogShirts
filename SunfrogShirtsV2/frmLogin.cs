using DevExpress.XtraEditors;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using SunfrogShirtsV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SunfrogShirtsV2
{
    public partial class frmLogin : MaterialForm
    {
        private ApplicationUser User;
        private MaterialSkinManager mSkin;
        private CookieContainer cookieContainer = new CookieContainer();
        private List<OProduct> listProductDesign;
        private List<Cookie> lsCookies = new List<Cookie>();

        public delegate void SendUser(ApplicationUser user, List<Cookie> cookies, List<OProduct> oProduct);
        public SendUser senduser;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mSkin = MaterialSkinManager.Instance;
            mSkin.AddFormToManage(this);
            mSkin.Theme = MaterialSkinManager.Themes.LIGHT;
            mSkin.ColorScheme = new ColorScheme(Primary.Cyan800, Primary.BlueGrey900, Primary.DeepPurple100, Accent.Cyan700, TextShade.WHITE);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUserName.Text.Trim();
                var password = txtPassword.Text.Trim(); // ApplicationLibary.Base64Decode("");
                //OpenFileDialog open = new OpenFileDialog();
                //if (DialogResult.OK == open.ShowDialog())
                //{
                //    string dataFile = File.ReadAllText(open.FileName);
                //    var encodeData = ApplicationLibary.Base64Decode(dataFile);
                //    JObject jObj = JObject.Parse(encodeData);

                //    var data = jObj["data"].ToString();
                //    var offset = jObj["offset"].ToString();

                //    var dataLogin = data.Remove(0, int.Parse(offset));
                //    var afterDecode = ApplicationLibary.Base64Decode(dataLogin);

                //    JObject jObjLogin = JObject.Parse(afterDecode);
                //    username = jObjLogin["username"].ToString();
                //    password = jObjLogin["password"].ToString();

                //}
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
            btnLogin.Enabled = false;
            progressLogin.Value = 80;
            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    lsCookies = new List<Cookie>();
                    HttpWebRequest wRequest_1 = (HttpWebRequest)WebRequest.Create("https://manager.sunfrog.com/");
                    wRequest_1.Method = "GET";
                    wRequest_1.CookieContainer = new CookieContainer();
                    WebResponse resp_1 = wRequest_1.GetResponse();

                    foreach (Cookie cookie in ((HttpWebResponse)resp_1).Cookies)
                    {
                        lsCookies.Add(cookie);
                    }

                    //data
                    //username=lchoang1995%40gmail.com&password=Omega%40111&login=Login
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    var enUserName = HttpUtility.UrlEncode(username);
                    var enPassword = HttpUtility.UrlEncode(password);
                    string data = "username=" + enUserName + "&password=" + enPassword + "&login=Login";
                    byte[] postDataBytes = encoding.GetBytes(data);

                    //cookieContainer.Add()

                    HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create("https://manager.sunfrog.com/");
                    wRequest.Method = "POST";
                    wRequest.ContentType = "application/x-www-form-urlencoded";
                    wRequest.ContentLength = postDataBytes.Length;
                    ApplicationLibary.TryAddCookie(wRequest, lsCookies);

                    using (Stream sr = wRequest.GetRequestStream())
                    {
                        sr.Write(postDataBytes, 0, postDataBytes.Length);
                    }
                    WebResponse resp = wRequest.GetResponse();
                    foreach (Cookie cookie in ((HttpWebResponse)resp).Cookies)
                    {
                        //cookieContainer.Add(cookie);
                        lsCookies[2] = cookie;
                    }

                    var pageSource = "";
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        pageSource = sr.ReadToEnd();
                    }
                    resp.Close();

                    //<strong style="font-size:1.5em; line-height:15px; padding-bottom:0px;" class="clearfix">(.*?)</strong>
                    var regex = "<strong style=\"font-size:1.5em; line-height:15px; padding-bottom:0px;\" class=\"clearfix\">(?<myId>.*?)</strong>";
                    MatchCollection matchCollection = Regex.Matches(pageSource, regex);

                    if (matchCollection.Count == 0)
                    {
                        btnLogin.Invoke((MethodInvoker)delegate { btnLogin.Enabled = true; });
                        progressLogin.Invoke((MethodInvoker)delegate { progressLogin.Value = 0; });
                        XtraMessageBox.Show("Email or Password incorrect!", "Message");
                        return;
                    }
                    foreach (Match match in matchCollection)
                    {
                        var id = match.Groups["myId"].Value;
                        User = new ApplicationUser();
                        User.Id = id;

                        //Load Design
                        HttpWebRequest rDesign = (HttpWebRequest)WebRequest.Create(ApplicationLibary.UrlDataDesign);
                        rDesign.Host = "manager.sunfrog.com";
                        rDesign.Referer = "https://manager.sunfrog.com/Designer/";

                        Dictionary<string, object> dataDesign = GetDataAPI(rDesign);
                        JArray objDesign = JArray.Parse(dataDesign["data"].ToString());
                        listProductDesign = new List<OProduct>();
                        for (int i = 0; i < objDesign.Count; i++)
                        {
                            OProduct p = new OProduct();
                            p.Id = int.Parse(objDesign[i]["id"].ToString());
                            p.Description = objDesign[i]["description"].ToString();
                            p.BasePrice = double.Parse(objDesign[i]["basePrice"].ToString());
                            p.BackPrintPrice = double.Parse(objDesign[i]["backPrintPrice"].ToString());
                            p.ImageFront = objDesign[i]["imageFront"].ToString();
                            p.ImageBack = objDesign[i]["imageBack"].ToString();
                            p.Colors = new List<OColor>();
                            JArray jColors = JArray.Parse(objDesign[i]["colors"].ToString());
                            for (int j = 0; j < jColors.Count; j++)
                            {
                                OColor c = new OColor();
                                c.Id = int.Parse(jColors[j]["id"].ToString());
                                c.Name = jColors[j]["name"].ToString();
                                c.Value = jColors[j]["value"].ToString();
                                c.Texture = jColors[j]["texture"].ToString();
                                p.Colors.Add(c);
                            }
                            listProductDesign.Add(p);
                        }
                        //Send data
                        if (senduser != null)
                        {
                            senduser(User, lsCookies, listProductDesign);
                            this.Invoke((MethodInvoker)delegate { this.Close(); });
                        }
                    }

                }
                catch (Exception ex)
                {
                    progressLogin.Invoke((MethodInvoker)delegate { progressLogin.Value = 0; });
                    XtraMessageBox.Show(ex.Message, "Error");
                    btnLogin.Invoke((MethodInvoker)delegate { btnLogin.Enabled = true; });
                }
            }));
            t.Start();
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
        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (User == null)
            {
                Application.ExitThread();
                Application.Exit();
            }
        }
        
        private Dictionary<string, object> GetDataAPI(HttpWebRequest wRequest, string data2Send = "")
        {
            wRequest.Method = "GET";
            wRequest.UserAgent = ApplicationLibary.BROWSER_FIREFOX;
            wRequest.Headers.Add("Origin", ApplicationLibary.Origin);
            wRequest.ServicePoint.Expect100Continue = false;
            wRequest.ProtocolVersion = HttpVersion.Version11;
            wRequest.Timeout = 90000;
            wRequest.ReadWriteTimeout = 90000;
            wRequest.KeepAlive = true;

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
                cookies.Add(cookie);
            }

            String htmlString;
            using (var reader = new StreamReader(wResponse.GetResponseStream()))
            {
                htmlString = reader.ReadToEnd();
            }
            wResponse.Close();
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            dataReturn.Add("cookies", cookies);
            dataReturn.Add("data", htmlString);
            return dataReturn;
        }
    }
}
