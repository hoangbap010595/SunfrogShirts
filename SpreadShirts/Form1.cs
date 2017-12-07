using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using DevExpress.XtraEditors;

namespace SpreadShirts
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        //https://www.spreadshirt.com/userarea/-C6840/User/ApiKeys/index
        //apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b
        //sig=da24c460f069ccc8a499fd96b395e8329cd9b670
        //time=1510655123467
        private string API_KEY = "1c711bf5-b82d-40de-bea6-435b5473cf9b";//912dac48-fb39-4aef-af8c-d1260755bcad
        private string SECRET = "fd9f23cc-2432-4a69-9dad-bbd57b7b9fdd";//29503fe1-800c-430c-98f0-b8f08e13123d
        private static CookieContainer cookieApplication = new CookieContainer();
        private string BROWSER_CHROME = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private static string BROWSER_FIREFOX = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0";
        private ApplicationUser User;
        private string currToken = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string urlPage = "https://www.spreadshirt.com";
                HttpWebRequest wRequestPage = (HttpWebRequest)WebRequest.Create(urlPage);
                wRequestPage.Method = "GET";
                wRequestPage.Host = "www.spreadshirt.com";
                wRequestPage.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                wRequestPage.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wRequestPage.UseDefaultCredentials = true;
                wRequestPage.UserAgent = BROWSER_FIREFOX;
                wRequestPage.Referer = urlPage;
                wRequestPage.CookieContainer = cookieApplication;

                var rPage = GetDataAPI(wRequestPage);
                cookieApplication = (CookieContainer)rPage["cookies"];

                //Login Page
                string urlLogin = "https://www.spreadshirt.com/api/v1/sessions?mediaType=json";
                string data2Send = "{\"username\":\"hoangbap1595@gmail.com\", \"password\":\"Thienan@111\", \"rememberMe\":false}";

                HttpWebRequest wRequestLogin = (HttpWebRequest)WebRequest.Create(urlLogin);
               // wRequestLogin.CookieContainer = cookieApplication;
                wRequestLogin.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wRequestLogin.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                wRequestLogin.Host = "www.spreadshirt.com";
                wRequestLogin.ContentType = "application/x-www-form-urlencoded";
                wRequestLogin.Referer = "https://www.spreadshirt.com/";

                Dictionary<string, object> step2Login = PostDataAPI(wRequestLogin, data2Send);
                cookieApplication = (CookieContainer)step2Login["cookies"];
                var rs = step2Login["data"].ToString();
                if (int.Parse(step2Login["status"].ToString()) == -1)
                {
                    XtraMessageBox.Show("Sai thông tin tài khoản hoặc mật khẩu\n" + rs, "Thông báo");
                    return;
                }

                WebResponse rspLogin = wRequestLogin.GetResponse();
                cookieApplication = new CookieContainer();
                foreach (Cookie cookie in ((HttpWebResponse)rspLogin).Cookies)
                {
                    cookieApplication.Add(cookie);
                }
                var locationHeader = rspLogin.Headers["location"];
                var cookieHeader = rspLogin.Headers["Set-Cookie"];
                //fixCookies(wRequestLogin, (HttpWebResponse)rspLogin);
                String htmlString;
                using (var reader = new StreamReader(rspLogin.GetResponseStream()))
                {
                    htmlString = reader.ReadToEnd();
                }
                var obj = JObject.Parse(htmlString);
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
                if (User.USER_ID != "")
                    MessageBox.Show(User.USER_ID, "UID");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            cookieApplication = new CookieContainer();

            cookieApplication.Add(new Cookie("sprd_auth_token", User.SESSION_ID) { Domain = ".spreadshirt.com" });
            //https://partner.spreadshirt.com/api/v1/users/302719724/design-uploads?apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b&locale=us_US&mediaType=json&sig=bad28978fba33b86bd7f52bc4027231c81515da8&time=1510932983372
            string tURL = "https://partner.spreadshirt.com/api/v1/users/" + User.USER_ID + "/design-uploads?apiKey=1c711bf5-b82d-40de-bea6-435b5473cf9b&locale=us_US&mediaType=json&sig=bad28978fba33b86bd7f52bc4027231c81515da8&time=1510932983372";
            string temp = encodeURL(tURL, "", "GET", "us_US", "json", "");
            HttpWebRequest wGETRequest = (HttpWebRequest)WebRequest.Create(temp);
            wGETRequest.Method = "GET";
            wGETRequest.Host = "partner.spreadshirt.com";
            wGETRequest.UserAgent = BROWSER_FIREFOX;
            wGETRequest.Accept = "application/json, text/plain, */*";
            wGETRequest.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wGETRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wGETRequest.Referer = "https://partner.spreadshirt.com/designs";
            wGETRequest.CookieContainer = cookieApplication;

            WebResponse responseGET = wGETRequest.GetResponse();
            StreamReader responseGETReader = new StreamReader(responseGET.GetResponseStream());
            string rsGET = responseGETReader.ReadToEnd();

            foreach (Cookie cookie in ((HttpWebResponse)responseGET).Cookies)
            {
                cookieApplication.Add(cookie);
            }
            HttpWebRequest wGETRequest2 = (HttpWebRequest)WebRequest.Create(temp);
            wGETRequest2.Method = "GET";
            wGETRequest2.Host = "partner.spreadshirt.com";
            wGETRequest2.UserAgent = BROWSER_FIREFOX;
            wGETRequest2.Accept = "application/json, text/plain, */*";
            wGETRequest2.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wGETRequest2.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wGETRequest2.Referer = "https://partner.spreadshirt.com/designs";
            wGETRequest2.CookieContainer = cookieApplication;

            WebResponse responseGET2 = wGETRequest2.GetResponse();
            StreamReader responseGETReader2 = new StreamReader(responseGET2.GetResponseStream());
            string rsGET2 = responseGETReader2.ReadToEnd();

            foreach (Cookie cookie in ((HttpWebResponse)responseGET2).Cookies)
            {
                cookieApplication.Add(cookie);
            }

            string url = "https://partner.spreadshirt.com/api/v1/users/" + User.USER_ID + "/design-uploads";
            //string url = USER_HREF + "/design-uploads";
            var urlUploadImage = encodeURL(url, "createProductIdea=true");
            string fileUrl = @"C:\Users\HoangLe\Desktop\Tool up spreadshirt\file anh\1 PT011117.png";

            HttpWebRequest wRequestDesign = (HttpWebRequest)WebRequest.Create(urlUploadImage);
            wRequestDesign.Method = "POST";
            wRequestDesign.Host = "partner.spreadshirt.com";
            wRequestDesign.UserAgent = BROWSER_FIREFOX;
            wRequestDesign.Accept = "application/json, text/plain, */*";
            wRequestDesign.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
            wRequestDesign.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            wRequestDesign.Referer = "https://partner.spreadshirt.com/designs";
            wRequestDesign.CookieContainer = cookieApplication;

            var data = HttpUploadFile(url, fileUrl, "filedata", "image/png", null);

        }
        public static Dictionary<string, object> HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            HttpWebRequest wr = null;
            WebResponse wresp = null;
            try
            {
                Console.WriteLine(string.Format("Uploading {0} to {1}", file, url));
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                wr = (HttpWebRequest)WebRequest.Create(url);
                wr.Host = "partner.spreadshirt.com";
                wr.UserAgent = BROWSER_FIREFOX;
                wr.Accept = "application/json";
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.Headers.Add("Origin", "https://pro.teechip.com");
                wr.Headers.Add("Accept-Language", "vi-VN,vi;q=0.8,en-US;q=0.5,en;q=0.3");
                wr.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                wr.CookieContainer = cookieApplication;
                wr.KeepAlive = true;

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();


                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                data.Add("data", wresp.Headers["Location"]);
                data.Add("status", 1);
            }
            catch (Exception ex)
            {
                data.Add("data", ex.Message);
                data.Add("status", -1);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
            return data;
        }
        private Dictionary<string, object> PostDataAPI(HttpWebRequest wRequest, string data2Send)
        {
            Dictionary<string, object> dataReturn = new Dictionary<string, object>();
            CookieContainer cookies = new CookieContainer();
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] postDataBytes = encoding.GetBytes(data2Send);

                wRequest.Method = "POST";
                wRequest.UserAgent = BROWSER_FIREFOX;
                wRequest.ContentLength = postDataBytes.Length;

                using (Stream sr = wRequest.GetRequestStream())
                {
                    sr.Write(postDataBytes, 0, postDataBytes.Length);
                }

                HttpWebResponse wResponse = (HttpWebResponse)wRequest.GetResponse();
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
            wRequest.UserAgent = BROWSER_FIREFOX;
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

        /// <summary>
        /// take any string and encrypt it using SHA1 then
        /// return the encrypted data
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private string GetSHA1HashData(string data)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string getTimeStamp()
        {
            double unixTimestamp = Math.Round((DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds, 3);
            return unixTimestamp.ToString().Replace(".", "");
        }
        private string encodeURL(string url, string defaultParam = "", string method = "POST", string locale = "", string mediaType = "", string sessionId = "")
        {

            string t_url = url.Split('?')[0];
            string t_time = getTimeStamp();
            string t_data = method + t_url + " " + t_time + " " + SECRET;
            string t_sig = GetSHA1HashData(t_data);

            int index = t_url.IndexOf('?');
            var newUrl = t_url;
            if (index == -1)
                newUrl += "?";
            else
                newUrl += "&";
            newUrl += defaultParam == "" ? "" : defaultParam + "&";
            newUrl += "apiKey=" + API_KEY;
            if (locale != "")
                newUrl += "&locale=" + locale;//us_US
            if (mediaType != "")
                newUrl += "&mediaType=" + mediaType;//json
            newUrl += "&sig=" + t_sig + "&time=" + t_time;
            if (sessionId != "")
                newUrl += "&sessionId=" + sessionId;
            return newUrl;
        }
    }
}
